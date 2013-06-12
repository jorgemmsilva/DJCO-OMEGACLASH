using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour {
	
	public GameObject author;
	public float range = 50.0f;
	public float time_to_live = 20.0f;
	public float timeActive = 5.0f;
	public float attractiveForce = 100.0f;
	
	private float starttime = 0;
	private bool active = false;

	void OnCollisionEnter(Collision other)
	{
		if (!active && other.gameObject != author)
		{
			this.rigidbody.velocity = Vector3.zero;
			this.GetComponent<TrailRenderer>().enabled = false;
			this.rigidbody.isKinematic = true;
			starttime = 0;
			time_to_live = timeActive;
			active = true;
		}
    }
	void FixedUpdate () {
		starttime +=Time.deltaTime;
		if (starttime > time_to_live)
		{
			Collider[] Colliders;
				
			Colliders = Physics.OverlapSphere(transform.position, range);
			
			for(int i = 0; i<Colliders.Length; i++)
			{
				if(Colliders[i].gameObject.tag == "Player")
				{
					Colliders[i].rigidbody.AddExplosionForce(attractiveForce * timeActive, this.transform.position, range, -1.0f,ForceMode.Acceleration);
				}
			}
			
			Destroy(this.gameObject);
			Destroy(this);
		}
		
		if(active)
		{
			Collider[] Colliders;
				
			Colliders = Physics.OverlapSphere(transform.position, range);
			
			for(int i = 0; i<Colliders.Length; i++)
			{
				if(Colliders[i].gameObject.tag == "Player")
				{
					Colliders[i].rigidbody.AddExplosionForce(-attractiveForce, this.transform.position, range, -1.0f,ForceMode.Acceleration);
				}
			}
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