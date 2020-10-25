using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float verticalSpeed;
    public float verticalBoundary;

    private ScreenOrientation screenOrient;
    private RectTransform backgroundTransf;

    void Start()
    {
        Rect screenRect = GetComponentInParent<RectTransform>().rect;
        //Debug.Log(screenRect.width + " " + screenRect.height);
        //Debug.Log(Screen.width + " " + Screen.height);
        GameController.OrientationChange.AddListener(_OrientationChange);
        screenOrient = Screen.orientation;
        if(screenOrient == ScreenOrientation.LandscapeLeft)
            GetComponentInParent<RectTransform>().Rotate(0, 0, -90);
        backgroundTransf = GetComponentInParent<RectTransform>();
    }


    void Update()
    {
        _Move();
        _CheckBounds();
    }

    private void _Reset()
    {
        if(screenOrient == ScreenOrientation.Portrait)
            transform.position += new Vector3(0.0f, -verticalBoundary * 4);
        if(screenOrient == ScreenOrientation.LandscapeLeft)
            transform.position += new Vector3(-verticalBoundary * 4, 0);
    }

    private void _Move()
    {
        if(screenOrient == ScreenOrientation.Portrait)
            transform.position -= new Vector3(0.0f, verticalSpeed, 0) * Time.deltaTime;
        else if(screenOrient == ScreenOrientation.LandscapeLeft)
            transform.position -= new Vector3(verticalSpeed, 0, 0) * Time.deltaTime;        
    }

    private void _CheckBounds()
    {
        //Debug.Log(transform.position.y);
        // if the background is lower than the bottom of the screen then reset
        
        if (screenOrient == ScreenOrientation.Portrait && transform.position.y <= verticalBoundary)
        {
            _Reset();
        }
        if (screenOrient == ScreenOrientation.LandscapeLeft && transform.position.x <= verticalBoundary)
        {
            _Reset();
        }
    }
    private void _OrientationChange(ScreenOrientation scrOri)
    {
        screenOrient = scrOri;
        if (scrOri == ScreenOrientation.LandscapeLeft)
        {
            //float temp = GetComponentInParent<RectTransform>().rect.width;
            backgroundTransf.sizeDelta = new Vector2(3040, 1440);
            backgroundTransf.Rotate(0,0,-90);
            backgroundTransf.anchoredPosition = new Vector3(backgroundTransf.anchoredPosition.y, backgroundTransf.anchoredPosition.x,0);
        }
        if (scrOri == ScreenOrientation.Portrait)
        {
            backgroundTransf.sizeDelta = new Vector2(1440, 3040);
            backgroundTransf.Rotate(0, 0, 90);
            backgroundTransf.anchoredPosition = new Vector3(backgroundTransf.anchoredPosition.y, backgroundTransf.anchoredPosition.x, 0);
        }
    }
}
