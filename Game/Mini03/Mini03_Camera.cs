using UnityEngine;

public class Mini03_Camera : MonoBehaviour
{
	[SerializeField] Vector3 cameraRot;
	[SerializeField] Transform playerTrans;
	[SerializeField] Vector3 cameraPos;         // �̴� ���� ���� ȭ�鿡 ������ ī�޶� ��ġ ���� ��

	void Awake()
	{
		transform.rotation = Quaternion.Euler(cameraRot);  // ī�޶� �ʱ� ȸ�� �� ����
	}

	void LateUpdate()     // ��� lateUpdate�� �ؾ���..
	{
		CameraPos();
	}

	void CameraPos()
	{
		transform.position = playerTrans.position + cameraPos;       // �÷��̾��� ��ġ�� ���� ī�޶� ��ġ
	}
}
