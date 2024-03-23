using System.Collections;
using UnityEngine;

public class Mini02_Guest : MonoBehaviour
{
    public Transform target;       // 카운터 앞에 있는 주문 라인을 받는 변수
    CapsuleCollider col;          // 자신에게 붙어 있는 콜라이더를 받아오는 변수

    Vector3 originPos;             // 스폰지역으로 되돌아올 떄 사용되기 위한 변수

    public bool endBool = false;   // 주문이 끝났다는 것을 알리는 변수

    Animator anim;                  // 자신에게 붙어 있는 애니메이터를 받을 변수
    int posKeyId;                   // 애니메이터를 최적화하기 위한 인트키

    bool isStop = true;           // 바로 앞에 손님이 있다면 멈추기 위한 변수
    bool counterStop = false;     // 카운트에 도착했다면...

    public int charInt = 0;       // 자신이 어떤 도넛을 원하는지 뽑는 랜덤 인트 변수
                                  // (0 = 구멍/오븐/핑크, 1 = 구멍/오븐/초코, 2 = 구멍/튀김/핑크, 3 = 구멍/튀김/초코,
                                  //  4 = 스타/오븐/핑크, 5 = 스타/오븐/초코, 6 = 스타/튀김/핑크, 7 = 스타/튀김/초코)

    public Mini02_Player mini02_Player;    // 스폰 스크립트에서 플레이어 스크립트를 받아옴
    public Mini02_Spawn mini02_Spawn;      // 스폰 스크립트에서 스폰 스크립트를 받아옴
    Mini02_CountLine mini02_CountLine;     // 카운트라인 스크립트를 받아옴

    WaitForSeconds delay;

    LayerMask mask;              // 레이어를 미리 설정

    Quaternion countRot;         // 카운트 쪽으로 가는 회전 값
    Quaternion endRot;           // 시작 지점 쪽으로 가는 회전 값

    Coroutine coroutine;

    void Awake()
    {
        originPos = transform.position;      // 처음위치(스폰 위치)를 변수에 담는다.

        col = GetComponent<CapsuleCollider>();          // 자신의 콜라이더 등록
        anim = GetComponent<Animator>();                   // 자신의 애니메이터 등록
        posKeyId = Animator.StringToHash("isRun");          // 최적화 작업

        countRot = Quaternion.Euler(0, -90, 0);   // 회전 값 설정
        endRot = Quaternion.Euler(0, 90, 0);

        mini02_CountLine = target.GetComponent<Mini02_CountLine>(); 
        mask = LayerMask.GetMask("Monster");  // 손님만 받기
        delay = new WaitForSeconds(0.05f);
    }


	void OnEnable()
	{
        charInt = Random.Range(0, 8);                     // 랜덤으로 도넛을 뽑는다.
        counterStop = false;       // 카운트에 도착했다는 변수 초기화
        endBool = false;           // 끝나는 변수 초기화
        col.isTrigger = false;     // 손님 트리거 끄기

        StartCoroutine(updateCoroutine());      // 최적화 코루틴
    }



