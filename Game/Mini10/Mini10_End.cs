using UnityEngine;

public class Mini10_End : MonoBehaviour          // �ص� ť�꿡 ������
{
	public int fallCount;      // �� �������������� ������ ������ �� ����
	int stageCount;            // �� �������������� ������ �� ����

	void Start()
	{
		stageCount = transform.parent.childCount - 2;      // �� ���������� ť�� ���ڸ� �޾ƿ�
	}


	public bool CheckEnd()          // �÷��̾ �ص� ť�꿡 ������ ����
	{
		if (stageCount.Equals(fallCount))      // ������ ť�� ���ڿ� �� ���������� ť�� ���ڰ� ���ٸ�..
		{
			return true;         // true �Ѱ�
		}

		return false;          // ������ ť�� ���ڿ� �� ���������� ť�� ���ڰ� �ٸ��ٸ�..
	}
}
