using UnityEngine;

public class Mini10_Cage : MonoBehaviour    // �������� ù������ ������ ������
{
	bool isStart = false;       // 1�� �Ŀ� �������Ͱ� ���ư��� ���� ����

	void Start()
	{
		Invoke("Invoke_Start", 1.0f);      // �κ�ũ �Լ� 1�� �Ŀ� ����
	}

	void Invoke_Start()
	{
		isStart = true;        // ���� ������Ʈ �����Ѵ�.
	}


	void Update()      
	{
		if (isStart.Equals(true))       // 1�� �Ŀ� ����
		{
			transform.position += Vector3.down * Time.deltaTime;   // ������ �Ʒ��� �� �������� �Ѵ�.

			if (transform.localPosition.z <= -1.0f)      // ������ �� ���ϋ����� �������ٸ�..
			{
				Destroy(transform.gameObject);            // �� ������ ����..
			}
		}
	}
}
