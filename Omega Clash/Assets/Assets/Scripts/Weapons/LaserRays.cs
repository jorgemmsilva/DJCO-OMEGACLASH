using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserRays : MonoBehaviour {
	
	public Transform LaserPoint;
	public Transform LaserRay;
	
	public List<Transform> before = new List<Transform>();
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!transform.root.root.networkView.isMine)
			return;
		
		if(before.Count!=0)
		{
			for(int i=0;i<before.Count;i++)
			{
				Destroy(before[i].gameObject);
				Destroy(before[i]);
			}
			before.Clear();
		}
		FireAll fireall=(FireAll)(this.gameObject.GetComponent<FireAll>());
		if(Input.GetMouseButton(0) && fireall.weapons[fireall.weapon_selected].type == WeaponType.Laser)
		{
			Transform origin = new GameObject().transform;
			origin.position = transform.position;
			origin.forward = transform.forward;
			before.Add(origin);

			
			int count=0;
			while(count<25)
			{
				count++;
				Vector3 fwd = origin.TransformDirection(Vector3.forward);
				RaycastHit info = new RaycastHit();
				
				if (Physics.Raycast(origin.position, fwd, out info, 1000.0f))
				{
					Transform myray;
					myray=(Transform)Network.Instantiate(LaserPoint, origin.position, origin.rotation, 0);
					Destroy(myray.gameObject);
					Destroy(myray);
					//before.Add(myray);
					
					Transform myray_dest;
					myray_dest=(Transform)Network.Instantiate(LaserPoint, info.point, origin.rotation, 0);
					Destroy(myray_dest.gameObject);
					Destroy(myray_dest);
					//before.Add(myray_dest);
					
					Vector3 halfway = origin.position + ((info.point - origin.position)/2);
                    float halfdistance = ((info.point - origin.position).magnitude)/2;
                    Transform myray_middle;
                    myray_middle=(Transform)Instantiate(LaserRay, halfway, origin.rotation);
                    myray_middle.localScale += new Vector3(0,halfdistance,0);
                    myray_middle.Rotate(new Vector3(90,0,0));
                    before.Add(myray_middle);
	
					if(info.collider.gameObject.tag == "Mirror")
					{
						Vector3 direction = info.point - origin.position;
						Vector3 reflection = Vector3.Reflect(direction, info.normal);
	
						origin.position = info.point;
						origin.forward = reflection;
					}
					else
					{						
						return;
					}
				}
				else
				{
					Transform myray;
					myray=(Transform)Network.Instantiate(LaserPoint, origin.position, origin.rotation, 0);
					Destroy(myray.gameObject);
					Destroy(myray);
					//before.Add(myray);
					
					Transform myray_dest;
					myray_dest=(Transform)Network.Instantiate(LaserPoint, origin.position + (fwd * 1000), origin.rotation, 0);
					Destroy(myray_dest.gameObject);
					Destroy(myray_dest);
					//before.Add(myray_dest);
					
					Vector3 halfway = origin.position + (fwd * 500);
                    Transform myray_middle;
                    myray_middle=(Transform)Instantiate(LaserRay, halfway, origin.rotation);
                    myray_middle.localScale += new Vector3(0,500,0);
                    myray_middle.Rotate(new Vector3(90,0,0));
                    before.Add(myray_middle);
				}
			}
		}
	}
}