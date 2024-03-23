using System.Collections;
using UnityEngine;

public class Mini06_Bee : MonoBehaviour
{
    enum MonsterState
    {
        Moving,
        AttackWait,
        Attack,
        Back,
    }
    MonsterState State;

    GameObject nearObject;                // 현재 벌과 닿은 양봉통

    Vector3 backPos;                      // 공격하기 직전 장소

    Vector3 tempVec;         // 랜덤 위치를 담는 변수

    bool isRun_Wait = false;                 // 공격 목표 설정 후 부터, 되돌아 온 후까지 공격 못하도록 막는 변수
    bool isWaitRun = false;          // 공격 전 코루틴이 실행되고 있는지 묻는 변수(한번만 실행할려고...)

    public Mini06_Spawn mini06_Spawn;      // 반납 시키기 위한 스폰 스크립트
    public Mini06_Player mini06_Player;    // 플레이어 스크립트
    public Mini06_Bear mini06_Bear;        // 곰 스크립트
    Mini06_Box mini06_Box;                 // 박스는 여러개라 레이에 닿을때만 스크립트 가져오는 변수

    Animator anim;                       // 애니메이터는 플레이어와 몬스터가 공통으로 지닐 가능성 높음


    WaitForSeconds delay_01;
    WaitForSeconds delay_02;
    WaitForSeconds delay_03;

    int runId;
    int attackId;

    string tag03;

    Vector3 worldDown;             // 이 벌 아래 방향

    void Awake()
	{
		anim = transform.GetComponent<Animator>();

        runId = Animator.StringToHash("posKey");
        attackId = Animator.StringToHash("attackKey");

        tag03 = "Cushion";

        worldDown = new Vector3(0, -1, 0);               // 이 벌 아래 방향

        delay_01 = new WaitForSeconds(0.2f);
        delay_02 = new WaitForSeconds(2.0f);
        delay_03 = new WaitForSeconds(3.0f);
    }

	void OnEnable()
	{
        State = MonsterState.Moving;

        tempVec.x = Random.Range(-19.5f, 19.5f);                         // 다시 위치 랜덤
        tempVec.y = 2.0f;
        tempVec.z = Random.Range(-15.5f, 15.5f);                         // 다시 위치 랜덤

        StartCoroutine(RayCoroutine());        // 아래로 레이를 쏨
    }


	void OnDisable()
    {
        mini06_Spawn.list_Bee.Add(transform.gameObject);        // 리스트에 다시 넣는다..
        isRun_Wait = false;
        isWaitRun = false;           // 공격 전 코루틴이 끝났다고 알림
    }

	void FixedUpdate()
	{
        switch (State)
        {
            case MonsterState.Moving:         // 움직이는 상태
                RunToRandPos();
                break;
            case MonsterState.AttackWait:     // 공격 전 대기 상태
                if (isWaitRun.Equals(false))         // 코루틴이 실행되고 있지 않다면... 한번만 실행하기 위해서...
                {
                    StartCoroutine(Before_Attack_Coroutine());       // 공격 직전에 
                }

                transform.LookAt(nearObject.transform);         // 목표를 보는거는 계속 해야 한다.
                break;
            case MonsterState.Attack:         // 공격 하는 상태
                Attack_Bee();
                break;
            default:                          // 공격 직전 위치로 돌아가는 상태
                Back_Bee();
                break;
        }
    }



    void RunToRandPos()             // 다음 위치로 가는 함수
    {
        anim.SetBool(runId, true);        // 이동 애니메이션 실행

        transform.position = Vector3.MoveTowards(transform.position, tempVec, Time.deltaTime * 4);    // 랜덤으로 뽑은 위치로 간다.
        transform.LookAt(tempVec);         // 위치를 보면서 간다.

        if ((tempVec - transform.position).magnitude < 0.1f)       // 목표 위치에 다다르면...
        {
            tempVec.x = Random.Range(-19.5f, 19.5f);                         // 다시 위치 랜덤
            tempVec.z = Random.Range(-15.5f, 15.5f);                         // 다시 위치 랜덤
        }
    }


