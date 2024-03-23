using UnityEngine;

public class Mini05_Witch : MonoBehaviour
{
    public Transform player;       // 확인된 플레이어 오브젝트

    enum MonsterState_Mini05         // 상태에 따라 이 캐릭터의 행동이 다름
    {
        Moving01,
        Moving02,
        Attack,
        Wait,
    }
    MonsterState_Mini05 State_Mini05;   // 몬스터 상태를 받는 변수

    public int thisInt = 0;                  // 이 궁수가 가지는 고유 번호(이 방향으로 감)
    public int thisStartInt = 0;             // 이 궁수가 가지는 고유 번호(처음에는 이 방향... 문으로 감)       
    public Mini05_Spawn mini05_Spawn;        // 스폰 스크립트

    Vector3 tempSpot;
    int tempInt;

    Vector3 dir;                   // 몬스터가 바라보는 방향

    GameObject fire_Ball;                 // 파이어볼 오브젝트
    Vector3 origin_PosF;

    Animator anim;                       // 애니메이터는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    [SerializeField] float speed;        // 이동 속도 변수는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    Vector3 awakeSpot;                   // 문 뒤 위치 변수
    Vector3 spot01;                      // 문 앞 위치 변수
    Vector3 spot07;                      // 50m 위치 변수

    Vector3 originPos;                      // 처음 위치 저장 변수

    int attackId;                 // 공격하는 애니메이터를 받는 변수


	void Awake()
	{
        originPos = transform.position;

        awakeSpot = new Vector3(150.0f, 0.5f, thisStartInt);        // 문 뒤 위치
        spot01 = new Vector3(128.0f, 0.5f, thisStartInt);           // 문 앞 위치
        float heightFloat = Random.Range(20.0f, 50.0f);          // 랜덤으로 높이 지정
        spot07 = new Vector3(50.0f, heightFloat, thisInt);       //  20 ~ 50 높이 설정

        anim = GetComponent<Animator>();
        attackId = Animator.StringToHash("isAttack");        // 공격하는 애니메이터 저장
        tempSpot = awakeSpot;

        fire_Ball = transform.GetChild(3).gameObject;
        origin_PosF = fire_Ball.transform.localPosition;
    }


    void OnDisable()
    {
        mini05_Spawn.list_Witch.Add(transform.gameObject);        // 스폰 마녀 리스트에 반납
        transform.position = originPos;                           // 처음 위치를 받는다.
        State_Mini05 = MonsterState_Mini05.Moving01;

        fire_Ball.SetActive(false);
        fire_Ball.transform.localPosition = origin_PosF;

        transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));

        tempInt = 0;
        tempSpot = awakeSpot;
        anim.SetBool(attackId, false);     // 공격 애니메이션 실행
    }


    void Update()
    {
        if (State_Mini05.Equals(MonsterState_Mini05.Moving01))
        {
            MovingToPos();                         // 자신이 해당된 입구 뒤로 가는 함수
        }
        else if (State_Mini05.Equals(MonsterState_Mini05.Attack))  // 공격
        {

        }
        else if (State_Mini05.Equals(MonsterState_Mini05.Wait))
        {
            Fire_Ball_03();
        }
        else
        {

        }
    }


    void MovingToPos()           // 문 뒤로 가는 함수
    {
        dir = tempSpot - transform.position;                // 방향 설정
        transform.LookAt(tempSpot);      // 가는 방향으로 바라보록...
        transform.position += dir.normalized * speed * Time.deltaTime;   // 목표로 이동

        if (dir.magnitude <= 0.1f)           // 목표에 다달았다면....
        {
            tempInt++;
            SwichFuction();
        }
    }



    void SwichFuction()
    {
        switch (tempInt)
        {
            case 0:                      // 입구 뒤로 가는 곳
                tempSpot = awakeSpot;
                break;
            case 1:                      // 입구 앞으로 가는 곳
                tempSpot = spot01;
                break;
            case 2:                      // 50m로 가는 곳
                tempSpot = spot07;
                break;
            case 3:          // 공격
                State_Mini05 = MonsterState_Mini05.Attack;
                Attack();
                break;
        }
    }


    void Attack()                       // 공격 애니메이션..............................................................
    {
        anim.SetBool(attackId, true);     // 공격 애니메이션 실행
        transform.LookAt(player);
        fire_Ball.SetActive(true);         // 파이어볼 활성화
    }

    public void Fire_Ball_01()                // 공격 애니메이션에 부착(초반), 파이어볼 생성
    {
        State_Mini05 = MonsterState_Mini05.Wait;      // 파이어볼 발사
    }


    void Fire_Ball_03()
    {
        fire_Ball.transform.position = Vector3.MoveTowards(fire_Ball.transform.position, player.position, Time.deltaTime * 10);
        // 파이어볼을 플레이어한테 향한다.

        if ((player.position - fire_Ball.transform.position).magnitude < 10.0f)   // 플레이어와 가까워 지면...
        {
            mini05_Spawn.End_Game();      // 끝냄

            State_Mini05 = MonsterState_Mini05.Moving02;
        }
    }
}
