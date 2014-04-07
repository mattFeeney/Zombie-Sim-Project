using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class MultidimensionalInt
{
    public int[] region = new int[4];

    public int this[int index] {
        get {
            return region[index];
        }

        set { 
            region[index] = value; 
        }
    }

    public int Length {
        get {
            return region.Length;
        }
    }

    public long LongLength {
        get {
            return region.LongLength;
        }
    }
}

public class Settings : MonoBehaviour {
	
	public string fileName = "SmartSettings.txt";
	public string ComPort = "COM3";
	public int Units = 18;
	public float PlateSize = 500;
	public int noOfPlayers = 2;
	public MultidimensionalInt[] Players = new MultidimensionalInt[2];
	//string[] fields = new string[]{};
	
	// Use this for initialization
	void Awake () {
		//ReadCsv();
		//if (ValueChanged()) before it would check for changes but too many checks just do once
		WriteCsv();
	}
	
	/*private void ReadCsv()
    {
		// Use using StreamReader for disposing.
		using (StreamReader reader = new StreamReader(fileName))
		{
		    // Use while != null pattern for loop
		    string line;
		    while ((line = reader.ReadLine()) != null)
		    {
				fields = line.Split(","[0]);
		    }
			reader.Close();
		}
	}*/
	
/*	private bool ValueChanged()
    {
		if(System.Convert.ToInt32(fields[0]) != Units || System.Convert.ToSingle(fields[1]) != PlateSize ||
		   fields[2] != ComPort || System.Convert.ToInt32(fields[3]) != noOfPlayers || System.Convert.ToInt32(fields[3]) != Players[0].region[0])
			return true;
		return false;
	}*/
	
	private void WriteCsv()
    {
		// Use using StreamReader for disposing.
		System.IO.File.WriteAllText(@"SmartSettings.txt",string.Empty); //clear the settings file cause weird issue was occuring
		using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
		{
			using (StreamWriter writer = new StreamWriter(stream))
			{
				string strSettings = Units + "," + PlateSize +  "," + ComPort + "," + noOfPlayers;
				for (int playNo = 0; playNo < noOfPlayers; playNo++)
				{
					for(int regionId = 0; regionId < 4; regionId++)
					{
						strSettings += "," + Players[playNo].region[regionId];	
					}
				}
				writer.WriteLine(strSettings);
			}
		}
		
	}
}
