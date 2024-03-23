using System.Collections;
using UnityEngine;

public class Mini06_Bear : MonoBehaviour
{
    enum MonsterState
    {
        Idle,
        Moving,
        AttackWait,
        Attack,
        Back,
    }
    MonsterState State;


    [SerializeField] Transform[] BearSitPos;     // 곰이 앉을 위치

    [SerializeField] SkinnedMeshRenderer BearRender;         // 곰 렌더러(폭주할때 바꿀려고...)
    Color originColor;                             // 곰 렌데러 메테리얼 색깔을 바꿀 컬러(폭주 때...)

    public int BearHp;     // 곰의 체력을 알리는 변수

    public bool isCrazy = false;            // 폭주 상태로 알리는 변수
    public bool isBack = false;             // 돌아가고 있다고 알리는 변수

    Vector3 tempSitPos;                // 폭주 끝나고, 곰이 앉을 자리를 받는 임시 변수

    public GameObject bear_Web;        // 곰을 감싸는 그물 

    Coroutine coroutine_CrazyStop;     // 폭주상태를 끝내는 코루틴을 받을 변수
    Coroutine coroutine_Check;         // 폭주 상태에서 랜덤 위치를 찾는 코루틴을 받을 변수

    bool isFirst_Idle = false;         // 대기 상태에 돌입했을 때, 중복을 방지하기 위한 변수
    bool isCheck_Idle = false;         // 폭주 상태에서 대기 상태의 중복 방지 변수

    Vector3 tempVec;         // 랜덤 위치를 담는 변수

    bool isRun_Web = false;            // 그물에 걸렸을 때 중복 방지 변수
    bool isPlayer_Hit = false;         // 플레이어를 히트 시킬 수 있는지 묻는 변수

    BoxCollider boxCol;                // 곰 콜라이더

    public GameObject player_Mini06;
    Mini06_Player mini06_Player;         // 플레이어 스크립트를 받는 변수
    Rigidbody player_Rigid;


    WaitForSeconds delay_Crazy;          // 폭주 시간..

    Vector3 col_Size01;           // 곰 콜라이더 작은 사이즈
    Vector3 col_Center01;
    Vector3 col_Size02;           // 곰 콜라이더 큰 사이즈
    Vector3 col_Center02;

    Animator anim;                       // 애니메이터는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    WaitForSeconds delay_01;
    WaitForSeconds delay_02;

    int runId;
    int attackId;
    int webId;

    string tag01;
    string tag03;

    string invoke_Text01;

    void Start()
    {
        runId = Animator.StringToHash("posKey");
        attackId = Animator.StringToHash("posAttack");
        webId = Animator.StringToHash("posWeb");

        tag01 = "Monster";
        tag03 = "Spring";

        invoke_Text01 = "Invoke_PlayerHit";

        BearHp = 2;                 // 벌한테 두대 맞으면 폭주하도록 하자!
        
        State = MonsterState.Idle;      // 처음에는 앉아있는 상태로..

        originColor = BearRender.material.color;           // 현재 스킨 메테리얼의 색깔을 받는다.

        anim = transform.GetComponent<Animator>();              // 자신의 애니메이션을 받는다.
        boxCol = transform.GetComponent<BoxCollider>();         // 자신의 콜라이더를 받는다.

        mini06_Player = player_Mini06.GetComponent<Mini06_Player>();
        player_Rigid = player_Mini06.GetComponent<Rigidbody>();

        col_Center01 = new Vector3(0.0f, 0.5f, -0.7f);
        col_Size01 = new Vector3(2.0f, 0.9f, 1.7f);
        col_Center02 = new Vector3(0.0f, 0.9f, 0.2f);
        col_Size02 = new Vector3(2.0f, 1.8f, 2.5f);

        delay_01 = new WaitForSeconds(1.0f);
        delay_02 = new WaitForSeconds(3.0f);
        delay_Crazy = new WaitForSeconds(30.0f);
    }

    void FixedUpdate()
    {
        switch (State)
        {
            case MonsterState.Idle:            // 움직이기 전   
                Idle();
                break;
            case MonsterState.AttackWait:          // 다른 곳으로 달리기 전 체크 상태
                CheckIdle();
                break;
            case MonsterState.Moving:      // 달리기 상태
                Crazy();
                break;
            case MonsterState.Attack:          // 플레이어가 던진 그물에 맞을때 상태
                WepIdle();
                break;
            default:                           // 곰이 앉을 자리로 가는 상태
                BackSit();
                break;
        }
    }


