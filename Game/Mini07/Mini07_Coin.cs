using UnityEngine;

public class Mini07_Coin : MonoBehaviour       // ���ο� ������
{
	public Mini07_Spawn mini07_Spawn;      // ���� ��ũ��Ʈ(�����Ҷ� �̸� �޾ƿ´�...)

    [SerializeField] protected float speedRot = 10.0f;     // ȸ���� �ӵ��� �޴� ����(������ ���� ȸ�� �ӵ��� �ٸ� �� �־ �ø��� ������ �ʵ带 ��...)


    void Update()
    {
        ItemRot();
    }

    void ItemRot()               // �������� ȸ���� ����ϴ� �Լ�
    {
        transform.Rotate(0, Time.deltaTime * speedRot, 0);
    }



    public void End_Area()
    {
        mini07_Spawn.InsertQueue_Coin(transform.gameObject);       // ������ �ݳ��Ѵ�..
    }



 //   void OnDisable()          // ��Ȱ��ȭ�� �÷��̾�� �Ѵ�.
	//{
	//	mini07_Spawn.InsertQueue_Coin(transform.gameObject);       // ������ �ݳ��Ѵ�..
	//}
}
