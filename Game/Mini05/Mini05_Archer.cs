using System.Collections;
using UnityEngine;

public class Mini05_Archer : MonoBehaviour
{
    public Transform player;       // Ȯ�ε� �÷��̾� ������Ʈ

    public int thisInt = 0;                  // �� �ü��� ������ ���� ��ȣ(�� �������� ��)
    public int thisStartInt = 0;             // �� �ü��� ������ ���� ��ȣ(ó������ �� ����... ������ ��)       
    public Mini05_Spawn mini05_Spawn;        // ���� ��ũ��Ʈ

    Vector3 dir;                   // ���Ͱ� �ٶ󺸴� ����

    Vector3 tempSpot;              // ���� ��ġ�� ���� ��ġ ����
    int tempInt;                   // ���� ��ġ�� ���� ���� ����

    Transform Cannon;                       // �÷��̾����� �����Ҷ� ���Ǵ� ��ǥ
    public Transform FailPos;                      // �÷��̾����� �����Ҷ� ���еǴ� ��ǥ

    Transform Target;                       // ȭ���� ��ǥ�� �޴� ����

    float firingAngle = 45.0f;       // ������Ģ ����?
    float gravity = 9.8f;            // �߷�?

    Transform Projectile;            // �߻�ü
    Transform myTransform;                  // ó�� ��ġ�� �����ϴ� ����(ȭ��)

    int currentMeter;                       // ���� �� ���Ϳ� �ִ��� �޴� ����
    Vector3 tempRot;                        // �ִϸ��̼� ������ �ü��� ��, �ణ ȸ���� ���� �Ѵ�...

    bool isRun = false;               // ��ٸ��� �ڷ�ƾ�� ����ǰ� �ִ��� ���� ����(true�� ��ٸ��� �ִٶ�� ��)

    [SerializeField] float speed;        // �̵� �ӵ� ������ �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����


    enum MonsterState_Mini05         // ���¿� ���� �� ĳ������ �ൿ�� �ٸ�
    {
        Moving01,
        Attack,
        Wait,
    }
    MonsterState_Mini05 State_Mini05;   // ���� ���¸� �޴� ����

    Vector3 awakeSpot;                   // �� �� ��ġ ����
    Vector3 spot01;                      // �� �� ��ġ ����
    Vector3 spot02;                      // 100m ��ġ ����
    Vector3 spot03;                      // 90m ��ġ ����
    Vector3 spot04;                      // 80m ��ġ ����
    Vector3 spot05;                      // 70m ��ġ ����
    Vector3 spot06;                      // 60m ��ġ ����
    Vector3 spot07;                      // 50m ��ġ ����

    Vector3 originPos;                      // ó�� ��ġ ���� ����

    Animator anim;                       // �ִϸ����ʹ� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    WaitForSeconds delay_01;
    WaitForSeconds delay_02;

    int runId;                    // �޸��� �ִϸ����͸� �޴� ����
    int attackId;                 // �����ϴ� �ִϸ����͸� �޴� ����



	void Awake()
	{
        anim = GetComponent<Animator>();                     // �� ��ũ��Ʈ�� ���� �ü��� �ִϸ����͸� ������
        attackId = Animator.StringToHash("isAttack");        // �����ϴ� �ִϸ����� ����
        runId = Animator.StringToHash("isRun");              // �޸��� �ִϸ����� ����

        originPos = transform.position;  // ���� ��ġ�� ����
        myTransform = transform;         // ���� ��ġ ����(ȭ�� ��)
        Cannon = player;                 // �߻� ��ǥ ��ġ�� ����(���� ��ũ��Ʈ���� ó��)

        awakeSpot = new Vector3(150.0f, 0, thisStartInt);        // �� �� ��ġ
        spot01 = new Vector3(128.0f, 0, thisStartInt);           // �� �� ��ġ
        spot02 = new Vector3(100.0f, 0, thisInt);                // 100m ��ġ
        spot03 = new Vector3(90.0f, 0, thisInt);       // ù��° ���... (���� Ȯ�� 10%)
        spot04 = new Vector3(80.0f, 0, thisInt);       // �ι�° ���... (���� Ȯ�� 30%)
        spot05 = new Vector3(70.0f, 0, thisInt);       // ����° ���... (���� Ȯ�� 50%)
        spot06 = new Vector3(60.0f, 0, thisInt);       // �׹��� ���... (���� Ȯ�� 80%)
        spot07 = new Vector3(50.0f, 0, thisInt);       // �ټ����� ���... (���� Ȯ�� 100%)

        Projectile = transform.GetChild(2).transform;     // �߻�ü ������Ʈ�� �޴´�.

        anim.SetBool(runId, true);          // ���� �ִϸ��̼� ����

        Rot_Setting();                      // ȸ���� ���⼭ ������???
        tempSpot = awakeSpot;               // ó�� �̵��ϴ� ���� �ѱ��.

        delay_01 = new WaitForSeconds(1.0f);   // ȭ���� ��ٸ��� �ð�
        delay_02 = new WaitForSeconds(5.0f);
    }


