using TMPro;
using UnityEngine;

public class FXandAudioImplementation : MonoBehaviour
{
    [SerializeField] GameObject suck;
    [SerializeField] ParticleSystem blow;
    [SerializeField] GameObject cam;
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioClip[] audios;
    public bool sucking;
    bool suckingLoop;

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

            if (!suckingLoop)
            {
                playerAudio.clip = audios[0];
                playerAudio.Play();
                suckingLoop = true;
            }
            else if (!playerAudio.isPlaying)
            {
                playerAudio.loop = true;
                playerAudio.clip = audios[1];
                playerAudio.Play();
            }
            
        }
        else
        {
            if (suckingLoop)
            {
                playerAudio.loop = false;
                playerAudio.clip = audios[2];
                playerAudio.Play();
                suckingLoop = false;
            }


            if (suck.gameObject.activeSelf)
            {
                suck.SetActive(false);
            }
        }

    }
    public void Blow()
    {
        sucking = false;
        suckingLoop = false;

        int currentScream = Random.Range(3, 5);
        playerAudio.loop = false;
        playerAudio.clip = audios[currentScream];
        playerAudio.Play();

        blow.Play();
    }
}
