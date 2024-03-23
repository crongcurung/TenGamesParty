using System.Collections;
using UnityEngine;

public class Mini06_Bear : MonoBehaviour
{
    enum MonsterState
    {
        Idle,
        Moving,
        AttackWait,
        Attack,
        Back,
    }
    MonsterState State;


    [SerializeField] Transform[] BearSitPos;     // ���� ���� ��ġ

    [SerializeField] SkinnedMeshRenderer BearRender;         // �� ������(�����Ҷ� �ٲܷ���...)
    Color originColor;                             // �� ������ ���׸��� ������ �ٲ� �÷�(���� ��...)

    public int BearHp;     // ���� ü���� �˸��� ����

    public bool isCrazy = false;            // ���� ���·� �˸��� ����
    public bool isBack = false;             // ���ư��� �ִٰ� �˸��� ����

    Vector3 tempSitPos;                // ���� ������, ���� ���� �ڸ��� �޴� �ӽ� ����

    public GameObject bear_Web;        // ���� ���δ� �׹� 

    Coroutine coroutine_CrazyStop;     // ���ֻ��¸� ������ �ڷ�ƾ�� ���� ����
    Coroutine coroutine_Check;         // ���� ���¿��� ���� ��ġ�� ã�� �ڷ�ƾ�� ���� ����

    bool isFirst_Idle = false;         // ��� ���¿� �������� ��, �ߺ��� �����ϱ� ���� ����
    bool isCheck_Idle = false;         // ���� ���¿��� ��� ������ �ߺ� ���� ����

    Vector3 tempVec;         // ���� ��ġ�� ��� ����

    bool isRun_Web = false;            // �׹��� �ɷ��� �� �ߺ� ���� ����
    bool isPlayer_Hit = false;         // �÷��̾ ��Ʈ ��ų �� �ִ��� ���� ����

    BoxCollider boxCol;                // �� �ݶ��̴�

    public GameObject player_Mini06;
    Mini06_Player mini06_Player;         // �÷��̾� ��ũ��Ʈ�� �޴� ����
    Rigidbody player_Rigid;


    WaitForSeconds delay_Crazy;          // ���� �ð�..

    Vector3 col_Size01;           // �� �ݶ��̴� ���� ������
    Vector3 col_Center01;
    Vector3 col_Size02;           // �� �ݶ��̴� ū ������
    Vector3 col_Center02;

    Animator anim;                       // �ִϸ����ʹ� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    WaitForSeconds delay_01;
    WaitForSeconds delay_02;

    int runId;
    int attackId;
    int webId;

    string tag01;
    string tag03;

    string invoke_Text01;

    void Start()
    {
        runId = Animator.StringToHash("posKey");
        attackId = Animator.StringToHash("posAttack");
        webId = Animator.StringToHash("posWeb");

        tag01 = "Monster";
        tag03 = "Spring";

        invoke_Text01 = "Invoke_PlayerHit";

        BearHp = 2;                 // ������ �δ� ������ �����ϵ��� ����!
        
        State = MonsterState.Idle;      // ó������ �ɾ��ִ� ���·�..

        originColor = BearRender.material.color;           // ���� ��Ų ���׸����� ������ �޴´�.

        anim = transform.GetComponent<Animator>();              // �ڽ��� �ִϸ��̼��� �޴´�.
        boxCol = transform.GetComponent<BoxCollider>();         // �ڽ��� �ݶ��̴��� �޴´�.

        mini06_Player = player_Mini06.GetComponent<Mini06_Player>();
        player_Rigid = player_Mini06.GetComponent<Rigidbody>();

        col_Center01 = new Vector3(0.0f, 0.5f, -0.7f);
        col_Size01 = new Vector3(2.0f, 0.9f, 1.7f);
        col_Center02 = new Vector3(0.0f, 0.9f, 0.2f);
        col_Size02 = new Vector3(2.0f, 1.8f, 2.5f);

        delay_01 = new WaitForSeconds(1.0f);
        delay_02 = new WaitForSeconds(3.0f);
        delay_Crazy = new WaitForSeconds(30.0f);
    }

    void FixedUpdate()
    {
        switch (State)
        {
            case MonsterState.Idle:            // �����̱� ��   
                Idle();
                break;
            case MonsterState.AttackWait:          // �ٸ� ������ �޸��� �� üũ ����
                CheckIdle();
                break;
            case MonsterState.Moving:      // �޸��� ����
                Crazy();
                break;
            case MonsterState.Attack:          // �÷��̾ ���� �׹��� ������ ����
                WepIdle();
                break;
            default:                           // ���� ���� �ڸ��� ���� ����
                BackSit();
                break;
        }
    }


