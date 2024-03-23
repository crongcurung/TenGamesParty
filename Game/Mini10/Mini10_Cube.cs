using System.Collections;
using UnityEngine;
using TMPro;

public class Mini10_Cube : MonoBehaviour         // �������� �ȿ� ����(��ŸƮ, ���� ����) ��ο� �����Ǿ� ����
{
	[SerializeField] Mini10_End mini10_End;

	TextMeshPro textMesh;                        // �� ���� ���� ���� ���ڸ� �ޱ� ���� ����
    int thisCount;                               // ���� ���ڸ� �����ϱ� ���� ����

	Renderer render;                             // ���� ť�� ������Ʈ �������� ����
	Rigidbody rigid;

	WaitForSeconds delay;                        // �ڷ�ƾ ����ȭ

	Color color_0;
	Color color_1;
	Color color_2;
	Color color_3;
	Color color_4;
	Color color_5;
	Color color_6;
	Color color_7;
	Color color_8;
	Color color_9;


	void Start()
    {
		render = GetComponent<Renderer>();                                  // ť���� ������ �޾ƿ�
		textMesh = transform.GetChild(0).GetComponent<TextMeshPro>();       // �� ���� �ڽĿ� �ִ� ���� ������Ʈ�� �����´�.
        thisCount = int.Parse(textMesh.text);       // ó�� ������ ���ڸ� �޾ƿ�


		switch (thisCount)        // ó�� ������ ���ڿ� ���� ���׸����� �޾ƿ�
		{
			case 1:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				break;
			case 2:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				break;
			case 3:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				break;
			case 4:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				color_4 = new Color(180f / 255f, 149f / 255f, 173f / 255f);
				break;
			case 5:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				color_4 = new Color(180f / 255f, 149f / 255f, 173f / 255f);
				color_5 = new Color(157f / 255f, 142f / 255f, 180f / 255f);
				break;
			case 6:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				color_4 = new Color(180f / 255f, 149f / 255f, 173f / 255f);
				color_5 = new Color(157f / 255f, 142f / 255f, 180f / 255f);
				color_6 = new Color(158f / 255f, 186f / 255f, 232f / 255f);
				break;
			case 7:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				color_4 = new Color(180f / 255f, 149f / 255f, 173f / 255f);
				color_5 = new Color(157f / 255f, 142f / 255f, 180f / 255f);
				color_6 = new Color(158f / 255f, 186f / 255f, 232f / 255f);
				color_7 = new Color(110f / 255f, 165f / 255f, 183f / 255f);
				break;
			case 8:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				color_4 = new Color(180f / 255f, 149f / 255f, 173f / 255f);
				color_5 = new Color(157f / 255f, 142f / 255f, 180f / 255f);
				color_6 = new Color(158f / 255f, 186f / 255f, 232f / 255f);
				color_7 = new Color(110f / 255f, 165f / 255f, 183f / 255f);
				color_8 = new Color(60f / 255f, 195f / 255f, 123f / 255f);
				break;
			default:
				color_1 = new Color(180f / 255f, 180f / 255f, 144f / 255f);
				color_2 = new Color(176f / 255f, 160f / 255f, 143f / 255f);
				color_3 = new Color(200f / 255f, 163f / 255f, 165f / 255f);
				color_4 = new Color(180f / 255f, 149f / 255f, 173f / 255f);
				color_5 = new Color(157f / 255f, 142f / 255f, 180f / 255f);
				color_6 = new Color(158f / 255f, 186f / 255f, 232f / 255f);
				color_7 = new Color(110f / 255f, 165f / 255f, 183f / 255f);
				color_8 = new Color(60f / 255f, 195f / 255f, 123f / 255f);
				color_9 = new Color(45f / 255f, 140f / 255f, 89f / 255f);
				break;
		}

		color_0 = new Color(255f / 255f, 255f / 255f, 255f / 255f);
		rigid = GetComponent<Rigidbody>();
		Cube_Number(thisCount);          // ť�� ���׸��� ����     

		delay = new WaitForSeconds(0.25f);             // �ڷ�ƾ ����ȭ
	}


	public int StepPlayer()        // �� ������ ����� ��, ���� �����ϴ� �Լ�(public���� �ּ� �÷��̾�� ���ٰ����ϵ��� ��)
    {
		if (thisCount.Equals(0))       // �� ť���� ���ڰ� 0�̸� ����
		{
			return 0;
		}


		thisCount--;       // ���ڸ� ���δ�.
		textMesh.text = thisCount.ToString();     // ����ȯ�ؼ� ���ڷ� ������

		Cube_Number(thisCount);     // ť�� ���׸��� ����

		if (thisCount.Equals(0))                   // �پ�� ���ڰ� 0�̶��..
		{
			mini10_End.fallCount++;                 // ť�갡 �����ٰ� �ص忡 �˸�

			StartCoroutine(FallingCoroutine());   // �� ������ �����̴ٰ� ������ �ڷ�ƾ�� ����
        }

		return thisCount;      // �پ�� ���ڸ� ��ȯ�Ѵ�.
	}


	void Cube_Number(int num)          // ť�� ���׸��� ����
	{
		switch (num)
		{
			case 0:
				render.material.color = color_0;
				break;
			case 1:
				render.material.color = color_1;
				break;
			case 2:
				render.material.color = color_2;
				break;
			case 3:
				render.material.color = color_3;
				break;
			case 4:
				render.material.color = color_4;
				break;
			case 5:
				render.material.color = color_5;
				break;
			case 6:
				render.material.color = color_6;
				break;
			case 7:
				render.material.color = color_7;
				break;
			case 8:
				render.material.color = color_8;
				break;
			case 9:
				render.material.color = color_9;
				break;
		}
	}


	void CubeFall(int randInt)                        // �������� ���� ���� ���ϴ� �Լ�
	{
		if (randInt.Equals(0))                        //  ----------------- �������� ���� ���ڷ� �������� ������ ����
		{
			transform.rotation = Quaternion.Euler(20, 0, 20);
		}
		else if (randInt.Equals(1))
		{
			transform.rotation = Quaternion.Euler(-20, 0, 20);
		}
		else if (randInt.Equals(2))
		{
			transform.rotation = Quaternion.Euler(20, 0, -20);
		}
		else if (randInt.Equals(3))
		{
			transform.rotation = Quaternion.Euler(-20, 0, -20);
		}
	}

	/////////////////////////////////
	// �ڷ�ƾ ����...

	IEnumerator FallingCoroutine()        // ť�갡 0�̶�� �������� �Ÿ��� �ڷ�ƾ
	{
		render.enabled = false;           // �����̱�...
		textMesh.enabled = false;
		yield return delay;               // 0.25�� ����...

		render.enabled = true;
		textMesh.enabled = true;
		yield return delay;

		render.enabled = false;
		textMesh.enabled = false;
		yield return delay;

		render.enabled = true;
		textMesh.enabled = true;
		yield return delay;

		rigid.isKinematic = false;              // �������� ���� �޴´ٰ� �˸���.
		rigid.AddForce(Vector3.down * 500.0f);  // ������ ����Ʈ����...
		gameObject.layer = 1;                  // ���̾ �ٲ㼭 �÷��̾ ����ȵǵ��� ��

		int randInt = Random.Range(0, 4);       // ��� ������ ����Ʈ���� ���� ����
		CubeFall(randInt);                      // ť�� ���� ����ħ

		yield return new WaitForSeconds(3.0f);         // 3�� ��, ��Ȱ��ȭ

		transform.gameObject.SetActive(false);
	}
}
