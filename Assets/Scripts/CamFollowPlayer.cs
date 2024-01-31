using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    public Transform target; // The target to follow (player)
    public float smoothSpeed = 0.25f; // The smoothness of the camera movement
    public Vector3 offset;
    private Vector3 velocity =  Vector3.zero;
    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Dog").GetComponent<Transform>();
        }
    }
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position+offset;
            //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;

        }
    }
}
