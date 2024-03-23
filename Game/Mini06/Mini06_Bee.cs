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

    GameObject nearObject;                // ���� ���� ���� �����

    Vector3 backPos;                      // �����ϱ� ���� ���

    Vector3 tempVec;         // ���� ��ġ�� ��� ����

    bool isRun_Wait = false;                 // ���� ��ǥ ���� �� ����, �ǵ��� �� �ı��� ���� ���ϵ��� ���� ����
    bool isWaitRun = false;          // ���� �� �ڷ�ƾ�� ����ǰ� �ִ��� ���� ����(�ѹ��� �����ҷ���...)

    public Mini06_Spawn mini06_Spawn;      // �ݳ� ��Ű�� ���� ���� ��ũ��Ʈ
    public Mini06_Player mini06_Player;    // �÷��̾� ��ũ��Ʈ
    public Mini06_Bear mini06_Bear;        // �� ��ũ��Ʈ
    Mini06_Box mini06_Box;                 // �ڽ��� �������� ���̿� �������� ��ũ��Ʈ �������� ����

    Animator anim;                       // �ִϸ����ʹ� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����


    WaitForSeconds delay_01;
    WaitForSeconds delay_02;
    WaitForSeconds delay_03;

    int runId;
    int attackId;

    string tag03;

    Vector3 worldDown;             // �� �� �Ʒ� ����

    void Awake()
	{
		anim = transform.GetComponent<Animator>();

        runId = Animator.StringToHash("posKey");
        attackId = Animator.StringToHash("attackKey");

        tag03 = "Cushion";

        worldDown = new Vector3(0, -1, 0);               // �� �� �Ʒ� ����

        delay_01 = new WaitForSeconds(0.2f);
        delay_02 = new WaitForSeconds(2.0f);
        delay_03 = new WaitForSeconds(3.0f);
    }

	void OnEnable()
	{
        State = MonsterState.Moving;

        tempVec.x = Random.Range(-19.5f, 19.5f);                         // �ٽ� ��ġ ����
        tempVec.y = 2.0f;
        tempVec.z = Random.Range(-15.5f, 15.5f);                         // �ٽ� ��ġ ����

        StartCoroutine(RayCoroutine());        // �Ʒ��� ���̸� ��
    }


	void OnDisable()
    {
        mini06_Spawn.list_Bee.Add(transform.gameObject);        // ����Ʈ�� �ٽ� �ִ´�..
        isRun_Wait = false;
        isWaitRun = false;           // ���� �� �ڷ�ƾ�� �����ٰ� �˸�
    }

	void FixedUpdate()
	{
        switch (State)
        {
            case MonsterState.Moving:         // �����̴� ����
                RunToRandPos();
                break;
            case MonsterState.AttackWait:     // ���� �� ��� ����
                if (isWaitRun.Equals(false))         // �ڷ�ƾ�� ����ǰ� ���� �ʴٸ�... �ѹ��� �����ϱ� ���ؼ�...
                {
                    StartCoroutine(Before_Attack_Coroutine());       // ���� ������ 
                }

                transform.LookAt(nearObject.transform);         // ��ǥ�� ���°Ŵ� ��� �ؾ� �Ѵ�.
                break;
            case MonsterState.Attack:         // ���� �ϴ� ����
                Attack_Bee();
                break;
            default:                          // ���� ���� ��ġ�� ���ư��� ����
                Back_Bee();
                break;
        }
    }



    void RunToRandPos()             // ���� ��ġ�� ���� �Լ�
    {
        anim.SetBool(runId, true);        // �̵� �ִϸ��̼� ����

        transform.position = Vector3.MoveTowards(transform.position, tempVec, Time.deltaTime * 4);    // �������� ���� ��ġ�� ����.
        transform.LookAt(tempVec);         // ��ġ�� ���鼭 ����.

        if ((tempVec - transform.position).magnitude < 0.1f)       // ��ǥ ��ġ�� �ٴٸ���...
        {
            tempVec.x = Random.Range(-19.5f, 19.5f);                         // �ٽ� ��ġ ����
            tempVec.z = Random.Range(-15.5f, 15.5f);                         // �ٽ� ��ġ ����
        }
    }


    void Attack_Bee()                // ��ǥ�� �ٰ����� �Լ�
    {
        transform.position = Vector3.MoveTowards(transform.position, nearObject.transform.position, Time.deltaTime * 6);    // �ش� ��������� ����!
        transform.LookAt(nearObject.transform);     // �ش� ������� ��� ����.

        if ((nearObject.transform.position - transform.position).magnitude < 0.1f)    // �ش� ����뿡 �ٴٸ���...
        {
            if (nearObject.layer.Equals(4))      // ��ǥ�� ������̶��...        Water
            {
                if (mini06_Box.hp_Int < 3)        // �ٽ� Ȯ��
                {
                    mini06_Box.Hit_Fuction();      // ü���� ��´�.
                }
            }
            else if (nearObject.layer.Equals(6))     // ��ǥ�� �÷��̾���...
            {
                
                mini06_Player.MinusHp_Mini06();      // ü���� ��� ��Ʈ �̹��� ���
            }
            else if (nearObject.CompareTag(tag03))        // ��ǥ�� ���̶��...
            {
                mini06_Bear.Minus_Bear();

            }
            State = MonsterState.Back;     // �ǵ��ư���
        }
    }



    void Back_Bee()           // �ٽ� �ǵ��ư��� �Լ�
    {
        anim.SetBool(runId, true);               // �̵� �ִϸ��̼��� Ų��.
        anim.SetBool(attackId, false);           // ���� �ִϸ��̼��� ����.

        transform.position = Vector3.MoveTowards(transform.position, backPos, Time.deltaTime * 4);     // ���� �� ������ ��ġ�� �ǵ��ư���.
        transform.LookAt(backPos);      // �ش� ��ġ�� ���� ����.

        if ((backPos - transform.position).magnitude < 0.1f)     // �ǵ��ư��� ��ġ�� �ٴٸ���...
        {
            StartCoroutine(Wait_AfterAttack());     // �� �ʰ� ���� ���ϵ��� ���� �ڷ�ƾ ����(��� ��� ���� ����...)
            State = MonsterState.Moving;              // ��� ���·� ��
        }
    }


    IEnumerator RayCoroutine()        // �Ʒ��� ���̸� ��
    {
        while (true)
        {
            if (isRun_Wait.Equals(false))
            {
                
                RaycastHit hitInfo5;
                if (Physics.Raycast(this.transform.position, worldDown, out hitInfo5, 7.0f))   // ���� ���� �Ʒ��� ������
                {
                    if (hitInfo5.collider.gameObject.layer.Equals(4))    // ����뿡 ������...           Water
                    {
                        nearObject = hitInfo5.transform.gameObject;           // ���� ������Ʈ�� �ѱ��.
                        mini06_Box = hitInfo5.transform.GetComponent<Mini06_Box>();
                        if (mini06_Box.hp_Int < 3)    // ������� ü���� �ִٸ�..
                        {
                            State = MonsterState.AttackWait;                  // ������ ��Ȳ���� ����.
                        }
                    }
                    else if (hitInfo5.collider.gameObject.layer.Equals(6))    // �÷��̾ ������...   Player
                    {
                        nearObject = hitInfo5.transform.gameObject;       // ���� ������Ʈ�� �ѱ��.

                        if (mini06_Player.isAlive.Equals(true))      // �÷��̾ ��� �ִٸ�..
                        {
                            State = MonsterState.AttackWait;                  // ������ ��Ȳ���� ����.
                        }
                    }
                    else if (hitInfo5.transform.CompareTag(tag03))      // ���̶��...
                    {
                        nearObject = hitInfo5.transform.gameObject;

                        if (mini06_Bear.isCrazy.Equals(false) || mini06_Bear.isBack.Equals(false))   // ���ֳ� ���ư��� ���°� �ƴ϶��..
                        {
                            State = MonsterState.AttackWait;                  // ������ ��Ȳ���� ����.
                        }

                    }
                }
            }
            yield return delay_01;       // 0.2�� ����
        }
    }




    IEnumerator Before_Attack_Coroutine()         // ���� �ϱ� �� ��� ������ ��ٸ��� �ϴ� �ڷ�ƾ
    {
        isWaitRun = true;           // ���� �� �ڷ�ƾ�� ����ǰ� �ִٰ� �˸�
        isRun_Wait = true;          // ������ �����ϰ� �ִٰ� �˸�(�� ���� �ٸ� �� ���� ���ϵ��� ���� ����...)

        anim.SetBool(runId, false);            // �̵� �ִϸ��̼��� ����
        anim.SetBool(attackId, true);          // ���� �ִϸ��̼��� Ų��. �ִϸ��̼� ���� ���ұ� ������ ���� �ִϸ��̼� ���������� ����

        backPos = transform.position;             // ���� ��ġ�� �ǵ��ư� ��ġ�� �ѱ��.
        yield return delay_03;

        State = MonsterState.Attack;              // �� �� ��ٸ� ���� ������ �Ѵ�.

        isWaitRun = false;           // ���� �� �ڷ�ƾ�� �����ٰ� �˸�
    }

    
    IEnumerator Wait_AfterAttack()           // ���� �� ���� �Ÿ� �������� �ʰ� Ʈ���Ÿ� ���� �ڷ�ƾ
    {
        yield return delay_02;
        isRun_Wait = false;        // ���� ������ �����ϴٰ� �˸�
    }
}
