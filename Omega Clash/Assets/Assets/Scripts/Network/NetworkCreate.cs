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
		Debug.Log ("connecting");
	    SpawnPlayer();
	}
	void OnDisconnectedFromServer (NetworkDisconnection info)
	{
		//recheck all stats
        Debug.Log("Disconnected from server: " + info);
    }
	
	void SpawnPlayer()
	{
		if(Network.isServer)
		{
			//TODO check team/spawnpoint to put, put it there
			this.GetComponent<ServerStatus>().playersTeam1++;
			Debug.Log ("incrementing");
		}

    	Transform myPlayer = (Transform)Network.Instantiate(Prefab, transform.position, transform.rotation, 0);
		foreach (Transform child in myPlayer)
		{
		  	//child is your child transform
			if(child.name == "FirePosition")
			{
				Camera.main.transform.parent = myPlayer;
				Camera.main.transform.localPosition = new Vector3(0.0f, 1.75f, 0.0f);
				Camera.main.transform.localRotation = Quaternion.identity;
				child.parent = Camera.main.transform;
				child.localPosition = new Vector3(0.0f, -0.75f, 1.0f);
				child.localRotation = Quaternion.identity;
				return;
			}
		}
	}
}