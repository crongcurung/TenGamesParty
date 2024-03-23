using UnityEngine;

public class Mini06_Camera : MonoBehaviour
{
	[SerializeField] Material Default_SkyBox;

	[SerializeField] Transform player;         // 카메라가 잡을 플레이어 캐릭터 오브젝트
	[SerializeField] Vector3 cameraPos;         // 미니 게임 마다 화면에 보여줄 카메라 위치 조정 값

	void Awake()
	{
		Material skyBox_Mini06 = Default_SkyBox;      // 스카이 박스를 가져온다.
		RenderSettings.skybox = skyBox_Mini06;       // 스카이 박스 교체

		transform.rotation = Quaternion.Euler(new Vector3(35.0f, 0, 0));  // 카메라 초기 회전 값 조절
	}

	[SerializeField] float a1;
	[SerializeField] float a2;

	[SerializeField] float b1;
	[SerializeField] float b2;

	void LateUpdate()     // 요거 lateUpdate로 해야함..
	{
		CameraPos();
	}

	void CameraPos()
	{
		transform.position = player.position + cameraPos;       // 플레이어의 위치에 따른 카메라 위치

		transform.position = new Vector3(Mathf.Clamp(player.position.x, -15, 15), player.position.y,          // 좌, 우
			Mathf.Clamp(player.position.z, -13, 13)) + cameraPos;                                             // 아래, 위
																													// 카메라 영역 제한
	}
}
