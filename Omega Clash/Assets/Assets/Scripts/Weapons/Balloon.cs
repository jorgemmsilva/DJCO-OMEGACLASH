using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {

	public Transform content;
	public float time_to_live = 3000;
	public float speed = 10;
	public float gravity = 10.0f;
	
	private float starttime = 0;
	private float distancefire = 5;
	private Vector3 initialpos;
	
	// Use this for initialization
	void Start () {
		initialpos = transform.position;
		starttime = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		starttime +=Time.deltaTime;
		if (starttime > time_to_live)
		{
			Destroy(this.gameObject);
			Destroy(this);
		}
		if(networkView.isMine)
		{
		    if (transform.position.y > initialpos.y + distancefire)
			{
				// make it go down
				rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
		    }
			if (rigidbody.velocity.magnitude < 1)
			{					
				//give random force, y component zero
				Vector3 randomForce = Vector3.zero;
				randomForce.x = Random.Range(-1.0f, 1.0f);
				randomForce.z = Random.Range(-1.0f, 1.0f);
	        	rigidbody.AddForce(randomForce, ForceMode.VelocityChange);
			}
			// We apply going up manually
		    rigidbody.AddForce(new Vector3 (0, gravity * rigidbody.mass, 0));
		}	
	}
	
	void OnTriggerEnter(Collider other) {
		Network.Instantiate(content, transform.position, transform.rotation, 0);
		Destroy(this.gameObject);
        Destroy(this);		
    }
}
