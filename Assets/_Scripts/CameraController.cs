using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private ScreenOrientation screenOrient;
    // Start is called before the first frame update
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
