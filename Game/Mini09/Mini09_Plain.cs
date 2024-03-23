using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini09_Plain : MonoBehaviour       // ���ư��� ����⿡ ������
{
    [SerializeField] Mini09_Camera mini09_Camera;      // ī�޶� ��ũ��Ʈ

    [SerializeField] Transform startPos;               // ó�� �÷��̾ �����ϴ� ��ġ
    [SerializeField] TextMeshProUGUI distanceText;     // �Ÿ��� ǥ���Ǵ� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI coinText;         // ���� ������ ǥ���ϴ� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI heightText;       // ���̰� ǥ���Ǵ� �ؽ�Ʈ
    [SerializeField] GameObject dirCube;               // ����� �տ� ������ �����ִ� ť��
    [SerializeField] Slider slider;                    // ���� �����̵�

    Rigidbody rigid;            // ������� ������ �ٵ�

    float angle = 0.0f;         // ������� �߻� ȸ���� �޴� ����

    bool endBool = false;       // ����Ⱑ..
    bool isFloor = false;       // ����Ⱑ �ٴڿ� ��Ҵ��� �˸��� ����
    bool isInvoke = false;      // �κ�ũ�� ����ƴ��� ���� ����

    public float distance = 0;     // �Ÿ��� ��Ÿ���� ����
    float height = 0;       // ���̸� ��Ÿ���� ����
    float temp = 0.0f;      // �ִ� ���̸� ��Ÿ���� ����(���̰� �� ���� �� ����)
    public int coinInt = 0;        // �Դ� ������ ��Ÿ���� ����

    string invoke_Text;


    Vector3 vector01;
    Vector3 vector02;

    bool isFallFail = false;

    public bool isEnd = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        invoke_Text = "Invoke_EndGame";

        vector01 = new Vector3(0, 1, 0);
        vector02 = new Vector3(0, 1, 0.3f);

    }

    void Update()
    {
        if (isEnd.Equals(true))
        {
            return;
        }


        if (transform.position.y < -50.0f)        // ����Ⱑ ��� ���� �ٴ� �Ʒ��� �������ٸ� ����
        {
            mini09_Camera.EndGame();
            StartCoroutine(End_Coroutine());
        }

        if (isFallFail.Equals(false))
        {
            Text_Fuction();                           // �ؽ�Ʈ�� �����ϴ� �Լ�
        }

        if (transform.position.y < -30.0f && isFallFail.Equals(false))        // �÷��̾ ���� �κ� �������ٸ�..
        {
            isFallFail = true;

            //AudioMng.ins.PlayEffect("Fail02");    // �ٴ� ���ۿ� ������

            mini09_Camera.EndGame();
        }

        if (endBool.Equals(false))                
        {
            transform.up = rigid.velocity;        // ����� ������ �����ֱ� ���� �̷��� �ؾ���(�ٴڿ� ������ true�� �Ǽ� ȸ���� �� ��)
        }

        if (isInvoke.Equals(true))                // �κ�ũ�� ����ǰ� �ִٸ�...
        {
            if (rigid.velocity != Vector3.zero)       // �ٽ� �����δ�!
            {
                isInvoke = false;                 // �κ�ũ�� �����ٰ� �Ѵ�.
                CancelInvoke(invoke_Text);   // �κ�ũ �ߴ�
            }
            return;     // �Ʒ� ���� ���ϰ� �Ѵ�.
        }

        if (rigid.velocity.Equals(Vector3.zero) && isFloor.Equals(true))  // �ӵ��� 0�̰�, �ٴڿ� ��� �ִٸ�..
        {
            isInvoke = true;                     // �κ�ũ�� �����Ѵ�.
            Invoke(invoke_Text, 2.0f);      // 2�� �Ŀ� ���� �����ٰ� �˸���.
        }
    }

    IEnumerator End_Coroutine()
    {
        yield return new WaitForSeconds(2.0f);
        mini09_Camera.End_Game(true);
    }

    void Invoke_EndGame()      // �ٴڿ� ���, �ӵ��� 0�̸� �����ٰ� �˸��� �κ�ũ ����
    {
        mini09_Camera.EndGame();   // ī�޶� ���� �Լ� ����
    }


    void Text_Fuction()        // �ؽ�Ʈ�� ǥ���ϴ� �Լ�
    {
        distance = transform.position.z - startPos.transform.position.z;    // �Ÿ��� ��Ÿ����.
        height = transform.position.y - startPos.transform.position.y;      // ���̸� ��Ÿ����.
        distanceText.text = distance.ToString("N0");                  // �Ÿ� �ؽ�Ʈ�� ��Ÿ����.

        if (height <= 0.0f)     // ���̰� 0 ���̸�
        {
            height = 0.0f;      // ���̸� 0���� ����
        }

        if (temp <= height)     // ���� ���̰� �� ���̺��� ���ٸ�
        {
            temp = height;              // ���� ���̸� �ִ� ���̷� �Ѵ�.
            slider.maxValue = temp;     
            heightText.text = temp.ToString("N1");   // �ִ� ���̸� �ؽ�Ʈ�� �ѱ��.
        }

        slider.value = height;        // �����̴��� ���� ���̷� �����ش�.
    }


    public void Shotting_Plain(float speedFloat, int powerInt, float angleFloat)       // �÷��̾�� �޾ƿ� ������ ����⸦ ������.
    {
        angle = 90.0f - angleFloat;          // ������ �ݴ�� �ؾ��Ѵ�...
        transform.Rotate(new Vector3(angle, 0, 0));       // ������� ������ �����.
        Vector3 dir = dirCube.transform.position - transform.position;      // ������� ������ �������� ���ϰ� �Ѵ�.
        rigid.AddForce(dir.normalized * speedFloat * (powerInt + 1.5f) * 30.0f);       // �� �Ŀ��� �߻��Ѵ�.
    }



    /////////////////////// Ʈ���� ����...

    void OnCollisionEnter(Collision collision)        // �ݶ��̴� ����
    {
		if (collision.gameObject.layer.Equals(3))     // �ٴڿ� �������
		{
			endBool = true;       // ���� �����ڰ� �˷���
			isFloor = true;       // �ٴڿ� ��Ҵٰ� �˷���

            AudioMng.ins.PlayEffect("Cloud");    // �ٴڿ� ����
        }
	}

	void OnCollisionExit(Collision collision)        // �ݶ��̴� Ż��
    {
		if (collision.gameObject.layer.Equals(3))     // �ٴڿ��� �������Դٸ�..
        {
			isFloor = false;      // �ٴ��ϰ� �� ��Ҵٰ� �˷���
		}
	}

    

    void OnTriggerEnter(Collider other)         // Ʈ���� ����
    {
        if (other.gameObject.layer.Equals(7))       // Monster
        {
            other.gameObject.SetActive(false);       // �Դ� ������ ��´ٸ�..
            coinInt++;
            coinText.text = coinInt.ToString();

            AudioMng.ins.PlayEffect("Score_Up");    // ���� ����
        }
        else if (other.gameObject.layer.Equals(8))       // Object
        {
            AudioMng.ins.PlayEffect("HitApple");    // ����̵� ����

            if (rigid.velocity.y < -50.0f && rigid.velocity.y > -75.0f)
            {
                rigid.AddForce(vector01 * 3000.0f);

                return;
            }
            else if (rigid.velocity.y <= -75.0f)
            {
                rigid.AddForce(vector01 * 3500.0f);

                return;
            }
            rigid.AddForce(vector01 * 1000.0f);

        }
        else if (other.gameObject.layer.Equals(1))
        {
            endBool = false;                 // ���� �����ڴ°� ���� �Ϸ� ��
            rigid.AddForce(vector02 * 1000.0f);

            AudioMng.ins.PlayEffect("Bomb");    // ����̵� ����
        }
    }
}
