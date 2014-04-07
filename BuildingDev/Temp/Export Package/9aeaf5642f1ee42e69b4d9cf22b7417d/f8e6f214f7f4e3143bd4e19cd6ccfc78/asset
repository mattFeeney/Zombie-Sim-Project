using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LoadExistingData : MonoBehaviour {

	private string cvsPath = "";
	private bool bReading  = false;
	private string[] fields = new string[]{};
	private static int currLine = 0;
	private List<string> lines = new List<string>();
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnGUI () {
		float win=Screen.width*0.6f;
		float w1=win*0.15f;
		
		GUILayout.BeginArea(new Rect(120, 10, 100, 100));
		if (GUILayout.Button("Load Existing"))
		{
			DisplayData();
			//disable other buttons
		}
		if (GUILayout.Button("Stop Existing"))
		{
			DisplayData();
			//clear and enable other buttons
		}
		GUILayout.EndArea();
		
		if(bReading)
			StartCoroutine(DisplayCords());
		                          
		if (fields.Length >= 4) 
		{	
			GUILayout.BeginArea(new Rect(0, 220, 500, 100));
			GUILayout.BeginHorizontal();	
			GUILayout.Label(fields[0], GUILayout.Width(w1));	
			GUILayout.Label(fields[1], GUILayout.Width(w1));	
			GUILayout.Label(fields[2], GUILayout.Width(w1));	
			GUILayout.Label(fields[3], GUILayout.Width(w1));	
			GUILayout.EndHorizontal(); 
			GUILayout.EndArea();
		}
	}
	
	void DisplayData()
	{
		cvsPath = EditorUtilityOpenFilePanel.Apply();

		//read in the csv file to array
		if (cvsPath != "")
			ReadCsv(cvsPath);
		
		//display on the screen
		
	}
	
	private void ReadCsv(string path)
    {
		// Use using StreamReader for disposing.
		using (StreamReader reader = new StreamReader(path))
		{
		    // Use while != null pattern for loop
		    string line;
		    while ((line = reader.ReadLine()) != null)
		    {
				lines.Add(line);
		    }
			reader.Close();
		}
		bReading = true;
	}	
	
	IEnumerator DisplayCords()
	{
		if(bReading)
		{
			bReading = false;
			string line = lines[currLine];
		   	fields = line.Split(","[0]);
			currLine++;
			StartCoroutine(WaitHere());
			Debug.Log("current line " + currLine + "\n line " + lines.Count + "reading " + bReading);
			
			if (currLine >= lines.Count)
				bReading = false;
		}
		yield return 0;
	}
	
	IEnumerator WaitHere()
	{
		yield return new WaitForSeconds(0.04f);
		bReading = true;
	}
}
