using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Camera cam; //�÷��̾� ī�޶�
    public GameObject flash; //������ ���
    public int MoveSpeed = 5, //�̵� �հ�ӵ�
        walkSpeed = 5, //�ȴ� �ӵ�
        RunSpeed = 15; //�ٴ� �ӵ�
    public float Stamina = 1000; //���(���¹̳�)
    public float camSpeed = 2; //ī�޶� ����
    public GameObject HideObject; //���� ������Ʈ (���������� ����)

    float xRotate; //ī�޶� ���
    bool isflash = false; //������ ���� ����
    Vector3 HideOffPos; //���� ���� ��ġ

    public GameManager.PlayerState state; //�÷��̾��� ���� (�븻,���� �� �ִ�,����)
    Rigidbody rigid; //������ٵ�
    private void Start()
    {
        Cursor.visible = false; //Ŀ�� �����
        Cursor.lockState = CursorLockMode.Locked; //Ŀ�� ����
        rigid = GetComponent<Rigidbody>(); //������ٵ� ����
    }
    private void Update()
    {
        StateMove();//���� ��ȭ
    }
    void StateMove()
    {
        if (state == GameManager.PlayerState.Normal) //�븻 ���¿�����
        {
            //��� ���� ����
            CamMove();
            Moving();
            FlashLight();
            DrawRay();
        }
        else if (state == GameManager.PlayerState.CanHide) //���� �� �ִ� ���¿�����
        {
            //��� ���� ����
            CamMove();
            Moving();
            FlashLight();
            DrawRay();
            if (/*Input.GetKeyDown(KeyCode.E)*/Input.GetMouseButtonDown(0)) //��ȣ�ۿ��� ������ ��
            {
                state = GameManager.PlayerState.Hide; //���� ���·� ��ȯ
                HideOffPos = transform.position; //���� ��ġ ����
                Hide(); //���� �ൿ ����
                print("Hide");
            }
        }
        else if (state == GameManager.PlayerState.Hide) //���� ������ �� 
        {
            //������ �� ������ ī�޶�� �������� ��밡��
            CamMove();
            FlashLight();
            if (/*Input.GetKeyDown(KeyCode.E)*/Input.GetMouseButtonDown(0)) //��ȣ�ۿ��� ������ ��
            {
                state = GameManager.PlayerState.CanHide; //���� �� �ִ� ���·� ��ȯ
                HideOff(); //������ �ൿ ����
                print("Set");
            }
        }
    }
    void Hide() //���� �ൿ
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false; //�浹 ������ ���� �ݶ��̴��� ��Ȱ��ȭ
        Destroy(gameObject.GetComponent<Rigidbody>()); //�߶� ������ ���� ������ٵ� ����
        transform.position = HideObject.transform.position; //���� ������Ʈ�� ��ġ�� �̵�
        transform.eulerAngles = HideObject.transform.eulerAngles; //���� ������Ʈ�� �þ߷� ��ȯ
        GameManager.instance.CanHideText.SetActive(false);
    }
    void HideOff() //������ �ൿ
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = true; //�ݶ��̴��� �ٽ� Ȱ��ȭ
        gameObject.AddComponent<Rigidbody>(); //������ٵ� �ٽ� �����ϰ�
        rigid = GetComponent<Rigidbody>(); //������ٵ� �ٽ� �������� ��
        rigid.freezeRotation = true; //������ٵ�� ���� ���ϴ� ������ ����
        transform.position = HideOffPos; //�����ߴ� ��ġ�� �̵�
    }
    void CamMove() //ī�޶� ������
    {
        float yRotateSize = Input.GetAxis("Mouse X") * camSpeed;
        float yRotate = transform.eulerAngles.y + yRotateSize;
        float xRotateSize = -Input.GetAxis("Mouse Y") * camSpeed;
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);
        transform.eulerAngles = new Vector3(0, yRotate, 0);
        cam.transform.eulerAngles = new Vector3(xRotate, yRotate + 90, 0);


    }
    void Moving() //�̵�
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0)
            //����Ʈ�� ���� ���·� ���¹̳��� �ִٸ�
        {
            Stamina--; //���¹̳��� ����鼭
            MoveSpeed = RunSpeed; //�� ������ �̵�
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && Stamina < 1000)
            //����Ʈ�� ������ �ʰ� ���¹̳��� �ִٸ�
        {
            Stamina += 0.5f; //���¹̳��� ���� �߰�
            MoveSpeed = walkSpeed; //�⺻ �ӵ��� �̵�
        }
        else //���¹̳��� ���ٸ�
        {
            MoveSpeed = walkSpeed; //�⺻ �ӵ��� �̵�
        }
        Vector3 Move = new Vector3(h * MoveSpeed, 0, v * MoveSpeed);
        Vector3 lookforward = new Vector3(-transform.forward.x, 0, -transform.forward.z).normalized;
        Vector3 lookright = new Vector3(transform.right.x, 0, transform.right.z).normalized;
        Vector3 MoveDir = lookforward * Move.x + lookright * Move.z;
        rigid.velocity = new Vector3(MoveDir.x, rigid.velocity.y, MoveDir.z);

        GameManager.instance.StaminaSlider.value = Stamina;
    }
    void FlashLight() //������
    {
        if (Input.GetKeyDown(KeyCode.F)) //F�� ������
        {
            isflash = !isflash; //������ ���� ��ȯ
        }
        flash.SetActive(isflash); //��ȯ�� ���·� ����
    }
    void DrawRay()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, RayDistance))
        {
            if (hit.collider.transform.CompareTag("HideObject"))
            {
                HideObject = hit.collider.gameObject;
                state = GameManager.PlayerState.CanHide;
                GameManager.instance.CanHideText.SetActive(true);
            }
        }
        else
        {
            HideObject = null;
            state = GameManager.PlayerState.Normal;
            GameManager.instance.CanHideText.SetActive(false);
        }
    }
    RaycastHit hit;
    public float RayDistance = 2;
    private void OnDrawGizmos()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * RayDistance, Color.red);
    }
}
