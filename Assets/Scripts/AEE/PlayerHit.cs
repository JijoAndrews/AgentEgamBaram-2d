using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public Vector3 hitDir;
    public int weaponId;
    public GameObject curWeapon;
    public bool isfly;
    public List<GameObject> bodyParts, movabbleBodyParts,wepons;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void gothit( GameObject body)
    {
        if (!isfly && curWeapon != null)
        {
            GameObject tempFlyWeaspon = Instantiate(curWeapon, transform);
            tempFlyWeaspon.transform.localScale = new Vector3(1f, 1f, 1f);
            tempFlyWeaspon.GetComponent<GunManager>().weaponfly(gameObject);
            isfly = true;
            Destroy(FinalAnimTest.instance.Weapon);
        }


        if (body.gameObject.tag !="Gun")
        {
            Debug.Log("Body part which has been hit--" + body.name);
            int tempbodyId = movabbleBodyParts.IndexOf(body);
            movabbleBodyParts[tempbodyId].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            if (movabbleBodyParts[tempbodyId].GetComponent<HingeJoint2D>())
            {
                movabbleBodyParts[tempbodyId].GetComponent<HingeJoint2D>().enabled = false;
            }
            movabbleBodyParts[tempbodyId].GetComponent<Rigidbody2D>().AddForce(Vector2.up * 200f, ForceMode2D.Impulse);
        }

      
       //collision.gameObject.transform.parent.GetComponent<Enemy>().ignoreBodypart = collision.gameObject.name;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }


   
}
