using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Weapon {
	public Transform bullet;
	public string name;
	public int ammo;
	public string val1String;
	public float val1Scale;
	public float val1;
	public string val2String;
	public float val2Scale;
	public float val2;
	public WeaponType type;
};

[Serializable]
public enum WeaponType { Bazooka, Mola, MinaExplosiva, MinaElastica, Ilusao, Balao, BolaPinchona, MeleeWimshurst, Laser, BlackHole, CampoMagnetico };

public class FireAll : MonoBehaviour {
	
	public Weapon[] weapons;
	public int weapon_selected;
	public Texture2D hudTexture;
	public Texture2D crosshairTexture;
	
	void OnGUI() {
		if (!transform.root.root.networkView.isMine)
			return;
		
		GUI.Label(new Rect((Screen.width-64)/2,(Screen.height-64)/2,64,64),crosshairTexture);
		
		Weapon weapon=weapons[weapon_selected];
		int offsetX=(Screen.width-1024)/2;
		int offsetY=Screen.height-128;
		GUI.Label(new Rect(offsetX,offsetY,1024,128),hudTexture);
		GUI.Label(new Rect(offsetX+170,offsetY+30, 300, 20), "Health=100");
		GUI.Label(new Rect(offsetX+320,offsetY+30, 400, 20), "Weapon="+weapon.name);
		string weaponSet="";
		foreach (Weapon w in weapons) {
			if (w.ammo>=0) {
				weaponSet+=", "+w.name;
			}
		}
		weaponSet="{"+weaponSet.Substring(2)+"}";
		GUI.Label(new Rect(offsetX+320,offsetY+60, 300, 100), "Weapon\u2208" + weaponSet);
		GUI.Label(new Rect(offsetX+640,offsetY+30, 400, 100), "Ammo="+weapon.ammo);
		GUI.Label(new Rect(offsetX+640,offsetY+60, 400, 100), weapon.val1String.Replace("?",weapon.val1.ToString()));
		GUI.Label(new Rect(offsetX+640,offsetY+75, 400, 100), weapon.val2String.Replace("?",weapon.val2.ToString()));
	}
	
	// Update is called once per frame
	void Update () {
		if (!transform.root.root.networkView.isMine)
			return;
		
		float delta=Input.GetAxis("Mouse ScrollWheel");
		if (Input.GetKey(KeyCode.LeftShift)) {
			weapons[weapon_selected].val2+=weapons[weapon_selected].val2Scale*delta;
		}
		else {
			weapons[weapon_selected].val1+=weapons[weapon_selected].val1Scale*delta;	
		}
		
		if(Input.GetMouseButtonDown(1))
		{
			do {
				weapon_selected=(weapon_selected+1)%(weapons.Length);
			}
			while (weapons[weapon_selected].ammo<0);
		}
		if(Input.GetMouseButtonDown(0) && weapons[weapon_selected].ammo>0)
		{
			Weapon weapon=weapons[weapon_selected];
			weapon.ammo--;
			if (weapon.type != WeaponType.Laser) {
				Transform bullet=(Transform)Network.Instantiate(weapon.bullet, transform.position, transform.rotation, 0);
				switch (weapon.type)
				{
					// NOTA: Unidade de força do Unity = 1/50 N
					case WeaponType.Balao:
						break;
					case WeaponType.Bazooka:
						bullet.GetComponent<Bazooka>().authorId = this.transform.root.gameObject.GetComponent<CharacterStatus>().id;
						bullet.rigidbody.mass=weapon.val2;
						bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * 50, ForceMode.Force);
						this.transform.root.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * (-50), ForceMode.Force);
						break;
					case WeaponType.BlackHole:
						bullet.GetComponent<BlackHole>().author = this.transform.root.gameObject;
						bullet.rigidbody.mass=weapon.val2;
						bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * 50, ForceMode.Force);
						this.transform.root.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * (-50), ForceMode.Force);
						break;
					case WeaponType.BolaPinchona:
						bullet.rigidbody.mass=weapon.val2;
						bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * 50, ForceMode.Force);
						this.transform.root.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * (-50), ForceMode.Force);
						break;
					case WeaponType.CampoMagnetico:
						bullet.GetComponent<MagneticField>().author = this.transform.root.gameObject;
						bullet.rigidbody.mass=weapon.val2;
						bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * 50, ForceMode.Force);
						this.transform.root.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * (-50), ForceMode.Force);
						break;
					case WeaponType.Ilusao:
						bullet.GetComponent<ilussionScript>().time_to_live = weapon.val1;
						bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * 100, ForceMode.Force);
						break;
					case WeaponType.MeleeWimshurst:
						break;
					case WeaponType.MinaElastica:
						bullet.GetComponent<MinaElastica>().author = this.transform.root.gameObject;
						bullet.GetComponent<MinaElastica>().force = weapon.val1;
						bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * 100, ForceMode.Force);
						break;
					case WeaponType.MinaExplosiva:
						break;
					case WeaponType.Mola:
						bullet.GetComponent<Mola>().author = this.transform.root.gameObject;
						//bullet.rigidbody.mass=weapon.val2;
						bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * 50, ForceMode.Force);
						this.transform.root.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * (-50), ForceMode.Force);
						break;
				}
			}
		}
	}
}
