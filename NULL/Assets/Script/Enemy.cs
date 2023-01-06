using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform[] MovePoint; //��������
    public GameObject player; //�÷��̾�
    NavMeshAgent nav; //NavMeshAgent
    RaycastHit hit; //����ĳ��Ʈ
    Vector3 MoveRot; //�÷��̾� �� ����
    public float curtime, Movecooltime = 7; //������, ������ ���ð�
    public bool IsCastPlayer = false; //�÷��̾� ���� ����
    public float RayDistance; //���� ����

    public float WarningPanelAlpha = 0; //�÷��̾� ������ ��� �г� ���İ�
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyAI();
        DrawRay();
        MoveRot = (player.transform.position - transform.position).normalized;
        EnemyPanelAlpha();
    }
    void EnemyPanelAlpha() //�÷��̾� ������ ��� �г� ���� ����
    {
        if (IsCastPlayer) //���� ���϶�
        {
            //�г� ������ �Ӿ���
            WarningPanelAlpha = Mathf.Lerp(WarningPanelAlpha, 0.03f, 0.02f); 
        }
        else //���� ���� �ƴ� ��
        {
            //�г� ������ ��������
            WarningPanelAlpha = Mathf.Lerp(WarningPanelAlpha, 0f, 0.02f);
        }
        //�ٲ� �� ����
        GameManager.instance.WarningPanel.color = new Color(1, 0.1f, 0, WarningPanelAlpha);
    }
    void EnemyAI() //�� AI
    {
        if (IsCastPlayer) //�÷��̾� ���� ���� ��
        {
            //�÷��̾ �����ٸ�
            if (GameManager.instance.player.state == GameManager.PlayerState.Hide)
            {
                //����� ���� �ʾҴٸ� 
                if (!GameManager.instance.playerStat.isflash)
                {
                    //�ð� ���
                    curtime += Time.deltaTime;
                    if (curtime >= Movecooltime) //�ð��� ������
                    {
                        IsCastPlayer = false; //���� ����
                    }
                    //���� �÷��̾��� �Ÿ��� 0.7 ���϶��
                    if (Vector3.Distance(player.transform.position,transform.position) <= 0.7f)
                    {
                        GameManager.instance.GameOver(); //���ӿ���
                    }
                }
                else //�������� �״ٸ�
                {
                    Dead();
                }
            }
            else //�������� �ʴٸ�
            {
                Dead();
            }
        }
        else //�������� �ʾҴٸ�
        {
            //�������� ������
            if (Mathf.Abs(nav.velocity.x) <= 0 || Mathf.Abs(nav.velocity.z) <= 0)
            {
                //�ð� ���
                curtime += Time.deltaTime;
                if (curtime >= Movecooltime) //�ð��� ������
                {
                    print(nav.velocity); 
                    //������ ������������ �̵�
                    nav.SetDestination(MovePoint[Random.Range(0, MovePoint.Length)].position);
                    curtime = 0; //�ð� 0���� �ʱ�ȭ
                }

            }
        }
    }
    void Dead() //�÷��̾ �����Ǿ��� ��
    {
        //�÷��̾�� ��� �̵�
        nav.SetDestination(player.transform.position);
        curtime = 0; //�ð� 0���� �ʱ�ȭ
        //�÷��̾���� �Ÿ��� 1�� 1.2 ���̶��
        //���������� ������������ �Ÿ��� 0���� ���� ���ӿ��� ó���� �Ǿ������)
        if (nav.remainingDistance < 1.2f && nav.remainingDistance > 1f)
        {
            GameManager.instance.GameOver(); //���ӿ���
        }
    }
    void DrawRay() //��Ÿ� ǥ��
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("HideObject")); //���� ������Ʈ ����
        //�÷��̾� �������� ���̸� ���
        if (Physics.Raycast(transform.position, MoveRot, out hit, RayDistance, layerMask))
        {
            //���� �÷��̾ ������
            if (hit.collider.CompareTag("Player"))
            {
                //���� �÷��̾ ���� �����̸�
                if (GameManager.instance.player.state == GameManager.PlayerState.Hide)
                {
                    //���� ���¿��� �������� �״ٸ�
                    if (GameManager.instance.playerStat.isflash)
                    {
                        //���� ���� Ȱ��ȭ
                        IsCastPlayer = true;
                        print("������!");
                    }
                }
                else //���� �÷��̾ ���� �ʾҴٸ�
                {
                    //���� ���� Ȱ��ȭ
                    IsCastPlayer = true;
                    print("������!");
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, MoveRot * RayDistance,Color.red);
    }
}
