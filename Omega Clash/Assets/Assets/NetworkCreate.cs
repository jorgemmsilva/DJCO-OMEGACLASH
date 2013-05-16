using UnityEngine;
using System.Collections;

public class NetworkCreate : MonoBehaviour {

	public Transform Prefab;
 
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
		Camera.main.transform.parent = myPlayer;
		
		//Camera.main.transform.localPosition = new Vector3(0,6,-10);		
		//Camera.main.transform.localRotation = Quaternion.Euler(25,0,0);
	}
}