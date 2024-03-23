using System.Collections.Generic;
using UnityEngine;

public class Mini08_Spawn : MonoBehaviour                        // ���� �θ� ������
{
    [SerializeField] GameObject ghost;

    [SerializeField] Transform playerTrans;                   // �÷��̾ �޴´�.
    [SerializeField] Mini08_Player mini08_Player;

    GameObject prefab;          // ������ �Ŵ������� ������Ʈ�� �޾ƿ� ����

    Queue<GameObject> queue_Chost = new Queue<GameObject>();       // ���� ������Ʈ Ǯ��

    void Awake()
    {
        prefab = ghost;               // ���� ������Ʈ..
        for (int i = 0; i < 60; i++)       // ���� 20���� ���� �� ����
        {
            GameObject p_object = Instantiate(prefab);  // ���� ����
            Mini08_Monster mini08_Monster = p_object.GetComponent<Mini08_Monster>();
            mini08_Monster.mini08_Spawn = this;   // ���� ��ũ��Ʈ�� �ѱ��.
            mini08_Monster.player = playerTrans;  // �÷��̾ �ѱ��.
            mini08_Monster.mini08_Player = mini08_Player;
            queue_Chost.Enqueue(p_object);       // ������Ʈ Ǯ��
        }
    }

    public void InsertQueue_Chost(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(�ٴ�)
    {
        queue_Chost.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Chost()           // ť���� ��ü�� �������� �Լ�(�ٴ�)
    {
		if (queue_Chost.Count.Equals(0))         // ���� ť�� ���ɿ� �ϳ��� ���ٸ�..
		{
			return null;   // null�� �ѱ��.
		}

        GameObject t_object = queue_Chost.Dequeue();
        t_object.SetActive(true);
        return t_object;
    }
}
