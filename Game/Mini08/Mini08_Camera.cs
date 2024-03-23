using UnityEngine;

public class Mini08_Camera : MonoBehaviour
{
	[SerializeField] Material Default_SkyBox;

	[SerializeField] Transform player;         // ī�޶� ���� �÷��̾� ĳ���� ������Ʈ
	[SerializeField] Vector3 cameraPos;         // �̴� ���� ���� ȭ�鿡 ������ ī�޶� ��ġ ���� ��

	void Awake()
	{
		Material skyBox_Mini08 = Default_SkyBox;      // ��ī�� �ڽ��� �����´�.
		RenderSettings.skybox = skyBox_Mini08;       // ��ī�� �ڽ� ��ü

		cameraPos = new Vector3(0.0f, 5.5f, -5.5f);                    // ī�޶� ��ġ ����
		transform.rotation = Quaternion.Euler(new Vector3(45.0f, 0, 0));  // ī�޶� �ʱ� ȸ�� �� ����
	}


	void LateUpdate()     // ��� lateUpdate�� �ؾ���..
	{
		CameraPos();
	}

	void CameraPos()
	{
		transform.position = player.position + cameraPos;       // �÷��̾��� ��ġ�� ���� ī�޶� ��ġ


		transform.position = new Vector3(Mathf.Clamp(player.position.x, -14.5f, 14.5f), player.position.y,          // ��, ��
			Mathf.Clamp(player.position.z, -15.0f, 16.0f)) + cameraPos;                                             // �Ʒ�, ��
																											  // ī�޶� ���� ����
	}
}
