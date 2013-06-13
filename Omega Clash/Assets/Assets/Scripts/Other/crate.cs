using UnityEngine;
using System.Collections;

public class crate : MonoBehaviour {
	
	public WeaponType ammoType;
	public int ammo;
	
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player")
		{
			foreach (Weapon w in ((FireAll)(other.gameObject.GetComponentInChildren<FireAll>())).weapons) {
				if (w.type==ammoType) {w.ammo+=ammo;}
			}
			Destroy(this.gameObject);
		}
	}
}
