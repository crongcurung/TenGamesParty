using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Mini03_Monster : MonoBehaviour   // 미니03의 몬스터에 부착됨
{
    public Transform patrol;       // 현재 스테이지에서 맵을 돌아다닐 수 있는, 패트롤 위치 정보를 담고 있는 부모(스폰에서 넘겨줌)
    public Mini03_Player mini03_Player;

    bool rayBool = false;

    Transform lightTrans;          // 몬스터가 들고 있는 손전등 라이트
    Collider lightCol;             // 몬스터가 들고 있는 손전등의 메시 콜라이더
    Vector3 originScale;           // 손전등의 원래 크기
    Vector3 wallScale;             // 손전등이 벽에 닿았을 때 작아지는 크기

    Renderer render;               // 손전등의 렌더러
    Color tempColor;               // 손전등의 컬러를 담은 임시 변수

    int layerInt;                  // 

    float m_angle = 0.0f;          // 레이 쏠때 각도??

    bool isSee = false;            // 플레이어를 봤다고 알리는 변수
    bool patrolBool = false;       // 정찰을 할 수 있냐고 묻는 변수

    Vector3 velocity = Vector3.zero; //???

    Vector3 randPos;
    int runId_Mini03;                    // 달리는 애니메이터를 받는 변수
    LayerMask P_layerMask_Mini03;

    WaitForSeconds delay_01;
    WaitForSeconds delay_02;

    NavMeshAgent nma;

    Animator monsterAnim;         // 몬스터의 애니메이터를 받는 변수

    enum MonsterState
    {
        Moving,
        Attack,
    }
    MonsterState State;

    public Transform player;       // 확인된 플레이어 오브젝트
    [SerializeField] protected float m_distance = 4;    //

    string tag01;

    void Awake()
    {
        nma = gameObject.GetComponent<NavMeshAgent>();       // 이 몬스터를 네브메쉬를 넘김
        monsterAnim = gameObject.GetComponent<Animator>();   // 애니메이터를 변수로 넘긴다.
        runId_Mini03 = Animator.StringToHash("isRun");              // 달리는 애니메이터 저장
        
        State = MonsterState.Moving;              // 처음에는 달리는 상태(패트롤)

        tag01 = "Player";

        m_angle = 90.0f;                               // 플레이어를 볼떄 각도
        m_distance = 2.0f;

        layerInt = 1 << LayerMask.NameToLayer("WALL");           // 레이어 검색에 벽을 넘긴다.
        lightTrans = transform.GetChild(2).transform;              // 손전등을 받아온다.
        lightCol = lightTrans.GetComponent<Collider>();            // 손전등의 (메쉬)콜라이더를 받아온다.
        originScale = lightTrans.localScale;                       // 손전등의 원래 크기를 받아온다.
        wallScale = new Vector3(1.4f, 0.7f, 1.4f);                 // 벽에 닿으면 손전등 작아지는 크기

        render = transform.GetChild(2).GetComponent<Renderer>();   // 손전등의 렌더러
        tempColor = render.material.color;                         // 원래 손전등의 색깔

        delay_01 = new WaitForSeconds(0.05f);
        delay_02 = new WaitForSeconds(0.05f);

        P_layerMask_Mini03 = LayerMask.GetMask("Player");           // 레이어를 플레이어만 필요함;
    }

	void OnEnable()                    // 몬스터가 켜질때...
	{
        patrolBool = false;            // 순찰 체크하도록 false로
        isSee = false;                 // 봤다는 거 초기
        
        State = MonsterState.Moving;              // 처음에는 정찰(돌아다니는 걸로)
        monsterAnim.SetBool(runId_Mini03, true);         // 서 있는 애니메이션 실행
        render.material.color = tempColor;        // 손전등의 색깔을 원래대로 바꾼다.

        StartCoroutine(RayCoroutine());           // 레이 코루틴 다시 시작(벽 탐지)
        StartCoroutine(MonsterCoroutine());       // 레이 코루틴 다시 시작(플레이어 탐지)
    }


	void FixedUpdate()
    {
        switch (State)               // 이 몬스터의 상태에 따라 행동이 달라짐
        {
            case MonsterState.Moving:  // 움직이는 상태라면..(패트롤)
                Patrol();              // 정찰 시작
                break;
            case MonsterState.Attack:  // 플레이어를 발견해서 쫓아가는 상태
                RunToPlayer();         // 플레이어로 향함
                break;
        }
    }



    void Patrol()               // 정찰
    {
        if (patrolBool.Equals(false))                   // 정찰 구역이 어딘지 체크함
        {
            patrolBool = true;                     // 체크는 한 번만하기
            lightCol.enabled = false;              // 손전등의 메쉬 콜라이더를 끈다.

            int randInt = Random.Range(0, patrol.childCount);      // 다음 패트롤 위치를 랜덤으로 뽑는다.
            randPos = patrol.GetChild(randInt).position;

            nma.speed = 2.0f;               // 정찰 스피드로 낮춘다.
            nma.SetDestination(randPos);    // 다음 패트롤 위치로 이동!
            transform.position = Vector3.SmoothDamp(transform.position, nma.nextPosition, ref velocity, 0.1f);   // 일단 이걸로.....
        }

        if ((randPos - transform.position).magnitude < 1.0f)         // 정찰 구역에 가까이 가면
        {
            patrolBool = false;                     // 체크 한 번 더 하기!!
        }
    }


    void Mon_Sight()          // 몬스터 시야 함수
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_distance, P_layerMask_Mini03);       // 몬스터 근처에 플레이어가 있는지 확인

        
        if (t_cols.Length > 0)     // 근처에 플레이어가 있다면...
        {
            Transform t_tfPlayer = t_cols[0].transform;     // 일단 변수에 플레이어를 담고

            Vector3 t_direction = (t_tfPlayer.position - transform.position).normalized;        // 플레이어와 몬스터 사이의 방향
            float t_angle = Vector3.Angle(t_direction, transform.forward);                      // 각도를 표현

            if (t_angle < m_angle * 0.5f)        // 설정된 각도 안에 플레이어가 있다면...
            {
                if (Physics.Raycast(transform.position + transform.up * 0.5f, t_direction, out RaycastHit t_hit, m_distance))   // 레이를 쏜다.(앞으로)
                {
                    if (t_hit.transform.CompareTag(tag01))          // 플레이어를 발견했다면...
                    {
                        player = t_hit.transform;           // 플레이어를 담고

                        Light_Red();                                   // 손전등의 색깔을 바꾼다.(빨강)

                        isSee = true;                                  // 플레이어를 발견했다고 알림
                        State = MonsterState.Attack;                   // 플레이어를 향해 간다고 상태 변경
                    }
                }
            }
        }
    }

    void RunToPlayer()                         // 플레이어를 향해 가는 함수...
    {
        if ((player.position - transform.position).magnitude < 1.0f)     // 플레이어와 몬스터와의 거리가 가깝다면...
        {
            if (mini03_Player.isInBin.Equals(true))              // 플레이어 스크립트 안에서 쓰레기통에 들어갔는지 체크한다.
            {
                patrolBool = false;            // 순찰 체크하도록 false로
                isSee = false;                         // 플레이어를 못 봤다고 알림
                State = MonsterState.Moving;           // 정찰 상태로 바꿈
                transform.LookAt(player);    // 잠시(??????) 플레이어를 향해 본다.

                render.material.color = tempColor;     // 손전등의 색깔을 원래대로 바꾼다.
                return;
            }
        }

        nma.speed = 4.0f;        // 플레이어를 향해 갈떄, 스피드를 올린다.
        nma.SetDestination(player.position);         // 플레이어를 향해 간다.
        transform.position = Vector3.SmoothDamp(transform.position, nma.nextPosition, ref velocity, 0.1f);   // 회전 관련??
    }

    void Light_Red()         // 손전등을 빨간색으로 바꾸는 함수
    {
        render.material.color = Color.red;              // 손전등의 메테리얼을 빨강색으로 바꾼다.
        Color playerColor = render.material.color;      
        playerColor.a = 0.58f;                          // 알파 값도 바꾼다...
        render.material.color = playerColor;

        AudioMng.ins.PlayEffect("Whistle");

        lightCol.enabled = true;                        // 손전등의 메쉬 콜라이더를 킨다.
    }



	////////////////////////////// 코루틴 구역.....


	IEnumerator MonsterCoroutine()         // 플레이어를 발견하기 위해 레이를 쏘는 탐지 코루틴
    {
        while (true)        // 무한 반복
        {
            if (isSee.Equals(false))     // 플레이어를 발견 못 했다면..
            {
                Mon_Sight();             // 몬스터와 플레이어와의 사이에 뭐가 있는지 체크
            }
            yield return delay_02;          // 0.3초 마다
        }
    }


    IEnumerator RayCoroutine()                            // 몬스터 앞에 벽이 있는지 체크하는 코루틴
    {
        while (true)
        {
            if (rayBool.Equals(false))      // 벽에 닿지 않았었다면...(현재는 아직 손전등 스케일이 큼)
            {
                if (Physics.Raycast(this.transform.position + transform.up * 0.6f, transform.forward, 1.4f, layerInt))    // 몬스터 앞에 벽이 있다면..
                {
                    lightTrans.localScale = wallScale;  // 손전등의 크기를 줄임
                    rayBool = true;                     // 벽에 닿았다고 알림
                }
            }
            else      // 이미 벽에 닿았다면...(현재는 손전등 스케일이 작음)
            {
                if (!Physics.Raycast(this.transform.position + transform.up * 0.6f, transform.forward, 1.4f, layerInt))   // 몬스터 앞에 벽이 없다면..
                {
                    lightTrans.localScale = originScale;  // 손전등의 크기를 원래대로 바꿈
                    rayBool = false;                      // 벽에 안 닿았다고 알림
                }
            }
            yield return delay_01;
        }
    }


    ///////////////////////////////  트리거 구역



}
