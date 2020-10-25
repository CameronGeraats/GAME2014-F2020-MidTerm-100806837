using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * File: CameraController.cs
 * Name: Cameron Geraats
 * ID: 100806837
 * Last Modified: 24/10/20
 * Description:
 *      Manages the camera rotation, and any other future camera scripts
 * Revisions: No previous revisions
 */
public class CameraController : MonoBehaviour
{
    private ScreenOrientation screenOrient;

    // Start is called before the first frame update
    // Listens for Orientation change
    void Start()
    {
        GameController.OrientationChange.AddListener(_OrientationChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _OrientationChange(ScreenOrientation scrOri)
    {
        screenOrient = scrOri;
        if (scrOri == ScreenOrientation.LandscapeLeft)
        {
            //float temp = GetComponentInParent<RectTransform>().rect.width;
            GetComponentInParent<RectTransform>().sizeDelta = new Vector2(3040,1440);
        }
        if (scrOri == ScreenOrientation.Portrait)
        {
            GetComponentInParent<RectTransform>().sizeDelta = new Vector2(1440, 3040);
        }
    }
}