    void Idle()
    {
        if (isFirst_Idle.Equals(false))          // �ߺ� ����...
        {
            isFirst_Idle = true;                 // ����ȭ ����
            boxCol.center = col_Center01;           // �ݶ��̴��� ����� �۰��Ѵ�.
            boxCol.size = col_Size01;              

            anim.SetBool(runId, false);          // �ִϸ��̼��� �⺻ ���·�(�ɴ� ��) �Ѵ�.
            anim.SetBool(attackId, false);
        }
    }


    void CheckIdle()
    {
        if (isCheck_Idle.Equals(false))          // �ߺ� ����
        {
            isCheck_Idle = true;                 // ����ȭ
            coroutine_Check = StartCoroutine(Check_Idel());               // üũ �ڷ�ƾ�� �����Ѵ�.
        }
    }


    void Crazy()                      // ���� ���� �Լ�
    {
        anim.SetBool(attackId, false);       // �̵� �ִϸ��̼��� Ų��.
        anim.SetBool(runId, true);

        transform.position = Vector3.MoveTowards(transform.position, tempVec, Time.deltaTime * 10);    // �������� ���� ��ġ�� ����.
        transform.LookAt(tempVec);         // ��ġ�� ���鼭 ����.

        if ((tempVec - transform.position).magnitude < 0.1f)       // ��ǥ ��ġ�� �ٴٸ���...
        {
            State = MonsterState.AttackWait;                         // ���� üũ ���·� ����.
        }
    }


    void WepIdle()
    {
        if (isRun_Web.Equals(false))          // �ߺ� ����
        {
            isRun_Web = true;
            StartCoroutine(Wep_Coroutine());            // �׹� �ڷ�ƾ�� �����Ѵ�.
        }
    }


    void BackSit()            // ���� �ڸ��� ���� �Լ�
    {
        if (isCrazy.Equals(true))                    // ó�� ���⿡ ���� �� true���...
        {
            isCrazy = false;                    // false�� �ٲ㼭 �ٽ� �� ������ �ϱ�...
            isBack = true;                      // �ǵ��ư��ٰ� �˸�

            anim.SetBool(runId, true);       // �̵� �ִϸ��̼��� Ŵ
            anim.SetBool(attackId, false);
            BearRender.material.color = originColor;      // ���׸����� ������ �ٽ� ������� �ٲ۴�.
        }

        transform.position = Vector3.MoveTowards(transform.position, tempSitPos, Time.deltaTime * 2);        // ���� ��ġ�� �̵�
        transform.LookAt(tempSitPos);           // ���� ��ġ�� �ٶ󺸱�

        if ((tempSitPos - transform.position).magnitude < 0.1f)       // ���� ��ġ�� �ٴٸ���...
        {
            isBack = false;               // �ǵ��ư��ٴ� �͵� ������
            BearHp = 2;                   // �ٽ� ü�� ȸ��

            isFirst_Idle = false;          // �ߺ� ������ ��
            State = MonsterState.Idle;     // ���� ���·� �ٲ�
        }
    }


    public void Minus_Bear()                       // ���� ���� ���̸�...
    {
        if (!BearHp.Equals(0))                     // ���� ü���� 0�� �ƴ϶��..
        {
            BearHp--;                              // ���� ü���� ��´�.

            if (BearHp.Equals(0))                  // ��� ü���� 0�� �Ǿ��ٸ�..
            {
                isCrazy = true;                    // ���ֻ��¶�� �˸�
                
                BearRender.material.color = Color.red;        // ���� ���¿��� ���׸����� ������ �������� �ٲ۴�.

                coroutine_CrazyStop = StartCoroutine(CrazyStopCoroutine());    // ���ְ� ���۵Ǹ� ������ �ڷ�ƾ�� �����Ѵ�.
                State = MonsterState.AttackWait;                                // �̵��� ���� ���� üũ ���·� �Ѿ��.
            }
        }
    }


    void Sit_Bear()        // ��ó�� ���� ���� ��ġ ã�� �Լ�
    {
        tempSitPos = BearSitPos[0].position;
        for (int i = 1; i < BearSitPos.Length; i++)        // ��ġ ����Ʈ�� �����鼭
        {
            tempSitPos = (transform.position - tempSitPos).magnitude <= (transform.position - BearSitPos[i].position).magnitude ? tempSitPos : BearSitPos[i].position;
            // ��� ��ġ�� �� ���� ��ġ�� ������� Ȯ���ϸ�, ������ ���� �ִ´�.
        }
    }


    //////////////////  �ڷ�ƾ ����...


    IEnumerator Check_Idel()                  // ���� ��ġ�� �̴� üũ �ڷ�ƾ
    {
        tempVec.x = Random.Range(-19.5f, 19.5f);                         // �ٽ� ��ġ ����
        tempVec.z = Random.Range(-15.5f, 15.5f);                         // �ٽ� ��ġ ����


        anim.SetBool(runId, false);
        anim.SetBool(attackId, true);                  // ������ �Ÿ��� �ִϸ��̼��� Ų��.

        boxCol.center = col_Center02;    // �ݶ��̴��� ����� ũ�� �Ѵ�.
        boxCol.size = col_Size02;

        AudioMng.ins.PlayEffect("BearGrowl");    // �� ����
        yield return delay_01;           // 1�� �� �� ��...

        isCheck_Idle = false;                            // �ߺ� ���� ������ Ǯ��
        State = MonsterState.Moving;                      // �ٽ� ���� ���·� ����.
    }



