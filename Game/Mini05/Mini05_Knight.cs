using UnityEngine;

public class Mini05_Knight : MonoBehaviour         // 검사 프리팹에 부착됨
{
    public int thisInt = 0;                  // 이 궁수가 가지는 고유 번호(이 방향으로 감)
    public int thisStartInt = 0;             // 이 궁수가 가지는 고유 번호(처음에는 이 방향... 문으로 감)       
    public Mini05_Spawn mini05_Spawn;        // 스폰 스크립트

    Vector3 endPos;                      // 50m 위치 변수
    bool isAttack = false;               // 공격 애니메이션 최적화 변수
    int posInt = 0;

    Vector3 awakeSpot;                   // 문 뒤 위치 변수
    Vector3 spot01;                      // 문 앞 위치 변수
    Vector3 spot02;                      // 100m 위치 변수

    Vector3 originPos;                      // 처음 위치 저장 변수

    Animator anim;                       // 애니메이터는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    [SerializeField] protected float speed;        // 이동 속도 변수는 플레이어와 몬스터가 공통으로 지닐 가능성 높음
    int attackId;                 // 공격하는 애니메이터를 받는 변수

	void Awake()
	{
        originPos = transform.position;   // 검사 처음 위치를 담는다.(할당된 숫자로 하는거라 계속 이 위치로...)

        awakeSpot = new Vector3(150.0f, 0.5f, thisStartInt);        // 문 뒤 위치
        spot01 = new Vector3(128.0f, 0.5f, thisStartInt);           // 문 앞 위치
        spot02 = new Vector3(100.0f, 0.5f, thisInt);                // 100m 위치
        endPos = new Vector3(0, 0.5f, thisInt);                     // 공격 위치 

        anim = GetComponent<Animator>();
        attackId = Animator.StringToHash("isAttack");        // 공격하는 애니메이터 저장
    }


	void OnDisable()           // 비활성화 할떄..
	{
        mini05_Spawn.list_Knight.Add(transform.gameObject);     // 스폰 검사 리스트에 다시 담는다.
        transform.position = originPos;                         // 스폰 처음 위치로 바꾼다.

        posInt = 0;        // 다음 위치로 가는 숫자 변수
        isAttack = false;  // 공격 애니메이션 최적화 변수
    }



	void Update()
    {
        switch (posInt)              // 다음 위치 가는 숫자 변수에 따라..
        {
            case 0:                  // 0이라면..
                KnightToPos(awakeSpot);    // 성문 뒤로 가라
                break;
            case 1:                  // 1이라면..
                KnightToPos(spot01);       // 성문 앞으로 가라
                break;
            case 2:                  // 2이라면..
                KnightToPos(spot02);       // 100m로 가라
                break;
            case 3:                  // 3이라면..
                KnightToPos(endPos);       // 0m로 가라
                break;
            default:                  // 4이라면..
                Attack();                  // 공격하라
                break;
        }
    }

    

    void KnightToPos(Vector3 tempPos)     // 특정 위치로 가는 함수
    {
        Vector3 dir = tempPos - transform.position;                // 방향 설정
        transform.LookAt(tempPos);      // 가는 방향으로 바라보록...

        transform.position += dir.normalized * speed * Time.deltaTime * 2.0f;   // 목표로 이동

        if (dir.magnitude <= 0.5f)           // 목표에 다달았다면....
        {
            posInt++;
        }
    }


    void Attack()           // 공격하는 함수
    {
        if (isAttack.Equals(false))                // 최적화..
        {
            isAttack = true;                       // 최적화
            anim.SetBool(attackId, true);          // 공격 애니메이션 실행
        }
    }

    public void Attack_Minus()
    {
        mini05_Spawn.Hp_WallMinus();
    }
}
