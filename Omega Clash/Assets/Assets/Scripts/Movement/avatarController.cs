using UnityEngine;
using System.Collections;

public class avatarController : MonoBehaviour {
	
	public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
	public float rotatexspeed = 1.0F;
	public float rotateyspeed = 2.0F;
    private Vector3 moveDirection = Vector3.zero;
	private Vector3 angleRotate = Vector3.zero;
	
	void Awake() {
		
	    //if (!networkView.isMine)
    	//    enabled = false;
		
		animation.wrapMode = WrapMode.Loop;
		animation["jump"].wrapMode = WrapMode.ClampForever;
		animation.Stop();
	}
	
    void Update() {
		//constant no animation/normal here
		animation.CrossFade("idle");
		
		if(networkView.isMine)
		{
			
			// move according to local player input
			
	        CharacterController controller = GetComponent<CharacterController>();
	        if (controller.isGrounded) 
			{
	            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
				moveDirection.Normalize();
				if(moveDirection.magnitude>0)
				{
					if(moveDirection.magnitude>0.66) animation.CrossFade("sprint");
					else if(moveDirection.magnitude>0.33) animation.CrossFade("run");
					else animation.CrossFade("walk");
				}
				
	            moveDirection = transform.TransformDirection(moveDirection);
	            moveDirection *= speed;
	            if (Input.GetButton("Jump"))
	                moveDirection.y = jumpSpeed;
	            
	        }
			else
			{
				animation.CrossFade("jump");
			}
	        moveDirection.y -= gravity * Time.deltaTime;
	        controller.Move(moveDirection * Time.deltaTime);
						
			angleRotate.y += Input.GetAxis("Mouse X") * rotatexspeed;
			angleRotate.y = angleRotate.y % 360;
			angleRotate.x -= Input.GetAxis("Mouse Y") * rotateyspeed;
			angleRotate.x = angleRotate.x % 360;
			transform.rotation = Quaternion.Euler(angleRotate);
				
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