using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MenuPage, LevelPage, SettingPage,pausePage,victoryPage,deadPage,blackBg,Canvas,instrustion,dummyLevel,player,playerDupe,curlevelObj;
    public GameObject PlayBtn,BackBtn,LevelBtn,SettingBtn,QuitBtn,nextLevelBtn;
    public int curLevel;
    public GameObject[] levelsToPlay;
    public bool isPaused,OnmenuPage=true,itstimetoDestroy;
    public static MenuManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !OnmenuPage)
        {
                         
            blackBg.SetActive(true);
            SettingPage.SetActive(false);
            MenuPage.SetActive(false);
            LevelPage.SetActive(false);
            deadPage.SetActive(false);
            pausePage.SetActive(true);
            isPaused = true;
            OnmenuPage = true;

            FinalAnimTest.instance.currntSpeedDir = 0f;
            FinalAnimTest.instance.testspeed = 0f;
            FinalAnimTest.instance.rb.velocity = new Vector2(0f, FinalAnimTest.instance.rb.velocity.y);



            //Vector3 temPos = new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y, Canvas.transform.position.z);
            // Canvas.transform.position = temPos;

            CamFollow.instance.Isstarted = false;
            CamFollow.instance.IsZoomedIn = false;
            Camera.main.orthographicSize = 55f;

        }
    }


    public void enableBtnAction(string Status)
    {
        switch (Status)
        {
            case "Play":
                MenuPage.SetActive(false);
                LevelPage.SetActive(true);

                //blackBg.SetActive(false);
                //SettingPage.SetActive(false);
                //LevelPage.SetActive(false);
                //MenuPage.SetActive(false);
                //CamFollow.instance.Isstarted = true;
                break;

            case "Settings":
                MenuPage.SetActive(false);
                LevelPage.SetActive(false);
                pausePage.SetActive(false);
                deadPage.SetActive(false);
                SettingPage.SetActive(true);
                CamFollow.instance.Isstarted = false;
                break;

            case "Levels":
                MenuPage.SetActive(false);
                pausePage.SetActive(false);
                SettingPage.SetActive(false);
                deadPage.SetActive(false);
                LevelPage.SetActive(true);
                CamFollow.instance.Isstarted = false;
                break;

            case "Back":
                SettingPage.SetActive(false);
                LevelPage.SetActive(false);
                pausePage.SetActive(false);
                deadPage.SetActive(false);
                MenuPage.SetActive(true);
                CamFollow.instance.Isstarted = false;
                break;

            case "Quit":
                SettingPage.SetActive(false);
                LevelPage.SetActive(false);
                deadPage.SetActive(false);
                MenuPage.SetActive(true);
                CamFollow.instance.Isstarted = false;
                itstimetoDestroy = true;
                Application.Quit();
                break;

            case "MainMenu":

                //Vector3 temPos = new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y, Canvas.transform.position.z);
                //Canvas.transform.position = temPos;
                //GunManager.instance.isDead = true;

                Destroy(curlevelObj);
                player.transform.localPosition = new Vector3(-800f, 1f, 0f);
                player.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                player.GetComponent<FinalAnimTest>().isDead = false;
                Destroy(player.GetComponent<FinalAnimTest>().RagDollAgent);
                instrustion.SetActive(false);
                dummyLevel.SetActive(true);

                Camera.main.orthographicSize = 55f;
                blackBg.SetActive(true);
                SettingPage.SetActive(false);
                LevelPage.SetActive(false);
                pausePage.SetActive(false);
                victoryPage.SetActive(false);
                deadPage.SetActive(false);
                MenuPage.SetActive(true);
                itstimetoDestroy = true;

                player.GetComponent<FinalAnimTest>().isObjectPickedUp = false;
                player.SetActive(true);


                //  CamFollow.instance.Isstarted = false;
                // CamFollow.instance.IsZoomedIn = false;

                CamFollow.instance.Isstarted = true;
                CamFollow.instance.IsZoomedIn = false;

                break;


            case "Resume":

                pausePage.SetActive(false);
                blackBg.SetActive(false);
                SettingPage.SetActive(false);
                LevelPage.SetActive(false);
                MenuPage.SetActive(false);
                deadPage.SetActive(false);
                CamFollow.instance.Isstarted = true;
                OnmenuPage = false;

                break;


            case "Restart":

               // GunManager.instance.isDead = true;
                Destroy(curlevelObj);
                itstimetoDestroy = true;
               // GunManager.instance.istimetoselfDestroy = true;
                player.SetActive(false);
                victoryPage.SetActive(false);
                deadPage.SetActive(false);
                CamFollow.instance.resetPos = true;
                CamFollow.instance.Isstarted = true;

                player.transform.localPosition = new Vector3(-800f, 1f, 0f);
                player.GetComponent<FinalAnimTest>().isDead = false;
                player.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

                Destroy(player.GetComponent<FinalAnimTest>().RagDollAgent);
                LevelSelection(curLevel);
                // player.transform.localPosition = new Vector3(0f, 0f, 0f);
                player.SetActive(true);
                restBoolValsonPlayer();
                break;


            case "NextLevel":

                // GunManager.instance.isDead = true;
                player.SetActive(false);
                deadPage.SetActive(false);
                Destroy(curlevelObj);
                player.transform.localPosition = new Vector3(-800f, 1f, 0f);
                player.GetComponent<FinalAnimTest>().isDead = false;
                player.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0f, 0f, 0f);  
                Destroy(player.GetComponent<FinalAnimTest>().RagDollAgent);
                itstimetoDestroy = true;
                victoryPage.SetActive(false);
                OnmenuPage = false;

                CamFollow.instance.resetPos = true;
                CamFollow.instance.Isstarted = true;

                LevelSelection(curLevel + 1);
                player.SetActive(true);
                restBoolValsonPlayer();
                break;


            case "victory":
                blackBg.SetActive(true);
                curlevelObj.SetActive(false);
                dummyLevel.SetActive(true);
                instrustion.SetActive(false);
                victoryPage.SetActive(true);
                
                if(curLevel==levelsToPlay.Length-1){
                    nextLevelBtn.SetActive(false);
                }
                OnmenuPage = true;
                CamFollow.instance.Isstarted = true;
                CamFollow.instance.IsZoomedIn = false;
                break;
        }

        SoundManager.instance.playShootSound(12);
    }


    public void LevelSelection(int Id)
    {
        //if (!curlevelObj)
        //{
        //    Destroy(curlevelObj);
        //}

        Camera.main.orthographicSize = 50f;

        if (Id==0)
        {
            instrustion.SetActive(true);
        }

        dummyLevel.SetActive(false);
        curLevel = Id;
        curlevelObj = Instantiate(levelsToPlay[Id], Canvas.transform);
        curlevelObj.name = "level-" + Id;
        curlevelObj.transform.SetAsFirstSibling();
        curlevelObj.SetActive(true);

       

     // levelsToPlay[Id].SetActive(true);
        blackBg.SetActive(false);
        SettingPage.SetActive(false);
        LevelPage.SetActive(false);
        pausePage.SetActive(false);
        deadPage.SetActive(false);
        MenuPage.SetActive(false);
        CamFollow.instance.Isstarted = true;

        restBoolValsonPlayer();

        //player.transform.localPosition = new Vector3(-800f, 1f, 0f);
        //player.GetComponent<FinalAnimTest>().isDead = false;
        //player.transform.GetChild(0).transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        player.GetComponent<FinalAnimTest>().weaponId = curLevel;
        player.GetComponent<FinalAnimTest>().setWeaponinHands();


        OnmenuPage = false;

        SoundManager.instance.playShootSound(12);

        // itstimetoDestroy = false;

        //enableBtnAction("Play");
    }



    public void restBoolValsonPlayer()
    {
        FinalAnimTest.instance.isUnderRoll = false;
        FinalAnimTest.instance.IsMoving = false;
        FinalAnimTest.instance.isjumping = false;

    }
}
