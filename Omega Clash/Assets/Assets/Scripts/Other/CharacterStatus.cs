using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {
	
	[SerializeField]
	public enum Team { Red, Blue, None };
	
	[SerializeField]
	private string m_name;
	[SerializeField]
	private float m_health;
	[SerializeField]
	private Team m_team;
	[SerializeField]
	private int m_kills;
	[SerializeField]
	private int m_deaths;
	[SerializeField]
	private float m_damage;
	
	public void Start() {
		m_health=100;
		m_team=Team.None;
		GameObject menu=GameObject.FindGameObjectWithTag("Menu");
		if (menu) {
			m_name=((Menu)menu.GetComponent<Menu>()).username;
			m_team=((Menu)menu.GetComponent<Menu>()).team;
			Destroy(menu);
		}
	}
	
	public float health
    {
        get { return m_health; }
        set { m_health = value; }
    }

    public Team team
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
}
