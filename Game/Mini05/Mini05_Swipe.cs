using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mini05_Swipe : MonoBehaviour
{
    [SerializeField] GameObject weapon01;           // 캐논에 있는 토네이도 포신
    [SerializeField] GameObject weapon02;           // 캐논에 있는 뚫어뻥 포신
    [SerializeField] GameObject weapon03;           // 캐논에 있는 도넛 폭탄 포신
    
    [SerializeField] GameObject miniGame06_CrossHair;         // 뚫어뻥 크로스 헤어
    [SerializeField] GameObject miniGame06_Cross_Donut;       // 도넛 폭탄 크로스 헤어
    [SerializeField] GameObject miniGame06_Cross_Tonedo;      // 토네이도 크로스 헤어


    public int currentWeaponInt = 1;          // 현재 발사할 수 있는 무기는?  0번 : 토네이도, 1번 : 뚫어뻥, 2번 : 도넛 폭탄

    [SerializeField] Scrollbar scrollBar;          // 현재 스크롤바의 위치를 바탕으로 현재 페이지 검사
    public float swipeTime = 0.2f;       // 페이지가 스와이프 되는 시간
    public float swipeDistace = 50.0f;   // 페이지가 스와이프 되기 위해 움직여야 하는 최소 거리

    float[] scrollPageValues;         // 각 페이지의 위치 값 [0.0 - 1.0]
    float valueDistance = 0;          // 각 페이지 사이의 거리
    int currentPage = 0;              // 현재 페이지
    float startTouchX;                // 터치 시작 위치
    float endTouchX;                  // 터치 종료 위치
    bool isSwipeMode = false;         // 현재 스와이츠가 되고 있는지 체크


    public bool isTouch = false;            // 캐릭터 중앙에 플레이어가 터치를 했는지 묻는 변수

    bool isLeft;             // 왼쪽으로 가냐? true, 오른쪽으로 가냐? false
    bool isBack = false;

    void Awake()
    {
        scrollPageValues = new float[transform.childCount];    // 스크롤 되는 페이지의 각 value 값을 저장하는 배열 메모리 할당
        valueDistance = 1.0f / (scrollPageValues.Length - 1.0f);   // 스크롤 되는 페이지 사이의 거리

        for (int i = 0; i < scrollPageValues.Length; ++i)   // 스크롤 되는 페이지의 각 value 위치 설정 [0 <= value <= 1]
        {
            scrollPageValues[i] = valueDistance * i;
        }
    }


	void Start()
	{

        SetScrollBarValue(1);      // 최초 시작할 때 0번 페이지를 볼 수 있도록 설정
    }

    void Update()
    {
        UpdateInput();
    }


    ///////////////  일반 함수


    void SetScrollBarValue(int index)
    {
        currentPage = index;
        scrollBar.value = scrollPageValues[index];
    }


    void UpdateInput()
    {
        if (isSwipeMode.Equals(true) || isTouch.Equals(false))
        {
            return;                // 현재 스와이프를 진행중이라면 터치 불가
        }

#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))        // 마우스 왼쪽 버튼을 눌렀을 때 1회
        {
            startTouchX = Input.mousePosition.x;   // 터치 시작 지점(스와이프 방향 구분)
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchX = Input.mousePosition.x;      // 터치 종료 지점(스와이프 방향 구분)

            UpdateSwipe();
        }
#endif


#if UNITY_ANDROID

        if (Input.touchCount.Equals(1))
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase.Equals(TouchPhase.Began))
            {
                startTouchX = touch.position.x;    // 터치 시작 지점 (스와이프 방향 구분)
            }
            else if (touch.phase.Equals(TouchPhase.Ended))
            {
                endTouchX = touch.position.x;    // 터치 종료 지점(스와이프 방향 구분)

                UpdateSwipe();
            }
        }
