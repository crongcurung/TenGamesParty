using System.Collections;
using UnityEngine;

public class Mini05_Archer : MonoBehaviour
{
    public Transform player;       // 확인된 플레이어 오브젝트

    public int thisInt = 0;                  // 이 궁수가 가지는 고유 번호(이 방향으로 감)
    public int thisStartInt = 0;             // 이 궁수가 가지는 고유 번호(처음에는 이 방향... 문으로 감)       
    public Mini05_Spawn mini05_Spawn;        // 스폰 스크립트

    Vector3 dir;                   // 몬스터가 바라보는 방향

    Vector3 tempSpot;              // 다음 위치로 가는 위치 변수
    int tempInt;                   // 다음 위치로 가는 숫자 변수

    Transform Cannon;                       // 플레이어한테 공격할때 사용되는 목표
    public Transform FailPos;                      // 플레이어한테 공격할때 실패되는 목표

    Transform Target;                       // 화살의 목표를 받는 변수

    float firingAngle = 45.0f;       // 물리법칙 변수?
    float gravity = 9.8f;            // 중력?

    Transform Projectile;            // 발사체
    Transform myTransform;                  // 처음 위치를 저장하는 변수(화살)

    int currentMeter;                       // 현재 몇 미터에 있는지 받는 변수
    Vector3 tempRot;                        // 애니메이션 때문에 궁수가 쏠때, 약간 회전이 들어가야 한다...

    bool isRun = false;               // 기다리는 코루틴이 실행되고 있는지 묻는 변수(true면 기다리고 있다라는 뜻)

    [SerializeField] float speed;        // 이동 속도 변수는 플레이어와 몬스터가 공통으로 지닐 가능성 높음


    enum MonsterState_Mini05         // 상태에 따라 이 캐릭터의 행동이 다름
    {
        Moving01,
        Attack,
        Wait,
    }
    MonsterState_Mini05 State_Mini05;   // 몬스터 상태를 받는 변수

    Vector3 awakeSpot;                   // 문 뒤 위치 변수
    Vector3 spot01;                      // 문 앞 위치 변수
    Vector3 spot02;                      // 100m 위치 변수
    Vector3 spot03;                      // 90m 위치 변수
    Vector3 spot04;                      // 80m 위치 변수
    Vector3 spot05;                      // 70m 위치 변수
    Vector3 spot06;                      // 60m 위치 변수
    Vector3 spot07;                      // 50m 위치 변수

    Vector3 originPos;                      // 처음 위치 저장 변수

    Animator anim;                       // 애니메이터는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    WaitForSeconds delay_01;
    WaitForSeconds delay_02;

    int runId;                    // 달리는 애니메이터를 받는 변수
    int attackId;                 // 공격하는 애니메이터를 받는 변수



	void Awake()
	{
        anim = GetComponent<Animator>();                     // 이 스크립트를 가진 궁수의 애니메이터를 가져옴
        attackId = Animator.StringToHash("isAttack");        // 공격하는 애니메이터 저장
        runId = Animator.StringToHash("isRun");              // 달리는 애니메이터 저장

        originPos = transform.position;  // 현재 위치를 저장
        myTransform = transform;         // 현재 위치 저장(화살 용)
        Cannon = player;                 // 발사 목표 위치를 저장(스폰 스크립트에서 처리)

        awakeSpot = new Vector3(150.0f, 0, thisStartInt);        // 문 뒤 위치
        spot01 = new Vector3(128.0f, 0, thisStartInt);           // 문 앞 위치
        spot02 = new Vector3(100.0f, 0, thisInt);                // 100m 위치
        spot03 = new Vector3(90.0f, 0, thisInt);       // 첫번째 사격... (성공 확률 10%)
        spot04 = new Vector3(80.0f, 0, thisInt);       // 두번째 사격... (성공 확률 30%)
        spot05 = new Vector3(70.0f, 0, thisInt);       // 세번째 사격... (성공 확률 50%)
        spot06 = new Vector3(60.0f, 0, thisInt);       // 네번쨰 사격... (성공 확률 80%)
        spot07 = new Vector3(50.0f, 0, thisInt);       // 다섯번쨰 사격... (성공 확률 100%)

        Projectile = transform.GetChild(2).transform;     // 발사체 오브젝트를 받는다.

        anim.SetBool(runId, true);          // 공격 애니메이션 실행

        Rot_Setting();                      // 회전을 여기서 왜하지???
        tempSpot = awakeSpot;               // 처음 이동하는 곳을 넘긴다.

        delay_01 = new WaitForSeconds(1.0f);   // 화살쏘고 기다리는 시간
        delay_02 = new WaitForSeconds(5.0f);
    }


	void OnEnable()                     // 궁수가 활성화 됐을 때...
	{
        anim.SetBool(attackId, false);     // 공격 애니메이션 끔
        anim.SetBool(runId, true);         // 달리는 애니메이션 실행
    }

