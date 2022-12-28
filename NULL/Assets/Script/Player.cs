using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Camera cam; //플레이어 카메라
    public GameObject flash; //손전등 기능
    public float CamfieldValue = 60; //카메라 확대량
    public GameObject useObject; //사용 오브젝트 (유동적으로 지정)
    public GameManager.PlayerState state; //플레이어의 상태
    public PlayerStatus player;

    float xRotate; //카메라 계산
    Vector3 HideOffPos; //숨고 나올 위치

    Rigidbody rigid; //리지드바디

    RaycastHit hit;
    public float RayDistance = 2; //사거리
    private void Start()
    {
        Cursor.visible = false; //커서 지우기
        Cursor.lockState = CursorLockMode.Locked; //커서 고정
        rigid = GetComponent<Rigidbody>(); //리지드바디 대입
        StatInitial();
    }
    private void Update()
    {
        StateMove();//상태 변화
    }
    void StatInitial()
    {
        player.Stamina = 1000;
        player.Batery = 5000; //손전등 베터리
        player.MaxBatery = 5000; //최대 베터리
        player.isflash = false;
    }
    void StateMove()
    {
        if (state == GameManager.PlayerState.Normal) //노말 상태일 때
        {
            Moving();
            DrawRay();
        }
        else if (state == GameManager.PlayerState.CanHide) //숨을 수 있는 상태일 때
        {
            Moving();
            DrawRay();
            if (Input.GetMouseButtonDown(0)) //상호작용을 눌렀을 때
            {
                state = GameManager.PlayerState.Hide; //숨은 상태로 변환
                HideOffPos = transform.position; //현재 위치 저장
                Hide(); //숨는 행동 실행
                print("Hide");
            }
        }
        else if (state == GameManager.PlayerState.Hide) //숨은 상태일 때 
        {
            if (Input.GetMouseButtonDown(0)) //상호작용을 눌렀을 때
            {
                state = GameManager.PlayerState.CanHide; //숨을 수 있는 상태로 변환
                HideOff(); //나오는 행동 실행
                print("Set");
            }
        }
        else if (state == GameManager.PlayerState.CanPick) //주울 수 있는 상태일 때
        {
            Moving();
            DrawRay();
            if (Input.GetMouseButtonDown(0)) //상호작용을 눌렀을 때
            {
                GameManager.instance.haveKey = true; //열쇠를 얻고
                hit.collider.gameObject.SetActive(false); //열쇠를 지운다
            }
        }
        else if (state == GameManager.PlayerState.CanExit) //문 앞일때
        {
            Moving();
            DrawRay();
            if (Input.GetMouseButtonDown(0)) //상호작용을 눌렀을 때
            {
                if (GameManager.instance.haveKey) //열쇠가 있으면
                {
                    GameManager.instance.Ending(); //탈출
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
        //쉬프트를 누른 상태로 스태미나가 있다면
        {
            player.Stamina--; //스태미나를 지우면서
            player.MoveSpeed = player.RunSpeed; //더 빠르게 이동
            CamfieldValue = Mathf.Lerp(CamfieldValue, 75, 0.1f);
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && player.Stamina < 1000)
        //쉬프트를 누르지 않고 스태미나가 있다면
        {
            player.Stamina += 0.5f; //스태미나를 점점 추가
            player.MoveSpeed = player.walkSpeed; //기본 속도로 이동
            CamfieldValue = Mathf.Lerp(CamfieldValue, 60, 0.1f);
        }
        else //스태미나가 없다면
        {
            player.MoveSpeed = player.walkSpeed; //기본 속도로 이동
            CamfieldValue = Mathf.Lerp(CamfieldValue, 60, 0.1f);
        }
        GameManager.instance.StaminaSlider.value = player.Stamina;
        cam.fieldOfView = CamfieldValue;
    }
    void Hide() //숨는 행동
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false; //충돌 방지를 위해 콜라이더를 비활성화
        Destroy(gameObject.GetComponent<Rigidbody>()); //추락 방지를 위해 리지드바디 삭제
        transform.position = useObject.transform.position; //숨을 오브젝트의 위치로 이동
        transform.eulerAngles = useObject.transform.eulerAngles; //숨을 오브젝트의 시야로 변환
        GameManager.instance.CanText.enabled = false;
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
        float yRotateSize = Input.GetAxis("Mouse X") * player.camSpeed;
        float yRotate = transform.eulerAngles.y + yRotateSize;
        float xRotateSize = -Input.GetAxis("Mouse Y") * player.camSpeed;
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);
        transform.eulerAngles = new Vector3(0, yRotate, 0);
        cam.transform.eulerAngles = new Vector3(xRotate, yRotate + 90, 0);


    }
    void Moving() //이동
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 Move = new Vector3(h * player.MoveSpeed, 0, v * player.MoveSpeed);
        Vector3 lookforward = new Vector3(-transform.forward.x, 0, -transform.forward.z).normalized;
        Vector3 lookright = new Vector3(transform.right.x, 0, transform.right.z).normalized;
        Vector3 MoveDir = lookforward * Move.x + lookright * Move.z;
        rigid.velocity = new Vector3(MoveDir.x, rigid.velocity.y, MoveDir.z);
    }
    void FlashLight() //손전등
    {
        if (Input.GetKeyDown(KeyCode.F)) //F를 누르면
        {
            player.isflash = !player.isflash; //손전등 상태 전환
        }
        
        flash.SetActive(player.isflash); //전환한 상태로 실행
    }
    void DrawRay() //사거리
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, RayDistance))
        {
            switch (hit.collider.tag)
            {
                case "HideObject": //사거리에 들어온 것이 숨는 물체일 때
                    UseObjectFunction(GameManager.PlayerState.CanHide, "숨기");
                    //숨기 상태로 전환 & 숨기 텍스트
                    break;
                case "Exit": //사거리에 들어온 것이 출구일 때
                    state = GameManager.PlayerState.CanExit;
                    GameManager.instance.CanText.text = "열기"; //텍스트 열기로 변환
                    GameManager.instance.CanText.enabled = true; //열기 텍스트 활성화
                    break;
                case "Key": //사거리에 들어온 것이 열쇠일 때
                    UseObjectFunction(GameManager.PlayerState.CanPick, "줍기");
                    //줍기 상태로 전환 & 줍기 텍스트
                    break;
            }
        }
        else //사거리에 들어가지 않았을 때
        {
            useObject = null; //숨을 오브젝트는 없음
            state = GameManager.PlayerState.Normal; //기본 상태로 전환
            GameManager.instance.CanText.enabled = false;//숨기 텍스트 비활성화
        }
    }
    void UseObjectFunction(GameManager.PlayerState UseState,string text)
    {
        useObject = hit.collider.gameObject; //오브젝트 설정
        state = UseState; //사용 상태로 전환
        GameManager.instance.CanText.text = text; //텍스트 변환
        GameManager.instance.CanText.enabled = true; //텍스트 활성화
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * RayDistance, Color.red);
    }
}
