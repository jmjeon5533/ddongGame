using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = new GameManager();
    public enum PlayerState
    {
        Normal,
        CanHide,
        Hide,
        CanPick,
        CanExit
    }
    public Slider StaminaSlider;
    public Text CanText;
    public Text CantExitText;
    public PlayerStatus playerStat;
    public Player player;
    public Image bateryImage;
    public bool haveKey = false;
    //public int haveitem;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //CanText.enabled = false;
        //CantExitText.enabled = false;
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
        print(playerStat.Batery / playerStat.MaxBatery);
        bateryImage.fillAmount = playerStat.Batery / playerStat.MaxBatery;
    }
    public void Ending()
    {
        print("ending");
    }
}
