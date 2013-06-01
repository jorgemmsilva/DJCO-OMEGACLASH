using UnityEngine;
using System.Collections;

public class FireAll : MonoBehaviour {
	enum Weapons {Bazooka_Weapon, Mina_Weapon, Mola_Weapon, Ilussion_Weapon, Bouncy_Weapon, Ballon_Trap};

	public Transform Bazooka;
	public float Bazooka_Force = 1000.0f;
	public Transform Mina;
	public float Mina_Force = 100.0f;
	public Transform Mola;
	public float Mola_Force = 400.0f;
	public Transform Ilussion;
	public Transform Bouncy;
	public float Bouncy_Force = 400.0f;
	public Transform Ballon;

	
	private Weapons weapon_selected = Weapons.Bazooka_Weapon;

	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButtonDown(1))
		{
			switch (weapon_selected)
			{
				case Weapons.Bazooka_Weapon:
					weapon_selected = Weapons.Mina_Weapon;
					break;
				case Weapons.Mina_Weapon:
					weapon_selected = Weapons.Mola_Weapon;
					break;
				case Weapons.Mola_Weapon:
					weapon_selected = Weapons.Ilussion_Weapon;
					break;
				case Weapons.Ilussion_Weapon:
					weapon_selected = Weapons.Bouncy_Weapon;
					break;
				case Weapons.Bouncy_Weapon:
					weapon_selected = Weapons.Ballon_Trap;
					break;
				case Weapons.Ballon_Trap:
					weapon_selected = Weapons.Bazooka_Weapon;
					break;
				default:
					break;
			}
		}
		if(Input.GetMouseButtonDown(0))
		{
			Transform myBullet;
			switch (weapon_selected)
			{
				case Weapons.Bazooka_Weapon:
					myBullet = (Transform)Network.Instantiate(Bazooka, transform.position, transform.rotation, 0);
					myBullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * Bazooka_Force, ForceMode.Force);
					break;
				case Weapons.Mina_Weapon:
					myBullet = (Transform)Network.Instantiate(Mina, transform.position, transform.rotation, 0);
					myBullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * Mina_Force, ForceMode.Force);
					break;
				case Weapons.Mola_Weapon:
					myBullet = (Transform)Network.Instantiate(Mola, transform.position, transform.rotation, 0);
					myBullet.GetComponent<Mola>().author = transform.parent.gameObject;
					myBullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * Mola_Force, ForceMode.Force);
					break;
				case Weapons.Ilussion_Weapon:
					myBullet = (Transform)Network.Instantiate(Ilussion, transform.position, transform.rotation, 0);
					break;
				case Weapons.Bouncy_Weapon:
					myBullet = (Transform)Network.Instantiate(Bouncy, transform.position, transform.rotation, 0);
					myBullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * Bouncy_Force, ForceMode.Force);
					break;
				case Weapons.Ballon_Trap:
					myBullet = (Transform)Network.Instantiate(Ballon, transform.position, transform.rotation, 0);
					break;
				default:
					break;
			}
		}
	}
}
