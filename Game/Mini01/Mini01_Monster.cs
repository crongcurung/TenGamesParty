using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Mini01_Monster : MonoBehaviour     // ���Ϳ� ������
{
    Vector3 originalPos;      // ���Ͱ� ó�� ������ ��ġ�� ���� ����

    public Transform player;       // Ȯ�ε� �÷��̾� ������Ʈ
    public Mini01_Player mini01_Player;           // �÷��̾��� ��ũ��Ʈ�� ����

    bool isSee = false;

    Coroutine rayCoroutine;

    Animator anim_Mon;         // ������ �ִϸ����͸� �޴� ����

    float m_distance = 4;    //

    WaitForSeconds delay_01;

    int runId;                    // �޸��� �ִϸ����͸� �޴� ����
    int attackId;                 // �����ϴ� �ִϸ����͸� �޴� ����

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
        anim_Mon = gameObject.GetComponent<Animator>();   // �ִϸ����͸� ������ �ѱ��.
        runId = Animator.StringToHash("isRun");              // �޸��� �ִϸ����� ����
        attackId = Animator.StringToHash("isAttack");        // �����ϴ� �ִϸ����� ����

        nma = gameObject.GetComponent<NavMeshAgent>();       // �� ���͸� �׺�޽��� �ѱ�


        delay_01 = new WaitForSeconds(0.7f);    // 0.7�� ���� ����

        P_layerMask = LayerMask.GetMask("Player");           // ���̾ �÷��̾ �ʿ���;
    }


	void OnEnable()    // �� ���Ͱ� ���� ��..
	{
        State = MonsterState.Idle;       // ó������ ��ٸ��� ���·�...
        rayCoroutine = StartCoroutine(MonsterCoroutine());       // ���̸� ���� �ڷ�ƾ ����

        originalPos = transform.position;            // ���� ���� ��ġ�� ����
    }

	void OnDisable()   // �� ���Ͱ� ���� ��...
	{
        StopCoroutine(rayCoroutine);     // ���̸� ��� �ڷ�ƾ �ߴܽ�Ŵ
    }


    void FixedUpdate()
    {
        switch (State)               // �� ������ ���¿� ���� �ൿ�� �޶���
        {
            case MonsterState.Idle:    // ��ٸ��� ���¶��..
                Idle_FindCheck();
                break;
            case MonsterState.Moving:  // �����̴� ���¶��..
                Run_ToPlayer();
                break;
            case MonsterState.Attack:  // �����ϴ� ���¶��..
                Attack_ToPlayer();
                break;
            case MonsterState.Back:    // ���� �ڸ��� �ǵ��ư��� ���¶��..
                Back_ToOriginalPos();
                break;
        }
	}

    void Mon_Sight()         // ���Ϳ� �÷��̾���� ���̿� ���� �ִ��� üũ
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_distance, P_layerMask);       // ���� ������ �÷��̾��� �ݶ��̴� Ž��

        if (t_cols.Length > 0)       // Ž���� �ݶ��̴��� �ִٸ�...
        {
            Transform t_tfPlayer = t_cols[0].transform;   // Ž���� �ݶ��̴��� �÷��̾ �и��ϴ� ��ġ ������ �ѱ��.

            Vector3 t_direction = (t_tfPlayer.position - transform.position).normalized;     // ���Ϳ� �÷��̾���� ������ ���Ѵ�.

            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, t_direction, out RaycastHit t_hit, m_distance))
                // �� ���Ϳ��� �÷��̾� ������ ���̸� ���. �ٵ� �߰��� ���� ����(��) �� �ִٸ� �÷��̸� �� ã�Ҵٰ� �Ѵ�...
            {
                if (t_hit.collider.gameObject.layer.Equals(6))    // ������ �÷��̾�� ���̿� �ƹ� �͵� ���ٸ�..
                {
                    anim_Mon.SetBool(runId, true);                         // �޸��� �ִϸ��̼��� �ٲ�
                    State = MonsterState.Moving;               // �� ���� ���¸� �÷��̾ ���� �޸��� ������ �ٲ�
                }
            }
        }
    }


    void Idle_FindCheck()  // ���� ���ڸ��� ���� ����(�÷��̾ ��ĵ ���� ������ ���⸦ ��ٸ��� ��)
    {
        if (isSee.Equals(true))                        // �÷��̾ ������..(�̰Ŵ� �ǵ��ƿԴٴ� ����... �ᱹ : �Ʒ� �ִϸ��̼� �ѹ��� �����ϱ� ���� ��..)
        {
            isSee = false;                             // �ѹ��� �����ϱ� ���� �ٷ� �ٲ�
            anim_Mon.SetBool(runId, false);         // �� �ִ� �ִϸ��̼� ����
        }
    }


    void Run_ToPlayer()  // ���� �÷��̾ ��ĵ ���� �ȿ� �־�, �÷��̾ �Ѿ� ���� ��
    {
        isSee = true;      // �÷��̾ �ôٰ� �˸�
        Vector3 dir = player.position - transform.position;        // �� ���Ϳ� �÷��̾���� �Ÿ��� ���Ѵ�.

        if ((originalPos - transform.position).magnitude > 20)           // �� �Ÿ��� 20���� ���ٸ�...
        {
            State = MonsterState.Back;                            // ���͸� ���� ��ġ�� �ǵ��ư��� �Ѵ�.
            anim_Mon.SetBool(runId, true);                         // �޸��� �ִϸ��̼��� �ٲ�
            return;                                               // �Ʒ� ���� �� �ϰ� �Ѵ�.
        }

        nma.SetDestination(player.position);                // �÷��̾��� ��ġ�� ���� �̵�
        nma.updateRotation = false;
        nma.acceleration = 10.0f;

        Vector2 forward = new Vector2(transform.position.z, transform.position.x);          // �Ʒ��� ȸ�� ����?
        Vector2 steeringTarget = new Vector2(nma.steeringTarget.z, nma.steeringTarget.x);

        //������ ���� ��, ���Լ��� ���� ���Ѵ�.
        Vector2 dir2 = steeringTarget - forward;
        float angle = Mathf.Atan2(dir2.y, dir2.x) * Mathf.Rad2Deg;

        //���� ����
        transform.eulerAngles = Vector3.up * angle;

        if (dir.magnitude <= 0.8f)        // �÷��̾�� ���������..
        {
            State = MonsterState.Attack;       // �� ������ ���¸� ���� ���·� �ٲ�
        }
    }



    void Attack_ToPlayer()   // �÷��̾ ����� ������ �־�, �÷��̾ �����ϴ� ��
    {
        nma.SetDestination(transform.position);                   // �÷��̾�� ����� �����������, ���� ��ġ�� ������Ŵ
        anim_Mon.SetBool(attackId, true);                      // �����ϴ� �ִϸ��̼� ����

        Vector3 dir = player.position - transform.position;       // �÷��̾�� ������ ���̸� ���ϰ�
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);  // �÷��̾ ���� �ٶ󺻴�

        

        if (dir.magnitude > 0.8f)         // �÷��̾�� �־�����...
        {
            State = MonsterState.Moving;                  // �÷��̾ ���� �޸��� ���·� �ٲ�
            anim_Mon.SetBool(attackId, false);         // ���� �ִϸ��̼� ��
            anim_Mon.SetBool(runId, true);                         // �޸��� �ִϸ��̼��� �ٲ�
        }
    }



    void Back_ToOriginalPos()           // ���� ��ġ�� �ǵ��ư��� �Լ�
    {
        anim_Mon.SetBool(runId, true);       // �޸��� �ִϸ��̼��� ����

        Vector3 originalDir = originalPos - transform.position;          // ���� ��ġ�� ���� ������ ��ġ�� �Ÿ��� ���Ѵ�.

        nma.SetDestination(originalPos);                   // ���� ��ġ�� ����.
        nma.updateRotation = false;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(originalDir), 20 * Time.deltaTime);
        // ���� ��ġ�� �ٶ󺻴�.

        if (originalDir.magnitude <= 1.0f)   // ���� ��ġ�� �����������...
        {
            State = MonsterState.Idle;       // ��ٸ��� ���·� �ٲ۴�.
        }
    }


    public void PlayerHp()                   // ���� �ִϸ��̼ǿ� ������
    {
        AudioMng.ins.PlayEffect("Sword");   // �� �ֵθ��� �Ҹ�
        mini01_Player.PlayerHp();         // �� �Լ��� ����Ǹ�, �÷��̾��� ü���� ��´�.
    }


    //////////////////////   �ڷ�ƾ ����

    IEnumerator MonsterCoroutine()         // �÷��̾ �߰��ϱ� ���� ���̸� ��� Ž�� �ڷ�ƾ
    {
        while (true)        // ���� �ݺ�
        {
            if (isSee.Equals(false))     // �÷��̾ �߰� �� �ߴٸ�..
            {
                Mon_Sight();             // ���Ϳ� �÷��̾���� ���̿� ���� �ִ��� üũ
            }
            yield return delay_01;          // 0.7�� ����
        }
    }
}
