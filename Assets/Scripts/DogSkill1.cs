using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DogSkill1 : MonoBehaviour
{
    public float radius = 5f;
    public float forceStrength = 10f;
    public GameObject circleVisual;
    private Tilemap tilemap;
    private AudioSource audioSource;
    void Start()
    {
        if (tilemap == null)
        {
            // Find the "walkable" tilemap in the scene
            tilemap = GameObject.FindWithTag("Walkable").GetComponent<Tilemap>();
        }
        if (circleVisual != null)
        {
            // Disable the circle at the start
            circleVisual.SetActive(false);
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ApplyForceInCircle();
        }
    }

    void ApplyForceInCircle()
    {
        if (circleVisual != null)
        {
            // Enable the circle when the function is called
            circleVisual.SetActive(true);
            Invoke("DisableCircle", 0.2f); // Disable the circle after 0.5 seconds
        }
        Vector3Int cellPos = tilemap.WorldToCell(transform.position);
        Vector2Int _cellPos = new Vector2Int(cellPos.x, cellPos.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Sheep")){
                var controller = collider.GetComponent<Sheep>();
                if (controller != null&&controller.canScare)
                {

                    controller.CreateRoadPath(_cellPos);
                }
            }
           
            else if (collider.CompareTag("Wolf"))
            {
                var _controller = collider.GetComponent<WolfController>();
                if (_controller != null)
                {

                    _controller.CreateRoadPath(_cellPos);
                }
            }
            if (!audioSource.isPlaying)
            {
                PlaySound();
            }

        }
    }
    public void PlaySound()
    {
        // Set the volume based on the AudioManager's sheepVolume
        if (AudioManager.instance)
        {
            audioSource.volume = AudioManager.instance.sheepVolume;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
    void DisableCircle()
    {
        if (circleVisual != null)
        {
            // Disable the circle after a delay
            circleVisual.SetActive(false);
        }
    }

    /*private void OnDrawGizmos()
    {
        // Draw the circle in the Scene view for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }*/
}
