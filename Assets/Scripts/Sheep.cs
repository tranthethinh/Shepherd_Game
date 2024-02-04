using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Sheep : MonoBehaviour
{
    protected float detectionRange = 1.5f;
    public LayerMask obstacleLayer;
    public Transform dogTransform;

    public float runSpeed = 1f;
    protected Tilemap tilemap;
    protected Vector3Int[,] grid;
    protected AStar astar;
    protected List<Spot> roadPath = new List<Spot>();
    new Camera camera;
    protected BoundsInt bounds;
    protected Animator anim;
    protected Vector3 target;
    protected Rigidbody2D rb;

    protected List<Transform> listScare = new List<Transform>();
    protected List<Transform> listSheepAround = new List<Transform>();

    protected float minGroupDistance = 3f;
    protected float maxGroupDistance = 30f;

    protected bool isFlee = false;
    public bool canScare = true;
    public bool isRunToGroup = false;

    public bool isLeader = false;
    public bool isFollower = false;

    // Handle Random Rest And Move
    protected float timer = 0f;
    protected float actionDuration = 5;

    // Sound
    public AudioClip sheepSound;
    protected AudioSource audioSource;

    public Vector2Int start;
    public Vector2Int cellPosScare;
    public Vector3Int currentTarget;

    protected virtual void Start()
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
        camera = Camera.main;

        CreateGrid();

        astar = new AStar(grid, bounds.size.x, bounds.size.y);

        var state = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(state.fullPathHash, 0, Random.value);
        transform.position += new Vector3(Random.value, Random.value, 0);

        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        Vector3Int SheepCellPos = tilemap.WorldToCell(transform.position);
        //Vector3Int DogCellPos = tilemap.WorldToCell(dogTransform.position);
        start = new Vector2Int(SheepCellPos.x, SheepCellPos.y);
        //dogCellPos = new Vector2Int(DogCellPos.x, DogCellPos.y);
        //Debug.Log(start);

        //float distanceToDog = Vector2.Distance(transform.position, dogTransform.position);
        //Debug.Log(distanceToDog);
        if (roadPath == null || roadPath.Count == 0)
        {
            if (isFlee)
            {
                //CreateGrid();
                Vector3Int cellPos = tilemap.WorldToCell(GetClosestScare(listScare).position);
                cellPosScare = new Vector2Int(cellPos.x, cellPos.y);


                runSpeed = 2f;
                //roadPath = astar.CreatePath(grid, start, cellPosScare,45f, 1000);
                if (cellPosScare != null)
                {
                    CreateRoadPath(cellPosScare);
                }
                if (roadPath == null)
                    return;
                //SetSheepAroundAsFollowers(cellPosScare);
            }
            else if (isRunToGroup)
            {
                UpdateLists();
                CreatePathMoveToGroup();
                runSpeed = 1;
            }
            else if (!isRunToGroup && !isFlee)
            {
                HandleRandomRestAndMove();
            }
        }
        if (isFollower)
        {

            //MoveToTarget(target);
        }
        if (isLeader)
        {

        }
        else
        {
            audioSource.Stop();
        }




        /*if (Input.GetMouseButtonDown(0))
        {
            CreateGrid();
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(world);

            if (roadPath != null && roadPath.Count > 0)
                roadPath.Clear();

            roadPath = astar.CreatePath(grid, start, new Vector2Int(gridPos.x, gridPos.y), 1000);
            if (roadPath == null)
                return;


            
        }
        if (roadPath != null && roadPath.Count > 0)
        {
            MoveAlongPath(roadPath);
            anim.SetBool("IsMoving", true); // Set the IsMoving parameter to true
        }
        else
        {
            anim.SetBool("IsMoving", false); // Set the IsMoving parameter to false when not moving
        }*/




    }
    protected virtual void FixedUpdate()
    {
        Vector3Int SheepCellPos = tilemap.WorldToCell(transform.position);
        //Vector3Int DogCellPos = tilemap.WorldToCell(dogTransform.position);
        start = new Vector2Int(SheepCellPos.x, SheepCellPos.y);
        if (roadPath != null && roadPath.Count > 0)
        {
            MoveAlongPath(roadPath);
            anim.SetBool("IsMoving", true); // Set the IsMoving parameter to true
        }
        else
        {
            anim.SetBool("IsMoving", false); // Set the IsMoving parameter to false when not moving
        }
        UpdateLists();
    }
    void UpdateLists()
    {
        // Remove predators that are out of range
        listScare.RemoveAll(predator => predator == null || Vector2.Distance(transform.position, predator.position) > detectionRange);
        listSheepAround.RemoveAll(predator => predator == null || Vector2.Distance(transform.position, predator.position) > detectionRange);
        CheckAround();
    }


    void SetSheepAroundAsFollowers(Vector3 posScare)
    {
        foreach (Transform sheepTransform in listSheepAround)
        {
            Sheep sheepController = sheepTransform.GetComponent<Sheep>();

            //if (sheepController != null && !sheepController.isLeader && !sheepController.isFollower)
            if (sheepController != null && !sheepController.isFollower && !sheepController.isLeader)
            {
                sheepController.isFollower = true;
                sheepController.target = posScare;

                /*Vector2Int f_cellPosScare= cellPosScare;
                f_cellPosScare += start - sheepController.start;
                sheepController.CreateRoadPath(f_cellPosScare);*/
            }
        }
    }
    void SetSheepAroundAsFollowers(Vector2Int cellPosScare)
    {
        if (astar == null || grid == null)
        {
            Debug.LogError("Astar or grid is not initialized!");
            return;
        }
        foreach (Transform sheepTransform in listSheepAround)
        {
            Sheep sheepController = sheepTransform.GetComponent<Sheep>();


            if (sheepController != null && !sheepController.isFollower && !sheepController.isLeader&&sheepController.canScare)
            {
                sheepController.isFollower = true;


                Vector2Int f_cellPosScare = cellPosScare;
                f_cellPosScare += start - sheepController.start;
                sheepController.CreateRoadPath(f_cellPosScare);
            }
        }
    }
    void CheckAround()
    {
        // Clear elements from listScare
        listScare.Clear();

        // Clear elements from listSheepAround
        listSheepAround.Clear();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, minGroupDistance);

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
                        if (distance < detectionRange)// use this because in the distance that dog make flee is smaller than min minGroupDistance
                            listScare.Add(collider.transform);
                    }
                    else
                    {
                        listScare.Remove(collider.transform);
                    }
                }

            }
            else if (collider.CompareTag("Wolf") && !listScare.Contains(collider.transform))
            {
                WolfController wolfCollider = collider.GetComponent<WolfController>();
                if (wolfCollider != null)
                {

                    if (wolfCollider.isCausingFlee)
                    {
                        float distance = Vector2.Distance(transform.position, wolfCollider.transform.position);
                        if (distance < detectionRange)// use this because in the distance that dog make flee is smaller than min minGroupDistance
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

        //if (listScare.Count > 0 &&!isFollower)
        if (listScare.Count > 0)
        {
            // Flee from the closest dog
            isFlee = true;
            if (roadPath != null && roadPath.Count > 0 && isRunToGroup)
            {
                roadPath.Clear();
                //isRunToGroup= false;
            }
            if (!isFollower)
            {
                isLeader = true;
                if (!audioSource.isPlaying)
                {
                    PlaySheepSound();
                }
            }

        }
        else
        {
            isFlee = false;

            // Check if the sheep is in a group smaller than the threshold
            if (listSheepAround.Count < 4 && !isFlee)
            {
                isRunToGroup = true;
                //MoveToGroup();

            }
            else if (listSheepAround.Count > 4)
            {
                isRunToGroup = false;
            }

            //isLeader= false;
        }


    }
    public void PlaySheepSound()
    {
        // Set the volume based on the AudioManager's sheepVolume
        audioSource.volume = AudioManager.instance.sheepVolume;

        // Your logic to play sheep sounds
        audioSource.Play();
    }
    public void CreateRoadPath(Vector2Int cellPosScare)
    {
        if (astar == null || grid == null)
        {
            Debug.LogError("Astar or grid is not initialized!");
            return;
        }
        if (roadPath != null && roadPath.Count > 0)
        {
            roadPath.Clear();
        }
        roadPath = astar.CreatePath(grid, start, cellPosScare, 79f, 1000);
    }
    protected void CreatePathMoveToGroup()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, maxGroupDistance);

        Vector2 averagePosition = Vector2.zero;
        int sheepCount = 0;
        Collider2D closestSheepCollider = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Sheep") && !listSheepAround.Contains(collider.transform))
            {
                //averagePosition += (Vector2)collider.transform.position;
                sheepCount++;


                float distance = Vector2.Distance(transform.position, collider.transform.position);

                // Check if the current collider is closer than the previously found closest one
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSheepCollider = collider;
                }
            }
        }

        // 'closestSheepCollider' contains the reference to the closest sheep collider
        if (closestSheepCollider != null)
        {
            Vector3Int SheepCellPos = tilemap.WorldToCell(transform.position);
            start = new Vector2Int(SheepCellPos.x, SheepCellPos.y);

            Vector3Int cellPos = tilemap.WorldToCell(closestSheepCollider.transform.position);
            Vector2Int destination = new Vector2Int(cellPos.x, cellPos.y);
            // Move to the average position
            roadPath = astar.CreatePath(grid, start, destination, 1000);
            //Debug.Log(sheepCount + "   from" + start + "to" + destination);
            if (roadPath == null)
            {
                return;
            }
            else
            {
                //Debug.Log(" has new roadmap");
            }
        }


        /*if (sheepCount > 1)
        {
            averagePosition /= sheepCount;

            Vector3Int SheepCellPos = tilemap.WorldToCell(transform.position);
            start = new Vector2Int(SheepCellPos.x, SheepCellPos.y);

            Vector3Int cellPos = tilemap.WorldToCell(averagePosition);
            Vector2Int destination = new Vector2Int(cellPos.x, cellPos.y);
            // Move to the average position
            roadPath = astar.CreatePath(grid, start, destination, 1000);
            Debug.Log(sheepCount+"   from"+start +"to" +destination);
            if (roadPath == null)
            {
                return;
            }
            else
            {
                Debug.Log(" has new roadmap");
            }
            
        }*/
    }
    protected virtual Transform GetClosestScare(List<Transform> listScare)
    {
        Transform closestScare = null;
        float closestDistance = float.MaxValue;

        foreach (Transform scare in listScare)
        {
            float distance = Vector2.Distance(transform.position, scare.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestScare = scare;
            }
        }

        return closestScare;
    }
    private void MoveToTarget(Vector3 _target)
    {
        // Calculate the distance between the current position and the target position
        float distanceToTarget = Vector3.Distance(transform.position, _target);
        Debug.Log(distanceToTarget);
        // Check if the object is close enough to the target
        if (distanceToTarget > 0.5f) // You can adjust this threshold based on your needs
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, _target, runSpeed * Time.deltaTime);
        }
        else
        {
            isFollower = false;
        }


    }
    private void MoveAlongPath(List<Spot> _roadPath)
    {
        if (_roadPath.Count > 0)
        {
            // Get the target position using Vector3Int coordinates
            Vector3 targetPosition = tilemap.GetCellCenterWorld(new Vector3Int(_roadPath[0].X, _roadPath[0].Y, 0));
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            // Move towards the target position using Vector3.MoveTowards
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);
            //MoveToTarget(targetPosition);
            /*Vector3 followerTarget = targetPosition;
            if (_roadPath.Count > 3)
            {
                followerTarget = tilemap.GetCellCenterWorld(new Vector3Int(_roadPath[3].X, _roadPath[3].Y, 0));
            }
            if (isLeader)
            {
                CheckAround();
                //SetSheepAroundAsFollowers(followerTarget);
                SetSheepAroundAsFollowers(cellPosScare);
            }*/
            //rb.velocity = new Vector2(moveDirection.x * runSpeed, moveDirection.y * runSpeed);
            /*if (moveDirection.magnitude < 0.1f)
            {
                rb.velocity *= 0f;// make player slow down if has no force
            }
            else
            {

                moveDirection.Normalize(); // Normalize to ensure consistent speed in all directions
                                      //Debug.Log(movement);


                // Apply the force to the Rigidbody
                rb.AddForce(moveDirection * runSpeed);
                // Limit the player's maximum speed
                if (rb.velocity.magnitude > runSpeed)
                {
                    rb.velocity = rb.velocity.normalized * runSpeed;
                }
            }*/
            if (moveDirection.x > 0.4)
            {
                transform.localScale = new Vector3(-2, 2, 1);
            }
            else
            {
                transform.localScale = new Vector3(2, 2, 1);
            }

            // Check if the sheep has reached the current target point
            if (Vector3.Distance(transform.position, targetPosition) < 0.4f)
            {
                // Remove the reached point from the path
                _roadPath.RemoveAt(0);

                if (isLeader && cellPosScare != null)
                {
                    UpdateLists();
                    SetSheepAroundAsFollowers(cellPosScare);
                }
            }
        }

        if (_roadPath.Count == 0)
        {
            rb.velocity = Vector2.zero; // Stop the rigidbody when the path is empty
            anim.SetBool("IsMoving", false); // Set the IsMoving parameter to false when the path is empty
            isFollower = false;
            isLeader = false;
        }
        else
        {
            anim.SetBool("IsMoving", true); // Set the IsMoving parameter to true
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
    protected void HandleRandomRestAndMove()
    {
        // If the timer exceeds the actionDuration, switch between resting and moving
        if (timer >= actionDuration)
        {

            GenerateNewActionDuration();
            GenerateNewTarget();
        }
        else
        {
            // The sheep is moving towards the target position
            if (Vector3.Distance(transform.position, target) > 0.1f && timer < 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, runSpeed * Time.deltaTime);

                // Calculate the move direction
                Vector3 moveDirection = target - transform.position;

                // Check the absolute value of moveDirection.x
                if (Mathf.Abs(moveDirection.x) > 0.1f)
                {
                    // Flip the sheep's scale based on the movement direction along the x-axis
                    transform.localScale = new Vector3(-Mathf.Sign(moveDirection.x) * 2, 2, 1);
                }
            }

        }

        // Increment the timer
        timer += Time.deltaTime;
    }

    void GenerateNewActionDuration()
    {
        // Generate a new random duration for resting or moving
        actionDuration = Random.Range(2f, 11f); // Adjust the range as needed

        // Reset the timer
        timer = 0f;
    }

    void GenerateNewTarget()
    {
        // Generate a random target position around the sheep's current position
        float randomX = transform.position.x + Random.Range(-1f, 1f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);

        // Set the random target position
        target = new Vector3(randomX, randomY, 0f);
    }
}
