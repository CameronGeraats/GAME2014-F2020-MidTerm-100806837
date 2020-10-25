using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

/*
 * File: PlayerController.cs
 * Name: Cameron Geraats
 * ID: 100806837
 * Last Modified: 24/10/20
 * Description:
 *      Manages the player movement, input, bullet firing, and restricts player to screen boundary
 * Revisions: No previous revisions
 */
public class PlayerController : MonoBehaviour
{
    public BulletManager bulletManager;

    [Header("Boundary Check")]
    public float boundary;

    [Header("Player Speed")]
    public float speed;
    public float maxSpeed;
    public float lerpValue;

    [Header("Bullet Firing")]
    public float fireDelay;

    // Private variables
    private Rigidbody2D m_rigidBody;
    private Vector3 m_touchesEnded;
    private ScreenOrientation screenOrient;
    private RectTransform playerRect;

    // Start is called before the first frame update
    // Sets up the private variables and modifies the rotation if the screen is rotated
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
    // Runs Movement, Boundary Checking, and Bullet Firing methods per frame
    void Update()
    {
        _Move();
        _CheckBounds();
        _FireBullet();
    }

    // Fires a bullet every 60 frames from the Bullet Pool if available
     private void _FireBullet()
    {
        // delay bullet firing 
        if(Time.frameCount % 60 == 0 && bulletManager.HasBullets())
        {
            bulletManager.GetBullet(transform.position);
        }
    }

    // Handles player input and Moves the player avatar
    private void _Move()
    {
        float direction = 0.0f;
        // Handles portrait mode input
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
                transform.position = new Vector2(Mathf.Lerp(transform.position.x, m_touchesEnded.x, lerpValue), transform.position.y);
            }
            else
            {
                Vector2 newVelocity = m_rigidBody.velocity + new Vector2(direction * speed, 0.0f);
                m_rigidBody.velocity = Vector2.ClampMagnitude(newVelocity, maxSpeed);
                m_rigidBody.velocity *= 0.99f;
            }
        }
        // Handles landscape left mode input
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
                transform.position = new Vector2(transform.position.x, Mathf.Lerp(transform.position.y, m_touchesEnded.y, lerpValue));
            }
            else
            {
                Vector2 newVelocity = m_rigidBody.velocity + new Vector2(0, direction * speed);
                m_rigidBody.velocity = Vector2.ClampMagnitude(newVelocity, maxSpeed);
                m_rigidBody.velocity *= 0.99f;
            }
        }
    }

    // Checks that the player is between the screen boundaries
    // Portrait orientation checks the X-axis
    // Landscape orientation checks the Y-axis
    private void _CheckBounds()
    {        
        // check right bounds
        if (screenOrient == ScreenOrientation.Portrait && playerRect.anchoredPosition.x >= boundary)        //if (transform.position.x >= horizontalBoundary)
        {
            playerRect.anchoredPosition = new Vector2(boundary, playerRect.anchoredPosition.y);            //transform.position = new Vector3(horizontalBoundary, transform.position.y, 0.0f);
        }
        if (screenOrient == ScreenOrientation.LandscapeLeft && playerRect.anchoredPosition.y >= boundary)        //if (transform.position.x >= horizontalBoundary)
        {
            playerRect.anchoredPosition = new Vector2(playerRect.anchoredPosition.x, boundary);            //transform.position = new Vector3(horizontalBoundary, transform.position.y, 0.0f);
        }
        // check left bounds
        if (screenOrient == ScreenOrientation.Portrait && playerRect.anchoredPosition.x <= -boundary)        //if (transform.position.x <= -horizontalBoundary)
        {
            playerRect.anchoredPosition = new Vector2(-boundary, playerRect.anchoredPosition.y);            //transform.position = new Vector3(-horizontalBoundary, transform.position.y, 0.0f);
        }
        if (screenOrient == ScreenOrientation.LandscapeLeft && playerRect.anchoredPosition.y <= -boundary)        //if (transform.position.x <= -horizontalBoundary)
        {
            playerRect.anchoredPosition = new Vector2(playerRect.anchoredPosition.x, -boundary);              //transform.position = new Vector3(-horizontalBoundary, transform.position.y, 0.0f);
        }

    }

    // When the Orientation is changed, this method re-orients the player to the new coordinates and rotation
    // This class has a listener to the GameController event which gets invoked
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
