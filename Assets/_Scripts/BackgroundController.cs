using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float verticalSpeed;
    public float verticalBoundary;

    void Start()
    {
        Rect screenRect = GetComponentInParent<RectTransform>().rect;
        //Debug.Log(screenRect.width + " " + screenRect.height);
        //Debug.Log(Screen.width + " " + Screen.height);
        GameController.OrientationChange.AddListener(_OrientationChange);
    }


    void Update()
    {
        _Move();
        _CheckBounds();
    }

    private void _Reset()
    {
        transform.position += new Vector3(0.0f, -verticalBoundary * 4);
    }

    private void _Move()
    {
        transform.position -= new Vector3(0.0f, verticalSpeed, 0) * Time.deltaTime;
    }

    private void _CheckBounds()
    {
        //Debug.Log(transform.position.y);
        // if the background is lower than the bottom of the screen then reset
        if (transform.position.y <= verticalBoundary)
        {
            _Reset();
        }
    }
    private void _OrientationChange(ScreenOrientation scrOri)
    {

    }
}