	void OnEnable()                     // �ü��� Ȱ��ȭ ���� ��...
	{
        anim.SetBool(attackId, false);     // ���� �ִϸ��̼� ��
        anim.SetBool(runId, true);         // �޸��� �ִϸ��̼� ����
    }

	void OnDisable()                     // �ü��� ��Ȱ��ȭ ���� ��...
    {
        mini05_Spawn.list_Archer.Add(transform.gameObject);       // ��Ȱ��ȭ �Ǿ��� �� �� �ü��� �ٽ� �ü� ���� ����Ʈ�� �ִ´�..
        transform.position = originPos;                           // �� �ü��� ��ġ�� ó�� ��ġ��(�ѹ� ���� ���ʹ� ���� ������ �׻� ����) �ѱ��.
        State_Mini05 = MonsterState_Mini05.Moving01;              // �ü� ���� �ʱ�ȭ�� �Ѵ�.

        Projectile.gameObject.SetActive(false);                   // �߻�ü�� ����.
        Projectile.position = myTransform.localPosition;          // �߻�ä�� ���� ��ġ�� �ű��.

        tempInt = 0;               // ���� ��ġ ������ �ʱ�ȭ �Ѵ�.
        tempSpot = awakeSpot;      // ���� ��ġ�� �ʱ�ȭ �Ѵ�.
    }

    void Update()
    {
        if (State_Mini05.Equals(MonsterState_Mini05.Moving01))   // �̵�
        {
            MovingToPos();                         // �ڽ��� �ش�� �Ա� �ڷ� ���� �Լ�
        }
        else if (State_Mini05.Equals(MonsterState_Mini05.Attack))  // ����
        {
            Attack();
        }
        else        // ��ٸ�
        {
            Waithing();
        }
    }


    void MovingToPos()           // �� �ڷ� ���� �Լ�
    {
        dir = tempSpot - transform.position;                // ���� ����
        transform.LookAt(tempSpot);      // ���� �������� �ٶ󺸷�...
        transform.position += dir.normalized * speed * Time.deltaTime;   // ��ǥ�� �̵�

        if (dir.magnitude <= 0.1f)           // ��ǥ�� �ٴ޾Ҵٸ�....
        {
            tempInt++;           // ���� ��ġ�� ���� ���� ����
            SwichFuction();      // ���� ��ġ ������ ��� �Լ�
        }
    }

    void Rot_Setting()           // �ü��� ȭ�� �� ��, ȸ�� ��ġ(�ü��� Ư����.......)
    {
        int rotInt;
        switch (thisInt)         // �� �ü��� ��ġ ���ڿ� ����(�������� ���� ��) ȸ�� ���ڸ� �޴´�.
        {
            case -145:           // �� ������ Z��(��, ��) ��ġ���...
                rotInt = 50;     // ȸ�� ������ 50���� ��
                break;
            case -125:
                rotInt = 45;     // ȸ�� ������ 45���� ��
                break;
            case -105:
                rotInt = 40;     // ȸ�� ������ 40���� ��
                break;
            case -85:
                rotInt = 35;     // ȸ�� ������ 35���� ��
                break;
            case -65:
                rotInt = 25;     // ȸ�� ������ 25���� ��
                break;
            case -45:
                rotInt = 15;     // ȸ�� ������ 15���� ��
                break;
            case -25:
                rotInt = 0;      // ȸ�� ������ 0���� ��
                break;
            case -5:
                rotInt = -15;     // ȸ�� ������ -15���� ��
                break;
            case 15:
                rotInt = -25;     // ȸ�� ������ -25���� ��
                break;
            case 35:
                rotInt = -35;     // ȸ�� ������ -35���� ��
                break;
            case 55:
                rotInt = -45;     // ȸ�� ������ -45���� ��
                break;
            case 75:
                rotInt = -55;     // ȸ�� ������ -55���� ��
                break;
            case 95:
                rotInt = -60;     // ȸ�� ������ -60���� ��
                break;
            case 115:
                rotInt = -70;     // ȸ�� ������ -70���� ��
                break;
            default:
                rotInt = -75;     // ȸ�� ������ -75���� ��
                break;
        }

        tempRot = new Vector3(0, rotInt, 0);     // �� �ü��� ȸ���� ���ڿ� ���� ��´�.(�̰Ŵ� �� ����.)
    }


