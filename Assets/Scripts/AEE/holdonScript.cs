using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdonScript : MonoBehaviour
{
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
        if(collision.gameObject.tag == "Body")
        {
            Debug.Log("collision check with obstacles--" + collision.gameObject.name);
            FinalAnimTest.instance.rb.velocity = Vector2.zero;
        }
    }
}
