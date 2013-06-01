using UnityEngine;
using System.Collections;

public class Bazooka : MonoBehaviour {

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player")
		{
			Debug.Log("BATEU BAZZOKA");
			other.gameObject.rigidbody.AddForce(rigidbody.velocity, ForceMode.Force);
			Destroy(this.gameObject);
		}
    }
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		// IMPORTANT
		// Same order on writing and reading.
	    if (stream.isWriting)
	    {
		    Vector3 myPosition = transform.position;
		    stream.Serialize(ref myPosition);
			
			Quaternion myRotation = transform.rotation;
			stream.Serialize(ref myRotation);	
			
	    }
	    else
	    {
	        Vector3 receivedPosition = Vector3.zero;
	        stream.Serialize(ref receivedPosition); //"Decode" it and receive it
	        transform.position = receivedPosition;
			
			Quaternion receivedRotation = new Quaternion();
	        stream.Serialize(ref receivedRotation); //"Decode" it and receive it
	        transform.rotation = receivedRotation;
	    }
	}
}