using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
//check sheep move to group and run to it and eat it
// run away from dog
public class WolfController : MonoBehaviour
{

    private bool isRun;
    private bool canEat = true;
    private float speed=1;
    private int detectionRange=100;
    private int circleRadius = 5;// wolf move around sheep
    private int circleSpeed = 15;//speed change degree
    private int minDetectionRange = 3;
    public bool isCausingFlee=true;
    private bool isMoveAroundSheep;
    private Tilemap tilemap;
    public Vector3Int[,] grid;
    AStar astar;
    List<Spot> roadPath = new List<Spot>();
    BoundsInt bounds;
    public GameObject blood;
    private Animator anim;

    private Rigidbody2D rb;

    List<Transform> listScare = new List<Transform>();
    List<Transform> listSheepAround = new List<Transform>();

    private float currentAngle;
    private Transform targetSheep;

    private Vector2Int start;

    private void Awake()
    {
        
        
    }
    void Start()
    {
        if (tilemap == null)
        {
            // Find the "walkable" tilemap in the scene
            tilemap = GameObject.FindWithTag("Walkable").GetComponent<Tilemap>();
        }
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tilemap.CompressBounds();

        bounds = tilemap.cellBounds;
        CreateGrid();
        astar = new AStar(grid, bounds.size.x, bounds.size.y);

        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (listScare.Count>0)
        {
            if (roadPath == null || roadPath.Count == 0||isMoveAroundSheep)
            {
                isMoveAroundSheep = false;
                Transform scarepos = GetClosest(listScare);
                Vector3Int cellPos = tilemap.WorldToCell(scarepos.position);
                Vector2Int cellPosScare = new Vector2Int(cellPos.x, cellPos.y);
                if (roadPath != null && roadPath.Count > 0)
                {
                    roadPath.Clear();
                }

                
                //roadPath = astar.CreatePath(grid, start, cellPosScare,45f, 1000);
                CreateRoadPath(cellPosScare);
                if (roadPath == null)
                    return;
            }
            speed = 2f;
            anim.SetBool("IsRun", true);
        }
        else if (listSheepAround.Count>0)
        {
            Transform _targetSheep = GetClosest(listSheepAround);
            if (_targetSheep != targetSheep || targetSheep == null)
            {
                targetSheep = _targetSheep;
                if (targetSheep != null)
                {
                    Sheep sheepEat = targetSheep.GetComponent<Sheep>();
                    if (sheepEat.isRunToGroup && canEat)
                    {
                        Vector3Int SheepCellPos = tilemap.WorldToCell(sheepEat.transform.position);
                        Vector2Int target = new Vector2Int(SheepCellPos.x, SheepCellPos.y);
                        
                        speed = 3;
                        anim.SetBool("IsRun", true);
                        roadPath = astar.CreatePath(grid, start, target, 1000);
                    }
                    else { 
                        speed = 1;
                        anim.SetBool("IsRun", false);
                    }

                    Vector3 directionToSheep = transform.position- targetSheep.position;
                    currentAngle = Mathf.Atan2(directionToSheep.x, directionToSheep.y) * Mathf.Rad2Deg;
                }
                //MoveAroundSheep(targetSheep, currentAngle);
            }
            if (roadPath == null || roadPath.Count == 0)
            {
                MoveAroundSheep(targetSheep, currentAngle);
                currentAngle += 20;
                isMoveAroundSheep = true;
            }
        }else
        {
            speed = 1;
            anim.SetBool("IsRun", false);
            isMoveAroundSheep = false;
        }
    }
    private void FixedUpdate()
    {
        
        Vector3Int wolfCellPos = tilemap.WorldToCell(transform.position);
        start = new Vector2Int(wolfCellPos.x, wolfCellPos.y);

        if (roadPath != null && roadPath.Count > 0)
        {
            MoveAlongPath(roadPath);
            //Debug.Log("ok");
        }
        else
        {
            anim.SetBool("IsRun", false); // Set the IsRun 
        }
        UpdateLists();
    }
    private void MoveAlongPath(List<Spot> _roadPath)
    {
        if (_roadPath.Count > 0)
        {
            // Get the target position using Vector3Int coordinates
            Vector3 targetPosition = tilemap.GetCellCenterWorld(new Vector3Int(_roadPath[0].X, _roadPath[0].Y, 0));
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            // Move towards the target position using Vector3.MoveTowards
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            
            if (moveDirection.x > 0.4)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            // Check if the sheep has reached the current target point
            if (Vector3.Distance(transform.position, targetPosition) < 0.4f)
            {
                // Remove the reached point from the path
                _roadPath.RemoveAt(0);

                
            }
        }

        if (_roadPath.Count == 0)
        {
            rb.velocity = Vector2.zero; // Stop the rigidbody when the path is empty
            anim.SetBool("IsRun", false); // Set the IsMoving parameter to false when the path is empty
            
        }
        else
        {
            
        }
    }

