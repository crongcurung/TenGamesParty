using UnityEngine;

public class Mini01_Digged : MonoBehaviour         // ���� ������Ʈ�� ������
{
	public Mini01_Spawn mini01_Spawn;


	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.layer)         // ���� ������Ʈ�� ���̾...
		{
			case 4:                            // ���� ������                Water
				mini01_Spawn.InsertQueue_Digged(transform.gameObject);    // �� ������ ť�� �ݳ���Ų��.
				break;
			case 3:                            // �� �ٲ�ſ� ������           WALL
				mini01_Spawn.InsertQueue_Digged(transform.gameObject);    // �� ������ ť�� �ݳ���Ų��.
				break;
			case 7:                            // ���Ϳ� ������             Monster
				AudioMng.ins.PlayEffect("HitApple");    // ���Ͱ� ������ ��Ƽ� ������
				mini01_Spawn.InsertQueue_Digged(transform.gameObject);    // �� ������ ť�� �ݳ���Ų��.
				mini01_Spawn.InsertQueue_Monster(other.gameObject);      // ���� ���͸� ť�� �ݳ���Ų��.
				break;
			case 8:                            // ��ǳ�⿡ ������           Object
				mini01_Spawn.InsertQueue_Digged(transform.gameObject);    // �� ������ ť�� �ݳ���Ų��.
				break;
		}
	}
}
