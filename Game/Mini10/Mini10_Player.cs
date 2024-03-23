using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini10_Player : MonoBehaviour
{
    [SerializeField] Joystic joystic;

    [SerializeField] AudioSource audio_Black;

    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // 점수를 서버에 등록시킬 수 있는 버튼

	[SerializeField] Mini10_Stage mini10_Stage;
    [SerializeField] Mini10_Camera mini10_Camera;

    [SerializeField] Image Right_Image;
    [SerializeField] Sprite[] sprite_Array;   // 0 : 들어올리기, 1 : 부딪히기, 2 : 코인 내려 놓기, 4 : 지도, 5 : 지도 풀기

    Vector3 moveDir;                     // 이동 변수는 플레이어가 지닐 가능성 높음
    Animator anim;                       // 애니메이터는 플레이어와 몬스터가 공통으로 지닐 가능성 높음
    Rigidbody rigid;                 // 리지드바디는 플레이어와 몬스터가 공통으로 지닐 가능성 높음
    Vector3 rotBan = Vector3.zero;                      // 리지드바디로 이동 시, 캐릭터가 계속 회전하는 문제를 막기 위한 변수

    [SerializeField] float speed;        // 이동 속도 변수는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    GameObject nearObject;          // 플레이어와 가까운 곳에 아이템을 받는 변수
    Transform upTrans;              // 플레이어 위에 아이템을 들고 있을 위치
    Transform nearNote;             // 플레이어가 밟고 있는 발판

    bool isItem;                    // 플레이어가 아이템을 들고 있는지 체크
    bool isDonut;                   // 플레이어가 도넛을 들고 있는지 체크


    public bool isCameraMove = false;      // 카메라가 움직이냐고 묻는 변수
    public bool isMapCamera = false;

    bool stopPlayer = true;            // 플레이어가 멈춰있는지, 움직이고 있는 묻는 변수

    bool startCube = false;          // 처음 발판에서 나오면 false로 바꿈, 맵보기 위한 변수
    bool isFirst = false;            // 처음 발판을 위한 불 변수

    bool isEnd = false;              // 플레이어가 끝났는지 묻는 변수

    Transform blackHoleTrans;      // 블랙홀 영역(9칸)에 닿았는지 묻는 변수

    int runId;
    int attackId;

    string tag01;
    string tag02;
    string tag03;
    string tag04;
    string tag05;
    string tag06;
    string tag07;
    string tag08;
    string tag09;
    string tag10;

    string hor_Text;
    string ver_Text;

    bool endBool = false;
    bool isSpring = false;    // 스프링에 닿았냐?

    void Start()
    {
        isItem = false;
        isDonut = false;

        runId = Animator.StringToHash("isRun");
        attackId = Animator.StringToHash("isAttack");

        tag01 = "Coin";
        tag02 = "Respawn";
        tag03 = "Spring";
        tag04 = "Cushion";
        tag05 = "Bear";
        tag06 = "Honey";
        tag07 = "BlackHole";
        tag08 = "Monster";
        tag09 = "Note";
        tag10 = "Finish";

        hor_Text = "Horizontal";
        ver_Text = "Vertical";

        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        nearObject = null;
        upTrans = transform.GetChild(2).transform;

        StartCoroutine(rayCoroutine());                // 밑으로 레이를 쏘는 코루틴 실행

        AudioMng.ins.Play_BG("Mini10_B");
    }



	void FixedUpdate()
    {
        rigid.velocity = moveDir * speed;          // 벨로시티로 이동

        if (!moveDir.x.Equals(0) || !moveDir.z.Equals(0))               // wasd 아무거나라도 움직인다면..
        {
            anim.SetBool(runId, true);         // 움직이는 애니메이션 실행
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z)), 0.2f);
            // 플레이어의 회전 값

            stopPlayer = false;                              // 플레이어가 움직이고 있다고 알려줌
        }
        else
        {
            anim.SetBool(runId, false);       // 멈추는 애니메이션 실행
            rigid.angularVelocity = rotBan;          // 플레이어가 무언가에 충돌시 계속 회전하는 문제가 있어서 막아버림

            stopPlayer = true;                 // 플레이어가 멈춰있다고 알려줌
        }
    }

    void Update()
    {
        if (isEnd.Equals(true))         // 플레이어가 죽었다면 
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Press_RightButton();   
        }

        if (isCameraMove.Equals(false))         // 카메라가 이동 중이 아니라면...
        {
            if (isMapCamera.Equals(false))
            {
                Move();
            }
        }
        else                       // 카메라가 이동 중이라면..
        {
            moveDir.x = 0;         // 플레이어를 멈춘다..
            moveDir.z = 0;

        }

        if (blackHoleTrans != null && stopPlayer.Equals(true) && isCameraMove.Equals(false) && isMapCamera.Equals(false))   // 블랙홀 오브젝트가 비어있지 않고, 플레이어가 멈춰있고, 맵 카메라가 작동중이지 않을때...
        {
            transform.position += (blackHoleTrans.position - transform.position).normalized * Time.deltaTime * 0.5f;
            // 플레이어를 블랙홀 쪽으로 이동
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
    }

    

    public void MinusHp(bool failBool)                  ////////////////////////////////////////////////////////////////////////////////////////////////
    {
        if (failBool.Equals(false))      // 떨어진다.
        {
            transform.GetComponent<Collider>().isTrigger = true;
            rigid.constraints = RigidbodyConstraints.None;                            // 리지드 바디를 고정을 없애 버림
            rigid.AddForce(Vector3.down * 5000f);        // 플레이어를 밑으로 보내버림

            speed = 0.0f;
            isEnd = true;

            StartCoroutine(EndGame_Falling());
        }
        else                             // 아이템이 안 맞는 경우
        {
            speed = 0.0f;
            isEnd = true;

            End_Game();
        }
        

        
    }

    IEnumerator EndGame_Falling()
    {
        yield return new WaitForSeconds(2.0f);
        End_Game();
    }

	int thisInt = 0;

	public void End_Game()                   // 게임이 끝났을 때 함수
    {
        Result_Panel.SetActive(true);
        Game_Panel.SetActive(true);

        int scoreInt = mini10_Stage.stageInt;

        Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + scoreInt.ToString();

        if (Main.ins.nowPlayer.maxScore_List[9] >= scoreInt)    // 최고점을 못 넘은다면...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[9].ToString();
        }
        else        // 최고점을 넘은 경우 (신기록)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + scoreInt.ToString();

            Main.ins.nowPlayer.maxScore_List[9] = scoreInt;
            Main.ins.SaveData();

            thisInt = scoreInt;
			GPGS_Button.interactable = true;        // 신기록을 세우면 점수 서버 등록 버튼을 활성화한다. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // 배경음악을 끈다.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;
    }


	public void Press_GPGS_10()   // 신기록 시, 서버에 등록
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no10, thisInt);   // 점수를 서버에 등록
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no10);          // 리더보드를 띄운다.
	}


	///////////////////////////////
	// 지역 함수 구역

	public void Press_RightButton()             // 엔터를 누른다면..
    {
        if (isMapCamera.Equals(true))
        {
            mini10_Camera.StopMapCamera();

            moveDir.x = 0;         // 플레이어를 멈춘다..
            moveDir.z = 0;


            Right_Image.sprite = sprite_Array[0];   // 들어올리기 이미지
        }

        if (isItem.Equals(true))         // 아이템을 들고 있다면..
        {
            

            if (isDonut.Equals(true))
            {
                

                if (nearNote != null && nearNote.childCount.Equals(1))       // 현재 플레이어가 도넛을 들고 있고, 밟고 있는 발판이 아이템이 없다면
                {
                    

                    anim.SetBool(attackId, false);
                    isItem = false;     // 아이템을 들고 있지 않다고 알린다.
                    upTrans.GetChild(0).position = nearNote.position;            // 발판에 있는 아이템 포스 위치에 들고 있던 도넛을 넣는다.
                    upTrans.GetChild(0).rotation = Quaternion.Euler(Vector3.zero);
                    upTrans.GetChild(0).parent = nearNote.transform;             // 들고 있던 도넛의 부모를 발판 아이템 포스로 바꾼다.

                    Right_Image.sprite = sprite_Array[0]; 

                    isDonut = false;        // 도넛을 들고 있지 않다고 알린다.
                }
            }
            else
            {

            }
        }
        else                            // 아이템을 들지 있지 않다면..       isItem == false
        {

            if (nearObject != null)
            {

                isItem = true;              // 아이템을 들고 있다고 알림
                anim.SetBool(attackId, true);

                AudioMng.ins.PlayEffect("Cloud");    // 아이템 들기

                nearObject.transform.position = upTrans.position;     // 플레이어 위에 아이템을 올린다.
                nearObject.transform.parent = upTrans;                // 아이템의 부모를 바꾼다.

                if (nearObject.CompareTag(tag01))    // 방금 든 아이템이 도넛이라면..
                {
                    isDonut = true;                   // 도넛을 들고 있다고 알림
                    Right_Image.sprite = sprite_Array[2];     // 내려놓기 이미지
                }
                else
                {
                    Right_Image.sprite = sprite_Array[1];      // 충돌 이미지
                }
            }


            if (startCube.Equals(false))        // 스타트 큐브가 닿았다면..
            {
                if (isCameraMove.Equals(false) && nearNote.CompareTag(tag02))     // 카메라가 이동하지 않고, 밟고 있는 것이 스타트 큐브라면..
                {
                    isFirst = true;
                    startCube = true;

                    moveDir.x = 0;         // 플레이어를 멈춘다..
                    moveDir.z = 0;


                    AudioMng.ins.PlayEffect("Click");    // 지도 펼침
                    mini10_Camera.MapCamera(nearNote.GetChild(0).transform);          // 맵 카메라 발동

                    Right_Image.sprite = sprite_Array[4];   // 지도 풀기 이미지
                }
            }
        }
    }


    void StepCube(Transform stepCube)             // 지금 밟은 발판을 체크한다.
    {
        int cubeCount = stepCube.GetComponent<Mini10_Cube>().StepPlayer();      // 밟은 발판의 숫자를 줄인다...
        if (cubeCount.Equals(0))                                                // 위에서 받아온 숫자가 0이면 
        {
            nearNote = null;    // 가까운 발판이 없다고 알림
        }


        if (stepCube.childCount.Equals(2))   // 방해물 혹은 아이템이 있는 곳이다..
        {
            if (stepCube.GetChild(1).gameObject.layer.Equals(8))      // 아이템들...
            {
                if (isItem.Equals(false))       // 아이템을 안 들고 있었다면,,
                {
                    nearObject = stepCube.GetChild(1).gameObject;      // 가까운 오브젝트에 아이템을 일단 넣는다!
                }
            }
            else         // 방해물...
            {
                if (stepCube.GetChild(1).CompareTag(tag03))         // 스프링에 닿으면...
                {
                    CheckItem(stepCube, tag04, 0);    // 아이템이랑 방해 오브젝트 비교
                }
                else if (stepCube.GetChild(1).CompareTag(tag05))      // 곰에 닿으면...
                {
                    CheckItem(stepCube, tag06, 1);    // 아이템이랑 방해 오브젝트 비교
                }
                else if (stepCube.GetChild(1).CompareTag(tag07)) // 블랙홀에 닿으면...
                {
                    CheckItem(stepCube, tag08, 2);    // 아이템이랑 방해 오브젝트 비교

                    blackHoleTrans = null;     // 닿은 블랙홀 영역이 없다고 알림(블랙홀에 끝나든 말든 일단 실행)
                    speed = 3.5f;              // 속도를 원래대로 바꿈
                }
            }
        }
        else     // 아무 아이템이랑 방해물이 없는 큐브라면..
        {
            if (isDonut.Equals(false))   // 도넛을 안들고 있다면
            {
                nearObject = null;        // 가까운 오브젝트가 없다고 알린다.
            }
        }
    }

    void CheckItem(Transform stepCube, string tagString, int itemId)       // 아이템을 비교하는 함수
    {
        

        if (isItem.Equals(true) && upTrans.GetChild(0).CompareTag(tagString))   // 플레이어가 아이템을 들고 있고, 태그끼리 맞는다면..
        {
            Destroy(upTrans.GetChild(0).gameObject);     // 플레이어가 들고 있던 아이템을 없앤다.
            Destroy(stepCube.GetChild(1).gameObject);    // 발판 위에 있던 방해물을 없앤다.

            AudioMng.ins.PlayEffect("Score_Up");    // 아이템 맞음
            Right_Image.sprite = sprite_Array[0];   // 들어올리기 이미지

            isItem = false;                              // 아이템을 들고 있지 않다고 알린다.
            nearObject = null;

            anim.SetBool(attackId, false);             // 들고 다니는 애니메이션 중단
        }
        else       // 플레이어가 아이템을 들고 있지 않거나, 태그가 안맞는다면..
        {
            if (itemId.Equals(0))       // 스프링
            {
                isSpring = true;
                rigid.AddForce(Vector3.up * 100000.0f);     // 끝냄

                AudioMng.ins.PlayEffect("DdiYong");    // 스프링에 닿음
            }
            else if (itemId.Equals(1))  // 곰
            {
                transform.GetChild(1).GetComponent<Renderer>().enabled = false;      // 플레이어 랜더 비활성화
                transform.position = stepCube.GetChild(1).transform.position;
                isEnd = true;    // 끝났다고 알림

                moveDir.x = 0;   // 플레이어 멈춤
                moveDir.z = 0;

                AudioMng.ins.PlayEffect("BearGrowl");    // 곰에 닿음
            }
            else                        // 블랙홀
            {
                transform.GetChild(1).GetComponent<Renderer>().enabled = false;      // 플레이어 랜더 비활성화
                isEnd = true;    // 끝났다고 알림
                moveDir.x = 0;   // 플레이어 멈춤
                moveDir.z = 0;

                //AudioMng.ins.Effect_Speed(0.3f);
                //AudioMng.ins.PlayEffect("Meow");    // 블랙홀에 닿음

                audio_Black.Play();
            }

            StartCoroutine(FailCoroutine(true));
        }
    }

    /////////////////////////////////////////////////////////
    // 코루틴 구역


    IEnumerator FailCoroutine(bool isBool)          // 게임이 끝났을떄 나는 소리
    {
        endBool = true;
        yield return new WaitForSeconds(3.0f);

        MinusHp(isBool);
    }


    IEnumerator rayCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.03f);             // 코루틴 최적화

        LayerMask mask = LayerMask.GetMask("WALL");                  // 
        Vector3 down = transform.TransformDirection(Vector3.down);    // 플레이어 밑으로(로컬 기준) 레이 방향을 설정
        RaycastHit hitInfo;                                           // hit된 정보를 받아오는 변수
        GameObject PrevObj = null;

        while (isSpring.Equals(false) || endBool.Equals(false))
        {
            if (Physics.Raycast(this.transform.position + Vector3.up * 0.5f, down, out hitInfo, 1.0f, mask)) // 레이를 아래로 쏴서 발판인지 체크
            {
                if (!hitInfo.transform.gameObject.Equals(PrevObj))        // 이름으로 밟은 큐브가 바뀌었는지 판단...(최적화인데 스트링하는게 맘에 걸림...)
                {
                    nearNote = hitInfo.transform;
                    if (nearNote.CompareTag(tag09))      // 일반 큐브
                    {
                        StepCube(hitInfo.transform);       // 발판 체크S
                        isFirst = true;   // 스타트 발판에서 빠져나왔다고 알림

                        if (startCube.Equals(false))
                        {
                            startCube = true;

                            Right_Image.sprite = sprite_Array[0];   // 들어올리기 이미지
                        }
                    }
                    else if (nearNote.CompareTag(tag02))   // 스타트 큐브
                    {
                        if (isFirst.Equals(false))  // ???
                        {
                            isFirst = true;
                            startCube = false;
                        }
                        

                        if (isItem.Equals(false))     // 아이템을 들고 있지 않다면..
                        {
                            nearObject = null;        // 가까운 오브젝트 없앰
                        }


                        if (nearNote.childCount.Equals(2))   // 방해물 혹은 아이템이 있는 곳이다..
                        {
                            if (nearNote.GetChild(1).gameObject.layer.Equals(8))      // 아이템들...
                            {
                                if (isItem.Equals(false))       // 아이템을 안 들고 있었다면,,
                                {
                                    nearObject = nearNote.GetChild(1).gameObject;      // 가까운 오브젝트에 아이템을 일단 넣는다!

                                }
                            }
                        }

                    }
                    else if (nearNote.CompareTag(tag10))   // 앤드 큐브
                    {
                        if (isItem.Equals(false))
                        {
                            nearObject = null;
                        }

                        isFirst = true;   // 스타트 발판에서 빠져나왔다고 알림
                        startCube = true;
                        if (isDonut.Equals(true))       // 도넛을 알고 있고,
                        {
                            bool checkBool = nearNote.GetComponent<Mini10_End>().CheckEnd();    // 앤드 체크를 해서
                             
                            if (checkBool.Equals(true))  // true라면, 다음 스테이지로!!
                            {
                                Destroy(upTrans.GetChild(0).gameObject);   // 들고 있던 도넛 없앰

                                isItem = false;
                                isDonut = false;

                                isFirst = false;
                                startCube = false;

                                nearObject = null;
                                nearNote = null;

                                isCameraMove = true;

                                AudioMng.ins.PlayEffect("SpeedUp");    // 스테이지 이동
                                transform.position = mini10_Stage.Stage_Spawn();       // 다음 스테이지를 스폰한다.
                                
                                mini10_Camera.NextStageMove();         // 카메라 이동 함수 실행
                                anim.SetBool(attackId, false);       // 들고 있던 애니메이션 중단

                                Right_Image.sprite = sprite_Array[3];   // 지도 이미지
                            }
                        }
                    }

                    PrevObj = hitInfo.transform.gameObject;     // 발판 교체
                }
            }
            else            // 큐브가 아닌 곳(실패)에 닿을 경우
            {
                if (isCameraMove.Equals(false))
                {
                    nearObject = null;
                    nearNote = null;

                    if (endBool.Equals(false))
                    {
                        endBool = true;

                        MinusHp(false);
                    }
                    
                }
            }

            yield return delay;
        }
    }

    

	/////////////////////////    트리거 구역


	void OnTriggerStay(Collider other)
	{
        if (other.gameObject.layer.Equals(7))       // 블랙홀에 닿는다면
        {
            blackHoleTrans = other.transform;    // 블랙홀 영역에 닿았다고 알림
            speed = 2.8f;                        // 속도를 줄임
        }
	}


	void OnTriggerExit(Collider other)
	{
        if (other.gameObject.layer.Equals(7))       // 블랙홀에서 빠져나온다면..
        {
            blackHoleTrans = null;               // 블랙홀 영역에서 빠져나왔다고 알림
            speed = 3.5f;                        // 속도를 원래대로 바꾼다
        }
    }
}
