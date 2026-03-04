using UnityEngine;

public class FXandAudioImplementation : MonoBehaviour
{
    public PlayerBreath playerBreath;
    public GameObject suck;
    public ParticleSystem blow;
    public GameObject cam;

    // Update is called once per frame
    void Update()
    {
        suck.transform.rotation = cam.transform.rotation;
        blow.transform.rotation = cam.transform.rotation;
        if (playerBreath.doSuck > 0)
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

        if (playerBreath.doBlow > 0)
        {
            blow.Play();
        }
    }
}
