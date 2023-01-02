using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "Scriptable Object Asset/Player")]
public class PlayerStatus : ScriptableObject
{
    public int MoveSpeed = 5, //이동 합계속도
       walkSpeed = 5, //걷는 속도
       RunSpeed = 15; //뛰는 속도
    public float Stamina = 1000; //기력(스태미나)
    public float Batery = 5000; //손전등 베터리
    public float MaxBatery = 5000; //최대 베터리
    public float camSpeed = 2; //카메라 감도
    public bool isflash = false; //손전등 전원 유무
    public int isQuest;
}
