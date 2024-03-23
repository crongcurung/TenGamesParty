using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Mini08_Player : MonoBehaviour     // 플레이어에 부착됨
{
    [SerializeField] Joystic joystic;

    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // 점수를 서버에 등록시킬 수 있는 버튼

	[SerializeField] GameObject hammer;
    [SerializeField] GameObject gang;

    [SerializeField] Image timeImage;
    [SerializeField] TextMeshProUGUI[] Text_Array;     // 0 : 돌, 1 : 흙, 2 : 묘지, 3 : 부서진 묘지, 4 : 유령 개수

    int timeInt = 0;

    Animator anim;                       // 애니메이터는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    [SerializeField] Image Right_Image;
    [SerializeField] Button Right_Button;
    [SerializeField] Sprite[] sprite_Array;       // 0 : 아무것도 안 닿았을떄, 1 : 수리, 2 : 돌, 3 : 흙, 4 : 포탈

    [SerializeField] GameObject Item_Book;              // 중앙에서 빙글도는 아이템 오브젝트
    [SerializeField] Image Item_Image;
    [SerializeField] Sprite[] spriteItem_Array;   // 0 : 퇴마 이미지, 1 : 삭제 이미지, 2 : 재벌 이미지, 3 : 수리 이미지, 4 : 채집 이미지, 5 : 포탈 이미지, 6 : ???

    [SerializeField] Transform[] potal_Array;           // 포탈 4개를 받는다.
    [SerializeField] GameObject startCube;              // 퇴마 상태할때 나타나는 스타 오브젝트..

    [SerializeField] GameObject[] cemetry_Array;     // 묘지 리스트
    List<int> cemetry_ListInt = new List<int>();        // 묘지 비활성화 인트 리스트

    [SerializeField] float speed;        // 이동 속도 변수는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    Vector3 moveDir;                     // 이동 변수는 플레이어가 지닐 가능성 높음

    bool isPotal = false;   // 플레이어가 포탈에 닿았냐고 묻는 변수
    int potalInt = 0;       // 플레이어가 몇번 포탈로 갈 수 있는지 묻는 변수

    int potal_OneTwoInt = 0;         // 1번 셋트 이동 횟수 변수
    int potal_ThreeFourInt = 0;      // 2번 셋트 이동 횟수 변수

    int stoneInt = 0;       // 돌의 개수
    int soilInt = 0;        // 흙의 개수

    bool isExorcism = false;         // 현재 퇴마 상태인지 묻는 변수

    public Action followAction;      // 유령 함수를 받는 액션 변수(한번에 뿌릴려고...)

    WaitForSeconds delay_03;         // 후반전 딜레이

    bool moveLock = false;      // 플레이어 이동 못하게 하는 변수..

    int countInt = 0;           // 활성화된 묘지 개수를 받는 변수
    int BrokenInt = 0;          // 부서진 묘지 개수를 받는 변수
    int GhostInt = 0;           // 활성화된 유령 개수를 받는 변수 

    int potal_LimitInt = 0;          // 포탈 최대 이동 횟수(전반전, 후반전)
    int star_LimitInt = 0;           // 퇴마 스타의 유지시간(전반전, 후반전)
    int up_Int = 0;                  // 재벌 스킬 발동시, 자원 상승 숫자(전반전, 후반전)
    int repair_Skill = 0;            // 수리 시간 스킬 발동시, 수리 시간 숫자(전반전, 후반전)
    int extra_Skill = 0;             // 추가 자원 스킬 발동시, 추가 자원 숫자(전반전, 후반전)

    int repairTime = 0;   // 수리 시간
    int extraInt = 0;   // 채집 숫자

    int cemetryInt = 0;   // 묘지 활성화 개수

    bool isCemetry = false;     // 묘지에 닿았나?
    bool isStone = false;       // 돌 벽에 닿았나?
    bool isSoil = false;        // 흙 벽에 닿았나?

    Transform current_This;      // 현재 플레이어랑 붙어 있는 상호작용 오브젝트

    WaitForSeconds delay_01;
    WaitForSeconds delay_02;

    int runId;
    int cemetryId;
    int stoneId;
    int soilId;

    string invoke_Text01;
    string invoke_Text02;
    string invoke_Text03;
    string invoke_Text04;
    string invoke_Text05;
    string invoke_Text06;

    string hor_Text;
    string ver_Text;

    string tag01;
    string tag02;
    string tag03;

    

    void Start()
    {
        anim = GetComponent<Animator>();

        runId = Animator.StringToHash("isRun");
        cemetryId = Animator.StringToHash("isCemetry");
        stoneId = Animator.StringToHash("isStone");
        soilId = Animator.StringToHash("isSoil");

        invoke_Text01 = "Invoke_Fixed";
        invoke_Text02 = "Invoke_Stone";
        invoke_Text03 = "Invoke_Soil";
        invoke_Text04 = "Invoke_Exorcism";
        invoke_Text05 = "Invoke_Repair";
        invoke_Text06 = "Invoke_Gather";

        hor_Text = "Horizontal";
        ver_Text = "Vertical";

        tag01 = "Honey";
        tag02 = "Spring";
        tag03 = "Bear";


        stoneInt = 20;             // 돌 숫자
        soilInt = 20;              // 흙 숫자
        Text_Array[0].text = stoneInt.ToString();  // 돌, 흙 숫자 텍스트로 표시
        Text_Array[1].text = soilInt.ToString();

        repairTime = 3;     // 수리 시간

        for (int i = 0; i < 20; i++)     // 묘지 비활성화 리스트 셋팅
		{
            cemetry_ListInt.Add(i);      
        }

        delay_01 = new WaitForSeconds(60.0f);
        delay_02 = new WaitForSeconds(30.0f);
        delay_03 = new WaitForSeconds(240.0f);

        Start_Spawn();

        StartCoroutine(Item_Coroutine());        // 아이템 활성화 코루틴
        StartCoroutine(Cemetry_Spawn());         // 묘지 활성화 코루틴
        StartCoroutine(After_Coroutine());       // 후반전으로 가는 코루틴 실행

        timeImage.fillAmount = 0;

		repair_Skill = 3;      // 처음 수리시간

		AudioMng.ins.Play_BG("Mini08_B");
    }

    

    void FixedUpdate()
    {
        if (!moveDir.x.Equals(0) || !moveDir.z.Equals(0))               // wasd 아무거나라도 움직인다면..
        {
            anim.SetBool(runId, true);         // 움직이는 애니메이션 실행
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z)), 0.5f);
            // 플레이어 회전

            transform.position += new Vector3(moveDir.x, 0, moveDir.z).normalized * Time.fixedDeltaTime * speed;      // 플레이어 이동
        }
        else
        {
            anim.SetBool(runId, false);       // 멈추는 애니메이션 실행
        }
    }

    bool scoreBool = false;

    void Update()
    {
        timeImage.fillAmount += Time.deltaTime / 60.0f;

        if (timeImage.fillAmount >= 0.5f && scoreBool.Equals(false))
        {
            scoreBool = true;
            timeInt++;
        }


        if (timeImage.fillAmount.Equals(1))
        {
            timeImage.fillAmount = 0;
            timeInt++;
            scoreBool = false;
        }

        if (moveLock.Equals(true))     // 돌, 흙, 묘지 고치는 등 못 움직이는 상태면...
        {
            moveDir.x = 0;     // 이걸 해야 한다.
            moveDir.z = 0;
            return;            // 아래 실행 못하게 한다.
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) == true)       // 오른쪽 버튼을 누른다면...
        {
            Press_RightButton();
        }

        Move();
    }

    void Start_Spawn()
    {
		for (int i = 0; i < 3; i++)
		{
            int randInt = UnityEngine.Random.Range(0, cemetry_ListInt.Count);

            cemetry_Array[cemetry_ListInt[randInt]].SetActive(true);
            cemetry_ListInt.RemoveAt(randInt);                           // 중복 방지
            cemetryInt++;
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

    


    public void Press_RightButton()        // 오른쪽 버튼을 누른다면..
    {
        if (isSoil.Equals(true))           // 현재 흙에 닿고 있다면...
        {
            Soil_Gather();
        }
        else if (isStone.Equals(true))     // 현재 돌에 닿고 있다면..
        {
            Stone_Gather();
        }
        else if (isCemetry.Equals(true))   // 현재 묘지에 닿고 있다면...
        {
            Cemetry_Fixed();
        }
        else if (isPotal.Equals(true))     // 현재 포탈에 닿고 있다면...
        {
            transform.position = new Vector3(potal_Array[potalInt].position.x, transform.position.y, potal_Array[potalInt].position.z);    // 지정된 포탈로 감

            AudioMng.ins.PlayEffect("SpeedUp");    // 포탈 먹음

            if (potalInt.Equals(0) || potalInt.Equals(1))           // 플레이어가 포탈01이랑 02에 있다면..
            {
                potal_OneTwoInt++;                                  // 포탈 셋트 1번의 이동횟수를 올린다.

                if (potal_OneTwoInt.Equals(potal_LimitInt))         // 포탈 셋트 1번의 이동 제한에 걸린다면...
                {
                    potal_Array[0].gameObject.SetActive(false);     // 포탈 01, 02을 비활성화한다.
                    potal_Array[1].gameObject.SetActive(false);

                    isPotal = false;                                // 포탈이 사라지면, 안 닿았다고 해줘야 한다.
                }
            }
            else                                                    // 플레이어가 포탈03이랑 04에 있다면...
            {
                potal_ThreeFourInt++;                               // 포탈 셋트 2번의 이동횟수를 올린다.

                if (potal_ThreeFourInt.Equals(potal_LimitInt))      // 포탈 셋트 2번의 이동 제한에 걸린다면...
                {
                    potal_Array[2].gameObject.SetActive(false);     // 포탈 03, 04을 비활성화한다.
                    potal_Array[3].gameObject.SetActive(false);

                    isPotal = false;                                // 포탈이 사라지면, 안 닿았다고 해줘야 한다.
                }
            }
        }
    }

    

    public void CemetryText_Fuction(int upDownInt)          // 현재 활성화된 묘지의 개수를 알려주는 함수
    {
        countInt = countInt + upDownInt;
        Text_Array[2].text = countInt.ToString();
    }

    public void BrokenText_Fuction(int upDownInt)           // 현재 나온 부서진 묘지의 개수를 알려주는 함수
    {
        BrokenInt = BrokenInt + upDownInt;
        Text_Array[3].text = BrokenInt.ToString();  
    }

    public void GhostText_Fuction(int upDownInt)            // 현재 나온 유령의 개수를 알려주는 함수
    {
        GhostInt = GhostInt + upDownInt;
        Text_Array[4].text = GhostInt.ToString();


        if (GhostInt.Equals(60))
        {

            End_Game();
        }
    }


    void End_Game()                   // 게임이 끝났을 때 함수
    {
        Result_Panel.SetActive(true);
        Game_Panel.SetActive(true);

        AudioMng.ins.LoopEffect(false);    // 무한루프 종료


        Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + timeInt.ToString();

        if (Main.ins.nowPlayer.maxScore_List[7] >= timeInt)    // 최고점을 못 넘은다면...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[7].ToString();
        }
        else        // 최고점을 넘은 경우 (신기록)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + timeInt.ToString();

            Main.ins.nowPlayer.maxScore_List[7] = timeInt;
            Main.ins.SaveData();

			GPGS_Button.interactable = true;        // 신기록을 세우면 점수 서버 등록 버튼을 활성화한다. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // 배경음악을 끈다.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;
    }


	public void Press_GPGS_08()   // 신기록 시, 서버에 등록
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no8, timeInt);   // 점수를 서버에 등록
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no8);          // 리더보드를 띄운다.
	}


	void Cemetry_Fixed()      // 묘지를 고치는 함수
    {
        int cemetryInt = current_This.GetComponent<Mini08_Cemetry>().Check_Cemetry();     // 현재 고치는 묘지에서 묘지의 상태를 가져온다.
        


        switch (cemetryInt)      // 묘지의 상태에 따라...
        {
            case 1:              // 묘지의 상태가 부식 1단계라면..
                if (stoneInt > 0 && soilInt > 0)    // 돌과 흙이 1개씩 이상이라면..
                {
                    Right_Button.interactable = false;

                    stoneInt--;                          // 하나씩 줄임
                    soilInt--;

                    current_This.GetComponent<Mini08_Cemetry>().Player_Fixed();
                }
                else
                {
                    return;
                }
                break;
            case 2:              // 묘지의 상태가 부식 2단계라면..
                if (stoneInt > 1 && soilInt > 1)
                {
                    Right_Button.interactable = false;

                    stoneInt -= 2;                       // 두개씩 줄임
                    soilInt -= 2;

                    current_This.GetComponent<Mini08_Cemetry>().Player_Fixed();
                }
                else
                {
                    return;
                }
                break;
            case 3:              // 묘지의 상태가 부식 3단계라면..
                if (stoneInt > 2 && soilInt > 2)          // 세개씩 줄임
                {
                    Right_Button.interactable = false;

                    stoneInt -= 3;
                    soilInt -= 3;

                    current_This.GetComponent<Mini08_Cemetry>().Player_Fixed();
                }
                else
                {
                    return;
                }
                break;
            case 4:              // 묘지의 상태가 부식 4단계라면..(완전 부서짐)
                if (stoneInt > 4 && soilInt > 4)             // 다섯개씩 줄임
                {
                    Right_Button.interactable = false;

                    stoneInt -= 5;
                    soilInt -= 5;

                    current_This.GetComponent<Mini08_Cemetry>().Player_Fixed();
                }
                else
                {
                    return;
                }
                break;
            default:              // 묘지의 상태가 부식 0단계라면..
                return;
        }

        Text_Array[0].text = stoneInt.ToString();     // 묘지를 고쳤다면 깍인 재료를 텍스트로 나타낸다.
        Text_Array[1].text = soilInt.ToString();

        moveLock = true;                                           // 묘지를 고친다면 못 움직이게 해야한다.
        anim.SetBool(runId, false);                              // 달리는 애니메이션을 끄고
        anim.SetBool(cemetryId, true);                           // 묘지를 고치는 애니메이션을 킨다.
        hammer.SetActive(true);
        transform.LookAt(current_This);                            // 해당 묘지를 바라보도록 한다.


		//if (!repair_Skill.Equals(3))                                 // 수리 시간이 3초가 아니라면(수리 시간 스킬이 발동되었다는 뜻)
		//      {
		//          repairTime = repair_Skill;                             // 수리 시간 스킬 시간을 받아온다.(전반전, 후반전이 있어서 이렇게 한다...)
		//      }

		repairTime = repair_Skill;
		AudioMng.ins.PlayEffect("Fix");    // 묘지 고치는 소리
        Invoke(invoke_Text01, repairTime);                        // 수리 하는 것을 끝내는 것을 인보크로 한다.

    }

    void Invoke_Fixed()               // 수리를 끝내는 인보크..
    {
        Right_Button.interactable = true;

        AudioMng.ins.LoopEffect(false);    // 수리 소리 무한루프 종료
        AudioMng.ins.StopEffect();

        moveLock = false;                   // 다시 움직이게 한다.
        anim.SetBool(cemetryId, false);   // 묘지 고치는 애니메이션을 끈다.
        hammer.SetActive(false);
    }



    void Stone_Gather()       // 돌을 캐는 함수
    {
        moveLock = true;      // 플레이어를 못 움직이게 한다.
        Right_Button.interactable = false;

        anim.SetBool(runId, false);     // 달리는 애니메이션을 끈다.
        anim.SetBool(stoneId, true);    // 돌을 캐는 애니메이션을 킨다.
        gang.SetActive(true);
        transform.LookAt(current_This);   // 돌 벽의 가운데를 보도록 한다.

        AudioMng.ins.LoopEffect(true);
        AudioMng.ins.PlayEffect("Pickaxe");    //  돌 캐는 소리

        followAction?.Invoke();           // 활성화된 유령들을 자기쪽으로 오도록 한다.
        Invoke(invoke_Text02, 3.0f);     // 돌 캐는 걸 끝내는 인보크 실행
    }

    void Invoke_Stone()       // 돌을 캘때 인보크
    {
        moveLock = false;       // 다시 움직이게 한다.
        Right_Button.interactable = true;

        anim.SetBool(stoneId, false);   // 돌 캐는 애니메이션을 끈다.
        gang.SetActive(false);

        AudioMng.ins.LoopEffect(false);

        Stone_Up();          // 돌 스코어를 올리는 함수를 실행
    }

    void Stone_Up()                 // 돌의 스코어를 올리는 함수
    {
        if (!extraInt.Equals(0))      // 추가 자원 채취가 0이 아니라면...(자원 채취 스킬이 발동되었다는 뜻)
        {
            extraInt = extra_Skill;   // 추가 자원 채취 숫자를 받아온다.(전반전, 후반전이 있어서 이렇게 한다...)
        }

        stoneInt += 4 + extraInt;     // 돌 숫자를 올린다.

        if (stoneInt > 99)            // 올린 돌 숫자가 99를 넘어간다면..
        {
            stoneInt = 99;            // 99로 맞춘다.
        }

        Text_Array[0].text = stoneInt.ToString();      // 바뀐 돌 텍스트를 올린다.
    }





    void Soil_Gather()        // 흙을 캐는 함수
    {
        moveLock = true;      // 플레이어를 못 움직이게 한다.
        Right_Button.interactable = false;

        anim.SetBool(runId, false);     // 달리는 애니메이션을 끈다.
        anim.SetBool(soilId, true);     // 흙을 캐는 애니메이션을 킨다.
        transform.LookAt(current_This);   // 흙 벽의 가운데를 보도록 한다.

        AudioMng.ins.LoopEffect(true);
        AudioMng.ins.PlayEffect("Shovel");    //  흙 캐는 소리

        followAction?.Invoke();           // 활성화된 유령들을 자기쪽으로 오도록 한다.
        Invoke(invoke_Text03, 3.0f);      // 흙 캐는 걸 끝내는 인보크 실행
    }

    void Invoke_Soil()       // 흙을 캘때 인보크
    {
        moveLock = false;       // 다시 움직이게 한다.
        Right_Button.interactable = true;

        anim.SetBool(soilId, false);   // 흙 캐는 애니메이션을 끈다.

        AudioMng.ins.LoopEffect(false);

        Soil_Up();          // 흙 스코어를 올리는 함수를 실행
    }

    void Soil_Up()                 // 흙의 스코어를 올리는 함수
    {
        if (!extraInt.Equals(0))      // 추가 자원 채취가 0이 아니라면...(자원 채취 스킬이 발동되었다는 뜻)
        {
            extraInt = extra_Skill;   // 추가 자원 채취 숫자를 받아온다.(전반전, 후반전이 있어서 이렇게 한다...)
        }

        soilInt += 4 + extraInt;     // 흙 숫자를 올린다.

        if (soilInt > 99)            // 올린 흙 숫자가 99를 넘어간다면..
        {
            soilInt = 99;            // 99로 맞춘다.
        }

        Text_Array[1].text = soilInt.ToString();      // 바뀐 흙 텍스트를 올린다.
    }






    void Potal_Check(GameObject potal)   // 현재 플레이어가 어느 포탈에 있고, 어디로 갈 수 있는지 체크해주는 함수
    {
        if (potal.CompareTag(tag01))          // 포탈 01에 닿으면..
        {
            potalInt = 1;      // 2번 포탈로 갈 수 있다고 알림
        }
        else if (potal.CompareTag(tag02))      // 포탈 02에 닿으면..
        {
            potalInt = 0;      // 1번 포탈로 갈 수 있다고 알림
        }
        else if (potal.CompareTag(tag03))      // 포탈 03에 닿으면..
        {
            potalInt = 3;      // 4번 포탈로 갈 수 있다고 알림
        }
        else                                 // 포탈 04에 닿으면..
        {
            potalInt = 2;      // 3번 포탈로 갈 수 있다고 알림
        }
    }



    // 1이면      퇴마 : 일정시간(10초) 동안 귀신에게 피격될 경우, 귀신은 소멸함.
    // 2이면      삭제 : 필드 위 임의의 묘지 5개를 없앤다.
    // 3이면      재벌 : 흙 및 돌을 즉시 30개씩 획득한다.
    // 4이면      수리 : 일정시간(30초)동안 묘지 복구 시간을 1초로 단축한다.
    // 5이면      채집 : 일정시간(30초) 동안 흙 및 돌 획득양을 2개씩 더 획득한다.
    // 6이면      포탈 : 10회 사용할 수 있는 포탈을 생성한다.

    int rantInt;

    void Item_Check()      // 플레이어가 아이템을 먹었을 때 스킬을 랜덤으로 발동시키는 함수
    {
        rantInt = UnityEngine.Random.Range(0, 6);      // 6가지 스킬
        //rantInt = 3;

        switch (rantInt)          // 랜덤으로 뽑은 숫자에 따라..
        {
            case 0:
                AudioMng.ins.PlayEffect("Score_Up");    // 퇴마 먹음
                Exorcism_01();    // 퇴마

                Item_Image.sprite = spriteItem_Array[0];       // 퇴마 이미지
                break;
            case 1:
                AudioMng.ins.PlayEffect("Bomb");    // 삭제 먹음
                Delete_02();      // 삭제

                Item_Image.sprite = spriteItem_Array[1];       // 삭제 이미지
                break;
            case 2:
                AudioMng.ins.PlayEffect("Cloud");    // 재벌 먹음
                Rich_03();        // 재벌

                Item_Image.sprite = spriteItem_Array[2];       // 재벌 이미지
                break;
            case 3:
                AudioMng.ins.PlayEffect("Fix");    // 수리 먹음
                Repair_04();      // 수리

                Item_Image.sprite = spriteItem_Array[3];       // 수리 이미지
                break;
            case 4:
                AudioMng.ins.PlayEffect("Click02");    // 채집 먹음
                Gather_05();      // 채집

                Item_Image.sprite = spriteItem_Array[4];       // 채집 이미지
                break;
            default:
                AudioMng.ins.PlayEffect("SpeedUp");    // 포탈 먹음
                Potal_06();       // 포탈

                Item_Image.sprite = spriteItem_Array[5];       // 포탈 이미지
                break;
        }
    }


    void Exorcism_01()    // 퇴마
    {
        isExorcism = true;                     // 퇴마(스타)가 발동되고 있다고 알린다.
        startCube.SetActive(true);             // 플레이어 스타 오브젝트를 킨다.

        CancelInvoke(invoke_Text04);       // 퇴마 인보크가 실행되고 있다고 중단시킨다.
        Invoke(invoke_Text04, star_LimitInt);    // 퇴마 인보크를 실행한다.
    }

    void Invoke_Exorcism()    // 퇴마 인보크
    {
        startCube.SetActive(false);       // 시간이 되면 스타 오브젝트를 끈다.

        if (rantInt.Equals(0))
        {
            Item_Image.sprite = spriteItem_Array[6];
        }

        isExorcism = false;               // 퇴마가 끝났다고 알린다.
    }


    void Delete_02()      // 삭제
    {
        int randInt = 20 - cemetry_ListInt.Count;    // 20에서 활성화된 묘지 숫자 리스트의 개수를 뺀 숫자를 넘긴다.
        int tempInt = Mathf.Clamp(randInt, 0, 5);    // 위에 숫자를 최소 0부터 최대 5까지 안에다가 가둔다.
        randInt--;                                   // 리스트는 0부터라 숫자 1을 줄인다.

        for (int i = 0; i < tempInt; i++)    // 가둔 숫자 만큼 돌린다.
		{
            bool isEnd = false;              // 무한 반복 끝내는 변수

			while (isEnd.Equals(false))      // true가 될때까지 계속 돌린다.
			{
                if(cemetry_Array[randInt].activeSelf.Equals(true)) // 랜덤으로 뽑은 묘지가 활성화라면...
                {
                    isEnd = true;     // 루프를 끝낸다..
                }
                else
                {
                    randInt = UnityEngine.Random.Range(0, 20);     // 다시 랜덤을 뽑는다.
                }
			}
            cemetry_Array[randInt].SetActive(false);      // 활성화된 묘지를 비활성화 한다.
            randInt = UnityEngine.Random.Range(0, 20);   // 랜덤을 뽑는다.
        }
	}


    void Rich_03()        // 재벌
    {
        stoneInt += up_Int;                         // 20개씩 늘림
        soilInt += up_Int;

        if (stoneInt > 99)      // 99가 넘은다면..
        {
            stoneInt = 99;      // 99를 고정시킨다.
        }

        if (soilInt > 99)
        {
            soilInt = 99;
        }

        Text_Array[0].text = stoneInt.ToString();   // 바뀐 재료를 텍스트를 올린다.
        Text_Array[1].text = soilInt.ToString();
    }

    

    void Repair_04()      // 수리 시간
    {
        CancelInvoke(invoke_Text05);     // 수리 시간 인보크를 중단시킨다.

        if (potal_LimitInt.Equals(7))
        {
			repair_Skill = 2;       // 전, 후반전 수리시간을 가지고 온다.
        }
        else
        {
			repair_Skill = 1;

		}

        Invoke(invoke_Text05, 30.0f);  // 30초 동안 수리 시간을 줄인다.
    }

    void Invoke_Repair()    // 수리 시간 인보크
    {
        repair_Skill = 3;

        if (rantInt.Equals(3))
        {
            Item_Image.sprite = spriteItem_Array[6];       // 수리 이미지
        }
    }

    

    void Gather_05()      // 채집
    {
        CancelInvoke(invoke_Text06);

        extraInt = extra_Skill;      // 전, 후반전 추가 자원 숫자를 가지고 온다.

        Invoke(invoke_Text06, 30);  // 30초 동안 추가 채집을 한다.
    }

    void Invoke_Gather()   // 채집 인보크
    {
        extraInt = 0;      // 30초 후에, 추가 채집을 없앤다.

        if (rantInt.Equals(4))
        {
            Item_Image.sprite = spriteItem_Array[6];       // 수리 이미지
        }
    }


    void Potal_06()       // 포탈
    {
        int randInt = UnityEngine.Random.Range(0, 2);          // 1, 2번 셋트 중 어느 셋트로 할지 정하는 랜덤 변수

        if (randInt.Equals(0))                     // 1번 셋트라면...
        {
            potal_OneTwoInt = 0;                   // (1번 셋트)포탈 타는 카운트를 0으로 맞춤(초기화)

            potal_Array[0].gameObject.SetActive(true);     // 포탈 1번 셋트를 킨다.
            potal_Array[1].gameObject.SetActive(true);
        }
        else                                        // 2번 셋트라면...
        {
            potal_ThreeFourInt = 0;                  // (2번 셋트) 포탈 타는 카운트를 0으로 맞춤(초기화)

            potal_Array[2].gameObject.SetActive(true);     // 포탈 2번 셋트를 킨다.
            potal_Array[3].gameObject.SetActive(true);
        }
    }


    ////////////////////// 트리거 구역.........

    

    void OnTriggerEnter(Collider other)        // 트리거에 닿았다면...
    {
        switch (other.gameObject.layer)      // 닿을 레이어가..
        {
            case 4:            // 가운데 책에 닿았을 경우...  Water
                other.gameObject.SetActive(false);      // 아이템 책을 비활성화한다.
                Item_Check();
                break;
            case 3:            // 흙 벽에 닿았을 경우...   wall
                isSoil = true;         // 흙 벽에 닿았다고 알린다.
                current_This = other.transform;   // 흙벽을 넘긴다.

                Right_Image.sprite = sprite_Array[3];     // 흙에 닿았을 경우...
                break;
            case 1:            // 돌 벽에 닿았을 경우...    Transparentfx
                isStone = true;        // 돌 벽에 닿았다고 알린다.
                current_This = other.transform;   // 돌 벽을 넘긴다.

                Right_Image.sprite = sprite_Array[2];     // 돌에 닿았을 경우...
                break;
            case 7:         // 몬스터에 닿으면...    monster
                Touch_Monster(other);
                break;
            case 6:           // 아무 포탈이나 닿으면..    player
                isPotal = true;        // 포탈에 닿았다고 알린다.
                Potal_Check(other.gameObject);

                Right_Image.sprite = sprite_Array[4];     // 포탈에 닿았을 경우...
                break;
            case 8:          // 묘지에 닿았을 경우..      object
                isCemetry = true;      // 묘지에 닿았다고 알린다.
                current_This = other.transform;   // 묘지를 넘긴다.

                Right_Image.sprite = sprite_Array[1];     // 묘지에 닿았을 경우...
                break;
        }
    }


	void OnTriggerExit(Collider other)        // 트리거에서 빠져 나왔다면...
    {
        switch (other.gameObject.layer)    // 빠져 나온 레이어..
        {
            case 3:        // 흙 벽에서 빠져나왔다면...      wall
                isSoil = false;     // 흙 벽에서 빠져나왔다고 알린다.
                current_This = null;

                Right_Image.sprite = sprite_Array[0];     // 흙에서 빠져 나왔다면...
                break;
            case 1:        // 돌 벽에서 빠져나왔다면...    Transparentfx
                isStone = false;    // 돌 벽에서 빠져나왔다고 알린다.
                current_This = null;

                Right_Image.sprite = sprite_Array[0];     // 돌에서 빠져 나왔다면...
                break;
            case 6:       // 포탈에서 빠져나왔다면...   player
                isPotal = false;    // 포탈에서 빠져나왔다고 알린다.

                Right_Image.sprite = sprite_Array[0];     // 포탈에서 빠져 나왔다면...
                break;
            case 8:       // 묘지에서 빠져나왔다면...    object
                isCemetry = false;  // 묘지에서 빠져나왔다고 알린다.
                current_This = null;

                Right_Image.sprite = sprite_Array[0];     // 포탈에서 빠져 나왔다면...
                break;
        }
    }

    void Touch_Monster(Collider other)    // 몬스터에 닿았다면..
    {
        if (isExorcism.Equals(true))      // 플레이어가 퇴마 상태라면..
        {
            AudioMng.ins.LoopEffect(false);    // 흙, 돌 소리 무한루프 종료
            AudioMng.ins.PlayEffect("Score_Up");    // 퇴마 먹음

            other.gameObject.SetActive(false);   // 닿은 몬스터를 비활성화 시킨다.
        }
        else                              // 퇴마 상태가 아니라면...
        {
            End_Game();
        }
    }


    

    public void Cemetry_Minus(int listInt)      // 묘지 스크립트에서 자신이 비활성화 되었다고 한다면..          ///////////////////////////////////////////////////////////////////////////////////
    {
        cemetryInt--;                           // 묘지 숫자를 줄인다.

        cemetry_ListInt.Add(listInt);           // 묘지 숫자 리스트에 해당 묘지의 번호를 담는다.
    }

    ////////////////////////   코루틴 구역...

    IEnumerator Cemetry_Spawn()           // 묘지 활성화 코루틴
    {
        while (true)
        {
            yield return delay_02;   // 30초 동안 계속

            if (!cemetryInt.Equals(20))       // 묘지 숫자 리스트가 20이 아니라면
            {
                int randInt = UnityEngine.Random.Range(0, cemetry_ListInt.Count);
                cemetry_Array[cemetry_ListInt[randInt]].SetActive(true);      // 랜덤으로 뽑은거 활성화
                cemetry_ListInt.RemoveAt(randInt);                           // 중복 방지
                cemetryInt++;
            }
        }
    }


    IEnumerator Item_Coroutine()           // 아이템 활성화 코루틴
    {
        while (true)
        {
            yield return delay_01;         // 1분 동안 계속
            AudioMng.ins.LoopEffect(false);
            AudioMng.ins.PlayEffect("Enter");    // 아이템 먹는 소리
            Item_Book.SetActive(true);     // 아이템 책 활성화
        }
    }


    IEnumerator After_Coroutine()        // 후반전으로 가는 코루틴
    {
        potal_LimitInt = 7;     // 전반전은 포탈 제한 7개로
        star_LimitInt = 15;     // 전반전은 퇴마 지속시간 15초로
        up_Int = 20;            // 전반전은 재벌 상승 20으로
        extra_Skill = 2;        // 전반전은 추가 자원 2개로

        yield return delay_03;       // 240초 후...(4분 후...)

        potal_LimitInt = 12;    // 후반전은 포탈 제한 12개로
        star_LimitInt = 25;     // 후반전은 퇴마 지속시간 25초로
        up_Int = 30;            // 후반전은 재벌 상승 30으로
        extra_Skill = 3;        // 후반전은 추가 자원 3개로
    }
}
