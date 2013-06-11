using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserRays : MonoBehaviour {
	
	public Transform LaserRay;
	

	public List<Transform> before = new List<Transform>();
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(before.Count!=0)
		{
			for(int i=0;i<before.Count;i++)
			{
				Destroy(before[i].gameObject);
			}
			before.Clear();
		}
		FireAll fireall=(FireAll)(this.gameObject.GetComponent<FireAll>());
		if(Input.GetMouseButton(0) && fireall.weapons[fireall.weapon_selected].laser)
		{
			Transform origin = new GameObject().transform;
			origin.position = transform.position;
			origin.forward = transform.forward;
			int count=0;
			while(true && count<3)
			{
				count++;
				Vector3 fwd = origin.TransformDirection(Vector3.forward);
				RaycastHit info = new RaycastHit();
				//Debug.DrawRay(transform.position,fwd,Color.red,1000);
	        	if (Physics.Raycast(origin.position, fwd, out info, 1000.0f))
				{
					Vector3 halfway = origin.position + ((info.point - origin.position)/2);
					float halfdistance = ((info.point - origin.position).magnitude)/2;
					Transform myray;
					myray=(Transform)Network.Instantiate(LaserRay, halfway, origin.rotation, 0);
					myray.localScale += new Vector3(0,halfdistance,0);
					myray.Rotate(new Vector3(90,0,0));
					
					before.Add(myray);
	
					if(info.collider.gameObject.tag == "Mirror")
					{
						Vector3 direction = info.point - origin.position;
						Vector3 reflection = Vector3.Reflect(direction, info.normal);
	
						origin.position = info.point;
						origin.forward = reflection;
						Debug.DrawRay(origin.position,origin.forward,Color.red,2);
						//print("MIRROR" + Random.Range(-100,100));
					}
					else
					{						
						return;
					}
				}
				else
				{
					Vector3 halfway = origin.position + (fwd * 500);
					Transform myray;
					myray=(Transform)Network.Instantiate(LaserRay, halfway, origin.rotation, 0);
					myray.localScale += new Vector3(0,500,0);
					myray.Rotate(new Vector3(90,0,0));
					
					before.Add(myray);
				}
			}
		}
	}
}