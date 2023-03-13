using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{

    public float killTime;
    public string shelltype;
    public int shellId;
    
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == "Shell")
        {
            gameObject.layer = LayerMask.NameToLayer("Dead");
        }
        Destroy(gameObject, killTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Body")
        {
            //Physics2D.IgnoreLayerCollision(5, 8);
            // Debug.Log("shell is colliding--" + collision.gameObject.name);
             Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());

        }



        if (collision.gameObject.tag == "Body" && gameObject.name == "Shell")
        {
            // Physics2D.IgnoreLayerCollision(5, 8);
             Physics2D.IgnoreLayerCollision(8, 9);

            // Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
            // Debug.Log("shell is colliding--" + collision.gameObject.name);
             //Debug.Log("shell is colliding--" + collision.gameObject.name);

        }

        if (collision.gameObject.tag == "Gun")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
        }



        //if (collision.gameObject.tag == "Ground" && shelltype != null)
        //{
        //    SoundManager.instance.playShellSound(shellId);
        //}
    }
}
