using UnityEngine;

public class FXandAudioImplementation : MonoBehaviour
{
    public PlayerBreath playerBreath;
    public GameObject suck;
    public ParticleSystem blow;

    // Update is called once per frame
    void Update()
    {
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
