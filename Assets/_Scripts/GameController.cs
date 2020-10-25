using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static UnityEvent<ScreenOrientation> OrientationChange = new UnityEvent<ScreenOrientation>();
    ScreenOrientation screenOrient;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Orient: " + Screen.orientation);
        screenOrient = Screen.orientation;
        Debug.Log(Screen.safeArea);

        // Adjusts the camera size to reflect the current device screen size
        Camera.main.orthographicSize = Screen.height / 2;
    }

    // Update is called once per frame
    void Update()
    {       
        if (screenOrient != Screen.orientation)
        {
            screenOrient = Screen.orientation;
            OrientationChange.Invoke(Screen.orientation);
            Debug.Log("Update Orient: " + Screen.orientation);
            Camera.main.orthographicSize = Screen.height / 2;
        }
    }
}
