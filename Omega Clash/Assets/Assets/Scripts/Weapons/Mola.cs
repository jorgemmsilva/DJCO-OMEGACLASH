using UnityEngine;
using System.Collections;

public class Mola : MonoBehaviour {
	public int authorId;
	public float constant;
	
	void OnCollisionEnter(Collision other)
	{
		if(networkView.isMine)
		{
			if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<CharacterStatus>().id != authorId)
			{
				this.gameObject.GetComponent<NetworkView>().RPC ("Join", RPCMode.All, authorId, other.gameObject.GetComponent<CharacterStatus>().id);
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
	void Initialize(int id, float incconstant)
	{
		authorId = id;
		constant = incconstant;
	}
	
	[RPC]
	void Join(int firstAvatarId, int secondAvatarId)
	{
		GameObject[] avatars = GameObject.FindGameObjectsWithTag("Player");
		GameObject firstAvatar = null;
		GameObject secondAvatar = null;
		
		foreach (GameObject avatar in avatars)
		{
            if (avatar.GetComponent<CharacterStatus>().id == firstAvatarId)
			{
				firstAvatar = avatar;
            }
			if (avatar.GetComponent<CharacterStatus>().id == secondAvatarId)
			{
				secondAvatar = avatar;
            }
        }
		
		SpringJoint springJoint = firstAvatar.AddComponent<SpringJoint>();
		firstAvatar.AddComponent<MolaDestroy>();
		springJoint.maxDistance = 0.0f;
		springJoint.minDistance = 0.5f;
		springJoint.spring = constant;
		springJoint.damper = 2 * Mathf.Sqrt(secondAvatar.rigidbody.mass * springJoint.spring);
		springJoint.connectedBody = secondAvatar.rigidbody;
	}
}