    void SwichFuction()                  // ���� ��ġ ������ ��� �Լ�
    {
        switch (tempInt)                 // ���� ��ġ ���� ������ ����...
        {
            case 0:                      // �Ա� �ڷ� ���� ��
                tempSpot = awakeSpot;    // 0�϶� ��ġ��
                break;
            case 1:                      // �Ա� ������ ���� ��
                tempSpot = spot01;    // 1�϶� ��ġ��
                break;
            case 2:                      // 100m�� ���� ��
                tempSpot = spot02;    // 2�϶� ��ġ��
                break;
            case 3:                      // 90m�� ���� ��      ���⼭���� ���.
                tempSpot = spot03;    // 3�϶� ��ġ��
                currentMeter = 90;       // �� �ü��� ���� 90m�� �ִٰ� �˸���.
                break;
            case 4:                      // 80m�� ���� ��
                tempSpot = spot04;    // 4�϶� ��ġ��
                State_Mini05 = MonsterState_Mini05.Attack;     // ���� ���·� �ٷ� �ٲ�
                currentMeter = 80;       // �� �ü��� ���� 80m�� �ִٰ� �˸���.
                break;
            case 5:                      // 70m�� ���� ��
                tempSpot = spot05;    // 5�϶� ��ġ��
                State_Mini05 = MonsterState_Mini05.Attack;     // ���� ���·� �ٷ� �ٲ�
                currentMeter = 70;       // �� �ü��� ���� 70m�� �ִٰ� �˸���.
                break;
            case 6:                      // 60m�� ���� ��
                tempSpot = spot06;    // 6�϶� ��ġ��
                State_Mini05 = MonsterState_Mini05.Attack;     // ���� ���·� �ٷ� �ٲ�
                currentMeter = 60;       // �� �ü��� ���� 60m�� �ִٰ� �˸���.
                break;
            case 7:                      // 50m�� ���� ��
                tempSpot = spot07;    // 7�϶� ��ġ��
                State_Mini05 = MonsterState_Mini05.Attack;     // ���� ���·� �ٷ� �ٲ�
                currentMeter = 50;       // �� �ü��� ���� 50m�� �ִٰ� �˸���.
                break;
            case 8:          // ����
                State_Mini05 = MonsterState_Mini05.Attack;     // ���� ���·� �ٷ� �ٲ�
                Attack();
                break;
            case 9:         // ��� ��
                State_Mini05 = MonsterState_Mini05.Wait;      // ��ٸ� ���·� �ٷ� �ٲ�
                break;
        }
    }


    void Attack()                       // ���� �ִϸ��̼�..............................................................
    {
        anim.SetBool(attackId, true);     // ���� �ִϸ��̼� ����
        anim.SetBool(runId, false);          // ���� �ִϸ��̼� ����

        transform.rotation = Quaternion.Euler(tempRot);      // ���� �ҋ��� �̸� �غ�� ȸ������ �ٲ۴�.
    }


    public void AfterAttack()             // ���� ���Ŀ� ����� �Լ�(���� �ִϸ��̼� �������� ����)
    {
        anim.SetBool(attackId, false);    // ���� �ִϸ��̼� ��
        anim.SetBool(runId, false);    // ���� �ִϸ��̼� ��

        State_Mini05 = MonsterState_Mini05.Wait;        // ��� ��
    }

