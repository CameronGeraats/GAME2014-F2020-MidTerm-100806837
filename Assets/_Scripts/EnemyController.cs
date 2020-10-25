using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float horizontalSpeed;
    public float horizontalBoundary;
    public float direction;

    private ScreenOrientation screenOrient;
    private RectTransform enemyRect;

    void Start()
    {
        GameController.OrientationChange.AddListener(_OrientationChange);
        enemyRect = GetComponentInParent<RectTransform>();
        screenOrient = Screen.orientation;
        if (screenOrient == ScreenOrientation.LandscapeLeft)
            enemyRect.Rotate(0, 0, -90);
    }
    
    void Update()
    {
        _Move();
        _CheckBounds();
    }

    private void _Move()
    {
        if (screenOrient == ScreenOrientation.Portrait)
            transform.position += new Vector3(horizontalSpeed * direction * Time.deltaTime, 0.0f, 0.0f);
        else if (screenOrient == ScreenOrientation.LandscapeLeft)            
            transform.position += new Vector3(0, horizontalSpeed * direction * Time.deltaTime, 0.0f);
    }

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
