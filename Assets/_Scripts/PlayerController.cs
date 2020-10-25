using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public BulletManager bulletManager;

    [Header("Boundary Check")]
    public float horizontalBoundary;

    [Header("Player Speed")]
    public float horizontalSpeed;
    public float maxSpeed;
    public float horizontalTValue;

    [Header("Bullet Firing")]
    public float fireDelay;

    // Private variables
    private Rigidbody2D m_rigidBody;
    private Vector3 m_touchesEnded;
    private ScreenOrientation screenOrient;
    private RectTransform playerRect;

    // Start is called before the first frame update
    void Start()
    {
        m_touchesEnded = new Vector3();
        m_rigidBody = GetComponent<Rigidbody2D>();
        GameController.OrientationChange.AddListener(_OrientationChange);
        playerRect = GetComponentInParent<RectTransform>();
        screenOrient = Screen.orientation;
        if (screenOrient == ScreenOrientation.LandscapeLeft)
            playerRect.Rotate(0, 0, -90);
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _CheckBounds();
        _FireBullet();
    }

     private void _FireBullet()
    {
        // delay bullet firing 
        if(Time.frameCount % 60 == 0 && bulletManager.HasBullets())
        {
            bulletManager.GetBullet(transform.position);
        }
    }

    private void _Move()
    {
        float direction = 0.0f;

        if (screenOrient == ScreenOrientation.Portrait)
        {
            // touch input support
            foreach (var touch in Input.touches)
            {
                var worldTouch = Camera.main.ScreenToWorldPoint(touch.position);

                if (worldTouch.x > transform.position.x)
                {
                    // direction is positive
                    direction = 1.0f;
                }

                if (worldTouch.x < transform.position.x)
                {
                    // direction is negative
                    direction = -1.0f;
                }

                m_touchesEnded = worldTouch;

            }

            // keyboard support
            if (Input.GetAxis("Horizontal") >= 0.1f)
            {
                // direction is positive
                direction = 1.0f;
            }

            if (Input.GetAxis("Horizontal") <= -0.1f)
            {
                // direction is negative
                direction = -1.0f;
            }

            if (m_touchesEnded.x != 0.0f)
            {
                transform.position = new Vector2(Mathf.Lerp(transform.position.x, m_touchesEnded.x, horizontalTValue), transform.position.y);
            }
            else
            {
                Vector2 newVelocity = m_rigidBody.velocity + new Vector2(direction * horizontalSpeed, 0.0f);
                m_rigidBody.velocity = Vector2.ClampMagnitude(newVelocity, maxSpeed);
                m_rigidBody.velocity *= 0.99f;
            }
        }
        else if (screenOrient == ScreenOrientation.LandscapeLeft)
        {
            // touch input support
            foreach (var touch in Input.touches)
            {
                var worldTouch = Camera.main.ScreenToWorldPoint(touch.position);
               // Debug.Log(worldTouch.x + " w " + worldTouch.y);     
                //Debug.Log(playerRect.anchoredPosition.y);
                if (worldTouch.y > playerRect.anchoredPosition.y + Screen.height / 2)
                {
                    // direction is positive
                    direction = 1.0f;
                }

                if (worldTouch.y < playerRect.anchoredPosition.y + Screen.height / 2)
                {
                    // direction is negative
                    direction = -1.0f;
                }

                m_touchesEnded = worldTouch;

            }

            // keyboard support
            if (Input.GetAxis("Vertical") >= 0.1f)
            {
                // direction is positive
                direction = 1.0f;
            }

            if (Input.GetAxis("Vertical") <= -0.1f)
            {
                // direction is negative
                direction = -1.0f;
            }

            if (m_touchesEnded.y != 0.0f)
            {
                //Debug.Log(m_touchesEnded.x + " w " + m_touchesEnded.y);
                transform.position = new Vector2(transform.position.x, Mathf.Lerp(transform.position.y, m_touchesEnded.y, horizontalTValue));
            }
            else
            {
                Vector2 newVelocity = m_rigidBody.velocity + new Vector2(0, direction * horizontalSpeed);
                m_rigidBody.velocity = Vector2.ClampMagnitude(newVelocity, maxSpeed);
                m_rigidBody.velocity *= 0.99f;
            }
        }
    }

    private void _CheckBounds()
    {        
        // check right bounds
        if (screenOrient == ScreenOrientation.Portrait && playerRect.anchoredPosition.x >= horizontalBoundary)        //if (transform.position.x >= horizontalBoundary)
        {
            playerRect.anchoredPosition = new Vector2(horizontalBoundary, playerRect.anchoredPosition.y);            //transform.position = new Vector3(horizontalBoundary, transform.position.y, 0.0f);
        }
        if (screenOrient == ScreenOrientation.LandscapeLeft && playerRect.anchoredPosition.y >= horizontalBoundary)        //if (transform.position.x >= horizontalBoundary)
        {
            playerRect.anchoredPosition = new Vector2(playerRect.anchoredPosition.x, horizontalBoundary);            //transform.position = new Vector3(horizontalBoundary, transform.position.y, 0.0f);
        }
        // check left bounds
        if (screenOrient == ScreenOrientation.Portrait && playerRect.anchoredPosition.x <= -horizontalBoundary)        //if (transform.position.x <= -horizontalBoundary)
        {
            playerRect.anchoredPosition = new Vector2(-horizontalBoundary, playerRect.anchoredPosition.y);            //transform.position = new Vector3(-horizontalBoundary, transform.position.y, 0.0f);
        }
        if (screenOrient == ScreenOrientation.LandscapeLeft && playerRect.anchoredPosition.y <= -horizontalBoundary)        //if (transform.position.x <= -horizontalBoundary)
        {
            playerRect.anchoredPosition = new Vector2(playerRect.anchoredPosition.x, -horizontalBoundary);              //transform.position = new Vector3(-horizontalBoundary, transform.position.y, 0.0f);
        }

    }

    private void _OrientationChange(ScreenOrientation scrOri)
    {
        screenOrient = scrOri;
        if (scrOri == ScreenOrientation.LandscapeLeft)
        {
            playerRect.sizeDelta = new Vector2(playerRect.rect.height, playerRect.rect.width);
            playerRect.Rotate(0, 0, -90);
            playerRect.pivot = new Vector2(playerRect.pivot.y, playerRect.pivot.x);
            playerRect.anchorMax = new Vector2(playerRect.anchorMax.y, playerRect.anchorMax.x);
            playerRect.anchorMin = new Vector2(playerRect.anchorMin.y, playerRect.anchorMin.x);
            playerRect.anchoredPosition = new Vector3(playerRect.anchoredPosition.y, playerRect.anchoredPosition.x, 0);
            m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.y, m_rigidBody.velocity.x);
        }
        if (scrOri == ScreenOrientation.Portrait)
        {
            playerRect.sizeDelta = new Vector2(playerRect.rect.height, playerRect.rect.width);
            playerRect.Rotate(0, 0, 90);
            playerRect.pivot = new Vector2(playerRect.pivot.y, playerRect.pivot.x);
            playerRect.anchorMax = new Vector2(playerRect.anchorMax.y, playerRect.anchorMax.x);
            playerRect.anchorMin = new Vector2(playerRect.anchorMin.y, playerRect.anchorMin.x);
            playerRect.anchoredPosition = new Vector3(playerRect.anchoredPosition.y, playerRect.anchoredPosition.x, 0);
            m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.y, m_rigidBody.velocity.x);
        }
    }
}