	void OnDisable()                     // 궁수가 비활성화 됐을 때...
    {
        mini05_Spawn.list_Archer.Add(transform.gameObject);       // 비활성화 되었을 떄 이 궁수를 다시 궁수 스폰 리스트에 넣는다..
        transform.position = originPos;                           // 이 궁수의 위치를 처음 위치로(한번 나온 몬스터는 스폰 지점이 항상 같다) 넘긴다.
        State_Mini05 = MonsterState_Mini05.Moving01;              // 궁수 상태 초기화를 한다.

        Projectile.gameObject.SetActive(false);                   // 발사체를 끈다.
        Projectile.position = myTransform.localPosition;          // 발사채를 원래 위치로 옮긴다.

        tempInt = 0;               // 다음 위치 변수를 초기화 한다.
        tempSpot = awakeSpot;      // 다음 위치를 초기화 한다.
    }

    void Update()
    {
        if (State_Mini05.Equals(MonsterState_Mini05.Moving01))   // 이동
        {
            MovingToPos();                         // 자신이 해당된 입구 뒤로 가는 함수
        }
        else if (State_Mini05.Equals(MonsterState_Mini05.Attack))  // 공격
        {
            Attack();
        }
        else        // 기다림
        {
            Waithing();
        }
    }


    void MovingToPos()           // 문 뒤로 가는 함수
    {
        dir = tempSpot - transform.position;                // 방향 설정
        transform.LookAt(tempSpot);      // 가는 방향으로 바라보록...
        transform.position += dir.normalized * speed * Time.deltaTime;   // 목표로 이동

        if (dir.magnitude <= 0.1f)           // 목표에 다달았다면....
        {
            tempInt++;           // 다음 위치로 가는 숫자 증가
            SwichFuction();      // 다음 위치 변수를 담는 함수
        }
    }

    void Rot_Setting()           // 궁수의 화살 쏠 떄, 회전 위치(궁수의 특성상.......)
    {
        int rotInt;
        switch (thisInt)         // 이 궁수의 위치 숫자에 따라(스폰에서 정해 줌) 회전 숫자를 받는다.
        {
            case -145:           // 이 몬스터의 Z값(좌, 우) 위치라면...
                rotInt = 50;     // 회전 조정값 50으로 함
                break;
            case -125:
                rotInt = 45;     // 회전 조정값 45으로 함
                break;
            case -105:
                rotInt = 40;     // 회전 조정값 40으로 함
                break;
            case -85:
                rotInt = 35;     // 회전 조정값 35으로 함
                break;
            case -65:
                rotInt = 25;     // 회전 조정값 25으로 함
                break;
            case -45:
                rotInt = 15;     // 회전 조정값 15으로 함
                break;
            case -25:
                rotInt = 0;      // 회전 조정값 0으로 함
                break;
            case -5:
                rotInt = -15;     // 회전 조정값 -15으로 함
                break;
            case 15:
                rotInt = -25;     // 회전 조정값 -25으로 함
                break;
            case 35:
                rotInt = -35;     // 회전 조정값 -35으로 함
                break;
            case 55:
                rotInt = -45;     // 회전 조정값 -45으로 함
                break;
            case 75:
                rotInt = -55;     // 회전 조정값 -55으로 함
                break;
            case 95:
                rotInt = -60;     // 회전 조정값 -60으로 함
                break;
            case 115:
                rotInt = -70;     // 회전 조정값 -70으로 함
                break;
            default:
                rotInt = -75;     // 회전 조정값 -75으로 함
                break;
        }

        tempRot = new Vector3(0, rotInt, 0);     // 이 궁수의 회전을 숫자에 따라 담는다.(이거는 쭉 간다.)
    }


    void SwichFuction()                  // 다음 위치 변수를 담는 함수
    {
        switch (tempInt)                 // 다음 위치 숫자 변수에 따라...
        {
            case 0:                      // 입구 뒤로 가는 곳
                tempSpot = awakeSpot;    // 0일때 위치로
                break;
            case 1:                      // 입구 앞으로 가는 곳
                tempSpot = spot01;    // 1일때 위치로
                break;
            case 2:                      // 100m로 가는 곳
                tempSpot = spot02;    // 2일때 위치로
                break;
            case 3:                      // 90m로 가는 곳      여기서부터 쏜다.
                tempSpot = spot03;    // 3일때 위치로
                currentMeter = 90;       // 이 궁수가 현재 90m에 있다고 알린다.
                break;
            case 4:                      // 80m로 가는 곳
                tempSpot = spot04;    // 4일때 위치로
                State_Mini05 = MonsterState_Mini05.Attack;     // 공격 상태로 바로 바꿈
                currentMeter = 80;       // 이 궁수가 현재 80m에 있다고 알린다.
                break;
            case 5:                      // 70m로 가는 곳
                tempSpot = spot05;    // 5일때 위치로
                State_Mini05 = MonsterState_Mini05.Attack;     // 공격 상태로 바로 바꿈
                currentMeter = 70;       // 이 궁수가 현재 70m에 있다고 알린다.
                break;
            case 6:                      // 60m로 가는 곳
                tempSpot = spot06;    // 6일때 위치로
                State_Mini05 = MonsterState_Mini05.Attack;     // 공격 상태로 바로 바꿈
                currentMeter = 60;       // 이 궁수가 현재 60m에 있다고 알린다.
                break;
            case 7:                      // 50m로 가는 곳
                tempSpot = spot07;    // 7일때 위치로
                State_Mini05 = MonsterState_Mini05.Attack;     // 공격 상태로 바로 바꿈
                currentMeter = 50;       // 이 궁수가 현재 50m에 있다고 알린다.
                break;
            case 8:          // 공격
                State_Mini05 = MonsterState_Mini05.Attack;     // 공격 상태로 바로 바꿈
                Attack();
                break;
            case 9:         // 잠시 쉼
                State_Mini05 = MonsterState_Mini05.Wait;      // 기다림 상태로 바로 바꿈
                break;
        }
    }


