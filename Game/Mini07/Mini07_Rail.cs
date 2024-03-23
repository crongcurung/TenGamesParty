using System.Collections;
using UnityEngine;

public class Mini07_Rail : MonoBehaviour
{
	public Mini07_Spawn mini07_Spawn;

	int railInt;
	WaitForSeconds delay01;

	void Awake()
	{
		if (transform.CompareTag("Spring"))       // 流急
		{
			railInt = 0;
		}
		else if (transform.CompareTag("Bear"))    // 哭率
		{
			railInt = 1;
		}
		else                                      // 坷弗率(喉发圈)
		{ 
			railInt = 2;
		}

		delay01 = new WaitForSeconds(1.0f);
	}

	void OnEnable()
	{
		StartCoroutine(Insert_Coroutine());
	}


	IEnumerator Insert_Coroutine()
	{
		yield return delay01;

		if (railInt.Equals(0))
		{
			mini07_Spawn.InsertQueue_Straight(transform.gameObject);
		}
		else if (railInt.Equals(1))
		{
			mini07_Spawn.InsertQueue_Left(transform.gameObject);
		}
		else
		{
			mini07_Spawn.InsertQueue_Right(transform.gameObject);
		}
	}
}
