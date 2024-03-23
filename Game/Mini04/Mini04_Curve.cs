using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Mini04_Curve : MonoBehaviour
{
    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // 점수를 서버에 등록시킬 수 있는 버튼

	[SerializeField] AudioSource audio_Bowl;

    [SerializeField] TextMeshProUGUI scoreText;   // 스코어 텍스트
    [SerializeField] GameObject[] Hp_List;

    [SerializeField] Image donut_Image;
    [SerializeField] Sprite[] sprite_Array;      // 0 : 딸기, 1 : 초코

    int scoreInt = 0;              // 스코어 점수를 올리는 변수
    int failInt = 0;               // 패배 점수를 올리는 변수

    [SerializeField] Transform[] cups;       // 그릇 5개을 담은 변수

    [SerializeField] Transform[] Pos_3x3;    // 그릇 3개일떄 위치 조정
    [SerializeField] Transform[] Pos_4x4;    // 그릇 4개일떄 위치 조정
    [SerializeField] Transform[] Pos_5x5;    // 그릇 5개일떄 위치 조정

    [SerializeField] GameObject[] donut_Array;     // 그릇 안에 들어갈 도넛(두개)

    float speed;               // 그릇 돌리는 속도

    [Range(0, 1)]             
    public float Test = 0;     // 그릇 돌리는 사이 값(보간?)

    Animator anim;           // 그릇 전체는 90도로 꺽는 애니메이션을 받기 위해...

    int randInt;             // 딸기도넛만 나올떄 도넛의 위치를 바꾸는 변수?

    bool isSecondTouch = false;                // 플레이어가 터치를 한번 했냐 묻는 변수(그릇 고를 때...)
    bool isCurveEnd = false;                   // 그릇들이 모두 섞여졌나?(플레이어가 그릇을 고를 준비가 되었는지..?)
    bool isCorrect = false;                    // 정답인지 묻는 변수..

    WaitForEndOfFrame delay_01;          // 다음 프레임 딜레이..
    WaitForSeconds delay_02;             // 2초 딜레이..

    [SerializeField] Animator anim_Player;               // 플레이어의 애니메이터를 가져온다.
    [SerializeField] Animator anim_Monster;              // 몬스터의 애니메이터를 가져온다.

    [SerializeField] GameObject[] donutThrow_Array;      // 던지는 도넛 배열(두개)

    [SerializeField] Transform Enemy_HeadPos;            // 몬스터 머리 위치(던지는 도넛 최종지점)

    Vector3 originPos_ThrowDonut;                        // 던지는 도넛 처음위치(던지고 난 후, 다시 이쪽으로 되돌아와야 한다)

    Mini04_Bowl mini04_Bowl;                             // 그릇 안에 붙어 있는 스크립트

    [SerializeField] GameObject enermy_Bat;              // 몬스터가 들고 있는 뿅망치
    bool isShield = false;                               // 지금 플레이어가 머리에 뭐가 씌워있는지?

    [SerializeField] GameObject[] head_Helmet;            // 0 : 그릇, 1 : 수박
    GameObject temp_Helmet;                               // 머리 위에 씌울 오브젝트 임시로 받는 변수

    [SerializeField] Transform HeadThrow_Pos;            // 튕겨져 나온 헬멧이 튕겨 나갈 최종 지점

    GameObject touch_Bowl;                               // 첫번쨰 터치할때 그릇을 받는 변수
    int bowlInt;                                         // 이번 스테이지는 몇 그릇이 나와야하는지 알아보는 변수

    [SerializeField] GameObject fly_Obj;                 // 날아다니는 오브젝트
    [SerializeField] Collider[] cloud_Col;               // 구름 오브젝트(5개)

    bool isCloud = false;        // 이번 스테이지에서 구름이 나왔냐?
    bool isFly = false;          // 이번 스테이지에서 날아다니는 오브젝트가 나왔냐?

    int firstInt = 0;            // 빨간 도넛의 위치를 받는 변수(스테이지4 이상)
    int secondInt = 0;           // 초코 도넛의 위치를 받는 변수(스테이지4 이상)
    bool isDonutStrow = true;    // true면 딸기, false면 초코
    bool isRotEnd = false;            // true면 90도로 가는 회전이 끝났다, false면 아직 안 끝났다..(정확히는 그릇이 움직일떄 true가 됨)

    string tag01;
    string tag02;
    string tag03;
    string tag04;

    int anim_Id01;
    int anim_Id02;
    int anim_Id03;
    int anim_Id04;
    int anim_Id05;

    Camera cameraMain;

    void Start()
    {
        tag01 = "Cushion";
        tag02 = "Note";
        tag03 = "Honey";
        tag04 = "Spring";

        audio_Bowl.volume = AudioMng.ins.GetEffectVolume();

        anim_Id01 = Animator.StringToHash("New State");
        anim_Id02 = Animator.StringToHash("isRot");
        anim_Id03 = Animator.StringToHash("isAttack");
        anim_Id04 = Animator.StringToHash("Enemy_Attack");
        anim_Id05 = Animator.StringToHash("Enemy_Damage");

        failInt = 3;

        cameraMain = Camera.main;

        anim = GetComponent<Animator>();         // 그릇들을 90도로 움직일 수 있는 애니메이션을 가져옴
        originPos_ThrowDonut = donutThrow_Array[0].transform.position;      // 던지는 도넛의 원래 위치를 가져옴(하나만 해도 된다)
        Stage_fuction(0);                                                   // 스테이지를 세팅한다.
        delay_01 = new WaitForEndOfFrame();        // 다음 프레임
        delay_02 = new WaitForSeconds(2.0f);       // 2초 후

        AudioMng.ins.Play_BG("Mini04_B");
    }

    

	void Update()
	{
        if (Input.GetMouseButtonDown(0))          // 플레이어가 누른다면?
        {
            RaycastHit hit;      
            Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (isRotEnd.Equals(true) && hit.collider != null)      // 90도 회전이 끝나고 누른 것이 콜라이더가 있다면..
            {
                if (hit.transform.CompareTag(tag01))          // 수박 헬멧에 닿으면...
                {
                    hit.transform.gameObject.SetActive(false);    // 닿으면 수박은 지운다.

                    AudioMng.ins.PlayEffect("Click02");    // 수박 그릇 클릭
                    Shield_Active(1);    // 수박 헬멧을 씌운다..
                }
            }

            if (isSecondTouch.Equals(false) && isCurveEnd.Equals(true) && hit.collider != null)  // 지금 처음 누른고, 모두 그릇이 섞였고, 콜라이더가 있다면..
            {
                if (hit.transform.CompareTag(tag02))       // 그릇을 눌렀다면..
                {
                    AudioMng.ins.PlayEffect("Click02");    // 섞인 그릇 클릭
                    mini04_Bowl = hit.transform.GetComponent<Mini04_Bowl>();      // 누른 그릇의 스크립트를 가져온다.

                    if (hit.transform.childCount.Equals(2))     // 자식이 두개라는 것은 그릇안에 무언가 있다는 뜻..
                    {
                        if (isDonutStrow.Equals(true))          // 정답은 딸기 도넛이라는 뜻
                        {
                            if (hit.transform.GetChild(1).CompareTag(tag03))     // 정답            honey 딸기, spring은 초코
                            {
                                isCorrect = true;                           // 정답이라고 알림
                            }
                            else      // 잘못된 도넛이 있었다는 뜻....
                            {
                                StartCoroutine(Attack_Monster());
                            }
                        }
                        else                                    // 정답은 초코 도넛이라는 뜻
                        {
                            if (hit.transform.GetChild(1).CompareTag(tag04))     // 정답            honey 딸기, spring은 초코
                            {
                                isCorrect = true;                           // 정답이라고 알림
                            }
                            else      // 잘못된 도넛이 있었다는 뜻....
                            {
                                StartCoroutine(Attack_Monster());      // 몬스터 뿅망치 공격 실행..
                            }
                        }
                        hit.transform.GetChild(1).parent = null;        // 누른 그릇의 도넛을 떨어트린다.(이미 그릇안에 도넛이 있다고 했음)
                    }
                    else                   // 빈 그릇을 선택했음...
                    {
                        StartCoroutine(Attack_Monster());      // 몬스터 뿅망치 공격 실행..
                    }
                    mini04_Bowl.Buddle_Bowl();         // 선택한 그릇을 부들거리게 한다.

                    isSecondTouch = true;              // 한번 눌렀다고 알려줌
                    touch_Bowl = hit.transform.gameObject;  // 선택한 그릇을 임시로 담는다.
                }
            }
            else if(isSecondTouch.Equals(true) && hit.collider != null)     // 두 번째 누른거고, 콜라이더가 있다면...
            {
                if (isCorrect.Equals(true))               // 플레이어가 정답을 맞췄다면...(도넛이 2개더라도, 정답이라면 정답 도넛을 골랐다는 뜻이다..)
                {
                    if (hit.transform.gameObject.layer.Equals(4))        // 다시 누른 것이 도넛이라면..
                    {
                        hit.transform.gameObject.SetActive(false);        // 누른 도넛을 비활성화 한다.
                        StartCoroutine(Attack_Player());      // 플레이어 던지기 공격 실행..
                    }
                }
                else                                      // 플레이어가 틀렸다면...
                {
                    if (mini04_Bowl.isBuddle_End.Equals(true))    // 첫번째 눌렀던 그릇이 부들 거리는 애니메이션이 끝났다면...(플레이어는 이제 누를 수 있음..)
                    {
                        if (hit.transform.CompareTag(tag02))     // 그릇을 눌렀다면.. (첫 번째 누른 도넛이랑 다를수도 있다)
                        {
                            if (hit.transform.gameObject.Equals(touch_Bowl))     // 다시 누른 것이 튕겨진 그릇이랑 같다면...
                            {
                                hit.transform.gameObject.SetActive(false);       // 튕겨진 그릇(누른 도넛)을 비활성화

                                AudioMng.ins.PlayEffect("Click02");    // 튕긴 그릇 클릭
                                Shield_Active(0);    // 그릇 헬멧을 씌운다..
                            }
                        }
                    }
                }
            }
        }
	}

    void Shield_Active(int helmetInt)    // 헬멧을 뭘로 씌워야할지 보는 함수, 0 : 그릇, 1 : 수박
    {
        if (isShield.Equals(false))                      // 쉴드(헬멧)가 없었다면..
        {
            isShield = true;                             // 쉴드(헬멧)가 되었다고 알린다.
            temp_Helmet = head_Helmet[helmetInt];        // 그릇 헬멧을 변수에 담는다.
            temp_Helmet.SetActive(true);                 // 그릇 헬멧을 활성화 시킨다.
        }
    }

    void Fly_isActive(bool isFlyActive)   // 날아다니는 오브젝트를 활성화할지, 비활성화할지 보는 함수. true면 키고, false면 끈다.
    {
        isFly = isFlyActive;              // 활성화면 활성화 됐다고, 비활성화면 비활성화 됐다고 알려줌
        fly_Obj.SetActive(isFlyActive);   // 날아다니는 오브젝트를 활성화 비활성화 한다.
    }

    void Cloud_isActive(bool isCloudActive)  // 구름을 활성화할지, 비활성화할지 보는 함수. true면 키고, false면 끈다.
    {
        isCloud = isCloudActive;          // 활성화면 활성화 됐다고, 비활성화면 비활성화 됐다고 알려줌
        for (int i = 0; i < 5; i++)       // 구름이 다섯개라 5개 모두 해야함
        {
            cloud_Col[i].gameObject.SetActive(isCloudActive);   // 구름 활성화, 비활성화 한다.
        }
    }

    void Cloud_isTouch(bool isTouch)    // 구름을 터치할 수 있는지 묻는 함수
    {
        for (int i = 0; i < 5; i++)     // 구름은 총 5개
        {
            cloud_Col[i].enabled = isTouch;     // 구름 콜라이더를 활성화면 활성화로, 비활성화면 비활성화로 한다.
        }
    }

    void Setting_Fuction(int scoreInt)    // 스테이지 세팅 전 초기화(아닌 것도 있음)하는 함수(첫번째는 실행되지 않는다..)
    {
        isSecondTouch = false;            // 터치를 몇번했는지 묻는 변수 초기화
        isCurveEnd = false;               // 그릇이 다 섞였는지 묻는 변수 초기화
        isCorrect = false;                // 정답이라고 알린 변수 초기화
        isRotEnd = false;                 // 그릇이 90도로 회전되었는지 묻는 변수 초기화

        donut_Array[0].transform.parent = null;                              // 도넛 부모 초기화(혹시 몰라서..)
        donut_Array[0].transform.rotation = Quaternion.Euler(Vector3.zero);
        donut_Array[1].transform.parent = null;
        donut_Array[1].transform.rotation = Quaternion.Euler(Vector3.zero);

        anim_Monster.Play(anim_Id01);      // 몬스터 다시 일어설떄 씀...(혹시 몰라서...)
        anim_Monster.speed = 1.0f;           // 속도 원위치 (혹시 몰라서...)
        enermy_Bat.SetActive(false);         // 뿅망치 비활성화 (혹시 몰라서...)
        touch_Bowl.transform.rotation = Quaternion.Euler(Vector3.zero);   // 그릇 회전 초기화...

        int stageLevel = 0;      // 스테이지 레벨이 몇인지 묻는 변수
        if (scoreInt < 5)        // 스코어가 5 미만이라면..
        {
            stageLevel = 0;      // 스테이지 레벨은 0이다..
        }
        else if (scoreInt < 10)    // 스코어가 10 미만이라면..
        {
            stageLevel = 1;      // 스테이지 레벨은 1이다..
        }
        else if (scoreInt < 15)    // 스코어가 15 미만이라면..
        {
            stageLevel = 2;      // 스테이지 레벨은 2이다..
        }
        else if (scoreInt < 30)    // 스코어가 30 미만이라면..
        {
            stageLevel = 3;      // 스테이지 레벨은 3이다..
        }
        else if (scoreInt < 45)    // 스코어가 45 미만이라면..
        {
            stageLevel = 4;      // 스테이지 레벨은 4이다..
        }
        else                     // 스코어가 45 이상이라면..
        {
            stageLevel = 5;      // 스테이지 레벨은 5이다..
        }


        if (isCloud.Equals(true))       // 이번 스테이지에서 구름이 나왔었다면..(혹시 모르니 비활성화)
        {
            Cloud_isActive(false);      // 구름을 비활성화
        }

        if ((scoreInt % 5).Equals(0) && !scoreInt.Equals(0))      // 승리 5마다 구름 나오도록....
        {
            Cloud_isActive(true);   // 구름을 활성화
            Cloud_isTouch(false);   // 구름을 아직은 터치 못하도록 한다...
        }

        if ((scoreInt % 20).Equals(4) && scoreInt > 43)             // 날아다니는 오브젝트 나오도록 함
        {
            Fly_isActive(true);     // 날아다니는 오브젝트를 활성화 함
        }

        Stage_fuction(stageLevel);    // 다음 스테이지 셋팅
    }


    void Stage_fuction(int stageLevel)          // 다음 스테이지 셋팅 함수
    {
        audio_Bowl.pitch = 0.1f;


        if (stageLevel >= 3)   // 도넛 2개가 나와야 함..
        {
            donut_Array[0].SetActive(true);    // 도넛 두개 활성화
            donut_Array[1].SetActive(true);

            int randInt_Two = Random.Range(0, 2);    // 정답을 미리 뽑음
            if (randInt_Two.Equals(0))               // 딸기 도넛이 걸리면...
            {
                donut_Image.sprite = sprite_Array[0];
                isDonutStrow = true;                       // 딸기 도넛이 걸렸다고 알려줌
            }
            else                                      // 초코 도넛이 걸리면...
            {
                donut_Image.sprite = sprite_Array[1];
                isDonutStrow = false;                       // 초코 도넛이 걸렸다고 알려줌
            }
        }
        else                   // 도넛 1개만 나와야 한다.
        {
            donut_Array[0].SetActive(true);      // 딸기 도넛만 활성화
        }

        donut_Image.gameObject.SetActive(false);
        switch (stageLevel)             // 스테이지 레벨에 따라~
        {
            case 0:            // 스테이지 레벨 1.. 도넛 3개
                bowlInt = 3;   // 그릇 개수를 3개로 함
                speed = 1.0f;  // 그릇 돌리는 스피드를 1로 초기화
                break;
            case 1:            // 스테이지 레벨 2.. 도넛 4개
                bowlInt = 4;   // 그릇 개수를 4개로 함
                speed = 1.0f;  // 그릇 돌리는 스피드를 1로 초기화
                break;
            case 2:            // 스테이지 레벨 3.. 도넛 5개
                bowlInt = 5;   // 그릇 개수를 5개로 함
                speed = 1.0f;  // 그릇 돌리는 스피드를 1로 초기화
                break;
            case 3:            // 스테이지 레벨 4.. 도넛 3개
                bowlInt = 3;   // 그릇 개수를 3개로 함
                speed = 1.5f;  // 그릇 돌리는 스피드를 1.5로 초기화
                break;
            case 4:            // 스테이지 레벨 5.. 도넛 4개
                bowlInt = 4;   // 그릇 개수를 4개로 함
                speed = 1.8f;  // 그릇 돌리는 스피드를 1.8로 초기화
                break;
            default:            // 스테이지 레벨 6.. 도넛 5개
                bowlInt = 5;   // 그릇 개수를 5개로 함
                speed = 2.1f;  // 그릇 돌리는 스피드를 2.1로 초기화
                break;
        }


        randInt = Random.Range(0, bowlInt);    // 그릇 위치를 랜덤으로 뽑아온다.
        for (int i = 0; i < 5; i++)            // 활성화된 그릇이 몇개인지는 모르지만 5개 모두 그냥 비활성화 시킴
        {
            cups[i].gameObject.SetActive(false);
        }


        if (bowlInt.Equals(3))     // 그릇 개수가 3개라면..
        {
            Donut_Setting(stageLevel, Pos_3x3, randInt);       // 도넛 위치 셋팅
        }
        else if (bowlInt.Equals(4))    // 그릇 개수가 4개라면..
        {
            Donut_Setting(stageLevel, Pos_4x4, randInt);       // 도넛 위치 셋팅
        }
        else                           // 그릇 개수가 5개라면..
        {
            Donut_Setting(stageLevel, Pos_5x5, randInt);       // 도넛 위치 셋팅
        }
        anim.SetBool(anim_Id02, true);       // 그릇 90도 회전시키는 애니메이션 실행
    }


    void Donut_Setting(int stageLevel, Transform[] pos_Array, int randInt)      // 도넛 위치 셋팅 함수
    {
        if (stageLevel >= 3)                  // 스테이지 레벨이 3이상이라면...
        {
            int first = Random.Range(0, bowlInt);
            int second = Random.Range(0, bowlInt);

            while (first.Equals(second))         // 중복 방지..
            {
                second = Random.Range(0, bowlInt);
            }
            
            firstInt = first;       // 딸기 도넛의 위치를 담는다.
            secondInt = second;     // 초코 도넛의 위치를 담는다.

            donut_Array[0].transform.position = pos_Array[first].position;     // 딸기 도넛의 위치를 셋팅한다.
            donut_Array[1].transform.position = pos_Array[second].position;     // 초코 도넛의 위치를 셋팅한다.
        }
        else                                   // 스테이지 레벨이 3 미만이라면..
        {
            donut_Array[0].transform.position = pos_Array[randInt].position;     // 딸기 도넛의 위치를 셋팅한다.
        }

        for (int i = 0; i < pos_Array.Length; i++)      // 배열의 길이에 따라..
        {
            cups[i].gameObject.SetActive(true);         // 앞에서 부터 활성화
            cups[i].position = pos_Array[i].position;   // 위치도 앞에서 부터..
        }
    }


    void Score_Up()         // 스코어를 올린다. (스테이지 상승)
    {
        scoreInt++;         // 스코어를 올리고
        scoreText.text = scoreInt.ToString();   // 텍스트에 반영한다.
    }

    void Fail_Up()          // 패배를 올린다. (2까지는 봐줌)
    {
        failInt--;          // 패배 점수를 올리고

        Hp_List[failInt].SetActive(false);

        if (failInt.Equals(0))
        {
            Result_Panel.SetActive(true);
            Game_Panel.SetActive(true);

            Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + scoreInt.ToString();

            if (Main.ins.nowPlayer.maxScore_List[3] >= scoreInt)    // 최고점을 못 넘은다면...
            {
                Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[3].ToString();
            }
            else        // 최고점을 넘은 경우 (신기록)
            {
                Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + scoreInt.ToString();

                Main.ins.nowPlayer.maxScore_List[3] = scoreInt;
                Main.ins.SaveData();

				GPGS_Button.interactable = true;        // 신기록을 세우면 점수 서버 등록 버튼을 활성화한다. /////////////////////////////////////////////////////////////////////////////
			}

            AudioMng.ins.Pause_BG();              // 배경음악을 끈다.
            AudioMng.ins.PlayEffect("Fail02");

            Time.timeScale = 0;
        }
    }

	public void Press_GPGS_04()   // 신기록 시, 서버에 등록
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no4, scoreInt);   // 점수를 서버에 등록
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no4);          // 리더보드를 띄운다.
	}


	public void Bowl_RotEnd()           // 애니메이션에 부착됨 ....90도 회전이 끝나면...
    {
        if (scoreInt < 15)     // 스코어가 15미만이라면..(스테이지 레벨 3 미만)
        {
            donut_Array[0].transform.parent = cups[randInt];           // 딸기 도넛의 부모를
            donut_Array[0].transform.rotation = Quaternion.Euler(Vector3.zero);   // 회전을 0으로..
        }
        else                   // 스코어가 15이상이라면..(스테이지 레벨 3 이상)
        {
            donut_Array[0].transform.parent = cups[firstInt];              // 딸기, 초코 도넛 셋팅
            donut_Array[0].transform.rotation = Quaternion.Euler(Vector3.zero);
            donut_Array[1].transform.parent = cups[secondInt];
            donut_Array[1].transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        anim.SetBool(anim_Id02, false);         // 90도 회전 애니메이션을 끝냄
        //StartCoroutine(Wait_Bowl());          //
        StartCoroutine(Shake(30));    // 이때, 그릇을 섞는다..
    }

    Vector3 BezierTest(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4, float Value)   // 배지어 곡선 함수
    {
        Vector3 A = Vector3.Lerp(P_1, P_2, Value);
        Vector3 B = Vector3.Lerp(P_2, P_3, Value);
        Vector3 C = Vector3.Lerp(P_3, P_4, Value);

        Vector3 D = Vector3.Lerp(A, B, Value);
        Vector3 E = Vector3.Lerp(B, C, Value);

        Vector3 F = Vector3.Lerp(D, E, Value);

        return F;
    }

    public void HitAndShield_Check()   // 몬스터에 때리는 애니메이션에 부착됨. 플레이어를 쳤을때 쉴드 여부 체크 함수
    {
        if (isShield.Equals(true))     // 현재 쉴드 상태라면..
        {
            AudioMng.ins.PlayEffect("HitBowl");     // 그릇으로 뿅망치을 막음
            StartCoroutine(Fly_Coroutine());
        }
        else                           // 현재 쉴드 상태가 아니라면..
        {
            AudioMng.ins.PlayEffect("HitHammer");     // 뿅망치에 맞음
            StartCoroutine(Fail_Wait());
        }
    }


    //////////////////   코루틴 구역

    IEnumerator Attack_Monster()             // 몬스터가 플레이러를 향해 공격(뿅망치)을 하는 코루틴
    {
        while (mini04_Bowl.isBuddle_End.Equals(false))    // 해당 그릇의 부들 거림이 끝났다면..
        {
            yield return delay_01;    // 부들 거림 끝날때까지 계속 돌린다..
        }

        enermy_Bat.SetActive(true);             // 뿅 망치 등장
        anim_Monster.Play(anim_Id04, -1, 0.0f);     // 상대방이 플레이어에게 뿅망치로 때리는 애니메이션
        anim_Monster.speed = 0.3f;             // 속도 조절(스테이지 마다 속도를 올려야 함)

        yield return delay_02;    // 걍 코루틴이라 맞춰놈
    }


    IEnumerator Fail_Wait()             // 패배했을 때 fail 함수 실행과 다음 스테이지로 가는 거 실행
    {
        Fail_Up();                  // 패배를 늘림
        yield return delay_02;      // 2초 기다리고..
        Setting_Fuction(scoreInt);  // 스테이지 초기화 셋팅 실행
    }


    IEnumerator Fly_Coroutine()          // 머리에 씌운 헬멧을 튕겨져 나가는 코루틴
    {
        float time = 0;         // 이 시간까지 튕겨져 나감
        isShield = false;       // 쉴드가 없어졌다고 알려줌
        Vector3 originPos_Head = temp_Helmet.transform.position;      // 헬멧의 처음 위치를 담음
        Quaternion originRot_Head = temp_Helmet.transform.rotation;   // 헬멧의 처음 회전을 다음

        while (time < 2.0f)   // 2초동안
        {
            time += Time.deltaTime;     
            temp_Helmet.transform.position = Vector3.MoveTowards(temp_Helmet.transform.position, HeadThrow_Pos.position, Time.deltaTime * 4.0f);
            temp_Helmet.transform.rotation = Quaternion.Euler(new Vector3(time * 500.0f, temp_Helmet.transform.rotation.y, temp_Helmet.transform.rotation.z));
            // 헬멧을 튕겨 나가 보이도록 한다.
            yield return delay_01;   // 2초 안 동안 계속 돌림
        }

        temp_Helmet.SetActive(false);         // 튕겨진 헬멧 비활성화.
        temp_Helmet.transform.position = originPos_Head;  // 튕겨진 헬멧을 다시 원 위치로..
        temp_Helmet.transform.rotation = originRot_Head;

        Setting_Fuction(scoreInt);           // 스테이지 초기화 셋팅 실행

        yield return delay_01;
    }




    IEnumerator Attack_Player()                      // 플레이어가 적에게 도넛 던지는 코루틴
    {
        AudioMng.ins.PlayEffect("SpeedUp");    // 사과 던지는 소리

        GameObject tempThrow_Donut;                  // 던지는 도넛을 임시로 받는 변수
        if (isDonutStrow.Equals(true))               // 던지는 변수가 딸기 도넛이라면..
        {
            donutThrow_Array[0].SetActive(true);     // 던지는 딸기 도넛 활성화..
            tempThrow_Donut = donutThrow_Array[0];   // 딸기 도넛을 임시 변수에 담는다..
        }
        else                                          // 던지는 변수가 초코 도넛이라면...
        {
            donutThrow_Array[1].SetActive(true);     // 던지는 초코 도넛 활성화..
            tempThrow_Donut = donutThrow_Array[1];   // 초코 도넛을 임시 변수에 담는다.
        }

        anim_Player.SetBool(anim_Id03, true);       // 플레이어 공격 애니메이션 실행(던지기 모션)
        bool isTouch = false;                        // 던지는 도넛이 몬스터에 맞았냐 묻는 변수

        while (isTouch.Equals(false))
        {
            tempThrow_Donut.transform.position = Vector3.MoveTowards(tempThrow_Donut.transform.position, Enemy_HeadPos.position, 4.0f * Time.deltaTime);
            // 던지는 도넛을 몬스터 머리쪽으로 던진다.
            if ((Enemy_HeadPos.position - tempThrow_Donut.transform.position).magnitude < 0.1f)  // 던지는 도넛이 몬스터와 가까워지면..
            {
                isTouch = true;                            // 맞았다고 알려줌(반복 중단)
                tempThrow_Donut.SetActive(false);          // 던지는 도넛 비활성화..
                tempThrow_Donut.transform.position = originPos_ThrowDonut;  // 던지는 도넛 위치 초기화..
                anim_Monster.Play(anim_Id05, -1, 0.0f);         // 맞아 쓰러지는 애니메이션 동작
                anim_Player.SetBool(anim_Id03, false);              // 플레이어 공격 애니메이션 중단

                AudioMng.ins.PlayEffect("HitApple");    // 상대방 사과에 맞고 쓰러지는 소리
                Score_Up();                     // 스코어를 올린다..
                yield return delay_02;          // 2초간 쉬고..

                anim_Monster.Play(anim_Id01);      // 몬스터를 다시 일어설떄 씀...

                yield return delay_02;          // 다시 2초간 쉰다..
                Setting_Fuction(scoreInt);      // 스테이지 초기화 셋팅 실행
            }

            yield return delay_01;
        }

        yield return delay_02;
    }


    IEnumerator Shake(int count)        // 그릇을 섞는 코루틴..
    {
        yield return delay_02;     // 2초간 기다린다.
        isRotEnd = true;           // 이떄 90도 회전이 끝났다고 알린다.

        if (isCloud.Equals(true))  // 구름이 켜져 있었다면..
        {
            Cloud_isTouch(true);   // 구름을 터치 할 수 있다고 알린다..
        }

        while (count > 0)        // 지정된 숫자 만큼 반복...
        {
            int first = Random.Range(0, bowlInt);
            int second = Random.Range(0, bowlInt);

            audio_Bowl.Play();
            audio_Bowl.pitch += 0.1f;

            while (first.Equals(second))           // 중복 방지..
            {
                second = Random.Range(0, bowlInt);
            }

            Transform first_Obj = cups[first];
            Transform second_Obj = cups[second];

            Vector3 origin_01 = first_Obj.position;
            Vector3 origin_02 = second_Obj.position;

            Vector3 P1_1 = first_Obj.position;
            Vector3 P2_1 = new Vector3(first_Obj.position.x, -1.15f , 1.55f);   // -0.5f
            Vector3 P3_1 = new Vector3(second_Obj.position.x, -1.15f, 1.55f);   // -0.5f
            Vector3 P4_1 = second_Obj.position;
             
            Vector3 P1_2 = first_Obj.position;
            Vector3 P2_2 = new Vector3(first_Obj.position.x, -1.15f, -0.5f);   // -0.5f
            Vector3 P3_2 = new Vector3(second_Obj.position.x, -1.15f, -0.5f);   // -0.5f
            Vector3 P4_2 = second_Obj.position;


            while (Test <= 1)
            {
                Test += Time.deltaTime * speed;

                first_Obj.position = BezierTest(P1_1, P2_1, P3_1, P4_1, Test);
                second_Obj.position = BezierTest(P4_2, P3_2, P2_2, P1_2, Test);

                yield return delay_01;
            }
            speed += 0.1f;

            first_Obj.position = origin_02;
            second_Obj.position = origin_01;
            Test = 0;
            count--;
            yield return delay_01;
        }

        

        isCurveEnd = true;           // 그릇이 전부 섞였다고 알린다.
        donut_Image.gameObject.SetActive(true);

        if (isCloud.Equals(true))    // 구름이 활성화 되어 있다면..
        {
            Cloud_isTouch(false);    // 구름 터치를 못해게 한다..
        }

        if (isFly.Equals(true))      // 날아다니는 오브젝트가 활성화 되어 있다면..
        {
            Fly_isActive(false);     // 날아다니는 오브젝트를 비활성화..
        }
    }
}
