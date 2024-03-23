using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mini01_Spawn : MonoBehaviour
{
	[SerializeField] GameObject knight_Prefab;

	[SerializeField] GameObject potion_H;
	[SerializeField] GameObject potion_S;
	[SerializeField] GameObject potion_V;

	[SerializeField] GameObject digg;

	[SerializeField] GameObject fixedObj;

	[SerializeField] Transform FixedPos;
	[SerializeField] Transform itemPos;

	[SerializeField] Transform playerTrans;
	[SerializeField] Mini01_Player mini01_Player;

	List<int> mon_IntList = new List<int>();
	Queue<GameObject> queue_Monster = new Queue<GameObject>();     // 몬스터 오브젝트 풀링
	
	Queue<GameObject> queue_PotionH = new Queue<GameObject>();     // 무적포션 오브젝트 풀링
	Queue<GameObject> queue_PotionV = new Queue<GameObject>();     // 삽 포션 오브젝트 풀링
	Queue<GameObject> queue_PotionS = new Queue<GameObject>();     // 스피드 포션 오브젝트 풀링

	Queue<GameObject> queue_Digged = new Queue<GameObject>();     // 함정 오브젝트 풀링

	GameObject prefab;          // 프리팹 매니져에서 오브젝트를 받아올 변수
	GameObject mon_Prefab;      // 위에 프리팹 변수에서 몬스터 프리팹으로 뽑을 변수


	void Awake()
	{
		IntList_Setting();   // 중복 방지 int 리스트 값을 설정하는 함수 실행

		Mini01_Monster mini01_Monster;
		Mini01_Potion mini01_Potion;


		prefab = knight_Prefab;
		for (int i = 0; i < 40; i++)       // 몬스터 40정도 생성 및 보관
		{
			mon_Prefab = Instantiate(prefab);// 몬스터 생성

			mini01_Monster = mon_Prefab.GetComponent<Mini01_Monster>();

			mini01_Monster.player = playerTrans;
			mini01_Monster.mini01_Player = mini01_Player;
			queue_Monster.Enqueue(mon_Prefab);       // 오브젝트 풀링
		}

		prefab = potion_H;      // 무적 포션을 넘긴다.
		GameObject potion_Prefab;   // 포션을 소환받을 변수

		for (int i = 0; i < 2; i++)            // 2개 정도 무적 포션을 가지고 온다.
		{
			potion_Prefab = Instantiate(prefab);        // 무적 포션을 스폰한다.

			mini01_Potion = potion_Prefab.GetComponent<Mini01_Potion>();

			mini01_Potion.mini01_Spawn = this;
			mini01_Potion.itemPos_This = itemPos;
			queue_PotionH.Enqueue(potion_Prefab);       // 스폰된 무적 포션을 큐에 넣는다.
		}

		prefab = potion_V;        // 삽 포션을 넘긴다.
		for (int i = 0; i < 2; i++)            // 2개 정도 삽 포션을 가지고 온다.
		{
			potion_Prefab = Instantiate(prefab);        // 삽 포션을 스폰한다.

			mini01_Potion = potion_Prefab.GetComponent<Mini01_Potion>();

			mini01_Potion.mini01_Spawn = this;
			mini01_Potion.itemPos_This = itemPos;
			queue_PotionV.Enqueue(potion_Prefab);       // 스폰된 삽 포션을 큐에 넣는다.
		}

		prefab = potion_S;        // 스피드 포션을 넘긴다.
		for (int i = 0; i < 2; i++)            // 2개 정도 스피드 포션을 가지고 온다.
		{
			potion_Prefab = Instantiate(prefab);        // 스피드 포션을 스폰한다.

			mini01_Potion = potion_Prefab.GetComponent<Mini01_Potion>();

			mini01_Potion.mini01_Spawn = this;
			mini01_Potion.itemPos_This = itemPos;
			queue_PotionS.Enqueue(potion_Prefab);       // 스폰된 스피드 포션을 큐에 넣는다.
		}


		prefab = digg;        // 함정을 넘긴다.
		for (int i = 0; i < 30; i++)            // 30개 정도 함정을 가지고 온다.
		{
			potion_Prefab = Instantiate(prefab);             // 함정을 스폰한다.
			potion_Prefab.GetComponent<Mini01_Digged>().mini01_Spawn = this;
			queue_Digged.Enqueue(potion_Prefab);             // 스폰된 함정을 큐에 넣는다.
		}

		List<int> IntList = new List<int>();

		for (int i = 0; i < FixedPos.childCount; i++)
		{
			IntList.Add(i);
		}
		

		prefab = fixedObj;        // 방해물 오브젝트.........
		for (int i = 0; i < 3; i++)            // 3
		{
			int randInt = Random.Range(0, IntList.Count);
			Instantiate(prefab, FixedPos.GetChild(IntList[randInt]).position, Quaternion.identity);             // 
			IntList.RemoveAt(randInt);
		}

		Spawn_Potion();
	}

	

	void Start()
	{
		for (int i = 0; i < 3; i++)
		{
			GetQueue_Monster();        // 미리 몬스터 세 마리를 필드에 놓는다.
		}
	}


	void IntList_Setting()        // 중복 방지 int 리스트 값을 설정하는 함수
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			mon_IntList.Add(i);    // 0부터 122까지 넣는다.
		}
	}


	void Spawn_Potion()             // 플레이어쪽에서 접근할 수 있게 public으로 함
	{
		int randInt = Random.Range(0, 3);          // 랜덤 숫자를 받고나서
		GameObject potion;                         // 프피팹 스폰을 받는 변수

		switch (randInt)
		{
			case 0:          // 랜덤 숫자가 0이라면..
				potion = GetQueue_Potion(0);     // 무적 포션을 큐에서 가져온다.
				break;
			case 1:          // 랜덤 숫자가 1이라면..
				potion = GetQueue_Potion(1);     // 삽 포션을 큐에서 가져온다.
				break;
			default:         // 랜덤 숫자가 2이라면..
				potion = GetQueue_Potion(2);     // 스피드 포션을 큐에서 가져온다.
				break;
		}

		randInt = Random.Range(0, itemPos.childCount);                   // 포션을 스폰할 위치를 랜덤으로 가져온다.
		potion.transform.position = itemPos.GetChild(randInt).position;
	}

	////////////////////////////////////////  몬스터 큐

	public void InsertQueue_Monster(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(몬스터)
	{
		queue_Monster.Enqueue(p_object);      // 몬스터을 큐에 반납시킨다.
		p_object.GetComponent<NavMeshAgent>().enabled = false;     // 이 몬스터의 네브메쉬를 비활성화 시킨다.(혹시 몰라서...)
		p_object.SetActive(false);            // 큐에 넣을 때는 비활성화
	}

	public GameObject GetQueue_Monster()           // 큐에서 객체를 빌려오는 함수(몬스터)
	{
		if (queue_Monster.Count.Equals(0))         // 몬스터 큐에 하나도 없다면...(필드 위에 몬스터 개수 제한 있음)
		{
			return null;                           // null 값을 리턴시킨다.
		}

		GameObject t_object = queue_Monster.Dequeue();     // 몬스터 큐에서 몬스터를 하나 가지고 온다.
 		

		if (mon_IntList.Count.Equals(0))                   // 중복 방지 리스트에 하나도 안 남았다면..
		{
			IntList_Setting();                                     // 이거 해도 될려나???????????
		}

		int monPos = Random.Range(0, mon_IntList.Count);    // 랜덤으로 몬스터를 스폰 시킬 위치를 가지고 온다.

		t_object.transform.position = transform.GetChild(mon_IntList[monPos]).position;
		t_object.SetActive(true);                          // 큐에서 가지고 온 몬스터를 활성화!
		t_object.GetComponent<NavMeshAgent>().enabled = true;        // 네브 메쉬를 킨다.

		mon_IntList.RemoveAt(monPos);         // 위치 중복 제거를 위해, 방금 위치 값을 넣었던 숫자를 리스트에서 없앤다.

		return t_object;      // 큐에서 꺼낸 몬스터 오브젝트를 리턴시킨다.
	}





	////////////////////////////////////////  포션 큐

	public void InsertQueue_Potion(GameObject p_object, int num)     // 사용한 객체를 큐에 다시 반납시키는 함수(포션)
	{
		switch (num)      // 반납시키는 포션의 번호에 따라..
		{
			case 0:       // 무적 포션이라면..
				queue_PotionH.Enqueue(p_object);    // 무적 포션을 큐에 반납시킨다.
				break;
			case 1:       // 삽 포션이라면...
				queue_PotionV.Enqueue(p_object);    // 삽 포션을 큐에 반납시킨다.
				break;
			default:      // 스피드 포션이라면...
				queue_PotionS.Enqueue(p_object);    // 스피드 포션을 큐에 반납시킨다.
				break;
		}
		p_object.SetActive(false);          // 큐에 넣을 때는 비활성화
	}

	public GameObject GetQueue_Potion(int num)           // 큐에서 객체를 빌려오는 함수(포션)
	{
		GameObject t_object;                      // 큐에서 내보낼 포션을 받는 변수

		switch (num)   // 무슨 포션을 내보낼지 번호가 온다면..
		{
			case 0:       // 무적 포션이라면..
				t_object = queue_PotionH.Dequeue();   // 무적 포션 하나를 큐에서 가지고 온다.
				t_object.SetActive(true);             // 큐에서 내보낼 때에는 활성화
				break;
			case 1:        // 삽 포션이라면...
				t_object = queue_PotionV.Dequeue();   // 삽 포션 하나를 큐에서 가지고 온다.
				t_object.SetActive(true);             // 큐에서 내보낼 때에는 활성화
				break;
			default:       // 스피드 포션이라면...
				t_object = queue_PotionS.Dequeue();   // 스피드 포션 하나를 큐에서 가지고 온다.
				t_object.SetActive(true);             // 큐에서 내보낼 때에는 활성화
				break;
		}

		return t_object;    // 방금 랜덤을 통해 나온 포션을 리턴시킨다.
	}




	////////////////////////////////////////  함정 큐

	public void InsertQueue_Digged(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(함정)
	{
		queue_Digged.Enqueue(p_object);           // 함정을 큐에 반납시킨다.
		p_object.SetActive(false);                // 큐에 반납 할 때는 비활성화
	}

	public GameObject GetQueue_Digged()           // 큐에서 객체를 빌려오는 함수(함정)
	{
		if (queue_Digged.Count.Equals(0))         // 함정 큐에 아무 것도 없다면...(필드 위에 개수 제한 있어서...)
		{
			return null;                          // null 값을 리턴시킨다.
		}

		GameObject t_object = queue_Digged.Dequeue();    // 함정 큐에서 함정 하나를 받아온다.
		t_object.SetActive(true);                        // 큐에서 내보낼떄는 활성화!

		return t_object;       // 큐에서 나온 함정 오브젝트를 리턴시킨다.
	}
}





