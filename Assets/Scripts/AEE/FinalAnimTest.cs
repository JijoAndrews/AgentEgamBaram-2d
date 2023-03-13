using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalAnimTest : MonoBehaviour
   
{
    public GameObject agent,head,rightFoot,leftFoot,bulletPrefab,Weapon, defaultHand, MovHand, gunPos, tempobj,ragdollPrefab,RagDollAgent, bloodPart;
    public Animator playerAnimator;
    public Rigidbody2D rb;
    public LayerMask objectMask;
    public Vector3 agentPos,agentScale;
    public Quaternion agentRot;
    public float agentSpeed, agentJumpSpeed, currntSpeedDir, testspeed, slowdownLength, slowdownFactor, secs,milliSecs;
    public int weaponId;
    public bool isSliding, IsMoving, isjumping, isontheground, isUnderRoll, completed, starttoRoll, starttoRay, isObjectPickedUp, bullettimerOn,pickup,EnableThePower,isDead;
    public List<HingeJoint2D> bodyPartJoints;
    public List<GameObject> weaponstoUse;
    public List<Vector3> weapoanfloatSize;
    public static FinalAnimTest instance;
    Vector3 dist1;
   
    // Start is called before the first frame update
    void Start()
    {
        instance = this;     
    }

    public void setWeaponinHands()
    {
        if (weaponId != 0f)
        {
            if (weaponId <= 4)
            {
                Destroy(Weapon);
                GameObject tempGun = Instantiate(weaponstoUse[weaponId], gunPos.transform.position, Quaternion.identity, gunPos.transform.parent);
                tempGun.name = weaponstoUse[weaponId].name;
                tempGun.transform.localScale = weapoanfloatSize[weaponId];
                Weapon = tempGun;

                //RagDollAgent.GetComponent<PlayerHit>().curWeapon = tempGun;

                defaultHand.SetActive(false);
                MovHand.SetActive(true);
                Weapon.GetComponent<GunManager>().isrotateStarted = false;

                if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Knife)
                {
                    Weapon.transform.position = gunPos.transform.position;
                    //Weapon.GetComponent<Rigidbody2D>().simulated = true;
                    //Weapon.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

                    Weapon.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.1f);
                    Weapon.GetComponent<BoxCollider2D>().offset = new Vector2(0, 60);

                }
                else
                {
                    Weapon.transform.position = gunPos.transform.position;
                    Weapon.GetComponent<Rigidbody2D>().simulated = false;
                }
              
                Weapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                Weapon.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sign(Weapon.transform.localScale.x) * -90f);
                Weapon.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Weapon.GetComponent<GunManager>().IntheHand = true;

                //isObjectPickedUp = true;
                Invoke("enableShoot", 0.1f);
            }


        }

    }

    public void enableShoot()
    {
        isObjectPickedUp = true;

    }

    void undoSlowmotion()
    {
        Time.timeScale += (1f /slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    void slowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));
        agentPos = transform.position;
        agentRot = agent.transform.rotation;
        agentScale = transform.localScale;

        tempobj.transform.position = mousePos;
        dist1 = (mousePos - MovHand.transform.position).normalized;
        MovHand.transform.rotation = Quaternion.LookRotation(Vector3.forward, -dist1);
        Debug.DrawLine(MovHand.transform.position, tempobj.transform.position, Color.cyan);
        isontheground = isOntheGround();
       
        if(!MenuManager.instance.OnmenuPage)
        {
            jumpingMovement(1);
            playerMovement();
        }
       

        if (starttoRay)
        {
            CheckWhileOfftheGround(head);
        }

        if (Input.GetMouseButtonUp(0) && isObjectPickedUp && Weapon && !MenuManager.instance.OnmenuPage && Weapon.name !="smggun" && Weapon.name!="Ak47Gun")
        {

            if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Pistol && !bullettimerOn)
            {
                bullettimerOn = true;
                GameObject Tempbullet1 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, dist1), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y, 0f);
                Tempbullet1.GetComponent<BulletScript>().trailTail.startWidth = 0.2f;
                Tempbullet1.GetComponent<BulletScript>().trailTail.time = 1f;
                CamFollow.instance.camshakeOn = true;
                Weapon.GetComponent<GunManager>().shellthrow(0.5f);
                SoundManager.instance.playShootSound(1);
            }


            if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.SliencedPistol && !bullettimerOn)
            {
                bullettimerOn = true;
                GameObject Tempbullet1 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, dist1), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y, 0f);
                Tempbullet1.GetComponent<BulletScript>().trailTail.startWidth = 0.2f;
                Tempbullet1.GetComponent<BulletScript>().trailTail.time = 1f;
                CamFollow.instance.camshakeOn = true;
                Weapon.GetComponent<GunManager>().shellthrow(0.5f);
                SoundManager.instance.playShootSound(10);
            }

            if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Shotgun && !bullettimerOn)
            {
                bullettimerOn = true;
                GameObject Tempbullet3 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, new Vector3(dist1.x + 0.1f, dist1.y + 0.1f, 0f)), transform.parent);
                Tempbullet3.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y + 0.2f, 0f);

                GameObject Tempbullet = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, new Vector3(dist1.x + 0.1f, dist1.y + 0.1f, 0f)), transform.parent);
                Tempbullet.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y + 0.1f, 0f);

                GameObject Tempbullet1 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, dist1), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y, 0f);


                GameObject Tempbullet2 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, new Vector3(dist1.x - 0.1f, dist1.y - 0.1f, 0f)), transform.parent);
                Tempbullet2.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y - 0.1f, 0f);

                GameObject Tempbullet4 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, new Vector3(dist1.x + 0.1f, dist1.y + 0.1f, 0f)), transform.parent);
                Tempbullet4.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y - 0.2f, 0f);
                CamFollow.instance.camshakeOn = true;

                Weapon.GetComponent<GunManager>().shellthrow(1f);
                SoundManager.instance.playShootSound(0);

            }

            if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Aks && !bullettimerOn)
            {
                bullettimerOn = true;
                GameObject Tempbullet1 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, dist1), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y, 0f);
                Tempbullet1.GetComponent<BulletScript>().trailTail.startWidth = 0.5f;
                Tempbullet1.GetComponent<BulletScript>().trailTail.time = 1.5f;
                CamFollow.instance.camshakeOn = true;
                Weapon.GetComponent<GunManager>().shellthrow(0.8f);
                SoundManager.instance.playShootSound(3);
            }

            if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Smg && !bullettimerOn)
            {
                bullettimerOn = true;
                GameObject Tempbullet1 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, dist1), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y, 0f);
                Tempbullet1.GetComponent<BulletScript>().trailTail.startWidth = 0.3f;
                Tempbullet1.GetComponent<BulletScript>().trailTail.time = 1f;
                CamFollow.instance.camshakeOn = true;
                Weapon.GetComponent<GunManager>().shellthrow(0.6f);
                SoundManager.instance.playShootSound(3);
            }

            if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Sniper && !bullettimerOn)
            {
                bullettimerOn = true;
                GameObject Tempbullet1 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, dist1), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y, 0f);
                Tempbullet1.GetComponent<BulletScript>().trailTail.startWidth = 1f;
                Tempbullet1.GetComponent<BulletScript>().trailTail.time = 2f;
                CamFollow.instance.camshakeOn = true;
                Weapon.GetComponent<GunManager>().shellthrow(1.2f);
                SoundManager.instance.playShootSound(4);
            }
        }


        if((Input.GetMouseButton(0) && isObjectPickedUp && Weapon && !MenuManager.instance.OnmenuPage && Weapon.name != "smggun") ||(Input.GetMouseButton(0) && isObjectPickedUp && Weapon && !MenuManager.instance.OnmenuPage && Weapon.name != "Ak47Gun"))
        {

            if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Smg && !bullettimerOn)
            {
                bullettimerOn = true;
                GameObject Tempbullet1 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, dist1), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y, 0f);
                Tempbullet1.GetComponent<BulletScript>().trailTail.startWidth = 0.3f;
                Tempbullet1.GetComponent<BulletScript>().trailTail.time = 1f;
                CamFollow.instance.camshakeOn = true;
                Weapon.GetComponent<GunManager>().shellthrow(0.6f);
                SoundManager.instance.playShootSound(3);
            }



            if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Aks && !bullettimerOn)
            {
                bullettimerOn = true;
                GameObject Tempbullet1 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, dist1), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y, 0f);
                Tempbullet1.GetComponent<BulletScript>().trailTail.startWidth = 0.5f;
                Tempbullet1.GetComponent<BulletScript>().trailTail.time = 1.5f;
                CamFollow.instance.camshakeOn = true;
                Weapon.GetComponent<GunManager>().shellthrow(0.8f);
                SoundManager.instance.playShootSound(3);
            }

        }









            if (Input.GetKey(KeyCode.E) && !MenuManager.instance.OnmenuPage)
        {
            GetComponent<CircleCollider2D>().enabled = true;
            pickup = true;
            SoundManager.instance.playmovementSound(4,"PickUp", 0.1f);

        }
        else
        {
            GetComponent<CircleCollider2D>().enabled = false;
            pickup = false;
        }



        if (Input.GetMouseButtonUp(1) && Weapon != null && isObjectPickedUp && !MenuManager.instance.OnmenuPage)
        {

            if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Knife)
            {
                Weapon.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                Weapon.GetComponent<BoxCollider2D>().offset = new Vector2(0,0);
                Weapon.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
           // Weapon.transform.SetParent(gameObject.transform.parent);//the og transform
            Weapon.transform.SetParent(MenuManager.instance.curlevelObj.transform);

            Weapon.GetComponent<GunManager>().IntheHand = false;
            Weapon.GetComponent<GunManager>().isrotateStarted = true;
            Weapon.GetComponent<GunManager>().rotateDirection = Vector3.back;
            Weapon.GetComponent<Rigidbody2D>().simulated = true;
            Weapon.GetComponent<Rigidbody2D>().AddForce(new Vector2(dist1.x, dist1.y) * 100f, ForceMode2D.Impulse);
            //gun.GetComponent<Rigidbody2D>().AddTorque(50f,ForceMode2D.Impulse);
            isObjectPickedUp = false;

            Weapon = null;

            //RagDollAgent.GetComponent<PlayerHit>().curWeapon = null;


            //RagDollAgent.GetComponent<PlayerHit>().curWeapon = null;


            MovHand.SetActive(false);
             defaultHand.SetActive(true);
            SoundManager.instance.playShootSound(5);


        }


        if (bullettimerOn && Weapon!=null && !MenuManager.instance.OnmenuPage)
        {
            bulletTimeCheck(Weapon.GetComponent<GunManager>().bulletTimerval);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            //RagDollAgent.transform.position = agentPos;
            //RagDollAgent.transform.rotation = agentRot;
            //RagDollAgent.SetActive(true);
            //RagDollAgent.GetComponent<PlayerHit>().gothit();
            //gameObject.SetActive(false);

            //Enemy.instance.gothitBySlide();
            //dropDead();


        }

        checkfootPos();
    }



    public void dropDead(string deathType)
    {

        if (!isDead)
        {
            RagDollAgent = Instantiate(ragdollPrefab, transform.parent);//added to check to restart player as new
            RagDollAgent.name = "AgentRagdoll";//added to check to restart player as new

            RagDollAgent.transform.position = agentPos;
            RagDollAgent.transform.rotation = agentRot;
            RagDollAgent.transform.localScale = agentScale;

            RagDollAgent.SetActive(true);

            if (Weapon != null)
            {
                RagDollAgent.GetComponent<PlayerHit>().curWeapon = Weapon;
            }
            //  Destroy(Weapon);
            //RagDollAgent.GetComponent<PlayerHit>().gothit(bodyPart);
            //GameObject bullpart = Instantiate(bloodPart, transform.position, Quaternion.identity, RagDollAgent.transform.parent);
            //RagDollAgent.AddComponent<Rigidbody2D>();
            //RagDollAgent.GetComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Dynamic;
            //RagDollAgent.GetComponent<Rigidbody2D>().gravityScale = 1f;

            if (deathType == "Bomb")
            {
                //GameObject bullpart = Instantiate(bloodPart,RagDollAgent.transform.position,Quaternion.identity,RagDollAgent.transform);
                RagDollAgent.GetComponent<PlayerHit>().bodyParts[5].GetComponent<Rigidbody2D>().AddForce(new Vector2(-dist1.x, -dist1.y + 10f) * 100f, ForceMode2D.Impulse);
                RagDollAgent.GetComponent<PlayerHit>().bodyParts[5].GetComponent<Rigidbody2D>().AddTorque(50f * 150f, ForceMode2D.Impulse);
            }

            isDead = true;
            gameObject.SetActive(false);

            MenuManager.instance.deadPage.SetActive(true);//enable once video recording is done
            MenuManager.instance.blackBg.SetActive(true);//--------------this also------------
            SoundManager.instance.playShootSound(13);
        }
    }


    public void playerMovement()
    {
        if (Input.GetKey(KeyCode.A) && !isSliding)
        {  
            IsMoving = true;
            playerAnimator.SetBool("Idle", false);
            playerAnimator.SetBool("startMov", true);
            rb.velocity = new Vector2(-agentSpeed, rb.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey(KeyCode.D) && !isSliding)
        {
          
            IsMoving = true;
            playerAnimator.SetBool("Idle", false);
            playerAnimator.SetBool("startMov", true);
            rb.velocity = new Vector2(agentSpeed, rb.velocity.y);
            rb.AddForce(dist1 * agentSpeed);

            transform.localScale = new Vector3(1, 1, 1);          
        }

        currntSpeedDir = rb.velocity.x;
        testspeed = Mathf.Abs(currntSpeedDir);

        if (Input.GetKey(KeyCode.S) && currntSpeedDir != 0 && !isjumping && !isUnderRoll)//the underroll is added recently for sliding bug
        {
           
            IsMoving = true;
           // rb.velocity = new Vector2(currntSpeedDir, rb.velocity.y);
   
            rb.velocity = new Vector2(currntSpeedDir, rb.velocity.y);
            playerAnimator.SetBool("Slide", isOntheGround());
            playerAnimator.SetBool("startMov", false);
            playerAnimator.SetBool("Idle", false);
            // SoundManager.instance.playShootSound(7);
            //SoundManager.instance.playerMovemntSoundSimple(5);
            SoundManager.instance.playmovementSound(5,"SlideAway", 0.1f);
            isSliding = true;


        }






        if (!Input.anyKey  && isOntheGround())
        {
            playerAnimator.SetBool("startMov", false);
            //playerAnimator.SetBool("startJump", false);
            playerAnimator.SetBool("Slide", false);
            playerAnimator.SetBool("Idle", true);
            isSliding = false;
            IsMoving = false;
            decreseSpeed();

        }



        if (!Input.anyKey && !isOntheGround())
        {
          
            IsMoving = false;
           // decreseSpeed();
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("TheUnderRoll"))
        {
            playerAnimator.SetBool("startJump", false);
            isjumping = false;

        }

        isUnderRoll = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("wings") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("TheUnderRoll");



        if (!isSliding)
        {
            undoSlowmotion();
        }



        if (!isSliding && !IsMoving)
        {
            slowMotion();
        }


        if (isSliding && IsMoving)
        {
            if(Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D))
            {
                CamFollow.instance.IsZoomedIn = false;
                CamFollow.instance.camshakeOn = true;
                undoSlowmotion();

            }
            else
            {
                slowMotion();

            }

           // slowMotion();
        }

    }

    public void decreseSpeed()
    {
        rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(0, rb.velocity.y), Time.deltaTime * 2f);
        if (testspeed < 1f)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }


    public bool isOntheGround()
    {
        Ray2D ray1 = new Ray2D(agent.transform.position, Vector2.down);
        RaycastHit2D rayhit = Physics2D.Raycast(ray1.origin, ray1.direction, 20f, objectMask);

        if (rayhit)
        {
            Debug.DrawRay(ray1.origin, ray1.direction * 20f, Color.yellow);
        }
        else
        {
            Debug.DrawRay(ray1.origin, ray1.direction * 20f, Color.red);
        }

        return rayhit;
    }




    public bool CheckWhileOfftheGround(GameObject Startpoint)
    {
        Ray2D ray1;

        if (Startpoint.name!="Head")
        {
             ray1 = new Ray2D(Startpoint.transform.position, Vector2.left);

        }
        else
        {
             ray1 = new Ray2D(Startpoint.transform.position, Vector2.down);

        }

        RaycastHit2D rayhit = Physics2D.Raycast(ray1.origin, ray1.direction, 20f, objectMask);

        if (rayhit)
        {
            Debug.DrawRay(ray1.origin, ray1.direction * 20f, Color.green);
            //starttoRoll = true;
            starttoRay = false;
           // playerAnimator.SetBool("startRoll",true);
            playerAnimator.SetTrigger("roll");
        }
        else
        {
            Debug.DrawRay(ray1.origin, ray1.direction * 20f, Color.blue);
        }

        return rayhit;
    }



    public void jumpingMovement(int type)
    {

        if (Input.GetKeyDown(KeyCode.Space) && !isUnderRoll && isOntheGround())
        {
            isSliding = false;
            // IsMoving = false;
            isjumping = true;
            Debug.Log("jumped");
            playerAnimator.SetBool("Slide", false);
            playerAnimator.SetBool("Idle", false);
            playerAnimator.SetBool("startJump", true);
            playerAnimator.SetBool("startMov", false);
            rb.velocity += Vector2.up * agentJumpSpeed;//og
            SoundManager.instance.playerMovemntSoundSimple(1);
            Invoke("startAdelayforRay", 0.5f);
        }
    }



    public void startAdelayforRay()
    {
        starttoRay = true;
    }

    public void bulletTimeCheck(float checkSecs)
    {
        secs += 0.001f;
        if (secs > checkSecs)
        {
            secs = 0f;
            bullettimerOn = false;
        }
        else
        {
            bullettimerOn = true;
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickup && collision.gameObject.tag == "Gun" && !isObjectPickedUp && !Weapon && collision.GetComponent<GunManager>().isinEnemeyHand == false)//collision.gameObject.name == "gun" 
        {
            MovHand.SetActive(true);
            defaultHand.SetActive(false);

            Weapon = collision.gameObject;

            //RagDollAgent.GetComponent<PlayerHit>().curWeapon = Weapon;
            // Debug.Log("the weapon - " + Weapon.name + "- is present");

            Weapon.GetComponent<GunManager>().isrotateStarted = false;
            if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Knife)
            {
                Weapon.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.1f);
                Weapon.GetComponent<BoxCollider2D>().offset = new Vector2(0, 60);
                Weapon.transform.position = gunPos.transform.position;
            }
            else
            {
                Weapon.transform.position = gunPos.transform.position;
            }
            // Weapon.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            Weapon.transform.SetParent(gunPos.transform.parent);
            Weapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            Weapon.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sign(Weapon.transform.localScale.x) * -90f);
            Weapon.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Weapon.GetComponent<Rigidbody2D>().simulated = false;
            Weapon.GetComponent<GunManager>().IntheHand = true;
            isObjectPickedUp = true;

            weaponId = (int)Weapon.GetComponent<GunManager>().GunType;
        }
        else
        {
            if (isObjectPickedUp && Weapon)
            {
                // Debug.Log("Gun present");
            }
        }

        if(collision.gameObject.tag == "victoryBox")
        {
            MenuManager.instance.OnmenuPage = true;
            MenuManager.instance.enableBtnAction("victory");
            rb.velocity = new Vector2(0f, 0f);
        }


        if (collision.gameObject.tag == "DeathBar")
        {
            MenuManager.instance.OnmenuPage = true;
            rb.velocity = new Vector2(0f, 0f);
            dropDead("Bomb");
           // SoundManager.instance.playShootSound(13);
        }
    }


    public void gunHandle( Collider2D collision)
    {
        if (pickup && collision.gameObject.tag == "Gun" && !isObjectPickedUp && !Weapon && collision.GetComponent<GunManager>().isinEnemeyHand==false)//collision.gameObject.name == "gun" 
        {
            MovHand.SetActive(true);
            defaultHand.SetActive(false);

            Weapon = collision.gameObject;


            // Debug.Log("the weapon - " + Weapon.name + "- is present");
            Weapon.GetComponent<GunManager>().isrotateStarted = false;

            if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Knife)
            {
                Weapon.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.1f);
                Weapon.transform.position = gunPos.transform.position;
            }
            else
            {
                Weapon.transform.position = gunPos.transform.position;
            }
            // Weapon.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            Weapon.transform.SetParent(gunPos.transform);
            Weapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            Weapon.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sign(Weapon.transform.localScale.x) * -90f);
            Weapon.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Weapon.GetComponent<Rigidbody2D>().simulated = false;
            Weapon.GetComponent<GunManager>().IntheHand = true;
            isObjectPickedUp = true;
        }
        else
        {
            if (isObjectPickedUp && Weapon)
            {
                // Debug.Log("Gun present");
            }
        }
    }


   


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag!= "Ground")
        {
            // Debug.Log("check---" + collision.gameObject.name);
            //rb.velocity = new Vector2(0f,0f);
        }


        if (collision.gameObject.tag == "Gun")
        {
            // Debug.Log("check---" + collision.gameObject.name);

            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
           // gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }



        if (collision.gameObject.tag == "Bullet")
        {
            // Debug.Log("check---" + collision.gameObject.name);
            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
           // gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }


        if (collision.gameObject.tag == "Enemy")
        {

            if (collision.gameObject.transform.parent.GetComponent<Enemy>().IsDead)
            {
                Physics2D.IgnoreLayerCollision(9, 8);
            }


            if (!collision.gameObject.transform.parent.GetComponent<Enemy>().IsDead && isSliding)
            {
                Debug.Log("check-while sliding--" + collision.gameObject.name);

                rb.velocity = new Vector2(currntSpeedDir, rb.velocity.y);//slide even when hitting the Enemey
                collision.gameObject.transform.parent.GetComponent<Enemy>().gothitBySlide();
                Physics2D.IgnoreLayerCollision(9, 8);
            }

        }
    }


    public void checkfootPos()
    {
        if(!rightFoot.GetComponent<footParticle>().isFootOntheGround() && leftFoot.GetComponent<footParticle>().isFootOntheGround())
        {
           // SoundManager.instance.playmovementSound(0);
        }
        
    }

}
