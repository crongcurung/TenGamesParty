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

		Material skyBox_Mini02 = Default_SkyBox;      // 스카이 박스를 가져온다.
		RenderSettings.skybox = skyBox_Mini02;       // 스카이 박스 교체
	}

    public void SetResolution()
    {
		Camera camera = GetComponent<Camera>();     // 카메라를 가져옴
		Rect rect = camera.rect;                    // 카매라 길이를 가져옴
		float scaleheight = ((float)Screen.width / Screen.height) / ((float)3040 / 1440); // (가로 / 세로)
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
