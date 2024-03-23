using UnityEngine;

public class Mini04_Cloud : MonoBehaviour     // ���� �ټ����� ��� ������..
{
	Vector3 currentPos;               // ���� ��ġ���� ���Ʒ�
	Vector3 originPos;               // �ʱ�ȭ ��

	float delta = 0.5f;                // ���Ʒ�
	float speed = 3.0f;                // �ӵ�

	bool randBool;                     // ó���� ���� ����, �Ʒ��� ���� ���ϴ� ���� ��

	Vector3 m_Offset;
	float m_ZCoord;

	bool isTouch;                     // ���� �÷��̾ ��ġ�� �ϰ� �ִ��� ���ϰ� �ִ���?
	public bool isTouchClouds;        // ������ ��ġ�� �����Ѱ�?

	Camera cameraMain;

	void Awake()
	{
		currentPos = transform.position;         // �ϴ� �� �� ó�� ��ġ ����
		originPos = transform.position;

		cameraMain = Camera.main;
		isTouchClouds = true;
	}

	void OnEnable()          // ������ ��...
	{
		float scaleFloat = Random.Range(1.5f, 3.0f);     // ������ ũ�⸦ ��������..
		Vector3 scaleVector = new Vector3(scaleFloat, scaleFloat, scaleFloat);     // ũ�� ����
		transform.localScale = scaleVector;         // ũ�� ����

		delta = Random.Range(0.2f, 0.4f);         // �� �Ʒ� �̵� ���� ��������..
		speed = Random.Range(2.0f, 3.5f);         // ������ �ӵ��� ��������...

		randBool = (Random.value > 0.5f);         // ó���� ���� ����, �Ʒ��� ���� ���ϴ� ���� ��

		isTouch = false;
	}


	void OnDisable()          // ������ ��...
	{
		transform.position = originPos;          // ó�� ��ġ�� �ʱ�ȭ
		currentPos = originPos;
	}

	void Update()
	{
		if (isTouch.Equals(false))           // ���� �÷��̾ ��ġ�� ���ϰ� �ִٸ�?
		{
			Vector3 v = currentPos;     // ���� ��ġ�� ����

			if (randBool.Equals(true))        // ���� ���� true�̸�..
			{
				v.y += delta * Mathf.Cos(Time.time * speed * 0.2f);   // ����
			}
			else
			{
				v.y -= delta * Mathf.Cos(Time.time * speed * 0.2f);   // �Ʒ���
			}
			transform.position = v;     // ��ġ ����
		}
	}



	void OnMouseDown()      // ó�� �÷��̾ ��ġ�Ѵٸ�
	{
		m_ZCoord = cameraMain.WorldToScreenPoint(gameObject.transform.position).z;
		m_Offset = gameObject.transform.position - GetMouseWorldPosition();

		AudioMng.ins.PlayEffect("Cloud");    // ���� �Ҹ�
		isTouch = true;         // ���� �÷��̾ ��ġ�ϰ� �ִٰ� �˸�
	}

	void OnMouseDrag()      // �÷��̾ �巡�� ���̶��...
	{
		transform.position = GetMouseWorldPosition() + m_Offset;
	}

	void OnMouseUp()       // �÷��̾ ��ġ���� �ն��ٸ�...
	{
		isTouch = false;        // ���� �÷��̾ ��ġ�� ���ϰ� �ִٰ� �˸�

		currentPos = transform.position;        // ���� ��ġ�� ����

	}

	Vector3 GetMouseWorldPosition()        // ������ǥ�� ��ȯ �Լ�1
	{
		Vector3 mousePoint = Input.mousePosition;
		mousePoint.z = m_ZCoord;

		return cameraMain.ScreenToWorldPoint(mousePoint);
	}
}
