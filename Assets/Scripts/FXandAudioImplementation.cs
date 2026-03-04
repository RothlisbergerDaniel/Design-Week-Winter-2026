using UnityEngine;

public class FXandAudioImplementation : MonoBehaviour
{
    [SerializeField] GameObject suck;
    [SerializeField] ParticleSystem blow;
    [SerializeField] GameObject cam;
    public bool sucking;

    // Update is called once per frame
    void Update()
    {
        suck.transform.rotation = cam.transform.rotation;
        blow.transform.rotation = cam.transform.rotation;


        if (sucking)
        {
            if (!suck.gameObject.activeSelf)
            {
                suck.SetActive(true);
            }
        }
        else
        {
            if (suck.gameObject.activeSelf)
            {
                suck.SetActive(false);
            }
        }

    }
    public void Blow()
    {
            blow.Play();
    }
}
