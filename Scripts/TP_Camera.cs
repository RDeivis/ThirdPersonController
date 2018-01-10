using UnityEngine;
using System.Collections;

public class TP_Camera : MonoBehaviour {

    public GameObject target;
    public GameObject cameraLookAt;
    public GameObject cameraPos;
    public bool clips = false;
    private float x = 0f;
    private float y = 0f;
    private float y2 = 0f;
    private float z = 0f;
    public float zoomSpeed = 0.25f;
    private bool debugMode = false;
    public static TP_Camera instance;
    private float distanceZ = 2.5f;
	private float lDist = 2.5f;
    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (TP_Controller.instance.ThirdPerson)
            CheckIfInTheWay();
        else
            transform.position = target.transform.position + target.transform.forward * 0.2f;
        
        if (Input.GetKeyDown(KeyCode.F3))
            debugMode = !debugMode;

		float vel = 0f;
		distanceZ = Mathf.SmoothDamp (distanceZ, lDist, ref vel, 0.1f);

		if(clips)
			cameraPos.transform.position = target.transform.position - target.transform.forward * lDist;
		else
			cameraPos.transform.position = target.transform.position - target.transform.forward * distanceZ;
    }
		
    public void rotateFPS(float angleY)
    {
        transform.LookAt(cameraLookAt.transform);
		cameraLookAt.transform.position = new Vector3(cameraLookAt.transform.position.x, cameraPos.transform.position.y + angleY / 100, cameraLookAt.transform.position.z);
    }

    public void rotate(float distance, float anglesX, float anglesY)
    {
		
        //distanceZ = distance;
		lDist = distance;
		if (!clips) {
			x = target.transform.position.x + distanceZ * Mathf.Cos (anglesX * Mathf.PI / 180);
			z = target.transform.position.z + distanceZ * Mathf.Sin (anglesX * Mathf.PI / 180);
			y = target.transform.position.y + distanceZ * Mathf.Cos (anglesY * Mathf.PI / 180);
		} else {
			x = target.transform.position.x + lDist * Mathf.Cos (anglesX * Mathf.PI / 180);
			z = target.transform.position.z + lDist * Mathf.Sin (anglesX * Mathf.PI / 180);
			y = target.transform.position.y + lDist * Mathf.Cos (anglesY * Mathf.PI / 180);
		}
        transform.LookAt(target.transform);
        transform.position = new Vector3(x, y, z);
        transform.LookAt(target.transform);
    }
    public void rotateSnap(float distance, float anglesY)
    {
        //distanceZ = distance;
		lDist = distance;
		if (!clips) {
			y = target.transform.position.y + distanceZ * Mathf.Cos (anglesY * Mathf.PI / 180);
		} else {
			y = target.transform.position.y + lDist * Mathf.Cos (anglesY * Mathf.PI / 180);
		}
        transform.LookAt(target.transform);
        transform.position = new Vector3(cameraPos.transform.position.x, y, cameraPos.transform.position.z);
        transform.LookAt(target.transform);
    }

    void CheckIfInTheWay()
    {
        float distance = GetComponent<Camera>().nearClipPlane;
        float aspect = GetComponent<Camera>().aspect;
        float FOV = GetComponent<Camera>().fieldOfView * Mathf.Deg2Rad;
        Vector3 pos = transform.position;

        float height = Mathf.Tan(FOV);
        float width = height * aspect;

        float height2 = height;
        float width2 = width;

        height += 1f;
        width += 1.5f;

        

        if (debugMode)
        {
            Debug.DrawLine(target.transform.position, pos + transform.up * height * distance + transform.right * width * distance - transform.forward, Color.blue);
            Debug.DrawLine(target.transform.position, pos - transform.up * height * distance + transform.right * width * distance - transform.forward, Color.blue);
            Debug.DrawLine(target.transform.position, pos - transform.up * height * distance - transform.right * width * distance - transform.forward, Color.blue);
            Debug.DrawLine(target.transform.position, pos + transform.up * height * distance - transform.right * width * distance - transform.forward, Color.blue);
            Debug.DrawRay(target.transform.position, transform.position - target.transform.position - transform.forward, Color.red);

            Debug.DrawLine(transform.position + transform.up * height2 * distance + transform.right * width2 * distance, transform.position - transform.up * height2 * distance + transform.right * width2 * distance, Color.black);
            Debug.DrawLine(transform.position - transform.up * height2 * distance + transform.right * width2 * distance, transform.position - transform.up * height2 * distance - transform.right * width2 * distance, Color.black);
            Debug.DrawLine(transform.position - transform.up * height2 * distance - transform.right * width2 * distance, transform.position + transform.up * height2 * distance - transform.right * width2 * distance, Color.black);
            Debug.DrawLine(transform.position + transform.up * height2 * distance - transform.right * width2 * distance, transform.position + transform.up * height2 * distance + transform.right * width2 * distance, Color.black);
        }

        RaycastHit hit;

        if (Physics.Linecast(target.transform.position, pos + transform.up * height * distance + transform.right * width * distance - transform.forward, out hit))
        {
            if (hit.collider.tag != "MainCamera" && hit.collider.tag != "Player")
            {
                clips = true;
                TP_Controller.instance.distance -= zoomSpeed;
            }
        }


        else if (Physics.Linecast(target.transform.position, pos - transform.up * height * distance + transform.right * width * distance - transform.forward, out hit))
        {
            if (hit.collider.tag != "MainCamera" && hit.collider.tag != "Player")
            {
                TP_Controller.instance.distance -= zoomSpeed;
                clips = true;
            }
        }


        else if (Physics.Linecast(target.transform.position, pos - transform.up * height * distance - transform.right * width * distance - transform.forward, out hit))
        {
            if (hit.collider.tag != "MainCamera" && hit.collider.tag != "Player")
            {
                TP_Controller.instance.distance -= zoomSpeed;
                clips = true;
            }
        }


        else if (Physics.Linecast(target.transform.position, pos + transform.up * height * distance - transform.right * width * distance - transform.forward, out hit))
        {
            if (hit.collider.tag != "MainCamera" && hit.collider.tag != "Player")
            {
                TP_Controller.instance.distance -= zoomSpeed;
                clips = true;
            }
        }


        else if (Physics.Raycast(target.transform.position, transform.position - target.transform.position, out hit, TP_Controller.instance.distance - 1f))
        {
            if (hit.collider.tag != "MainCamera" && hit.collider.tag != "Player")
            {
                TP_Controller.instance.distance -= zoomSpeed;
                clips = true;
            }
        }

        else if(!Physics.Raycast(target.transform.position, transform.position - target.transform.position, out hit, TP_Controller.instance.lastDistance - 2f) &&
            !Physics.Linecast(target.transform.position, pos + transform.up * height * distance + transform.right * width * distance - transform.forward * 2f, out hit) &&
            !Physics.Linecast(target.transform.position, pos + transform.up * height * distance - transform.right * width * distance - transform.forward * 2f, out hit) &&
            !Physics.Linecast(target.transform.position, pos - transform.up * height * distance - transform.right * width * distance - transform.forward * 2f, out hit) &&
            !Physics.Linecast(target.transform.position, pos - transform.up * height * distance + transform.right * width * distance - transform.forward * 2f, out hit))//l
        {

            if(TP_Controller.instance.distance != TP_Controller.instance.lastDistance)
                TP_Controller.instance.distance += zoomSpeed;
            clips = false;
        }

       
    }
}
