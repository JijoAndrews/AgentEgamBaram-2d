using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BreakableGlass : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject glassParticles,bookParticle,glasstohide,sideGlass;
    public bool isglassBroken;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.tag == "Body")
        //{
            
        //    if (glasstohide != null)
        //    {
        //        glasstohide.SetActive(false);
        //    }

        //    sideGlass.GetComponent<Image>().enabled = false;
        //    SoundManager.instance.playShootSound(11);
        //}
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Body" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Gun" || collision.gameObject.tag=="BarrelBomb")
        {     
            if (!isglassBroken)
            {
                if (glasstohide != null)
                {
                    glasstohide.SetActive(false);
                }

                if (bookParticle != null)
                {
                    bookParticle.SetActive(true);
                }

                glassParticles.GetComponent<ParticleSystem>().Play();
                isglassBroken = true;
                sideGlass.GetComponent<Image>().enabled = false;
                SoundManager.instance.playShootSound(11);
            }
           
        }


       // Debug.Log("glasses broken--" + collision.gameObject.name);
    }
}
