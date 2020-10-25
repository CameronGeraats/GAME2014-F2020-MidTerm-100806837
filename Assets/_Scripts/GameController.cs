using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * File: GameController.cs
 * Name: Cameron Geraats
 * ID: 100806837
 * Last Modified: 24/10/20
 * Description:
 *      Manages the game orientation and Camera size
 * Revisions: No previous revisions
 */
public class GameController : MonoBehaviour
{
    public static UnityEvent<ScreenOrientation> OrientationChange = new UnityEvent<ScreenOrientation>();
    ScreenOrientation screenOrient;

    // Start is called before the first frame update
    // Sets up the private variables and modifies the camera size based on the device
    void Start()
    {
        Debug.Log("Start Orient: " + Screen.orientation);
        screenOrient = Screen.orientation;
        //Debug.Log(Screen.safeArea);

        // Adjusts the camera size to reflect the current device screen size
        Camera.main.orthographicSize = Screen.height / 2;
    }

    // Update is called once per frame
    // Invokes the Orientation change event if the orientation changes
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
