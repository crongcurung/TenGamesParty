using UnityEngine;

public class Mini10_End : MonoBehaviour          // 앤드 큐브에 부착됨
{
	public int fallCount;      // 이 스테이지에서의 떨어진 발판의 총 개수
	int stageCount;            // 이 스테이지에서의 발판의 총 개수

	void Start()
	{
		stageCount = transform.parent.childCount - 2;      // 이 스테이지의 큐브 숫자를 받아옴
	}


	public bool CheckEnd()          // 플레이어가 앤드 큐브에 닿으면 실행
	{
		if (stageCount.Equals(fallCount))      // 떨어진 큐브 숫자와 이 스테이지의 큐브 숫자가 같다면..
		{
			return true;         // true 넘겨
		}

		return false;          // 떨어진 큐브 숫자와 이 스테이지의 큐브 숫자가 다르다면..
	}
}
