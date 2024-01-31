using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private Animator anim;
    public float maxSpeed = 1;
    public bool isCausingFlee;// to make the sheep runaway or not
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetBool("isSmallWalk", !anim.GetBool("isSmallWalk"));
            
        }
        if (anim.GetBool("isSmallWalk"))
        {
            maxSpeed = 0.5f;
            
        }
        else
        {
            maxSpeed = 2;
        }
        if(anim.GetBool("isSmallWalk")|| rb.velocity.magnitude < 0.1f)
        {
            isCausingFlee = false;
        }
        else
        {
            isCausingFlee = true;
        }
        

    }
    private void FixedUpdate()
    {
        PlayerControl();
        
        // Calculate the player's speed

        float playerSpeed = rb.velocity.magnitude;
        //Debug.Log(playerSpeed);
        // Update the animator parameter based on the player's speed
        anim.SetFloat("speed", playerSpeed);

        
    }
    void PlayerControl()
    {
        // Get player input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //Debug.Log(horizontalInput +verticalInput);
        //anim.SetFloat("inputY", rb.velocity.y);
        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);
        if (movement.magnitude < 0.1f)
        {
            rb.velocity *= 0f;// make player slow down if has no force
        }
        else
        {
           
            movement.Normalize(); // Normalize to ensure consistent speed in all directions
            //Debug.Log(movement);
            

            // Apply the force to the Rigidbody
            rb.AddForce(movement * maxSpeed);
            // Limit the player's maximum speed
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }

        // Check the vertical movement
        float absX = Mathf.Abs(horizontalInput);
        float absY = Mathf.Abs(verticalInput);

        anim.SetBool("isRunHorizontal", absX > absY);
        anim.SetBool("isRunUp", !anim.GetBool("isRunHorizontal") && verticalInput > 0.1);
        anim.SetBool("isRunDown", !anim.GetBool("isRunHorizontal") && verticalInput < -0.1);



        // Rotate the player based on the horizontal input
        if (horizontalInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
