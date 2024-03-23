using UnityEngine;

public class Mini01_Potion : MonoBehaviour
{
	[SerializeField] float speedRot = 10.0f;     // 회전의 속도를 받는 변수(아이템 마다 회전 속도가 다를 수 있어서 시리얼 라이즈 필드를 씀...)

	public Mini01_Spawn mini01_Spawn;
	public Transform itemPos_This;

	void Update()
	{
		ItemRot();
	}

	void ItemRot()               // 아이템의 회전을 담당하는 함수
	{
		transform.Rotate(0, Time.deltaTime * speedRot, 0);
	}


	public void Spawn_Potion()             // 플레이어쪽에서 접근할 수 있게 public으로 함
	{
		int randInt = Random.Range(0, 3);          // 랜덤 숫자를 받고나서
		GameObject potion;                         // 프피팹 스폰을 받는 변수

		switch (randInt)
		{
			case 0:          // 랜덤 숫자가 0이라면..
				potion = mini01_Spawn.GetQueue_Potion(0);     // 무적 포션을 큐에서 가져온다.
				break;
			case 1:          // 랜덤 숫자가 1이라면..
				potion = mini01_Spawn.GetQueue_Potion(1);     // 삽 포션을 큐에서 가져온다.
				break;
			default:         // 랜덤 숫자가 2이라면..
				potion = mini01_Spawn.GetQueue_Potion(2);     // 스피드 포션을 큐에서 가져온다.
				break;
		}

		randInt = Random.Range(0, itemPos_This.childCount);                   // 포션을 스폰할 위치를 랜덤으로 가져온다.
		potion.transform.position = itemPos_This.GetChild(randInt).position;
	}
}
