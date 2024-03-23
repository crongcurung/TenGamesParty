using UnityEngine;

public class Mini10_Cage : MonoBehaviour    // 스테이지 첫번쨰에 감옥에 부착됨
{
	bool isStart = false;       // 1초 후에 업데이터가 돌아가기 위한 변수

	void Start()
	{
		Invoke("Invoke_Start", 1.0f);      // 인보크 함수 1초 후에 실행
	}

	void Invoke_Start()
	{
		isStart = true;        // 이제 업데이트 실행한다.
	}


	void Update()      
	{
		if (isStart.Equals(true))       // 1초 후에 실행
		{
			transform.position += Vector3.down * Time.deltaTime;   // 감옥을 아래로 쭉 내려가게 한다.

			if (transform.localPosition.z <= -1.0f)      // 감옥이 안 보일떄까지 내려간다면..
			{
				Destroy(transform.gameObject);            // 이 감옥을 없앰..
			}
		}
	}
}
