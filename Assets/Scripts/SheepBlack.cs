using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SheepBlack : Sheep
{
    private bool canHit = false;
    private Transform scarePos;
    protected override void Update()
    {

        Vector3Int SheepCellPos = tilemap.WorldToCell(transform.position);
        
        start = new Vector2Int(SheepCellPos.x, SheepCellPos.y);
        
        if (roadPath == null || roadPath.Count == 0)
        {
            if (isFlee)
            {
                scarePos = GetClosestScare(listScare);
                Vector3Int cellPos = tilemap.WorldToCell(scarePos.position);
                cellPosScare = new Vector2Int(cellPos.x, cellPos.y);
                runSpeed = 2f;
                if (Random.Range(0, 100) < 50)
                {
                    if (cellPosScare != null)
                    {
                        CreateRoadPath(cellPosScare);
                    }
                    if (roadPath == null)
                        return;
                }
                else
                {
                    runSpeed = 4;
                    canHit = true;
                    roadPath = astar.CreatePath(grid, start, cellPosScare, 1000);
                }
            }
            else if (isRunToGroup)
            {
                
                CreatePathMoveToGroup();
                runSpeed = 1;
            }
            else if (!isRunToGroup && !isFlee)
            {
                HandleRandomRestAndMove();
            }

        }
       
        /*if (canHit)
        {
            if (Vector3.Distance(transform.position, scarePos.position) < 0.4f)
            {
                canHit=false;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, minGroupDistance);

                foreach (Collider2D collider in colliders)
                {

                    if (collider.CompareTag("Dog"))
                    {
                        Rigidbody rb = collider.GetComponent<Rigidbody>();


                        if (rb != null)
                        {

                            Vector3 forceDirection = (collider.transform.position - transform.position).normalized;


                            float forceMagnitude = 1000f;
                            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
                        }
                    }
                }
            }
        }*/
    }
}
