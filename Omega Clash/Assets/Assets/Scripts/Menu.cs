using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public Texture2D menuTexture;
	public string ip;
 	public int connectionPort = 25001;
	
	void OnGUI() {
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),menuTexture);
		if(GUI.Button(new Rect((Screen.width-128)/2,250,128,32),"Host Game")) {
			Network.InitializeServer(32, connectionPort, false);
			Application.LoadLevel(1);
		}
		if(GUI.Button(new Rect((Screen.width-128)/2+64,300,128,32),"Join Game")) {
			Network.Connect(ip, connectionPort);
		}
		ip=GUI.TextArea(new Rect((Screen.width-128)/2-64,300,128,32),ip);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
