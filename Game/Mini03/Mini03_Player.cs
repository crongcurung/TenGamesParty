using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class Mini03_Player : MonoBehaviour         // 미니03 플레이어 부착됨
{
    [SerializeField] Joystic joystic;

    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // 점수를 서버에 등록시킬 수 있는 버튼

	Animator anim;                       // 애니메이터는 플레이어와 몬스터가 공통으로 지닐 가능성 높음
    Rigidbody rigid;                 // 리지드바디는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    NavMeshAgent nma;
    [SerializeField] protected float speed;        // 이동 속도 변수는 플레이어와 몬스터가 공통으로 지닐 가능성 높음
    Vector3 rotBan = Vector3.zero;                      // 리지드바디로 이동 시, 캐릭터가 계속 회전하는 문제를 막기 위한 변수
    Vector3 moveDir;                     // 이동 변수는 플레이어가 지닐 가능성 높음

    Renderer playerRenderer;
    Collider playerCol;

    GameObject playerOfBin;
    Vector3 originPos;

    [SerializeField] Mini03_Spawn mini03_Spawn;

    [SerializeField] Button Right_Button;
    [SerializeField] Image Right_image;
    [SerializeField] Sprite[] sprite_Array;       // 0 : 통 안, 1 : 통 밖


    public bool isInBin = false;            // 현재 플레이어가 쓰레기통에 들어가 있냐고 물어보는 변수

    bool isMonLight = false;

    GameObject player_Light;
    Vector3 open_Pos;
    Quaternion open_Rot;
    Vector3 closed_Pos;
    Quaternion closed_Rot;

    string hor_Text;
    string ver_Text;

    string tag01;
    string tag02;
    string tag03;
    string tag04;
    string tag05;
    string tag06;
    string tag07;

    int runId;

    void Start()
    {
        playerCol = GetComponent<Collider>();      // 플레이어의 콜라이더를 받는다.
        playerRenderer = transform.GetChild(1).GetComponent<Renderer>();       // 플레이어의 렌더러를 받는다.
        nma = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();              // 플레이어의 리지드 바디를 가지고 온다.
        anim = GetComponent<Animator>();                // 플레이어의 애니메이션을 가지고 온다.

        hor_Text = "Horizontal";
        ver_Text = "Vertical";

        tag01 = "Note";
        tag02 = "Player";
        tag03 = "Spring";
        tag04 = "Monster";
        tag05 = "Coin";
        tag06 = "BlackHole";             //
        tag07 = "Bear";

        runId = Animator.StringToHash("isRun");

        Right_Button.interactable = false;              // 처음에는 쓰레기통에 닿을리 없으니 버튼을 비활성화 시킴

        Right_image.sprite = sprite_Array[0];

        player_Light = transform.GetChild(2).gameObject;
        open_Pos = new Vector3(-0.3f, 1.6f, 0);
        open_Rot = Quaternion.Euler(new Vector3(-75, -90, 90));
        closed_Pos = new Vector3(0, 1.6f, 0);
        closed_Rot = Quaternion.Euler(new Vector3(-90, 0, 0));

        Stage_Setting();

        AudioMng.ins.Play_BG("Mini03_B");
    }

    void FixedUpdate()
    {
        rigid.velocity = moveDir * speed;          // 벨로시티로 이동

        if (!moveDir.x.Equals(0) || !moveDir.z.Equals(0))               // wasd 아무거나라도 움직인다면..
        {
            anim.SetBool(runId, true);         // 움직이는 애니메이션 실행
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z)), 0.2f);
            // 플레이어의 회전 값
        }
        else
        {
            anim.SetBool(runId, false);       // 멈추는 애니메이션 실행
            rigid.angularVelocity = rotBan;          // 플레이어가 무언가에 충돌시 계속 회전하는 문제가 있어서 막아버림
        }
    }

    void Update()
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

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
			Press_RightButton();
		}
    }


	public void Press_RightButton()         // 메인 버튼, 쓰레기통에 들어가기
	{
        if (playerOfBin != null)          // 쓰레기 통에 닿고 있다면..
        {
            moveDir.x = 0.0f;
            moveDir.z = 0.0f;

            if (isInBin.Equals(false))           // 현재 플레이어가 쓰레기통에 안 들어간 상태라면..(이제 들어가야 한다!)
            {
                if (isMonLight.Equals(true))     // 몬스터 불빛의 닿는 상태라면..
                {
                    return;
                }

                AudioMng.ins.PlayEffect("Click02");

                isInBin = true;

                gameObject.tag = tag01;         // 플레이어 테그 바꾸기
                gameObject.layer = 1;            // 플레이어 레이어 바꾸기

                Right_image.sprite = sprite_Array[1];
                originPos = transform.position;   // 현재 플레이어의 위치를 담아두기

                transform.position = new Vector3(playerOfBin.transform.position.x, 0.0f, playerOfBin.transform.position.z);
                playerRenderer.enabled = false;   // 플레이어 렌더러를 없애버임

                nma.enabled = false;         // 네브메쉬 끄기
                playerCol.enabled = false;           // 콜라이더 끄기

                playerOfBin.transform.GetChild(1).transform.localPosition = closed_Pos;
                playerOfBin.transform.GetChild(1).transform.localRotation = closed_Rot;
                player_Light.SetActive(false);

                speed = 0.0f;
            }
            else                                // 현재 플레이어가 쓰레기통에 들어간 상태라면..(빠져 나와야 한다!)
            {
                AudioMng.ins.PlayEffect("Back");

                isInBin = false;

                gameObject.tag = tag02;        // 태그 원래대로 하기
                gameObject.layer = 6;             // 레이어 원래대로 하기

                Right_image.sprite = sprite_Array[0];

                transform.position = originPos;   // 담아두었던 위치에 플레이어 위치로 옮김
                playerRenderer.enabled = true;   // 플레이어 렌더러를 다시 살림

                nma.enabled = true;          // 네브메쉬 켜기
                playerCol.enabled = true;            // 콜라이더 켜기

                playerOfBin.transform.GetChild(1).transform.localPosition = open_Pos;
                playerOfBin.transform.GetChild(1).transform.localRotation = open_Rot;
                player_Light.SetActive(true);

                speed = 4.0f;
            }
        }
        else                                   // 쓰레기 통에 닿지 않았다면...(그냥 무시)
        {

        }
	}

    void Stage_Setting()        // 스테이지를 스폰하고, 플레이어 캐릭터를 이동시키는 함수
    {
        nma.enabled = false;                // 네브메쉬 끄기(플레이어를 다른 스테이지로 옮겨야 해서)
        Transform stageTrans = mini03_Spawn.StageSpawn();     // 스테이지를 스폰한다.
        transform.position = stageTrans.GetChild(2).position;      // 플레이어의 위치를 현재 스폰된 스테이지의 start point로 이동시킨다.
        nma.enabled = true;                 // 네브매쉬 다시 켜기(플레이어 이 떄쯤이면 다른 스테이지로 옮겨져 있어서 다시 켜기)
    }


    ///////////////// 트리거 구역......


    public void GameOver()
    {
        Result_Panel.SetActive(true);     // 결과 창이 띄게 한다.
        Game_Panel.SetActive(true);

        Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + mini03_Spawn.stageInt.ToString();

        if (Main.ins.nowPlayer.maxScore_List[2] >= mini03_Spawn.stageInt)    // 최고점을 못 넘은다면...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[2].ToString();
        }
        else        // 최고점을 넘은 경우 (신기록)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + mini03_Spawn.stageInt.ToString();

            Main.ins.nowPlayer.maxScore_List[2] = mini03_Spawn.stageInt;
            Main.ins.SaveData();

			GPGS_Button.interactable = true;        // 신기록을 세우면 점수 서버 등록 버튼을 활성화한다. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // 배경음악을 끈다.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;     // 멈춤
    }

	public void Press_GPGS_03()   // 신기록 시, 서버에 등록
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no3, mini03_Spawn.stageInt);   // 점수를 서버에 등록
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no3);          // 리더보드를 띄운다.
	}


	void OnTriggerEnter(Collider other)
    {
        if (isInBin.Equals(false))                     // 플레이어가 쓰레기통에 안 들어갔다면..
        {
            if (other.CompareTag(tag03))              // 다음 스테이지로 가는 바닥을 밟으면...
            {

                AudioMng.ins.PlayEffect("Score_Up");
                Stage_Setting();                        // 다음 스테이지 스폰 ㄱㄱ
            }

            if (other.CompareTag(tag04))                    // 몬스터에 닿으면...
            {

                if (other.name.Equals("Pipe02"))         // 타워에 걸리면...
                {
                    other.transform.parent.LookAt(transform.position);   // 타워를 플레이어를 보도록 한다.
                }


                GameOver();
            }

            if (other.CompareTag(tag01))    // 골인 지점 바로 앞에 닿으면..
            {
                other.transform.GetChild(0).gameObject.SetActive(true);        // 초록색 빛이 위로 올라가는 파티클 나오게 한다.
            }

            if (other.CompareTag(tag05))    // 소리 영역에 닿으면...
            {
                AudioMng.ins.PlayEffect("Check");         // 딸깍 소리 나게 한다.
            }
        }
    }

    void OnTriggerStay(Collider other)          // 쓰레기통 안으로 가면
    {
        if (isInBin.Equals(false))                     // 플레이어가 쓰레기통에 안 들어갔다면..
        {
            if (other.CompareTag(tag06))
            {
                playerOfBin = other.gameObject;     // 닿은 쓰레기통을 담는 오브젝트에 넣기

                Right_Button.interactable = true;
            }
            
            if (other.CompareTag(tag07))    // 몬스터 손전등에 닿는다면..
            {
                isMonLight = true;                   // 몬스터 손전등에 닿았다고 알림
            }
        }
    }

    void OnTriggerExit(Collider other)          // 쓰레기통 밖으로 나가면
    {
        if (other.CompareTag(tag06))    // 쓰레기통과 닿지 않는다면..
        {
            playerOfBin = null;             // 쓰레기통 트리거에서 빠져나왔다면 닿은 쓰레기통을 담는 오브젝트에 null 넣기

            Right_Button.interactable = false;
        }

        if (other.CompareTag(tag07))    // 몬스터 손전등에 안 닿는다면..
        {
            isMonLight = false;                   // 몬스터 손전등에 안 닿았다고 알림
        }
    }
}
