using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
	
	// Update is called once per frame
	void FixedUpdate () {
		Destroy (this.gameObject);
		Destroy (this);
	}
}
