using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simplejumpcontroller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject agent,Head,MovHand,defaultHand,tempobj, tempobj1, Weapon,gunPos, bulletPrefab;
    public Rigidbody2D rb;
    public Animator playerAnimator;
    public LayerMask objectMask;
    public Vector2 theVelocity;
    public Vector3 dist1;
    public float agentSpeed,agentJumpSpeed,nTime,currntSpeedDir,testspeed,secs,bulletMinTime,Angle,dist, slowdownFactor = 0.05f, slowdownLength = 2f;
    public bool isGrounded,isStartJump,isHeadonGround,isUnderRoll,isIdle,isSliding,isObjectPickedUp,bullettimerOn,IsMoving;
    public List<GameObject> gunsAvailable;

    public static Simplejumpcontroller instance;
    AnimatorStateInfo animaStateInfo;
    float val;
    void Start()
    {
        instance = this;
        rb = gameObject.GetComponent<Rigidbody2D>();
        animaStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        nTime = animaStateInfo.normalizedTime;

    }


    void undoSlowmotion()
    {
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
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

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3( Input.mousePosition.x, Input.mousePosition.y,100f));
        tempobj.transform.position = mousePos;
       // tempobj1.transform.position = mousePos + new Vector3(0f,3f,0f);

        dist1 = (mousePos- MovHand.transform.position).normalized;
        Angle = Mathf.Round(Mathf.Atan2(dist1.y, dist1.x) * Mathf.Rad2Deg);//og
        MovHand.transform.rotation = Quaternion.LookRotation(Vector3.forward,-dist1);
        Debug.DrawLine(MovHand.transform.position, tempobj.transform.position, Color.cyan);
        theVelocity = new Vector2(Mathf.Abs(rb.velocity.x),Mathf.Round( Mathf.Abs(rb.velocity.y)));
        isGrounded = isOntheGround();
        isStartJump = !isOntheGround();
        isHeadonGround= beamFromHead();

        jumpingMovement(1);
        playerMovement();
        playerAnimator.SetBool("startRoll", isHeadonGround);


        //if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("startMov"))
        //{
        //    // Avoid any reload.
        //}


        if (Input.GetMouseButtonUp(0) && isObjectPickedUp && Weapon)
        {

            if(Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Pistol && !bullettimerOn)
            {
                bullettimerOn = true;               
                GameObject Tempbullet1 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, dist1), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y, 0f);           
            }


            if (Weapon.GetComponent<GunManager>().GunType == GunManager.gunTypes.Shotgun && !bullettimerOn)
            {
                bullettimerOn = true;

                GameObject Tempbullet = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, new Vector3(dist1.x + 0.1f, dist1.y + 0.1f, 0f)), transform.parent);
                Tempbullet.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y + 0.1f, 0f);

                GameObject Tempbullet1 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, dist1), transform.parent);
                Tempbullet1.GetComponent<BulletScript>().Direction = new Vector3(dist1.x, dist1.y, 0f);


                GameObject Tempbullet2 = Instantiate(bulletPrefab, Weapon.GetComponent<GunManager>().bulletPos.transform.position, Quaternion.LookRotation(Vector3.forward, new Vector3(dist1.x - 0.1f, dist1.y - 0.1f, 0f)), transform.parent);
                Tempbullet2.GetComponent<BulletScript>().Direction = new Vector3(dist1.x , dist1.y - 0.1f, 0f);
            }
        }





        if (Input.GetKey(KeyCode.E))
        {
            GetComponent<CircleCollider2D>().enabled = true;
        }
        else 
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }



        if (Input.GetMouseButtonUp(1) && Weapon!=null && isObjectPickedUp)
        {


            Weapon.transform.SetParent(gameObject.transform.parent);
            Weapon.GetComponent<GunManager>().IntheHand = false;
            Weapon.GetComponent<GunManager>().isrotateStarted = true;
            Weapon.GetComponent<GunManager>().rotateDirection = Vector3.back;
            Weapon.GetComponent<Rigidbody2D>().simulated = true;
            Weapon.GetComponent<Rigidbody2D>().AddForce(new Vector2(dist1.x, dist1.y) * 100f, ForceMode2D.Impulse);
            //gun.GetComponent<Rigidbody2D>().AddTorque(50f,ForceMode2D.Impulse);
            isObjectPickedUp = false;
            Weapon = null;

            // MovHand.SetActive(false);
            //  defaultHand.SetActive(true);
        }





        if (bullettimerOn)
        {
            bulletTimeCheck();
        }
    }


    public bool isOntheGround()
    {
        Ray2D ray1 = new Ray2D(agent.transform.position, Vector2.down);
        RaycastHit2D rayhit = Physics2D.Raycast(ray1.origin, ray1.direction, 20f,objectMask);

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

    public void bulletTimeCheck()
    {
        secs += 0.01f;
        if (secs>4f)
        {
            secs = 0f;
            bullettimerOn = false;
        }
        else
        {
            bullettimerOn = true;
        }
    }

    public void playerMovement()
    {
        if (Input.GetKey(KeyCode.A) && !isSliding)
        {
            IsMoving = true;
            isIdle = false;
            rb.velocity = new Vector2(-agentSpeed, rb.velocity.y);
            transform.localScale = new Vector3(-1,1,1);
            playerAnimator.SetBool("Idle", false);
            playerAnimator.SetBool("startMov", isOntheGround());
            // currntSpeedDir = rb.velocity.x;
            undoSlowmotion();

        }

        if (Input.GetKey(KeyCode.D) && !isSliding)
        {
            IsMoving = true;
            isIdle = false;
            rb.velocity = new Vector2(agentSpeed, rb.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
            playerAnimator.SetBool("Idle", false);
            playerAnimator.SetBool("startMov", isOntheGround());
            // currntSpeedDir = rb.velocity.x;
            undoSlowmotion();

        }

        currntSpeedDir = rb.velocity.x;
        testspeed = Mathf.Abs(currntSpeedDir);

        if (Input.GetKey(KeyCode.S) && currntSpeedDir != 0)
        {
            IsMoving = false;
            isIdle = false;
            isSliding = true;
            //if (currntSpeedDir!=0)
            //{
            rb.velocity = new Vector2(currntSpeedDir, rb.velocity.y);
            //}
            // transform.localScale = new Vector3(-1, 1, 1);
            playerAnimator.SetBool("Idle", false);
            playerAnimator.SetBool("Slide", isOntheGround());
            slowMotion();
        }



        if (Input.GetKey(KeyCode.X) && Weapon)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            Weapon.GetComponent<Rigidbody2D>().simulated = true;
            Weapon.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 250f);
           // gun.GetComponent<Rigidbody2D>().AddTorque(50f);
            Weapon.transform.SetParent(gameObject.transform.parent);
           // gun = null;
            isObjectPickedUp = false;
        }

        if (!Input.anyKey && isOntheGround() && !isIdle)
        {
            playerAnimator.SetBool("startMov", false);
            playerAnimator.SetBool("startJump", false);
            playerAnimator.SetBool("Slide",false);
            playerAnimator.SetBool("Idle", true);
            isSliding = false;
            IsMoving = false;
            decreseSpeed();
            slowMotion();
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("TheUnderRoll"))
            {
               playerAnimator.SetBool("startJump", false);

            }

           isUnderRoll = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("floating") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("TheUnderRoll");

    }



    public void jumpingMovement(int type)
    {

        if (Input.GetKey(KeyCode.Space) && isOntheGround() && !isUnderRoll)
        {
            isIdle = false;
            playerAnimator.SetBool("startJump", true);
            playerAnimator.SetBool("Slide", false);
            playerAnimator.SetBool("startMov", false);
          //Debug.Log("playerJump-" + type);
            rb.velocity = Vector2.up * agentJumpSpeed;//og

        }
    }


    public bool beamFromHead()
    {
        Ray2D ray1 = new Ray2D(Head.transform.position, Vector2.down);
        RaycastHit2D rayhit = Physics2D.Raycast(ray1.origin, ray1.direction, 20f, objectMask);

        if (rayhit)
        {
            //Debug.Log("object-" + rayhit.collider.gameObject.name);
            Debug.DrawRay(ray1.origin, ray1.direction * 20f, Color.green);
        }
        else
        {
            Debug.DrawRay(ray1.origin, ray1.direction * 20f, Color.blue);
        }

        return rayhit;
    } 


    public void decreseSpeed()
    {
        rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(0, rb.velocity.y), Time.deltaTime * 2f);
        if (testspeed<1f)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            isIdle = true;
            //Debug.Log("checking before-" + isIdle);
        }
    }



    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name == "gun" && !isObjectPickedUp &&!Weapon)
    //    {
    //        //MovHand.SetActive(true);
    //        //defaultHand.SetActive(false);

    //        Weapon = collision.gameObject;
    //       // Debug.Log("the weapon - " + Weapon.name + "- is present");
    //        Weapon.GetComponent<GunManager>().isrotateStarted = false;
    //        Weapon.transform.position = gunPos.transform.position;
    //        Weapon.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    //        Weapon.transform.SetParent(gunPos.transform);
    //        Weapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    //        Weapon.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sign(Weapon.transform.localScale.x) * -90f);
    //        Weapon.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //        Weapon.GetComponent<Rigidbody2D>().simulated = false;
    //        Weapon.GetComponent<GunManager>().IntheHand = true;
    //        isObjectPickedUp = true;





    //        //collision.gameObject.GetComponent<GunManager>().isrotateStarted = false;
    //        //collision.gameObject.transform.position = gunPos.transform.position;
    //        //collision.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    //        //collision.gameObject.transform.SetParent(gunPos.transform);
    //        //collision.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    //        //collision.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sign(collision.gameObject.transform.localScale.x) * -90f);
    //        //collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //        //collision.gameObject.GetComponent<Rigidbody2D>().simulated = false;
    //        //collision.gameObject.GetComponent<GunManager>().IntheHand = true;
    //        //Weapon = collision.gameObject;
    //        //Debug.Log("the weapon - " + Weapon.name + "- is present");
    //        //isObjectPickedUp = true;
    //    }
    //    else
    //    {
    //        if (isObjectPickedUp && Weapon)
    //        {
    //           // Debug.Log("Gun present");
    //        }
    //    }
    //}

    public void OnCollisionEnter2D(Collision2D collision)
    {
       // Debug.Log("check---" + collision.gameObject.name);

        if (collision.gameObject.tag == "Gun")
        {
           // Debug.Log("check---" + collision.gameObject.name);
            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }


        if (collision.gameObject.tag == "Enemy")
        {

            if (collision.gameObject.transform.parent.GetComponent<Enemy>().IsDead)
            {
                Debug.Log("check---" + collision.gameObject.name);
                Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            
        }


        if (collision.gameObject.tag == "Body")
        {
            //Debug.Log("check---" + collision.gameObject.name);
            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

}