    void Attack_Bee()                // 목표로 다가가는 함수
    {
        transform.position = Vector3.MoveTowards(transform.position, nearObject.transform.position, Time.deltaTime * 6);    // 해당 양봉통으로 돌격!
        transform.LookAt(nearObject.transform);     // 해당 양봉통을 계속 본다.

        if ((nearObject.transform.position - transform.position).magnitude < 0.1f)    // 해당 양봉통에 다다르면...
        {
            if (nearObject.layer.Equals(4))      // 목표가 양봉통이라면...        Water
            {
                if (mini06_Box.hp_Int < 3)        // 다시 확인
                {
                    mini06_Box.Hit_Fuction();      // 체력을 깍는다.
                }
            }
            else if (nearObject.layer.Equals(6))     // 목표가 플레이어라면...
            {
                
                mini06_Player.MinusHp_Mini06();      // 체력을 깍고 하트 이미지 깍기
            }
            else if (nearObject.CompareTag(tag03))        // 목표가 곰이라면...
            {
                mini06_Bear.Minus_Bear();

            }
            State = MonsterState.Back;     // 되돌아가기
        }
    }



    void Back_Bee()           // 다시 되돌아가는 함수
    {
        anim.SetBool(runId, true);               // 이동 애니메이션을 킨다.
        anim.SetBool(attackId, false);           // 공격 애니메이션을 끈다.

        transform.position = Vector3.MoveTowards(transform.position, backPos, Time.deltaTime * 4);     // 공격 전 설정한 위치로 되돌아간다.
        transform.LookAt(backPos);      // 해당 위치를 향해 본다.

        if ((backPos - transform.position).magnitude < 0.1f)     // 되돌아가는 위치에 다다르면...
        {
            StartCoroutine(Wait_AfterAttack());     // 몇 초간 공격 못하도록 막는 코루틴 실행(사실 계속 막고 있음...)
            State = MonsterState.Moving;              // 대기 상태로 감
        }
    }


    IEnumerator RayCoroutine()        // 아래로 레이를 쏨
    {
        while (true)
        {
            if (isRun_Wait.Equals(false))
            {
                
                RaycastHit hitInfo5;
                if (Physics.Raycast(this.transform.position, worldDown, out hitInfo5, 7.0f))   // 월드 방향 아래로 레이저
                {
                    if (hitInfo5.collider.gameObject.layer.Equals(4))    // 양봉통에 닿으면...           Water
                    {
                        nearObject = hitInfo5.transform.gameObject;           // 닿은 오브젝트를 넘긴다.
                        mini06_Box = hitInfo5.transform.GetComponent<Mini06_Box>();
                        if (mini06_Box.hp_Int < 3)    // 양봉통의 체력이 있다면..
                        {
                            State = MonsterState.AttackWait;                  // 공격전 상황으로 간다.
                        }
                    }
                    else if (hitInfo5.collider.gameObject.layer.Equals(6))    // 플레이어에 닿으면...   Player
                    {
                        nearObject = hitInfo5.transform.gameObject;       // 닿은 오브젝트를 넘긴다.

                        if (mini06_Player.isAlive.Equals(true))      // 플레이어가 살아 있다면..
                        {
                            State = MonsterState.AttackWait;                  // 공격전 상황으로 간다.
                        }
                    }
                    else if (hitInfo5.transform.CompareTag(tag03))      // 곰이라면...
                    {
                        nearObject = hitInfo5.transform.gameObject;

                        if (mini06_Bear.isCrazy.Equals(false) || mini06_Bear.isBack.Equals(false))   // 폭주나 돌아가는 상태가 아니라면..
                        {
                            State = MonsterState.AttackWait;                  // 공격전 상황으로 간다.
                        }

                    }
                }
            }
            yield return delay_01;       // 0.2초 마다
        }
    }




    IEnumerator Before_Attack_Coroutine()         // 공격 하기 전 잠시 위에서 기다리게 하는 코루틴
    {
        isWaitRun = true;           // 공격 전 코루틴이 실행되고 있다고 알림
        isRun_Wait = true;          // 공격을 실행하고 있다고 알림(이 벌이 다른 거 공격 못하도록 막는 역할...)

        anim.SetBool(runId, false);            // 이동 애니메이션을 끄고
        anim.SetBool(attackId, true);          // 공격 애니메이션을 킨다. 애니메이션 길을 막았기 때문에 공격 애니메이션 마지막에서 멈춤

        backPos = transform.position;             // 현재 위치를 되돌아갈 위치로 넘긴다.
        yield return delay_03;

        State = MonsterState.Attack;              // 몇 초 기다린 다음 공격을 한다.

        isWaitRun = false;           // 공격 전 코루틴이 끝났다고 알림
    }

    
    IEnumerator Wait_AfterAttack()           // 공격 후 같은 거를 공격하지 않게 트리거를 막는 코루틴
    {
        yield return delay_02;
        isRun_Wait = false;        // 이제 공격이 가능하다고 알림
    }
}
