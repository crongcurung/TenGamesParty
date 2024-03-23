using UnityEngine;

public class Mini06_Net : MonoBehaviour
{
    public bool isAttack = false;         // �÷��̾ ������ ��ư�� ������?

    string tag01;

	void Awake()
	{
        tag01 = "Monster";
    }

	void OnTriggerEnter(Collider other)
    {
        if (isAttack.Equals(true))          // �÷��̾ ������ ��ư�� ������? (��ư�� �� ������ ���� �ߵ� ���ϰ� �ҷ��� �̷��� ��)
        {
            if (other.gameObject.CompareTag(tag01))       // ���� ������...
            {
                other.gameObject.SetActive(false);       // ���߿� �ݳ����� �ٲ��
                AudioMng.ins.PlayEffect("Score_Up");    // �� �״� �Ҹ�
            }
        }
    }
}
