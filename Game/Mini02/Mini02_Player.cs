using UnityEngine;
using UnityEngine.UI;

public class Mini02_Player : MonoBehaviour
{
    [SerializeField] Joystic joystic;

    public bool isHoleOrStar = false;       // 현재 도넛 모양인지, 스타 모양인지 묻는 변수
    public bool isOvenOrFryer = false;      // 현재 도넛이 오븐 상태였는지, 튀김 상태였는지 묻는 변수

    public int playerCompleteInt = 0;           // 0 : 도우, 1 : 틀, 2 : 오븐, 튀김, 3 : 토핑

    bool isMoving = false;        // 현재 플레이어가 방향키를 움직이고 있는지 알아보는 변수
    bool isMove = false;

    Vector3 rayPos;         // 레이 크기를 조절하는 변수
    Vector3 posUp;          // 플레이어를 더이상 위로 못 올라가게 하는 변수
    Vector3 posDown;        // 플레이어를 더이상 아래로 못 내려가게 하는 변수

    Vector3 dough_Pos;       // 키친 카메라의 위치 값과, 회전 값
    Quaternion dough_Rot;
    Vector3 frame_Pos;
    Quaternion frame_Rot;
    Vector3 oven_Pos;
    Quaternion oven_Rot;
    Vector3 fry_Pos;
    Quaternion fry_Rot;
    Vector3 topping_Pos;
    Quaternion topping_Rot;

    [SerializeField] Transform kitchenCamera;     // 키친 카메라
    int shelfNum;                   // 현재 플레이어가 어느 카운터에 있는지 알리는 변수

    bool isPanel = false;              // 플레이어가 다른 것을 하고 있는지 묻는 변수

    [SerializeField] GameObject RightButton;              // 오른쪽 버튼(메인)

    [SerializeField] Image Right_Image;

    [SerializeField] GameObject ScorePanel;               // 스코어와 시간이 표현되는 패널(메인)
    [SerializeField] GameObject[] Donut_Panel;            // 각 카운트들의 패널 0 : , 1 : , 2 : , 3 : , 4 :

    public int scoreInt = 0;

    Vector3 origin_CameraPos;
    Vector3 moveDir;                     // 이동 변수는 플레이어가 지닐 가능성 높음

    string tag01;
    string tag02;
    string tag03;
    string tag04;
    string tag05;

    string hor_Text;
    string ver_Text;

    void Start()
	{
        playerCompleteInt = 0;

        tag01 = "BlackHole";
        tag02 = "Bear";
        tag03 = "Spring";
        tag04 = "Honey";
        tag05 = "Cushion";

        hor_Text = "Horizontal";
        ver_Text = "Vertical";

        dough_Pos = new Vector3(-139.9f, 4.2f, 0.83f);
        dough_Rot = Quaternion.Euler(new Vector3(56.742f, 0, 0));
        frame_Pos = new Vector3(-100.62f, 8.66f, -5.55f);
        frame_Rot = Quaternion.Euler(new Vector3(46.458f, 0, 0));
        oven_Pos = new Vector3(-65.39f, 2.57f, 16.04f);
        oven_Rot = Quaternion.Euler(new Vector3(18.61f, 0, 0));
        fry_Pos = new Vector3(-64.98f, 6.203f, 21.787f);
        fry_Rot = Quaternion.Euler(new Vector3(60.858f, 0, 0));
        topping_Pos = new Vector3(-9.91f, 8.54f, 42.39f);
        topping_Rot = Quaternion.Euler(new Vector3(90.0f, 0, 0));

        origin_CameraPos = kitchenCamera.position;

        rayPos = new Vector3(0, 0.5f, 0);       // 레이 길이 조정 값

        posUp = new Vector3(0, 0, 2);           // 이동 조정 값
        posDown = new Vector3(0, 0, -2);        
        OneShotRay();                        // 처음 블록이 어딘지 알아본다.

        AudioMng.ins.Play_BG("Mini02_B");
    }


