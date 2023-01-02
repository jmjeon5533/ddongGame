using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public NPCT Text;
    public int i = 0;
    private void Update()
    {
        //if (GameManager.instance.player.state == GameManager.PlayerState.CanTalk)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        GameManager.instance.player.state = GameManager.PlayerState.Talk;
        //        NPCTexting();
        //        if (Text.Text.Length <= i + 1)
        //        {
        //            GameManager.instance.player.state = GameManager.PlayerState.CanTalk;
        //            i = 0;
        //            GameManager.instance.NPCTextEnd();
        //        }
        //    }
        //}
    }
    public void NPCTexting()
    {
        GameManager.instance.NPCText.text = Text.Text[i];
        i++;
    }
}
