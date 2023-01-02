using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = new GameManager();
    public enum PlayerState
    {
        Normal, //�⺻
        CanHide, //���� ��ü�� ������ ���� ��
        Hide, //������ ��
        CanPick, //��ü�� �ֿ� �� ���� ��
        CanExit, //�ⱸ�� ������ ���� ��
        CanTalk, //NPC�� ��ȭ�� �� ���� ��
        Talk
    }
    public Slider StaminaSlider; //���¹̳� ��
    public Text CanText; //�� �� �ִ� �ൿ �ؽ�Ʈ
    public Text CantExitText; //������ ���� �� �ؽ�Ʈ
    public PlayerStatus playerStat; //�÷��̾� ���� ��ũ���ͺ� ������Ʈ
    public Player player; //�÷��̾� ��ũ��Ʈ
    public Image bateryImage; //������ ���͸� �̹���
    public bool haveKey = false; //���� ����

    public GameObject NPCPanel; //NPC ��� â
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
    //    Time.timeScale = 0; //�ð� ����
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
