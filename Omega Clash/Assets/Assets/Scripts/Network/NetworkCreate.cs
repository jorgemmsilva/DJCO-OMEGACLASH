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
		foreach (Transform child in myPlayer)
		{
		  //child is your child transform
			if(child.name == "FirePosition")
			{
				child.parent = Camera.main.transform;
				Camera.main.transform.parent = myPlayer;
				return;
			}	
		}
		//Camera.main.transform.parent = myPlayer.GetComponent<FirePosition>();
		
		//Camera.main.transform.localPosition = new Vector3(0,6,-10);		
		//Camera.main.transform.localRotation = Quaternion.Euler(25,0,0);
	}
}