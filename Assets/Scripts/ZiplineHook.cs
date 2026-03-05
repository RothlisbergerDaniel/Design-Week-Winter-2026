using Unity.VisualScripting;
using UnityEngine;

public class ZiplineHook : MonoBehaviour
{
    private Vector3 startPos;
    private Rigidbody rb;
    private GameObject player;
    private PlayerBreath pb;
    private LineRenderer lr;

    public float returnSpeed;
    public float range = 1;
    public float playerAttachDistance;
    public float homeReleaseDistance; // shouldn't be changed much. How close to the spool to "release" the player
    public float verticalLaunchStrength = 1;
    public float horizontalLaunchStrength = 3;

    private float onPlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position; // store starting position for later
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player"); // get the player without needing to assign reference in engine
        pb = player.GetComponent<PlayerBreath>();
        onPlayer = 0;

        // Add a LineRenderer component // TAKEN DIRECTLY FROM UNITY DOCUMENTATION ! ! !
        lr = gameObject.GetComponent<LineRenderer>();

        // Set the material
        lr.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        lr.startColor = Color.black;

        // Set the width
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;

        // Set the number of vertices
        lr.positionCount = 2;

        // Set the positions of the vertices
        lr.SetPosition(0, new Vector3(0, 0, 0));
        lr.SetPosition(1, new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (onPlayer <= 0) { rb.linearVelocity += ((startPos - transform.position) / range) * returnSpeed * Time.fixedDeltaTime; }
        else { rb.linearVelocity += ((startPos - transform.position)) * returnSpeed * Time.fixedDeltaTime; } // pull back at FULL strength to FLING the player
        
        rb.linearVelocity *= 0.95f; // friction

        if (Vector3.Magnitude(player.transform.position - transform.position) <= playerAttachDistance && pb.doSuck > 0) // if we're close enough to the player, then...
        {
            onPlayer = 1; // attach
            Color mc = gameObject.GetComponent<Renderer>().material.color;
            gameObject.GetComponent<Renderer>().material.color = new Color(mc.r, mc.g, mc.b, 0.5f);
            rb.linearVelocity = new Vector3();
            //Debug.Log("Attached!");
        }

        /*if (onPlayer > 0 && pb.doSuck > 0)
        {
            Vector3 ptf = pb.cam.transform.position;
            transform.position = new Vector3(ptf.x, ptf.y-0.25f, ptf.z) + (pb.cam.transform.forward*0.75f);
        }*/

        if (onPlayer > 0 &&  pb.doSuck <= 0)
        {
            
            player.GetComponent<PlayerMovement>().vel = rb.linearVelocity;
            player.GetComponent<PlayerMovement>().onHook = true;

            Vector3 pvel = player.GetComponent<PlayerMovement>().vel;

            if (Vector3.Magnitude(startPos - transform.position) <= homeReleaseDistance) 
            {
                player.GetComponent<PlayerMovement>().vel = new Vector3(pvel.x * horizontalLaunchStrength, pvel.y * verticalLaunchStrength, pvel.z * horizontalLaunchStrength);
                player.GetComponent<PlayerMovement>().onHook = false;
                onPlayer = 0;
                //onPlayer = 0; Debug.Log("Released!"); 
            }
        }
        if (pb.doBlow > 0 || (pb.doSuck > 0 && Vector3.Magnitude(player.transform.position - transform.position) > playerAttachDistance)) { onPlayer = 0; } // force detach by blowing
        if (onPlayer <= 0) 
        { 
            player.GetComponent<PlayerMovement>().onHook = false; Color mc = gameObject.GetComponent<Renderer>().material.color;
            gameObject.GetComponent<Renderer>().material.color = new Color(mc.r, mc.g, mc.b, 1f);
        }

        lr.SetPosition(0, startPos);
        lr.SetPosition(1, transform.position);
    }
}
