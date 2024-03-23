using UnityEngine;

public class Mini01_Donut : MonoBehaviour       // ��Ʈ ���ӿ� ������
{
    [SerializeField] Transform itemPos;                   // �������� ���� ���� ��ġ �Ǵ� Ư�� ��򰡿� �����ϱ� ������ ���� ��ġ�� �ޱ� ���� ����
    [SerializeField] float speedRot = 10.0f;     // ȸ���� �ӵ��� �޴� ����(������ ���� ȸ�� �ӵ��� �ٸ� �� �־ �ø��� ������ �ʵ带 ��...)
    [SerializeField] Mini01_Spawn mini01_Spawn;

    int randInt = 0;
    int prevInt = 0;           // ��� ��ġ�� ���Դ� �������� ���� ����

    void Start()
    {
        ResetItemPos();      // ó������ ������ ����� ������ ����
    }

    void Update()
    {
        ItemRot();
    }

    void ItemRot()               // �������� ȸ���� ����ϴ� �Լ�
    {
        transform.Rotate(0, Time.deltaTime * speedRot, 0);
    }


    public void ResetItemPos()             // �÷��̾��ʿ��� ������ �� �ְ� public���� ��
    {
        randInt = Random.Range(0, itemPos.transform.childCount);        // �������� ��Ʈ ���� ��ġ�� �ٲ۴�.


        while (prevInt.Equals(randInt))
        {
            randInt = Random.Range(0, itemPos.transform.childCount);        // �������� ��Ʈ ���� ��ġ�� �ٲ۴�.
        }

        prevInt = randInt;

        transform.position = itemPos.transform.GetChild(randInt).transform.position;   // ������ ��ġ�� �ٲ�
        mini01_Spawn.GetQueue_Monster();       // ��Ʈ ������ ������ ���Ͱ� �������� �Ѵ�.
    }
}
