using UnityEngine;
using System.Collections;

public class MinaElastica : MonoBehaviour {
	
	public int authorId;
	public float force = 2000.0f;
	
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<CharacterStatus>().id != authorId)
		{
			other.gameObject.audio.clip=audio.clip;
			other.gameObject.audio.Play();
			this.gameObject.GetComponent<NetworkView>().RPC ("Fire", RPCMode.All, other.gameObject.GetComponent<CharacterStatus>().id);
			Network.Destroy(GetComponent<NetworkView>().viewID);
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
	
	[RPC]
	void Initialize(int id, float forceinc)
	{
		authorId = id;
		force = forceinc;
	}
	
	[RPC]
	void Fire(int firstAvatarId)
	{
		GameObject[] avatars = GameObject.FindGameObjectsWithTag("Player");
		GameObject firstAvatar = null;
		
		foreach (GameObject avatar in avatars)
		{
            if (avatar.GetComponent<CharacterStatus>().id == firstAvatarId)
			{
				firstAvatar = avatar;
				break;
            }
		}
		if (firstAvatar.networkView.isMine)
		{
			firstAvatar.rigidbody.AddForce(Vector3.up * force * 50, ForceMode.Force);
		}
	}
}
