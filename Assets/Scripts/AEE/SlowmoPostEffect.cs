using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class SlowmoPostEffect : MonoBehaviour
{
    // Start is called before the first frame update

    public PostProcessLayer curLayer;
    public PostProcessVolume curVol;
    public PostProcessProfile pp;
    private ChromaticAberration ppCa;
    private Vignette ppVi;
    private LensDistortion ppLd;

    private float vVal, lVal, cVal;
    private float vi, le, ch;
    private float dvi=0.4f, dle = 35f, dch = 1f;

    private float speed = 0.1f;
    public static SlowmoPostEffect instance;
    
    
    void Start()
    {
        instance = this;
        ppCa = pp.GetSetting<ChromaticAberration>();
        ppVi = pp.GetSetting<Vignette>();
        ppLd = pp.GetSetting<LensDistortion>();
    }

    // Update is called once per frame
    void Update()
    {

        //if (CamFollow.instance.camshakeOn)
        //{
        //    curLayer.enabled = false;
        //    curVol.enabled = false;
        //}
        //else
        //{
        //    StartCoroutine(enablePP(2f));
        //}

        UpdateIntensities();
        // ppCa.intensity.value = Mathf.SmoothDamp(ppCa.intensity.value, ch, ref cVal, speed);
        // ppVi.intensity.value = Mathf.SmoothDamp(ppVi.intensity.value, vi, ref vVal, speed);
        // ppLd.intensity.value = Mathf.SmoothDamp(ppLd.intensity.value, le, ref lVal, speed);

        // ppCa.intensity.value = Mathf.SmoothStep(ppCa.intensity.value, ch, speed);
        // ppVi.intensity.value = Mathf.SmoothStep(ppVi.intensity.value, vi, speed);
        // ppLd.intensity.value = Mathf.SmoothStep(ppLd.intensity.value, le, speed);
    }

    
    public void UpdateIntensities()
    {
        if (((!FinalAnimTest.instance.IsMoving && !CamFollow.instance.camshakeOn) && !MenuManager.instance.OnmenuPage) || ((FinalAnimTest.instance.isSliding && !CamFollow.instance.camshakeOn) && !MenuManager.instance.OnmenuPage))
        {
            float f = FinalAnimTest.instance.agentSpeed;
            vi = dvi * (81.05f - f);
            le = dle * (81.1f - f);
            ch = dch * (81.1f - f);

            ppCa.intensity.value = Mathf.SmoothStep(ppCa.intensity.value, ch, speed);
            ppVi.intensity.value = Mathf.SmoothStep(ppVi.intensity.value, vi, speed);
            ppLd.intensity.value = Mathf.SmoothStep(ppLd.intensity.value, le, speed);

        }
        else
        {
            float f = FinalAnimTest.instance.agentSpeed;
            //vi = dvi * (80f - f);
            //le = dle * (80f - f);
            //ch = dch * (80f - f);

            vi = 0f;
            le = 0f;
            ch = 0f;


            //ppCa.intensity.value = 0f;
            //ppVi.intensity.value = 0f;
            //ppLd.intensity.value = 0f;

            
            ppCa.intensity.value = Mathf.SmoothStep(ppCa.intensity.value, 0f, 0.05f);
            ppVi.intensity.value = Mathf.SmoothStep(ppVi.intensity.value, 0f, 0.05f);
            ppLd.intensity.value = Mathf.SmoothStep(ppLd.intensity.value, 0f, 0.05f);
            StartCoroutine(enablePP(0.01f));
        }

        //float f = Simplejumpcontroller.instance.agentSpeed;
        //vi = dvi * (81f - f);
        //le = dle * (81f - f);
        //ch = dch * (81f - f);

        //ppCa.intensity.value = Mathf.SmoothStep(ppCa.intensity.value, ch, speed);
       // ppVi.intensity.value = Mathf.SmoothStep(ppVi.intensity.value, vi, speed);
        //ppLd.intensity.value = Mathf.SmoothStep(ppLd.intensity.value, le, speed);
    }


    public IEnumerator enablePP(float secs)
    {
        yield return new WaitForSeconds(secs);
      //  Debug.Log("cam bounce whil shooting begins");
        CamFollow.instance.camshakeOn = false;
    }
}