    public void Shooting()                // ���� �߹ݿ� ����� �Լ�(���� �ִϸ��̼� �߰��� ����)
    {
        StartCoroutine(SimulateProjectile());     // ȭ���� �߻��ϴ� �ڷ�ƾ ����
    }

    

    void RandomTarget(int currentMeter)           // �� ��� ��ġ���� ��� ��(����)���� ���� ���ϴ� �Լ�
    {
        int randInt = Random.Range(0, 10);       // ���� Ȯ�� ���ϱ�
        int FailPercent;                     // ���� Ȯ��

        switch (currentMeter)                // ���� ���͸� �ľ��ؼ�
        {
            case 90:                         // ���� ���Ͱ� 90m�̶��...
                FailPercent = 9;             // ȭ�� ���� Ȯ���� 90%�� �ٲ۴�.
                break;
            case 80:                         // ���� ���Ͱ� 80m�̶��...
                FailPercent = 7;             // ȭ�� ���� Ȯ���� 70%�� �ٲ۴�.
                break;
            case 70:                         // ���� ���Ͱ� 70m�̶��...
                FailPercent = 5;             // ȭ�� ���� Ȯ���� 50%�� �ٲ۴�.
                break;
            case 60:                         // ���� ���Ͱ� 60m�̶��...
                FailPercent = 2;             // ȭ�� ���� Ȯ���� 20%�� �ٲ۴�.
                break;
            default:                         // ���� ���Ͱ� 50m�̶��...
                FailPercent = 0;             // ȭ�� ���� Ȯ���� 0%�� �ٲ۴�.
                break;
        }

        if (randInt < FailPercent)   // ���� Ȯ�� (��)
        {
            Target = FailPos;        // Ÿ���� ���� �������� ����
        }
        else               // ���� Ȯ�� 10%
        {
            Target = Cannon;         // Ÿ���� �÷��̾�� ����
        }
    }

    void Waithing()              // ��� ��ٸ��� �Լ�
    {
        if (isRun.Equals(false))        // �ڷ�ƾ�� ����ǰ� ���� �ʴٸ�...
        {
            StartCoroutine(WaitCoroutine());         // ��ٸ��� �ڷ�ƾ ����
        }
    }




    //////////////////////////////       �ڷ�ƾ ����...


    IEnumerator WaitCoroutine()                      // ��ٸ��� �ڷ�ƾ ����(���� ��ٷȴٰ� �̵� Ȥ�� ���� ��� ����)
    {
        isRun = true;                             // �ڷ�ƾ�� ����ǰ� �ִٰ� �˸�
        yield return delay_02;    // ��� ��ٸ� ������

        anim.SetBool(runId, true);    // ���� �ִϸ��̼� ��

        Projectile.gameObject.SetActive(false);   // ȭ���� ��Ȱ��ȭ ��Ŵ
        
        if (!tempInt.Equals(8))     // ���� ���°� �ƴ϶��...
        {
            State_Mini05 = MonsterState_Mini05.Moving01;    // �̵� ���·� �ٲ�
        }
        else                        // ���� ���¶��...
        {
            State_Mini05 = MonsterState_Mini05.Attack;      // ���� ���·� �ٲ�      
        }
        isRun = false;             // �ڷ�ƾ�� �����ٰ� �˷���
    }



    IEnumerator SimulateProjectile()               // ȭ���� �߻��ϴ� �ڷ�ƾ
    {
        RandomTarget(currentMeter);             // ��� ��ġ���� ���� �˾ƺ��� �Լ�

        yield return delay_01;    // ��� ��ٸ� ��...
        Projectile.gameObject.SetActive(true);    // ȭ�� ������Ʈ Ȱ��ȭ...


        Projectile.position = transform.position;                                  // ȭ�� ��ġ�� �ڽſ��� �����´�
        float target_Distance = Vector3.Distance(Projectile.position, Target.position);                        // ���� ��ġ�� ��ǥ ��ġ ������ �Ÿ��� ���Ѵ�.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);  // �������� ���ϵ��� §��.
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        float flightDuration = target_Distance / Vx;
        Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);                  // ȸ��??
        float elapse_time = 0;

        while (elapse_time < flightDuration)        // ȭ���� ���󰣴�.
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
}
