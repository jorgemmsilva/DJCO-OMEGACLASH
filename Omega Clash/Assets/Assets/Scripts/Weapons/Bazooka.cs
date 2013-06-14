using UnityEngine;
using System.Collections;

public class Bazooka : MonoBehaviour {
	
	public int authorId;
	public float force;

	void OnCollisionEnter(Collision other)
	{
		if(networkView.isMine)
		{
			if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<CharacterStatus>().id != authorId)
			{
				float damage = force/50;
				other.gameObject.GetComponent<NetworkView>().RPC ("TakeDMG", RPCMode.All, damage, authorId);

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
	void Initialize(int id, float mass, float incforce)
	{
		authorId = id;
		rigidbody.mass = mass;
		force = incforce;
	}

}