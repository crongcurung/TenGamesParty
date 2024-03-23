using UnityEngine;

public class Mini07_Camera : MonoBehaviour
{
	[SerializeField] Material Default_SkyBox;

	[SerializeField] Transform player;         // 카메라가 잡을 플레이어 캐릭터 오브젝트

	float zPos = 5.00f;        // Z값 조정 값

	void Awake()
	{
		Material skyBox_Mini07 = Default_SkyBox;      // 스카이 박스를 가져온다.
		RenderSettings.skybox = skyBox_Mini07;       // 스카이 박스 교체

		transform.position = new Vector3(0.0f, 8.5f, -7.0f);                    // 카메라 위치 조정
		transform.rotation = Quaternion.Euler(new Vector3(30.00f, 0, 0));  // 카메라 초기 회전 값 조절
	}


	void LateUpdate()     // 요거 lateUpdate로 해야함..
    {
        CameraPos();
    }



	void CameraPos()
	{
        transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z - zPos);       // 게임이 안 끝난 경우는. 중앙에서 쭉 앞으로 가기만 한다.
    }
}
