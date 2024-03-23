using UnityEngine;

public class Mini01_Digged : MonoBehaviour         // 함정 오브젝트에 부착됨
{
	public Mini01_Spawn mini01_Spawn;


	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.layer)         // 닿은 오브젝트의 레이어가...
		{
			case 4:                            // 물에 닿으면                Water
				mini01_Spawn.InsertQueue_Digged(transform.gameObject);    // 이 함정을 큐에 반납시킨다.
				break;
			case 3:                            // 물 바뀐거에 닿으면           WALL
				mini01_Spawn.InsertQueue_Digged(transform.gameObject);    // 이 함정을 큐에 반납시킨다.
				break;
			case 7:                            // 몬스터에 닿으면             Monster
				AudioMng.ins.PlayEffect("HitApple");    // 몬스터가 함정에 닿아서 없어짐
				mini01_Spawn.InsertQueue_Digged(transform.gameObject);    // 이 함정을 큐에 반납시킨다.
				mini01_Spawn.InsertQueue_Monster(other.gameObject);      // 닿은 몬스터를 큐에 반납시킨다.
				break;
			case 8:                            // 선풍기에 닿으면           Object
				mini01_Spawn.InsertQueue_Digged(transform.gameObject);    // 이 함정을 큐에 반납시킨다.
				break;
		}
	}
}
