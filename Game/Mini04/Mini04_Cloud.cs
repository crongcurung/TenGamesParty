using UnityEngine;

public class Mini04_Cloud : MonoBehaviour     // 구름 다섯개에 모두 부착됨..
{
	Vector3 currentPos;               // 현재 위치에서 위아래
	Vector3 originPos;               // 초기화 용

	float delta = 0.5f;                // 위아래
	float speed = 3.0f;                // 속도

	bool randBool;                     // 처음에 위로 갈지, 아래로 갈지 정하는 랜덤 불

	Vector3 m_Offset;
	float m_ZCoord;

	bool isTouch;                     // 현제 플레이어가 터치를 하고 있는지 않하고 있는지?
	public bool isTouchClouds;        // 구름이 터치가 가능한가?

	Camera cameraMain;

	void Awake()
	{
		currentPos = transform.position;         // 일단 둘 다 처음 위치 저장
		originPos = transform.position;

		cameraMain = Camera.main;
		isTouchClouds = true;
	}

	void OnEnable()          // 켜졌을 때...
	{
		float scaleFloat = Random.Range(1.5f, 3.0f);     // 구름의 크기를 랜덤으로..
		Vector3 scaleVector = new Vector3(scaleFloat, scaleFloat, scaleFloat);     // 크기 조정
		transform.localScale = scaleVector;         // 크기 조정

		delta = Random.Range(0.2f, 0.4f);         // 위 아래 이동 갭을 랜덤으로..
		speed = Random.Range(2.0f, 3.5f);         // 구름의 속도를 램덤으로...

		randBool = (Random.value > 0.5f);         // 처음에 위로 갈지, 아래로 갈지 정하는 랜덤 불

		isTouch = false;
	}


	void OnDisable()          // 꺼졌을 때...
	{
		transform.position = originPos;          // 처음 위치로 초기화
		currentPos = originPos;
	}

	void Update()
	{
		if (isTouch.Equals(false))           // 현재 플레이어가 터치를 안하고 있다면?
		{
			Vector3 v = currentPos;     // 지금 위치를 저장

			if (randBool.Equals(true))        // 랜덤 불이 true이면..
			{
				v.y += delta * Mathf.Cos(Time.time * speed * 0.2f);   // 위로
			}
			else
			{
				v.y -= delta * Mathf.Cos(Time.time * speed * 0.2f);   // 아래로
			}
			transform.position = v;     // 위치 저장
		}
	}



	void OnMouseDown()      // 처음 플레이어가 터치한다면
	{
		m_ZCoord = cameraMain.WorldToScreenPoint(gameObject.transform.position).z;
		m_Offset = gameObject.transform.position - GetMouseWorldPosition();

		AudioMng.ins.PlayEffect("Cloud");    // 구름 소리
		isTouch = true;         // 현재 플레이어가 터치하고 있다고 알림
	}

	void OnMouseDrag()      // 플레이어가 드래그 중이라면...
	{
		transform.position = GetMouseWorldPosition() + m_Offset;
	}

	void OnMouseUp()       // 플레이어가 터치에서 손뗀다면...
	{
		isTouch = false;        // 현재 플레이어가 터치를 안하고 있다고 알림

		currentPos = transform.position;        // 현재 위치를 저장

	}

	Vector3 GetMouseWorldPosition()        // 월드좌표로 변환 함수1
	{
		Vector3 mousePoint = Input.mousePosition;
		mousePoint.z = m_ZCoord;

		return cameraMain.ScreenToWorldPoint(mousePoint);
	}
}
