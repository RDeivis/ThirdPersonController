using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSCounter : MonoBehaviour {

    float FPS = 0f;
    float showDelay = 0f;
    bool debugMode = false;

	void Update () {
        getButtonToggled();
        checkToPrint();
    }

    void checkToPrint()
    {
        if (debugMode)
        {
            if (showDelay > 0.25)
            {
                calculateFPS();
                showDelay = 0;
            }
            else
            {
                showDelay = showDelay + Time.deltaTime;
            }
        }
        else
            GetComponent<Text>().text = "";
    }

    void calculateFPS()
    {
        FPS = 1.0f / Time.deltaTime;
        GetComponent<Text>().text = "FPS: " + Mathf.Floor(FPS);
    }

    void getButtonToggled()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            debugMode = !debugMode;
        }
    }
}