    IEnumerator CrazyStopCoroutine()                    // ���� ��, ���ָ� ������ �ڷ�ƾ
    {
        yield return delay_Crazy;           // ũ������ ��� �ߵ� ��, 30�ʰ� �Ǹ� ������ �ֱ�...

        Sit_Bear();

        StopCoroutine(coroutine_Check);      // Ȥ�ö� üũ �ڷ�ƾ�� ���� ���̶�� ��������.

        isCheck_Idle = false;                // �ߺ� ���� ������ Ǭ��.
        State = MonsterState.Back;        // ��� ��ġ�� ������� Ȯ�� ������, �� ��ġ�� ���ư��� ���·� �Ѵ�.
    }



    IEnumerator Wep_Coroutine()                // �׹��� �¾��� ��... ��� ����
    {
        if (isCrazy.Equals(true))                   // ó�� ���⿡ ���� �� true���...
        {
            isCrazy = false;                   // false�� �ٲ㼭 �ٽ� �� ������ �ϱ�...
            isBack = true;

            BearRender.material.color = originColor;        // ���׸����� ������ �ٽ� ������� �ٲ۴�.
        }

        StopCoroutine(coroutine_CrazyStop);         // ���ָ� ���� �ڷ�ƾ�� �ߴ�
        StopCoroutine(coroutine_Check);             // üũ �ڷ�ƾ�� �ߴ�

        isCheck_Idle = false;                       // �ߺ� ������ Ǭ��.

        anim.SetBool(runId, false);
        anim.SetBool(attackId, false);
        bear_Web.SetActive(true);                   // �׹� ������Ʈ�� Ȱ��ȭ
        anim.SetBool(webId, true);               // �׹� �ִϸ��̼� Ų��.

        yield return delay_02;      // 3�� �Ŀ�..

        anim.SetBool(webId, false);             // �׹� �ִϸ��̼��� ����. 
        bear_Web.SetActive(false);                 // �׹� ������Ʈ�� ��Ȱ��ȭ

        anim.SetBool(runId, true);              // �̵� �ִϸ��̼��� Ų��.
        anim.SetBool(attackId, false);

        Sit_Bear();

        isRun_Web = false;              // �ߺ� ������ Ǭ��.
        State = MonsterState.Back;   // ���� �ڸ��� ���ư��� ���·�
    }


    void Invoke_PlayerHit()
    {
        isPlayer_Hit = false;                        // �÷��̾ ��Ʈ ��ų �� �ִٰ� �˸�
    }



    ///////////////////  Ʈ���� ����


    void OnTriggerEnter(Collider other)
    {
        if (isCrazy.Equals(true) && isBack.Equals(false))             // ���ֻ����̰�, ���ư��� �ʴ� ��Ȳ�̶��...
        {
            if (other.gameObject.CompareTag(tag01))          // ���� ����� ���..
            {
                other.gameObject.SetActive(false);          // �ݳ� ���Ѿ� ��
                AudioMng.ins.PlayEffect("Score_Up");    // �� �״� �Ҹ�
            }

            if (other.gameObject.layer.Equals(6) && isPlayer_Hit.Equals(false))           // �÷��̾ ���, ��Ʈ��ų �� �ִ� ��Ȳ�̶��...
            {
                mini06_Player.isTouch_Bear = true;

                player_Rigid.velocity = (other.transform.position - transform.position).normalized * 50.0f; // ��ǳ ȿ��..

                if (mini06_Player.isAlive.Equals(true))            // �÷��̾ ���� ��� �ִٸ�...
                {                    
                    isPlayer_Hit = true;                    // �÷��̾ ��Ʈ��ų �� ���ٰ� �˸�
                    mini06_Player.MinusHp_Mini06();    // ü���� ��� ��Ʈ �̹��� ���

                    Invoke(invoke_Text01, 1.0f);
                }
            }

            if (other.gameObject.CompareTag(tag03))           // �׹��� �¾��� ���...
            {
                other.gameObject.SetActive(false);          // �ݳ� ���Ѿ� ��
                AudioMng.ins.PlayEffect("BearWeb");    // �� ����
                State = MonsterState.Attack;               // �׹��� �°� ����ϴ� ���·� ����.
            }

            if (other.gameObject.layer.Equals(4))       // ����뿡 ������...          Water
            {
                if (other.GetComponent<Mini06_Box>().hp_Int < 3)    // ���� �� ������� ü���� 1 �̻��̶��...
                {
                    other.GetComponent<Mini06_Box>().Hit_Fuction();      // ü���� ��´�.
                }
            }
        }
    }
}
