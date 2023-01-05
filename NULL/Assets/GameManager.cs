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
    public Text NPCName;
    public Text NPCText;
    public NPC npc;
    public bool isText = false;
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
        if (player.state != PlayerState.Talk)
        {
            batery();
        }
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
    public void NPCTexting(NPC Npc)
    {
        npc = Npc;
        Time.timeScale = 0; //�ð� ����
        NPCPanel.SetActive(true);
        NPCName.text = player.useObject.name;
        player.state = PlayerState.Talk;
        StartCoroutine(Texting());
    }
    public IEnumerator Texting()
    {
        isText = true;
        for (int j = 0; j < npc.TextScript.Text[player.i].Length; j++)
        {
            NPCText.text = npc.TextScript.Text[player.i].Substring(0, j + 1);
            yield return new WaitForSecondsRealtime(0.07f);
        }
        isText = false;
    }
}
