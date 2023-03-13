using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SoundManager instance;
    public GameObject soundPrefab,LeftFoot,RightFoot,zoomSound,PickupSound,slideSound;
    public Slider sfxSlider, musicSlider;
    public AudioSource bgm;
    public AudioClip bgm1;
    public bool footSoundOn;
    public AudioClip[] shootSounds,movementSounds,shellSounds;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        bgm.Play();

       
    }

    // Update is called once per frame
    void Update()
    {

        bgm.volume = musicSlider.value;

        if (Input.GetKeyUp(KeyCode.P))
        {
            playShootSound(1);
        }
    }


    public void playShootSound(int id)
    {
      //  Debug.Log("soundPLayer-" + id);
        GameObject tempSound = Instantiate(soundPrefab, gameObject.transform);
        tempSound.GetComponent<soundclipcheck>().myAudioSource.clip= shootSounds[id];
        tempSound.GetComponent<soundclipcheck>().myAudioSource.Play();

    }



    public void playmovementSound(int id ,string soundnickname,float delaytime)
    {

        if(soundnickname == "LeftFoot" && LeftFoot == null)
        {
            GameObject footSound = Instantiate(soundPrefab, gameObject.transform);
            footSound.name = soundnickname;
            if (soundnickname == "LeftFoot")
            {
                LeftFoot = footSound;
            }
            else if (soundnickname == "RightFoot")
            {
                {
                    RightFoot = footSound;

                }
            }

            footSound.GetComponent<soundclipcheck>().myAudioSource.clip = movementSounds[id];
            footSound.GetComponent<soundclipcheck>().myAudioSource.PlayDelayed(delaytime);

        }


        if (soundnickname == "RightFoot" && RightFoot == null)
        {
            GameObject footSound = Instantiate(soundPrefab, gameObject.transform);
            footSound.name = soundnickname;
          
            if (soundnickname == "RightFoot")
            {
                RightFoot = footSound;

            }
            else if (soundnickname == "LeftFoot")
            {
                {
                    LeftFoot = footSound;

                }
            }

            footSound.GetComponent<soundclipcheck>().myAudioSource.clip = movementSounds[id];
            footSound.GetComponent<soundclipcheck>().myAudioSource.PlayDelayed(delaytime);
        }



        if (soundnickname == "ZoomIn" && zoomSound == null)
        {
            GameObject zSound = Instantiate(soundPrefab, gameObject.transform);
            zSound.name = soundnickname;

            if (soundnickname == "ZoomIn")
            {
                zoomSound = zSound;

            }


            zoomSound.GetComponent<soundclipcheck>().myAudioSource.clip = movementSounds[id];
            // footSound.GetComponent<soundclipcheck>().myAudioSource.Play();
            zoomSound.GetComponent<soundclipcheck>().myAudioSource.PlayDelayed(delaytime);
        }

        if (soundnickname == "PickUp" && PickupSound == null)
        {
            GameObject pSound = Instantiate(soundPrefab, gameObject.transform);
            pSound.name = soundnickname;

            if (soundnickname == "PickUp")
            {
                PickupSound = pSound;

            }


            pSound.GetComponent<soundclipcheck>().myAudioSource.clip = movementSounds[id];
            pSound.GetComponent<soundclipcheck>().myAudioSource.PlayDelayed(delaytime);
        }




        if (soundnickname == "SlideAway" && slideSound == null && !FinalAnimTest.instance.isSliding)
        {
            GameObject sSound = Instantiate(soundPrefab, gameObject.transform);
            sSound.name = soundnickname;

            if (soundnickname == "SlideAway")
            {
                slideSound = sSound;

            }


            sSound.GetComponent<soundclipcheck>().myAudioSource.clip = movementSounds[id];
            sSound.GetComponent<soundclipcheck>().myAudioSource.PlayDelayed(delaytime);
        }

    }

    public void playShellSound(int id)
    {
        if (id > 0)
        {
            GameObject tempSound = Instantiate(soundPrefab, gameObject.transform);
            tempSound.GetComponent<soundclipcheck>().myAudioSource.PlayOneShot(shellSounds[Random.Range(id - 1, id)],0.5f);
        }
    }


    public void playerMovemntSoundSimple(int id)
    {
        GameObject tempSound = Instantiate(soundPrefab, gameObject.transform);
        tempSound.GetComponent<soundclipcheck>().myAudioSource.clip = movementSounds[id];
        tempSound.GetComponent<soundclipcheck>().myAudioSource.Play();
    }
}
