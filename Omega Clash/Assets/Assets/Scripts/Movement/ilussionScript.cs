using UnityEngine;
using System.Collections;
 
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class ilussionScript : MonoBehaviour {
 
	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public float time_to_live = 3.0f;
	
	private bool grounded = false;
	private float starttime = 0;
	
 
	void Awake () {
	    rigidbody.freezeRotation = true;
	    rigidbody.useGravity = false;

		animation.wrapMode = WrapMode.Loop;
		animation.Stop();
	}
 
	void FixedUpdate () {
		starttime +=Time.deltaTime;
		if (starttime > time_to_live)
		{
			Destroy(this.gameObject);
			Destroy(this);
		}
		//constant no animation/normal here
		animation.CrossFade("sprint");
		if(networkView.isMine)
		{
		    if (grounded)
			{
				
		        Vector3 targetVelocity = transform.TransformDirection(Vector3.forward);
				targetVelocity.Normalize();
		        targetVelocity *= speed;
	 
		        // Apply a force that attempts to reach our target velocity
		        Vector3 velocity = rigidbody.velocity;
		        Vector3 velocityChange = (targetVelocity - velocity);
		        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		        velocityChange.y = 0;
		        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
		        
		    }
	 
		    // We apply gravity manually for more tuning control
		    rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
	 
		    grounded = false;
		}
	}
 
	void OnCollisionStay () {
	    grounded = true;    
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
			
			rigidbody.velocity = Vector3.zero;
	    }
	}
}