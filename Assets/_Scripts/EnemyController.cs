using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float horizontalSpeed;
    public float horizontalBoundary;
    public float direction;

    private RectTransform enemyRect;

    void Start()
    {
        GameController.OrientationChange.AddListener(_OrientationChange);
        enemyRect = GetComponentInParent<RectTransform>();
    }
    
    void Update()
    {
        _Move();
        _CheckBounds();
    }

    private void _Move()
    {
        transform.position += new Vector3(horizontalSpeed * direction * Time.deltaTime, 0.0f, 0.0f);
    }

    private void _CheckBounds()
    {
        // check right boundary
        if (enemyRect.anchoredPosition.x >= horizontalBoundary)
        {
            direction = -1.0f;
        }

        // check left boundary
        if (enemyRect.anchoredPosition.x <= -horizontalBoundary)
        {
            direction = 1.0f;
        }
    }
    private void _OrientationChange(ScreenOrientation scrOri)
    {

    }
}
