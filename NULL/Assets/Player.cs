using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Camera cam; //플레이어 카메라
    public GameObject flash; //손전등 기능
    public int MoveSpeed = 5, //이동 합계속도
        walkSpeed = 5, //걷는 속도
        RunSpeed = 15; //뛰는 속도
    public float Stamina = 1000; //기력(스태미나)
    public float camSpeed = 2; //카메라 감도
    public GameObject HideObject; //숨을 오브젝트 (유동적으로 지정)

    float xRotate; //카메라 계산
    bool isflash = false; //손전등 전원 유무
    Vector3 HideOffPos; //숨고 나올 위치

    public GameManager.PlayerState state; //플레이어의 상태 (노말,숨을 수 있는,숨은)
    Rigidbody rigid; //리지드바디
    private void Start()
    {
        Cursor.visible = false; //커서 지우기
        Cursor.lockState = CursorLockMode.Locked; //커서 고정
        rigid = GetComponent<Rigidbody>(); //리지드바디 대입
    }
    private void Update()
    {
        StateMove();//상태 변화
    }
    void StateMove()
    {
        if (state == GameManager.PlayerState.Normal) //노말 상태에서는
        {
            //모든 동작 가능
            CamMove();
            Moving();
            FlashLight();
            DrawRay();
        }
        else if (state == GameManager.PlayerState.CanHide) //숨을 수 있는 상태에서는
        {
            //모든 동작 포함
            CamMove();
            Moving();
            FlashLight();
            DrawRay();
            if (/*Input.GetKeyDown(KeyCode.E)*/Input.GetMouseButtonDown(0)) //상호작용을 눌렀을 때
            {
                state = GameManager.PlayerState.Hide; //숨은 상태로 변환
                HideOffPos = transform.position; //현재 위치 저장
                Hide(); //숨는 행동 실행
                print("Hide");
            }
        }
        else if (state == GameManager.PlayerState.Hide) //숨은 상태일 때 
        {
            //움직일 수 없지만 카메라와 손전등은 사용가능
            CamMove();
            FlashLight();
            if (/*Input.GetKeyDown(KeyCode.E)*/Input.GetMouseButtonDown(0)) //상호작용을 눌렀을 때
            {
                state = GameManager.PlayerState.CanHide; //숨을 수 있는 상태로 변환
                HideOff(); //나오는 행동 실행
                print("Set");
            }
        }
    }
    void Hide() //숨는 행동
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false; //충돌 방지를 위해 콜라이더를 비활성화
        Destroy(gameObject.GetComponent<Rigidbody>()); //추락 방지를 위해 리지드바디 삭제
        transform.position = HideObject.transform.position; //숨을 오브젝트의 위치로 이동
        transform.eulerAngles = HideObject.transform.eulerAngles; //숨을 오브젝트의 시야로 변환
        GameManager.instance.CanHideText.SetActive(false);
    }
    void HideOff() //나오는 행동
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = true; //콜라이더를 다시 활성화
        gameObject.AddComponent<Rigidbody>(); //리지드바디를 다시 생성하고
        rigid = GetComponent<Rigidbody>(); //리지드바디를 다시 대입해준 후
        rigid.freezeRotation = true; //리지드바디로 인해 변하는 각도를 고정
        transform.position = HideOffPos; //저장했던 위치로 이동
    }
    void CamMove() //카메라 움직임
    {
        float yRotateSize = Input.GetAxis("Mouse X") * camSpeed;
        float yRotate = transform.eulerAngles.y + yRotateSize;
        float xRotateSize = -Input.GetAxis("Mouse Y") * camSpeed;
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);
        transform.eulerAngles = new Vector3(0, yRotate, 0);
        cam.transform.eulerAngles = new Vector3(xRotate, yRotate + 90, 0);


    }
    void Moving() //이동
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0)
            //쉬프트를 누른 상태로 스태미나가 있다면
        {
            Stamina--; //스태미나를 지우면서
            MoveSpeed = RunSpeed; //더 빠르게 이동
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && Stamina < 1000)
            //쉬프트를 누르지 않고 스태미나가 있다면
        {
            Stamina += 0.5f; //스태미나를 점점 추가
            MoveSpeed = walkSpeed; //기본 속도로 이동
        }
        else //스태미나가 없다면
        {
            MoveSpeed = walkSpeed; //기본 속도로 이동
        }
        Vector3 Move = new Vector3(h * MoveSpeed, 0, v * MoveSpeed);
        Vector3 lookforward = new Vector3(-transform.forward.x, 0, -transform.forward.z).normalized;
        Vector3 lookright = new Vector3(transform.right.x, 0, transform.right.z).normalized;
        Vector3 MoveDir = lookforward * Move.x + lookright * Move.z;
        rigid.velocity = new Vector3(MoveDir.x, rigid.velocity.y, MoveDir.z);

        GameManager.instance.StaminaSlider.value = Stamina;
    }
    void FlashLight() //손전등
    {
        if (Input.GetKeyDown(KeyCode.F)) //F를 누르면
        {
            isflash = !isflash; //손전등 상태 전환
        }
        flash.SetActive(isflash); //전환한 상태로 실행
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
