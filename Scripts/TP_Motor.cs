using UnityEngine;
using System.Collections;

public class TP_Motor : MonoBehaviour {

    public static TP_Motor instance;

    void Start()
    {
        instance = this;
    }

    public void Move(Vector3 direction, float speed)
    {
		//GetComponent<CharacterController>().Move(direction * speed * Time.deltaTime);
		GetComponent<Rigidbody>().position += direction * speed * Time.deltaTime;
    }
}
