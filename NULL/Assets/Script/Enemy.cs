using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform MovePoint;
    public GameObject player;
    NavMeshAgent nav;
    RaycastHit hit;
    Vector3 MoveRot;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyAI();
        DrawRay();
    }
    void EnemyAI()
    {
        if (player.GetComponent<Player>().state != GameManager.PlayerState.Hide)
        {
            nav.SetDestination(player.transform.position);
        }
        else
        {
            nav.SetDestination(transform.position);
        }
    }
    void DrawRay()
    {
        if(Physics.Raycast(transform.position,transform.forward, out hit,1))
        {
            if (hit.collider.CompareTag("Player"))
            {
                
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.instance.GameOver();
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward * 1,Color.red);
    }
}
