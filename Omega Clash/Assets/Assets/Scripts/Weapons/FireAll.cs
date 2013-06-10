using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Weapon {
	public Transform bullet;
	public string name;
	public int ammo;
	public float val;
	public bool rigid;
	public bool spring;
	public bool laser;
};

public class FireAll : MonoBehaviour {
	
	public Weapon[] weapons;
	public int weapon_selected;
	public Texture2D hudTexture;
	
	void OnGUI() {
		Weapon weapon=weapons[weapon_selected];
		int offsetX=(Screen.width-1024)/2;
		int offsetY=Screen.height-128;
		GUI.Label(new Rect(offsetX,offsetY,1024,128),hudTexture);
		GUI.Label(new Rect(offsetX+170,offsetY+30, 300, 20), "Health=100");
		GUI.Label(new Rect(offsetX+320,offsetY+30, 400, 20), "Weapon="+weapon.name);
		GUI.Label(new Rect(offsetX+320,offsetY+60, 400, 100), "Weapon\u2208" +
			"{Bazooka, Spring Mine, Spring Hook,\n Illusion, Rubber Ball, Balloon Trap, Laser}");
		GUI.Label(new Rect(offsetX+640,offsetY+30, 400, 100), "Ammo="+weapon.ammo);
		GUI.Label(new Rect(offsetX+640,offsetY+60, 400, 100), "Force="+weapon.val);
	}
	
	// Update is called once per frame
	void Update () {
		float delta=Input.GetAxis("Mouse ScrollWheel");
		weapons[weapon_selected].val+=10*delta;
		
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
			if (!weapon.laser) {
				Transform bullet=(Transform)Network.Instantiate(weapon.bullet, transform.position, transform.rotation, 0);
				if (weapon.spring) {
					bullet.GetComponent<Mola>().author = transform.parent.gameObject;
				}
				if (weapon.rigid) {
					bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val, ForceMode.Force);
				}
			}
		}
	}
}
