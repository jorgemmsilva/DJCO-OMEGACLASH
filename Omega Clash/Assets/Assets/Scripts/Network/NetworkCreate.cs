using UnityEngine;
using System.Collections;

public class NetworkCreate : MonoBehaviour {

	public Transform Prefab;
	
	void Start() {
		if (Network.peerType != NetworkPeerType.Disconnected) {
			SpawnPlayer();	
		}
	}
 
	void OnServerInitialized()
	{	
	    SpawnPlayer();
	}
	void OnConnectedToServer()
	{
	    SpawnPlayer();
	}
	void SpawnPlayer()
	{
    	Transform myPlayer = (Transform)Network.Instantiate(Prefab, transform.position, transform.rotation, 0);
		foreach (Transform child in myPlayer)
		{
		  	//child is your child transform
			if(child.name == "FirePosition")
			{
				Camera.main.transform.parent = myPlayer;
				Camera.main.transform.localPosition = new Vector3(0.0f, 1.75f, 0.0f);
				Camera.main.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
				child.parent = Camera.main.transform;
				child.localPosition = new Vector3(0.0f, -0.75f, 1.0f);
				child.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
				return;
			}	
		}
		//Camera.main.transform.parent = myPlayer.GetComponent<FirePosition>();
		
		//Camera.main.transform.localPosition = new Vector3(0,6,-10);		
		//Camera.main.transform.localRotation = Quaternion.Euler(25,0,0);
	}
}