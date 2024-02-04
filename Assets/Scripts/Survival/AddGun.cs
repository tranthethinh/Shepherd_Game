using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGun : MonoBehaviour
{
    private int maxGunCount = 6; // Maximum number of B objects around A

    private int currentGunCount = 0;
    private Vector2[] childPositions = { new Vector2(-0.5f, 0.3f), new Vector2(-0.5f, 0f), new Vector2(-0.5f, -0.3f),
                                        new Vector2(0.5f, 0.3f), new Vector2(0.5f, 0f), new Vector2(0.5f, -0.3f) };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gun" && currentGunCount < maxGunCount)
        {
            var gun = collision.GetComponent<GunController>();
            if (!gun.isChild)
            {
                gun.isChild = true;
                gun.canShoot = true;
                //Debug.Log("collision with gun");
                collision.transform.parent = transform;


                if (currentGunCount < childPositions.Length)
                {
                    collision.transform.localPosition = childPositions[currentGunCount];

                }


                currentGunCount++;
            }
        }
    }
}
