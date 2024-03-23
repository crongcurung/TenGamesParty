using UnityEngine;

public class Mini08_Monster : MonoBehaviour       // 유령 몬스터에 부착됨
{
    public Transform player;       // 확인된 플레이어 오브젝트

    enum MonsterState
    {
        Moving,
        Attack,
    }
    MonsterState State;

    [SerializeField] float speed;        // 이동 속도 변수는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    public Mini08_Spawn mini08_Spawn;          // 스폰 스크립트는 스폰할떄 받아옴..
    public Mini08_Player mini08_Player;
    Vector3 pos;                               // 유령이 이동해야할 최종 위치



	void OnEnable()       // 켜졌을때..
	{
        mini08_Player.followAction += FollwPlayer;           // 소리가 날떄 유령을 그쪽으로 가도록 할려고 액션으로 모아둠
        mini08_Player.GhostText_Fuction(1);                  // 유령 텍스트에 하나 올린다.

        State = MonsterState.Moving;    // 움직이는 상태...
    }

	void OnDisable()      // 꺼졌을떄...
	{
        //AudioMng.ins.LoopEffect(false);    // 흙, 돌 소리 무한루프 종료
        //AudioMng.ins.PlayEffect("Score_Up");    // 퇴마 먹음

        mini08_Player.followAction -= FollwPlayer;           // 유령이 비활성화이니 액션에서 함수를 뻄
        mini08_Player.GhostText_Fuction(-1);                 // 유령 텍스트에 하나를 뺀다.
        mini08_Spawn.InsertQueue_Chost(transform.gameObject);      // 이 유령을 큐에 반납한다.
    }

	void FixedUpdate()
    {
        switch (State)               // 이 몬스터의 상태에 따라 행동이 달라짐
        {
            case MonsterState.Moving:  // 움직이는 상태라면..
                Monster_Move();        // 그냥 돌아다닌다.
                break;
            case MonsterState.Attack:    // 플레이러를 향해 가는 상태라면..
                Monster_Attack();      // 소리가 나는쪽으로 간다.
                break;
        }
    }


    void Monster_Move()    // 그냥 돌아다니는 함수
    {
        Vector3 dir = (pos - transform.position).normalized;           // 현재 위치와 최종 위치 사이의 방향 구하기
        transform.LookAt(pos);                                         // 방향 바라보기
        transform.position += dir * speed * Time.deltaTime;            // 이동

        float distance = Vector3.Distance(transform.position, pos);    // 거리 구하기

        if (distance <= 0.1f)                                          // 거리가 일정 범위 안이라면...(도달)
        {
            pos.x = Random.Range(-20.0f, 20.0f);                         // 다시 위치 랜덤
            pos.z = Random.Range(-20.0f, 20.0f);                         // 다시 위치 랜덤
        }
    }

    void Monster_Attack()    // 소리가 나는 쪽으로 가는 함수
    {
        Vector3 dir = (pos - transform.position).normalized;           // 현재 위치와 최종 위치 사이의 방향 구하기
        transform.LookAt(pos);                                         // 방향 바라보기
        transform.position += dir * speed * Time.deltaTime;            // 이동

        float distance = Vector3.Distance(transform.position, pos);    // 거리 구하기

        if (distance <= 0.1f)                                          // 거리가 일정 범위 안이라면...(도달)
        {
            State = MonsterState.Moving;                                 // 돌아다니는 상태로 바꿈
            pos.x = Random.Range(-20.0f, 20.0f);                         // 다시 위치 랜덤
            pos.z = Random.Range(-20.0f, 20.0f);                         // 다시 위치 랜덤
        }
    }


    public void FollwPlayer()    // 유령이 소리나는 쪽으로 가도록 하는 함수
    {
        pos = player.transform.position;                            // 현재 플레이어 위치 저장
        State = MonsterState.Attack;
    }
}
