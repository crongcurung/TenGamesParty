using System.Collections;
using UnityEngine;

public class Mini02_Guest : MonoBehaviour
{
    public Transform target;       // ī���� �տ� �ִ� �ֹ� ������ �޴� ����
    CapsuleCollider col;          // �ڽſ��� �پ� �ִ� �ݶ��̴��� �޾ƿ��� ����

    Vector3 originPos;             // ������������ �ǵ��ƿ� �� ���Ǳ� ���� ����

    public bool endBool = false;   // �ֹ��� �����ٴ� ���� �˸��� ����

    Animator anim;                  // �ڽſ��� �پ� �ִ� �ִϸ����͸� ���� ����
    int posKeyId;                   // �ִϸ����͸� ����ȭ�ϱ� ���� ��ƮŰ

    bool isStop = true;           // �ٷ� �տ� �մ��� �ִٸ� ���߱� ���� ����
    bool counterStop = false;     // ī��Ʈ�� �����ߴٸ�...

    public int charInt = 0;       // �ڽ��� � ������ ���ϴ��� �̴� ���� ��Ʈ ����
                                  // (0 = ����/����/��ũ, 1 = ����/����/����, 2 = ����/Ƣ��/��ũ, 3 = ����/Ƣ��/����,
                                  //  4 = ��Ÿ/����/��ũ, 5 = ��Ÿ/����/����, 6 = ��Ÿ/Ƣ��/��ũ, 7 = ��Ÿ/Ƣ��/����)

    public Mini02_Player mini02_Player;    // ���� ��ũ��Ʈ���� �÷��̾� ��ũ��Ʈ�� �޾ƿ�
    public Mini02_Spawn mini02_Spawn;      // ���� ��ũ��Ʈ���� ���� ��ũ��Ʈ�� �޾ƿ�
    Mini02_CountLine mini02_CountLine;     // ī��Ʈ���� ��ũ��Ʈ�� �޾ƿ�

    WaitForSeconds delay;

    LayerMask mask;              // ���̾ �̸� ����

    Quaternion countRot;         // ī��Ʈ ������ ���� ȸ�� ��
    Quaternion endRot;           // ���� ���� ������ ���� ȸ�� ��

    Coroutine coroutine;

    void Awake()
    {
        originPos = transform.position;      // ó����ġ(���� ��ġ)�� ������ ��´�.

        col = GetComponent<CapsuleCollider>();          // �ڽ��� �ݶ��̴� ���
        anim = GetComponent<Animator>();                   // �ڽ��� �ִϸ����� ���
        posKeyId = Animator.StringToHash("isRun");          // ����ȭ �۾�

        countRot = Quaternion.Euler(0, -90, 0);   // ȸ�� �� ����
        endRot = Quaternion.Euler(0, 90, 0);

        mini02_CountLine = target.GetComponent<Mini02_CountLine>(); 
        mask = LayerMask.GetMask("Monster");  // �մԸ� �ޱ�
        delay = new WaitForSeconds(0.05f);
    }


	void OnEnable()
	{
        charInt = Random.Range(0, 8);                     // �������� ������ �̴´�.
        counterStop = false;       // ī��Ʈ�� �����ߴٴ� ���� �ʱ�ȭ
        endBool = false;           // ������ ���� �ʱ�ȭ
        col.isTrigger = false;     // �մ� Ʈ���� ����

        StartCoroutine(updateCoroutine());      // ����ȭ �ڷ�ƾ
    }



