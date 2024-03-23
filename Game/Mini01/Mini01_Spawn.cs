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
	Queue<GameObject> queue_Monster = new Queue<GameObject>();     // ���� ������Ʈ Ǯ��
	
	Queue<GameObject> queue_PotionH = new Queue<GameObject>();     // �������� ������Ʈ Ǯ��
	Queue<GameObject> queue_PotionV = new Queue<GameObject>();     // �� ���� ������Ʈ Ǯ��
	Queue<GameObject> queue_PotionS = new Queue<GameObject>();     // ���ǵ� ���� ������Ʈ Ǯ��

	Queue<GameObject> queue_Digged = new Queue<GameObject>();     // ���� ������Ʈ Ǯ��

	GameObject prefab;          // ������ �Ŵ������� ������Ʈ�� �޾ƿ� ����
	GameObject mon_Prefab;      // ���� ������ �������� ���� ���������� ���� ����


	void Awake()
	{
		IntList_Setting();   // �ߺ� ���� int ����Ʈ ���� �����ϴ� �Լ� ����

		Mini01_Monster mini01_Monster;
		Mini01_Potion mini01_Potion;


		prefab = knight_Prefab;
		for (int i = 0; i < 40; i++)       // ���� 40���� ���� �� ����
		{
			mon_Prefab = Instantiate(prefab);// ���� ����

			mini01_Monster = mon_Prefab.GetComponent<Mini01_Monster>();

			mini01_Monster.player = playerTrans;
			mini01_Monster.mini01_Player = mini01_Player;
			queue_Monster.Enqueue(mon_Prefab);       // ������Ʈ Ǯ��
		}

		prefab = potion_H;      // ���� ������ �ѱ��.
		GameObject potion_Prefab;   // ������ ��ȯ���� ����

		for (int i = 0; i < 2; i++)            // 2�� ���� ���� ������ ������ �´�.
		{
			potion_Prefab = Instantiate(prefab);        // ���� ������ �����Ѵ�.

			mini01_Potion = potion_Prefab.GetComponent<Mini01_Potion>();

			mini01_Potion.mini01_Spawn = this;
			mini01_Potion.itemPos_This = itemPos;
			queue_PotionH.Enqueue(potion_Prefab);       // ������ ���� ������ ť�� �ִ´�.
		}

		prefab = potion_V;        // �� ������ �ѱ��.
		for (int i = 0; i < 2; i++)            // 2�� ���� �� ������ ������ �´�.
		{
			potion_Prefab = Instantiate(prefab);        // �� ������ �����Ѵ�.

			mini01_Potion = potion_Prefab.GetComponent<Mini01_Potion>();

			mini01_Potion.mini01_Spawn = this;
			mini01_Potion.itemPos_This = itemPos;
			queue_PotionV.Enqueue(potion_Prefab);       // ������ �� ������ ť�� �ִ´�.
		}

		prefab = potion_S;        // ���ǵ� ������ �ѱ��.
		for (int i = 0; i < 2; i++)            // 2�� ���� ���ǵ� ������ ������ �´�.
		{
			potion_Prefab = Instantiate(prefab);        // ���ǵ� ������ �����Ѵ�.

			mini01_Potion = potion_Prefab.GetComponent<Mini01_Potion>();

			mini01_Potion.mini01_Spawn = this;
			mini01_Potion.itemPos_This = itemPos;
			queue_PotionS.Enqueue(potion_Prefab);       // ������ ���ǵ� ������ ť�� �ִ´�.
		}


		prefab = digg;        // ������ �ѱ��.
		for (int i = 0; i < 30; i++)            // 30�� ���� ������ ������ �´�.
		{
			potion_Prefab = Instantiate(prefab);             // ������ �����Ѵ�.
			potion_Prefab.GetComponent<Mini01_Digged>().mini01_Spawn = this;
			queue_Digged.Enqueue(potion_Prefab);             // ������ ������ ť�� �ִ´�.
		}

		List<int> IntList = new List<int>();

		for (int i = 0; i < FixedPos.childCount; i++)
		{
			IntList.Add(i);
		}
		

		prefab = fixedObj;        // ���ع� ������Ʈ.........
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
			GetQueue_Monster();        // �̸� ���� �� ������ �ʵ忡 ���´�.
		}
	}


	void IntList_Setting()        // �ߺ� ���� int ����Ʈ ���� �����ϴ� �Լ�
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			mon_IntList.Add(i);    // 0���� 122���� �ִ´�.
		}
	}


	void Spawn_Potion()             // �÷��̾��ʿ��� ������ �� �ְ� public���� ��
	{
		int randInt = Random.Range(0, 3);          // ���� ���ڸ� �ް���
		GameObject potion;                         // ������ ������ �޴� ����

		switch (randInt)
		{
			case 0:          // ���� ���ڰ� 0�̶��..
				potion = GetQueue_Potion(0);     // ���� ������ ť���� �����´�.
				break;
			case 1:          // ���� ���ڰ� 1�̶��..
				potion = GetQueue_Potion(1);     // �� ������ ť���� �����´�.
				break;
			default:         // ���� ���ڰ� 2�̶��..
				potion = GetQueue_Potion(2);     // ���ǵ� ������ ť���� �����´�.
				break;
		}

		randInt = Random.Range(0, itemPos.childCount);                   // ������ ������ ��ġ�� �������� �����´�.
		potion.transform.position = itemPos.GetChild(randInt).position;
	}

	////////////////////////////////////////  ���� ť

	public void InsertQueue_Monster(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(����)
	{
		queue_Monster.Enqueue(p_object);      // ������ ť�� �ݳ���Ų��.
		p_object.GetComponent<NavMeshAgent>().enabled = false;     // �� ������ �׺�޽��� ��Ȱ��ȭ ��Ų��.(Ȥ�� ����...)
		p_object.SetActive(false);            // ť�� ���� ���� ��Ȱ��ȭ
	}

	public GameObject GetQueue_Monster()           // ť���� ��ü�� �������� �Լ�(����)
	{
		if (queue_Monster.Count.Equals(0))         // ���� ť�� �ϳ��� ���ٸ�...(�ʵ� ���� ���� ���� ���� ����)
		{
			return null;                           // null ���� ���Ͻ�Ų��.
		}

		GameObject t_object = queue_Monster.Dequeue();     // ���� ť���� ���͸� �ϳ� ������ �´�.
 		

		if (mon_IntList.Count.Equals(0))                   // �ߺ� ���� ����Ʈ�� �ϳ��� �� ���Ҵٸ�..
		{
			IntList_Setting();                                     // �̰� �ص� �ɷ���???????????
		}

		int monPos = Random.Range(0, mon_IntList.Count);    // �������� ���͸� ���� ��ų ��ġ�� ������ �´�.

		t_object.transform.position = transform.GetChild(mon_IntList[monPos]).position;
		t_object.SetActive(true);                          // ť���� ������ �� ���͸� Ȱ��ȭ!
		t_object.GetComponent<NavMeshAgent>().enabled = true;        // �׺� �޽��� Ų��.

		mon_IntList.RemoveAt(monPos);         // ��ġ �ߺ� ���Ÿ� ����, ��� ��ġ ���� �־��� ���ڸ� ����Ʈ���� ���ش�.

		return t_object;      // ť���� ���� ���� ������Ʈ�� ���Ͻ�Ų��.
	}





	////////////////////////////////////////  ���� ť

	public void InsertQueue_Potion(GameObject p_object, int num)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(����)
	{
		switch (num)      // �ݳ���Ű�� ������ ��ȣ�� ����..
		{
			case 0:       // ���� �����̶��..
				queue_PotionH.Enqueue(p_object);    // ���� ������ ť�� �ݳ���Ų��.
				break;
			case 1:       // �� �����̶��...
				queue_PotionV.Enqueue(p_object);    // �� ������ ť�� �ݳ���Ų��.
				break;
			default:      // ���ǵ� �����̶��...
				queue_PotionS.Enqueue(p_object);    // ���ǵ� ������ ť�� �ݳ���Ų��.
				break;
		}
		p_object.SetActive(false);          // ť�� ���� ���� ��Ȱ��ȭ
	}

	public GameObject GetQueue_Potion(int num)           // ť���� ��ü�� �������� �Լ�(����)
	{
		GameObject t_object;                      // ť���� ������ ������ �޴� ����

		switch (num)   // ���� ������ �������� ��ȣ�� �´ٸ�..
		{
			case 0:       // ���� �����̶��..
				t_object = queue_PotionH.Dequeue();   // ���� ���� �ϳ��� ť���� ������ �´�.
				t_object.SetActive(true);             // ť���� ������ ������ Ȱ��ȭ
				break;
			case 1:        // �� �����̶��...
				t_object = queue_PotionV.Dequeue();   // �� ���� �ϳ��� ť���� ������ �´�.
				t_object.SetActive(true);             // ť���� ������ ������ Ȱ��ȭ
				break;
			default:       // ���ǵ� �����̶��...
				t_object = queue_PotionS.Dequeue();   // ���ǵ� ���� �ϳ��� ť���� ������ �´�.
				t_object.SetActive(true);             // ť���� ������ ������ Ȱ��ȭ
				break;
		}

		return t_object;    // ��� ������ ���� ���� ������ ���Ͻ�Ų��.
	}




	////////////////////////////////////////  ���� ť

	public void InsertQueue_Digged(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(����)
	{
		queue_Digged.Enqueue(p_object);           // ������ ť�� �ݳ���Ų��.
		p_object.SetActive(false);                // ť�� �ݳ� �� ���� ��Ȱ��ȭ
	}

	public GameObject GetQueue_Digged()           // ť���� ��ü�� �������� �Լ�(����)
	{
		if (queue_Digged.Count.Equals(0))         // ���� ť�� �ƹ� �͵� ���ٸ�...(�ʵ� ���� ���� ���� �־...)
		{
			return null;                          // null ���� ���Ͻ�Ų��.
		}

		GameObject t_object = queue_Digged.Dequeue();    // ���� ť���� ���� �ϳ��� �޾ƿ´�.
		t_object.SetActive(true);                        // ť���� ���������� Ȱ��ȭ!

		return t_object;       // ť���� ���� ���� ������Ʈ�� ���Ͻ�Ų��.
	}
}





