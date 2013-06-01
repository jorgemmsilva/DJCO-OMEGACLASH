using UnityEngine;
using System.Collections;

public class avatarForce : MonoBehaviour {

	public Vector3 impact = Vector3.zero;
	private CharacterController character;
	
	void Start()
	{
	  character = GetComponent<CharacterController>();
	}
	
	// call this function to add an impact force:
	public void AddImpact(Vector3 dir, float force)
	{
		dir.Normalize();
	  	if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
	  	impact += dir.normalized * force / character.rigidbody.mass;
	}
	
	void Update()
	{
		// apply the impact force:
	  	if (impact.magnitude > 0.2) character.Move(impact * Time.deltaTime);
	  	// consumes the impact energy each cycle:
	  	impact = Vector3.Lerp(impact, Vector3.zero, Time.deltaTime);
	}
}
