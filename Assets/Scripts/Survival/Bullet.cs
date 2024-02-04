using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;  // Speed of the bullet
    public float lifeTime = 2f;  // Time until the bullet is destroyed
    private Transform target;  // Target for the bullet
    public void Seek(Transform _target)
    {
        target = _target;
    }
    void Start()
    {

        
            Destroy(gameObject, lifeTime);
        

    }
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        else
        {
           
            Vector3 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sheep"))
        {

            Destroy(gameObject);
            MoveToDog sheep = collision.GetComponent<MoveToDog>();
            if (sheep != null)
            {
                if (sheep.currentHp < 3)
                {
                    sheep.getFood();
                }
                
            }
            
        }
    }
    
}
