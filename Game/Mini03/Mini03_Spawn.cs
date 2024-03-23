using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class Mini03_Spawn : MonoBehaviour
{
	[SerializeField] GameObject cap_Prefab;

	[SerializeField] GameObject[] Stage_Array01;
	[SerializeField] GameObject[] Stage_Array02;
	[SerializeField] GameObject[] Stage_Array03;

	[SerializeField] TextMeshProUGUI stageText;    // 현재 몇번째 스테이지인지 알려주는 텍스트

	[SerializeField] Image timeImage;                     // 시계 이미지

	[SerializeField] Mini03_Player mini03_Player;

	GameObject prefab;          // 프리팹 매니져에서 오브젝트를 받아올 변수
	GameObject mon_Prefab;      // 위에 프리팹 변수에서 몬스터 프리팹으로 뽑을 변수

	public int stageInt = 0;         // 현재 스테이지를 받는 인트 변수

	List<int> intList_10 = new List<int>();    // 중복 방지 인트 100개씩 들어간다.
	List<int> intList_15 = new List<int>();
	List<int> intList_19 = new List<int>();

	Queue<GameObject> queue_Monster = new Queue<GameObject>();       // 몬스터를 담을 큐
	Queue<GameObject> Temp_Queue = new Queue<GameObject>();          // 현재 활성화된 몬스터를 담을 큐

	GameObject prev_Stage = null;       // 이전 스테이지를 담는 변수

	int stageLevel_Num = 0;             // 현재 스테이지는 레벨이 몇인가? (0 ~ 2)
	bool isFirst = true;                // 첫 번째 스테이지만

	int currentStageLevelNum = 0;        // 스테이지 레벨(시간 용)

	bool isEnd = false;

	void Awake()
	{
		Time.timeScale = 1;     // 한 번 더 하는 경우 스케일이 0일 수도 있어서 여기서 1로 한다.


		IntLit_Setting();   // 중복 제거 인트 셋팅

		prefab = cap_Prefab;         // 몬스터 프리팹을 받아온다.
		for (int i = 0; i < 3; i++)       // 몬스터 4개(한번에 최대 나오는 숫자가 4라서...) 생성 및 보관
		{
			mon_Prefab = Instantiate(prefab);        // 몬스터 생성

			mon_Prefab.GetComponent<Mini03_Monster>().mini03_Player = mini03_Player;

			queue_Monster.Enqueue(mon_Prefab);       // 오브젝트 풀링
			mon_Prefab.SetActive(false);                  // 비활성화
		}
	}

	void Start()
	{
		stageInt = 1;

		stageText.text = stageInt.ToString();

		timeImage.fillAmount = 1;
	}

	void Update()
	{
		if (isEnd.Equals(false))
		{
			CountTime();     // 남은 시간을 알려주는 함수
		}
	}


	void IntLit_Setting()             // 중복 제거 인트 리스트에 값을 넣는다.
	{
		for (int i = 0; i < 100; i++)   // 100개씩 넣는다.
		{
			intList_10.Add(i);
			intList_15.Add(i);
			intList_19.Add(i);
		}
	}



	void CountTime()       // 남은 시간을 알려주는 함수
	{
		if (currentStageLevelNum.Equals(0))         // 현재 스테이지가 10 x 10면
		{
			timeImage.fillAmount -= Time.deltaTime / 60.0f;
		}
		else if (currentStageLevelNum.Equals(1))         // 현재 스테이지가 15 x 15면
		{
			timeImage.fillAmount -= Time.deltaTime / 120.0f;
		}
		else                                // 현재 스테이지가 19 x 19면
		{
			timeImage.fillAmount -= Time.deltaTime / 180.0f;
		}

		
		if (timeImage.fillAmount.Equals(0.0f))
		{
			isEnd = true;
			mini03_Player.GameOver();
		}

	}




	////////////////////// 몬스터 구역


	public void InsertQueue_Monster(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(몬스터)
	{
		queue_Monster.Enqueue(p_object);      // 몬스터을 큐에 반납시킨다.
		p_object.GetComponent<NavMeshAgent>().enabled = false;     // 이 몬스터의 네브메쉬를 비활성화 시킨다.(혹시 몰라서...)
		p_object.SetActive(false);            // 큐에 넣을 때는 비활성화
	}

	public GameObject GetQueue_Monster(Transform currentStage)           // 큐에서 객체를 빌려오는 함수(몬스터)
	{
		GameObject t_object = queue_Monster.Dequeue();     // 몬스터 큐에서 몬스터를 하나 가지고 온다.
		t_object.SetActive(true);                          // 큐에서 가지고 온 몬스터를 활성화!

		int rand_Mon = Random.Range(0, currentStage.GetChild(3).childCount);

		t_object.transform.position = currentStage.GetChild(3).GetChild(rand_Mon).transform.position;      // 몬스터 위치 조정(일단 위치 중복 가능)
		t_object.GetComponent<Mini03_Monster>().patrol = currentStage.GetChild(4).transform;
		t_object.GetComponent<NavMeshAgent>().enabled = true;        // 네브 메쉬를 킨다.

		return t_object;      // 큐에서 꺼낸 몬스터 오브젝트를 리턴시킨다.
	}




	////////////////////////// 스테이지 구역

	public Transform StageSpawn()            // 스테이지 스폰(활성화...)
	{
		if (isFirst.Equals(false))           // 처음에 한 번 건너 뛰기 위해 이렇게 만들었다..
		{
			Destroy(prev_Stage);             // 이전 스테이지를 없앤다.

			int removeInt = 0;               // 스테이지에 따라 몇개 없앨건지 본다.
			switch (stageLevel_Num)          // 스테이지가 사라질 떄 거기에 있던 몬스터들도 사라지게 해야 한다.
			{
				case 0:                      // 이전 스테이지가 3레벨이라면...
					removeInt = 3;           // 4마리 반납
					break;
				case 1:                      // 이전 스테이지가 1레벨이라면...
					removeInt = 1;           // 1마리 반납
					break;
				case 2:                      // 이전 스테이지가 2레벨이라면...
					removeInt = 2;           // 3마리 반납
					break;
			}

			for (int i = 0; i < removeInt; i++)           // 해당된 스테이지 레벨에 따라 몬스터를 반납한다.
			{
				InsertQueue_Monster(Temp_Queue.Dequeue());      // 임시 큐에 저장되었던 몬스터들을 반납한다.
			}

			if (currentStageLevelNum.Equals(2))
			{
				currentStageLevelNum = 0;
			}
			else
			{
				currentStageLevelNum++;
			}

			stageInt++;
			stageText.text = stageInt.ToString();
		}
		else
		{
			isFirst = false;        // 첫번째 끝났다고 알림
		}

		Transform stageTrans;           // 활성화된 스테이지를 받는 변수

		switch (stageLevel_Num)         // 스테이지 레벨은 1, 2, 3으로 구별
		{
			case 0:           // 10 x 10 스테이지
				stageTrans = StageSpawn_10();
				break;
			case 1:           // 15 x 15 스테이지
				stageTrans = StageSpawn_15();
				break;
			default:          // 19 x 19 스테이지
				stageTrans = StageSpawn_19();
				break;
		}

		timeImage.fillAmount = 1;
		stageLevel_Num++;                  // 스테이지 레벨을 올린다.
		if (stageLevel_Num.Equals(3))      // 스테이지 레벨이 3이라면 다시 0으로...
		{
			stageLevel_Num = 0;
		}

		return stageTrans;          // 활성화된 스테이지의 위치를 넘긴다.
	}


	Transform StageSpawn_10()           // 스테이지 10 스폰
	{
		int randInt = Random.Range(0, intList_10.Count);      // 현재 남은 중복 인트 개수를 가져와서
		int randInt10 = intList_10[randInt];                  // 숫자를 뽑아온다.
		//randInt10 = 0;                                        

		GameObject stage_Prefab = Stage_Array01[randInt10];     // 스테이지 프리팹 스폰
		prev_Stage = Instantiate(stage_Prefab);            // 스폰된 스테이지를 이전 스테이지 변수로 넘긴다.

		GameObject mon_Prefab = GetQueue_Monster(prev_Stage.transform);         // 몬스터도 스폰한다.
		Temp_Queue.Enqueue(mon_Prefab);        // 스폰된 몬스터를 임시 큐에 집어 넣는다.

		intList_10.RemoveAt(randInt);          // 중복 제거
		return stage_Prefab.transform;         // 스폰된 스테이지 위치를 넘긴다.
	}


	Transform StageSpawn_15()           // 스테이지 15 스폰
	{
		int randInt = Random.Range(0, intList_15.Count);      // 현재 남은 중복 인트 개수를 가져와서
		int randInt15 = intList_15[randInt];                  // 숫자를 뽑아온다.
		//randInt15 = 0;

		GameObject stage_Prefab = Stage_Array02[randInt15];     // 스테이지 프리팹 스폰
		prev_Stage = Instantiate(stage_Prefab);            // 스폰된 스테이지를 이전 스테이지 변수로 넘긴다.

		for (int i = 0; i < 2; i++)    // 15레벨은 3마리 스폰한다.
		{
			GameObject mon_Prefab = GetQueue_Monster(prev_Stage.transform);         // 몬스터도 스폰한다.
			Temp_Queue.Enqueue(mon_Prefab);          // 스폰된 몬스터를 임시 큐에 집어 넣는다.
		}

		intList_15.RemoveAt(randInt);          // 중복 제거

		return stage_Prefab.transform;         // 스폰된 스테이지 위치를 넘긴다.
	}


	Transform StageSpawn_19()           // 스테이지 19 스폰
	{
		int randInt = Random.Range(0, intList_19.Count);      // 현재 남은 중복 인트 개수를 가져와서
		int randInt19 = intList_19[randInt];                  // 숫자를 뽑아온다.
		//randInt19 = 0;

		GameObject stage_Prefab = Stage_Array03[randInt19];     // 스테이지 프리팹 스폰
		prev_Stage = Instantiate(stage_Prefab);            // 스폰된 스테이지를 이전 스테이지 변수로 넘긴다.

		for (int i = 0; i < 3; i++)    // 19레벨은 3마리 스폰한다.
		{
			GameObject mon_Prefab = GetQueue_Monster(prev_Stage.transform);         // 몬스터도 스폰한다.
			Temp_Queue.Enqueue(mon_Prefab);        // 스폰된 몬스터를 임시 큐에 집어 넣는다.
		}

		intList_19.RemoveAt(randInt);          // 중복 제거

		if (intList_19.Count.Equals(0))        // 중복 제거 리스트에 아무 것도 없다면...(마지막 것만 검사하면 되지 다른 레벨에서는 검사를 안 한다)
		{
			IntLit_Setting();                  // 리스트 셋팅
		}

		return stage_Prefab.transform;         // 스폰된 스테이지 위치를 넘긴다.
	}
}
