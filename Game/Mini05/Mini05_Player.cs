using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Mini05_Player : MonoBehaviour
{
    [SerializeField] Joystic joystic;

    bool isRun_BreakThrough = false;       // ���� �վ�� ���� �Ǵ��� ���� ����
    bool isRun_DonutBomb = false;          // ���� ���� ��ź�� ���� �Ǵ��� ���� ����
    bool isRun_Tonedo = false;             // ���� ����̵��� ���� �Ǵ��� ���� ����

    [SerializeField] Mini05_Spawn mini05_Spawn;         // ���� ��ũ��Ʈ�� �����´�.

    Transform bulletPos;               // źȯ���� ó�� ���� ���

    public Image BreakThrough_Image;      // �վ ������ ��ư �̹���
    public Image DonutBomb_Image;         // ���� ��ź ������ ��ư �̹���
    public Image Tonedo_Image;            // ����̵� ������ ��ư �̹���


    public Mini05_CrossHair crossHair_Break;          // ũ�ν� ���(�վ) ��ũ��Ʈ... �ִϸ��̼� ������..
    public Mini05_CrossHair crossHair_Donut;          // ũ�ν� ���(���� ��ź) ��ũ��Ʈ... �ִϸ��̼� ������..

    public Mini05_Swipe mini05_Swipe;                 // �������� �ϳ��� �����´�.

    public Action action;     // �κ�ũ�� �޴� ����(ī�޶� ��鸮�°�)

    bool isRun = false;         // ī�޶� ��鸮�� �ڷ�ƾ�� ����ǰ� �ִ��� ���� ����

    Coroutine coroutine05;          // ī�޶� ��鸮�� �ڷ�ƾ ����
	WaitForSeconds delay_Camera;    // ī�޶� ��鸮�� �ڷ�ƾ �ð�

    Vector3 moveDir;                     // �̵� ������ �÷��̾ ���� ���ɼ� ����

    [SerializeField] float speed;        // �̵� �ӵ� ������ �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    string tag01;

    string hor_Text;
    string ver_Text;

    void Start()
    {
        bulletPos = transform.GetChild(3).transform;                                              // ���� �߻� ������ �޾� ���´�.

        tag01 = "Cushion";

        hor_Text = "Horizontal";
        ver_Text = "Vertical";

        delay_Camera = new WaitForSeconds(2.0f);       // ī�޶� ��鸮�� ���� 2�ʰ�...

        AudioMng.ins.Play_BG("Mini05_B");
    }

    void FixedUpdate()
    {
        if (isRun.Equals(true))          // ī�޶� ��鸮�� �ڷ�ƾ�� ���� ���̶��...
        {
            return;                      // �ƹ� �͵� ���ϰ� ����!!
        }

        if (!moveDir.x.Equals(0) || !moveDir.z.Equals(0))               // wasd �ƹ��ų��� �����δٸ�..
        {
            Left();            // �¿� ȸ�� �� ����
            MoveZ();           // ��, �Ʒ� ȸ�� �� ����

            if (mini05_Swipe.currentWeaponInt.Equals(1))      // �վ
            {
                crossHair_Break.MovingState(true);              // ũ�ν� ��� �þ�� �ִϸ��̼� ����(�̵�)
            }
            else if (mini05_Swipe.currentWeaponInt.Equals(2))      // ���� ��ź
            {
                crossHair_Donut.MovingState(true);              // ũ�ν� ��� �þ�� �ִϸ��̼� ����(�̵�)
            }
        }
        else
        {
            if (mini05_Swipe.currentWeaponInt.Equals(1))      // �վ
            {
                crossHair_Break.MovingState(false);              // ũ�ν� ��� �پ��� �ִϸ��̼� ����(�̵�)
            }
            else if (mini05_Swipe.currentWeaponInt.Equals(2))      // ���� ��ź
            {
                crossHair_Donut.MovingState(false);              // ũ�ν� ��� �پ��� �ִϸ��̼� ����(�̵�)
            }
        }
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.KeypadEnter) && isRun.Equals(false))
        {
            Press_RightButton();
        }
        else                                                  
        {
            if (mini05_Swipe.currentWeaponInt.Equals(1))      // �վ
            {
                crossHair_Break.Shooting(false);              // ũ�ν� ��� �پ��� �ִϸ��̼� ����(�̵�)
            }
            else if (mini05_Swipe.currentWeaponInt.Equals(2))        // ���� ��ź
            {
                crossHair_Donut.Shooting(false);              // ũ�ν� ��� �پ��� �ִϸ��̼� ����(�̵�)
            }
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





    void Left()       // �¿� ȸ�� �� ����
    {
        if (transform.localEulerAngles.y > 250.0f && transform.localEulerAngles.y < 271.5f && moveDir.x < 0.0f)              // ���� ȸ�� ����
        {
            return;
        }

        if (transform.localEulerAngles.y < 180.0f && transform.localEulerAngles.y > 88.5f && moveDir.x > 0.0f)              // ������ ȸ�� ����
        {
            return;
        }

		Vector3 eulerAngle = new Vector3(0f, Time.fixedDeltaTime * moveDir.x * speed, 0f);   // ȸ��
		transform.Rotate(eulerAngle, Space.World);
		Quaternion rot = transform.rotation;
		transform.rotation = Quaternion.Euler(eulerAngle) * rot;
	}

    void MoveZ()        // ��, �Ʒ� ȸ�� �� ����
    {
        if (transform.localEulerAngles.z < 200.0f && transform.localEulerAngles.z > 45.0f && moveDir.z > 0.0f)    // �� ȸ�� ����
        {
            return;
        }

        if (transform.localEulerAngles.z > 70.0f && transform.localEulerAngles.z < 300.0f && moveDir.z < 0.0f)    // �Ʒ� ȸ�� ����
        {
            return;
        }
        Vector3 eulerAngle = new Vector3(0f, 0f, Time.fixedDeltaTime * moveDir.z * speed);   // ȸ��
        transform.Rotate(eulerAngle, Space.Self);
        transform.localRotation *= Quaternion.Euler(eulerAngle);
    }
    

    public void Press_RightButton()                       // �߻�, ������ ��ư�� �����ٸ�...
    {
        if (mini05_Swipe.currentWeaponInt.Equals(1))      // �վ
        {
            if (isRun_BreakThrough.Equals(false))             // �վ �� ��� �ð��� �����ٸ�...
            {
                AudioMng.ins.PlayEffect("Dough");    // �վ �߻� �Ҹ�
                StartCoroutine(Shot_Coroutine(0));            // �߻� �ڷ�ƾ ����(�վ)
                crossHair_Break.Shooting(true);               // �վ ũ�ν� ��� �ִϸ��̼� ����(����)
            }
        }
        else if (mini05_Swipe.currentWeaponInt.Equals(2))     // ���� ��ź
        {
            if (isRun_DonutBomb.Equals(false))             // ���� ��ź �� ��� �ð��� �����ٸ�...
            {
                AudioMng.ins.PlayEffect("TrainSide");    // ��ź �߻� �Ҹ�
                StartCoroutine(Shot_Coroutine(1));            // �߻� �ڷ�ƾ ����(���� ��ź)
                crossHair_Donut.Shooting(true);               // ���� ��ź ũ�ν� ��� �ִϸ��̼� ����(����)
            }
        }
        else                                                  // ����̵�
        {
            if (isRun_Tonedo.Equals(false))                     // ����̵� �� ��� �ð��� �����ٸ�...
            {
                AudioMng.ins.PlayEffect("SpeedUp");    // ����̵� �߻� �Ҹ�
                StartCoroutine(Shot_Coroutine(2));            // �߻� �ڷ�ƾ ����(����̵�)
            }
        }
    }



    ////////////////////////  �ڷ�ƾ ����...........

    IEnumerator StopPlayer()     // �÷��̾� ������ ��� ����
    {
        isRun = true;             // ī�޶� ��鸮�� �ڷ�ƾ�� ����ǰ� �ִٰ� �˸�
        action?.Invoke();         // �κ�ũ�� �˸�(ī�޶� ��鸮�� �Ÿ� ����)

        AudioMng.ins.PlayEffect("HitArrow");    // ȭ�쿡 �´� �Ҹ�
        yield return delay_Camera;     // 2�� ���� ���� ������..
        isRun = false;            // �ڷ�ƾ�� �����ٰ� �˸�
    }



    IEnumerator Shot_Coroutine(int weaponNum)      // 0 : �վ, 1 : ���� ��ź, 2 : ����̵�
    {
        GameObject t_object;
        Image tempImage;
        float tempFloat;

        if (weaponNum.Equals(0))       // �վ
        {
            isRun_BreakThrough = true;          // ���� �ڷ�ƾ �������̴�, �վ�� �߻��� �� ���ٰ� �˸�
            t_object = mini05_Spawn.GetQueue_BreakThrough();      // �վ ������Ʈ Ǯ������ ������
            tempImage = BreakThrough_Image;                       // ������ �Ʒ��� �վ �̹����� ��� ��´�.
            tempFloat = 0.8f;                                     // �վ ��� �ð��� ��´�.
        }
        else if (weaponNum.Equals(1))   // ���� ��ź
        {
            isRun_DonutBomb = true;          // ���� �ڷ�ƾ �������̴�, ���� ��ź�� �߻��� �� ���ٰ� �˸�
            t_object = mini05_Spawn.GetQueue_DonutBomb();      // ������ź ������Ʈ Ǯ������ ������
            tempImage = DonutBomb_Image;                       // ������ �Ʒ��� ���� ��ź �̹����� ��� ��´�.
            tempFloat = 5.0f;                                  // ���� ��ź ��� �ð��� ��´�.
        }
        else                            // ����̵�
        {
            isRun_Tonedo = true;          // ���� �ڷ�ƾ �������̴�, ����̵��� �߻��� �� ���ٰ� �˸�
            t_object = mini05_Spawn.GetQueue_Tonedo();      // ����̵� ������Ʈ Ǯ������ ������
            tempImage = Tonedo_Image;                       // ������ �Ʒ��� ����̵� �̹����� ��� ��´�.
            tempFloat = 10.0f;                              // ����̵� ��� �ð��� ��´�.
        }

        t_object.transform.position = bulletPos.position;     // �߻�뿡�� ����
        t_object.transform.rotation = bulletPos.rotation;     // ���� �߻�� ȸ������ ����

        Rigidbody bulletRigid = t_object.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.right * 100.0f;      // �߻�뿡 ������?���� �߻�


        tempImage.fillAmount = 0.0f;     // �վ ������ ��ư �̹���
        while (tempImage.fillAmount <= 0.9999f)          // 1�� �ϸ� �ȵȴ�;;;
        {
            tempImage.fillAmount += Time.deltaTime / tempFloat;      // ��� �ð� ��� �ɷ� ���ð��� ���.

            yield return null;
        }

        if (weaponNum.Equals(0))        // �վ
        {
            isRun_BreakThrough = false;          // ���� �ڷ�ƾ �������̴�, �վ�� �߻��� �� ���ٰ� �˸�
        }
        else if (weaponNum.Equals(1))        // ���� ��ź
        {
            isRun_DonutBomb = false;          // ���� �ڷ�ƾ �������̴�, ���� ��ź�� �߻��� �� ���ٰ� �˸�
        }
        else                            // ����̵�
        {
            isRun_Tonedo = false;          // ���� �ڷ�ƾ �������̴�, ����̵��� �߻��� �� ���ٰ� �˸�
        }

        yield return null;
    }


    ////////////////////////////// Ʈ���� ����....

	void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag(tag01))       // ȭ�쿡 �¾��� ���....
        {
            if (isRun.Equals(true))           // ī�޶� ��鸮�� �ڷ�ƾ�� ����ǰ� ������...
            {
                StopCoroutine(coroutine05);          // ����ǰ� �ִ� �ڷ�ƾ �ߴ�
            }
            coroutine05 = StartCoroutine(StopPlayer());     // ī�޶� ��鸮�� �ڷ�ƾ ����
        }
    }
}
