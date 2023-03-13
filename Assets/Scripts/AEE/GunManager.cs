using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GunManager : MonoBehaviour
{
    public GameObject player,canvas,bulletPos,ShellPos, bulletPrefab,shellPrefab,burstPart,bludPart;
    public float bulletTimerval;
    public TrailRenderer throwTrail;
    public List<Sprite> shellsSprites;
    public Sprite curShellsprite;
    public bool IsAvailable,IntheHand,isrotateStarted,isinEnemeyHand,isDead,istimetoselfDestroy;
    public Vector3 Direction,rotateDirection;
    public gunTypes GunType;
    public static GunManager instance;

    public enum gunTypes
    {
        Knife,
        Pistol,
        Smg,
        Shotgun,
        Aks,
        Sniper,
        SliencedPistol
       
    }
   
    void Start()
    {
        instance = this;
        curShellsprite = shellsSprites[((int)GunType)];

    }
   
    void Update()
    {

        //if ((MenuManager.instance.itstimetoDestroy && !IsAvailable ) || (MenuManager.instance.itstimetoDestroy && !isinEnemeyHand ) ||( MenuManager.instance.itstimetoDestroy && IntheHand))
        //{
        //    //MenuManager.instance.itstimetoDestroy = false;

        //    Debug.Log("gun going to be destroyed--" + gameObject.name);
        //    Destroy(gameObject);
        //}


        //if ((istimetoselfDestroy && !IsAvailable) || (istimetoselfDestroy && !isinEnemeyHand) || (istimetoselfDestroy && IntheHand))
        //{
        //  //MenuManager.instance.itstimetoDestroy = false;
        //    Debug.Log("gun going to be destroyed--" + gameObject.name);
        //    Destroy(gameObject);
        //}

        //if (istimetoselfDestroy && MenuManager.instance.itstimetoDestroy)
        //{
        //    Debug.Log("the gameobject needs to be destroyed--" + gameObject.name);
        //    istimetoselfDestroy = false;
        //}


        if (IsAvailable)
        {
            IsAvailable = false;
            player = FindObjectOfType<FinalAnimTest>().gameObject;
        }

        if (isrotateStarted)
        {
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            // transform.rotation= Quaternion.AngleAxis(angle, Vector3.forward);

            transform.Rotate(rotateDirection * 1000f * Time.deltaTime);
            throwTrail.enabled = true;
            throwTrail.transform.Rotate(rotateDirection * 1000f * Time.deltaTime);
            //  transform.Rotate(0f, 0f, angle,0f);
        }


        //if (isDead)
        //{
        //    Destroy(gameObject);
        //}

        //if (Input.GetKey(KeyCode.Q))
        //{
        //    GameObject Tempbullet1 = Instantiate(bulletPrefab,bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, player.transform.position.normalized), transform.parent);
        //    Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(player.transform.position.x, player.transform.position.y, 0f);
        //}

    }

    public void shellthrow(float _shellsize)
    {
        burstPart.GetComponent<ParticleSystem>().Play();
       // GameObject temp = Instantiate(shellPrefab, ShellPos.transform.position,Quaternion.identity,canvas.transform);//the og
        GameObject temp = Instantiate(shellPrefab, ShellPos.transform.position, Quaternion.identity, player.transform.parent);

        temp.transform.localScale = new Vector3(_shellsize, _shellsize, _shellsize);
        temp.name = "Shell";
        temp.GetComponent<Image>().sprite = curShellsprite;
        temp.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100f, ForceMode2D.Impulse);
        temp.GetComponent<Rigidbody2D>().AddTorque(50f, ForceMode2D.Impulse);

        temp.GetComponent<SelfDestroy>().shelltype = GunType.ToString();
        temp.GetComponent<SelfDestroy>().shellId = (int)GunType;
    }


    public void weaponfly(GameObject wepParent)
    {
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        isrotateStarted = true;
        rotateDirection = Vector3.back;
       // Debug.Log("collider 1---" + wepParent.gameObject.name + "-----collider2-----" + gameObject.name);
       // Physics2D.IgnoreCollision(wepParent.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>());
       if(wepParent.name!= "AgentRagdoll")
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(player.transform.position.x, player.transform.position.y + 0.1f) * 0.1f, ForceMode2D.Impulse);
            IntheHand = false;

        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().simulated = true;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100f, ForceMode2D.Impulse);

        }

         isinEnemeyHand = false;
         istimetoselfDestroy = true;

        // rotateDirection = Vector3.forward;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
       // Debug.Log("check---" + collision.gameObject.name);

        if (collision.gameObject.tag == "Body")
        {
            throwTrail.enabled = false;
           // Debug.Log("check---" + collision.gameObject.name);
            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
           
            if (!isrotateStarted)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            //player.GetComponent<Simplejumpcontroller>().Weapon = null;
        }


        if (collision.gameObject.tag == "Gun")
        {
          //  Debug.Log("check-Gun--" + collision.gameObject.name);
            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (collision.gameObject.tag == "Ground" )
        {
            throwTrail.enabled = false;
            //  Debug.Log("check-ground--" + collision.gameObject.name);
            //Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //SoundManager.instance.playShootSound(9);
            isrotateStarted = false;
          //  player.GetComponent<Simplejumpcontroller>().Weapon = null;
           // Direction = (player.transform.position - transform.position).normalized;
           // GetComponent<Rigidbody2D>().AddForce(new Vector2(Direction.x, Direction.y) * 50f, ForceMode2D.Impulse);
           // GetComponent<Rigidbody2D>().AddTorque(40f, ForceMode2D.Impulse);
        }


        if (collision.gameObject.tag == "Enemy" && !isinEnemeyHand && !collision.gameObject.transform.parent.GetComponent<Enemy>().IsDead)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Direction = (player.transform.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Direction.x, Direction.y) * 50f, ForceMode2D.Impulse);
            rotateDirection = Vector3.forward;
            Debug.Log("check");
            SoundManager.instance.playShootSound(6);

            // GetComponent<Rigidbody2D>().AddTorque(40f, ForceMode2D.Impulse);
            //transform.Rotate(Vector3.forward * 5f);
        }



        if (collision.gameObject.tag == "Enemy" && GunType == gunTypes.Knife && !collision.gameObject.transform.parent.GetComponent<Enemy>().IsDead)
        {

            Debug.Log("Enemey is dead--" + collision.gameObject.name);
            GameObject bullpart = Instantiate(bludPart, transform.position, Quaternion.identity, collision.gameObject.transform.parent);
            collision.gameObject.transform.parent.GetComponent<Enemy>().gothitDir = Direction;
            collision.gameObject.transform.parent.GetComponent<Enemy>().bP = collision.gameObject;
            collision.gameObject.transform.parent.GetComponent<Enemy>().gotHit = true;

            SoundManager.instance.playShootSound(6);
            //Physics2D.IgnoreLayerCollision(5,8);
            //Destroy(gameObject);

        }







        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Obstacles")
        {

            istimetoselfDestroy = true;

        }


            throwTrail.enabled = false;

    }



    public void OnTriggerEnter2D(Collider2D collision)
    {

        //if (collision.gameObject.tag == "EnemyBullet" && GunType == gunTypes.Knife)
        //{

        //    Debug.Log("Enemey is dead--" + collision.gameObject.name);
        //  //  Destroy(collision.gameObject);

        //    //GameObject bullpart = Instantiate(bludPart, transform.position, Quaternion.identity, collision.gameObject.transform.parent);
        //    //collision.gameObject.transform.parent.GetComponent<Enemy>().gothitDir = Direction;
        //    //collision.gameObject.transform.parent.GetComponent<Enemy>().bP = collision.gameObject;
        //    //collision.gameObject.transform.parent.GetComponent<Enemy>().gotHit = true;

        //    SoundManager.instance.playShootSound(6);
        //    //Physics2D.IgnoreLayerCollision(5,8);
        //    //Destroy(gameObject);

        //}
    }
}
