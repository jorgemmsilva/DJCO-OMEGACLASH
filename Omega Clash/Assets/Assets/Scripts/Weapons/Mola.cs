using UnityEngine;
using System.Collections;

public class Mola : MonoBehaviour {
	public GameObject author;
	
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player" && other.gameObject != author)
		{
			Debug.Log("author:" + author + " other"+ other.gameObject);
			SpringJoint springJoint = author.AddComponent<SpringJoint>();
			springJoint.maxDistance = 0.0f;
			springJoint.minDistance = 0.5f;
			springJoint.spring = 1000.0f;
			springJoint.damper = 2 * Mathf.Sqrt(other.gameObject.rigidbody.mass * springJoint.spring);
			springJoint.connectedBody = other.gameObject.rigidbody;
			//Destroy (this.gameObject);
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
