using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mini05_Swipe : MonoBehaviour
{
    [SerializeField] GameObject weapon01;           // ĳ�� �ִ� ����̵� ����
    [SerializeField] GameObject weapon02;           // ĳ�� �ִ� �վ ����
    [SerializeField] GameObject weapon03;           // ĳ�� �ִ� ���� ��ź ����
    
    [SerializeField] GameObject miniGame06_CrossHair;         // �վ ũ�ν� ���
    [SerializeField] GameObject miniGame06_Cross_Donut;       // ���� ��ź ũ�ν� ���
    [SerializeField] GameObject miniGame06_Cross_Tonedo;      // ����̵� ũ�ν� ���


    public int currentWeaponInt = 1;          // ���� �߻��� �� �ִ� �����?  0�� : ����̵�, 1�� : �վ, 2�� : ���� ��ź

    [SerializeField] Scrollbar scrollBar;          // ���� ��ũ�ѹ��� ��ġ�� �������� ���� ������ �˻�
    public float swipeTime = 0.2f;       // �������� �������� �Ǵ� �ð�
    public float swipeDistace = 50.0f;   // �������� �������� �Ǳ� ���� �������� �ϴ� �ּ� �Ÿ�

    float[] scrollPageValues;         // �� �������� ��ġ �� [0.0 - 1.0]
    float valueDistance = 0;          // �� ������ ������ �Ÿ�
    int currentPage = 0;              // ���� ������
    float startTouchX;                // ��ġ ���� ��ġ
    float endTouchX;                  // ��ġ ���� ��ġ
    bool isSwipeMode = false;         // ���� ���������� �ǰ� �ִ��� üũ


    public bool isTouch = false;            // ĳ���� �߾ӿ� �÷��̾ ��ġ�� �ߴ��� ���� ����

    bool isLeft;             // �������� ����? true, ���������� ����? false
    bool isBack = false;

    void Awake()
    {
        scrollPageValues = new float[transform.childCount];    // ��ũ�� �Ǵ� �������� �� value ���� �����ϴ� �迭 �޸� �Ҵ�
        valueDistance = 1.0f / (scrollPageValues.Length - 1.0f);   // ��ũ�� �Ǵ� ������ ������ �Ÿ�

        for (int i = 0; i < scrollPageValues.Length; ++i)   // ��ũ�� �Ǵ� �������� �� value ��ġ ���� [0 <= value <= 1]
        {
            scrollPageValues[i] = valueDistance * i;
        }
    }


	void Start()
	{

        SetScrollBarValue(1);      // ���� ������ �� 0�� �������� �� �� �ֵ��� ����
    }

    void Update()
    {
        UpdateInput();
    }


    ///////////////  �Ϲ� �Լ�


    void SetScrollBarValue(int index)
    {
        currentPage = index;
        scrollBar.value = scrollPageValues[index];
    }


    void UpdateInput()
    {
        if (isSwipeMode.Equals(true) || isTouch.Equals(false))
        {
            return;                // ���� ���������� �������̶�� ��ġ �Ұ�
        }

#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))        // ���콺 ���� ��ư�� ������ �� 1ȸ
        {
            startTouchX = Input.mousePosition.x;   // ��ġ ���� ����(�������� ���� ����)
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchX = Input.mousePosition.x;      // ��ġ ���� ����(�������� ���� ����)

            UpdateSwipe();
        }
#endif


#if UNITY_ANDROID

        if (Input.touchCount.Equals(1))
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase.Equals(TouchPhase.Began))
            {
                startTouchX = touch.position.x;    // ��ġ ���� ���� (�������� ���� ����)
            }
            else if (touch.phase.Equals(TouchPhase.Ended))
            {
                endTouchX = touch.position.x;    // ��ġ ���� ����(�������� ���� ����)

                UpdateSwipe();
            }
        }
