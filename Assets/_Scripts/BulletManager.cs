using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * File: BulletManager.cs
 * Name: Cameron Geraats
 * ID: 100806837
 * Last Modified: 24/10/20
 * Description:
 *      Manages the bullet creation call, pooling of created bullets, and release/return of bullets to queue
 * Revisions: No previous revisions
 */
[System.Serializable]
public class BulletManager : MonoBehaviour
{
    public BulletFactory bulletFactory;
    public int MaxBullets;

    private Queue<GameObject> m_bulletPool;


    // Start is called before the first frame update
    // Sets the bullet pool up, by creating MaxBullet number of bullets
    void Start()
    {
        _BuildBulletPool();
    }

    private void _BuildBulletPool()
    {
        // create empty Queue structure
        m_bulletPool = new Queue<GameObject>();

        for (int count = 0; count < MaxBullets; count++)
        {
            var tempBullet = bulletFactory.createBullet();
            m_bulletPool.Enqueue(tempBullet);
        }
    }

    public GameObject GetBullet(Vector3 position)
    {
        var newBullet = m_bulletPool.Dequeue();
        newBullet.SetActive(true);
        newBullet.transform.position = position;
        return newBullet;
    }

    public bool HasBullets()
    {
        return m_bulletPool.Count > 0;
    }

    public void ReturnBullet(GameObject returnedBullet)
    {
        returnedBullet.SetActive(false);
        m_bulletPool.Enqueue(returnedBullet);
    }
}
