using System.Collections.Generic;
using UnityEngine;

public class Mini08_Spawn : MonoBehaviour                        // 묘지 부모에 부착됨
{
    [SerializeField] GameObject ghost;

    [SerializeField] Transform playerTrans;                   // 플레이어를 받는다.
    [SerializeField] Mini08_Player mini08_Player;

    GameObject prefab;          // 프리팹 매니져에서 오브젝트를 받아올 변수

    Queue<GameObject> queue_Chost = new Queue<GameObject>();       // 유령 오브젝트 풀링

    void Awake()
    {
        prefab = ghost;               // 유령 오브젝트..
        for (int i = 0; i < 60; i++)       // 유령 20정도 생성 및 보관
        {
            GameObject p_object = Instantiate(prefab);  // 유령 생성
            Mini08_Monster mini08_Monster = p_object.GetComponent<Mini08_Monster>();
            mini08_Monster.mini08_Spawn = this;   // 스폰 스크립트를 넘긴다.
            mini08_Monster.player = playerTrans;  // 플레이어를 넘긴다.
            mini08_Monster.mini08_Player = mini08_Player;
            queue_Chost.Enqueue(p_object);       // 오브젝트 풀링
        }
    }

    public void InsertQueue_Chost(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(바닥)
    {
        queue_Chost.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Chost()           // 큐에서 객체를 빌려오는 함수(바닥)
    {
		if (queue_Chost.Count.Equals(0))         // 만약 큐에 유령에 하나도 없다면..
		{
			return null;   // null를 넘긴다.
		}

        GameObject t_object = queue_Chost.Dequeue();
        t_object.SetActive(true);
        return t_object;
    }
}