#endif
    }



    void UpdateSwipe()
    {
        isLeft = startTouchX < endTouchX ? true : false;    // 스와이프 방향

        if (Mathf.Abs(startTouchX - endTouchX) < swipeDistace)    // 너무 작은 거리를 움직였을 떄는 스와이프 못하게 만들기..
        {
            isBack = true;       // 되돌아가는 중이라고 알림
            StartCoroutine(OnSwipeOneStep(currentPage));     // 원래 페이지로 스와이프해서 돌아간다.

            isTouch = false;      // 터치를 안했다고 알림
            return;
        }


        if (isLeft.Equals(true))      // 이동 방향이 왼쪽일 때...
        {
            currentPage--;
        }
        else                     // 이동 방향이 오른쪽 일때...
        {
            currentPage++;
        }

        StartCoroutine(OnSwipeOneStep(currentPage));     // 스와이프해서 넘기기
    }


    ///////////////////  코루틴 관련...


    IEnumerator OnSwipeOneStep(int index)   // 페이지를 한 장 옆으로 넘기는 스와이프 효과 재생
    {
        float start = scrollBar.value;
        float current = 0;
        float percent = 0;

        isSwipeMode = true;

        while (percent < 1)     // 완료되기 전까지..
        {
            current += Time.deltaTime;
            percent = current / swipeTime;

            scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);    // 이동

            yield return null;
        }

        if (isBack.Equals(false))     // 조금 밖에 안해서 되돌아가는 중이 아니라면...
        {
            if (isLeft.Equals(false))             // 오른쪽에서 왼쪽으로... 
            {
                scrollBar.value = scrollPageValues[index - 1];
                transform.GetChild(0).SetSiblingIndex(2);         // 자식 순서 바꾸기
                currentPage--;        // 현재 페이지

                if (weapon01 != null)             // 스크립트 하나에만 집어넣을 거라서 이렇게 함(최적화)
                {
                    if (currentWeaponInt.Equals(2))
                    {
                        currentWeaponInt = 0;
                    }
                    else
                    {
                        currentWeaponInt++;
                    }


                    if (currentWeaponInt.Equals(0))
                    {
                        weapon03.SetActive(false);
                        weapon01.SetActive(true);

                        miniGame06_Cross_Donut.SetActive(false);
                        miniGame06_Cross_Tonedo.SetActive(true);
                    }
                    else if (currentWeaponInt.Equals(1))
                    {
                        weapon01.SetActive(false);
                        weapon02.SetActive(true);

                        miniGame06_Cross_Tonedo.SetActive(false);
                        miniGame06_CrossHair.SetActive(true);
                    }
                    else     // 2일 경우..
                    {
                        weapon02.SetActive(false);
                        weapon03.SetActive(true);

                        miniGame06_CrossHair.SetActive(false);
                        miniGame06_Cross_Donut.SetActive(true);
                    }
                }
            }
            else                             // 왼쪽에서 오른쪽으로...
            {
                scrollBar.value = scrollPageValues[index + 1];
                transform.GetChild(2).SetSiblingIndex(0);         // 자식 순서 바꾸기
                currentPage++;

                if (weapon01 != null)             // 스크립트 하나에만 집어넣을 거라서 이렇게 함(최적화)
                {
                    if (currentWeaponInt.Equals(0))    // 현재(넘어가기 이전) 무기가 토네이도인 경우
                    {
                        currentWeaponInt = 2;
                    }
                    else                          // 현재(넘어가기 이전) 무기가 토네이도인 경우
                    {
                        currentWeaponInt--;
                    }


                    if (currentWeaponInt.Equals(0))    // 현재(넘어가기 이후) 무기가 토네이도인 경우
                    {
                        weapon02.SetActive(false);
                        weapon01.SetActive(true);

                        miniGame06_CrossHair.SetActive(false);
                        miniGame06_Cross_Tonedo.SetActive(true);
                    }
                    else if (currentWeaponInt.Equals(1))    // 현재(넘어가기 이후) 무기가 뚫어뻥인 경우
                    {
                        weapon03.SetActive(false);
                        weapon02.SetActive(true);

                        miniGame06_Cross_Donut.SetActive(false);
                        miniGame06_CrossHair.SetActive(true);
                    }
                    else                               // 현재(넘어가기 이후) 무기가 도넛 폭탄인 경우
                    {
                        weapon01.SetActive(false);
                        weapon03.SetActive(true);

                        miniGame06_Cross_Tonedo.SetActive(false);
                        miniGame06_Cross_Donut.SetActive(true);
                    }
                }
            }
        }
        else
        {
            isBack = false;       // 되돌아가는 중이라고 알림
        }

        isSwipeMode = false;      // 터치가 끝났다고 알림
        isTouch = false;          // 터치가 끝났다고 알림
    }
}
