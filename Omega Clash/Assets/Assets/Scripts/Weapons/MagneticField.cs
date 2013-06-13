using UnityEngine;
using System.Collections;

public class MagneticField : MonoBehaviour {
	
	public GameObject author;
	public float range = 50.0f;
	public float time_to_live = 20.0f;
	public float magneticForce = 1000.0f;
	public MagneticPole type = MagneticPole.Push;
	
	public enum MagneticPole { Push, Pull };
	
	private float starttime = 0;
	
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject != author)
		{
			Collider[] Colliders;
				
			Colliders = Physics.OverlapSphere(transform.position, range);
			
			float Force = magneticForce;
			if ( type == MagneticPole.Pull)
				Force = -Force;
				
			for(int i = 0; i<Colliders.Length; i++)
			{
				if(Colliders[i].gameObject.tag == "Player")
				{
					Colliders[i].rigidbody.AddExplosionForce(Force * 50, this.transform.position, range, 1.0f,ForceMode.Force);
				}
			}
			
			Destroy(this.gameObject);
			Destroy(this);
		}
    }
	
	void FixedUpdate () {
		starttime +=Time.deltaTime;
		if (starttime > time_to_live)
		{
			Collider[] Colliders;
				
			Colliders = Physics.OverlapSphere(transform.position, range);
			
			float Force = magneticForce;
			if ( type == MagneticPole.Pull)
				Force = -Force;
			
			for(int i = 0; i<Colliders.Length; i++)
			{
				if(Colliders[i].gameObject.tag == "Player")
				{
					Colliders[i].rigidbody.AddExplosionForce(Force * 50, this.transform.position, range, 1.0f,ForceMode.Force);
				}
			}
			
			Destroy(this.gameObject);
			Destroy(this);
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