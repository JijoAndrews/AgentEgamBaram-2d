using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgManager : MonoBehaviour
{

    public float lenght,starPos,paralaxEffect;
    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject cloudClone = Instantiate(cloudPrefab,frntpos.transform.position,Quaternion.identity,transform);
        //cloudClone.SetActive(true);
        starPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //camXpos = Camera.main.transform.position.x;
        //fPos = frntpos.transform.localPosition;
        //bPos = backPos.transform.localPosition;



        //if (camXpos < xlimitVal)
        //{

        //}
    }


    public void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1f - paralaxEffect));
        float dist = (cam.transform.position.x * paralaxEffect);
        transform.position = new Vector3(starPos + dist, transform.position.y, transform.position.z);

        if (temp > starPos + lenght)
        {
            starPos += lenght;
        }
        else if (temp < starPos - lenght)
        {
            starPos -= lenght;
        }
    }
}