	void FixedUpdate()
    {
        if (isStop.Equals(true))      // 멈춘다면 리턴
        {
            return;
        }

        if (endBool.Equals(false))    // 끝나지 않았다면 카운트를 향해 달린다.
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 2 * Time.fixedDeltaTime);
            transform.rotation = countRot;      // 처음지역으로 바라보기
        }
        else   // 끝났다면 처음지역으로 달린다.
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, 2 * Time.fixedDeltaTime);
            transform.rotation = endRot;      // 처음지역으로 바라보기
        }
    }

    public void GoBack()       // 카운터 지역에서 끝나면 처음지역으로 되돌아가는 함수
    {
        endBool = true;          // 일단 끝내기
        col.isTrigger = true;     // 다른 손님하고 안 겹치기 위해서, 트리거 켜두기

        this.gameObject.layer = 8;    // 자신의 레이어도 바꾸기

        StopCoroutine(coroutine);
    }


    IEnumerator updateCoroutine()             // 코루틴으로 최적화
    {
        while (true)
        {
            if (col.isTrigger.Equals(true) && (transform.position - originPos).magnitude < 0.2f)  // 자신의 콜라이더가 트리거 상태이고, 처음지역까지 거의 다 오면 
            {
                //mini02_Spawn.InsertQueue_Monster(transform.gameObject);        // 없애버림

                this.gameObject.layer = 7;
                mini02_Spawn.spawnInt--;                      // 스폰 숫자 줄이기
                transform.gameObject.SetActive(false);
            }

            RaycastHit hit;

            if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), out hit, 1.0f, mask) && col.isTrigger.Equals(false))
            // 손님이 바라보는 쪽으로 레이저쏘기, 그리고 상대가 트리거가 안켜져 있어야 한다.
            {
                isStop = true;   // 트리거가 꺼진 손님이 있으면 멈춘다. 
            }
            else
            {
                isStop = false;  // 트리거가 켜진 손님이 있으면 무시
            }


            if (isStop.Equals(true) || (target.position - transform.position).magnitude < 0.1f)  // 현재 멈추지 않았거나, 목표 지점에 도착했다면
            {
                anim.SetBool(posKeyId, false);   // 달리기 모션 꺼짐
            }
            else
            {
                anim.SetBool(posKeyId, true);     // 달리기 모션 켜짐
            }

            if ((transform.position - target.position).magnitude < 0.01f && counterStop.Equals(false))      // 몬스터가 카운터에 도착했다면...
            {
                counterStop = true;    // 카운트에 도착했다고 알림
                coroutine = StartCoroutine(GoBackCoroutine());           // 되돌아오게 하는 코루틴

                mini02_CountLine.SettingMenu(charInt, this, goBackTime);        // 메뉴 셋팅하기!

                
            }

            yield return delay;
        }
    }

    float goBackTime = 0;

    IEnumerator GoBackCoroutine()        // 되돌아오게 하는 코루틴
    {
        if (mini02_Player.scoreInt <= 3)             // 스코어가 10 이하라면...
        {
            goBackTime = 60.0f;                      // 되돌아가는 시간을 60초로 설정
        }
        else if (mini02_Player.scoreInt <= 6)             // 스코어가 20 이하라면...
        {
            goBackTime = 57.0f;                      // 되돌아가는 시간을 57초로 설정
        }
        else if (mini02_Player.scoreInt <= 9)             // 스코어가 30 이하라면...
        {
            goBackTime = 54.0f;                      // 되돌아가는 시간을 54초로 설정
        }
        else if (mini02_Player.scoreInt <= 12)             // 스코어가 40 이하라면...
        {
            goBackTime = 51.0f;                      // 되돌아가는 시간을 51초로 설정
        }
        else if (mini02_Player.scoreInt <= 15)             // 스코어가 50 이하라면...
        {
            goBackTime = 48.0f;                      // 되돌아가는 시간을 48초로 설정
        }
        else if (mini02_Player.scoreInt <= 18)             // 스코어가 60 이하라면...
        {
            goBackTime = 45.0f;                      // 되돌아가는 시간을 45초로 설정
        }
        else if (mini02_Player.scoreInt <= 21)             // 스코어가 70 이하라면...
        {
            goBackTime = 42.0f;                      // 되돌아가는 시간을 42초로 설정
        }
        else if (mini02_Player.scoreInt <= 24)             // 스코어가 80 이하라면...
        {
            goBackTime = 39.0f;                      // 되돌아가는 시간을 39초로 설정
        }
        else if (mini02_Player.scoreInt <= 27)             // 스코어가 90 이하라면...
        {
            goBackTime = 36.0f;                      // 되돌아가는 시간을 36초로 설정
        }
		else if (mini02_Player.scoreInt <= 30)             // 
		{
			goBackTime = 33.0f;                      // 
		}
		else if (mini02_Player.scoreInt <= 33)             // 
		{
			goBackTime = 30.0f;                      // 
		}
		else                                              // 
        {
            goBackTime = 27.0f;                      // 되돌아가는 시간을 27초로 설정
        }


        yield return new WaitForSeconds(goBackTime);  // 지정된 시간 후에 되돌아가게 하는 

        AudioMng.ins.LoopEffect(false);
        AudioMng.ins.PlayEffect("Fail02");      // 손님 그냥 떠남
        mini02_CountLine.GoHome_Guest(true);        // 체력을 닳게하고 손님을 되돌아가게 하는 변수
    }
}
