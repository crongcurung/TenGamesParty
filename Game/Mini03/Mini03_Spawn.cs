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

	[SerializeField] TextMeshProUGUI stageText;    // ���� ���° ������������ �˷��ִ� �ؽ�Ʈ

	[SerializeField] Image timeImage;                     // �ð� �̹���

	[SerializeField] Mini03_Player mini03_Player;

	GameObject prefab;          // ������ �Ŵ������� ������Ʈ�� �޾ƿ� ����
	GameObject mon_Prefab;      // ���� ������ �������� ���� ���������� ���� ����

	public int stageInt = 0;         // ���� ���������� �޴� ��Ʈ ����

	List<int> intList_10 = new List<int>();    // �ߺ� ���� ��Ʈ 100���� ����.
	List<int> intList_15 = new List<int>();
	List<int> intList_19 = new List<int>();

	Queue<GameObject> queue_Monster = new Queue<GameObject>();       // ���͸� ���� ť
	Queue<GameObject> Temp_Queue = new Queue<GameObject>();          // ���� Ȱ��ȭ�� ���͸� ���� ť

	GameObject prev_Stage = null;       // ���� ���������� ��� ����

	int stageLevel_Num = 0;             // ���� ���������� ������ ���ΰ�? (0 ~ 2)
	bool isFirst = true;                // ù ��° ����������

	int currentStageLevelNum = 0;        // �������� ����(�ð� ��)

	bool isEnd = false;

	void Awake()
	{
		Time.timeScale = 1;     // �� �� �� �ϴ� ��� �������� 0�� ���� �־ ���⼭ 1�� �Ѵ�.


		IntLit_Setting();   // �ߺ� ���� ��Ʈ ����

		prefab = cap_Prefab;         // ���� �������� �޾ƿ´�.
		for (int i = 0; i < 3; i++)       // ���� 4��(�ѹ��� �ִ� ������ ���ڰ� 4��...) ���� �� ����
		{
			mon_Prefab = Instantiate(prefab);        // ���� ����

			mon_Prefab.GetComponent<Mini03_Monster>().mini03_Player = mini03_Player;

			queue_Monster.Enqueue(mon_Prefab);       // ������Ʈ Ǯ��
			mon_Prefab.SetActive(false);                  // ��Ȱ��ȭ
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
			CountTime();     // ���� �ð��� �˷��ִ� �Լ�
		}
	}


	void IntLit_Setting()             // �ߺ� ���� ��Ʈ ����Ʈ�� ���� �ִ´�.
	{
		for (int i = 0; i < 100; i++)   // 100���� �ִ´�.
		{
			intList_10.Add(i);
			intList_15.Add(i);
			intList_19.Add(i);
		}
	}



	void CountTime()       // ���� �ð��� �˷��ִ� �Լ�
	{
		if (currentStageLevelNum.Equals(0))         // ���� ���������� 10 x 10��
		{
			timeImage.fillAmount -= Time.deltaTime / 60.0f;
		}
		else if (currentStageLevelNum.Equals(1))         // ���� ���������� 15 x 15��
		{
			timeImage.fillAmount -= Time.deltaTime / 120.0f;
		}
		else                                // ���� ���������� 19 x 19��
		{
			timeImage.fillAmount -= Time.deltaTime / 180.0f;
		}

		
		if (timeImage.fillAmount.Equals(0.0f))
		{
			isEnd = true;
			mini03_Player.GameOver();
		}

	}




	////////////////////// ���� ����


	public void InsertQueue_Monster(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(����)
	{
		queue_Monster.Enqueue(p_object);      // ������ ť�� �ݳ���Ų��.
		p_object.GetComponent<NavMeshAgent>().enabled = false;     // �� ������ �׺�޽��� ��Ȱ��ȭ ��Ų��.(Ȥ�� ����...)
		p_object.SetActive(false);            // ť�� ���� ���� ��Ȱ��ȭ
	}

	public GameObject GetQueue_Monster(Transform currentStage)           // ť���� ��ü�� �������� �Լ�(����)
	{
		GameObject t_object = queue_Monster.Dequeue();     // ���� ť���� ���͸� �ϳ� ������ �´�.
		t_object.SetActive(true);                          // ť���� ������ �� ���͸� Ȱ��ȭ!

		int rand_Mon = Random.Range(0, currentStage.GetChild(3).childCount);

		t_object.transform.position = currentStage.GetChild(3).GetChild(rand_Mon).transform.position;      // ���� ��ġ ����(�ϴ� ��ġ �ߺ� ����)
		t_object.GetComponent<Mini03_Monster>().patrol = currentStage.GetChild(4).transform;
		t_object.GetComponent<NavMeshAgent>().enabled = true;        // �׺� �޽��� Ų��.

		return t_object;      // ť���� ���� ���� ������Ʈ�� ���Ͻ�Ų��.
	}




	////////////////////////// �������� ����

	public Transform StageSpawn()            // �������� ����(Ȱ��ȭ...)
	{
		if (isFirst.Equals(false))           // ó���� �� �� �ǳ� �ٱ� ���� �̷��� �������..
		{
			Destroy(prev_Stage);             // ���� ���������� ���ش�.

			int removeInt = 0;               // ���������� ���� � ���ٰ��� ����.
			switch (stageLevel_Num)          // ���������� ����� �� �ű⿡ �ִ� ���͵鵵 ������� �ؾ� �Ѵ�.
			{
				case 0:                      // ���� ���������� 3�����̶��...
					removeInt = 3;           // 4���� �ݳ�
					break;
				case 1:                      // ���� ���������� 1�����̶��...
					removeInt = 1;           // 1���� �ݳ�
					break;
				case 2:                      // ���� ���������� 2�����̶��...
					removeInt = 2;           // 3���� �ݳ�
					break;
			}

			for (int i = 0; i < removeInt; i++)           // �ش�� �������� ������ ���� ���͸� �ݳ��Ѵ�.
			{
				InsertQueue_Monster(Temp_Queue.Dequeue());      // �ӽ� ť�� ����Ǿ��� ���͵��� �ݳ��Ѵ�.
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
			isFirst = false;        // ù��° �����ٰ� �˸�
		}

		Transform stageTrans;           // Ȱ��ȭ�� ���������� �޴� ����

		switch (stageLevel_Num)         // �������� ������ 1, 2, 3���� ����
		{
			case 0:           // 10 x 10 ��������
				stageTrans = StageSpawn_10();
				break;
			case 1:           // 15 x 15 ��������
				stageTrans = StageSpawn_15();
				break;
			default:          // 19 x 19 ��������
				stageTrans = StageSpawn_19();
				break;
		}

		timeImage.fillAmount = 1;
		stageLevel_Num++;                  // �������� ������ �ø���.
		if (stageLevel_Num.Equals(3))      // �������� ������ 3�̶�� �ٽ� 0����...
		{
			stageLevel_Num = 0;
		}

		return stageTrans;          // Ȱ��ȭ�� ���������� ��ġ�� �ѱ��.
	}


	Transform StageSpawn_10()           // �������� 10 ����
	{
		int randInt = Random.Range(0, intList_10.Count);      // ���� ���� �ߺ� ��Ʈ ������ �����ͼ�
		int randInt10 = intList_10[randInt];                  // ���ڸ� �̾ƿ´�.
		//randInt10 = 0;                                        

		GameObject stage_Prefab = Stage_Array01[randInt10];     // �������� ������ ����
		prev_Stage = Instantiate(stage_Prefab);            // ������ ���������� ���� �������� ������ �ѱ��.

		GameObject mon_Prefab = GetQueue_Monster(prev_Stage.transform);         // ���͵� �����Ѵ�.
		Temp_Queue.Enqueue(mon_Prefab);        // ������ ���͸� �ӽ� ť�� ���� �ִ´�.

		intList_10.RemoveAt(randInt);          // �ߺ� ����
		return stage_Prefab.transform;         // ������ �������� ��ġ�� �ѱ��.
	}


	Transform StageSpawn_15()           // �������� 15 ����
	{
		int randInt = Random.Range(0, intList_15.Count);      // ���� ���� �ߺ� ��Ʈ ������ �����ͼ�
		int randInt15 = intList_15[randInt];                  // ���ڸ� �̾ƿ´�.
		//randInt15 = 0;

		GameObject stage_Prefab = Stage_Array02[randInt15];     // �������� ������ ����
		prev_Stage = Instantiate(stage_Prefab);            // ������ ���������� ���� �������� ������ �ѱ��.

		for (int i = 0; i < 2; i++)    // 15������ 3���� �����Ѵ�.
		{
			GameObject mon_Prefab = GetQueue_Monster(prev_Stage.transform);         // ���͵� �����Ѵ�.
			Temp_Queue.Enqueue(mon_Prefab);          // ������ ���͸� �ӽ� ť�� ���� �ִ´�.
		}

		intList_15.RemoveAt(randInt);          // �ߺ� ����

		return stage_Prefab.transform;         // ������ �������� ��ġ�� �ѱ��.
	}


	Transform StageSpawn_19()           // �������� 19 ����
	{
		int randInt = Random.Range(0, intList_19.Count);      // ���� ���� �ߺ� ��Ʈ ������ �����ͼ�
		int randInt19 = intList_19[randInt];                  // ���ڸ� �̾ƿ´�.
		//randInt19 = 0;

		GameObject stage_Prefab = Stage_Array03[randInt19];     // �������� ������ ����
		prev_Stage = Instantiate(stage_Prefab);            // ������ ���������� ���� �������� ������ �ѱ��.

		for (int i = 0; i < 3; i++)    // 19������ 3���� �����Ѵ�.
		{
			GameObject mon_Prefab = GetQueue_Monster(prev_Stage.transform);         // ���͵� �����Ѵ�.
			Temp_Queue.Enqueue(mon_Prefab);        // ������ ���͸� �ӽ� ť�� ���� �ִ´�.
		}

		intList_19.RemoveAt(randInt);          // �ߺ� ����

		if (intList_19.Count.Equals(0))        // �ߺ� ���� ����Ʈ�� �ƹ� �͵� ���ٸ�...(������ �͸� �˻��ϸ� ���� �ٸ� ���������� �˻縦 �� �Ѵ�)
		{
			IntLit_Setting();                  // ����Ʈ ����
		}

		return stage_Prefab.transform;         // ������ �������� ��ġ�� �ѱ��.
	}
}
