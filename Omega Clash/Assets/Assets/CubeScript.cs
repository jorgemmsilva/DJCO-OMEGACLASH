using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		if (!this.networkView.isMine)
        	enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.networkView.isMine)
		{
		    Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		    float speed = 5;
		    transform.Translate(speed * moveDir * Time.deltaTime);
		}	
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
	    if (stream.isWriting)
		{
	        Vector3 pos = transform.position;
	        stream.Serialize(ref pos);
		}
		else
		{
		        Vector3 receivedPosition = Vector3.zero;
		        stream.Serialize(ref receivedPosition);
		        transform.position = receivedPosition;
		}
	}			
}
