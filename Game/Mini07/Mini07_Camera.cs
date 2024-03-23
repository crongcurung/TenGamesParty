using UnityEngine;

public class Mini07_Camera : MonoBehaviour
{
	[SerializeField] Material Default_SkyBox;

	[SerializeField] Transform player;         // ī�޶� ���� �÷��̾� ĳ���� ������Ʈ

	float zPos = 5.00f;        // Z�� ���� ��

	void Awake()
	{
		Material skyBox_Mini07 = Default_SkyBox;      // ��ī�� �ڽ��� �����´�.
		RenderSettings.skybox = skyBox_Mini07;       // ��ī�� �ڽ� ��ü

		transform.position = new Vector3(0.0f, 8.5f, -7.0f);                    // ī�޶� ��ġ ����
		transform.rotation = Quaternion.Euler(new Vector3(30.00f, 0, 0));  // ī�޶� �ʱ� ȸ�� �� ����
	}


	void LateUpdate()     // ��� lateUpdate�� �ؾ���..
    {
        CameraPos();
    }



	void CameraPos()
	{
        transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z - zPos);       // ������ �� ���� ����. �߾ӿ��� �� ������ ���⸸ �Ѵ�.
    }
}
