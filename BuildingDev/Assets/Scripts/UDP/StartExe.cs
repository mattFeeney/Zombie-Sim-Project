using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class StartExe : MonoBehaviour {
	
	public string fileName = "SmartFloorUDPApp.exe";
	private string stringPath = null; 
	public Process myProcess = new Process(); 
	
	// Use this for initialization
	void Start () {
		myProcess.StartInfo.FileName = Application.dataPath + "/Executable/" + fileName; 
		myProcess.StartInfo.Arguments = stringPath; 
		myProcess.Start();
	}
	
	// Update is called once per frame
	//void OnApplicationQuit() {
		//myProcess.Kill();
	//}
}