    void Attack()                       // 공격 애니메이션..............................................................
    {
        anim.SetBool(attackId, true);     // 공격 애니메이션 실행
        anim.SetBool(runId, false);          // 공격 애니메이션 실행

        transform.rotation = Quaternion.Euler(tempRot);      // 공격 할떄는 미리 준비된 회전으로 바꾼다.
    }


    public void AfterAttack()             // 공격 이후에 실행될 함수(공격 애니메이션 마지막에 부착)
    {
        anim.SetBool(attackId, false);    // 공격 애니메이션 끔
        anim.SetBool(runId, false);    // 공격 애니메이션 끔

        State_Mini05 = MonsterState_Mini05.Wait;        // 잠시 쉼
    }

    public void Shooting()                // 공격 중반에 실행될 함수(공격 애니메이션 중간에 부착)
    {
        StartCoroutine(SimulateProjectile());     // 화살을 발사하는 코루틴 실행
    }

    

    void RandomTarget(int currentMeter)           // 쏠떄 어느 위치에서 어느 곳(랜덤)으로 쏠지 정하는 함수
    {
        int randInt = Random.Range(0, 10);       // 성공 확률 구하기
        int FailPercent;                     // 실패 확률

        switch (currentMeter)                // 현재 미터를 파악해서
        {
            case 90:                         // 현재 미터가 90m이라면...
                FailPercent = 9;             // 화살 실패 확률을 90%로 바꾼다.
                break;
            case 80:                         // 현재 미터가 80m이라면...
                FailPercent = 7;             // 화살 실패 확률을 70%로 바꾼다.
                break;
            case 70:                         // 현재 미터가 70m이라면...
                FailPercent = 5;             // 화살 실패 확률을 50%로 바꾼다.
                break;
            case 60:                         // 현재 미터가 60m이라면...
                FailPercent = 2;             // 화살 실패 확률을 20%로 바꾼다.
                break;
            default:                         // 현재 미터가 50m이라면...
                FailPercent = 0;             // 화살 실패 확률을 0%로 바꾼다.
                break;
        }

        if (randInt < FailPercent)   // 실패 확률 (앞)
        {
            Target = FailPos;        // 타겟을 실패 구역으로 설정
        }
        else               // 성공 확률 10%
        {
            Target = Cannon;         // 타켓을 플레이어로 설정
        }
    }

    void Waithing()              // 잠시 기다리는 함수
    {
        if (isRun.Equals(false))        // 코루틴이 실행되고 있지 않다면...
        {
            StartCoroutine(WaitCoroutine());         // 기다리는 코루틴 실행
        }
    }




    //////////////////////////////       코루틴 구역...


    IEnumerator WaitCoroutine()                      // 기다리는 코루틴 실행(조금 기다렸다가 이동 혹은 공격 명령 실행)
    {
        isRun = true;                             // 코루틴이 실행되고 있다고 알림
        yield return delay_02;    // 잠시 기다린 다음에

        anim.SetBool(runId, true);    // 공격 애니메이션 끔

        Projectile.gameObject.SetActive(false);   // 화살을 비활성화 시킴
        
        if (!tempInt.Equals(8))     // 공격 상태가 아니라면...
        {
            State_Mini05 = MonsterState_Mini05.Moving01;    // 이동 상태로 바꿈
        }
        else                        // 공격 상태라면...
        {
            State_Mini05 = MonsterState_Mini05.Attack;      // 공격 상태로 바꿈      
        }
        isRun = false;             // 코루틴이 끝났다고 알려줌
    }



    IEnumerator SimulateProjectile()               // 화살을 발사하는 코루틴
    {
        RandomTarget(currentMeter);             // 어느 위치에서 쏠지 알아보는 함수

        yield return delay_01;    // 잠시 기다린 후...
        Projectile.gameObject.SetActive(true);    // 화살 오브젝트 활성화...


        Projectile.position = transform.position;                                  // 화살 위치를 자신에게 가져온다
        float target_Distance = Vector3.Distance(Projectile.position, Target.position);                        // 현재 위치와 목표 위치 사이의 거리를 구한다.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);  // 포물선을 구하도록 짠다.
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        float flightDuration = target_Distance / Vx;
        Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);                  // 회전??
        float elapse_time = 0;

        while (elapse_time < flightDuration)        // 화살이 날라간다.
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
}
