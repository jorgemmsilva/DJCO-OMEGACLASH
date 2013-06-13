using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {
	
	[SerializeField]
	private float m_health;
	[SerializeField]
	private int m_team;
	[SerializeField]
	private int m_kills;
	[SerializeField]
	private int m_deaths;
	[SerializeField]
	private float m_damage;
	
	public float health
    {
        get { return m_health; }
        set { m_health = value; }
    }

    public int team
    {
        get { return m_team; }
        set { m_team = value; }
    }
	
	public int kills
    {
        get { return m_kills; }
        set { m_kills = value; }
    }

    public int deaths
    {
        get { return m_deaths; }
        set { m_deaths = value; }
    }
	
	public float damage
    {
        get { return m_damage; }
        set { m_damage = value; }
    }
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		// IMPORTANT
		// Same order on writing and reading.
	    if (stream.isWriting)
	    {
		    stream.Serialize(ref m_health);
			stream.Serialize(ref m_team);	
			stream.Serialize(ref m_kills);
			stream.Serialize(ref m_deaths);
			stream.Serialize(ref m_damage);
	    }
	    else
	    {
	        float receivedhealth = 0.0f;
	        stream.Serialize(ref receivedhealth); //"Decode" it and receive it
	        health = receivedhealth;
			
			int receivedteam = 0;
	        stream.Serialize(ref receivedteam); //"Decode" it and receive it
	        team = receivedteam;
			
			int receivedkills = 0;
	        stream.Serialize(ref receivedkills); //"Decode" it and receive it
	        kills = receivedkills;
			
			int receiveddeaths = 0;
	        stream.Serialize(ref receiveddeaths); //"Decode" it and receive it
	        deaths = receiveddeaths;
			
			float receiveddamage = 0.0f;
	        stream.Serialize(ref receiveddamage); //"Decode" it and receive it
	        damage = receiveddamage;
	    }
	}
	
}
