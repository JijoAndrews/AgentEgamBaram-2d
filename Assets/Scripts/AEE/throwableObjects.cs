using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwableObjects : MonoBehaviour
{
    public string throwableType;
    public GameObject target,bloodPart,coconutPart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnCollisionEnter2D(Collision2D collision)
    {
       // Debug.Log("hits----" + collision.gameObject.name +"-------collision Layer---"+ collision.gameObject.layer +"------compare layer---" + LayerMask.NameToLayer("Player"));

        //if (collision.gameObject.layer != LayerMask.NameToLayer("platforms") && collision.gameObject.layer == LayerMask.NameToLayer("Player") && target!=null)
        //{
        //    GameObject bullpart = Instantiate(bloodPart, transform.position, Quaternion.identity, gameObject.transform.parent);
        //    GameObject coconutElement = Instantiate(coconutPart, transform.position, Quaternion.identity,gameObject.transform.parent);
        //    target.GetComponent<FinalAnimTest>().dropDead();
        //    Debug.Log("throwable object hits----" + collision.gameObject.name+"-----layer---"+ collision.gameObject.layer + "--insantiated--" + bullpart.name +"----and---" + coconutElement.name);

        //    Destroy(gameObject);
        //}


        if (throwableType == "bananaBomb" && collision.gameObject.tag == "Body")
        {
            Debug.Log("hits----" + collision.gameObject.name + "-------collision Layer---" + collision.gameObject.layer + "------compare layer---" + LayerMask.NameToLayer("Player"));
            GameObject bullpart = Instantiate(bloodPart, transform.position, Quaternion.identity, gameObject.transform.parent);
            GameObject coconutElement = Instantiate(coconutPart, transform.position, Quaternion.identity,gameObject.transform.parent);
            target.GetComponent<FinalAnimTest>().dropDead("Bomb");
            SoundManager.instance.playShootSound(8);
            Destroy(gameObject);
        }

        if (throwableType == "Throwable_coconut" && collision.gameObject.tag=="Body")
        {
            Debug.Log("hits----" + collision.gameObject.name +"-------collision Layer---"+ collision.gameObject.layer +"------compare layer---" + LayerMask.NameToLayer("Player"));
            GameObject bullpart = Instantiate(bloodPart, transform.position, Quaternion.identity, gameObject.transform.parent);
            GameObject coconutElement = Instantiate(coconutPart, transform.position, Quaternion.identity,gameObject.transform.parent);
            target.GetComponent<FinalAnimTest>().dropDead("Bomb");
            SoundManager.instance.playShootSound(8);
            Destroy(gameObject);
        }



        if ((throwableType == "Throwable_coconut" && collision.gameObject.tag == "Obstacles" )||(throwableType == "bananaBomb" && collision.gameObject.tag == "Obstacles"))
        {
            Debug.Log("hits----" + collision.gameObject.name + "-------collision Layer---" + collision.gameObject.layer + "------compare layer---" + LayerMask.NameToLayer("Player"));
            GameObject coconutElement = Instantiate(coconutPart, transform.position, Quaternion.identity, gameObject.transform.parent);
            SoundManager.instance.playShootSound(8);
            Destroy(gameObject);
        }


        if ((throwableType == "Throwable_coconut" && collision.gameObject.tag == "Bullet") || (throwableType == "bananaBomb" && collision.gameObject.tag == "EnemyBullet"))
        {
            Debug.Log("hits----" + collision.gameObject.name + "-------collision Layer---" + collision.gameObject.layer + "------compare layer---" + LayerMask.NameToLayer("Player"));
            GameObject coconutElement = Instantiate(coconutPart, transform.position, Quaternion.identity, gameObject.transform.parent);
            SoundManager.instance.playShootSound(8);
            Destroy(gameObject);
        }

    }
}
