using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundclipcheck : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource myAudioSource;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        myAudioSource.volume = SoundManager.instance.sfxSlider.value;

        if(!myAudioSource.isPlaying)
        {
            if (gameObject.name == "footSound")
            {
                SoundManager.instance.footSoundOn = false;
            }

            Destroy(gameObject);
        }
    }
}
