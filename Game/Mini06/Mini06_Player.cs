using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mini06_Player : MonoBehaviour
{
    [SerializeField] Joystic joystic;

    [SerializeField] GameObject[] Hp_Array;

    Rigidbody rigid;                 // ������ٵ�� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����
    Animator anim;                       // �ִϸ����ʹ� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    [SerializeField] float speed;        // �̵� �ӵ� ������ �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    [SerializeField] Image rightButtonImage;    // ������ ��ư �̹���
    [SerializeField] Sprite[] sprite_Array;           // 0 : ���ڸ�ä, 1 : �׹�, 2 : ������ ��ü

    public bool isAlive;              // �÷��̾ ��� �ִ��� ���� ����

    public bool isTouch_Bear = false;    // �÷��̾ ���� ��ҳ� ���� ����

    bool isRun = false;         // ���ڸ� ä Ȥ�� �׹��� �ֵθ��� �ֳ�?
    bool isTable = false;                  // å�� ��ҳ�?

    [SerializeField] Mini06_Spawn mini06_Spawn;
    [SerializeField] Mini06_Net mini06_Net;        // ���ڸ� ä ��ũ��Ʈ
    [SerializeField] Transform tableObject;        // ���̺��� ��ġ(�ٶ󺼶� ���)

    [SerializeField] GameObject net_Player;           // �÷��̾ ��� �ִ� ���ڸ�ä ������Ʈ
    [SerializeField] GameObject web_player;           // �÷��̾ ��� �ִ� �׹� ������Ʈ


    int currentObject = 0;             // ���� ��� �ִ� ���� �����ΰ�? 0�� : ���ڸ�ä, 1�� : �׹�

    [SerializeField] GameObject net_Table;            // ���̺� �÷��� �ִ� ���ڸ�ä ������Ʈ
    [SerializeField] GameObject web_Table;            // ���̺� �÷��� �ִ� �׹� ������Ʈ

    [SerializeField] Transform middle;             // �׹��� ���� ��ġ

    int playerHp;                        // �÷��̾��� ü�� ����

    Vector3 moveDir;                     // �̵� ������ �÷��̾ ���� ���ɼ� ����

    int runId;
    int attackId01;
    int attackId02;

    string hor_Text;
    string ver_Text;

    string invoke_Text01;
    string invoke_Text02;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        isAlive = true;                   // �÷��̾ ����ִٰ� ��
        playerHp = 5;                     // �÷��̾� ü���� 5����...

        runId = Animator.StringToHash("isRun");
        attackId01 = Animator.StringToHash("isAttack");
        attackId02 = Animator.StringToHash("isAttack02");

        hor_Text = "Horizontal";
        ver_Text = "Vertical";

        invoke_Text01 = "Invoke_Wait_BearAttack";
        invoke_Text02 = "Invoke_Press";
    }

	void Start()
	{
        AudioMng.ins.Play_BG("Mini06_B");
    }

	void FixedUpdate()
    {
        if (!moveDir.x.Equals(0) || !moveDir.z.Equals(0))               // wasd �ƹ��ų��� �����δٸ�..
        {
            anim.SetBool(runId, true);         // �����̴� �ִϸ��̼� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z)), 0.5f);
            // �÷��̾� ȸ��

            transform.position += new Vector3(moveDir.x, 0, moveDir.z).normalized * Time.fixedDeltaTime * speed;      // �÷��̾� �̵�
        }
        else
        {
            anim.SetBool(runId, false);       // ���ߴ� �ִϸ��̼� ����
        }
    }

    void Update()
    {
        if (isTouch_Bear.Equals(true))        // ���� ��Ҵٸ�...
        {
            isTouch_Bear = false;        // �ߺ� ����

            Invoke(invoke_Text01, 1.0f);
        }

        Move();

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Press_RightButton();     // ������ ��ư�� �����ٸ�..
        }
    }



    void Move()         // �̵� �Լ��� �θ�ɷ� ��
    {
#if (UNITY_EDITOR)
        float hor = Input.GetAxis(hor_Text);
        float ver = Input.GetAxis(ver_Text);
#elif (UNITY_IOS || UNITY_ANDROID)
		float hor = joystic.Horizontal;
        float ver = joystic.Vertical;
#endif




        moveDir.x = hor;
        moveDir.z = ver;
    }

    void MinusHp()        // �÷��̾��� ü���� ��´�..
    {
        playerHp--;
        Hp_Array[playerHp].SetActive(false);

        AudioMng.ins.PlayEffect("Meow");    // ����� ��ħ
        if (playerHp <= 0)
        {
            isAlive = false;

            mini06_Spawn.End_Game();
        }
    }

    public void MinusHp_Mini06()
    {
        MinusHp();
    }

    public void Before_Attack()         // ���� ���� ��ư�� ������.
    {
        mini06_Net.isAttack = true;
    }

    public void After_Attack()          // ���� �ִϸ��̼� ��
    {
        mini06_Net.isAttack = false;
    }


    public void Press_RightButton()        // ������ ��ư�� �����ٸ�...
    {
        Invoke(invoke_Text02, 0.1f);

        if (isRun.Equals(false))                // ������ �ϰ� ���� �ʴٸ�...
        {
            if (currentObject.Equals(0))        // ���ڸ�ä�� ��� �ִٸ�..
            {
                if (isTable.Equals(true))         // å�� �������
                {
                    transform.LookAt(tableObject);      // ���̺��� �ٶ󺻴�.(�߾�)

                    AudioMng.ins.PlayEffect("TrainSide");    // ��ü �Ҹ�
                    rightButtonImage.sprite = sprite_Array[2];

                    net_Player.SetActive(false);        // ���ڸ�ä ��Ȱ��ȭ
                    web_player.SetActive(true);         // �׹� Ȱ��ȭ

                    net_Table.SetActive(true);          // ���̺� ���ڸ�ä Ȱ��ȭ
                    web_Table.SetActive(false);         // ���̺� �׹� ��Ȱ��ȭ

                    currentObject = 1;                  // ������ �׹��� ��� �ִٰ� �˸�

                    mini06_Net.isAttack = false;        // ���ڸ� ä�� �پ� �ִ� Ʈ���Ÿ� ��Ȱ��ȭ(Ȥ�� ����..)
                }
                else                        // å�� ���� �ʴٸ�...
                {
                    if (isRun.Equals(false))
                    {
                        AudioMng.ins.PlayEffect("Sword");    // ä �ֵθ��� �Ҹ�
                        StartCoroutine(netCoroutine());      // ���ڸ� ä�� �ֵθ��� �ڷ�ƾ ����
                    }
                }
            }
            else                        // �׹��� ��� �ִٸ�...
            {
                if (isTable.Equals(true))         // å�� �������
                {
                    transform.LookAt(tableObject);      // ���̺��� �ٶ󺻴�.(�߾�)

                    AudioMng.ins.PlayEffect("TrainSide");    // ��ü �Ҹ�
                    rightButtonImage.sprite = sprite_Array[2];

                    web_player.SetActive(false);        // �׹� ��Ȱ��ȭ
                    net_Player.SetActive(true);         // ���ڸ�ä Ȱ��ȭ

                    net_Table.SetActive(false);         // ���̺� �׹� ��Ȱ��ȭ
                    web_Table.SetActive(true);          // ���̺� ���ڸ�ä Ȱ��ȭ

                    currentObject = 0;                // ������ ���ڸ� ä�� ��� �ִٰ� �˸�
                }
                else                          // å�� ���� �ʾҴٸ�...
                {
                    AudioMng.ins.PlayEffect("SpeedUp");    // ä �ֵθ��� �Ҹ�
                    StartCoroutine(Shot_Web_Coroutine());
                }
            }
        }
    }

    void Invoke_Press()
    {
        if (currentObject.Equals(0))
        {
            anim.SetBool(attackId01, false);
        }
        else
        {
            anim.SetBool(attackId02, false);
        }
    }



    ////////////////  �ڷ�ƾ ����....

    void Invoke_Wait_BearAttack()
    {
        rigid.velocity = Vector3.zero;
    }
     



    IEnumerator netCoroutine()      // ���ڸ� ä�� �ֵθ��� �ڷ�ƾ
    {
        isRun = true;                // ���ڸ� ä�� �ֵθ��� �ִٰ� �˸�
        anim.SetBool(attackId01, true);   // ���ڸ� ä�� �ֵθ��� �ִϸ��̼� ����

		rightButtonImage.fillAmount = 0.0f;     // ������ź ������ ��ư �̹���
		while (rightButtonImage.fillAmount <= 0.9999f)          // 1�� �ϸ� �ȵȴ�;;;
		{
			rightButtonImage.fillAmount += Time.deltaTime;

			yield return null;
		}

		yield return null;
        isRun = false;               // �ڷ�ƾ�� �����ٰ� �˸�
    }


    IEnumerator Shot_Web_Coroutine()        // �׹��� ������ �ڷ�ƾ
    {
        isRun = true;                // �׹��� �ֵθ��� �ִٰ� �˸�

        anim.SetBool(attackId02, true);         // �׹��� ������ �ִϸ��̼� ����

        GameObject t_object = mini06_Spawn.GetQueue_Web();      // �׹� ������Ʈ Ǯ������ ������
        t_object.transform.position = middle.position;     // �߻�뿡�� ����
        t_object.transform.rotation = middle.rotation;     // ���� �߻�� ȸ������ ����

        Rigidbody bulletRigid = t_object.GetComponent<Rigidbody>();
        bulletRigid.velocity = transform.forward * 10.0f;               // �׹��� ����

		rightButtonImage.fillAmount = 0.0f;     // ������ź ������ ��ư �̹���
		while (rightButtonImage.fillAmount <= 0.9999f)          // 1�� �ϸ� �ȵȴ�;;;
		{
			rightButtonImage.fillAmount += Time.deltaTime;

			yield return null;
		}

		yield return null;

        isRun = false;               // �ڷ�ƾ�� �����ٰ� �˸�
    }


    /////////////// �ݸ��� ����...


	void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.layer.Equals(3))      // ���̺� ��Ҵٸ�...   WALL
        {
            isTable = true;      // ��Ҵٰ� �˷���

            
            rightButtonImage.sprite = sprite_Array[2];
            
        }
        else if (collision.gameObject.layer.Equals(8))   // ������ ��Ҵٸ�...  Object
        {
            AudioMng.ins.PlayEffect("HitApple");    // ���� ��ǳ �Ҹ�
            rigid.velocity = (transform.position - collision.transform.position).normalized * 50.0f; // ��ǳ ȿ��..
        }
    }

	void OnCollisionExit(Collision collision)
	{
        if (collision.gameObject.layer.Equals(3))      // ���̺��� �������ٸ�....   WALL
        {
            isTable = false;    // �������ٰ� �˷���

            if (currentObject.Equals(0))       // �÷��̾ ���� ���ڸ�ä�� ��� �ִٸ�..
            {
                rightButtonImage.sprite = sprite_Array[0];
            }
            else                  // �÷��̾ ���� �׹��� ��� �ִٸ�..
            {
                rightButtonImage.sprite = sprite_Array[1];
            }
        }
    }
}
