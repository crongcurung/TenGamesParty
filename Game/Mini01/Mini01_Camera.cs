using UnityEngine;

public class Mini01_Camera : MonoBehaviour         // 베이스 카메라 상속
{
	[SerializeField] Material Default_SkyBox;

	[SerializeField] Transform player;         // 카메라가 잡을 플레이어 캐릭터 오브젝트
	[SerializeField] Vector3 cameraPos;         // 미니 게임 마다 화면에 보여줄 카메라 위치 조정 값
	[SerializeField] Vector3 cameraRot;

	void Awake()
	{
		Material skyBox_Mini01 = Default_SkyBox;      // 스카이 박스를 가져온다.
		RenderSettings.skybox = skyBox_Mini01;       // 스카이 박스 교체

		transform.rotation = Quaternion.Euler(cameraRot);  // 카메라 초기 회전 값 조절
	}

	void LateUpdate()     // 요거 lateUpdate로 해야함..
	{
		CameraPos();
	}

	void CameraPos()
	{
		transform.position = player.transform.position + cameraPos;       // 플레이어의 위치에 따른 카메라 위치
		
		transform.position = new Vector3(Mathf.Clamp(player.position.x, -15.0f, 9.0f), player.position.y,          // 좌, 우
			Mathf.Clamp(player.position.z, -29.5f, -1.0f)) + cameraPos;                                             // 아래, 위
		// 카메라 영역 제한
	}

}
