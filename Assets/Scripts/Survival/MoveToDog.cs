using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveToDog : Sheep
{
    public GameObject dog;
    public GameObject gun;
    public Slider healthBar;
    public int currentHp = 0;
    GameManagerS gameManager;
    protected override void Start()
    {
        base.Start();
        canScare = false;

        dog = GameObject.FindWithTag("Dog");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerS>();
    }
    protected override void Update()
    {
       

        if (roadPath == null || roadPath.Count == 0)
        {
            
                if (dog != null)
                {
                    Vector3Int cellPos = tilemap.WorldToCell(dog.transform.position);
                    Vector2Int cellPosMoveTo = new Vector2Int(cellPos.x, cellPos.y);


                    //runSpeed = 2f;
                    //roadPath = astar.CreatePath(grid, start, cellPosScare,45f, 1000);
                    if (cellPosMoveTo != null)
                    {
                        roadPath = astar.CreatePath(grid, start, cellPosMoveTo, 1000);
                    }
                    if (roadPath == null)
                        return;
                }
            

        }
    }
    void UpdateHealthBar()
    {
        
        healthBar.value +=1;
    }
    public void getFood()
    {
        UpdateHealthBar();
        currentHp += 1;
        if(currentHp >=3)
        {
            if(gameManager != null)
            {
                gameManager.IncrementSheepSaved();
            }
            Destroy(gameObject, 2f);
            runSpeed = 3;
            Vector3Int cellPos = tilemap.WorldToCell(dog.transform.position);
            cellPosScare = new Vector2Int(cellPos.x, cellPos.y);
            CreateRoadPath(cellPosScare);
            if (Random.Range(0, 100) < 28)
            {
                Instantiate(gun,transform.position, Quaternion.identity);
            }
        }
    }
}