#endif
    }



    void UpdateSwipe()
    {
        isLeft = startTouchX < endTouchX ? true : false;    // �������� ����

        if (Mathf.Abs(startTouchX - endTouchX) < swipeDistace)    // �ʹ� ���� �Ÿ��� �������� ���� �������� ���ϰ� �����..
        {
            isBack = true;       // �ǵ��ư��� ���̶�� �˸�
            StartCoroutine(OnSwipeOneStep(currentPage));     // ���� �������� ���������ؼ� ���ư���.

            isTouch = false;      // ��ġ�� ���ߴٰ� �˸�
            return;
        }


        if (isLeft.Equals(true))      // �̵� ������ ������ ��...
        {
            currentPage--;
        }
        else                     // �̵� ������ ������ �϶�...
        {
            currentPage++;
        }

        StartCoroutine(OnSwipeOneStep(currentPage));     // ���������ؼ� �ѱ��
    }


    ///////////////////  �ڷ�ƾ ����...


    IEnumerator OnSwipeOneStep(int index)   // �������� �� �� ������ �ѱ�� �������� ȿ�� ���
    {
        float start = scrollBar.value;
        float current = 0;
        float percent = 0;

        isSwipeMode = true;

        while (percent < 1)     // �Ϸ�Ǳ� ������..
        {
            current += Time.deltaTime;
            percent = current / swipeTime;

            scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);    // �̵�

            yield return null;
        }

        if (isBack.Equals(false))     // ���� �ۿ� ���ؼ� �ǵ��ư��� ���� �ƴ϶��...
        {
            if (isLeft.Equals(false))             // �����ʿ��� ��������... 
            {
                scrollBar.value = scrollPageValues[index - 1];
                transform.GetChild(0).SetSiblingIndex(2);         // �ڽ� ���� �ٲٱ�
                currentPage--;        // ���� ������

                if (weapon01 != null)             // ��ũ��Ʈ �ϳ����� ������� �Ŷ� �̷��� ��(����ȭ)
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
                    else     // 2�� ���..
                    {
                        weapon02.SetActive(false);
                        weapon03.SetActive(true);

                        miniGame06_CrossHair.SetActive(false);
                        miniGame06_Cross_Donut.SetActive(true);
                    }
                }
            }
            else                             // ���ʿ��� ����������...
            {
                scrollBar.value = scrollPageValues[index + 1];
                transform.GetChild(2).SetSiblingIndex(0);         // �ڽ� ���� �ٲٱ�
                currentPage++;

                if (weapon01 != null)             // ��ũ��Ʈ �ϳ����� ������� �Ŷ� �̷��� ��(����ȭ)
                {
                    if (currentWeaponInt.Equals(0))    // ����(�Ѿ�� ����) ���Ⱑ ����̵��� ���
                    {
                        currentWeaponInt = 2;
                    }
                    else                          // ����(�Ѿ�� ����) ���Ⱑ ����̵��� ���
                    {
                        currentWeaponInt--;
                    }


                    if (currentWeaponInt.Equals(0))    // ����(�Ѿ�� ����) ���Ⱑ ����̵��� ���
                    {
                        weapon02.SetActive(false);
                        weapon01.SetActive(true);

                        miniGame06_CrossHair.SetActive(false);
                        miniGame06_Cross_Tonedo.SetActive(true);
                    }
                    else if (currentWeaponInt.Equals(1))    // ����(�Ѿ�� ����) ���Ⱑ �վ�� ���
                    {
                        weapon03.SetActive(false);
                        weapon02.SetActive(true);

                        miniGame06_Cross_Donut.SetActive(false);
                        miniGame06_CrossHair.SetActive(true);
                    }
                    else                               // ����(�Ѿ�� ����) ���Ⱑ ���� ��ź�� ���
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
            isBack = false;       // �ǵ��ư��� ���̶�� �˸�
        }

        isSwipeMode = false;      // ��ġ�� �����ٰ� �˸�
        isTouch = false;          // ��ġ�� �����ٰ� �˸�
    }
}
