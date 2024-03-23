using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mini02_FryInput : MonoBehaviour
{
    [SerializeField] Material[] Mat_Array;

    [SerializeField] Mini02_Player mini02_Player;      // 플레이어 스크립트

    public bool isFryerInput = false;         // 드래그 이미지를 튀김기 안에 넣었는지 묻는 변수

    public GameObject fryerPanel;          // 튀김 패널

    bool isCorrect01 = false;     // 앞면이 성공했는지 묻는 변수
    bool isCorrect02 = false;     // 뒷면이 성공했는지 묻는 변수

    bool isFront = true;              // 도넛이 앞면인지 묻는 변수
    bool isInput = false;             // 드래그 도넛이 튀김기에 넣었는지 묻는 변수

    [SerializeField] Image right_Image;
    [SerializeField] Sprite[] sprite_Array;          // 0 : 튀김, 1 : 뺵

    IEnumerator coroutine01;                 // 코루틴 초기화
    IEnumerator coroutine02;                 // 코루틴 초기화

    [SerializeField] Button mainButton;        // 튀김 패널에 있는 버튼

    bool isSuccess = false;               // 앞, 뒤 성공했을 시에 쓰일 최적화 변수

    [SerializeField] GameObject One_Donut;     // 프라이팬 안에 구멍 도넛
    [SerializeField] Renderer One_Donut_01;    // 프라이팬 안에 구멍 도넛의 윗면
    [SerializeField] Renderer One_Donut_02;    // 프라이팬 안에 구멍 도넛의 뒷면
    [SerializeField] Animator One_Donut_Anim;

    [SerializeField] GameObject Star_Donut;    // 프라이팬 안에 스타 도넛
    [SerializeField] Renderer Star_Donut_01;   // 프라이팬 안에 스타 도넛의 윗면
    [SerializeField] Renderer Star_Donut_02;   // 프라이팬 안에 스타 도넛의 뒷면
    [SerializeField] Animator Star_Donut_Anim;

    Material One_Donut_Fry_Mat_01;     // 구멍 도넛 윗면 프라이 메테리얼
    Material One_Donut_Fry_Mat_02;     // 구멍 도넛 뒷면 프라이 메테리얼
    Material Star_Donut_Fry_Mat_01;    // 스타 도넛 윗면 프라이 메테리얼
    Material Star_Donut_Fry_Mat_02;    // 스타 도넛 뒷면 프라이 메테리얼

    Material One_Donut_Origin_Mat;       // 구멍 도넛 기본 메테리얼
    Material Star_Donut_Origin_Mat;      // 스타 도넛 기본 메테리얼

    Animator anim;          // 해당 도넛의 애니메이션을 받는 변수
    Vector3 originPos;      // 도넛 원래 위치
    Quaternion originRot;   // 도넛 원래 회전

    WaitForSeconds delay01;     // 업데이트 코루틴
    WaitForSeconds delay02;     // 익은 도넛으로 코루틴
    WaitForSeconds delay03;     // 회전시 버튼을 막는 코루틴

    string anim01;    // 애니메이터 최적화
    string anim02;
    string anim03;

    void Awake()
	{
        anim01 = "Mini02_Fry_01";       // 튓면으로 가는 애니메이션
        anim02 = "Mini02_Fry_02";          // 앞면으로 가는 애니메이션
        anim03 = "Mini02_Fry_03";     // 날아가는 애니메이션

        One_Donut_Fry_Mat_01 = Mat_Array[0];
        One_Donut_Fry_Mat_02 = Mat_Array[1];
        Star_Donut_Fry_Mat_01 = Mat_Array[2];
        Star_Donut_Fry_Mat_02 = Mat_Array[3];
        One_Donut_Origin_Mat = Mat_Array[4];
        Star_Donut_Origin_Mat = Mat_Array[5];

        delay01 = new WaitForSeconds(0.1f);
        delay02 = new WaitForSeconds(2.5f);      // 코루틴이 실행되고 5초 후 안 익은 도넛 반죽이, 익은 반죽으로 변화
        delay03 = new WaitForSeconds(0.5f);
    }

	void OnEnable()       // 켜질 때...
    {
        if (mini02_Player.isHoleOrStar.Equals(false))
        {
            anim = One_Donut_Anim;      // 구멍 도넛의 애니메이션을 넘긴다.
            originPos = One_Donut.transform.position;       // 구멍 도넛 위치를 넘긴다.
            originRot = One_Donut.transform.rotation;
        }
        else
        {
            anim = Star_Donut_Anim;     // 스타 도넛의 애니메이션을 넘긴다.
            originPos = Star_Donut.transform.position;      // 스타 도넛 위치를 넘긴다.
            originRot = Star_Donut.transform.rotation;
        }

        coroutine01 = StartFry();     // 코루틴 초기화
        coroutine02 = FirstPress();   // 코루틴 초기화

        StartCoroutine(updateCoroutine());    // 최적화?
    }

    void OnDisable()      // 끝날 때..
    {
        mainButton.interactable = true;    // 완전 메인 버튼 활성화
        anim.Play("New State");  // 애니메이션 초기화

        if (mini02_Player.isHoleOrStar.Equals(false))      // 구멍 이라면..
        {
            One_Donut.transform.position = originPos;      // 구멍 도넛 위치 초기화
            One_Donut.transform.rotation = originRot;

            One_Donut_01.material = One_Donut_Origin_Mat;   // 렌더링 메테리얼 초기화
            One_Donut_02.material = One_Donut_Origin_Mat;

            One_Donut.SetActive(false);                     // 구멍 도넛 비활성화
        }
        else      // 스타라면...
        {
            Star_Donut.transform.position = originPos;      // 스타 도넛 위치 초기화
            Star_Donut.transform.rotation = originRot;

            Star_Donut_01.material = Star_Donut_Origin_Mat;   // 렌더링 메테리얼 초기화
            Star_Donut_02.material = Star_Donut_Origin_Mat;

            Star_Donut.SetActive(false);                    // 스타 도넛 비활성화
        }

        right_Image.sprite = sprite_Array[0];
        isCorrect01 = false;         // 초기화
        isCorrect02 = false;
        isFront = true;
        isInput = false;
        isSuccess = false;                      // 앞, 뒤 성공했을 시 변수 초기화

        StopCoroutine(coroutine01);
        StopCoroutine(coroutine02);
    }

    IEnumerator updateCoroutine()           // 최적화?
    {
        while (true)
        {
            if (isFryerInput.Equals(true))         // 반죽을 처음 튀김기를 넣었을 시
            {
                isFryerInput = false;        // 한번만 할꺼라 바로 끔
                isInput = true;
                StartCoroutine(coroutine01);                // 앞면에서 튀기기 시작!!
            }

            if (isCorrect01.Equals(true) && isCorrect02.Equals(true) && isSuccess.Equals(false))          //  앞면, 뒷면 둘 다 성공했다면...
            {
                isSuccess = true;                    // 앞, 뒤 성공시, 변수 최적화
                right_Image.sprite = sprite_Array[1];

                AudioMng.ins.LoopEffect(false);
                AudioMng.ins.PlayEffect("SpeedUp");      // 튀김 끝

                mini02_Player.isOvenOrFryer = true;          // true면 튀김, false면 오븐에서 했다는 뜻
                mini02_Player.playerCompleteInt = 3;     // 토핑 패널로 가라고 알림

                anim.Play(anim03);  // 날아가는 애니메이션
            }

            yield return delay01;
        }
    }


    IEnumerator StartFry()    // 앞면에서 실행, 01
    {
        yield return delay02;      // 코루틴이 실행되고 5초 후 안 익은 도넛 반죽이, 익은 반죽으로 변화


        if (mini02_Player.isHoleOrStar.Equals(false))      // 구멍이라면...
        {
            One_Donut_01.material = One_Donut_Fry_Mat_01;         // 구멍 도넛 윗면에, 프라이 메테리얼을 넣는다.
        }
        else         // 스타라면...
        {
            Star_Donut_01.material = Star_Donut_Fry_Mat_01;       // 스타 도넛 윗면에, 프라이 메테리얼을 넣는다.
        }

        isCorrect01 = true;                                // 앞면 성공
    }

    IEnumerator FirstPress()   // 뒷면에서 실행, 02
    {
        yield return delay02;        // 코루틴이 실행되고 5초 후 안 익은 도넛 반죽이, 익은 반죽으로 변화


        if (mini02_Player.isHoleOrStar.Equals(false))   // 구멍이라면...
        {
            One_Donut_02.material = One_Donut_Fry_Mat_02;         // 구멍 도넛 뒷면에, 프라이 메테리얼을 넣는다.
        }
        else           // 스타라면...
        {
            Star_Donut_02.material = Star_Donut_Fry_Mat_02;       // 스타 도넛 뒷면에, 프라이 메테리얼을 넣는다.
        }

        isCorrect02 = true;                                     // 뒷면 성공
    }


    IEnumerator Press_Wait()       // 도넛 회전을 끝낼때 버튼 누를 수 있도록
    {
        yield return delay03;               // 

        mainButton.interactable = true;     // 회전이 끝났을 때, 버튼을 누를 수 있도록 함
    }

    public void EndButton()   // 끝내는 버튼
    {
        fryerPanel.SetActive(false);      
        mini02_Player.Origin_Panel();  // 패널을 비활성화 하는 함수
    }

    public void Press_MainButton()   // 튀김 패널 안에 있는 메인 버튼을 누를 시..
    {
        if (isSuccess.Equals(true))             // 앞, 뒤 모두 성공시...
        {
            EndButton();      // 끝내는 함수

            return;
        }

        if (isInput.Equals(false))           // 드래그 도넛을 아직 이미지에 넣지 않았다면..
        {
            return;
        }

        mainButton.interactable = false;      // 도넛이 회전할 때 동안 버튼을 못 누르게 하기.
        StartCoroutine(Press_Wait());

        if (isFront.Equals(true) && (isCorrect02.Equals(false) || isCorrect01.Equals(false)))
        // 현재 앞면인 상태에서, (뒷면이 아직 성공을 못했거나, 앞면이 아직 성공을 못했거나) -> 게임이 안 끝난 상태라 회전이 가능하도록....
        {
            anim.Play(anim01);  // 튓면으로 가는 애니메이션

            isFront = false;               // 이제 뒷면으로!

            if (isCorrect01.Equals(false))         // 앞면이 아직 성공하지 않는 경우...(앞면에서 실행된 코루틴을 초기화 해야함!!!)
            {
                StopCoroutine(coroutine01);      // 앞면에서 튀기기 멈추기!!
                coroutine01 = StartFry();        // 코루틴 초기화
            }
            StartCoroutine(coroutine02);         // 뒷면에서 튀기기 시작!!
        }
        else if (isFront.Equals(false) && (isCorrect02.Equals(false) || isCorrect01.Equals(false)))
        // 현재 뒷면인 상태에서, (뒷면이 아직 성공을 못했거나, 앞면이 아직 성공을 못했거나) -> 게임이 안 끝난 상태라 회전이 가능하도록....
        {
            anim.Play(anim02);    // 앞면으로 가는 애니메이션

            isFront = true;           // 이제 앞면으로!

            if (isCorrect02.Equals(false))       // 뒷면이 아직 성공하지 않는 경우...(뒷면에서 실행된 코루틴을 초기화 해야함!!!)
            {
                StopCoroutine(coroutine02);      // 뒷면에서 튀기기 시작!!
                coroutine02 = FirstPress();      // 코루틴 초기화
            }
            StartCoroutine(coroutine01);        // 앞면에서 튀기기 시작!!
        }
    }
}
