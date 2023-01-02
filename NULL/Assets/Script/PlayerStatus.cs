using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "Scriptable Object Asset/Player")]
public class PlayerStatus : ScriptableObject
{
    public int MoveSpeed = 5, //�̵� �հ�ӵ�
       walkSpeed = 5, //�ȴ� �ӵ�
       RunSpeed = 15; //�ٴ� �ӵ�
    public float Stamina = 1000; //���(���¹̳�)
    public float Batery = 5000; //������ ���͸�
    public float MaxBatery = 5000; //�ִ� ���͸�
    public float camSpeed = 2; //ī�޶� ����
    public bool isflash = false; //������ ���� ����
    public int isQuest;
}
