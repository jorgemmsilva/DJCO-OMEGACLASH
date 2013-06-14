using UnityEngine;
using System.Collections;

public class BouncyBall : MonoBehaviour {
	
	public int authorId;
	public float time_to_live = 20.0f;
	public float explosiveForce = 1000.0f;
	public float range = 40.0f;
	
	private float starttime = 0;
	
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
	void Initialize(int id, float time)
	{
		authorId = id;
		time_to_live = time;
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
				Debug.Log("HERP DERP");
				float damage=10.0f;
				Colliders[i].gameObject.GetComponent<NetworkView>().RPC ("TakeDMG", RPCMode.All, damage, authorId);
				Colliders[i].rigidbody.AddExplosionForce(explosiveForce * 50, this.transform.position, range, 1.0f,ForceMode.Force);
			}
		}
	}
}
