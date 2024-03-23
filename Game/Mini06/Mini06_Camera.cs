using UnityEngine;

public class Mini06_Camera : MonoBehaviour
{
	[SerializeField] Material Default_SkyBox;

	[SerializeField] Transform player;         // ī�޶� ���� �÷��̾� ĳ���� ������Ʈ
	[SerializeField] Vector3 cameraPos;         // �̴� ���� ���� ȭ�鿡 ������ ī�޶� ��ġ ���� ��

	void Awake()
	{
		Material skyBox_Mini06 = Default_SkyBox;      // ��ī�� �ڽ��� �����´�.
		RenderSettings.skybox = skyBox_Mini06;       // ��ī�� �ڽ� ��ü

		transform.rotation = Quaternion.Euler(new Vector3(35.0f, 0, 0));  // ī�޶� �ʱ� ȸ�� �� ����
	}

	[SerializeField] float a1;
	[SerializeField] float a2;

	[SerializeField] float b1;
	[SerializeField] float b2;

	void LateUpdate()     // ��� lateUpdate�� �ؾ���..
	{
		CameraPos();
	}

	void CameraPos()
	{
		transform.position = player.position + cameraPos;       // �÷��̾��� ��ġ�� ���� ī�޶� ��ġ

		transform.position = new Vector3(Mathf.Clamp(player.position.x, -15, 15), player.position.y,          // ��, ��
			Mathf.Clamp(player.position.z, -13, 13)) + cameraPos;                                             // �Ʒ�, ��
																													// ī�޶� ���� ����
	}
}
