using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Mini10_Camera : MonoBehaviour          // �̴� 10 ī�޶� ������
{
	[SerializeField] Material Mini10_SkyBox;

	[SerializeField] Mini10_Player mini10_Player;     // �÷��̾� ĳ����
	[SerializeField] Transform player;         // ī�޶� ���� �÷��̾� ĳ���� ������Ʈ
	[SerializeField] Vector3 cameraPos;         // �̴� ���� ���� ȭ�鿡 ������ ī�޶� ��ġ ���� ��

	[SerializeField] Image Right_Image;
	[SerializeField] Sprite sprite;

	bool isNextMove = false;              // false�� ĳ���͸� �Ѿư�, true�� ī�޶� �ٸ� ���������� ��
	bool oddEvenBool = true;               // �������� �����κ��� Ȧ���� true, ¦���� false
	float rotateSpeed = 2.0f;              // ī�޶� �̵� �ӵ�

	WaitForSeconds delay;

	Quaternion originRot;
	Quaternion mapRot;

	bool isRun = false;

	Material material;
	string skyText;

	void Awake()
	{
		Material skyBox_Mini10 = Mini10_SkyBox;      // ��ī�� �ڽ��� �����´�.
		RenderSettings.skybox = skyBox_Mini10;       // ��ī�� �ڽ� ��ü

		cameraPos = new Vector3(0, 3.0f, -3.0f);                    // ī�޶� ��ġ ����
		originRot = Quaternion.Euler(new Vector3(45.0f, 0, 0));  // ī�޶� �ʱ� ȸ�� �� ����
		mapRot = Quaternion.Euler(new Vector3(90.0f, 0, 0));
		transform.rotation = originRot;

		material = RenderSettings.skybox;
		skyText = "_Rotation";

		delay = new WaitForSeconds(10.0f);     // 5�ʰ� �� ī�޶� �ߵ�
	}

	bool isMapCamera = false;      // �� ī�޶� ���� ���̳� ���� ����

	Coroutine coroutine;

	void LateUpdate()     // ��� lateUpdate�� �ؾ���..
	{
		if (isMapCamera.Equals(true))     // �� ī�޶� �������̶�� �ؿ� ���� ����
		{
			return;
		}

		if (isNextMove.Equals(false))    
		{
			CameraPos();       // ĳ���͸� ����
		}
		else
		{
			MoveCamera();      // �ٸ� ���������� �̵�
		}
	}

	void CameraPos()
	{
		transform.position = player.position + cameraPos;       // �÷��̾��� ��ġ�� ���� ī�޶� ��ġ

	}

	void MoveCamera()        // �ٸ� ���������� �̵�
	{
		transform.position = Vector3.Lerp(transform.position, player.transform.position + cameraPos, Time.deltaTime * 3.0f);     // �����ϰ� ī�޶� �̵�
		Vector3 LerpA = transform.position;                               // ���� ī�޶� ��ġ
		Vector3 LerpB = player.transform.position + cameraPos;            // �÷��̾ �ִ� ��ġ

		if (oddEvenBool.Equals(true))                     // Ȧ�� �����������...
		{
			material.SetFloat(skyText, Time.time * rotateSpeed);   // ��ī�̹ڽ� ȸ��
		}
		else                                         // ¦�� �����������...
		{
			material.SetFloat(skyText, Time.time * -1 * rotateSpeed);   // ��ī�̹ڽ� ȸ��
		}

		if ((LerpA - LerpB).magnitude < 0.005f)         // ���� ������������ �Ÿ��� 0.005f ��, �̶��...
		{
			oddEvenBool = !oddEvenBool;                  // Ȧ�� ¦�� ����

			isNextMove = false;      // ���� �� �Ա� ������, ī�޶� �̵��� ����!

			mini10_Player.isCameraMove = false;        // �÷��̾� ��ũ��Ʈ�� ī�޶� �̵��� �����ٰ� �˸�
			return;
		}
	}

	public void StopMapCamera()     // �� ī�޶� �ڷ�ƾ�� �������̶�� ���� ��Ȱ
	{
		if (isRun.Equals(true))
		{
			isRun = false;
			StopCoroutine(coroutine);


			AudioMng.ins.PlayEffect("Back");    // ���� ����
			mini10_Player.isMapCamera = false;

			isMapCamera = false;      // �� ī�޶� ����
			transform.rotation = originRot;  // ī�޶� �ʱ� ȸ�� �� ����
		}
	}

	public void NextStageMove()
	{
		isNextMove = true;              // ī�޶� �̵��� ��Ų�ٰ� �˸�
	}

	public void MapCamera(Transform cameraPos)      // �� ī�޶� �Լ�
	{
		isMapCamera = true;                        // �� ī�޶� �������̶�� �˸�

		transform.position = cameraPos.position;                            // �޾ƿ� ��ġ�� ī�޶� ��ġ�� �ٲ�
		transform.rotation = mapRot;       // ī�޶� ȸ���� 90���� ȸ��

		coroutine = StartCoroutine(WaitMapCamera());       // �� ī�޶� �ڷ�ƾ ����
	}

	IEnumerator WaitMapCamera()                       // ���� ������ ��, 5�� �ٽ� ���ư��� �ڷ�ƾ
	{
		isRun = true;
		mini10_Player.isMapCamera = true;

		yield return delay;       // 10�� �� �ڷ�ƾ ����
		isRun = false;
		mini10_Player.isMapCamera = false;


		AudioMng.ins.PlayEffect("Back");    // ���� ����
		Right_Image.sprite = sprite;

		isMapCamera = false;      // �� ī�޶� ����
		transform.rotation = originRot;  // ī�޶� �ʱ� ȸ�� �� ����
	}
}
