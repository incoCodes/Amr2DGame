using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 3.5f;
    [SerializeField] Vector2 jumpHeight = new Vector2(0f, 5f);
    [SerializeField] int jumpCount = 0;
    [SerializeField] int extraJumps = 2;
    [SerializeField] float teleDistance = 10f;
    [SerializeField] float timeSinceLastTele = Mathf.Infinity;
    [SerializeField] float teleTimer = 2f;

    public Transform groundCheckPoint;
    public LayerMask whatIsGround;
    private bool isGrounded;
    public Transform wallGrabPoint;
    private bool canGrab, isGrabbing;
    private float gravityStore;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityStore = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {


        Movement();
        timeSinceLastTele += Time.deltaTime;
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, whatIsGround);
        canGrab = Physics2D.OverlapCircle(wallGrabPoint.position, 0.2f, whatIsGround);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            jumpCount++;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) &&  timeSinceLastTele > teleTimer)
        {
            Teleport();
            timeSinceLastTele = 0;
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-1f, 1, 1f);
        }

        WallJumping();




    }

    private void Jump()
    {
        
          if (isGrounded || jumpCount < extraJumps) {
            
            rb.AddForce(jumpHeight, ForceMode2D.Impulse);
          }
        
      
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            jumpCount = 0;
        }
    }
    

    private void Movement()
    {
        float horiInput = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(horiInput, Quaternion.identity.y, 0);
        transform.Translate(direction * speed * Time.deltaTime);

     
    }

    private void Teleport()
    {
        float horiInput = Input.GetAxis("Horizontal");

        if (horiInput == 1)
        {
            transform.position = new Vector2(transform.position.x + teleDistance, transform.position.y);

        } 
        else if (horiInput == -1)
        {
            transform.position = new Vector2(transform.position.x - teleDistance, transform.position.y);
        }
    }

   private void WallJumping()
    {
        isGrabbing = false;
        if (canGrab && !isGrounded)
        {
            if ((transform.localScale.x == 1f && Input.GetAxisRaw("Horizontal") > 0) || ((transform.localScale.x == -1f && Input.GetAxisRaw("Horizontal") < 0)))
            {
                isGrabbing = true;
            }
        }

        if (isGrabbing == true)
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.gravityScale = gravityStore;
        }
    }
}