	void FixedUpdate()
    {
        if (isStop.Equals(true))      // ����ٸ� ����
        {
            return;
        }

        if (endBool.Equals(false))    // ������ �ʾҴٸ� ī��Ʈ�� ���� �޸���.
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 2 * Time.fixedDeltaTime);
            transform.rotation = countRot;      // ó���������� �ٶ󺸱�
        }
        else   // �����ٸ� ó���������� �޸���.
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, 2 * Time.fixedDeltaTime);
            transform.rotation = endRot;      // ó���������� �ٶ󺸱�
        }
    }

    public void GoBack()       // ī���� �������� ������ ó���������� �ǵ��ư��� �Լ�
    {
        endBool = true;          // �ϴ� ������
        col.isTrigger = true;     // �ٸ� �մ��ϰ� �� ��ġ�� ���ؼ�, Ʈ���� �ѵα�

        this.gameObject.layer = 8;    // �ڽ��� ���̾ �ٲٱ�

        StopCoroutine(coroutine);
    }


    IEnumerator updateCoroutine()             // �ڷ�ƾ���� ����ȭ
    {
        while (true)
        {
            if (col.isTrigger.Equals(true) && (transform.position - originPos).magnitude < 0.2f)  // �ڽ��� �ݶ��̴��� Ʈ���� �����̰�, ó���������� ���� �� ���� 
            {
                //mini02_Spawn.InsertQueue_Monster(transform.gameObject);        // ���ֹ���

                this.gameObject.layer = 7;
                mini02_Spawn.spawnInt--;                      // ���� ���� ���̱�
                transform.gameObject.SetActive(false);
            }

            RaycastHit hit;

            if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), out hit, 1.0f, mask) && col.isTrigger.Equals(false))
            // �մ��� �ٶ󺸴� ������ ���������, �׸��� ��밡 Ʈ���Ű� ������ �־�� �Ѵ�.
            {
                isStop = true;   // Ʈ���Ű� ���� �մ��� ������ �����. 
            }
            else
            {
                isStop = false;  // Ʈ���Ű� ���� �մ��� ������ ����
            }


            if (isStop.Equals(true) || (target.position - transform.position).magnitude < 0.1f)  // ���� ������ �ʾҰų�, ��ǥ ������ �����ߴٸ�
            {
                anim.SetBool(posKeyId, false);   // �޸��� ��� ����
            }
            else
            {
                anim.SetBool(posKeyId, true);     // �޸��� ��� ����
            }

            if ((transform.position - target.position).magnitude < 0.01f && counterStop.Equals(false))      // ���Ͱ� ī���Ϳ� �����ߴٸ�...
            {
                counterStop = true;    // ī��Ʈ�� �����ߴٰ� �˸�
                coroutine = StartCoroutine(GoBackCoroutine());           // �ǵ��ƿ��� �ϴ� �ڷ�ƾ

                mini02_CountLine.SettingMenu(charInt, this, goBackTime);        // �޴� �����ϱ�!

                
            }

            yield return delay;
        }
    }

    float goBackTime = 0;

    IEnumerator GoBackCoroutine()        // �ǵ��ƿ��� �ϴ� �ڷ�ƾ
    {
        if (mini02_Player.scoreInt <= 3)             // ���ھ 10 ���϶��...
        {
            goBackTime = 60.0f;                      // �ǵ��ư��� �ð��� 60�ʷ� ����
        }
        else if (mini02_Player.scoreInt <= 6)             // ���ھ 20 ���϶��...
        {
            goBackTime = 57.0f;                      // �ǵ��ư��� �ð��� 57�ʷ� ����
        }
        else if (mini02_Player.scoreInt <= 9)             // ���ھ 30 ���϶��...
        {
            goBackTime = 54.0f;                      // �ǵ��ư��� �ð��� 54�ʷ� ����
        }
        else if (mini02_Player.scoreInt <= 12)             // ���ھ 40 ���϶��...
        {
            goBackTime = 51.0f;                      // �ǵ��ư��� �ð��� 51�ʷ� ����
        }
        else if (mini02_Player.scoreInt <= 15)             // ���ھ 50 ���϶��...
        {
            goBackTime = 48.0f;                      // �ǵ��ư��� �ð��� 48�ʷ� ����
        }
        else if (mini02_Player.scoreInt <= 18)             // ���ھ 60 ���϶��...
        {
            goBackTime = 45.0f;                      // �ǵ��ư��� �ð��� 45�ʷ� ����
        }
        else if (mini02_Player.scoreInt <= 21)             // ���ھ 70 ���϶��...
        {
            goBackTime = 42.0f;                      // �ǵ��ư��� �ð��� 42�ʷ� ����
        }
        else if (mini02_Player.scoreInt <= 24)             // ���ھ 80 ���϶��...
        {
            goBackTime = 39.0f;                      // �ǵ��ư��� �ð��� 39�ʷ� ����
        }
        else if (mini02_Player.scoreInt <= 27)             // ���ھ 90 ���϶��...
        {
            goBackTime = 36.0f;                      // �ǵ��ư��� �ð��� 36�ʷ� ����
        }
		else if (mini02_Player.scoreInt <= 30)             // 
		{
			goBackTime = 33.0f;                      // 
		}
		else if (mini02_Player.scoreInt <= 33)             // 
		{
			goBackTime = 30.0f;                      // 
		}
		else                                              // 
        {
            goBackTime = 27.0f;                      // �ǵ��ư��� �ð��� 27�ʷ� ����
        }


        yield return new WaitForSeconds(goBackTime);  // ������ �ð� �Ŀ� �ǵ��ư��� �ϴ� 

        AudioMng.ins.LoopEffect(false);
        AudioMng.ins.PlayEffect("Fail02");      // �մ� �׳� ����
        mini02_CountLine.GoHome_Guest(true);        // ü���� ����ϰ� �մ��� �ǵ��ư��� �ϴ� ����
    }
}
