using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Weapon {
	public Transform bullet;
	public string name;
	public int ammo;
	public bool mana;
	public string val1String;
	public float val1Scale;
	public float val1Max;
	public float val1Min;
	public float val1;
	public string val2String;
	public float val2Scale;
	public float val2Max;
	public float val2Min;
	public float val2;
	public WeaponType type;
	public Material material;
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
		GUI.Label(new Rect(offsetX+170,offsetY+30, 300, 20), "Health="+this.transform.root.GetComponent<CharacterStatus>().health);
		GUI.Label(new Rect(offsetX+320,offsetY+30, 400, 20), "Weapon="+weapon.name);
		string weaponSet="";
		foreach (Weapon w in weapons) {
			if (w.ammo>=0) {
				weaponSet+=", "+w.name;
			}
		}
		weaponSet="{"+weaponSet.Substring(2)+"}";
		GUI.Label(new Rect(offsetX+320,offsetY+60, 300, 100), "Weapon\u2208" + weaponSet);
		if (weapon.mana) {
			GUI.Label(new Rect(offsetX+640,offsetY+30, 400, 100), "Energy="+weapon.ammo+"J");
		}
		else {
			GUI.Label(new Rect(offsetX+640,offsetY+30, 400, 100), "Ammo="+weapon.ammo);
		}
		GUI.Label(new Rect(offsetX+640,offsetY+60, 400, 100), weapon.val1String.Replace("?",weapon.val1.ToString()));
		GUI.Label(new Rect(offsetX+640,offsetY+75, 400, 100), weapon.val2String.Replace("?",weapon.val2.ToString()));
	}
	
	void Awake() {
		Screen.lockCursor=true;
		GameObject father = GameObject.FindGameObjectWithTag("Weapon");
		foreach (Transform child in father.transform)
		{
			child.renderer.material = weapons[weapon_selected].material;
		}
		
	}
	
	void FixedUpdate() {
		foreach (Weapon w in weapons) {
			if (w.mana && w.ammo<100000) {w.ammo++;}
		}
	}
	
	// Update is called once per frame
	void Update () {	
		if (!transform.root.root.networkView.isMine)
			return;
		
		if (Input.GetKey(KeyCode.Escape)) {
			Screen.lockCursor=false;
		}
		
		if (Input.GetKey(KeyCode.K)) {
			this.transform.root.gameObject.GetComponent<NetworkView>().RPC ("TakeDMG", RPCMode.All, 200.0f, 0);
		}
		
		float delta=Input.GetAxis("Mouse ScrollWheel");
		if (Input.GetKey(KeyCode.LeftShift)) {
			weapons[weapon_selected].val2+=weapons[weapon_selected].val2Scale*delta;
			if (weapons[weapon_selected].val2>weapons[weapon_selected].val2Max) {
				weapons[weapon_selected].val2=weapons[weapon_selected].val2Max;
			}
			if (weapons[weapon_selected].val2<weapons[weapon_selected].val2Min) {
				weapons[weapon_selected].val2=weapons[weapon_selected].val2Min;
			}
		}
		else {
			weapons[weapon_selected].val1+=weapons[weapon_selected].val1Scale*delta;
			if (weapons[weapon_selected].val1>weapons[weapon_selected].val1Max) {
				weapons[weapon_selected].val1=weapons[weapon_selected].val1Max;
			}
			if (weapons[weapon_selected].val1<weapons[weapon_selected].val1Min) {
				weapons[weapon_selected].val1=weapons[weapon_selected].val1Min;
			}
		}
		
		if(Input.GetMouseButtonDown(1))
		{
			do {
				weapon_selected=(weapon_selected+1)%(weapons.Length);
			}
			while (weapons[weapon_selected].ammo<0);

			GameObject father = GameObject.FindGameObjectWithTag("Weapon");
			foreach (Transform child in father.transform)
			{
				child.renderer.material = weapons[weapon_selected].material;
			}
			
			
			
		}
		if(Input.GetMouseButtonDown(0) && weapons[weapon_selected].ammo>0)
		{
			Weapon weapon=weapons[weapon_selected];
			if (!weapon.mana) {
				weapon.ammo--;
			}
			if (weapon.type != WeaponType.Laser) {
				Transform bullet;
				switch (weapon.type)
				{
					// NOTA: Unidade de força do Unity = 1/50 N
					case WeaponType.Balao:
						//dont need anything
						bullet=(Transform)Network.Instantiate(weapon.bullet, transform.position, transform.rotation, 0);
						bullet.gameObject.GetComponent<NetworkView>().RPC ("Initialize", RPCMode.All, this.transform.root.gameObject.GetComponent<CharacterStatus>().id, UnityEngine.Random.value > 0.5f);
						break;
					case WeaponType.Bazooka:
						bullet=(Transform)Network.Instantiate(weapon.bullet, transform.position, transform.rotation, 0);
						bullet.gameObject.GetComponent<NetworkView>().RPC ("Initialize", RPCMode.All, this.transform.root.gameObject.GetComponent<CharacterStatus>().id, weapon.val2, weapon.val1);
						bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * 50, ForceMode.Force);
						this.transform.root.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * (-50), ForceMode.Force);
						break;
					case WeaponType.BlackHole:
						bullet=(Transform)Network.Instantiate(weapon.bullet, transform.position, transform.rotation, 0);
						bullet.gameObject.GetComponent<NetworkView>().RPC ("Initialize", RPCMode.All, this.transform.root.gameObject.GetComponent<CharacterStatus>().id, weapon.val1, weapon.val2);
						//doing only here, intended only on creater
						bullet.rigidbody.isKinematic = false;
						bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * 1000 * 50, ForceMode.Force);
						break;
					case WeaponType.BolaPinchona:
						bullet=(Transform)Network.Instantiate(weapon.bullet, transform.position, transform.rotation, 0);
						bullet.gameObject.GetComponent<NetworkView>().RPC ("Initialize", RPCMode.All, this.transform.root.gameObject.GetComponent<CharacterStatus>().id, weapon.val2);
						bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * 50, ForceMode.Force);
						this.transform.root.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * (-50), ForceMode.Force);
						break;
					case WeaponType.CampoMagnetico:
						bullet=(Transform)Network.Instantiate(weapon.bullet, transform.position, transform.rotation, 0);
						bullet.gameObject.GetComponent<NetworkView>().RPC ("Initialize", RPCMode.All, this.transform.root.gameObject.GetComponent<CharacterStatus>().id, weapon.val1);
						//doing only here, intended only on creater
						bullet.rigidbody.isKinematic = false;
						bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * 1000 * 50, ForceMode.Force);
						break;
					case WeaponType.Ilusao:
						//dont need anything
						if (weapon.ammo>weapon.val1*10) {
							weapon.ammo-=(int)weapon.val1*10;
							bullet=(Transform)Network.Instantiate(weapon.bullet, transform.position, transform.rotation, 0);
							bullet.gameObject.GetComponent<NetworkView>().RPC ("Initialize", RPCMode.All, this.transform.root.gameObject.GetComponent<CharacterStatus>().team, weapon.val1);
						}
						break;
					case WeaponType.MeleeWimshurst:
						break;
					case WeaponType.MinaElastica:
						if (weapon.ammo>weapon.val1) {
							weapon.ammo-=(int)weapon.val1;
							bullet=(Transform)Network.Instantiate(weapon.bullet, transform.position, transform.rotation, 0);
							bullet.gameObject.GetComponent<NetworkView>().RPC ("Initialize", RPCMode.All, this.transform.root.gameObject.GetComponent<CharacterStatus>().id, weapon.val1);
							bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * 100, ForceMode.Force);
						}
						break;
					case WeaponType.MinaExplosiva:
						bullet=(Transform)Network.Instantiate(weapon.bullet, transform.position, transform.rotation, 0);
						break;
					case WeaponType.Mola:
						bullet=(Transform)Network.Instantiate(weapon.bullet, transform.position, transform.rotation, 0);
						bullet.gameObject.GetComponent<NetworkView>().RPC ("Initialize", RPCMode.All, this.transform.root.gameObject.GetComponent<CharacterStatus>().id, weapon.val1);
						bullet.rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * weapon.val1 * 50, ForceMode.Force);
						break;
				}
			}
		}
	}
}
