using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = new GameManager();
    public enum PlayerState
    {
        Normal, //기본
        CanHide, //숨을 물체가 가까이 있을 때
        Hide, //숨었을 때
        CanPick, //물체를 주울 수 있을 때
        CanExit, //출구와 가까이 있을 때
        CanTalk, //NPC와 대화할 수 있을 때
        Talk
    }
    public Slider StaminaSlider; //스태미나 바
    public Text CanText; //할 수 있는 행동 텍스트
    public Text CantExitText; //나가지 못할 때 텍스트
    public PlayerStatus playerStat; //플레이어 스텟 스크립터블 오브젝트
    public Player player; //플레이어 스크립트
    public Image bateryImage; //손전등 배터리 이미지
    public bool haveKey = false; //열쇠 유무

    public GameObject NPCPanel; //NPC 대사 창
    public Text NPCText;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        batery();
    }
    void batery()
    {
        if (playerStat.isflash)
        {
            playerStat.Batery -= 0.5f;
        }
        if (playerStat.Batery <= 0)
        {
            playerStat.isflash = false;
            player.flash.SetActive(false);
        }
        bateryImage.fillAmount = playerStat.Batery / playerStat.MaxBatery;
    }
    public void GameOver()
    {
        print("GameOver");
    }
    public void Ending()
    {
        print("ending");
    }
    //public void NPCTextSetting()
    //{
    //    Time.timeScale = 0; //시간 멈춤
    //    NPCPanel.SetActive(true);
    //    NPCPanel.transform.GetChild(0).GetComponent<Text>().text = player.useObject.name;
    //    player.useObject.GetComponent<NPC>().NPCTexting();
    //}
    //public void NPCTextEnd()
    //{
    //    Time.timeScale = 1;
    //    NPCPanel.SetActive(false);
    //}
}
