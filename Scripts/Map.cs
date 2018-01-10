using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public bool swing = false;
	public bool die = false;
	public bool lift1 = false;
	public bool lift2 = false;
	public bool lift3 = false;
	public float speed = 10f;
	public GameObject RotPoint;

	bool up = true;
	bool left = true;

	void Update(){
		if (swing) {

			float AngleZ = transform.rotation.eulerAngles.z;
			if (AngleZ > 49 && AngleZ < 51)
				left = false;
			else if (AngleZ > 309 && AngleZ < 311)
				left = true;

			if (left) {
				transform.RotateAround (RotPoint.transform.position, Vector3.forward, speed * Time.deltaTime);
			} else {
				transform.RotateAround (RotPoint.transform.position, Vector3.forward, -speed * Time.deltaTime);
			}

		} else if (die) {
			
		} else if (lift1) {
			float pos = transform.position.y;

			if (pos >= 10)
				up = false;
			else if (pos <= 0)
				up = true;

			if(up)
				transform.position += new Vector3 (0, 1*Time.deltaTime, 0);
			else
				transform.position -= new Vector3 (0, 1*Time.deltaTime, 0);
		} else if (lift2) {
			float pos = transform.position.y;

			if (pos >= 15)
				up = false;
			else if (pos <= 0)
				up = true;

			if(up)
				transform.position += new Vector3 (0, 1*Time.deltaTime, 0);
			else
				transform.position -= new Vector3 (0, 1*Time.deltaTime, 0);
		} else if (lift3) {
			float pos = transform.position.y;

			if (pos >= 25)
				up = false;
			else if (pos <= 0)
				up = true;

			if(up)
				transform.position += new Vector3 (0, 1*Time.deltaTime, 0);
			else
				transform.position -= new Vector3 (0, 1*Time.deltaTime, 0);
		} else {
			Debug.LogError ("Map object move type not selected!");
		}
	}

}
