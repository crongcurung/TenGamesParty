using UnityEngine;

public class Mini01_FixedObj : MonoBehaviour
{
    [SerializeField] float speedRot = 10.0f;     // ȸ���� �ӵ��� �޴� ����(������ ���� ȸ�� �ӵ��� �ٸ� �� �־ �ø��� ������ �ʵ带 ��...)

    void Update()
    {
        ItemRot();
    }


    void ItemRot()               // �������� ȸ���� ����ϴ� �Լ�
    {
        transform.Rotate(0, Time.deltaTime * speedRot, 0);
    }
}
