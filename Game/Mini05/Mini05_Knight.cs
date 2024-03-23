using UnityEngine;

public class Mini05_Knight : MonoBehaviour         // �˻� �����տ� ������
{
    public int thisInt = 0;                  // �� �ü��� ������ ���� ��ȣ(�� �������� ��)
    public int thisStartInt = 0;             // �� �ü��� ������ ���� ��ȣ(ó������ �� ����... ������ ��)       
    public Mini05_Spawn mini05_Spawn;        // ���� ��ũ��Ʈ

    Vector3 endPos;                      // 50m ��ġ ����
    bool isAttack = false;               // ���� �ִϸ��̼� ����ȭ ����
    int posInt = 0;

    Vector3 awakeSpot;                   // �� �� ��ġ ����
    Vector3 spot01;                      // �� �� ��ġ ����
    Vector3 spot02;                      // 100m ��ġ ����

    Vector3 originPos;                      // ó�� ��ġ ���� ����

    Animator anim;                       // �ִϸ����ʹ� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    [SerializeField] protected float speed;        // �̵� �ӵ� ������ �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����
    int attackId;                 // �����ϴ� �ִϸ����͸� �޴� ����

	void Awake()
	{
        originPos = transform.position;   // �˻� ó�� ��ġ�� ��´�.(�Ҵ�� ���ڷ� �ϴ°Ŷ� ��� �� ��ġ��...)

        awakeSpot = new Vector3(150.0f, 0.5f, thisStartInt);        // �� �� ��ġ
        spot01 = new Vector3(128.0f, 0.5f, thisStartInt);           // �� �� ��ġ
        spot02 = new Vector3(100.0f, 0.5f, thisInt);                // 100m ��ġ
        endPos = new Vector3(0, 0.5f, thisInt);                     // ���� ��ġ 

        anim = GetComponent<Animator>();
        attackId = Animator.StringToHash("isAttack");        // �����ϴ� �ִϸ����� ����
    }


	void OnDisable()           // ��Ȱ��ȭ �ҋ�..
	{
        mini05_Spawn.list_Knight.Add(transform.gameObject);     // ���� �˻� ����Ʈ�� �ٽ� ��´�.
        transform.position = originPos;                         // ���� ó�� ��ġ�� �ٲ۴�.

        posInt = 0;        // ���� ��ġ�� ���� ���� ����
        isAttack = false;  // ���� �ִϸ��̼� ����ȭ ����
    }



	void Update()
    {
        switch (posInt)              // ���� ��ġ ���� ���� ������ ����..
        {
            case 0:                  // 0�̶��..
                KnightToPos(awakeSpot);    // ���� �ڷ� ����
                break;
            case 1:                  // 1�̶��..
                KnightToPos(spot01);       // ���� ������ ����
                break;
            case 2:                  // 2�̶��..
                KnightToPos(spot02);       // 100m�� ����
                break;
            case 3:                  // 3�̶��..
                KnightToPos(endPos);       // 0m�� ����
                break;
            default:                  // 4�̶��..
                Attack();                  // �����϶�
                break;
        }
    }

    

    void KnightToPos(Vector3 tempPos)     // Ư�� ��ġ�� ���� �Լ�
    {
        Vector3 dir = tempPos - transform.position;                // ���� ����
        transform.LookAt(tempPos);      // ���� �������� �ٶ󺸷�...

        transform.position += dir.normalized * speed * Time.deltaTime * 2.0f;   // ��ǥ�� �̵�

        if (dir.magnitude <= 0.5f)           // ��ǥ�� �ٴ޾Ҵٸ�....
        {
            posInt++;
        }
    }


    void Attack()           // �����ϴ� �Լ�
    {
        if (isAttack.Equals(false))                // ����ȭ..
        {
            isAttack = true;                       // ����ȭ
            anim.SetBool(attackId, true);          // ���� �ִϸ��̼� ����
        }
    }

    public void Attack_Minus()
    {
        mini05_Spawn.Hp_WallMinus();
    }
}