	void Update()
	{
        if (isPanel.Equals(false))
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) && isPanel.Equals(false))
        {
            PressShelf();
        }
    }

    

    void Move()         // 이동 함수를 부모걸로 씀
	{
#if (UNITY_EDITOR)
        float hor = Input.GetAxis(hor_Text);
        float ver = Input.GetAxis(ver_Text);
#elif (UNITY_IOS || UNITY_ANDROID)
		float hor = joystic.Horizontal;
		float ver = joystic.Vertical;
#endif

        moveDir.x = hor;
        moveDir.z = ver;

        isMoving = (!moveDir.z.Equals(0) || !moveDir.x.Equals(0));   // 조이스틱을 움직이면 숫자를(0 ~ 1) 변수에 등록

        if (isMoving.Equals(false))   //  움직이지 않았다면...
        {
            isMove = true;       // 초기화...?
        }
        else
        {
            if (moveDir.z > 0.3f && isMove.Equals(true) && transform.position.z <= 3.5f)   // 위 방향키를 움직이고, 움직이는 변수가 true이면, 그리고 플레이어 이동 제한
            {
                isMove = false;                   // 뚝뚝 끊기 위해 움직이는 변수를 false로 둔다.(결국 한번 움직이면, 끝내게 된다....)

                transform.position += posUp;    // 한 칸씩 움직이도록 이동
                OneShotRay();
            }
            else if (moveDir.z < -0.3f && isMove.Equals(true) && transform.position.z >= -3.5f)           // 아래 방향키를 움직이고, 움직이는 변수가  true이면
            {
                isMove = false;                    // 뚝뚝 끊기 위해 움직이는 변수를 false로 둔다.(결국 한번 움직이면, 끝내게 된다....)

                transform.position += posDown;    // 한 칸씩 움직이도록 이동
                OneShotRay();
            }
        }
    }


    void OneShotRay()            // 레이를 한번만 쏠려고 만듬
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(this.transform.position + rayPos, this.transform.forward, out hitInfo))      // 앞으로 레이를 쏜다.
        {
            if (hitInfo.transform.CompareTag(tag01))        // 도우 선반
            {
                shelfNum = 0;
            }
            else if (hitInfo.transform.CompareTag(tag02))             // 틀 선반
            {
                shelfNum = 1;
            }
            else if (hitInfo.transform.CompareTag(tag03))           // 오븐 선반
            {
                shelfNum = 2;
            }
            else if (hitInfo.transform.CompareTag(tag04))            // 튀김 선반
            {
                shelfNum = 3;
            }
            else if (hitInfo.transform.CompareTag(tag05))          // 토핑 선반
            {
                shelfNum = 4;
            }

        }
    }

    public void PressShelf()         // 버튼을 눌렀을 경우
    {
        switch (shelfNum)                  // 닿은 카운트에 따라
        {
            case 0:        // 도우 선반
                isPanel = true;                      // 패널이 열렸다고 알림
                ScorePanel.SetActive(false);         // 스코어 패널을 끔
                RightButton.SetActive(false);        // 오른쪽 버튼을 끔

                AudioMng.ins.PlayEffect("Enter");    // 패널 들어가는 소리

                kitchenCamera.GetComponent<Camera>().depth = 1;

                kitchenCamera.position = dough_Pos;    // 키친 카메라를 도우 키친쪽으로 이동시킴
                kitchenCamera.rotation = dough_Rot;
                Donut_Panel[0].SetActive(true);        // 도우 패널을 연다.
                break;
            case 1:             // 틀 선반
                if (playerCompleteInt.Equals(1))
                {
                    isPanel = true;                      // 패널이 열렸다고 알림
                    ScorePanel.SetActive(false);         // 스코어 패널을 끔
                    RightButton.SetActive(false);        // 오른쪽 버튼을 끔

                    AudioMng.ins.PlayEffect("Enter");    // 패널 들어가는 소리

                    kitchenCamera.position = frame_Pos;    // 키친 카메라를 틀 키친쪽으로 이동시킴
                    kitchenCamera.rotation = frame_Rot;
                    Donut_Panel[1].SetActive(true);        // 틀 패널을 연다.
                }
                
                break;
            case 2:           // 오븐 선반
                if (playerCompleteInt.Equals(0) || playerCompleteInt.Equals(1) || playerCompleteInt.Equals(3))
                {
                    break;
                }
                isPanel = true;                      // 패널이 열렸다고 알림
                ScorePanel.SetActive(false);         // 스코어 패널을 끔
                RightButton.SetActive(false);        // 오른쪽 버튼을 끔

                AudioMng.ins.PlayEffect("Enter");    // 패널 들어가는 소리

                kitchenCamera.position = oven_Pos;    // 키친 카메라를 오븐 키친쪽으로 이동시킴
                kitchenCamera.rotation = oven_Rot;
                Donut_Panel[3].SetActive(true);        // 오븐 패널을 연다.
                break;
            case 3:            // 튀김 선반
                if (playerCompleteInt.Equals(0) || playerCompleteInt.Equals(1) || playerCompleteInt.Equals(3))
                {
                    break;
                }
                isPanel = true;                      // 패널이 열렸다고 알림
                ScorePanel.SetActive(false);         // 스코어 패널을 끔
                RightButton.SetActive(false);        // 오른쪽 버튼을 끔

                AudioMng.ins.PlayEffect("Enter");    // 패널 들어가는 소리

                kitchenCamera.position = fry_Pos;    // 키친 카메라를 튀김 키친쪽으로 이동시킴
                kitchenCamera.rotation = fry_Rot;
                Donut_Panel[2].SetActive(true);        // 튀김 패널을 연다.
                break;
            default:          // 토핑 선반
                if (playerCompleteInt.Equals(3))
                {
                    isPanel = true;                      // 패널이 열렸다고 알림
                    ScorePanel.SetActive(false);         // 스코어 패널을 끔
                    RightButton.SetActive(false);        // 오른쪽 버튼을 끔

                    AudioMng.ins.PlayEffect("Enter");    // 패널 들어가는 소리

                    kitchenCamera.position = topping_Pos;    // 키친 카메라를 토핑 키친쪽으로 이동시킴
                    kitchenCamera.rotation = topping_Rot;
                    Donut_Panel[4].SetActive(true);        // 토핑 패널을 연다.
                }
                break;
        }


    }

    public void Origin_Panel()         // 다른 패널에서 빠져 나온다면...
    {
        AudioMng.ins.LoopEffect(false);
        AudioMng.ins.PlayEffect("Back");      // 토핑 실패

        RightButton.SetActive(true);                        // 오른쪽 버튼을 활성화
        kitchenCamera.position = origin_CameraPos;          // 키친 카메라를 디폴트쪽으로 옮김
        ScorePanel.SetActive(true);                         // 스코어 패널을 활성화
        isPanel = false;                                    // 패널이 안켜져 있다고 알림
    }

}


