using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Mini05_Player : MonoBehaviour
{
    [SerializeField] Joystic joystic;

    bool isRun_BreakThrough = false;       // 지금 뚫어뻥을 쏴도 되는지 묻는 변수
    bool isRun_DonutBomb = false;          // 지금 도넛 폭탄을 쏴도 되는지 묻는 변수
    bool isRun_Tonedo = false;             // 지금 토네이도을 쏴도 되는지 묻는 변수

    [SerializeField] Mini05_Spawn mini05_Spawn;         // 스폰 스크립트를 가져온다.

    Transform bulletPos;               // 탄환들의 처음 스폰 장소

    public Image BreakThrough_Image;      // 뚫어뻥 오른쪽 버튼 이미지
    public Image DonutBomb_Image;         // 도넛 폭탄 오른쪽 버튼 이미지
    public Image Tonedo_Image;            // 토네이도 오른쪽 버튼 이미지


    public Mini05_CrossHair crossHair_Break;          // 크로스 헤어(뚫어뻥) 스크립트... 애니메이션 때문에..
    public Mini05_CrossHair crossHair_Donut;          // 크로스 헤어(도넛 폭탄) 스크립트... 애니메이션 때문에..

    public Mini05_Swipe mini05_Swipe;                 // 스와이프 하나만 가져온다.

    public Action action;     // 인보크를 받는 변수(카메라 흔들리는거)

    bool isRun = false;         // 카메라 흔들리는 코루틴이 실행되고 있는지 묻는 변수

    Coroutine coroutine05;          // 카메라 흔들리는 코루틴 변수
	WaitForSeconds delay_Camera;    // 카메라 흔들리는 코루틴 시간

    Vector3 moveDir;                     // 이동 변수는 플레이어가 지닐 가능성 높음

    [SerializeField] float speed;        // 이동 속도 변수는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    string tag01;

    string hor_Text;
    string ver_Text;

    void Start()
    {
        bulletPos = transform.GetChild(3).transform;                                              // 무기 발사 지점을 받아 놓는다.

        tag01 = "Cushion";

        hor_Text = "Horizontal";
        ver_Text = "Vertical";

        delay_Camera = new WaitForSeconds(2.0f);       // 카메라 흔들리는 것은 2초간...

        AudioMng.ins.Play_BG("Mini05_B");
    }

    void FixedUpdate()
    {
        if (isRun.Equals(true))          // 카메라 흔들리는 코루틴이 실행 중이라면...
        {
            return;                      // 아무 것도 못하게 리턴!!
        }

        if (!moveDir.x.Equals(0) || !moveDir.z.Equals(0))               // wasd 아무거나라도 움직인다면..
        {
            Left();            // 좌우 회전 및 제한
            MoveZ();           // 위, 아래 회전 및 제한

            if (mini05_Swipe.currentWeaponInt.Equals(1))      // 뚫어뻥
            {
                crossHair_Break.MovingState(true);              // 크로스 헤어 늘어나는 애니메이션 실행(이동)
            }
            else if (mini05_Swipe.currentWeaponInt.Equals(2))      // 도넛 폭탄
            {
                crossHair_Donut.MovingState(true);              // 크로스 헤어 늘어나는 애니메이션 실행(이동)
            }
        }
        else
        {
            if (mini05_Swipe.currentWeaponInt.Equals(1))      // 뚫어뻥
            {
                crossHair_Break.MovingState(false);              // 크로스 헤어 줄어드는 애니메이션 실행(이동)
            }
            else if (mini05_Swipe.currentWeaponInt.Equals(2))      // 도넛 폭탄
            {
                crossHair_Donut.MovingState(false);              // 크로스 헤어 줄어드는 애니메이션 실행(이동)
            }
        }
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.KeypadEnter) && isRun.Equals(false))
        {
            Press_RightButton();
        }
        else                                                  
        {
            if (mini05_Swipe.currentWeaponInt.Equals(1))      // 뚫어뻥
            {
                crossHair_Break.Shooting(false);              // 크로스 헤어 줄어드는 애니메이션 실행(이동)
            }
            else if (mini05_Swipe.currentWeaponInt.Equals(2))        // 도넛 폭탄
            {
                crossHair_Donut.Shooting(false);              // 크로스 헤어 줄어드는 애니메이션 실행(이동)
            }
        }
    }

    void Move()         // 이동 함수를 부모걸로 씀
    {
#if (UNITY_EDITOR)
        float hor = Input.GetAxis(hor_Text);
        float ver = Input.GetAxis(ver_Text);
#elif (UNITY_IOS || UNITY_ANDROID)
		float hor = joystic.Horizontal;
        float ver = joystic.Vertical;
#endif

        moveDir.x = hor;
        moveDir.z = ver;
    }





    void Left()       // 좌우 회전 및 제한
    {
        if (transform.localEulerAngles.y > 250.0f && transform.localEulerAngles.y < 271.5f && moveDir.x < 0.0f)              // 왼쪽 회전 제한
        {
            return;
        }

        if (transform.localEulerAngles.y < 180.0f && transform.localEulerAngles.y > 88.5f && moveDir.x > 0.0f)              // 오른쪽 회전 제한
        {
            return;
        }

		Vector3 eulerAngle = new Vector3(0f, Time.fixedDeltaTime * moveDir.x * speed, 0f);   // 회전
		transform.Rotate(eulerAngle, Space.World);
		Quaternion rot = transform.rotation;
		transform.rotation = Quaternion.Euler(eulerAngle) * rot;
	}

    void MoveZ()        // 위, 아래 회전 및 제한
    {
        if (transform.localEulerAngles.z < 200.0f && transform.localEulerAngles.z > 45.0f && moveDir.z > 0.0f)    // 위 회전 제한
        {
            return;
        }

        if (transform.localEulerAngles.z > 70.0f && transform.localEulerAngles.z < 300.0f && moveDir.z < 0.0f)    // 아래 회전 제한
        {
            return;
        }
        Vector3 eulerAngle = new Vector3(0f, 0f, Time.fixedDeltaTime * moveDir.z * speed);   // 회전
        transform.Rotate(eulerAngle, Space.Self);
        transform.localRotation *= Quaternion.Euler(eulerAngle);
    }
    

    public void Press_RightButton()                       // 발사, 오른쪽 버튼을 누른다면...
    {
        if (mini05_Swipe.currentWeaponInt.Equals(1))      // 뚫어뻥
        {
            if (isRun_BreakThrough.Equals(false))             // 뚫어뻥 재 사용 시간이 지났다면...
            {
                AudioMng.ins.PlayEffect("Dough");    // 뚫어뻥 발사 소리
                StartCoroutine(Shot_Coroutine(0));            // 발사 코루틴 실행(뚫어뻥)
                crossHair_Break.Shooting(true);               // 뚫어뻥 크로스 헤어 애니메이션 실행(공격)
            }
        }
        else if (mini05_Swipe.currentWeaponInt.Equals(2))     // 도넛 폭탄
        {
            if (isRun_DonutBomb.Equals(false))             // 도넛 폭탄 재 사용 시간이 지났다면...
            {
                AudioMng.ins.PlayEffect("TrainSide");    // 폭탄 발사 소리
                StartCoroutine(Shot_Coroutine(1));            // 발사 코루틴 실행(도넛 폭탄)
                crossHair_Donut.Shooting(true);               // 도넛 폭탄 크로스 헤어 애니메이션 실행(공격)
            }
        }
        else                                                  // 토네이도
        {
            if (isRun_Tonedo.Equals(false))                     // 토네이도 재 사용 시간이 지났다면...
            {
                AudioMng.ins.PlayEffect("SpeedUp");    // 토네이도 발사 소리
                StartCoroutine(Shot_Coroutine(2));            // 발사 코루틴 실행(토네이도)
            }
        }
    }



    ////////////////////////  코루틴 관련...........

    IEnumerator StopPlayer()     // 플레이어 동작은 잠시 멈춤
    {
        isRun = true;             // 카메라 흔들리는 코루틴이 실행되고 있다고 알림
        action?.Invoke();         // 인보크로 알림(카메라 흔들리는 거를 받음)

        AudioMng.ins.PlayEffect("HitArrow");    // 화살에 맞는 소리
        yield return delay_Camera;     // 2초 정도 멈춘 다음에..
        isRun = false;            // 코루틴이 끝났다고 알림
    }



    IEnumerator Shot_Coroutine(int weaponNum)      // 0 : 뚫어뻥, 1 : 도넛 폭탄, 2 : 토네이도
    {
        GameObject t_object;
        Image tempImage;
        float tempFloat;

        if (weaponNum.Equals(0))       // 뚫어뻥
        {
            isRun_BreakThrough = true;          // 현재 코루틴 실행중이니, 뚫어뻥을 발사할 수 없다고 알림
            t_object = mini05_Spawn.GetQueue_BreakThrough();      // 뚫어뻥 오브젝트 풀링에서 가져옴
            tempImage = BreakThrough_Image;                       // 오른쪽 아래에 뚫어뻥 이미지를 잠시 담는다.
            tempFloat = 0.8f;                                     // 뚫어뻥 대기 시간을 담는다.
        }
        else if (weaponNum.Equals(1))   // 도넛 폭탄
        {
            isRun_DonutBomb = true;          // 현재 코루틴 실행중이니, 도넛 폭탄을 발사할 수 없다고 알림
            t_object = mini05_Spawn.GetQueue_DonutBomb();      // 도넛폭탄 오브젝트 풀링에서 가져옴
            tempImage = DonutBomb_Image;                       // 오른쪽 아래에 도넛 폭탄 이미지를 잠시 담는다.
            tempFloat = 5.0f;                                  // 도넛 폭탄 대기 시간을 담는다.
        }
        else                            // 토네이도
        {
            isRun_Tonedo = true;          // 현재 코루틴 실행중이니, 토네이도를 발사할 수 없다고 알림
            t_object = mini05_Spawn.GetQueue_Tonedo();      // 토네이도 오브젝트 풀링에서 가져옴
            tempImage = Tonedo_Image;                       // 오른쪽 아래에 토네이도 이미지를 잠시 담는다.
            tempFloat = 10.0f;                              // 토네이도 대기 시간을 담는다.
        }

        t_object.transform.position = bulletPos.position;     // 발사대에서 시작
        t_object.transform.rotation = bulletPos.rotation;     // 현재 발사대 회전으로 맞춤

        Rigidbody bulletRigid = t_object.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.right * 100.0f;      // 발사대에 오른쪽?으로 발사


        tempImage.fillAmount = 0.0f;     // 뚫어뻥 오른쪽 버튼 이미지
        while (tempImage.fillAmount <= 0.9999f)          // 1로 하면 안된다;;;
        {
            tempImage.fillAmount += Time.deltaTime / tempFloat;      // 잠시 시간 담는 걸로 대기시간을 잰다.

            yield return null;
        }

        if (weaponNum.Equals(0))        // 뚫어뻥
        {
            isRun_BreakThrough = false;          // 현재 코루틴 실행중이니, 뚫어뻥을 발사할 수 없다고 알림
        }
        else if (weaponNum.Equals(1))        // 도넛 폭탄
        {
            isRun_DonutBomb = false;          // 현재 코루틴 실행중이니, 도넛 폭탄을 발사할 수 없다고 알림
        }
        else                            // 토네이도
        {
            isRun_Tonedo = false;          // 현재 코루틴 실행중이니, 토네이도를 발사할 수 없다고 알림
        }

        yield return null;
    }


    ////////////////////////////// 트리거 구역....

	void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag(tag01))       // 화살에 맞았을 경우....
        {
            if (isRun.Equals(true))           // 카메라 흔들리는 코루틴이 실행되고 있으면...
            {
                StopCoroutine(coroutine05);          // 실행되고 있던 코루틴 중단
            }
            coroutine05 = StartCoroutine(StopPlayer());     // 카메라 흔들리는 코루틴 실행
        }
    }
}