    void Idle()
    {
        if (isFirst_Idle.Equals(false))          // 중복 방지...
        {
            isFirst_Idle = true;                 // 최적화 변수
            boxCol.center = col_Center01;           // 콜라이더의 사이즈를 작게한다.
            boxCol.size = col_Size01;              

            anim.SetBool(runId, false);          // 애니메이션을 기본 상태로(앉는 거) 한다.
            anim.SetBool(attackId, false);
        }
    }


    void CheckIdle()
    {
        if (isCheck_Idle.Equals(false))          // 중복 방지
        {
            isCheck_Idle = true;                 // 최적화
            coroutine_Check = StartCoroutine(Check_Idel());               // 체크 코루틴을 실행한다.
        }
    }


    void Crazy()                      // 폭주 상태 함수
    {
        anim.SetBool(attackId, false);       // 이동 애니메이션을 킨다.
        anim.SetBool(runId, true);

        transform.position = Vector3.MoveTowards(transform.position, tempVec, Time.deltaTime * 10);    // 랜덤으로 뽑은 위치로 간다.
        transform.LookAt(tempVec);         // 위치를 보면서 간다.

        if ((tempVec - transform.position).magnitude < 0.1f)       // 목표 위치에 다다르면...
        {
            State = MonsterState.AttackWait;                         // 랜덤 체크 상태로 간다.
        }
    }


    void WepIdle()
    {
        if (isRun_Web.Equals(false))          // 중복 방지
        {
            isRun_Web = true;
            StartCoroutine(Wep_Coroutine());            // 그물 코루틴을 실행한다.
        }
    }


    void BackSit()            // 앉을 자리로 가는 함수
    {
        if (isCrazy.Equals(true))                    // 처음 여기에 왔을 때 true라면...
        {
            isCrazy = false;                    // false로 바꿔서 다시 못 들어오게 하기...
            isBack = true;                      // 되돌아간다고 알림

            anim.SetBool(runId, true);       // 이동 애니메이션을 킴
            anim.SetBool(attackId, false);
            BearRender.material.color = originColor;      // 메테리얼의 색깔을 다시 원래대로 바꾼다.
        }

        transform.position = Vector3.MoveTowards(transform.position, tempSitPos, Time.deltaTime * 2);        // 앉을 위치로 이동
        transform.LookAt(tempSitPos);           // 앉을 위치를 바라보기

        if ((tempSitPos - transform.position).magnitude < 0.1f)       // 앉을 위치에 다다르면...
        {
            isBack = false;               // 되돌아간다는 것도 꺼버림
            BearHp = 2;                   // 다시 체력 회복

            isFirst_Idle = false;          // 중복 방지를 끔
            State = MonsterState.Idle;     // 앉은 상태로 바꿈
        }
    }


    public void Minus_Bear()                       // 곰이 벌에 쏘이면...
    {
        if (!BearHp.Equals(0))                     // 곰의 체력이 0이 아니라면..
        {
            BearHp--;                              // 곰의 체력을 깍는다.

            if (BearHp.Equals(0))                  // 깍는 체력이 0이 되었다면..
            {
                isCrazy = true;                    // 폭주상태라고 알림
                
                BearRender.material.color = Color.red;        // 폭주 상태에는 메테리얼의 색깔을 빨강으로 바꾼다.

                coroutine_CrazyStop = StartCoroutine(CrazyStopCoroutine());    // 폭주가 시작되면 끝내는 코루틴을 실행한다.
                State = MonsterState.AttackWait;                                // 이동을 위한 폭주 체크 상태로 넘어간다.
            }
        }
    }


    void Sit_Bear()        // 근처에 곰이 앉을 위치 찾는 함수
    {
        tempSitPos = BearSitPos[0].position;
        for (int i = 1; i < BearSitPos.Length; i++)        // 위치 리스트를 돌리면서
        {
            tempSitPos = (transform.position - tempSitPos).magnitude <= (transform.position - BearSitPos[i].position).magnitude ? tempSitPos : BearSitPos[i].position;
            // 어느 위치가 더 현재 위치와 가까운지 확인하며, 변수에 집어 넣는다.
        }
    }


    //////////////////  코루틴 관련...


    IEnumerator Check_Idel()                  // 랜덤 위치를 뽑는 체크 코루틴
    {
        tempVec.x = Random.Range(-19.5f, 19.5f);                         // 다시 위치 랜덤
        tempVec.z = Random.Range(-15.5f, 15.5f);                         // 다시 위치 랜덤


        anim.SetBool(runId, false);
        anim.SetBool(attackId, true);                  // 으르렁 거리는 애니메이션을 킨다.

        boxCol.center = col_Center02;    // 콜라이더의 사이즈를 크게 한다.
        boxCol.size = col_Size02;

        AudioMng.ins.PlayEffect("BearGrowl");    // 곰 폭주
        yield return delay_01;           // 1초 간 쉰 뒤...

        isCheck_Idle = false;                            // 중복 방지 변수를 풀고
        State = MonsterState.Moving;                      // 다시 폭주 상태로 간다.
    }



