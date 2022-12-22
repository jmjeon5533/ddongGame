using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Camera cam;
    public Slider StaminaSlider;
    public int MoveSpeed = 5,
        walkSpeed = 5,
        RunSpeed = 15;
    public float Stamina = 1000;
    public float camSpeed = 2;
    float xRotate;

    Rigidbody rigid;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        float yRotateSize = Input.GetAxis("Mouse X") * camSpeed;
        float yRotate = transform.eulerAngles.y + yRotateSize;
        float xRotateSize = -Input.GetAxis("Mouse Y") * camSpeed;
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);
        transform.eulerAngles = new Vector3(0, yRotate, 0);
        cam.transform.eulerAngles = new Vector3(xRotate, yRotate + 90, 0);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0)
        {
            Stamina--;
            MoveSpeed = RunSpeed;
        }
        else if(!Input.GetKey(KeyCode.LeftShift) && Stamina < 1000)
        {
            Stamina += 0.5f;
            MoveSpeed = walkSpeed;
        }
        else
        {
            MoveSpeed = walkSpeed;
        }
        StaminaSlider.value = Stamina;
        Vector3 Move = new Vector3(h * MoveSpeed, 0, v * MoveSpeed);
        Vector3 lookforward = new Vector3(-transform.forward.x,0, -transform.forward.z).normalized;
        Vector3 lookright = new Vector3(transform.right.x, 0, transform.right.z).normalized;
        Vector3 MoveDir = lookforward * Move.x + lookright * Move.z;
        rigid.velocity = new Vector3(MoveDir.x,rigid.velocity.y,MoveDir.z);
    }
}
