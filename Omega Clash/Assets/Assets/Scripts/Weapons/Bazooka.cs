using UnityEngine;
using System.Collections;

public class Bazooka : MonoBehaviour {
	
	public int authorId;

	void OnCollisionEnter(Collision other)
	{
		if(networkView.isMine)
		{
			if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<CharacterStatus>().id != authorId)
			{
				other.gameObject.rigidbody.AddForce(rigidbody.velocity, ForceMode.Force);
				other.gameObject.GetComponent<CharacterStatus>().health -= 10;
				
				float damage = 10.0f;
				other.gameObject.GetComponent<NetworkView>().RPC ("TakeDMG", RPCMode.All, damage);

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
			
			float mymass = this.rigidbody.mass;
			stream.Serialize(ref mymass);
			
			int myauthorId = this.authorId;
			stream.Serialize(ref myauthorId);
	    }
	    else
	    {
	        Vector3 receivedPosition = Vector3.zero;
	        stream.Serialize(ref receivedPosition); //"Decode" it and receive it
	        transform.position = receivedPosition;
			
			float receivedmymass = 0.0f;
	        stream.Serialize(ref receivedmymass); //"Decode" it and receive it
	        this.rigidbody.mass = receivedmymass;
			
			int receivedmyauthorId = 0;
	        stream.Serialize(ref receivedmyauthorId); //"Decode" it and receive it
	        this.authorId = receivedmyauthorId;			
	    }
	}
}