    IEnumerator CrazyStopCoroutine()                    // 폭주 후, 폭주를 끝내는 코루틴
    {
        yield return delay_Crazy;           // 크레이지 모드 발동 후, 30초가 되면 가만히 있기...

        Sit_Bear();

        StopCoroutine(coroutine_Check);      // 혹시라도 체크 코루틴이 실행 중이라면 꺼버린다.

        isCheck_Idle = false;                // 중복 방지 변수를 푼다.
        State = MonsterState.Back;        // 어느 위치가 가까운지 확인 했으니, 그 위치로 돌아가는 상태로 한다.
    }



    IEnumerator Wep_Coroutine()                // 그물에 맞았을 때... 대기 상태
    {
        if (isCrazy.Equals(true))                   // 처음 여기에 왔을 때 true라면...
        {
            isCrazy = false;                   // false로 바꿔서 다시 못 들어오게 하기...
            isBack = true;

            BearRender.material.color = originColor;        // 메테리얼의 색깔을 다시 원래대로 바꾼다.
        }

        StopCoroutine(coroutine_CrazyStop);         // 폭주를 끄는 코루틴을 중단
        StopCoroutine(coroutine_Check);             // 체크 코루틴도 중단

        isCheck_Idle = false;                       // 중복 방지를 푼다.

        anim.SetBool(runId, false);
        anim.SetBool(attackId, false);
        bear_Web.SetActive(true);                   // 그물 오브젝트를 활성화
        anim.SetBool(webId, true);               // 그물 애니메이션 킨다.

        yield return delay_02;      // 3초 후에..

        anim.SetBool(webId, false);             // 그물 애니메이션을 끈다. 
        bear_Web.SetActive(false);                 // 그물 오브젝트를 비활성화

        anim.SetBool(runId, true);              // 이동 애니메이션을 킨다.
        anim.SetBool(attackId, false);

        Sit_Bear();

        isRun_Web = false;              // 중복 방지를 푼다.
        State = MonsterState.Back;   // 앉을 자리로 돌아가는 상태로
    }


    void Invoke_PlayerHit()
    {
        isPlayer_Hit = false;                        // 플레이어를 히트 시킬 수 있다고 알림
    }



    ///////////////////  트리거 구역


    void OnTriggerEnter(Collider other)
    {
        if (isCrazy.Equals(true) && isBack.Equals(false))             // 폭주상태이고, 돌아가지 않는 상황이라면...
        {
            if (other.gameObject.CompareTag(tag01))          // 벌에 닿았을 경우..
            {
                other.gameObject.SetActive(false);          // 반납 시켜야 함
                AudioMng.ins.PlayEffect("Score_Up");    // 벌 죽는 소리
            }

            if (other.gameObject.layer.Equals(6) && isPlayer_Hit.Equals(false))           // 플레이어에 닿고, 히트시킬 수 있는 상황이라면...
            {
                mini06_Player.isTouch_Bear = true;

                player_Rigid.velocity = (other.transform.position - transform.position).normalized * 50.0f; // 장풍 효과..

                if (mini06_Player.isAlive.Equals(true))            // 플레이어가 아직 살아 있다면...
                {                    
                    isPlayer_Hit = true;                    // 플레이어를 히트시킬 수 없다고 알림
                    mini06_Player.MinusHp_Mini06();    // 체력을 깍고 하트 이미지 깍기

                    Invoke(invoke_Text01, 1.0f);
                }
            }

            if (other.gameObject.CompareTag(tag03))           // 그물에 맞았을 경우...
            {
                other.gameObject.SetActive(false);          // 반납 시켜야 함
                AudioMng.ins.PlayEffect("BearWeb");    // 곰 폭주
                State = MonsterState.Attack;               // 그물을 맞고 대기하는 상태로 간다.
            }

            if (other.gameObject.layer.Equals(4))       // 양봉통에 닿으면...          Water
            {
                if (other.GetComponent<Mini06_Box>().hp_Int < 3)    // 만약 이 양봉통의 체력이 1 이상이라면...
                {
                    other.GetComponent<Mini06_Box>().Hit_Fuction();      // 체력을 깍는다.
                }
            }
        }
    }
}
