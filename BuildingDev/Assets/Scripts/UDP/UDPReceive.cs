/*

	-----------------------
	UDP-Receive (send to)
	-----------------------
	// http://msdn.microsoft.com/de-de/libr....aspx#ID0E3BAC
	
	
	// > receive 
	// 127.0.0.1 : 8051
	
	// send
	// nc -u 127.0.0.1 8051

*/
using UnityEngine;
using System.Collections;

using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour {
    
    // receiving Thread
    Thread receiveThread; 

	// udpclient object
	private UdpClient client;
	private PlayerActions playerAction;

    public int ListenToPort; 
	public string lastReceivedUDPPacket="";
	public string allReceivedUDPPackets=""; // clean up this from time to time!
	[HideInInspector]
	public bool bCheckCalibrate = false, bCalculateAverage = false;
	public bool displayDebug = false;
	
	public static float[] averageForce;
	public struct force_COP_struct
    {
		public float frame;
        public float force_N;
        public float x_cop_mm;
        public float y_cop_mm;
		public float region;
    }
	
	private float[,] calibrateData; 
	public force_COP_struct[] resultData;
	//private string[] fields = new string[5]{"","","","", ""}; 
	private int[] indexCalibrate;
	private int CalibrateFrameAmount = 300;
	private int noOfPlayer;
	[HideInInspector]
	public int playerNo = 0;
	
    public void Start()
    {
    	init();	
    }
    
    // OnGUI
    void OnGUI()
	{
		if(displayDebug)
		{
			float win=Screen.width*0.6f;
			float w1=win*0.15f;
			
			Rect rectObj=new Rect(1040,10,500,700);
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.UpperLeft;
			style.normal.textColor = Color.white;
			GUI.Box(rectObj,"# UDPReceive\n127.0.0.1 "+ListenToPort+" #\n"
						+ "\nLast Packet: \n"+ lastReceivedUDPPacket
						+ "\n\nAll Messages: \n"+allReceivedUDPPackets, style);
			
			
			//need an a for loop and to check what player and if needs to be redrawn if more than one player
			GUILayout.BeginArea(new Rect(0, 200, 500, 100));
			GUILayout.BeginHorizontal();
			GUILayout.Label("Player", GUILayout.Width(w1));
			GUILayout.Label("Frame", GUILayout.Width(w1));
			GUILayout.Label("Force", GUILayout.Width(w1));	
			GUILayout.Label("X cord", GUILayout.Width(w1));	
			GUILayout.Label("Y cord", GUILayout.Width(w1));
			GUILayout.Label("State", GUILayout.Width(w1));
			GUILayout.EndHorizontal(); 
			
			//check below where it recieves the data and add new line from there
			//prob is its been overwritten get data from array instead :)
			for(int currPlayer = 0; currPlayer < noOfPlayer; currPlayer++)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(resultData[currPlayer].region.ToString(), GUILayout.Width(w1));
				GUILayout.Label(resultData[currPlayer].frame.ToString(), GUILayout.Width(w1));
				GUILayout.Label(resultData[currPlayer].force_N.ToString(), GUILayout.Width(w1));	
				GUILayout.Label(resultData[currPlayer].x_cop_mm.ToString(), GUILayout.Width(w1));	
				GUILayout.Label(resultData[currPlayer].y_cop_mm.ToString(), GUILayout.Width(w1));
				//playeraction has strcurrstates
				GUILayout.Label(playerAction.strCurrentForce[currPlayer], GUILayout.Width(w1));
				GUILayout.EndHorizontal(); 
			}
			GUILayout.EndArea();
		}
	}
	    
    // init
    private void init()
    {
        print("UDPSend.init()");
        
        // define port
        ListenToPort = 11000;
		
		noOfPlayer = GetComponent<Settings>().noOfPlayers;
		
		resultData = new force_COP_struct[noOfPlayer];
		averageForce = new float[noOfPlayer];
		indexCalibrate = new int[noOfPlayer];
		
		//CalibrateFrameAmount = CalibrateFrameAmount * playerAmount;
		calibrateData = new float[noOfPlayer, CalibrateFrameAmount];
		
        // status
        print("Sending to 127.0.0.1 : "+ListenToPort);
		playerAction = GetComponent<PlayerActions>();
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
	
	void OnDisable()
	{
		if ( receiveThread!= null)
			receiveThread.Abort();
		
		client.Close();
	} 

	// receive thread 
    private void ReceiveData() 
    {
		float[] fData = new float[5];
		string strTypeFlag = "", text = "";
		int clearCounter = 0;
		const int ibyteSize = 4;
        client = new UdpClient(ListenToPort);
		
        while (true) 
        {
            try 
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

				//need to know what type we are dealing with
                text = Encoding.UTF8.GetString(data);
				if(text == "Results" || text == "Message")
					strTypeFlag = text;	//set the upcoming types
				else
				{
					if(strTypeFlag == "Results")
					{		
						for (int i = 0; i < data.Length; i++)
						{
							fData[i/ibyteSize] = System.BitConverter.ToSingle(data, i);
							//fields[i/ibyteSize] = fData[i/ibyteSize].ToString();
							i += 3;
						}
						
						playerNo = Convert.ToInt32(fData[4]);
	
						//make it easier to read later
						resultData[playerNo].frame = fData[0];
						resultData[playerNo].force_N = fData[1];
						resultData[playerNo].x_cop_mm = fData[2];
						resultData[playerNo].y_cop_mm = fData[3];
						resultData[playerNo].region = fData[4];
						
						/*print("-------------------------");
						print("region " + resultData[playerNo].region);
						print("x cord  " + resultData[playerNo].x_cop_mm);
						print("y cord  " + resultData[playerNo].y_cop_mm);
						print("force  " + resultData[playerNo].force_N);*/
						
						//bCheckCalibrate is been set in UDPSend
						if(bCheckCalibrate)
						{
							//would like to have used the frame number but can cause issue if started streaming already
							indexCalibrate[playerNo]++;
							calibrateData[playerNo, indexCalibrate[playerNo]] = resultData[playerNo].force_N;
						}
						else
						{
							playerAction.JumpDetect(playerNo);
							playerAction.CurrentDirection(playerNo);  //check movement direction
						}
					}
					else if(strTypeFlag == "Message")
					{	
						clearCounter++;
		                print(">> " + text);
		                lastReceivedUDPPacket=text;
						
						//Tidy up the received messages
						if(clearCounter == 20)
						{
							allReceivedUDPPackets = "";
							clearCounter = 0;
						}
		                allReceivedUDPPackets=allReceivedUDPPackets + "\n" +text;
					}
				}
            }
            catch (Exception err) 
            {
                print(err.ToString());
            }
        }
    }
	
	void Update()
	{
		//bCalculateAverage is been set in UDPSend
		if(bCalculateAverage)
			Calibrate();
	}
	
	private void Calibrate()
    {
		string strCalDebug = "";
		Settings settings = GameObject.Find("Smart Floor").GetComponent<Settings>();
		bCalculateAverage = false;
		
		for(int currPlayer=0; currPlayer < settings.noOfPlayers; currPlayer++)
		{
			for(int i=0; i < resultData[currPlayer].frame; i++)
			{
				averageForce[currPlayer] += calibrateData[currPlayer, i];
			}
			averageForce[currPlayer] = averageForce[currPlayer]/resultData[currPlayer].frame;
			strCalDebug += "P" + currPlayer + "'s average is " + averageForce[currPlayer] + "\n";
			indexCalibrate[currPlayer] = 0;
		}
		print(strCalDebug);
	}

    // cleans up the rest
    private string getLatestUDPPacket()
    {
    	allReceivedUDPPackets="";
    	return lastReceivedUDPPacket;
    }
}