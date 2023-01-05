using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform[] MovePoint;
    public GameObject player;
    NavMeshAgent nav;
    RaycastHit hit;
    Vector3 MoveRot;
    public float curtime, Movecooltime = 7;
    public bool IsCastPlayer = false;
    public float RayDistance;
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
    }
    void EnemyAI()
    {
        if (IsCastPlayer)
        {
            if (GameManager.instance.player.state == GameManager.PlayerState.Hide)
            {
                if (!GameManager.instance.playerStat.isflash)
                {
                    curtime += Time.deltaTime;
                    if (curtime >= Movecooltime)
                    {
                        IsCastPlayer = false;
                    }
                    if (Vector3.Distance(player.transform.position,transform.position) <= 0.7f)
                    {
                        GameManager.instance.GameOver();
                    }
                }
                else
                {
                    Dead();
                }
            }
            else
            {
                Dead();
            }
        }
        else
        {
            if (Mathf.Abs(nav.velocity.x) <= 0 || Mathf.Abs(nav.velocity.z) <= 0)
            {
                curtime += Time.deltaTime;
                if (curtime >= Movecooltime)
                {
                    print(nav.velocity);
                    nav.SetDestination(MovePoint[Random.Range(0, 3)].position);
                    curtime = 0;
                }

            }
        }
    }
    void Dead()
    {
        nav.SetDestination(player.transform.position);
        curtime = 0;
        if (nav.remainingDistance < 1.2f && nav.remainingDistance > 1f)
        {
            GameManager.instance.GameOver();
        }
    }
    void DrawRay()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("HideObject"));
        if (Physics.Raycast(transform.position, MoveRot, out hit, RayDistance, layerMask))
        {
            if (hit.collider.CompareTag("Player"))
            {

                if (GameManager.instance.player.state == GameManager.PlayerState.Hide)
                {
                    if (GameManager.instance.playerStat.isflash)
                    {
                        IsCastPlayer = true;
                        print("°¨ÁöµÊ!");
                    }
                }
                else
                {
                    IsCastPlayer = true;
                    print("°¨ÁöµÊ!");
                }
                print(hit.collider.gameObject.name);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, MoveRot * RayDistance,Color.red);
    }
}
