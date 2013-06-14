using UnityEngine;
using System.Collections;

public class Ballon : MonoBehaviour {
	
	public int authorId;
	public Transform content_good;
	public Transform content_bad;
	public bool is_good = false;
	public float time_to_live = 3000;
	public float speed = 10;
	public float gravity = 10.0f;
	
	private float starttime = 0;
	private float distancefire = 5;
	private Vector3 initialpos;
	
	void OnCollisionEnter(Collision other)
	{
		if(networkView.isMine)
		{
			if (!((other.gameObject.tag == "Player" && other.gameObject.GetComponent<CharacterStatus>().id == authorId) || other.gameObject.GetInstanceID() == this.gameObject.GetInstanceID()))
			{
				if(is_good)
					Network.Instantiate(content_good, transform.position, transform.rotation, 0);
				else
					Network.Instantiate(content_bad, transform.position, transform.rotation, 0);

				Network.Destroy(GetComponent<NetworkView>().viewID);
			}
		}
    }
	
	// Use this for initialization
	void Start () {
		initialpos = transform.position;
		starttime = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if(networkView.isMine)
		{
			starttime +=Time.deltaTime;
			if (starttime > time_to_live)
			{
				Network.Destroy(GetComponent<NetworkView>().viewID);
			}
		    if (transform.position.y > initialpos.y + distancefire)
			{
				// make it go down
				rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
		    }
			if (rigidbody.velocity.magnitude < 1)
			{					
				//give random force, y component zero
				Vector3 randomForce = Vector3.zero;
				randomForce.x = Random.Range(-1.0f, 1.0f);
				randomForce.z = Random.Range(-1.0f, 1.0f);
	        	rigidbody.AddForce(randomForce, ForceMode.VelocityChange);
			}
			// We apply going up manually
		    rigidbody.AddForce(new Vector3 (0, gravity * rigidbody.mass, 0));
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
	    }
	    else
	    {
	        Vector3 receivedPosition = Vector3.zero;
	        stream.Serialize(ref receivedPosition); //"Decode" it and receive it
	        transform.position = receivedPosition;
	    }
	}
	
	[RPC]
	void Initialize(int id, bool incgood)
	{
		authorId = id;
		is_good = incgood;
	}
}
