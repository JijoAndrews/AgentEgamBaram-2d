using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov2 : MonoBehaviour
{
    GameObject player;
    public GameObject checkPos, agentAega;
    public Animator playerAnimator;
    public Animation playerAnim;
    public float Agentspeed, AgentJumpSpeed, bodydistFromfloor, slowdownFactor = 0.05f, slowdownLength = 2f;
    public bool isPlayerMoving, IsplayerNeartheFloor, isjumpStart, testJump;
    public Rigidbody2D rb;
    public GameObject floorobj;
    public ColliderDistance2D coldist;
    public BoxCollider2D bodyCollider;
    public LayerMask playerLayermask;
    public float val;
    void Start()
    {
        player = this.gameObject;
    }

    void undoSlowmotion()
    {
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    void slowMotion()
    {
        //Time.timeScale = slowdownFactor;
        //Time.fixedDeltaTime = Time.timeScale * .02f;
    }


    void Update()
    {

        bodydistFromfloor = Mathf.Round(Mathf.Abs(rb.velocity.y));

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {

            if (IsGrounded2())
            {
                isPlayerMoving = true;
                rb.simulated = true;
                rb.velocity = new Vector2(-Agentspeed, rb.velocity.y);
                playerAnimator.SetBool("Rolltime", false);
                playerAnimator.SetBool("Testrun", true);
            }
            else
            {

                float midaircontrol = 1f;
                rb.velocity += new Vector2(-Agentspeed * midaircontrol * Time.deltaTime, 0);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -Agentspeed, +Agentspeed), rb.velocity.y);
            }

            isPlayerMoving = true;
            rb.simulated = true;
            transform.localScale = new Vector3(-1, 1, 1);
            undoSlowmotion();



            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded2() && !testJump)
            {
                isjumpStart = true;
                playerAnimator.SetBool(" Testrun", false);
                playerAnimator.SetBool("Testjump", true);

                //rb.velocity = new Vector2(rb.velocity.x, AgentJumpSpeed);
                rb.velocity = Vector2.up * AgentJumpSpeed; //og
            }



        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
           


            if (IsGrounded2())
            {
                isPlayerMoving = true;
                rb.simulated = true;
                rb.velocity = new Vector2(Agentspeed, rb.velocity.y);
                playerAnimator.SetBool("Rolltime", false);
                playerAnimator.SetBool("Testrun", true);

            }
            else
            {

                float midaircontrol = 1f;
                rb.velocity += new Vector2(Agentspeed * midaircontrol * Time.deltaTime, 0);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -Agentspeed, +Agentspeed), rb.velocity.y);
            }

            isPlayerMoving = true;
            rb.simulated = true;
            transform.localScale = new Vector3(1, 1, 1);
            undoSlowmotion();



        }


        if (Input.GetKeyDown(KeyCode.Space) && !testJump)
        {
            isjumpStart = true;
            playerAnimator.SetBool(" Testrun", false);
            playerAnimator.SetBool("Testjump", true);
            //playerAnimator.SetBool("Rolltime", true);

            //rb.velocity = new Vector2(rb.velocity.x, AgentJumpSpeed);

            rb.velocity = Vector2.up * AgentJumpSpeed; //og


            //Invoke("jumpStars", 0.5f);
        }


        if (!Input.anyKey)
        {
            playerAnimator.SetBool("Testrun", false);
            playerAnimator.SetBool("Testjump", false);
            playerAnimator.SetBool("Rolltime", false);
            isPlayerMoving = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            slowMotion();
        }



        //if (IsGrounded2() && isjumpStart)
        //{
        //    // Debug.Log("hello");
        //    // rb.simulated = false;
        //    // playerAnimator.SetBool("Rolltime", true);

        //    isjumpStart = false;
        //}


    

        val = Mathf.Abs(rb.velocity.y);

        //if (val > 1)
        //{
        //    testJump = true;
        //    IsGrounded2();
        //    playerAnimator.SetBool("Rolltime", true);

        //    //Debug.Log("is grounding-");
        //}
        //else
        //{
        //    playerAnimator.SetBool("Testjump", false);//test
        //    testJump = false;
        //}


        IsplayerNeartheFloor = IsGrounded2();

    }
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(bodyCollider.bounds.center, bodyCollider.bounds.size, 0f, Vector2.down, 0.1f, playerLayermask);
        return raycastHit2d.collider != null;
    }



    private bool IsGrounded2()
    {
        Ray2D ray1 = new Ray2D(bodyCollider.transform.position, Vector2.down);

       RaycastHit2D rayhit = Physics2D.Raycast(agentAega.transform.position, Vector2.down, 0.1f,playerLayermask);
      


        if (rayhit)
        {
            Debug.Log("object-" + rayhit.collider.gameObject.name);
            Debug.DrawRay(ray1.origin, ray1.direction * 20, Color.yellow);

        }
        else
        {
            Debug.DrawRay(ray1.origin, ray1.direction * 20, Color.red);
        }

         IsplayerNeartheFloor = rayhit.collider != null ? true : false;
        return rayhit.collider != null;
    }


    public void jumpStars()
    {
        isjumpStart = true;
    }
}
