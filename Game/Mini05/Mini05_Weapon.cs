using System.Collections;
using UnityEngine;

public class Mini05_Weapon : MonoBehaviour         // �վ, ������ź, ����̵��� �پ� �ִ�.
{
	public Mini05_Spawn mini05_Spawn;      // ���� ��ũ��Ʈ���� �� ������ ����Ʈ�� �����;� �ؼ� public���� �д�.

	int weaponInt = 0;

	Rigidbody rigid;                
	Collider col;
	Renderer render_ChildD;
	Collider col_ChildD;
	GameObject child_Donut;

	string invoke_Text01;
	string invoke_Text02;

	float delayFloat;
	WaitForSeconds delay;

	void Awake()
	{
		invoke_Text01 = "Invoke_Weapon";

		if (transform.CompareTag("Spring"))         // �վ
		{
			weaponInt = 0;                          // �վ�̶�� �˸���.
			delayFloat = 2.0f;
		}
		else if (transform.CompareTag("Bear"))      // ���� ��ź
		{
			invoke_Text02 = "Invoke_Donut";

			weaponInt = 1;                          // ���� ��ź�̶�� �˸���.
			col = transform.GetComponent<Collider>();
			render_ChildD = transform.GetChild(0).GetComponent<Renderer>();
			col_ChildD = transform.GetChild(0).GetComponent<Collider>();
			child_Donut = transform.GetChild(1).gameObject;

			delayFloat = 4.0f;
		}
		else                                         // ����̵�
		{
			weaponInt = 2;                          // ����̵���� �˸���.
			delayFloat = 6.0f;
		}

		delay = new WaitForSeconds(delayFloat);
		rigid = GetComponent<Rigidbody>();            // �� ������ ������ٵ��� �޴´�.
	}

	void OnEnable()        // Ȱ��ȭ �ɋ�...
	{
		if (weaponInt.Equals(0))
		{
			StartCoroutine(Coroutine_0());
		}
		else if (weaponInt.Equals(1))
		{
			StartCoroutine(Coroutine_1());
		}
		else
		{
			StartCoroutine(Coroutine_2());
		}
	}

	void OnDisable()        // ��Ȱ��ȭ �ɶ�...
	{
		if (weaponInt.Equals(1))    // ���� ��ź�϶�...
		{
			render_ChildD.enabled = true;     // ���� ��ź �ڽ��� ����(�� ��ź ����)�� Ų��(���� ��ź�� �����ϸ� ��ź ������ ������...)
			col.enabled = true;               // ���� ��ź�� �ݶ��̴�(�⺻ ��ź ������)�� Ų��.(���� ��ź�� �����ϸ� �⺻ ��ź �ݶ��̴��� ������...)
			child_Donut.SetActive(false);     // ���� ��ź�� �ڽ��� �ڽ�?(��ƼŬ)�� ����.
			col_ChildD.enabled = false;       // ���� ��ź�� �ڽ� �ݶ��̴��� ����.(ū ������ ����)
		}

		rigid.velocity = Vector3.zero;   // Ȥ�� �𸣴� ���ν�Ƽ�� ���η� �����.
	}



	//////////////// Ʈ���� ����....

	void OnTriggerEnter(Collider other)        
	{
		if (other.gameObject.layer.Equals(3))        // �ٴ�, ����, ������ ����� ���    WALL
		{
			if (weaponInt.Equals(0))           // �վ
			{
				mini05_Spawn.InsertQueue_BreakThrough(transform.gameObject);
			}
			else if (weaponInt.Equals(1))      // ���� ��ź
			{
				rigid.velocity = Vector3.zero;     // �ϴ� ����
				render_ChildD.enabled = false;     // ��ź ���� ����
				col.enabled = false;               // �⺻ ��ź �ݶ��̴��� ��
				child_Donut.SetActive(true);       // ��ƼŬ�̶� ū ��ź Ŵ
				col_ChildD.enabled = true;         // ū ��ź �ݶ��̴� Ŵ

				AudioMng.ins.PlayEffect("Bomb");    // ��ź ������ �Ҹ�

				CancelInvoke(invoke_Text01);    // �ð� ��� �ڷ�ƾ ��
				Invoke(invoke_Text02, 1.0f);    // ���� ��ź �ڷ�ƾ Ŵ
			}
		}
		else if (other.gameObject.layer.Equals(7))          // ���Ϳ� ����� ���(�⺻ ������ ��ź)
		{
			if (weaponInt.Equals(0))          // �վ
			{
				mini05_Spawn.InsertQueue_BreakThrough(transform.gameObject);
			}
			else if (weaponInt.Equals(1))     // ���� ��ź
			{
				rigid.velocity = Vector3.zero;     // �ϴ� ����
				render_ChildD.enabled = false;     // ���� ��ź ������ �����.
				col.enabled = false;               // �⺻ ��ź �ݶ��̴��� ��
				child_Donut.SetActive(true);       // ū ������ ��ź ��ƼŬ�� Ų��.
				col_ChildD.enabled = true;         // ū ��ź �ݶ��̴� Ŵ 

				AudioMng.ins.PlayEffect("Bomb");    // ��ź ������ �Ҹ�

				CancelInvoke(invoke_Text01);     // �ð� ��� �ڷ�ƾ ��
				Invoke(invoke_Text02, 1.0f);      // ���� ��ź �ڷ�ƾ Ŵ
			}

			other.gameObject.SetActive(false);        // ���͸� ���ش�.
		}

	}


	void OnTriggerStay(Collider other)          // ���� ��ź��...
	{
		if (weaponInt.Equals(1))     // ���� ��ź
		{
			if (other.gameObject.layer.Equals(7))     // ���Ͱ� ū ��ź �ȿ� �ִٸ�...
			{
				other.gameObject.SetActive(false);   // ���͸� ����
			}
		}
	}



	////////////////////////// �κ�ũ ����...

	void Invoke_Donut()
	{
		mini05_Spawn.InsertQueue_DonutBomb(transform.gameObject);       // ���� �ð��� ������ ���� ��ź �ݳ�
	}


	IEnumerator Coroutine_0()
	{
		yield return delay;
		mini05_Spawn.InsertQueue_BreakThrough(transform.gameObject);      // �վ �ݳ�
	}


	IEnumerator Coroutine_1()
	{
		yield return delay;
		mini05_Spawn.InsertQueue_DonutBomb(transform.gameObject);         // ���� ��ź �ݳ�
	}


	IEnumerator Coroutine_2()
	{
		yield return delay;
		mini05_Spawn.InsertQueue_Tonedo(transform.gameObject);            // ����̵� �ݳ�
	}

}
