using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Mini01_Monster : MonoBehaviour     // 몬스터에 부착됨
{
    Vector3 originalPos;      // 몬스터가 처음 생성된 위치를 받을 변수

    public Transform player;       // 확인된 플레이어 오브젝트
    public Mini01_Player mini01_Player;           // 플레이어의 스크립트를 변수

    bool isSee = false;

    Coroutine rayCoroutine;

    Animator anim_Mon;         // 몬스터의 애니메이터를 받는 변수

    float m_distance = 4;    //

    WaitForSeconds delay_01;

    int runId;                    // 달리는 애니메이터를 받는 변수
    int attackId;                 // 공격하는 애니메이터를 받는 변수

    LayerMask P_layerMask;

    NavMeshAgent nma;

    enum MonsterState
    {
        Idle,
        Moving,
        Attack,
        Back,
    }
    MonsterState State;
    

    void Awake()
    {
        anim_Mon = gameObject.GetComponent<Animator>();   // 애니메이터를 변수로 넘긴다.
        runId = Animator.StringToHash("isRun");              // 달리는 애니메이터 저장
        attackId = Animator.StringToHash("isAttack");        // 공격하는 애니메이터 저장

        nma = gameObject.GetComponent<NavMeshAgent>();       // 이 몬스터를 네브메쉬를 넘김


        delay_01 = new WaitForSeconds(0.7f);    // 0.7초 마다 실행

        P_layerMask = LayerMask.GetMask("Player");           // 레이어를 플레이어만 필요함;
    }


	void OnEnable()    // 이 몬스터가 켜질 때..
	{
        State = MonsterState.Idle;       // 처음에는 기다리는 상태로...
        rayCoroutine = StartCoroutine(MonsterCoroutine());       // 레이를 쓰는 코루틴 실행

        originalPos = transform.position;            // 켜질 때의 위치를 저장
    }

	void OnDisable()   // 이 몬스터가 꺼질 때...
	{
        StopCoroutine(rayCoroutine);     // 레이를 쏘던 코루틴 중단시킴
    }


    void FixedUpdate()
    {
        switch (State)               // 이 몬스터의 상태에 따라 행동이 달라짐
        {
            case MonsterState.Idle:    // 기다리는 상태라면..
                Idle_FindCheck();
                break;
            case MonsterState.Moving:  // 움직이는 상태라면..
                Run_ToPlayer();
                break;
            case MonsterState.Attack:  // 공격하는 상태라면..
                Attack_ToPlayer();
                break;
            case MonsterState.Back:    // 원래 자리로 되돌아가는 상태라면..
                Back_ToOriginalPos();
                break;
        }
	}

    void Mon_Sight()         // 몬스터와 플레이어와의 사이에 뭐가 있는지 체크
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_distance, P_layerMask);       // 몬스터 주위에 플레이어의 콜라이더 탐색

        if (t_cols.Length > 0)       // 탐색된 콜라이더가 있다면...
        {
            Transform t_tfPlayer = t_cols[0].transform;   // 탐색된 콜라이더는 플레이어가 분명하니 위치 정보를 넘긴다.

            Vector3 t_direction = (t_tfPlayer.position - transform.position).normalized;     // 몬스터와 플레이어와의 방향을 구한다.

            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, t_direction, out RaycastHit t_hit, m_distance))
                // 이 몬스터에서 플레이어 쪽으로 레이를 쏜다. 근데 중간에 가로 막는(벽) 게 있다면 플레이를 못 찾았다고 한다...
            {
                if (t_hit.collider.gameObject.layer.Equals(6))    // 하지만 플레이어와 사이에 아무 것도 없다면..
                {
                    anim_Mon.SetBool(runId, true);                         // 달리는 애니메이션을 바꿈
                    State = MonsterState.Moving;               // 이 몬스터 상태를 플레이어를 향해 달리는 것으로 바꿈
                }
            }
        }
    }


    void Idle_FindCheck()  // 현재 제자리에 멈춘 상태(플레이어가 스캔 영역 안으로 오기를 기다리는 중)
    {
        if (isSee.Equals(true))                        // 플레이어를 봤으면..(이거는 되돌아왔다는 거임... 결국 : 아래 애니메이션 한번만 실행하기 위한 것..)
        {
            isSee = false;                             // 한번만 실행하기 위해 바로 바꿈
            anim_Mon.SetBool(runId, false);         // 서 있는 애니메이션 실행
        }
    }


    void Run_ToPlayer()  // 현재 플레이어가 스캔 영역 안에 있어, 플레이어를 쫓아 가는 중
    {
        isSee = true;      // 플레이어를 봤다고 알림
        Vector3 dir = player.position - transform.position;        // 이 몬스터와 플레이어와의 거리를 구한다.

        if ((originalPos - transform.position).magnitude > 20)           // 그 거리가 20보다 높다면...
        {
            State = MonsterState.Back;                            // 몬스터를 원래 위치로 되돌아가게 한다.
            anim_Mon.SetBool(runId, true);                         // 달리는 애니메이션을 바꿈
            return;                                               // 아래 실행 못 하게 한다.
        }

        nma.SetDestination(player.position);                // 플레이어의 위치로 몬스터 이동
        nma.updateRotation = false;
        nma.acceleration = 10.0f;

        Vector2 forward = new Vector2(transform.position.z, transform.position.x);          // 아래는 회전 관련?
        Vector2 steeringTarget = new Vector2(nma.steeringTarget.z, nma.steeringTarget.x);

        //방향을 구한 뒤, 역함수로 각을 구한다.
        Vector2 dir2 = steeringTarget - forward;
        float angle = Mathf.Atan2(dir2.y, dir2.x) * Mathf.Rad2Deg;

        //방향 적용
        transform.eulerAngles = Vector3.up * angle;

        if (dir.magnitude <= 0.8f)        // 플레이어와 가까워지면..
        {
            State = MonsterState.Attack;       // 이 몬스터의 상태를 공격 상태로 바꿈
        }
    }



    void Attack_ToPlayer()   // 플레이어가 상당히 가까이 있어, 플레이어를 공격하는 중
    {
        nma.SetDestination(transform.position);                   // 플레이어와 충분히 가까워졌으니, 현재 위치에 고정시킴
        anim_Mon.SetBool(attackId, true);                      // 공격하는 애니메이션 실행

        Vector3 dir = player.position - transform.position;       // 플레이어와 몬스터의 사이를 구하고
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);  // 플레이어를 향해 바라본다

        

        if (dir.magnitude > 0.8f)         // 플레이어랑 멀어지면...
        {
            State = MonsterState.Moving;                  // 플레이어를 향해 달리는 상태로 바꿈
            anim_Mon.SetBool(attackId, false);         // 공격 애니메이션 끔
            anim_Mon.SetBool(runId, true);                         // 달리는 애니메이션을 바꿈
        }
    }



    void Back_ToOriginalPos()           // 원래 위치로 되돌아가는 함수
    {
        anim_Mon.SetBool(runId, true);       // 달리는 애니메이션을 실행

        Vector3 originalDir = originalPos - transform.position;          // 원래 위치와 현재 몬스터의 위치의 거리를 구한다.

        nma.SetDestination(originalPos);                   // 원래 위치로 간다.
        nma.updateRotation = false;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(originalDir), 20 * Time.deltaTime);
        // 원래 위치를 바라본다.

        if (originalDir.magnitude <= 1.0f)   // 원래 위치로 가까워졌으면...
        {
            State = MonsterState.Idle;       // 기다리는 상태로 바꾼다.
        }
    }


    public void PlayerHp()                   // 공격 애니메이션에 부착됨
    {
        AudioMng.ins.PlayEffect("Sword");   // 검 휘두르는 소리
        mini01_Player.PlayerHp();         // 이 함수가 실행되면, 플레이어의 체력을 깍는다.
    }


    //////////////////////   코루틴 구역

    IEnumerator MonsterCoroutine()         // 플레이어를 발견하기 위해 레이를 쏘는 탐지 코루틴
    {
        while (true)        // 무한 반복
        {
            if (isSee.Equals(false))     // 플레이어를 발견 못 했다면..
            {
                Mon_Sight();             // 몬스터와 플레이어와의 사이에 뭐가 있는지 체크
            }
            yield return delay_01;          // 0.7초 마다
        }
    }
}
