using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepFollowDog : Sheep
{
    protected override void Start()
    {
        base.Start();
        canScare = false;
    }
    protected override void Update()
    {
        /*isFlee = false;
        isRunToGroup = false;
        isLeader = false;
        base.Update();*/

        if (roadPath == null || roadPath.Count == 0)
        {
            if  (listScare.Count > 0 && listScare!=null)
            {
                Transform scarePos = GetClosestScare(listScare);
                if (scarePos != null)
                {
                    Vector3Int cellPos = tilemap.WorldToCell(scarePos.position);
                    Vector2Int cellPosFollow = new Vector2Int(cellPos.x, cellPos.y);


                    runSpeed = 2f;
                    //roadPath = astar.CreatePath(grid, start, cellPosScare,45f, 1000);
                    if (cellPosFollow != null)
                    {
                        roadPath = astar.CreatePath(grid, start, cellPosFollow, 1000);
                    }
                    if (roadPath == null)
                        return;
                }
            }

        }
    }
    protected override Transform GetClosestScare(List<Transform> listScare)
    {
        Transform closestDog = null;
        float closestDistance = float.MaxValue;

        foreach (Transform scare in listScare)
        {
            // Check if the current object has the tag "Dog"
            if (scare.CompareTag("Dog"))
            {
                float distance = Vector2.Distance(transform.position, scare.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDog = scare;
                }
            }
        }

        return closestDog;
    }
}
