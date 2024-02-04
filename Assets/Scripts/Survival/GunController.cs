using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GunController : MonoBehaviour
{
    private List<GameObject> collidingSheep = new List<GameObject>();
    public GameObject bulletPrefab;
    //private bool canShoot = true;
    private GameObject target;

    public static float fireRate = 0.5f;
    private float fireCooldown = 0;
    public bool canShoot = false;
    public bool isChild = false;
    private void Update()
    {
        target = FindClosestSheep();
        if (fireCooldown <= 0f && target != null && canShoot)
        {
            Shoot();
            fireCooldown = 1f / fireRate; // Reset the cooldown timer
        }

        // Decrease the cooldown timer
        fireCooldown -= Time.deltaTime;
    }
    private GameObject FindClosestSheep()
    {
        if (collidingSheep.Count == 0)
        {
            return null; // No sheep in the list
        }

        GameObject closestSheep = collidingSheep[0];
        float closestDistance = Vector3.Distance(transform.position, closestSheep.transform.position);

        foreach (GameObject sheep in collidingSheep)
        {
            if(sheep == null)
            {
                collidingSheep.Remove(sheep);
                continue;
            }
            float distance = Vector3.Distance(transform.position, sheep.transform.position);

            if (distance < closestDistance)
            {
                closestSheep = sheep;
                closestDistance = distance;
            }
        }

        return closestSheep;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Sheep") && !collidingSheep.Contains(other.gameObject))
        {
            
            collidingSheep.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Remove the colliding object from the list when it exits the trigger
        collidingSheep.Remove(other.gameObject);
    }
    void Shoot()
    {
        
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        
        if (bullet != null)
            bullet.Seek(target.transform);
    }
}
