using UnityEngine;

public class Mini03_Camera : MonoBehaviour
{
	[SerializeField] Vector3 cameraRot;
	[SerializeField] Transform playerTrans;
	[SerializeField] Vector3 cameraPos;         // 미니 게임 마다 화면에 보여줄 카메라 위치 조정 값

	void Awake()
	{
		transform.rotation = Quaternion.Euler(cameraRot);  // 카메라 초기 회전 값 조절
	}

	void LateUpdate()     // 요거 lateUpdate로 해야함..
	{
		CameraPos();
	}

	void CameraPos()
	{
		transform.position = playerTrans.position + cameraPos;       // 플레이어의 위치에 따른 카메라 위치
	}
}
