using System.Collections;
using UnityEngine;

public class Mini05_Camera : MonoBehaviour
{
    [SerializeField] Material Mini05_SkyBox;

    [SerializeField] float force = 0.0f;
    [SerializeField] Vector3 offset = Vector3.zero;

	Quaternion originRotate;             // 처음 카메라 회전 값는 받는 변수
	Quaternion tempRotate;

	public Mini05_Player miniGame05_Player;       // 플레이어 스크립트를 받는다.

	bool isRun = false;           // 현재 코루틴이 실행되고 있는지 묻는 변수

	Coroutine coroutine06_1;         // 흔들리는 카메라 코루틴을 받는 변수
	Coroutine coroutine06_2;         // 원상 복구하는 코루틴을 받는 변수

    WaitForSeconds delay;


	void Awake()
	{
        Material skyBox_Mini05 = Mini05_SkyBox;      // 스카이 박스를 가져온다.
        RenderSettings.skybox = skyBox_Mini05;       // 스카이 박스 교체
    }

	void Start()
	{
		originRotate = transform.localRotation;      // 현재 카메라 회전 값을 저장

		miniGame05_Player.action += ShakeFuction;    // 카메라 흔들리는 거를 action으로 플레이어 스크립트에 집어넣음

        delay = new WaitForSeconds(1.8f);
    }

	void ShakeFuction()                      // 흔들리는 카메라를 해주는 함수
	{
		if (isRun.Equals(true))                 // 코루틴이 실행중이면..
		{
			StopCoroutine(coroutine06_1);    // 흔들리는 카메라 코루틴 중단
			StopCoroutine(coroutine06_2);    // 원상복구 코루틴 중단
		}

		coroutine06_1 = StartCoroutine(ShakeCoroutine());         // 흔들리는 카메라 코루틴 실행
		coroutine06_2 = StartCoroutine(ResetCoroutine());         // 원상복구 코루틴 실행 (정확히는 1.8초 후에 실행)
	}


    ///////////////////  코루틴 관련..


    IEnumerator ShakeCoroutine()               // 흔들리는 카메라를 해주는 코루틴
    {
        isRun = true;    // 코루틴이 실행되고 있다고 알림

        tempRotate = transform.rotation;      // 흔들리기 직전 카메라의 회전을 받아옴

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

    IEnumerator ResetCoroutine()         // 원상복구 코루틴 실행
    {
        yield return delay;    // 1.8초 후에 실행

        StopCoroutine(coroutine06_1);             // 흔들리는 코루틴은 중단시킴

        while (Quaternion.Angle(transform.rotation, tempRotate) > 0.0f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, tempRotate, force * Time.deltaTime * 2.0f);

            yield return null;
        }

        transform.localRotation = originRotate;      // 미세한 차이때문에 아예 맞춰버림

        isRun = false;    // 코루틴이 끝났다고 알려줌
    }
}
