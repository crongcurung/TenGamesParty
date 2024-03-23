using System.Collections;
using UnityEngine;

public class Mini05_Camera : MonoBehaviour
{
    [SerializeField] Material Mini05_SkyBox;

    [SerializeField] float force = 0.0f;
    [SerializeField] Vector3 offset = Vector3.zero;

	Quaternion originRotate;             // ó�� ī�޶� ȸ�� ���� �޴� ����
	Quaternion tempRotate;

	public Mini05_Player miniGame05_Player;       // �÷��̾� ��ũ��Ʈ�� �޴´�.

	bool isRun = false;           // ���� �ڷ�ƾ�� ����ǰ� �ִ��� ���� ����

	Coroutine coroutine06_1;         // ��鸮�� ī�޶� �ڷ�ƾ�� �޴� ����
	Coroutine coroutine06_2;         // ���� �����ϴ� �ڷ�ƾ�� �޴� ����

    WaitForSeconds delay;


	void Awake()
	{
        Material skyBox_Mini05 = Mini05_SkyBox;      // ��ī�� �ڽ��� �����´�.
        RenderSettings.skybox = skyBox_Mini05;       // ��ī�� �ڽ� ��ü
    }

	void Start()
	{
		originRotate = transform.localRotation;      // ���� ī�޶� ȸ�� ���� ����

		miniGame05_Player.action += ShakeFuction;    // ī�޶� ��鸮�� �Ÿ� action���� �÷��̾� ��ũ��Ʈ�� �������

        delay = new WaitForSeconds(1.8f);
    }

	void ShakeFuction()                      // ��鸮�� ī�޶� ���ִ� �Լ�
	{
		if (isRun.Equals(true))                 // �ڷ�ƾ�� �������̸�..
		{
			StopCoroutine(coroutine06_1);    // ��鸮�� ī�޶� �ڷ�ƾ �ߴ�
			StopCoroutine(coroutine06_2);    // ���󺹱� �ڷ�ƾ �ߴ�
		}

		coroutine06_1 = StartCoroutine(ShakeCoroutine());         // ��鸮�� ī�޶� �ڷ�ƾ ����
		coroutine06_2 = StartCoroutine(ResetCoroutine());         // ���󺹱� �ڷ�ƾ ���� (��Ȯ���� 1.8�� �Ŀ� ����)
	}


    ///////////////////  �ڷ�ƾ ����..


    IEnumerator ShakeCoroutine()               // ��鸮�� ī�޶� ���ִ� �ڷ�ƾ
    {
        isRun = true;    // �ڷ�ƾ�� ����ǰ� �ִٰ� �˸�

        tempRotate = transform.rotation;      // ��鸮�� ���� ī�޶��� ȸ���� �޾ƿ�

        Vector3 originEuler = transform.eulerAngles;

        while (true)
        {
            float rotX = Random.Range(-offset.x, offset.x);
            float rotY = Random.Range(-offset.y, offset.y);
            float rotZ = Random.Range(-offset.z, offset.z);

            Vector3 randomRotate = originEuler + new Vector3(rotX, rotY, rotZ);
            Quaternion rot = Quaternion.Euler(randomRotate);

            while (Quaternion.Angle(transform.rotation, rot) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, force * Time.deltaTime);

                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator ResetCoroutine()         // ���󺹱� �ڷ�ƾ ����
    {
        yield return delay;    // 1.8�� �Ŀ� ����

        StopCoroutine(coroutine06_1);             // ��鸮�� �ڷ�ƾ�� �ߴܽ�Ŵ

        while (Quaternion.Angle(transform.rotation, tempRotate) > 0.0f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, tempRotate, force * Time.deltaTime * 2.0f);

            yield return null;
        }

        transform.localRotation = originRotate;      // �̼��� ���̶����� �ƿ� �������

        isRun = false;    // �ڷ�ƾ�� �����ٰ� �˷���
    }
}
