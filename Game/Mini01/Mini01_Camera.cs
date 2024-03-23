using UnityEngine;

public class Mini01_Camera : MonoBehaviour         // ���̽� ī�޶� ���
{
	[SerializeField] Material Default_SkyBox;

	[SerializeField] Transform player;         // ī�޶� ���� �÷��̾� ĳ���� ������Ʈ
	[SerializeField] Vector3 cameraPos;         // �̴� ���� ���� ȭ�鿡 ������ ī�޶� ��ġ ���� ��
	[SerializeField] Vector3 cameraRot;

	void Awake()
	{
		Material skyBox_Mini01 = Default_SkyBox;      // ��ī�� �ڽ��� �����´�.
		RenderSettings.skybox = skyBox_Mini01;       // ��ī�� �ڽ� ��ü

		transform.rotation = Quaternion.Euler(cameraRot);  // ī�޶� �ʱ� ȸ�� �� ����
	}

	void LateUpdate()     // ��� lateUpdate�� �ؾ���..
	{
		CameraPos();
	}

	void CameraPos()
	{
		transform.position = player.transform.position + cameraPos;       // �÷��̾��� ��ġ�� ���� ī�޶� ��ġ
		
		transform.position = new Vector3(Mathf.Clamp(player.position.x, -15.0f, 9.0f), player.position.y,          // ��, ��
			Mathf.Clamp(player.position.z, -29.5f, -1.0f)) + cameraPos;                                             // �Ʒ�, ��
		// ī�޶� ���� ����
	}

}
