using UnityEngine;
using System.Collections;

public class BouncyBall : MonoBehaviour {
	
	public GameObject author;
	public float time_to_live = 20.0f;
	public float explosiveForce = 1000.0f;
	public float range = 40.0f;
	
	private float starttime = 0;
	
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
					Debug.Log("I GOT THE POWA");
					Colliders[i].rigidbody.AddExplosionForce(explosiveForce * 50, this.transform.position, range, 1.0f,ForceMode.Force);
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
