using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public Texture2D menuTexture;
	public Texture2D logoTexture;
	public string username="Player";
	public string ip;
 	public int connectionPort = 25001;
	public int team = 0;
	
	void OnGUI() {
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),menuTexture);
		
		GUI.Label(new Rect((Screen.width-512)/2,0,512,256),logoTexture);
		
		username=GUI.TextArea(new Rect((Screen.width-256)/2,300,128,32),username);
		if(GUI.Button(new Rect((Screen.width-256)/2+128,300,64,32),"Ninja")) {
			team=1;
		}
		if(GUI.Button(new Rect((Screen.width-256)/2+128+64,300,64,32),"Pirate")) {
			team=2;
		}
		if(GUI.Button(new Rect((Screen.width-128)/2,400,128,32),"Host Game")) {
			Network.InitializeServer(32, connectionPort, false);
			Application.LoadLevel(1);
		}
		if(GUI.Button(new Rect((Screen.width-128)/2+64,450,128,32),"Join as Game")) {
			Network.Connect(ip, connectionPort);
			Application.LoadLevel(1);
		}
		ip=GUI.TextArea(new Rect((Screen.width-128)/2-64,450,128,32),ip);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake() {
		DontDestroyOnLoad (this);
	}
}
