using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simplebulletMov : MonoBehaviour
{
    // Start is called before the first frame update
    public TrailRenderer trailsRen;
    public Vector3 starpos,moveDirection;
    public float dist,speedofBullet,targetVal,xVal,curXval,targetPos;
    void Start()
    {
       // dist = 0f;
        starpos = transform.localPosition;

         //xVal = Mathf.Round(transform.position.x);
         targetPos = transform.localPosition.x + xVal;
         //dist = Mathf.Abs(Vector3.Distance(transform.position, starpos));

    }

    // Update is called once per frame
    void Update()
    {
        dist = Mathf.Round( Mathf.Abs(Vector3.Distance(transform.localPosition, starpos)));

        //if (dist > targetVal)
        //{
        //   // trailsRen.enabled = true;
        //    transform.Translate(moveDirection * Time.deltaTime * speedofBullet);
        //}

        curXval = transform.localPosition.x;

        if (transform.localPosition.x <= targetPos)
        {
            transform.Translate(moveDirection * Time.deltaTime * speedofBullet);
        }
    }
}
