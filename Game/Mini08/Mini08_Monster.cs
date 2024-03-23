using UnityEngine;

public class Mini08_Monster : MonoBehaviour       // ���� ���Ϳ� ������
{
    public Transform player;       // Ȯ�ε� �÷��̾� ������Ʈ

    enum MonsterState
    {
        Moving,
        Attack,
    }
    MonsterState State;

    [SerializeField] float speed;        // �̵� �ӵ� ������ �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    public Mini08_Spawn mini08_Spawn;          // ���� ��ũ��Ʈ�� �����ҋ� �޾ƿ�..
    public Mini08_Player mini08_Player;
    Vector3 pos;                               // ������ �̵��ؾ��� ���� ��ġ



	void OnEnable()       // ��������..
	{
        mini08_Player.followAction += FollwPlayer;           // �Ҹ��� ���� ������ �������� ������ �ҷ��� �׼����� ��Ƶ�
        mini08_Player.GhostText_Fuction(1);                  // ���� �ؽ�Ʈ�� �ϳ� �ø���.

        State = MonsterState.Moving;    // �����̴� ����...
    }

	void OnDisable()      // ��������...
	{
        //AudioMng.ins.LoopEffect(false);    // ��, �� �Ҹ� ���ѷ��� ����
        //AudioMng.ins.PlayEffect("Score_Up");    // �� ����

        mini08_Player.followAction -= FollwPlayer;           // ������ ��Ȱ��ȭ�̴� �׼ǿ��� �Լ��� �M
        mini08_Player.GhostText_Fuction(-1);                 // ���� �ؽ�Ʈ�� �ϳ��� ����.
        mini08_Spawn.InsertQueue_Chost(transform.gameObject);      // �� ������ ť�� �ݳ��Ѵ�.
    }

	void FixedUpdate()
    {
        switch (State)               // �� ������ ���¿� ���� �ൿ�� �޶���
        {
            case MonsterState.Moving:  // �����̴� ���¶��..
                Monster_Move();        // �׳� ���ƴٴѴ�.
                break;
            case MonsterState.Attack:    // �÷��̷��� ���� ���� ���¶��..
                Monster_Attack();      // �Ҹ��� ���������� ����.
                break;
        }
    }


    void Monster_Move()    // �׳� ���ƴٴϴ� �Լ�
    {
        Vector3 dir = (pos - transform.position).normalized;           // ���� ��ġ�� ���� ��ġ ������ ���� ���ϱ�
        transform.LookAt(pos);                                         // ���� �ٶ󺸱�
        transform.position += dir * speed * Time.deltaTime;            // �̵�

        float distance = Vector3.Distance(transform.position, pos);    // �Ÿ� ���ϱ�

        if (distance <= 0.1f)                                          // �Ÿ��� ���� ���� ���̶��...(����)
        {
            pos.x = Random.Range(-20.0f, 20.0f);                         // �ٽ� ��ġ ����
            pos.z = Random.Range(-20.0f, 20.0f);                         // �ٽ� ��ġ ����
        }
    }

    void Monster_Attack()    // �Ҹ��� ���� ������ ���� �Լ�
    {
        Vector3 dir = (pos - transform.position).normalized;           // ���� ��ġ�� ���� ��ġ ������ ���� ���ϱ�
        transform.LookAt(pos);                                         // ���� �ٶ󺸱�
        transform.position += dir * speed * Time.deltaTime;            // �̵�

        float distance = Vector3.Distance(transform.position, pos);    // �Ÿ� ���ϱ�

        if (distance <= 0.1f)                                          // �Ÿ��� ���� ���� ���̶��...(����)
        {
            State = MonsterState.Moving;                                 // ���ƴٴϴ� ���·� �ٲ�
            pos.x = Random.Range(-20.0f, 20.0f);                         // �ٽ� ��ġ ����
            pos.z = Random.Range(-20.0f, 20.0f);                         // �ٽ� ��ġ ����
        }
    }


    public void FollwPlayer()    // ������ �Ҹ����� ������ ������ �ϴ� �Լ�
    {
        pos = player.transform.position;                            // ���� �÷��̾� ��ġ ����
        State = MonsterState.Attack;
    }
}
