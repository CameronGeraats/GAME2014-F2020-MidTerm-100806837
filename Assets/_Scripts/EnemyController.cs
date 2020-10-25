using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * File: EnemyController.cs
 * Name: Cameron Geraats
 * ID: 100806837
 * Last Modified: 24/10/20
 * Description:
 *      Manages the enemy movement, and restricts enemy to screen boundary
 * Revisions: No previous revisions
 */
public class EnemyController : MonoBehaviour
{
    public float horizontalSpeed;
    public float horizontalBoundary;
    public float direction;

    private ScreenOrientation screenOrient;
    private RectTransform enemyRect;

    // Start is called before the first frame update
    // Sets up the private variables and modifies the rotation if the screen is rotated
    void Start()
    {
        GameController.OrientationChange.AddListener(_OrientationChange);
        enemyRect = GetComponentInParent<RectTransform>();
        screenOrient = Screen.orientation;
        if (screenOrient == ScreenOrientation.LandscapeLeft)
            enemyRect.Rotate(0, 0, -90);
    }
    
    // Moves the enemy then checks the boundary
    void Update()
    {
        _Move();
        _CheckBounds();
    }

    // Moves the enemy along one direction, X-axis for portrait OR Y-axis for Landscape
    private void _Move()
    {
        if (screenOrient == ScreenOrientation.Portrait)
            transform.position += new Vector3(horizontalSpeed * direction * Time.deltaTime, 0.0f, 0.0f);
        else if (screenOrient == ScreenOrientation.LandscapeLeft)            
            transform.position += new Vector3(0, horizontalSpeed * direction * Time.deltaTime, 0.0f);
    }


    // Checks that the enemy is between the screen boundaries
    // Portrait orientation checks the X-axis
    // Landscape orientation checks the Y-axis
    private void _CheckBounds()
    {   // check right bounds
        if (screenOrient == ScreenOrientation.Portrait && enemyRect.anchoredPosition.x >= horizontalBoundary)     
        {
            direction = -1.0f;
        }
        if (screenOrient == ScreenOrientation.LandscapeLeft && enemyRect.anchoredPosition.y >= horizontalBoundary)      
        {
            direction = -1.0f;
        }
        // check left bounds
        if (screenOrient == ScreenOrientation.Portrait && enemyRect.anchoredPosition.x <= -horizontalBoundary)        
        {
            direction = 1.0f;
        }
        if (screenOrient == ScreenOrientation.LandscapeLeft && enemyRect.anchoredPosition.y <= -horizontalBoundary)       
        {
            direction = 1.0f;
        }
    }

    // When the Orientation is changed, this method re-orients the enemy to the new coordinates and rotation
    // This class has a listener to the GameController event which gets invoked
    private void _OrientationChange(ScreenOrientation scrOri)
    {
        screenOrient = scrOri;
        if (scrOri == ScreenOrientation.LandscapeLeft)
        {
            enemyRect.sizeDelta = new Vector2(enemyRect.rect.height, enemyRect.rect.width);
            enemyRect.Rotate(0, 0, -90);
            enemyRect.pivot = new Vector2(enemyRect.pivot.y, enemyRect.pivot.x);
            enemyRect.anchorMax = new Vector2(enemyRect.anchorMax.y, enemyRect.anchorMax.x);
            enemyRect.anchorMin = new Vector2(enemyRect.anchorMin.y, enemyRect.anchorMin.x);
            enemyRect.anchoredPosition = new Vector3(enemyRect.anchoredPosition.y, enemyRect.anchoredPosition.x, 0);           
        }
        if (scrOri == ScreenOrientation.Portrait)
        {
            enemyRect.sizeDelta = new Vector2(enemyRect.rect.height, enemyRect.rect.width);
            enemyRect.Rotate(0, 0, 90);
            enemyRect.pivot = new Vector2(enemyRect.pivot.y, enemyRect.pivot.x);
            enemyRect.anchorMax = new Vector2(enemyRect.anchorMax.y, enemyRect.anchorMax.x);
            enemyRect.anchorMin = new Vector2(enemyRect.anchorMin.y, enemyRect.anchorMin.x);
            enemyRect.anchoredPosition = new Vector3(enemyRect.anchoredPosition.y, enemyRect.anchoredPosition.x, 0);           
        }
    }
}
