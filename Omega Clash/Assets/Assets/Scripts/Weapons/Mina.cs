using UnityEngine;
using System.Collections;

public class Mina : MonoBehaviour {
	
	public float force = 2000.0f;
	
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.rigidbody.AddForce(Vector3.up * force, ForceMode.Acceleration);
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
