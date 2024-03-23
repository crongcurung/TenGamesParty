using System.Collections.Generic;
using UnityEngine;

public class Mini09_Spawn : MonoBehaviour
{
    [SerializeField] GameObject tornado;
    [SerializeField] GameObject coin;

    [SerializeField] Transform[] Tornado_Pos;                // 토네이도 위치(총 100개)
    [SerializeField] Transform[] Coin_Pos;                   // 도넛 위치(총 300개)

    GameObject prefab;          // 프리팹 매니져에서 오브젝트를 받아올 변수

    List<GameObject> list_Tornado = new List<GameObject>();    // 토네이도 오브젝트 풀링
    List<GameObject> list_Coin = new List<GameObject>();       // 도넛 코인 오브젝트 풀링


    void Awake()
    {
        prefab = tornado;             // 토네이도 오브젝트..
        for (int i = 0; i < 100; i++)       // 토네이도 100개 생성
        {
            GameObject p_object = Instantiate(prefab);  // 토네이도 생성
            list_Tornado.Add(p_object);      
        }

		prefab = coin;               // 도넛 코인 오브젝트..
		for (int i = 0; i < 300; i++)       // 코인 도넛 생성
		{
			GameObject p_object = Instantiate(prefab);  // 도넛 코인 생성
            list_Coin.Add(p_object);      
        }
	}

    void Start()
    {
		for (int i = 0; i < 100; i++)  // 토네이도 100개 위치 조정
		{
            list_Tornado[i].transform.position = Tornado_Pos[i].position; 
        }

		for (int i = 0; i < 300; i++)  // 도넛 코인 300개 위치 조정
		{
			list_Coin[i].transform.position = Coin_Pos[i].position;
		}
	}

}
