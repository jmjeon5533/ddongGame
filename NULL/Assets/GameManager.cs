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
        Hide
    }
    public Slider StaminaSlider;
    public GameObject CanHideText;
    void Start()
    {
        CanHideText.SetActive(false);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
