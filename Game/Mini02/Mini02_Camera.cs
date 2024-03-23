using UnityEngine;

public class Mini02_Camera : MonoBehaviour
{
	[SerializeField] Material Default_SkyBox;

    [SerializeField] Camera kichienCamera;

	[SerializeField] Vector3 cameraPos;
	[SerializeField] Vector3 cameraRot;
	
	void Awake()
	{
        SetResolution();

		Material skyBox_Mini02 = Default_SkyBox;      // ��ī�� �ڽ��� �����´�.
		RenderSettings.skybox = skyBox_Mini02;       // ��ī�� �ڽ� ��ü
	}

    public void SetResolution()
    {
		Camera camera = GetComponent<Camera>();     // ī�޶� ������
		Rect rect = camera.rect;                    // ī�Ŷ� ���̸� ������
		float scaleheight = ((float)Screen.width / Screen.height) / ((float)3040 / 1440); // (���� / ����)
		float scalewidth = 1f / scaleheight;
		if (scaleheight < 1)
		{
			rect.height = scaleheight;
			rect.y = (1f - scaleheight) / 2f;
		}
		else
		{
			rect.width = scalewidth;
			rect.x = (1f - scalewidth) / 2f;
		}
		camera.rect = rect;
    }
}
