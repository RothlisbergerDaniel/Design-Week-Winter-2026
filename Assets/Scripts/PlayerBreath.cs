using UnityEngine;

public class PlayerBreath : MonoBehaviour
{
    [SerializeField]
    private GameObject cam; // used to aim for the player's sucking and blowing
    private CharacterController controller;

    public float suckStrength = 1;
    public float blowStrength = 1; // how much force to pull objects in with and blow them out with

    public float suckRange = 5;
    public float suckRadius = 2;
    public float blowRange = 5;
    public float blowRadius = 5; // range and radius at which these are effective

    public float maxCharge = 2; // in seconds
    public float minBlowCharge = 1; // minimum charge required to blow
    public float suckCooldown = 1; // time before the player can suck again after blowing
    private float breathTimer = 0; // internal variable to track how long the player has held their breath for
    private float doSuck = 0; // float so that we can set independent suck strength later
    private float doBlow = 0; // float so that we can set independent blow strength later

    [SerializeField]
    private LayerMask movableObjects;

    [SerializeField]
    private float gravReduction = 5; // how much to reduce gravity by when pulling objects upwards
    [SerializeField]
    private float underfootThreshold = 2; // how far below the player until an object is considered "underfoot" and can no longer be sucked upwards

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        breathTimer = 0;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (breathTimer < 0)
        {
            breathTimer = Mathf.Min(breathTimer + Time.deltaTime, 0); // cap at 0
            
        }

        if (Input.GetMouseButton(0) && breathTimer >= 0) // lmb held down and can inhale
        {
            breathTimer += Time.deltaTime; // increase breath timer
            doSuck = 1;
            if (breathTimer > maxCharge) // if we were just sucking or we've exceeded the max hold time
            {
                breathTimer = maxCharge; // cap breath
            }

        } else if (breathTimer > 0)
        {
            doSuck = 0; // cancel suck
            /*if (breathTimer >= minBlowCharge)
            {
                doBlow = 1; // use this to queue up an exhale
            }*/
            breathTimer = Mathf.Max(breathTimer - Time.deltaTime, 0); // reduce charge over time
        }
        if (Input.GetMouseButton(1) && breathTimer >= minBlowCharge)
        {
            doSuck = 0; // cancel suck
            doBlow = 1; // use this to queue up an exhale
            breathTimer = -2; // prevent spamming
        }


    }

    private void FixedUpdate()
    {
        if (doSuck > 0) // we must be sucking
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + (cam.transform.forward * suckRadius), suckRadius, cam.transform.forward, suckRange, movableObjects);

            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    Vector3 hp = hits[i].transform.position;

                    Vector3 suckDir = (transform.position - new Vector3(hp.x, hp.y- hits[i].transform.localScale.y, hp.z)).normalized; // subtract scale from hit position to make suck direction more accurate
                    if (suckDir.y > 0) { suckDir.y *= gravReduction; }

                    if (!((new Vector2(transform.position.x,transform.position.z) - new Vector2(hp.x, hp.z)).magnitude <= underfootThreshold && transform.position.y - hp.y < underfootThreshold * 3)) // as long as the object isn't too close horizontally and is below player
                    {
                        hits[i].rigidbody.AddForce(suckDir * suckStrength, ForceMode.Force); // only apply velocity if the object is far enough away so we can't fly
                    }

                    if ((transform.position - hp).magnitude <= underfootThreshold*2) { hits[i].rigidbody.linearVelocity = Vector3.zero; } // stop them from moving if too close to the player

                    
                }
            }
        }

        if (doBlow > 0)
        {
            doBlow = 0; // reset
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + (cam.transform.forward * blowRadius), blowRadius, cam.transform.forward, blowRange, movableObjects);

            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    Vector3 hp = hits[i].transform.position;

                    Vector3 blowDir = (cam.transform.forward).normalized; // blow everything in the direction of aim
                    //if (blowDir.y < 0) { blowDir.y *= gravReduction; }

                    //if (!((new Vector2(transform.position.x, transform.position.z) - new Vector2(hp.x, hp.z)).magnitude <= underfootThreshold && transform.position.y - hp.y < underfootThreshold * 3)) // as long as the object isn't too close horizontally and is below player
                    //{
                        hits[i].rigidbody.linearVelocity = new Vector3(); // reset velocity
                        hits[i].rigidbody.AddForce(blowDir * blowStrength, ForceMode.Impulse); // only apply velocity if the object is far enough away so we can't fly
                    //}


                }
            }

        }
    }
}
