using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour {
	
	public int authorId;
	public float range = 50.0f;
	public float time_to_live = 20.0f;
	public float timeActive = 5.0f;
	public float attractiveForce = 100.0f;
	
	private float starttime = 0;
	private bool active = false;

	void OnCollisionEnter(Collision other)
	{
		if (networkView.isMine)
		{
			if (!active && (other.gameObject.tag!="Player" || other.gameObject.GetComponent<CharacterStatus>().id != authorId))
			{
				this.rigidbody.velocity = Vector3.zero;
				this.GetComponent<TrailRenderer>().enabled = false;
				this.rigidbody.isKinematic = true;
				starttime = 0;
				time_to_live = timeActive;
				this.gameObject.GetComponent<NetworkView>().RPC ("Activate", RPCMode.All);
			}
		}
    }
	
	void FixedUpdate ()
	{
		if(networkView.isMine)
		{
			starttime +=Time.deltaTime;
			if (starttime > time_to_live)
			{
				this.gameObject.GetComponent<NetworkView>().RPC ("Explode", RPCMode.All);
				Network.Destroy(GetComponent<NetworkView>().viewID);
			}
		}
		
		if(active)
		{
			Collider[] Colliders;
				
			Colliders = Physics.OverlapSphere(transform.position, range);
			
			for(int i = 0; i<Colliders.Length; i++)
			{
				if(Colliders[i].gameObject.tag == "Player" && Colliders[i].gameObject.networkView.isMine)
				{
					Colliders[i].rigidbody.AddExplosionForce(-attractiveForce * 50, this.transform.position, range, 1.0f,ForceMode.Force);
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
			
	    }
	    else
	    {
	        Vector3 receivedPosition = Vector3.zero;
	        stream.Serialize(ref receivedPosition); //"Decode" it and receive it
	        transform.position = receivedPosition;
		}
	}
	
	[RPC]
	void Initialize(int id,float force, float Ontime)
	{
		authorId = id;
		attractiveForce = force;
		timeActive = Ontime;
	}
	
	[RPC]
	void Activate()
	{
		active = true;
	}
	
	[RPC]
	void Explode()
	{
		Collider[] Colliders;
			
		Colliders = Physics.OverlapSphere(transform.position, range);
		
		for(int i = 0; i<Colliders.Length; i++)
		{
			if(Colliders[i].gameObject.tag == "Player" && Colliders[i].gameObject.networkView.isMine)
			{
				Colliders[i].rigidbody.AddExplosionForce(attractiveForce * 10 * timeActive * 50, this.transform.position, range, 1.0f,ForceMode.Force);
			}
		}
	}
}