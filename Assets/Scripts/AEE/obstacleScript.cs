using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target,circleTrigger,burstParticle;
    public bool obstacleisDead,isshot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
           // circleTrigger.GetComponent<CircleCollider2D>().enabled = true;
        }
        else
        {
           // circleTrigger.GetComponent<CircleCollider2D>().enabled = false;
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("check which is the collision----" + collision.gameObject.name);

        if ((collision.gameObject.tag == "Bullet" && !obstacleisDead) || (collision.gameObject.tag == "EnemyBullet" && !obstacleisDead))
        {
            GameObject temp = Instantiate(burstParticle, transform.position, Quaternion.identity, gameObject.transform.parent);
            // GetComponent<BoxCollider2D>().enabled = false;
            obstacleisDead = true;
            circleTrigger.GetComponent<CircleCollider2D>().enabled = true;
            SoundManager.instance.playShootSound(8);        
             Destroy(gameObject,0.1f);
        }

        if (collision.gameObject.tag =="Enemy" && !isshot && obstacleisDead)
        {
            Debug.Log("check which is the collision----" + collision.gameObject.name);
            target = collision.gameObject;
            //target.transform.parent.GetComponent<Enemy>().gotHit = true;
            target.transform.parent.GetComponent<Enemy>().gothitbyBomb();
            isshot = true;
            Destroy(gameObject);
        }



        //if (Physics2D.GetIgnoreLayerCollision(5, 8))
        //{
        //    Debug.Log("bullet ignored collision oncollisionenter--" + collision.gameObject.name);

        //}

         //Debug.Log("bullet collision --" + collision.gameObject.name);


        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.name == "Player" && obstacleisDead)
        {
            target = collision.gameObject;
            target.GetComponent<FinalAnimTest>().dropDead("Bomb");
            Debug.Log("check which is the collision----" + collision.gameObject.name);
            Destroy(gameObject);
        }


    }


    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "EnemyBullet")
        {
            Debug.Log("check which is the collision----" + collision.gameObject.name);
            obstacleisDead = true;
            GameObject temp = Instantiate(burstParticle, transform.position, Quaternion.identity, gameObject.transform.parent);
            circleTrigger.GetComponent<CircleCollider2D>().enabled = true;
            SoundManager.instance.playShootSound(8);

             // Destroy(gameObject);
        }
        else
        {
           // Destroy(gameObject);
        }


        if (collision.gameObject.tag == "Enemy" && !isshot && obstacleisDead)
        {

            Debug.Log("check which is the collision----" + collision.gameObject.name);


        }

        if (Physics2D.GetIgnoreLayerCollision(5, 8))
        {
            Debug.Log("bullet ignored collision oncollisionenter--" + collision.gameObject.name);

        }

        //if (collision.gameObject.tag == "Bullet")
        //{
        //    Debug.Log("check which is the collision----" + collision.gameObject.name);
        //    //  Destroy(gameObject);
        //}


        if ((collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.name == "Agent" && obstacleisDead))
        {
            //obstacleisDead = false;
            target = collision.gameObject.transform.parent.gameObject;
            target.GetComponent<FinalAnimTest>().dropDead("Bomb");
            Debug.Log("check which is the collision----" + collision.gameObject.name);
            Destroy(gameObject,0.1f);
        }


        if (collision.gameObject.tag == "Enemy" && !isshot && obstacleisDead)
        {
            Debug.Log("check which is the collision----" + collision.gameObject.name);
            target = collision.gameObject;
          //  target.transform.parent.GetComponent<Enemy>().gothitbyBomb();
            isshot = true;
            Destroy(gameObject,0.1f);
        }
    }
}
