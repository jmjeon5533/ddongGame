using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.transform.CompareTag("Player"))
        //{
        //    collision.transform.GetComponent<Player>().HideObject = gameObject;
        //    collision.transform.GetComponent<Player>().state = GameManager.PlayerState.CanHide;
        //    GameManager.instance.CanHideText.SetActive(true);
        //}
    }
    private void OnCollisionExit(Collision collision)
    {
        //if (collision.transform.CompareTag("Player"))
        //{
        //    collision.transform.GetComponent<Player>().HideObject = null;
        //    collision.transform.GetComponent<Player>().state = GameManager.PlayerState.Normal;
        //    GameManager.instance.CanHideText.SetActive(false);
        //}
    }
}