    public void CreateGrid()
    {
        grid = new Vector3Int[bounds.size.x, bounds.size.y];
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
            {
                if (tilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    grid[i, j] = new Vector3Int(x, y, 0);
                }
                else
                {
                    grid[i, j] = new Vector3Int(x, y, 1);
                }
            }
        }
    }
    void UpdateLists()
    {
        CheckAround();
        // Remove predators that are out of range
        listScare.RemoveAll(predator => predator == null || Vector2.Distance(transform.position, predator.position) > minDetectionRange);
        listSheepAround.RemoveAll(predator => predator == null || Vector2.Distance(transform.position, predator.position) > detectionRange);
        
    }
    void CheckAround()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Dog") && !listScare.Contains(collider.transform))
            {
                PlayerController dogCollider = collider.GetComponent<PlayerController>();
                if (dogCollider != null)
                {

                    if (dogCollider.isCausingFlee)
                    {
                        float distance = Vector2.Distance(transform.position, dogCollider.transform.position);
                        if (distance < minDetectionRange)
                            listScare.Add(collider.transform);
                    }
                    else
                    {
                        listScare.Remove(collider.transform);
                    }
                }

            }
            else if (collider.CompareTag("Sheep") && !listSheepAround.Contains(collider.transform))
            {
                listSheepAround.Add(collider.transform);
            }

        }


    }
    Transform GetClosest(List<Transform> listAround)
    {
        Transform closest = null;
        float closestDistance = float.MaxValue;

        foreach (Transform obj in listAround)
        {
            if(obj ==null) continue;
            float distance = Vector2.Distance(transform.position, obj.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = obj;
            }

            
        }

        return closest;
    }

    void MoveAroundSheep(Transform sheep, float currentAngle)
    {
        if (sheep != null)
        {
            
            // Calculate a position in a circle around the sheep
            float x = sheep.position.x + circleRadius * Mathf.Cos(currentAngle);
            float y = sheep.position.y + circleRadius * Mathf.Sin(currentAngle);

            Vector3 destination = new Vector3(x,y, sheep.position.z);
            Vector3Int cellPos = tilemap.WorldToCell(destination);
            Vector2Int destinationCellPos = new Vector2Int(cellPos.x, cellPos.y);
            // Set the destination for the wolf
            Vector3Int wolfCellPos = tilemap.WorldToCell(transform.position);
            start = new Vector2Int(wolfCellPos.x, wolfCellPos.y);
            roadPath = astar.CreatePath(grid, start, destinationCellPos, 1000);
            /*Debug.Log(start);
            Debug.Log(destinationCellPos);
            Debug.Log(roadPath);*/
            if (roadPath != null && roadPath.Count > 0) { 
                //Debug.Log("ok roadpath");
             }
             // Update the angle for the next frame
            currentAngle += Time.deltaTime * circleSpeed;

            // Keep the angle within 0 to 360 degrees
            if (currentAngle > 360f)
            {
                currentAngle -= 360f;
            }
        }
    }
    public void CreateRoadPath(Vector2Int cellPosScare)
    {
        if (roadPath != null && roadPath.Count > 0)
        {
            roadPath.Clear();
        }
        roadPath = astar.CreatePath(grid, start, cellPosScare, 179f, 1000);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision != null && canEat)
        {
            
            if (collision.gameObject.CompareTag("Sheep"))
            {
                
                //SheepController sheep = collision.gameObject.GetComponent<SheepController>();
                if (collision.gameObject.GetComponent<Sheep>().isRunToGroup)
                {
                    
                    listSheepAround.Remove(collision.transform);
                    Destroy(collision.gameObject);
                    Instantiate(blood, transform.position, Quaternion.identity);
                    canEat = false;
                    Invoke("SetCanEat", 30);
                }
            }
        }
    }
    void SetCanEat()
    {
        canEat = true;
    }
}
