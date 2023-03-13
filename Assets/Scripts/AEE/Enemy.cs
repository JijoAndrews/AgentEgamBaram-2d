using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public static Enemy instance;
    public string ignoreBodypart,hitBodyPart;
    public enemeyTypes enemyStatus;


    public GameObject player, handPos, hand, otherHand, palam, weapon, bulletPrefab, bP, bloodPartPrefab, gunPos, ThrowableObject, coconut, checkbullet;
    public Vector3 gunDir,gothitDir,Handoffset,ThrowableHandAngle;
    public float Angle,secs,RanVal,distance, playerdist;
    public Vector3[] weaponScalevals;
    public GameObject[] weaponsAvailable;
    public List<string> ignoreBodypartList;
    public List<GameObject> BodyParts,bodypartsRuntime;
    public List<HingeJoint2D> jointsOnBody;

    public List<GameObject> BodyPartsfly;
    public List<HingeJoint2D> BodyPartsflyjoints;

    public Rigidbody2D bodyRb;
    public bool gotHit,IsDead,enableToshoot;
    Vector3 localScale;

    public enum enemeyTypes
    {
        stabber,
        pawn,
        closeCall,
        Agressor,
        Assault,
        Snipe,
        hitMan,
        englishkaran,
        kattadhurai,
    }

    void Start()
    {
        player = FindObjectOfType<FinalAnimTest>().gameObject;
        localScale = transform.localScale;
        setEnemeyWithproperGun(enemyStatus);
        //shoot();

    }

    void Update()
    {
        if(enemyStatus != enemeyTypes.kattadhurai)
        {
            gunDir = (player.transform.position - hand.transform.position).normalized;

        }
        else
        {
            gunDir = (player.transform.position - transform.position).normalized;

        }
        Angle = Mathf.Abs(Mathf.Round(Mathf.Atan2(gunDir.y, gunDir.x) * Mathf.Rad2Deg));//og


        playerdist = Vector3.Distance(player.transform.position, transform.position);
        enableToshoot = playerdist < distance ? true : false;


        instance = this;
        if (gotHit)
        {
            //timeToSplit();//the og
            timetoSplitV2();
            IsDead = true;
            gotHit = false;
        }


        if (!IsDead && enableToshoot)
        {

            shoot();
            if (Angle > 100)
            {
                gameObject.transform.localScale = new Vector3(-localScale.x, localScale.y,1f);
                //  gameObject.transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(-1f, 1f, 1f), 0.1f);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(localScale.x, localScale.y, 1f);
                //gameObject.transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), 0.1f);

            }


            // playerdist = Vector3.Distance(player.transform.position, transform.position);

            //enableToshoot = playerdist < distance ? true : false;

            //if (playerdist < distance)
            //{

            //}

            if(enemyStatus!= enemeyTypes.kattadhurai && enemyStatus != enemeyTypes.englishkaran)
            {
                palam.transform.position = handPos.transform.position;
                hand.transform.rotation = Quaternion.LookRotation(Vector3.forward, -gunDir);
                palam.transform.rotation = Quaternion.LookRotation(Vector3.forward, -gunDir);
            }
            //else
            //{
            //    otherHand.transform.rotation = Quaternion.Euler(0f, 0f, -ThrowableHandAngle.x);//leftHand
            //    hand.transform.rotation = Quaternion.Euler(0f, 0f, -ThrowableHandAngle.y);
            //    coconut = Instantiate(ThrowableObject, handPos.transform.position, Quaternion.identity, gameObject.transform.parent);

            //}


        }
        else if(!IsDead)
        {
            palam.transform.position = handPos.transform.position;
            hand.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            palam.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

            //Physics2D.IgnoreLayerCollision(5, 8);

            if (otherHand != null)
            {
                otherHand.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);//leftHand
            }
        }



        if (Input.GetKeyUp(KeyCode.T))
        {
            //throwing();
            coconut = Instantiate(ThrowableObject, handPos.transform.position, Quaternion.identity, gameObject.transform.parent);
            coconut.GetComponent<throwableObjects>().throwableType = ThrowableObject.name;
            coconut.name = "coconut";
            throwing(coconut);
        }
    }


    public void setEnemeyWithproperGun(enemeyTypes currnType )
    {

        if (currnType != enemeyTypes.kattadhurai && currnType!= enemeyTypes.englishkaran)
        {
            weapon = Instantiate(weaponsAvailable[(int)currnType], gunPos.transform.position, Quaternion.identity, palam.transform);
            weapon.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            weapon.transform.localScale = weaponScalevals[(int)currnType];
            weapon.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
            weapon.name = weaponsAvailable[(int)currnType].name;
            weapon.GetComponent<GunManager>().player = player;
            weapon.GetComponent<GunManager>().isinEnemeyHand = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
      
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BarrelBomb")
        {
            Debug.Log("trigger Check on enemey body--" + collision.gameObject.name);
            gothitbyBomb();

        }
    }

    public void timeToSplit()
    {
        Debug.Log("Time to split the body");

        if (!bodypartsRuntime.Contains(bP))
        {
            bodypartsRuntime.Add(bP);
        }
        
        //if (!IsDead)
        //{
            for (int i = 0; i < jointsOnBody.Count; i++)
            {

                jointsOnBody[i].enabled = true;
                jointsOnBody[i].gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
            //bodyRb.bodyType = RigidbodyType2D.Dynamic;
            //bodyRb.AddForce(gothitDir * 200f, ForceMode2D.Impulse);
            weapon.transform.SetParent(gameObject.transform.parent);
            weapon.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;


            if (bP!=null && bP.GetComponent<HingeJoint2D>()!=null)
            {
                bP.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                bP.GetComponent<HingeJoint2D>().enabled = false;
                bP.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 200f, ForceMode2D.Impulse);
                //Debug.Log("the body part that flying--" + bP.name);
                jointsOnBody.Remove(bP.GetComponent<HingeJoint2D>());
            }
       // }

        bodyRb.bodyType = RigidbodyType2D.Dynamic;
        bodyRb.AddForce(gothitDir * 200f, ForceMode2D.Impulse);

        for (int i = 0; i < BodyParts.Count; i++)
        {
            BodyParts[i].gameObject.layer = LayerMask.NameToLayer("Dead");
        }
        gameObject.layer = LayerMask.NameToLayer("Dead");

    }


    public void timetoSplitV2()
    {
       // Debug.Log("Time to split the body");

        if (!IsDead)
        {
            for (int i = 0; i < jointsOnBody.Count; i++)
            {

                jointsOnBody[i].enabled = true;
                jointsOnBody[i].gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }

        if (weapon!=null)
        {
            weapon.transform.SetParent(gameObject.transform.parent);
            weapon.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            weapon.GetComponent<GunManager>().isinEnemeyHand = false;
        }
    


        if (bP != null && bP.GetComponent<HingeJoint2D>() != null)
        {
            bP.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            bP.GetComponent<HingeJoint2D>().enabled = false;
            bP.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 200f, ForceMode2D.Impulse);
            //Debug.Log("the body part that flying--" + bP.name);
            //jointsOnBody.Remove(bP.GetComponent<HingeJoint2D>());
        }

        bodyRb.bodyType = RigidbodyType2D.Dynamic;
        bodyRb.AddForce(gothitDir * 200f, ForceMode2D.Impulse);




        for (int i = 0; i < BodyParts.Count; i++)
        {
            BodyParts[i].gameObject.layer = LayerMask.NameToLayer("Dead");
        }
        gameObject.layer = LayerMask.NameToLayer("Dead");



        if (Physics2D.GetIgnoreLayerCollision(5, 8))
        {
            Debug.Log("bullet ignored collision OntriggerEnter--" +gameObject.name);
        }
    }


    public void gothitbyBomb()
    {
        if (!IsDead)
        {
            IsDead = true;
            bodyRb.bodyType = RigidbodyType2D.Dynamic;
            bodyRb.AddForce(Vector3.up * 10f, ForceMode2D.Impulse);
            bodyRb.AddTorque(Random.Range(10, 180) * 100f, ForceMode2D.Impulse);
            SoundManager.instance.playShootSound(1);
            GameObject bullpart = Instantiate(bloodPartPrefab, transform.position, Quaternion.identity, gameObject.transform.parent);

            for (int i = 0; i < jointsOnBody.Count; i++)
            {
                jointsOnBody[i].enabled = true;
                jointsOnBody[i].gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }

            if (weapon != null)
            {
                
               // weapon.transform.parent = player.transform.parent;
                weapon.transform.SetParent(gameObject.transform.parent);

                weapon.transform.position = Vector3.zero;
                weapon.transform.position = palam.transform.position;
                weapon.GetComponent<GunManager>().weaponfly(gameObject);
                weapon = null;
            }


            for (int i = 0; i < BodyParts.Count; i++)
            {
                BodyParts[i].gameObject.layer = LayerMask.NameToLayer("Dead");
            }
              gameObject.layer = LayerMask.NameToLayer("Dead");
        }

       
    }

    public void gothitBySlide()
    {
        if (!IsDead)
        {
            IsDead = true;
            bodyRb.bodyType = RigidbodyType2D.Dynamic;
            bodyRb.AddForce(Vector3.up * 100f, ForceMode2D.Impulse);
            bodyRb.AddTorque(Random.Range(-10, 180) * 100f, ForceMode2D.Impulse);
            SoundManager.instance.playShootSound(1);
            GameObject bullpart = Instantiate(bloodPartPrefab, transform.position, Quaternion.identity, gameObject.transform.parent);

            for (int i = 0; i < jointsOnBody.Count; i++)
            {
                jointsOnBody[i].enabled = true;
                jointsOnBody[i].gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }

            if (weapon != null)
            {
                //weapon.transform.parent = player.transform.parent;
                weapon.transform.SetParent(gameObject.transform.parent);
                weapon.transform.position = Vector3.zero;
                weapon.transform.position = palam.transform.position;
                weapon.GetComponent<GunManager>().weaponfly(gameObject);
                weapon = null;
            }

            for (int i = 0; i < BodyParts.Count; i++)
            {
                BodyParts[i].gameObject.layer = LayerMask.NameToLayer("Dead");
            }
            gameObject.layer = LayerMask.NameToLayer("Dead");


        }

          




        //for (int i = 0; i < BodyParts.Count; i++)
        //{
        //    BodyParts[i].gameObject.layer = LayerMask.NameToLayer("Dead");
        //}
        //gameObject.layer = LayerMask.NameToLayer("Dead");
    }

    public void shoot()
    {
        secs += 0.001f;

        if (secs>RanVal && weapon!=null)
        {
           // secs = 0f;
           // RanVal = Random.Range(3f, 10f);

            //if (weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Pistol)
            //{
            //    GameObject Tempbullet1 = Instantiate(bulletPrefab, weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, gunDir), transform.parent);
            //    Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(gunDir.x, gunDir.y, 0f);
            //    // player.GetComponent<FinalAnimTest>().dropDead();
            //}



            if ( weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Pistol)
            {
                secs = 0f;

                GameObject Tempbullet1 = Instantiate(bulletPrefab, weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, gunDir), transform.parent);

                checkbullet = Tempbullet1;
               // Debug.Log("enemey shoots the bullet---" + Tempbullet1.transform.parent.name);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(gunDir.x, gunDir.y, 0f);
                Tempbullet1.GetComponent<BulletScript>().trailTail.startWidth = 0.2f;
                Tempbullet1.GetComponent<BulletScript>().trailTail.time = 1f;



                SoundManager.instance.playShootSound(1);
            }




            if (weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.SliencedPistol)
            {
                secs = 0f;
                GameObject Tempbullet1 = Instantiate(bulletPrefab, weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, gunDir), transform.parent);
                checkbullet = Tempbullet1;
                // Debug.Log("enemey shoots the bullet---" + Tempbullet1.transform.parent.name);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(gunDir.x, gunDir.y, 0f);
                Tempbullet1.GetComponent<BulletScript>().trailTail.startWidth = 0.2f;
                Tempbullet1.GetComponent<BulletScript>().trailTail.time = 1f;



                SoundManager.instance.playShootSound(10);
            }





            if (weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Shotgun )
            {
                secs = 0f;

                GameObject Tempbullet3 = Instantiate(bulletPrefab, weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, new Vector3(gunDir.x + 0.1f, gunDir.y + 0.1f, 0f)), transform.parent);
                Tempbullet3.GetComponent<BulletScript>().Direction = new Vector3(gunDir.x, gunDir.y + 0.2f, 0f);

                GameObject Tempbullet = Instantiate(bulletPrefab, weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, new Vector3(gunDir.x + 0.1f, gunDir.y + 0.1f, 0f)), transform.parent);
                Tempbullet.GetComponent<BulletScript>().Direction = new Vector3(gunDir.x, gunDir.y + 0.1f, 0f);

                GameObject Tempbullet1 = Instantiate(bulletPrefab, weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, gunDir), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(gunDir.x, gunDir.y, 0f);


                GameObject Tempbullet2 = Instantiate(bulletPrefab, weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, new Vector3(gunDir.x - 0.1f, gunDir.y - 0.1f, 0f)), transform.parent);
                Tempbullet2.GetComponent<BulletScript>().Direction = new Vector3(gunDir.x, gunDir.y - 0.1f, 0f);

                GameObject Tempbullet4 = Instantiate(bulletPrefab, weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, new Vector3(gunDir.x + 0.1f, gunDir.y + 0.1f, 0f)), transform.parent);
                Tempbullet4.GetComponent<BulletScript>().Direction = new Vector3(gunDir.x, gunDir.y - 0.2f, 0f);

              //  CamFollow.instance.camshakeOn = true;

                //weapon.GetComponent<GunManager>().shellthrow(1f);
                SoundManager.instance.playShootSound(0);

            }

            if (weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Aks)
            {
                secs = 0f;

                GameObject Tempbullet1 = Instantiate(bulletPrefab, weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, gunDir), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(gunDir.x, gunDir.y, 0f);
                Tempbullet1.GetComponent<BulletScript>().trailTail.startWidth = 0.5f;
                Tempbullet1.GetComponent<BulletScript>().trailTail.time = 1.5f;
               // CamFollow.instance.camshakeOn = true;
             //   weapon.GetComponent<GunManager>().shellthrow(0.8f);
                SoundManager.instance.playShootSound(3);
            }

            if (weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Smg)
            {
                secs = 0f;

                GameObject Tempbullet1 = Instantiate(bulletPrefab, weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, gunDir), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(gunDir.x, gunDir.y, 0f);
                Tempbullet1.GetComponent<BulletScript>().trailTail.startWidth = 0.3f;
                Tempbullet1.GetComponent<BulletScript>().trailTail.time = 1f;
               // CamFollow.instance.camshakeOn = true;
               // weapon.GetComponent<GunManager>().shellthrow(0.6f);
                SoundManager.instance.playShootSound(3);
            }

            if (weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Sniper)
            {
                secs = 0f;

                GameObject Tempbullet1 = Instantiate(bulletPrefab, weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, gunDir), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(gunDir.x, gunDir.y, 0f);
                Tempbullet1.GetComponent<BulletScript>().trailTail.startWidth = 1f;
                Tempbullet1.GetComponent<BulletScript>().trailTail.time = 2f;
               // CamFollow.instance.camshakeOn = true;
               // weapon.GetComponent<GunManager>().shellthrow(1.2f);
                SoundManager.instance.playShootSound(4);
            }

        }


        if (secs > RanVal && enemyStatus == enemeyTypes.kattadhurai && coconut==null && player.activeSelf == true)
        {
            secs = 0f;
            //otherHand.transform.rotation =Quaternion.Euler(0f, 0f, -ThrowableHandAngle.x);//leftHand
            // hand.transform.rotation = Quaternion.Euler(0f, 0f, -ThrowableHandAngle.y);//RightHand
            // ThrowableObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            // ThrowableObject.GetComponent<Rigidbody2D>().velocity += new Vector2(gunDir.x,gunDir.y) * 100f * Time.deltaTime;

            // ThrowableObject.GetComponent<Rigidbody2D>().AddForce( gunDir *  100f, ForceMode2D.Impulse);

            coconut = Instantiate(ThrowableObject,handPos.transform.position,Quaternion.identity,gameObject.transform.parent);
            coconut.GetComponent<throwableObjects>().throwableType = ThrowableObject.name;
            coconut.name = "coconut";
            throwing(coconut);
        }
    }



    public void throwing(GameObject cocnutobj)
    {
        otherHand.transform.rotation = Quaternion.Euler(0f, 0f, -ThrowableHandAngle.x);//leftHand
        hand.transform.rotation = Quaternion.Euler(0f, 0f, -ThrowableHandAngle.y);//RightHand

        //otherHand.transform.rotation = Quaternion.Euler(0f,0f,0f);//leftHand
       // hand.transform.rotation = Quaternion.Euler(0f,0f,0f);//RightHand


        cocnutobj.GetComponent<throwableObjects>().target = player;
        cocnutobj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        cocnutobj.GetComponent<Rigidbody2D>().velocity += new Vector2(gunDir.x, gunDir.y + 0.5f) * 100f;
        StartCoroutine(enableCertainBool(0.1f, cocnutobj, "collider"));
    }


    public IEnumerator enableCertainBool(float delayTime,GameObject item,string componentThatNeedsEnabling)
    {
        yield return new WaitForSeconds(delayTime);
        if (componentThatNeedsEnabling == "collider" && item!=null)
        {
            item.GetComponent<Collider2D>().enabled = true;
        }
    }
    
   
}
