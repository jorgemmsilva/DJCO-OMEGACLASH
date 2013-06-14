using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {
	
	[SerializeField]
	private int m_id;
	[SerializeField]
	private string m_name;
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
	
	public AudioClip deathClip;
	public Material ninjaMaterial;
	public Material pirateMaterial;
	
	public void Start() {
		m_health=100;
		m_team=0;
		GameObject menu=GameObject.FindGameObjectWithTag("Menu");
		if (menu && networkView.isMine) {
			m_name=((Menu)menu.GetComponent<Menu>()).username;
			m_team=((Menu)menu.GetComponent<Menu>()).team;
			Destroy(menu);
				
			if (m_team==1) {transform.FindChild("baseMale").renderer.material=ninjaMaterial;}
			else {this.transform.FindChild("baseMale").renderer.material=pirateMaterial;}
		}
	}
	
	public int id
    {
        get { return m_id; }
        set { m_id = value; }
    }
	
	public string name
    {
        get { return m_name; }
        set { m_name = value; }
    }
	
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
		    stream.Serialize(ref m_id);
			stream.Serialize(ref m_health);
			stream.Serialize(ref m_team);
			stream.Serialize(ref m_kills);
			stream.Serialize(ref m_deaths);
			
	    }
	    else
	    {
	        int receivedid = 0;
	        stream.Serialize(ref receivedid); //"Decode" it and receive it
	        id = receivedid;
			
			float receivedhealth = 0;
	        stream.Serialize(ref receivedhealth); //"Decode" it and receive it
	        health = receivedhealth;
			
			int receivedteam = m_team;
	        stream.Serialize(ref receivedteam); //"Decode" it and receive it
	        team = receivedteam;
		
			if (m_team==1) {transform.FindChild("baseMale").renderer.material=ninjaMaterial;}
			else {this.transform.FindChild("baseMale").renderer.material=pirateMaterial;}
			
			
			int receivedkills = 0;
	        stream.Serialize(ref receivedkills); //"Decode" it and receive it
	        kills = receivedkills;
			
			int receiveddeaths = 0;
	        stream.Serialize(ref receiveddeaths); //"Decode" it and receive it
	        deaths = receiveddeaths;
	    }
	}
	
	[RPC]
	void TakeDMG (float damage, int authorId)
	{
		if(networkView.isMine)
		{
			health -= damage;
			if(health <= 0)
			{
				this.audio.clip=deathClip;
				this.audio.Play();
				deaths++;
				health = 100;
				GameObject [] spawns = GameObject.FindGameObjectsWithTag("Respawn");
				Transform spawn_point = spawns[Random.Range(0,spawns.Length)].transform;
				this.transform.position = spawn_point.position;
				this.transform.rotation = spawn_point.rotation;
				this.rigidbody.velocity = Vector3.zero;
				
				GameObject[] avatars = GameObject.FindGameObjectsWithTag("Player");
				GameObject firstAvatar = null;
				
				foreach (GameObject avatar in avatars)
				{
		            if (avatar.GetComponent<CharacterStatus>().id == authorId)
					{
						firstAvatar = avatar;
						break;
		            }
				}
				firstAvatar.GetComponent<NetworkView>().RPC ("AddKill", RPCMode.All);
			}
		}
	}
	
	[RPC]
	void AddKill ()
	{
		if(networkView.isMine)
		{
			kills++;
		}
	}
}
