/*

	-----------------------
	UDP-Send
	-----------------------

        // todo: shutdown thread at the end
*/
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

public class UDPSend : MonoBehaviour
{
    //private static int localPort;
	private string strAppError = "";
	private bool bConnectEnabled = false;
	public bool displayDebug = false;
	
    // prefs 
    private string IP;  // define in init
	public int SendToPort;  // define in init
    
    // "connection" things
    IPEndPoint remoteEndPoint;
    private UdpClient client;
        
    // call it from shell (as program)
    /*private static void Main() 
    {
    	UDPSend sendObj=new UDPSend();
    	sendObj.init();
    	
    	// as server sending endless
    	sendObj.sendEndless(" endless infos \n");
    	
    }*/
	
	// start from unity3d
	public void Start()
	{
		init();	
	}

	void OnGUI () 
	{	
		if(displayDebug)
		{
			UDPReceive recieveScript = GetComponent<UDPReceive>();
			Rect rectObj=new Rect(10,550,500,700);
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.UpperLeft;
			style.normal.textColor = Color.red;
			GUI.Box(rectObj, strAppError,style);
			
			GUILayout.BeginArea(new Rect(10, 10, 100, 100));
	
			//disable buttons and this seems to be the only way to check since the error is fired from the .net app
			if(!bConnectEnabled || recieveScript.lastReceivedUDPPacket == "Exception caught.error opening portCOM3")
				GUI.enabled = true;
			else
				GUI.enabled = false;
			
			if (GUILayout.Button("Connect"))
			{
				sendMessage("Connect");
				StartCoroutine(SendNextMessage("Zero"));
				bConnectEnabled = true;
			}
			
			//disable buttons
			if(bConnectEnabled && recieveScript.lastReceivedUDPPacket != "Exception caught.error opening portCOM3")
				GUI.enabled = true;
			else
				GUI.enabled = false;
			
			if (GUILayout.Button("Disconnect"))
			{
				sendMessage("StopSmart");
				StartCoroutine(SendNextMessage("Connect"));
				bConnectEnabled = false;
			}
			
			GUI.enabled = true;
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(10, 80, 100, 130));
			if (GUILayout.Button("Validate"))
			{
				sendMessage("StopSmart");
				StartCoroutine(SendNextMessage("Validate"));
			}
			
			if (GUILayout.Button("Zero"))
			{
				sendMessage("StopSmart");
				StartCoroutine(SendNextMessage("Zero"));
			}
			
			if (GUILayout.Button("Calibrate"))
			{
				//must disable buttons also
				recieveScript.bCheckCalibrate = true;
				sendMessage("StartSmart");
				StartCoroutine(CalibrateWait());
			}
			
			if (GUILayout.Button("Start Capture"))
				sendMessage("StartSmart");
			
			if (GUILayout.Button("Stop Capture"))
				sendMessage("StopSmart");
			
			GUILayout.Label("Movement: ");
			GUILayout.EndArea();
		}
	}
    
	// init
	public void init()
	{
        print("UDPSend.init()");
        
        // define
        IP="127.0.0.1";
 		SendToPort=8051;
 		
		// ----------------------------
		// Send
		// ----------------------------
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), SendToPort);
        client = new UdpClient();
        
        // status
        print("Sending to "+IP+" : "+SendToPort);
        print("Testing: nc -lu "+IP+" : "+SendToPort);
	
	}

	// inputFromConsole
    private void inputFromConsole()
    {
    	try 
        {
            string text;
            do 
            {
                text = Console.ReadLine();

                if (text != "") 
                {
                    byte[] data = Encoding.UTF8.GetBytes(text);
                    client.Send(data, data.Length, remoteEndPoint);
                }
            } while (text != "");
        }
        catch (Exception err)
        {
            print(err.ToString());
        }

    }

    // sendData
    public void sendMessage(string message)
    {
    	try 
        {
			StartExe ExeScript = GetComponent<StartExe>();
			
			if (!ExeScript.myProcess.HasExited)
			{
				strAppError = "";
                byte[] data = Encoding.UTF8.GetBytes(message);
                client.Send(data, data.Length, remoteEndPoint);
			}
			else
			{
				strAppError = "Error the SmartFloorUDPApp.exe has exited please restart";
			}
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }
   
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			StartExe ExeScript = GetComponent<StartExe>();
			sendMessage("StopSmart");
			StartCoroutine(SendNextMessage("Connect"));
			ExeScript.myProcess.Kill();
		}
	}
	
   void OnApplicationQuit() {
		StartExe ExeScript = GetComponent<StartExe>();
		sendMessage("StopSmart");
		StartCoroutine(SendNextMessage("Connect"));
		ExeScript.myProcess.Kill();
	}
	
	IEnumerator SendNextMessage(string message)
	{
		yield return new WaitForSeconds(0.5f);
		sendMessage(message);
	}
	
	IEnumerator CalibrateWait()
	{
		UDPReceive recieveScript = GetComponent<UDPReceive>();
		yield return new WaitForSeconds(10);
		recieveScript.bCheckCalibrate = false;
		recieveScript.bCalculateAverage = true;
		//sendMessage("StopSmart");
	}
	
    // endless test
    private void sendEndless(string testStr)
    {
    	do
    	{
    		sendMessage(testStr);
    	}
    	while(true);
    	
    }
    
}