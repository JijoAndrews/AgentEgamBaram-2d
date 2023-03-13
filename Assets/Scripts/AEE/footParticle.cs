using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footParticle : MonoBehaviour
{
    public GameObject Player,dustPart,slideDustPartleft,footsound;
    public LayerMask objectMask;
    public static footParticle instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        isFootOntheGround();
        slideDustPartleft.gameObject.transform.localScale = new Vector3(Mathf.Sign(Player.transform.localScale.x), slideDustPartleft.gameObject.transform.localScale.y, slideDustPartleft.gameObject.transform.localScale.z);
    }


    public bool isFootOntheGround()
    {
        Ray2D ray1 = new Ray2D(gameObject.transform.position, Vector2.down);
        RaycastHit2D rayhit = Physics2D.Raycast(ray1.origin, ray1.direction, 0.8f, objectMask);

        if (rayhit)
        {
            Debug.DrawRay(ray1.origin, ray1.direction * 0.8f, Color.yellow);
        }
        else
        {
            Debug.DrawRay(ray1.origin, ray1.direction * 0.8f, Color.red);
        }

        return rayhit;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Ground")
        //{
        //    GameObject temp = Instantiate(dustPart, transform.position, Quaternion.identity);
        //}
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && Input.GetKey(KeyCode.S) && FindObjectOfType<FinalAnimTest>().testspeed>0f)
        {
            
            slideDustPartleft.SetActive(true);
           // slideDustPartleft.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            slideDustPartleft.SetActive(false);
        }


    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && isFootOntheGround() )
        {
           // Debug.Log("the foot--" + gameObject.name + "-collided object--" + collision.gameObject.name);
            SoundManager.instance.playmovementSound(0,gameObject.name,0.1f);
            if (!Player.GetComponent<FinalAnimTest>().isSliding)
            {
                Player.GetComponent<FinalAnimTest>().rb.velocity = Vector2.zero;

            }

        }


        if (collision.gameObject.tag == "Enemey" && collision.gameObject.transform.parent.GetComponent<Enemy>().IsDead)
        {
             Debug.Log("the foot-- -collided object--" + collision.gameObject.name);


            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
            //SoundManager.instance.playmovementSound(0, gameObject.name, 0.1f);
        }


        if ( collision.gameObject.tag == "Gun" && isFootOntheGround())
        {

          //  Debug.Log("the foot-- -collided object--" + collision.gameObject.name);
            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
        }

        if (collision.gameObject.tag == "Obstacles")
        {
            //Player.GetComponent<FinalAnimTest>().rb.velocity = Vector2.zero;
            //  Debug.Log("the foot-- -collided object--" + collision.gameObject.name);
            //.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
        }

    }


}
