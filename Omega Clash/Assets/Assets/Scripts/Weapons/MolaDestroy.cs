using UnityEngine;
using System.Collections;

public class MolaDestroy : MonoBehaviour {

	private float time_to_live = 1.0f;
	private float starttime = 0.0f;
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		starttime +=Time.deltaTime;
		if (starttime > time_to_live)
		{
			Destroy(this.gameObject.GetComponent<SpringJoint>());
			Destroy(this);
		}
	}
}
