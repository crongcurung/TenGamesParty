using System.Collections;
using UnityEngine;

public class Mini08_Cemetry : MonoBehaviour          // ������ ������
{
	[SerializeField] Mini08_Player mini08_Player;    // �÷��̾� ��ũ��Ʈ�� �����´�.
	[SerializeField] Mini08_Spawn mini08_Spawn;      // ���� ��ũ��Ʈ�� �����´�.
	[SerializeField] Mesh[] Cemetry_Mesh;            // ���� ���� ��ȭ�� ���� �޽��� �����´�(�� 5��)

	[SerializeField] SpriteRenderer spriteRander;
	[SerializeField] Sprite[] sprite_Array;

    MeshFilter meshFilter;                           // �� ������ ���¸� ��ȭ���� �޽� ������Ʈ

	[SerializeField] int thisInt;                    // �� ������ ��ȣ(������ �����ۿ� ���� �����Ǿ������� ����...)

	Coroutine coroutine;                             // ���� ��ȭ �ڷ�ƾ�� ���� ����

	int meshInt = 0;                                 // ���� ��ȭ�� ���� ��ȣ �ο� ����

	WaitForSeconds delay01;
	WaitForSeconds delay02;


	Transform top_Child;

	Vector3 oriPos;
	Vector3 pos05;

	Quaternion[] Rot_Array = new Quaternion[5];

	Coroutine ghost_Coroutine;

	void Awake()
    {
		top_Child = transform.GetChild(0).transform;

		oriPos = new Vector3(0, 1.976761f, 1.673394f);

		pos05 = new Vector3(-3.64f, 0.94f, 1.673396f);


		Rot_Array[0] = top_Child.localRotation;
		Rot_Array[1] = Quaternion.Euler(new Vector3(-90, 10, 0));
		Rot_Array[2] = Quaternion.Euler(new Vector3(-90, 20, 0));
		Rot_Array[3] = Quaternion.Euler(new Vector3(-90, 30, 0));
		Rot_Array[4] = Quaternion.Euler(new Vector3(-30.922f, -90, 90));

		meshFilter = transform.GetChild(2).GetComponent<MeshFilter>();
		delay01 = new WaitForSeconds(20.0f);
		delay02 = new WaitForSeconds(30.0f);
	}



	void OnEnable()          // ���� ��..
	{
		mini08_Player.CemetryText_Fuction(1);                 // ���� �ؽ�Ʈ�� �ϳ� �ø���.
		coroutine = StartCoroutine(Chage_Coroutine());        // ���� ��ȭ �ڷ�ƾ�� �����Ų��..
	}

	void OnDisable()         // ���� ��...
	{
		mini08_Player.CemetryText_Fuction(-1);              // ������ �ϳ� �پ��ٰ� �˸���.
		mini08_Player.Cemetry_Minus(thisInt);               // ���� ����Ʈ�� �ִ´�.(���� ��ų�� ���� ��)

		meshInt = 0;                                        // �޽� �ʱ�ȭ�Ѵ�.
		meshFilter.sharedMesh = Cemetry_Mesh[meshInt];      // �޽��� �ʱ�ȭ�Ѵ�.
		spriteRander.sprite = sprite_Array[meshInt];

		top_Child.localPosition = oriPos;
		top_Child.localRotation = Rot_Array[0];
	}


	public int Check_Cemetry()     // ���� ���� Ȯ��
	{
		if (meshInt.Equals(0))     // ���� �� ������ ���� ��ȭ�� ���ؼ� �׳� �����ع���
		{
			return 10;             // �ƹ� ���ڳ� �����ϸ� �ȴ�.
		}
		else
		{
			return meshInt;                // �Ʊ� ��Ƶ״� ���ڸ� �ѱ��.(���ڿ� ���� ��� �ڿ��� �ٸ��� ������...)
		}
	}


	public void Player_Fixed()      // �÷��̾ �� ������ ��ġ�� ���̶��..
	{
		if (meshInt.Equals(4))     // ���� �� ������ ���°� ���� �ı� ���¿��ٸ�...
		{
			StopCoroutine(ghost_Coroutine);
			mini08_Player.BrokenText_Fuction(-1);   // �μ��� �ؽ�Ʈ�� �ϳ� ���δ�..
		}

		StopCoroutine(coroutine);  // ���� ��ȭ �ڷ�ƾ�� �ߴܽ�Ų��.

		int tempInt = meshInt;         // ���� ���¸� ��� ��Ƶΰ�
		meshInt = 0;                   // ���� ���¸� �ʱ�ȭ�Ѵ�..
		meshFilter.sharedMesh = Cemetry_Mesh[meshInt];      // �ʱ�ȭ ���·� ���¸� �ٲ�
		spriteRander.sprite = sprite_Array[meshInt];

		top_Child.localRotation = Rot_Array[meshInt];
		top_Child.localPosition = oriPos;

		coroutine = StartCoroutine(Chage_Coroutine());      // �ٽ� ���� ��ȭ �ڷ�ƾ ����
	}



	/////////////////////////  �ڷ�ƾ ����..


	IEnumerator Chage_Coroutine()
	{
		while (!meshInt.Equals(4))         // ������ �μ��� ���°� �ƴ϶��...
		{
			yield return delay01;          // 20�ʰ� �� ����..
			meshInt++;                     // ���¸� ��ȭ�Ѵ�..
			meshFilter.sharedMesh = Cemetry_Mesh[meshInt];
			spriteRander.sprite = sprite_Array[meshInt];
			top_Child.localRotation = Rot_Array[meshInt];

			if (meshInt.Equals(4))         // ���� �μ��� ���¶��..
			{
				AudioMng.ins.LoopEffect(false);    // ��, �� �Ҹ� ���ѷ��� ����
				AudioMng.ins.PlayEffect("Cemetry");    // ���� �Ҹ�
				top_Child.localPosition = pos05;

				ghost_Coroutine = StartCoroutine(Ghost_Coroutine());      // ���� ���� �ڷ�ƾ�� �����Ѵ�...
				mini08_Player.BrokenText_Fuction(1);    // �μ��� �ؽ�Ʈ�� �ϳ� �ø���.
			}
		}
	}


	IEnumerator Ghost_Coroutine()      // ���� ���� �ڷ�ƾ..
	{
		while (true)
		{
			GameObject obj = mini08_Spawn.GetQueue_Chost();    // ���� �ϳ��� �����´�.

			if (obj != null)      // null �̸� 60���� �̻��̶�� ��!!!
			{
				obj.transform.position = transform.position;    // ������ �� ���� ��ġ�� �����´�.
			}

			yield return delay02;       // 30�� �� ����.
		}
	}
}
