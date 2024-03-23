using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Mini10_Camera : MonoBehaviour          // 미니 10 카메라에 부착됨
{
	[SerializeField] Material Mini10_SkyBox;

	[SerializeField] Mini10_Player mini10_Player;     // 플레이어 캐릭터
	[SerializeField] Transform player;         // 카메라가 잡을 플레이어 캐릭터 오브젝트
	[SerializeField] Vector3 cameraPos;         // 미니 게임 마다 화면에 보여줄 카메라 위치 조정 값

	[SerializeField] Image Right_Image;
	[SerializeField] Sprite sprite;

	bool isNextMove = false;              // false면 캐릭터를 쫓아감, true면 카메라가 다른 스테이지로 감
	bool oddEvenBool = true;               // 스테이지 순서로별로 홀수면 true, 짝수면 false
	float rotateSpeed = 2.0f;              // 카메라 이동 속도

	WaitForSeconds delay;

	Quaternion originRot;
	Quaternion mapRot;

	bool isRun = false;

	Material material;
	string skyText;

	void Awake()
	{
		Material skyBox_Mini10 = Mini10_SkyBox;      // 스카이 박스를 가져온다.
		RenderSettings.skybox = skyBox_Mini10;       // 스카이 박스 교체

		cameraPos = new Vector3(0, 3.0f, -3.0f);                    // 카메라 위치 조정
		originRot = Quaternion.Euler(new Vector3(45.0f, 0, 0));  // 카메라 초기 회전 값 조절
		mapRot = Quaternion.Euler(new Vector3(90.0f, 0, 0));
		transform.rotation = originRot;

		material = RenderSettings.skybox;
		skyText = "_Rotation";

		delay = new WaitForSeconds(10.0f);     // 5초간 맵 카메라 발동
	}

	bool isMapCamera = false;      // 맵 카메라가 실행 중이냐 묻는 변수

	Coroutine coroutine;

	void LateUpdate()     // 요거 lateUpdate로 해야함..
	{
		if (isMapCamera.Equals(true))     // 맵 카메라 실행중이라면 밑에 실행 안함
		{
			return;
		}

		if (isNextMove.Equals(false))    
		{
			CameraPos();       // 캐릭터를 따라감
		}
		else
		{
			MoveCamera();      // 다른 스테이지로 이동
		}
	}

	void CameraPos()
	{
		transform.position = player.position + cameraPos;       // 플레이어의 위치에 따른 카메라 위치

	}

	void MoveCamera()        // 다른 스테이지로 이동
	{
		transform.position = Vector3.Lerp(transform.position, player.transform.position + cameraPos, Time.deltaTime * 3.0f);     // 러프하게 카메라 이동
		Vector3 LerpA = transform.position;                               // 현재 카메라 위치
		Vector3 LerpB = player.transform.position + cameraPos;            // 플레이어가 있는 위치

		if (oddEvenBool.Equals(true))                     // 홀수 스테이지라면...
		{
			material.SetFloat(skyText, Time.time * rotateSpeed);   // 스카이박스 회전
		}
		else                                         // 짝수 스테이지라면...
		{
			material.SetFloat(skyText, Time.time * -1 * rotateSpeed);   // 스카이박스 회전
		}

		if ((LerpA - LerpB).magnitude < 0.005f)         // 다음 스테이지와의 거리가 0.005f 밑, 이라면...
		{
			oddEvenBool = !oddEvenBool;                  // 홀수 짝수 변경

			isNextMove = false;      // 거의 다 왔기 때문에, 카메라 이동을 멈춤!

			mini10_Player.isCameraMove = false;        // 플레이어 스크립트에 카메라 이동이 끝났다고 알림
			return;
		}
	}

	public void StopMapCamera()     // 맵 카메라 코루틴이 실행중이라면 끄는 역활
	{
		if (isRun.Equals(true))
		{
			isRun = false;
			StopCoroutine(coroutine);


			AudioMng.ins.PlayEffect("Back");    // 지도 끝냄
			mini10_Player.isMapCamera = false;

			isMapCamera = false;      // 맵 카메라 끝냄
			transform.rotation = originRot;  // 카메라 초기 회전 값 조절
		}
	}

	public void NextStageMove()
	{
		isNextMove = true;              // 카메라 이동을 시킨다고 알림
	}

	public void MapCamera(Transform cameraPos)      // 맵 카메라 함수
	{
		isMapCamera = true;                        // 맵 카메라 실행중이라고 알림

		transform.position = cameraPos.position;                            // 받아온 위치를 카메라 위치로 바꿈
		transform.rotation = mapRot;       // 카메라 회전을 90도로 회전

		coroutine = StartCoroutine(WaitMapCamera());       // 맵 카메라 코루틴 실행
	}

	IEnumerator WaitMapCamera()                       // 맵을 보여준 후, 5초 다시 돌아가는 코루틴
	{
		isRun = true;
		mini10_Player.isMapCamera = true;

		yield return delay;       // 10초 후 코루틴 끝남
		isRun = false;
		mini10_Player.isMapCamera = false;


		AudioMng.ins.PlayEffect("Back");    // 지도 끝냄
		Right_Image.sprite = sprite;

		isMapCamera = false;      // 맵 카메라 끝냄
		transform.rotation = originRot;  // 카메라 초기 회전 값 조절
	}
}
