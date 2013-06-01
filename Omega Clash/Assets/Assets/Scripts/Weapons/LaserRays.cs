using UnityEngine;
using System.Collections;

public class LaserRays : MonoBehaviour {
	
	public Transform LaserRay;
	
	private Transform before;
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(before!=null)
			Destroy(before.gameObject);
		if(Input.GetMouseButton(0))
		{
			Vector3 fwd = transform.TransformDirection(Vector3.forward);
			RaycastHit info = new RaycastHit();
			//Debug.DrawRay(transform.position,fwd,Color.red,1000);
        	if (Physics.Raycast(transform.position, fwd, out info, 1000.0f))
			{
				Vector3 halfway = transform.position + ((info.point - transform.position)/2);
				float halfdistance = ((info.point - transform.position).magnitude)/2;
				before = (Transform)Network.Instantiate(LaserRay, halfway, transform.rotation, 0);
				before.localScale += new Vector3(0,halfdistance,0);
				before.Rotate(new Vector3(90,0,0));

				if(info.collider.gameObject.tag == "Mirror")
				{
					Vector3 direction = info.point - transform.position;
					Vector3 reflection = Vector3.Reflect(direction, info.normal);

					Debug.DrawRay(info.point,reflection,Color.red,1000);
					//print("MIRROR" + Random.Range(-100,100));
				}
			}

		}
	}
}