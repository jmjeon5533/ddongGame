using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Camera cam; //�÷��̾� ī�޶�
    public GameObject flash; //������ ���
    public float CamfieldValue = 60; //ī�޶� Ȯ�뷮
    public GameObject useObject; //��� ������Ʈ (���������� ����)
    public GameManager.PlayerState state; //�÷��̾��� ����
    public PlayerStatus player;

    float xRotate; //ī�޶� ���
    Vector3 HideOffPos; //���� ���� ��ġ

    Rigidbody rigid; //������ٵ�

    RaycastHit hit;
    public float RayDistance = 2; //��Ÿ�
    private void Start()
    {
        Cursor.visible = false; //Ŀ�� �����
        Cursor.lockState = CursorLockMode.Locked; //Ŀ�� ����
        rigid = GetComponent<Rigidbody>(); //������ٵ� ����
        StatInitial();
    }
    private void Update()
    {
        StateMove();//���� ��ȭ
    }
    void StatInitial()
    {
        player.Stamina = 1000;
        player.Batery = 5000; //������ ���͸�
        player.MaxBatery = 5000; //�ִ� ���͸�
        player.isflash = false;
    }
    void StateMove()
    {
        if (state == GameManager.PlayerState.Normal) //�븻 ������ ��
        {
            Moving();
            DrawRay();
        }
        else if (state == GameManager.PlayerState.CanHide) //���� �� �ִ� ������ ��
        {
            Moving();
            DrawRay();
            if (Input.GetMouseButtonDown(0)) //��ȣ�ۿ��� ������ ��
            {
                state = GameManager.PlayerState.Hide; //���� ���·� ��ȯ
                HideOffPos = transform.position; //���� ��ġ ����
                Hide(); //���� �ൿ ����
                print("Hide");
            }
        }
        else if (state == GameManager.PlayerState.Hide) //���� ������ �� 
        {
            if (Input.GetMouseButtonDown(0)) //��ȣ�ۿ��� ������ ��
            {
                state = GameManager.PlayerState.CanHide; //���� �� �ִ� ���·� ��ȯ
                HideOff(); //������ �ൿ ����
                print("Set");
            }
        }
        else if (state == GameManager.PlayerState.CanPick) //�ֿ� �� �ִ� ������ ��
        {
            Moving();
            DrawRay();
            if (Input.GetMouseButtonDown(0)) //��ȣ�ۿ��� ������ ��
            {
                GameManager.instance.haveKey = true; //���踦 ���
                hit.collider.gameObject.SetActive(false); //���踦 �����
            }
        }
        else if (state == GameManager.PlayerState.CanExit) //�� ���϶�
        {
            Moving();
            DrawRay();
            if (Input.GetMouseButtonDown(0)) //��ȣ�ۿ��� ������ ��
            {
                if (GameManager.instance.haveKey) //���谡 ������
                {
                    GameManager.instance.Ending(); //Ż��
                }
                else
                {
                    print("Can't Exit");
                    GameManager.instance.CantExitText.enabled = true;
                    //GameManager.instance.CantExitText.enabled = false;
                }
            }
        }
        CamMove();
        FlashLight();
        StaminaSet();
    }
    void StaminaSet()
    {
        if (Input.GetKey(KeyCode.LeftShift) && player.Stamina > 0)
        //����Ʈ�� ���� ���·� ���¹̳��� �ִٸ�
        {
            player.Stamina--; //���¹̳��� ����鼭
            player.MoveSpeed = player.RunSpeed; //�� ������ �̵�
            CamfieldValue = Mathf.Lerp(CamfieldValue, 75, 0.1f);
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && player.Stamina < 1000)
        //����Ʈ�� ������ �ʰ� ���¹̳��� �ִٸ�
        {
            player.Stamina += 0.5f; //���¹̳��� ���� �߰�
            player.MoveSpeed = player.walkSpeed; //�⺻ �ӵ��� �̵�
            CamfieldValue = Mathf.Lerp(CamfieldValue, 60, 0.1f);
        }
        else //���¹̳��� ���ٸ�
        {
            player.MoveSpeed = player.walkSpeed; //�⺻ �ӵ��� �̵�
            CamfieldValue = Mathf.Lerp(CamfieldValue, 60, 0.1f);
        }
        GameManager.instance.StaminaSlider.value = player.Stamina;
        cam.fieldOfView = CamfieldValue;
    }
    void Hide() //���� �ൿ
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false; //�浹 ������ ���� �ݶ��̴��� ��Ȱ��ȭ
        Destroy(gameObject.GetComponent<Rigidbody>()); //�߶� ������ ���� ������ٵ� ����
        transform.position = useObject.transform.position; //���� ������Ʈ�� ��ġ�� �̵�
        transform.eulerAngles = useObject.transform.eulerAngles; //���� ������Ʈ�� �þ߷� ��ȯ
        GameManager.instance.CanText.enabled = false;
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
        float yRotateSize = Input.GetAxis("Mouse X") * player.camSpeed;
        float yRotate = transform.eulerAngles.y + yRotateSize;
        float xRotateSize = -Input.GetAxis("Mouse Y") * player.camSpeed;
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);
        transform.eulerAngles = new Vector3(0, yRotate, 0);
        cam.transform.eulerAngles = new Vector3(xRotate, yRotate + 90, 0);


    }
    void Moving() //�̵�
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 Move = new Vector3(h * player.MoveSpeed, 0, v * player.MoveSpeed);
        Vector3 lookforward = new Vector3(-transform.forward.x, 0, -transform.forward.z).normalized;
        Vector3 lookright = new Vector3(transform.right.x, 0, transform.right.z).normalized;
        Vector3 MoveDir = lookforward * Move.x + lookright * Move.z;
        rigid.velocity = new Vector3(MoveDir.x, rigid.velocity.y, MoveDir.z);
    }
    void FlashLight() //������
    {
        if (Input.GetKeyDown(KeyCode.F)) //F�� ������
        {
            player.isflash = !player.isflash; //������ ���� ��ȯ
        }
        
        flash.SetActive(player.isflash); //��ȯ�� ���·� ����
    }
    void DrawRay() //��Ÿ�
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, RayDistance))
        {
            switch (hit.collider.tag)
            {
                case "HideObject": //��Ÿ��� ���� ���� ���� ��ü�� ��
                    UseObjectFunction(GameManager.PlayerState.CanHide, "����");
                    //���� ���·� ��ȯ & ���� �ؽ�Ʈ
                    break;
                case "Exit": //��Ÿ��� ���� ���� �ⱸ�� ��
                    state = GameManager.PlayerState.CanExit;
                    GameManager.instance.CanText.text = "����"; //�ؽ�Ʈ ����� ��ȯ
                    GameManager.instance.CanText.enabled = true; //���� �ؽ�Ʈ Ȱ��ȭ
                    break;
                case "Key": //��Ÿ��� ���� ���� ������ ��
                    UseObjectFunction(GameManager.PlayerState.CanPick, "�ݱ�");
                    //�ݱ� ���·� ��ȯ & �ݱ� �ؽ�Ʈ
                    break;
            }
        }
        else //��Ÿ��� ���� �ʾ��� ��
        {
            useObject = null; //���� ������Ʈ�� ����
            state = GameManager.PlayerState.Normal; //�⺻ ���·� ��ȯ
            GameManager.instance.CanText.enabled = false;//���� �ؽ�Ʈ ��Ȱ��ȭ
        }
    }
    void UseObjectFunction(GameManager.PlayerState UseState,string text)
    {
        useObject = hit.collider.gameObject; //������Ʈ ����
        state = UseState; //��� ���·� ��ȯ
        GameManager.instance.CanText.text = text; //�ؽ�Ʈ ��ȯ
        GameManager.instance.CanText.enabled = true; //�ؽ�Ʈ Ȱ��ȭ
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * RayDistance, Color.red);
    }
}
