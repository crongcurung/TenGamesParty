using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mini02_OvenInput : MonoBehaviour
{
    [SerializeField] Material[] Mat_Array;

    [SerializeField] Mini02_Player mini02_Player;

    public bool isOvenInput = false;

    [SerializeField] GameObject ovenPanel;            // 오븐 패널

    [SerializeField] Image right_Image;
    [SerializeField] Sprite[] sprite_Array;      // 0 : 오븐 인, 오븐 아웃, 뺵

    IEnumerator coroutine01;               // 코루틴 초기화

    bool isInput = false;              // 오븐에 도넛을 넣었는지 묻는 변수
    bool isSuccess = false;             // 도넛을 오븐에 넣고 코루틴이 끝났는지(성공) 묻는 변수

    bool failTry = false;              // 왔다갔다 할려고 변수를 둠
    
    bool endButton = false;            // 끝났다고 알리는 변수

    [SerializeField] GameObject dragDough;          // 드래그 도넛(이미지) 

    [SerializeField] Transform Oven_Door;        // 오븐의 문 위치
    [SerializeField] Transform Tray;             // 오븐 쟁반 위치
    [SerializeField] Transform Tray_Pos_One;     // 처음 쟁반 위치
    [SerializeField] Transform Tray_Pos_Two;     // 두번째 쟁반 위치

    [SerializeField] GameObject One_Donut;          // 날아다니는 구멍 도넛
    [SerializeField] GameObject Star_Donut;         // 날아다니는 스타 도넛

    [SerializeField] Renderer One_Donut_Render;
    [SerializeField] Renderer Star_Donut_Render;

    [SerializeField] Animator One_Donut_Anim;
    [SerializeField] Animator Star_Donut_Anim;

    [SerializeField] MeshRenderer Oven_Renderer;        // 오븐 메테리얼을 바꾸기 위한 렌더러
    Material Oven_Mat_Yellow;          // 노랑불 오븐 메테리얼
    Material Oven_Mat_Red;             // 빨강불 오븐 메테리얼
    Material Oven_Mat_Green;           // 초록불 오븐 메테리얼

    Material Donut_One_Toast_Mat;      // 구멍, 오븐 도넛 메테리얼
    Material Donut_Star_Toast_Mat;     // 스타, 오븐 도넛 메테리얼

    Material origin_Mat;         // 원본 메테리얼을 담을 곳
    Renderer origin_Render;      // 원본 렌더를 담을 곳

    Animator anim;                // 날아다니는 도넛의 애니메이션을 받는 변수
    Vector3 origin_Donut_Pos;     // 도넛 원래 위치
    Quaternion origin_Donut_Rot;  // 도넛 원래 회전

    [SerializeField] GameObject LightInOven;     // 오븐 안에 있는 불빛

    bool isInDonut = false;        // 오븐에 도넛을 넣었는지 묻는 변수
    float x = 0.0f;

    WaitForSeconds delay01;
    WaitForSeconds delay02;
    WaitForSeconds delay03;

    int endId;             // 애니메이션 최적화

    Quaternion doorRot;      // 오븐 문 회전


    void Awake()
	{
        endId = Animator.StringToHash("isEnd");
        doorRot = Quaternion.Euler(Vector3.zero);      // 오븐 문 회전

        Oven_Mat_Yellow = Mat_Array[0];
        Oven_Mat_Red = Mat_Array[1];
        Oven_Mat_Green = Mat_Array[2];

        Donut_One_Toast_Mat = Mat_Array[3];
        Donut_Star_Toast_Mat = Mat_Array[4];

        delay01 = new WaitForSeconds(0.1f);
        delay02 = new WaitForSeconds(5.0f);
        delay03 = new WaitForSeconds(0.3f);
    }


	void Update()
    {
        if (isInDonut.Equals(true))     // 오븐에 도넛을 넣었다면..
        {
            if (x <= 90.0f)
            {
                x += Time.deltaTime * 500.0f;
                Oven_Door.rotation = Quaternion.Euler(new Vector3(x, 0, 0));      // 오븐 도어를 직각으로 한다.
            }
            else
            {
                isInDonut = false;   // 초기화

                if (LightInOven.activeSelf.Equals(false))     // 라이트가 활성화되지 않았다면..
                {
                    LightInOven.SetActive(true);         // 라이트 활성화
                }
            }
        }
    }

    void OnEnable()        // 켜질떄...
    {
        dragDough.SetActive(true);           // 드래그 다시 키기
        coroutine01 = WaitOvenInput();       // 코루틴 초기화

        Oven_Renderer.material = Oven_Mat_Yellow;                    // 오븐 메테리얼을 다시 노란불로 바꾼다.(초기화)

        if (mini02_Player.isHoleOrStar.Equals(false))       // 구멍 반죽
        {
            anim = One_Donut_Anim;               // 구멍 도넛 애니메이션 넘김
            origin_Render = One_Donut_Render;      // 구멍 도넛 렌더러를 넘김
            origin_Mat = origin_Render.material;                     // 구멍 도넛(기존) 메테리얼을 넘김

            origin_Donut_Pos = One_Donut.transform.localPosition;    // 구멍 도넛 로컬 위치를 넘김
            origin_Donut_Rot = One_Donut.transform.localRotation;
        }
        else                            // 스타 반죽
        {

            anim = Star_Donut_Anim;               // 스타 도넛 애니메이션 넘김
            origin_Render = Star_Donut_Render;      // 스타 도넛 렌더러를 넘김
            origin_Mat = origin_Render.material;                      // 스타 도넛(기존) 메테리얼을 넘김

            origin_Donut_Pos = Star_Donut.transform.localPosition;    // 스타 도넛 로컬 위치를 넘김
            origin_Donut_Rot = Star_Donut.transform.localRotation;
        }
        StartCoroutine(updateCoroutine());
    }

    void OnDisable()      // 끝날떄..
    {
        isInput = false;       // 초기화
        failTry = false;
        isSuccess = false;
        endButton = false;                     // 끝났다는 변수 초기화
        StopCoroutine(coroutine01);

        if (mini02_Player.isHoleOrStar.Equals(false))           // 구멍 도넛
        {
            origin_Render.material = origin_Mat;       // 도넛 메테리얼을 초기화한다.

            One_Donut.transform.localPosition = origin_Donut_Pos;       // 구멍 도넛을 초기화 한다.
            One_Donut.transform.localRotation = origin_Donut_Rot;
        }
        else                                 // 스타 도넛
        {
            origin_Render.material = origin_Mat;       // 도넛 메테리얼을 초기화한다.

            Star_Donut.transform.localPosition = origin_Donut_Pos;      // 스타 도넛을 초기화 한다.
            Star_Donut.transform.localRotation = origin_Donut_Rot;
        }

        right_Image.sprite = sprite_Array[0];

        isInDonut = false;     // 초기화(신작)
        Oven_Door.rotation = doorRot;     // 회전 초기화
        x = 0.0f;
        Tray.position = Tray_Pos_One.position;    // 쟁반 위치를 초기화
        One_Donut.SetActive(false);          // 날아다니는 도넛 비활성화
        Star_Donut.SetActive(false);

        LightInOven.SetActive(false);        // 라이트를 끈다.
        
        anim.SetBool(endId, false);        // 애니메이션 초기화
    }

    IEnumerator updateCoroutine()                         // 최적화 코루틴
    {
        while (true)
        {
            if (isOvenInput.Equals(true))      // 오븐에 도넛을 넣었는지..?
            {
                isOvenInput = false;      // 한번만 하기 때문에 바로 꺼버린다.
                
                isInDonut = true;                                 // 오븐에 닿았다고 알려줌(신작)
                Tray.position = Tray_Pos_Two.position;            // 쟁반 위치를 오븐 안으로

                isInput = true;                      // 오븐에 도넛을 넣었다고 알려줌
                StartCoroutine(coroutine01);         // 바로 5초를 세라고 코루틴 시작!!
            }

            yield return delay01;
        }
    }

    IEnumerator WaitOvenInput()                 // 도넛을 오븐에 넣고 난 다음 코루틴 
    {
        right_Image.sprite = sprite_Array[1];

        yield return delay02;

        isSuccess = true;        // 코루틴 시작 후, 5초가 지나면 성공으로 판정 함.
        Oven_Renderer.material = Oven_Mat_Green;                // 오븐 메테리얼을 초록불 메테리얼로 바꾼다.
        
        if (mini02_Player.isHoleOrStar.Equals(false))         // 구멍 도넛
        {
            origin_Render.material = Donut_One_Toast_Mat;       // 구멍, 오븐 메테리얼을 넘김
        }
        else                              // 스타 도넛
        {
            origin_Render.material = Donut_Star_Toast_Mat;      // 스타, 오븐 메테리얼을 넘김
        }
    }


    public void EndButton()
    {
        ovenPanel.SetActive(false);       // 오븐 패널 비활성화
        mini02_Player.Origin_Panel();
    }

    public void Press_Button()            // 오븐 패널에 있는 메인 버튼에 등록
    {
        if (endButton.Equals(true))                 // 끝났으면 메인으로..
        {
            EndButton();
            return;
        }

        if (isInput.Equals(true) && failTry.Equals(true))   // 버튼을 눌렀을 때, 도넛이 오븐 안에 있고. failTry가 true인 경우
        {
            AudioMng.ins.LoopEffect(true);
            AudioMng.ins.PlayEffect("Oven");

            

            isInDonut = true;     // 오븐에 닿았다고 알려줌(신작)
            Tray.position = Tray_Pos_Two.position;       // 쟁반 위치를 오븐 안으로
            Oven_Renderer.material = Oven_Mat_Yellow;    // 오븐 메테리얼을 노란불로 바꾼다.

            failTry = false;

            StartCoroutine(coroutine01);         // 다시 코루틴 시작!
            return;
        }


        if (isInput.Equals(true) && failTry.Equals(false))   // 버튼을 눌렀을 때, 도넛이 오븐 안에 있고. failTry가 false인 경우
        {
            AudioMng.ins.StopEffect();

            if (isSuccess.Equals(false))          // 아직 성공하지 않는 경우...
            {
                StopCoroutine(coroutine01);        // 코루틴 초기화
                coroutine01 = WaitOvenInput();     // 코루틴 초기화

                Oven_Door.rotation = doorRot;    // 회전 90도
                
                x = 0.0f;
                Tray.position = Tray_Pos_One.position;                   // 쟁반 위치 초기화
                Oven_Renderer.material = Oven_Mat_Red;                   // 오븐 메테리얼을 빨강불로 바꾼다.

                right_Image.sprite = sprite_Array[0];

                failTry = true;                // 성공하지 못했다고 알려줌
                LightInOven.SetActive(false);    // 라이트를 비활성화

                return;
            }

            AudioMng.ins.LoopEffect(false);
            AudioMng.ins.PlayEffect("SpeedUp");       // 이동 애니메이션 실행

            right_Image.sprite = sprite_Array[2];

            mini02_Player.playerCompleteInt = 3;     // 토핑 패널로 가라고 알림

            mini02_Player.isOvenOrFryer = false;          // true면 튀김, false면 오븐에서 했다는 뜻
            //
            Oven_Door.rotation = doorRot;    // 회전 90도

            LightInOven.SetActive(false);    // 라이트를 비활성화
            Tray.position = Tray_Pos_One.position;          // 쟁반 위치 초기화
            endButton = true;                         // 끝났다고 알려줌
            StartCoroutine(WaitComplete());
        }
    }

    IEnumerator WaitComplete()
    {
        yield return delay03;

        anim.SetBool(endId, true);
    }
}
