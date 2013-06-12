using UnityEngine;
using System.Collections;
 
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class RigidbodyFPSController : MonoBehaviour {
 
	public float speed = 10.0f;
	public float sprintMultiplier = 2.0f;
	
	public float gravity = 10.0f;
	public float maxVelocityChange = 0.4f;
	public float maxVelocity = 20.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	public float rotatexspeed = 1.0F;
	public float rotateyspeed = 2.0F;
	public float bottomRotationVertical = 45.0f;
	public float upperRotationVertical = 80.0f;
	public float minVelocityTreshold = 1.0f;
	
	private bool grounded = false;
	private Vector3 characterRotate = Vector3.zero;
	private Vector3 cameraRotate = Vector3.zero;
 
 
	void Awake () {
	    rigidbody.freezeRotation = true;
	    rigidbody.useGravity = false;

		animation.wrapMode = WrapMode.Loop;
		animation.Stop();
		if(networkView.isMine)
		{
			foreach (Transform child in gameObject.transform)
			{
				child.renderer.enabled = false;
			}
		}
	}
 
	void FixedUpdate () {
		//constant no animation/normal here
		animation.CrossFade("idle");
		if(networkView.isMine)
		{
			characterRotate.y += Input.GetAxis("Mouse X") * rotatexspeed;
			characterRotate.y = characterRotate.y % 360;
			transform.rotation = Quaternion.Euler(characterRotate);
			
			
			cameraRotate.y = 0;
			cameraRotate.z = 0;
			cameraRotate.x -= Input.GetAxis("Mouse Y") * rotateyspeed;
			if(cameraRotate.x < -upperRotationVertical) 
				cameraRotate.x = -upperRotationVertical;
			else if(cameraRotate.x > bottomRotationVertical)
				cameraRotate.x = bottomRotationVertical;
			
			Camera.main.transform.localRotation = Quaternion.Euler(cameraRotate);
			
			
		
	        // Calculate how fast we should be moving
	        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			targetVelocity.Normalize();
			if (Input.GetKey(KeyCode.LeftControl))
				targetVelocity *= sprintMultiplier;
			
			if(rigidbody.velocity.magnitude < minVelocityTreshold)
					rigidbody.velocity = Vector3.zero;
			
			if(targetVelocity.magnitude>0.0)
			{
				if(targetVelocity.magnitude>1.0) animation.CrossFade("sprint");
				else if(targetVelocity.magnitude>0.66) animation.CrossFade("run");
				else animation.CrossFade("walk");
				
				
			
				targetVelocity = transform.TransformDirection(targetVelocity);
		        targetVelocity *= speed;
				
	 
		        // Apply a force that attempts to reach our target velocity
		        Vector3 velocity = rigidbody.velocity;
		        Vector3 velocityChange = (targetVelocity - velocity);
				velocityChange.Normalize();
				velocityChange *= maxVelocityChange;
				
				
		        velocityChange.y = 0;
				
				if(rigidbody.velocity.magnitude < minVelocityTreshold)
					velocityChange *= 20;
				
				
				RaycastHit info = new RaycastHit();
				if(Physics.Raycast(rigidbody.transform.position, velocityChange, out info, 2.0f))
				{
					if(info.normal.normalized.y>0.4) rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
				}
				else rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
			}
			

			// Jump
	        if (grounded && canJump && Input.GetButton("Jump")) {
	            rigidbody.velocity = new Vector3(rigidbody.velocity.x, CalculateJumpVerticalSpeed(), rigidbody.velocity.z);
	        }
 
		    // We apply gravity manually for more tuning control
		    rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
	 
		    grounded = false;
		}
		else
		{
			this.rigidbody.velocity = Vector3.zero;
		}
	}
 
	void OnCollisionStay () {
	    grounded = true;    
	}
 
	float CalculateJumpVerticalSpeed () {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * jumpHeight * gravity);
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