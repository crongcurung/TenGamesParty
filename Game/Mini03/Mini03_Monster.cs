using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Mini03_Monster : MonoBehaviour   // �̴�03�� ���Ϳ� ������
{
    public Transform patrol;       // ���� ������������ ���� ���ƴٴ� �� �ִ�, ��Ʈ�� ��ġ ������ ��� �ִ� �θ�(�������� �Ѱ���)
    public Mini03_Player mini03_Player;

    bool rayBool = false;

    Transform lightTrans;          // ���Ͱ� ��� �ִ� ������ ����Ʈ
    Collider lightCol;             // ���Ͱ� ��� �ִ� �������� �޽� �ݶ��̴�
    Vector3 originScale;           // �������� ���� ũ��
    Vector3 wallScale;             // �������� ���� ����� �� �۾����� ũ��

    Renderer render;               // �������� ������
    Color tempColor;               // �������� �÷��� ���� �ӽ� ����

    int layerInt;                  // 

    float m_angle = 0.0f;          // ���� �� ����??

    bool isSee = false;            // �÷��̾ �ôٰ� �˸��� ����
    bool patrolBool = false;       // ������ �� �� �ֳİ� ���� ����

    Vector3 velocity = Vector3.zero; //???

    Vector3 randPos;
    int runId_Mini03;                    // �޸��� �ִϸ����͸� �޴� ����
    LayerMask P_layerMask_Mini03;

    WaitForSeconds delay_01;
    WaitForSeconds delay_02;

    NavMeshAgent nma;

    Animator monsterAnim;         // ������ �ִϸ����͸� �޴� ����

    enum MonsterState
    {
        Moving,
        Attack,
    }
    MonsterState State;

    public Transform player;       // Ȯ�ε� �÷��̾� ������Ʈ
    [SerializeField] protected float m_distance = 4;    //

    string tag01;

    void Awake()
    {
        nma = gameObject.GetComponent<NavMeshAgent>();       // �� ���͸� �׺�޽��� �ѱ�
        monsterAnim = gameObject.GetComponent<Animator>();   // �ִϸ����͸� ������ �ѱ��.
        runId_Mini03 = Animator.StringToHash("isRun");              // �޸��� �ִϸ����� ����
        
        State = MonsterState.Moving;              // ó������ �޸��� ����(��Ʈ��)

        tag01 = "Player";

        m_angle = 90.0f;                               // �÷��̾ ���� ����
        m_distance = 2.0f;

        layerInt = 1 << LayerMask.NameToLayer("WALL");           // ���̾� �˻��� ���� �ѱ��.
        lightTrans = transform.GetChild(2).transform;              // �������� �޾ƿ´�.
        lightCol = lightTrans.GetComponent<Collider>();            // �������� (�޽�)�ݶ��̴��� �޾ƿ´�.
        originScale = lightTrans.localScale;                       // �������� ���� ũ�⸦ �޾ƿ´�.
        wallScale = new Vector3(1.4f, 0.7f, 1.4f);                 // ���� ������ ������ �۾����� ũ��

        render = transform.GetChild(2).GetComponent<Renderer>();   // �������� ������
        tempColor = render.material.color;                         // ���� �������� ����

        delay_01 = new WaitForSeconds(0.05f);
        delay_02 = new WaitForSeconds(0.05f);

        P_layerMask_Mini03 = LayerMask.GetMask("Player");           // ���̾ �÷��̾ �ʿ���;
    }

	void OnEnable()                    // ���Ͱ� ������...
	{
        patrolBool = false;            // ���� üũ�ϵ��� false��
        isSee = false;                 // �ôٴ� �� �ʱ�
        
        State = MonsterState.Moving;              // ó������ ����(���ƴٴϴ� �ɷ�)
        monsterAnim.SetBool(runId_Mini03, true);         // �� �ִ� �ִϸ��̼� ����
        render.material.color = tempColor;        // �������� ������ ������� �ٲ۴�.

        StartCoroutine(RayCoroutine());           // ���� �ڷ�ƾ �ٽ� ����(�� Ž��)
        StartCoroutine(MonsterCoroutine());       // ���� �ڷ�ƾ �ٽ� ����(�÷��̾� Ž��)
    }


	void FixedUpdate()
    {
        switch (State)               // �� ������ ���¿� ���� �ൿ�� �޶���
        {
            case MonsterState.Moving:  // �����̴� ���¶��..(��Ʈ��)
                Patrol();              // ���� ����
                break;
            case MonsterState.Attack:  // �÷��̾ �߰��ؼ� �Ѿư��� ����
                RunToPlayer();         // �÷��̾�� ����
                break;
        }
    }



    void Patrol()               // ����
    {
        if (patrolBool.Equals(false))                   // ���� ������ ����� üũ��
        {
            patrolBool = true;                     // üũ�� �� �����ϱ�
            lightCol.enabled = false;              // �������� �޽� �ݶ��̴��� ����.

            int randInt = Random.Range(0, patrol.childCount);      // ���� ��Ʈ�� ��ġ�� �������� �̴´�.
            randPos = patrol.GetChild(randInt).position;

            nma.speed = 2.0f;               // ���� ���ǵ�� �����.
            nma.SetDestination(randPos);    // ���� ��Ʈ�� ��ġ�� �̵�!
            transform.position = Vector3.SmoothDamp(transform.position, nma.nextPosition, ref velocity, 0.1f);   // �ϴ� �̰ɷ�.....
        }

        if ((randPos - transform.position).magnitude < 1.0f)         // ���� ������ ������ ����
        {
            patrolBool = false;                     // üũ �� �� �� �ϱ�!!
        }
    }


    void Mon_Sight()          // ���� �þ� �Լ�
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_distance, P_layerMask_Mini03);       // ���� ��ó�� �÷��̾ �ִ��� Ȯ��

        
        if (t_cols.Length > 0)     // ��ó�� �÷��̾ �ִٸ�...
        {
            Transform t_tfPlayer = t_cols[0].transform;     // �ϴ� ������ �÷��̾ ���

            Vector3 t_direction = (t_tfPlayer.position - transform.position).normalized;        // �÷��̾�� ���� ������ ����
            float t_angle = Vector3.Angle(t_direction, transform.forward);                      // ������ ǥ��

            if (t_angle < m_angle * 0.5f)        // ������ ���� �ȿ� �÷��̾ �ִٸ�...
            {
                if (Physics.Raycast(transform.position + transform.up * 0.5f, t_direction, out RaycastHit t_hit, m_distance))   // ���̸� ���.(������)
                {
                    if (t_hit.transform.CompareTag(tag01))          // �÷��̾ �߰��ߴٸ�...
                    {
                        player = t_hit.transform;           // �÷��̾ ���

                        Light_Red();                                   // �������� ������ �ٲ۴�.(����)

                        isSee = true;                                  // �÷��̾ �߰��ߴٰ� �˸�
                        State = MonsterState.Attack;                   // �÷��̾ ���� ���ٰ� ���� ����
                    }
                }
            }
        }
    }

    void RunToPlayer()                         // �÷��̾ ���� ���� �Լ�...
    {
        if ((player.position - transform.position).magnitude < 1.0f)     // �÷��̾�� ���Ϳ��� �Ÿ��� �����ٸ�...
        {
            if (mini03_Player.isInBin.Equals(true))              // �÷��̾� ��ũ��Ʈ �ȿ��� �������뿡 ������ üũ�Ѵ�.
            {
                patrolBool = false;            // ���� üũ�ϵ��� false��
                isSee = false;                         // �÷��̾ �� �ôٰ� �˸�
                State = MonsterState.Moving;           // ���� ���·� �ٲ�
                transform.LookAt(player);    // ���(??????) �÷��̾ ���� ����.

                render.material.color = tempColor;     // �������� ������ ������� �ٲ۴�.
                return;
            }
        }

        nma.speed = 4.0f;        // �÷��̾ ���� ����, ���ǵ带 �ø���.
        nma.SetDestination(player.position);         // �÷��̾ ���� ����.
        transform.position = Vector3.SmoothDamp(transform.position, nma.nextPosition, ref velocity, 0.1f);   // ȸ�� ����??
    }

    void Light_Red()         // �������� ���������� �ٲٴ� �Լ�
    {
        render.material.color = Color.red;              // �������� ���׸����� ���������� �ٲ۴�.
        Color playerColor = render.material.color;      
        playerColor.a = 0.58f;                          // ���� ���� �ٲ۴�...
        render.material.color = playerColor;

        AudioMng.ins.PlayEffect("Whistle");

        lightCol.enabled = true;                        // �������� �޽� �ݶ��̴��� Ų��.
    }



	////////////////////////////// �ڷ�ƾ ����.....


	IEnumerator MonsterCoroutine()         // �÷��̾ �߰��ϱ� ���� ���̸� ��� Ž�� �ڷ�ƾ
    {
        while (true)        // ���� �ݺ�
        {
            if (isSee.Equals(false))     // �÷��̾ �߰� �� �ߴٸ�..
            {
                Mon_Sight();             // ���Ϳ� �÷��̾���� ���̿� ���� �ִ��� üũ
            }
            yield return delay_02;          // 0.3�� ����
        }
    }


    IEnumerator RayCoroutine()                            // ���� �տ� ���� �ִ��� üũ�ϴ� �ڷ�ƾ
    {
        while (true)
        {
            if (rayBool.Equals(false))      // ���� ���� �ʾҾ��ٸ�...(����� ���� ������ �������� ŭ)
            {
                if (Physics.Raycast(this.transform.position + transform.up * 0.6f, transform.forward, 1.4f, layerInt))    // ���� �տ� ���� �ִٸ�..
                {
                    lightTrans.localScale = wallScale;  // �������� ũ�⸦ ����
                    rayBool = true;                     // ���� ��Ҵٰ� �˸�
                }
            }
            else      // �̹� ���� ��Ҵٸ�...(����� ������ �������� ����)
            {
                if (!Physics.Raycast(this.transform.position + transform.up * 0.6f, transform.forward, 1.4f, layerInt))   // ���� �տ� ���� ���ٸ�..
                {
                    lightTrans.localScale = originScale;  // �������� ũ�⸦ ������� �ٲ�
                    rayBool = false;                      // ���� �� ��Ҵٰ� �˸�
                }
            }
            yield return delay_01;
        }
    }


    ///////////////////////////////  Ʈ���� ����



}
