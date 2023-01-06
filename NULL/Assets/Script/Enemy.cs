using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform[] MovePoint; //순찰지점
    public GameObject player; //플레이어
    NavMeshAgent nav; //NavMeshAgent
    RaycastHit hit; //레이캐스트
    Vector3 MoveRot; //플레이어 쪽 방향
    public float curtime, Movecooltime = 7; //현시점, 움직임 대기시간
    public bool IsCastPlayer = false; //플레이어 감지 유무
    public float RayDistance; //레이 길이

    public float WarningPanelAlpha = 0; //플레이어 감지중 경고 패널 알파값
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
    void EnemyPanelAlpha() //플레이어 감지중 경고 패널 알파 조정
    {
        if (IsCastPlayer) //감지 중일때
        {
            //패널 서서히 붉어짐
            WarningPanelAlpha = Mathf.Lerp(WarningPanelAlpha, 0.03f, 0.02f); 
        }
        else //감지 중이 아닐 때
        {
            //패널 서서히 투명해짐
            WarningPanelAlpha = Mathf.Lerp(WarningPanelAlpha, 0f, 0.02f);
        }
        //바뀐 값 조정
        GameManager.instance.WarningPanel.color = new Color(1, 0.1f, 0, WarningPanelAlpha);
    }
    void EnemyAI() //적 AI
    {
        if (IsCastPlayer) //플레이어 감지 중일 때
        {
            //플레이어가 숨었다면
            if (GameManager.instance.player.state == GameManager.PlayerState.Hide)
            {
                //손전등도 켜지 않았다면 
                if (!GameManager.instance.playerStat.isflash)
                {
                    //시간 계산
                    curtime += Time.deltaTime;
                    if (curtime >= Movecooltime) //시간이 지나면
                    {
                        IsCastPlayer = false; //감지 해제
                    }
                    //적과 플레이어의 거리가 0.7 이하라면
                    if (Vector3.Distance(player.transform.position,transform.position) <= 0.7f)
                    {
                        GameManager.instance.GameOver(); //게임오버
                    }
                }
                else //손전등을 켰다면
                {
                    Dead();
                }
            }
            else //숨어있지 않다면
            {
                Dead();
            }
        }
        else //감지되지 않았다면
        {
            //움직이지 않으면
            if (Mathf.Abs(nav.velocity.x) <= 0 || Mathf.Abs(nav.velocity.z) <= 0)
            {
                //시간 계산
                curtime += Time.deltaTime;
                if (curtime >= Movecooltime) //시간이 지나면
                {
                    print(nav.velocity); 
                    //랜덤한 순찰구역으로 이동
                    nav.SetDestination(MovePoint[Random.Range(0, MovePoint.Length)].position);
                    curtime = 0; //시간 0으로 초기화
                }

            }
        }
    }
    void Dead() //플레이어가 감지되었을 때
    {
        //플레이어에게 계속 이동
        nav.SetDestination(player.transform.position);
        curtime = 0; //시간 0으로 초기화
        //플레이어와의 거리가 1과 1.2 사이라면
        //순찰지점에 도착해있으면 거리가 0으로 변해 게임오버 처리가 되어버린다)
        if (nav.remainingDistance < 1.2f && nav.remainingDistance > 1f)
        {
            GameManager.instance.GameOver(); //게임오버
        }
    }
    void DrawRay() //사거리 표시
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("HideObject")); //숨기 오브젝트 제외
        //플레이어 방향으로 레이를 쏜다
        if (Physics.Raycast(transform.position, MoveRot, out hit, RayDistance, layerMask))
        {
            //만약 플레이어가 맞으면
            if (hit.collider.CompareTag("Player"))
            {
                //맞은 플레이어가 숨은 상태이면
                if (GameManager.instance.player.state == GameManager.PlayerState.Hide)
                {
                    //숨은 상태에서 손전등을 켰다면
                    if (GameManager.instance.playerStat.isflash)
                    {
                        //감지 상태 활성화
                        IsCastPlayer = true;
                        print("감지됨!");
                    }
                }
                else //맞은 플레이어가 숨지 않았다면
                {
                    //감지 상태 활성화
                    IsCastPlayer = true;
                    print("감지됨!");
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, MoveRot * RayDistance,Color.red);
    }
}
