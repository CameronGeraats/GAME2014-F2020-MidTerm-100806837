using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IApplyDamage
{
    public float verticalSpeed;
    public float verticalBoundary;
    public BulletManager bulletManager;
    public int damage;

    private ScreenOrientation screenOrient;
    private RectTransform bulletTransf;

    // Start is called before the first frame update
    void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();
        bulletTransf = GetComponentInParent<RectTransform>();
        GameController.OrientationChange.AddListener(_OrientationChange);
        screenOrient = Screen.orientation;
        if (screenOrient == ScreenOrientation.LandscapeLeft)
            bulletTransf.Rotate(0, 0, -90);
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _CheckBounds();
    }

    private void _Move()
    {
        if (screenOrient == ScreenOrientation.Portrait)
            transform.position += new Vector3(0.0f, verticalSpeed, 0) * Time.deltaTime;
        else if (screenOrient == ScreenOrientation.LandscapeLeft)
            transform.position += new Vector3(verticalSpeed, 0, 0) * Time.deltaTime;        
    }

    private void _CheckBounds()
    {
        if (screenOrient == ScreenOrientation.Portrait && bulletTransf.anchoredPosition.y > verticalBoundary)
        {
            bulletManager.ReturnBullet(gameObject);
        }
        if (screenOrient == ScreenOrientation.LandscapeLeft && bulletTransf.anchoredPosition.x > verticalBoundary)
        {
            bulletManager.ReturnBullet(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name);
        bulletManager.ReturnBullet(gameObject);
    }

    public int ApplyDamage()
    {
        return damage;
    }
    private void _OrientationChange(ScreenOrientation scrOri)
    {
        screenOrient = scrOri;
        if (scrOri == ScreenOrientation.LandscapeLeft)
        {
            bulletTransf.sizeDelta = new Vector2(bulletTransf.rect.height,bulletTransf.rect.width);
            bulletTransf.Rotate(0, 0, -90);
            bulletTransf.pivot = new Vector2(bulletTransf.pivot.y, bulletTransf.pivot.x);
            bulletTransf.anchorMax = new Vector2(bulletTransf.anchorMax.y, bulletTransf.anchorMax.x);
            bulletTransf.anchorMin = new Vector2(bulletTransf.anchorMin.y, bulletTransf.anchorMin.x);
            bulletTransf.anchoredPosition = new Vector3(bulletTransf.anchoredPosition.y, bulletTransf.anchoredPosition.x, 0);
        }
        if (scrOri == ScreenOrientation.Portrait)
        {
            bulletTransf.sizeDelta = new Vector2(bulletTransf.rect.height, bulletTransf.rect.width);
            bulletTransf.Rotate(0, 0, 90);
            bulletTransf.pivot = new Vector2(bulletTransf.pivot.y, bulletTransf.pivot.x);
            bulletTransf.anchorMax = new Vector2(bulletTransf.anchorMax.y, bulletTransf.anchorMax.x);
            bulletTransf.anchorMin = new Vector2(bulletTransf.anchorMin.y, bulletTransf.anchorMin.x);
            bulletTransf.anchoredPosition = new Vector3(bulletTransf.anchoredPosition.y, bulletTransf.anchoredPosition.x, 0);
        }
    }
}
