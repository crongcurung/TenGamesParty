using UnityEngine;

public class Mini01_Potion : MonoBehaviour
{
	[SerializeField] float speedRot = 10.0f;     // ȸ���� �ӵ��� �޴� ����(������ ���� ȸ�� �ӵ��� �ٸ� �� �־ �ø��� ������ �ʵ带 ��...)

	public Mini01_Spawn mini01_Spawn;
	public Transform itemPos_This;

	void Update()
	{
		ItemRot();
	}

	void ItemRot()               // �������� ȸ���� ����ϴ� �Լ�
	{
		transform.Rotate(0, Time.deltaTime * speedRot, 0);
	}


	public void Spawn_Potion()             // �÷��̾��ʿ��� ������ �� �ְ� public���� ��
	{
		int randInt = Random.Range(0, 3);          // ���� ���ڸ� �ް���
		GameObject potion;                         // ������ ������ �޴� ����

		switch (randInt)
		{
			case 0:          // ���� ���ڰ� 0�̶��..
				potion = mini01_Spawn.GetQueue_Potion(0);     // ���� ������ ť���� �����´�.
				break;
			case 1:          // ���� ���ڰ� 1�̶��..
				potion = mini01_Spawn.GetQueue_Potion(1);     // �� ������ ť���� �����´�.
				break;
			default:         // ���� ���ڰ� 2�̶��..
				potion = mini01_Spawn.GetQueue_Potion(2);     // ���ǵ� ������ ť���� �����´�.
				break;
		}

		randInt = Random.Range(0, itemPos_This.childCount);                   // ������ ������ ��ġ�� �������� �����´�.
		potion.transform.position = itemPos_This.GetChild(randInt).position;
	}
}
