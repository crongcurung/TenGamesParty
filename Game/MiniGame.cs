using UnityEngine;

public class MiniGame : MonoBehaviour            // �̴ϰ��� ���� ó�� ������Ʈ �����Ǿ� ����
{
	[SerializeField] GameObject[] Stage;         // �̴ϰ��� �����յ��� ��´�.

	void Awake()
	{
		Time.timeScale = 1;                      // �� ���� �ϴ� ��� �������� 0�� ���� �־ ���⼭ 1�� �Ѵ�.

		int num = Main.ins.MiniStageNum();       // ����� �̴ϰ����� ������ �;��ϴ��� ��´�.

		GameObject prefab = Stage[num];          // �̴ϰ��� �������� ��´�.

		Instantiate(prefab, Vector3.zero, Quaternion.identity);    // �̴ϰ��� �������� ��Ƴ��´�.
	}

}
