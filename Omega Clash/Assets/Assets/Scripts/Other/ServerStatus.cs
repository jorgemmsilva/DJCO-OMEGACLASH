using UnityEngine;
using System.Collections;

public class ServerStatus : MonoBehaviour {
	
	[SerializeField]
	public enum Team { Red, Blue };
	
	[SerializeField]
	private int m_maxPlayers;
	[SerializeField]
	private int m_playersTeam1;
	[SerializeField]
	private int m_playersTeam2;
	
	public int maxPlayers
    {
        get { return m_maxPlayers; }
        set { m_maxPlayers = value; }
    }

    public int playersTeam1
    {
        get { return m_playersTeam1; }
        set { m_playersTeam1 = value; }
    }
	
	public int playersTeam2
    {
        get { return m_playersTeam2; }
        set { m_playersTeam2 = value; }
    }
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		// IMPORTANT
		// Same order on writing and reading.
	    if (stream.isWriting)
	    {
		    stream.Serialize(ref m_maxPlayers);
			
			stream.Serialize(ref m_playersTeam1);			
			
			stream.Serialize(ref m_playersTeam2);
			
			Debug.Log("writing to server stats");
	    }
	    else
	    {
	        int receivedmaxPlayers = 0;
	        stream.Serialize(ref receivedmaxPlayers); //"Decode" it and receive it
	        m_maxPlayers = receivedmaxPlayers;
			
			int receivedplayersTeam1 = 0;
	        stream.Serialize(ref receivedplayersTeam1); //"Decode" it and receive it
	        m_playersTeam1 = receivedplayersTeam1;
			
			int receivedplayersTeam2 = 0;
	        stream.Serialize(ref receivedplayersTeam2); //"Decode" it and receive it
	        m_playersTeam2 = receivedplayersTeam2;
			
			Debug.Log("reading server stats");
	    }
	}
}
