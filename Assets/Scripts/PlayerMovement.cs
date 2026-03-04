using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject cam;
    [NonSerialized]
    public Rigidbody rb;

    public float speed = 1;
    public float friction = 0.2f; // amount to reduce speed by each frame; closer to 0 is slipperier but greater than 1 will INCREASE speed instead
    [NonSerialized]
    public Vector3 vel;

    public float jumpStrength = 1;
    public float gravity = 1;
    private bool grounded;


    private Vector2 pInput; // player movement input
    private bool jumpInput; // pounce input

    public float lookSensitivity = 3;
    private Vector2 rotation = Vector2.zero;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // get attached rigidbody (we'll be using a capsule collider for 3D movement
        vel = new Vector3(); // reset velocity
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

        pInput.x = Input.GetAxisRaw("Horizontal"); // we use two separate operations here as it's slightly
        pInput.y = Input.GetAxisRaw("Vertical");   // more efficient than creating a new Vector2 every frame
        if (Input.GetButtonDown("Jump")) { jumpInput = true; } // queue this up in update to ensure responsiveness

        if (Input.GetKey("escape")) // quit game
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        

        vel += Vector3.Normalize(transform.rotation * new Vector3(pInput.x, 0, pInput.y)) * speed * Time.deltaTime; // adjust player velocity, normalize increase to ensure consistency
        vel += new Vector3(0, -gravity, 0) * Time.deltaTime; // apply gravity
        RaycastHit rh;
        if (Physics.SphereCast(transform.position + new Vector3(0, 1, 0), 0.15f, Vector3.down, out rh, transform.localScale.y)) { vel.y = 0; grounded = true; } else { grounded = false; jumpInput = false; } // ground detection

        if (jumpInput && grounded)
        {
            jumpInput = false;
            vel.y += jumpStrength;
        }

        vel = new Vector3(vel.x * (1 - friction), vel.y, vel.z * (1 - friction)); //reduce horizontal velocity based on specified friction
        rb.linearVelocity = vel; //update player velocity
    }

    public void doLook()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y") * lookSensitivity;
        rotation.x = Mathf.Clamp(rotation.x, -60f, 60f);
        transform.eulerAngles = new Vector2(0, rotation.y) * lookSensitivity;
        cam.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
    }
}
