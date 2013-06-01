using UnityEngine;
using System.Collections;

public class Laser_Proj : MonoBehaviour {
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Mirror")
		{
			Debug.Log("MIRROR");
		}
    }
	
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag != "Mirror")
			Destroy(this.gameObject);
		else
			Debug.Log("MIRROR" + Random.Range(-1000,1000));
		if (other.gameObject.tag == "Player")
		{
			Debug.Log("BATEU LASER");
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