using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject cam;
    [NonSerialized]
    public Rigidbody rb;
    [NonSerialized] public Vector3 lastCheckpoint;

    public float speed = 1;
    public float airSpeed = 0.5f;
    public float friction = 0.2f; // amount to reduce speed by each frame; closer to 0 is slipperier but greater than 1 will INCREASE speed instead
    public float airFriction = 0.1f; // amount to reduce speed by when in the air
    [NonSerialized]
    public Vector3 vel;

    public float jumpStrength = 1;
    public float gravity = 1;
    [NonSerialized]
    public bool grounded;

    [NonSerialized]
    public bool onHook; // if the player is hooked onto a zipline or not


    private Vector2 pInput; // player movement input
    //private bool jumpInput;
    private float jumpBuffer = 0.15f; // jump buffering
    private float jumpBufferTime = 0;
    private float coyoteTime = 0.25f;
    private float coyoteTimer = 0;

    public float lookSensitivity = 3;
    private Vector2 rotation = Vector2.zero;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = (1f / 60f);
        rb = GetComponent<Rigidbody>(); // get attached rigidbody (we'll be using a capsule collider for 3D movement
        vel = new Vector3(); // reset velocity

        rotation.y = transform.rotation.y;
        lastCheckpoint = transform.position; // set last checkpoint to spawn position

#if UNITY_EDITOR
        lookSensitivity *= 1;
#else
        lookSensitivity *= 1;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        doLook();

        jumpBufferTime = Mathf.Max(0, jumpBufferTime - Time.deltaTime);
        

        pInput.x = Input.GetAxisRaw("Horizontal"); // we use two separate operations here as it's slightly
        pInput.y = Input.GetAxisRaw("Vertical");   // more efficient than creating a new Vector2 every frame
        if (Input.GetButtonDown("Jump")) { jumpBufferTime = jumpBuffer; } // queue this up in update to ensure responsiveness

        if (Input.GetKey("escape")) // quit game
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        
        if (grounded)
        {
            vel += Vector3.Normalize(transform.rotation * new Vector3(pInput.x, 0, pInput.y)) * speed * Time.deltaTime; // adjust player velocity, normalize increase to ensure consistency
            coyoteTimer = coyoteTime;
        } else
        {
            vel += Vector3.Normalize(transform.rotation * new Vector3(pInput.x, 0, pInput.y)) * airSpeed * Time.deltaTime; // lower aerial acceleration
            coyoteTimer = Mathf.Max(0, coyoteTimer - Time.fixedDeltaTime);
        }


        if (!onHook) { vel += new Vector3(0, -gravity, 0) * Time.deltaTime; } // apply gravity
        RaycastHit rh;
        bool hit = Physics.SphereCast(transform.position + new Vector3(0, 1, 0), 0.15f, Vector3.down, out rh, transform.localScale.y);
        if ((hit && !(rh.transform.gameObject.CompareTag("TutorialArea") || rh.transform.gameObject.CompareTag("checkpoint"))) && vel.y <= 0) { vel.y = 0; grounded = true; } else { grounded = false; } // ground detection - FIXED mistaken tutorial area detection

        if (jumpBufferTime > 0 && (grounded || coyoteTimer > 0))
        {
            jumpBufferTime = 0;
            coyoteTimer = 0;
            grounded = false;
            vel.y += jumpStrength;
        }

        if (grounded)
        {
            vel = new Vector3(vel.x * (1 - friction), vel.y, vel.z * (1 - friction)); //reduce horizontal velocity based on specified friction
        } else
        {
            vel = new Vector3(vel.x * (1 - airFriction), vel.y, vel.z * (1 - airFriction)); //accelerate more slowly in the air but have lower friction
        }

        rb.linearVelocity = vel; //update player velocity
        if (transform.position.y <= -13 || Input.GetKey("r"))
        {
            transform.position = lastCheckpoint;
            rb.linearVelocity = Vector3.zero;
            vel = Vector3.zero;
        }
    }

    public void doLook()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y") * lookSensitivity;
        rotation.x = Mathf.Clamp(rotation.x, -80f, 80f);
        transform.eulerAngles = new Vector2(0, rotation.y) * lookSensitivity;
        cam.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("checkpoint"))
        {
            lastCheckpoint = other.transform.position + other.GetComponent<Checkpoint>().respawnOffset;
        }
    }
}
