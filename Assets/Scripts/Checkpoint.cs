using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public Vector3 respawnOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Start()
    {
        
    }*/

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    private void OnDrawGizmos()
    {
        Color cc = new Color(0, 0.5f, 1, 0.5f);
        Gizmos.color = cc;
        Gizmos.DrawSphere(transform.position, gameObject.GetComponent<SphereCollider>().radius);
    }
}
