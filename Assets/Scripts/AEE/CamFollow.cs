using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    public GameObject followTarget;
    public Vector3 camOffset, startpos;
    public float zoomInVal, zoomOutVal;
    public AnimationCurve shakecurve;
    public float cameraSpeed, damping,curYpos,duration, strength;
    private Vector3 velocity = Vector3.zero;
    public bool camshakeOn,IsZoomedIn,Isstarted,resetPos;
    public static CamFollow instance;
    void Start()
    {
        instance = this;
    }


    private void LateUpdate()
    {
        if (camshakeOn)
        {
           // camshakeOn = false;
           // StartCoroutine(camShake());
        }
    }

    void FixedUpdate()
    {
        if (Isstarted)
        {
            curYpos = Mathf.Lerp(transform.position.y, followTarget.transform.position.y, 0.5f * Time.deltaTime);
            Vector3 desiredPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, followTarget.transform.position.z + -70f);
            Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, cameraSpeed * Time.deltaTime);
            transform.position = smoothPos;
            startpos = transform.position;
        }


        if (resetPos)
        {
            resetPos = false;
            transform.position = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, followTarget.transform.position.z + -70f);
        }

        // Vector3 desiredPos = followTarget.transform.position + camOffset;
        // Vector3 smoothPos = Vector3.Lerp(transform.position,desiredPos, 0.125f);
        //  transform.LookAt(followTarget.transform);
        // transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, damping);
        //Vector3 newpos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, -796f);
        // transform.position = Vector3.Slerp(transform.position, newpos, cameraSpeed * Time.deltaTime);


    }

    private void Update()
    {

        // ZoomminAndOut("In");


        if (Isstarted)
        {

            if (FinalAnimTest.instance.IsMoving)
            {
                ZoomminAndOut("Out");
                IsZoomedIn = false;
            }
            else
            {
                ZoomminAndOut("In");
                IsZoomedIn = true;
            }

            if (camshakeOn)
            {
                ZoomminAndOut("Shoot");

            }


        }
            
    }

    public IEnumerator camShake()
    {
        float elaspedTime = 0f;

        while(elaspedTime< duration)
        {
            elaspedTime += Time.deltaTime;
            strength = shakecurve.Evaluate(elaspedTime / duration);
            transform.position = startpos + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startpos;
    }


    public void ZoomminAndOut(string stat)
    {
        if (stat == "In")
        {
            gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(gameObject.GetComponent<Camera>().orthographicSize, zoomInVal, 0.5f * Time.deltaTime);
            if (!IsZoomedIn)
            {
                // SoundManager.instance.playmovementSound(2, "ZoomIn",0.1f);
                if (SoundManager.instance.zoomSound)
                {
                    SoundManager.instance.zoomSound.GetComponent<AudioSource>().Stop();
                }
                SoundManager.instance.playmovementSound(2, "ZoomIn", 0.1f);

            }
        }

        if (stat == "Out")
        {
            gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(gameObject.GetComponent<Camera>().orthographicSize, zoomOutVal, 1f * Time.deltaTime);
            if (IsZoomedIn)
            {
                if (SoundManager.instance.zoomSound)              
                {
                    SoundManager.instance.zoomSound.GetComponent<AudioSource>().Stop();
                } 
                
                    SoundManager.instance.playmovementSound(3, "ZoomIn",0.1f);
            }
        }


        if (stat == "Shoot" && camshakeOn)
        {
            gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(gameObject.GetComponent<Camera>().orthographicSize, zoomOutVal, 2f * Time.deltaTime);
            if (IsZoomedIn)
            {
                //SoundManager.instance.playmovementSound(3, "ZoomIn", 0.1f);


                if (SoundManager.instance.zoomSound)
                {
                    SoundManager.instance.zoomSound.GetComponent<AudioSource>().Stop();
                }

                SoundManager.instance.playmovementSound(3, "ZoomIn", 0.1f);
            }
        }
    }
}
