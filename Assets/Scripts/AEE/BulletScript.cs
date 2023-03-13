using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject bloodPart,burstPart,canvasParent;
    public TrailRenderer trailTail;
    public float trailwidth, bulletSelfDestroytime;
    public Vector3 Direction;
    public Quaternion rotation;
    public bool moveEnabled;
    public static BulletScript instance;
    void Start()
    {
        instance = this;
        Destroy(gameObject, bulletSelfDestroytime);
        //canvasParent = FindObjectOfType<FinalAnimTest>().gameObject.transform.parent.gameObject;
    }


    public void Update()
    {
        if (Direction != null)
        {
            // transform.position += Direction *100f * Time.deltaTime;
            // GetComponent<Rigidbody2D>().AddForce(Direction * 100f * Time.deltaTime);
            //GetComponent<Rigidbody2D>().velocity = Direction * 100f;
        }


        //if (MenuManager.instance.itstimetoDestroy)
        //{
        //    MenuManager.instance.itstimetoDestroy = false;
        //    Destroy(gameObject);
        //}
    }

    private void FixedUpdate()
    {
        if (Direction != null)
        {
             transform.position += Direction *300f * Time.deltaTime;
           // GetComponent<Rigidbody2D>().AddForce(Direction * 100f * Time.deltaTime);
          // GetComponent<Rigidbody2D>().velocity = Direction * 100f;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag!= "Bullet")
        {

            //  Debug.Log("Enemey is dead--" + collision.gameObject.name);

            if (collision.gameObject.tag == "Enemy")
            { 

               // Debug.Log("Enemey is dead--" + collision.gameObject.name);
                GameObject bullpart = Instantiate(bloodPart,transform.position,Quaternion.identity, collision.gameObject.transform.parent);
                collision.gameObject.transform.parent.GetComponent<Enemy>().gothitDir = Direction;
                collision.gameObject.transform.parent.GetComponent<Enemy>().bP = collision.gameObject;
                collision.gameObject.transform.parent.GetComponent<Enemy>().gotHit = true;
                SoundManager.instance.playShootSound(6);
                //Physics2D.IgnoreLayerCollision(5,8);
                Destroy(gameObject);

            }



            if (collision.gameObject.tag == "Body" && gameObject.tag!="Bullet")
            {

              // Debug.Log("the palyer body part that got hit--" + collision.gameObject.name);

                if(collision.gameObject.name == "Player")
                {
                    Debug.Log("the palyer body part that got hit--" + collision.gameObject.name);
                    Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());

                }


                if (collision.gameObject.name != "Player" && collision.gameObject.GetComponent<HingeJoint2D>())
                {
                    collision.gameObject.GetComponent<HingeJoint2D>().enabled = false;
                   // collision.gameObject.GetComponent<FinalAnimTest>().dropDead(collision.gameObject);

                }

                //  collision.gameObject.transform.parent.GetComponent<FinalAnimTest>().dropDead(collision.gameObject);

                GameObject bullpart = Instantiate(bloodPart, transform.position, Quaternion.identity, collision.gameObject.transform.parent);
                SoundManager.instance.playShootSound(6);


                //GameObject burstParticle = Instantiate(burstPart, transform.position, Quaternion.identity, collision.transform);
                Destroy(gameObject);
            }


            if (collision.gameObject.tag == "Body" && gameObject.tag == "Bullet")
            {


                Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
               // Debug.Log("collision ignored--" + collision.gameObject.name);

            }


            if (collision.gameObject.tag == "Ground" || collision.gameObject.name =="Barrel" || collision.gameObject.tag == "Gun" || collision.gameObject.tag == "Obstacles")
            {
                GameObject burstParticle = Instantiate(burstPart, transform.position, Quaternion.identity, collision.transform);
                SoundManager.instance.playShootSound(9);
                Destroy(gameObject);

            }

            //Debug.Log("destroy");


            if (collision.gameObject.name == "Bullet" && gameObject.tag == "EnemyBullet")
            {
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }


            //if (collision.gameObject.tag == "Gun" && collision.gameObject.GetComponent<GunManager>().GunType == GunManager.gunTypes.Knife)
            //{
            //    Debug.Log("Aruva dawwww--" + collision.gameObject.name);
            //}

        }


        if (collision.gameObject.tag != "EnemyBullet")
        {

            //Debug.Log("bullet got hit");

        }




        if (Physics2D.GetIgnoreLayerCollision(5, 8))
        {
            Debug.Log("bullet ignored collision oncollisionenter--" + collision.gameObject.name);

        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {

        //if (collision.gameObject.name == "Player" && gameObject.tag == "EnemyBullet")
        //{
        //    //Debug.Log("the palyer body part that got hit--" + collision.gameObject.name);
        //    collision.gameObject.GetComponent<FinalAnimTest>().dropDead();
        //}

        if (collision.gameObject.tag == "Gun")
        {
            if (collision.GetComponent<GunManager>().GunType == GunManager.gunTypes.Knife)
            {
                Debug.Log("Aruva dawwww--" + collision.gameObject.name);
                GameObject burstParticle = Instantiate(burstPart, transform.position, Quaternion.identity, collision.transform);
                SoundManager.instance.playShootSound(9);
                Destroy(gameObject);
            }
        }



        if (collision.gameObject.name == "Agent" && gameObject.tag == "EnemyBullet")
        {
           //Debug.Log("the palyer body part that got hit- by trigger-" + collision.gameObject.name);
            collision.gameObject.transform.parent.GetComponent<FinalAnimTest>().dropDead("GunShot");
        }


      


        if (collision.gameObject.transform.parent.name == "AgentRagdoll")
        {
            //Debug.Log("the palyer body part that got hit--" + collision.gameObject.name);
            collision.gameObject.transform.parent.GetComponent<PlayerHit>().gothit(collision.gameObject);

            GameObject burstParticle = Instantiate(burstPart, transform.position, Quaternion.identity, collision.transform);
            GameObject bullpart = Instantiate(bloodPart, transform.position, Quaternion.identity, collision.gameObject.transform.parent);
            SoundManager.instance.playShootSound(6);
            Destroy(gameObject);
        }




        if (collision.gameObject.layer == LayerMask.NameToLayer("Dead") && collision.gameObject.tag == "Enemy")
        {

          //Debug.Log("Enemey is in dead layer dead--" + collision.gameObject.name);
            GameObject bullpart = Instantiate(bloodPart, transform.position, Quaternion.identity, collision.gameObject.transform.parent);
            collision.gameObject.transform.parent.GetComponent<Enemy>().gothitDir = Direction;
            collision.gameObject.transform.parent.GetComponent<Enemy>().bP = collision.gameObject;
            collision.gameObject.transform.parent.GetComponent<Enemy>().gotHit = true;
            SoundManager.instance.playShootSound(6);
            Destroy(gameObject);


            
        }



        if (collision.gameObject.tag == "Ground")
        {
            GameObject burstParticle = Instantiate(burstPart, transform.position, Quaternion.identity, collision.transform);
            SoundManager.instance.playShootSound(9);
            Destroy(gameObject);

        }



        if (collision.gameObject.name == "Barrel")
        {
            GameObject burstParticle = Instantiate(burstPart, transform.position, Quaternion.identity, collision.transform);
            SoundManager.instance.playShootSound(9);
            Destroy(gameObject);
        }


        if (collision.gameObject.name == "Bullet" && gameObject.tag == "EnemyBullet")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }





        //if (Physics2D.GetIgnoreLayerCollision(5, 8))
        //{
        //    Debug.Log("bullet ignored collision OntriggerEnter--" + collision.gameObject.name);
        //}
        // Debug.Log("bullet ignored collision OntriggerEnter--" + collision.gameObject.name);

    }
}
