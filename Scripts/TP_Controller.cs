using UnityEngine;
using System.Collections;

public class TP_Controller : MonoBehaviour {

	public enum directions
    {
        idle,
        moveForward,
        moveBackward,
        moveForwardLeft,
        moveBackwardLeft,
        moveForwardRight,
        moveBackwardRight,
        moveLeft,
        moveRight
    }

    public float forwardSpeed = 10f;
    public float backwardSpeed = 5f;
    public float sideSpeed = 8f;
    public float maxY = 90f;
    public float minY = 0f;
    public float distance = 2.5f;
    public float maxDistance = 5f;
    public float minDistance = 1f;
    public float minDistanceClips = 0.2f;
    public float scrollSensitivity = 0.5f;
    public float mouseSensitivity = 10f;
    public float lastDistance = 2.5f;
    public float FPSMaxY = 60f;
    public float FPSMinY = -60f;
    public float deadZone = 0.1f;
    public bool ThirdPerson = true;

    public directions mState;

    float angleX = 0f;
    float angleY = 0f;
    bool freeCam = false;

    public static TP_Controller instance;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        determineDirection();
        performMovement();
        if (ThirdPerson)
        {
            getScrollWheelZoom();
            if (freeCam)
                cameraMovement();
            else
                cameraMovementWithSnap();

            cameraFreeMove();
        }
        else
        {
            firstPersonCameraControll();
        }
        getButtonPressed();
    }

    void getButtonPressed()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            ThirdPerson = !ThirdPerson;
    }

    void firstPersonCameraControll()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseX = Input.GetAxis("Mouse X");
        if (mouseX < deadZone && mouseX > -deadZone)
            mouseX = 0f;
        if (mouseY < deadZone && mouseY > -deadZone)
            mouseY = 0f;

        angleX += mouseX * mouseSensitivity;
        angleY += mouseY * mouseSensitivity;

        angleY = Mathf.Clamp(angleY, FPSMinY, FPSMaxY);
        transform.rotation = Quaternion.Euler(transform.rotation.x, angleX, transform.rotation.z);
        if (!ThirdPerson)
            TP_Camera.instance.rotateFPS(angleY);
    }

    void cameraMovementWithSnap()
    {
        float mouseX = Input.GetAxis("Mouse Y");
        float mouseY = Input.GetAxis("Mouse X");
        angleX -= mouseY * mouseSensitivity;
        angleY += mouseX * mouseSensitivity;

        angleY = Mathf.Clamp(angleY, minY, maxY);
        transform.rotation = Quaternion.Euler(transform.rotation.x, -angleX, transform.rotation.z);
        TP_Camera.instance.rotateSnap(distance, angleY);
    }

    void cameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse Y");
        float mouseY = Input.GetAxis("Mouse X");
        angleX -= mouseY * mouseSensitivity;
        angleY += mouseX * mouseSensitivity;

        angleY = Mathf.Clamp(angleY, minY, maxY);
        TP_Camera.instance.rotate(distance, angleX-90, angleY);
    }

    void cameraFreeMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            freeCam = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            freeCam = false;
        }
    }

    void getScrollWheelZoom()
    {
		
        if (!TP_Camera.instance.clips) {
			if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
				distance -= scrollSensitivity;
                lastDistance = distance;

			} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				distance += scrollSensitivity;
                lastDistance = distance;

			} 
        }

        if (!TP_Camera.instance.clips)
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        else
            distance = Mathf.Clamp(distance, minDistanceClips, maxDistance);
        lastDistance = Mathf.Clamp(lastDistance, minDistance, maxDistance);
    }

    void performMovement()
    {
        switch (mState)
        {
            case directions.idle:
                // anims
                break;
            case directions.moveBackward:
                TP_Motor.instance.Move(transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"), backwardSpeed);
                break;
            case directions.moveBackwardLeft:
                TP_Motor.instance.Move(transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"), backwardSpeed);
                break;
            case directions.moveBackwardRight:
                TP_Motor.instance.Move(transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"), backwardSpeed);
                break;
            case directions.moveForward:
                TP_Motor.instance.Move(transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"), forwardSpeed);
                break;
            case directions.moveForwardLeft:
                TP_Motor.instance.Move(transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"), forwardSpeed);
                break;
            case directions.moveForwardRight:
                TP_Motor.instance.Move(transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"), forwardSpeed);
                break;
            case directions.moveLeft:
                TP_Motor.instance.Move(transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"), sideSpeed);
                break;
            case directions.moveRight:
                TP_Motor.instance.Move(transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal"), sideSpeed);
                break;
        };
    }

    void determineDirection()
    {
        if(Input.GetAxis("Vertical") > 0)
        {
            if(Input.GetAxis("Horizontal") > 0)
            {
                mState = directions.moveForwardRight;
            }
            else if(Input.GetAxis("Horizontal") < 0)
            {
                mState = directions.moveForwardLeft;
            }
            else
            {
                mState = directions.moveForward;
            }
        }
        else if(Input.GetAxis("Vertical") < 0)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                mState = directions.moveBackwardRight;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                mState = directions.moveBackwardLeft;
            }
            else
            {
                mState = directions.moveBackward;
            }
        }
        else
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                mState = directions.moveRight;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                mState = directions.moveLeft;
            }
            else
            {
                mState = directions.idle;
            }
        }
    }
}
