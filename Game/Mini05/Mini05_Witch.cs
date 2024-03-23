using UnityEngine;

public class Mini05_Witch : MonoBehaviour
{
    public Transform player;       // Ȯ�ε� �÷��̾� ������Ʈ

    enum MonsterState_Mini05         // ���¿� ���� �� ĳ������ �ൿ�� �ٸ�
    {
        Moving01,
        Moving02,
        Attack,
        Wait,
    }
    MonsterState_Mini05 State_Mini05;   // ���� ���¸� �޴� ����

    public int thisInt = 0;                  // �� �ü��� ������ ���� ��ȣ(�� �������� ��)
    public int thisStartInt = 0;             // �� �ü��� ������ ���� ��ȣ(ó������ �� ����... ������ ��)       
    public Mini05_Spawn mini05_Spawn;        // ���� ��ũ��Ʈ

    Vector3 tempSpot;
    int tempInt;

    Vector3 dir;                   // ���Ͱ� �ٶ󺸴� ����

    GameObject fire_Ball;                 // ���̾ ������Ʈ
    Vector3 origin_PosF;

    Animator anim;                       // �ִϸ����ʹ� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    [SerializeField] float speed;        // �̵� �ӵ� ������ �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    Vector3 awakeSpot;                   // �� �� ��ġ ����
    Vector3 spot01;                      // �� �� ��ġ ����
    Vector3 spot07;                      // 50m ��ġ ����

    Vector3 originPos;                      // ó�� ��ġ ���� ����

    int attackId;                 // �����ϴ� �ִϸ����͸� �޴� ����


	void Awake()
	{
        originPos = transform.position;

        awakeSpot = new Vector3(150.0f, 0.5f, thisStartInt);        // �� �� ��ġ
        spot01 = new Vector3(128.0f, 0.5f, thisStartInt);           // �� �� ��ġ
        float heightFloat = Random.Range(20.0f, 50.0f);          // �������� ���� ����
        spot07 = new Vector3(50.0f, heightFloat, thisInt);       //  20 ~ 50 ���� ����

        anim = GetComponent<Animator>();
        attackId = Animator.StringToHash("isAttack");        // �����ϴ� �ִϸ����� ����
        tempSpot = awakeSpot;

        fire_Ball = transform.GetChild(3).gameObject;
        origin_PosF = fire_Ball.transform.localPosition;
    }


    void OnDisable()
    {
        mini05_Spawn.list_Witch.Add(transform.gameObject);        // ���� ���� ����Ʈ�� �ݳ�
        transform.position = originPos;                           // ó�� ��ġ�� �޴´�.
        State_Mini05 = MonsterState_Mini05.Moving01;

        fire_Ball.SetActive(false);
        fire_Ball.transform.localPosition = origin_PosF;

        transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));

        tempInt = 0;
        tempSpot = awakeSpot;
        anim.SetBool(attackId, false);     // ���� �ִϸ��̼� ����
    }


    void Update()
    {
        if (State_Mini05.Equals(MonsterState_Mini05.Moving01))
        {
            MovingToPos();                         // �ڽ��� �ش�� �Ա� �ڷ� ���� �Լ�
        }
        else if (State_Mini05.Equals(MonsterState_Mini05.Attack))  // ����
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


    void MovingToPos()           // �� �ڷ� ���� �Լ�
    {
        dir = tempSpot - transform.position;                // ���� ����
        transform.LookAt(tempSpot);      // ���� �������� �ٶ󺸷�...
        transform.position += dir.normalized * speed * Time.deltaTime;   // ��ǥ�� �̵�

        if (dir.magnitude <= 0.1f)           // ��ǥ�� �ٴ޾Ҵٸ�....
        {
            tempInt++;
            SwichFuction();
        }
    }



    void SwichFuction()
    {
        switch (tempInt)
        {
            case 0:                      // �Ա� �ڷ� ���� ��
                tempSpot = awakeSpot;
                break;
            case 1:                      // �Ա� ������ ���� ��
                tempSpot = spot01;
                break;
            case 2:                      // 50m�� ���� ��
                tempSpot = spot07;
                break;
            case 3:          // ����
                State_Mini05 = MonsterState_Mini05.Attack;
                Attack();
                break;
        }
    }


    void Attack()                       // ���� �ִϸ��̼�..............................................................
    {
        anim.SetBool(attackId, true);     // ���� �ִϸ��̼� ����
        transform.LookAt(player);
        fire_Ball.SetActive(true);         // ���̾ Ȱ��ȭ
    }

    public void Fire_Ball_01()                // ���� �ִϸ��̼ǿ� ����(�ʹ�), ���̾ ����
    {
        State_Mini05 = MonsterState_Mini05.Wait;      // ���̾ �߻�
    }


    void Fire_Ball_03()
    {
        fire_Ball.transform.position = Vector3.MoveTowards(fire_Ball.transform.position, player.position, Time.deltaTime * 10);
        // ���̾�� �÷��̾����� ���Ѵ�.

        if ((player.position - fire_Ball.transform.position).magnitude < 10.0f)   // �÷��̾�� ����� ����...
        {
            mini05_Spawn.End_Game();      // ����

            State_Mini05 = MonsterState_Mini05.Moving02;
        }
    }
}
