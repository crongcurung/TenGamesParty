using System.Collections.Generic;
using UnityEngine;

public class Mini09_Spawn : MonoBehaviour
{
    [SerializeField] GameObject tornado;
    [SerializeField] GameObject coin;

    [SerializeField] Transform[] Tornado_Pos;                // ����̵� ��ġ(�� 100��)
    [SerializeField] Transform[] Coin_Pos;                   // ���� ��ġ(�� 300��)

    GameObject prefab;          // ������ �Ŵ������� ������Ʈ�� �޾ƿ� ����

    List<GameObject> list_Tornado = new List<GameObject>();    // ����̵� ������Ʈ Ǯ��
    List<GameObject> list_Coin = new List<GameObject>();       // ���� ���� ������Ʈ Ǯ��


    void Awake()
    {
        prefab = tornado;             // ����̵� ������Ʈ..
        for (int i = 0; i < 100; i++)       // ����̵� 100�� ����
        {
            GameObject p_object = Instantiate(prefab);  // ����̵� ����
            list_Tornado.Add(p_object);      
        }

		prefab = coin;               // ���� ���� ������Ʈ..
		for (int i = 0; i < 300; i++)       // ���� ���� ����
		{
			GameObject p_object = Instantiate(prefab);  // ���� ���� ����
            list_Coin.Add(p_object);      
        }
	}

    void Start()
    {
		for (int i = 0; i < 100; i++)  // ����̵� 100�� ��ġ ����
		{
            list_Tornado[i].transform.position = Tornado_Pos[i].position; 
        }

		for (int i = 0; i < 300; i++)  // ���� ���� 300�� ��ġ ����
		{
			list_Coin[i].transform.position = Coin_Pos[i].position;
		}
	}

}
