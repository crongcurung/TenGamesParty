using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mini02_FryInput : MonoBehaviour
{
    [SerializeField] Material[] Mat_Array;

    [SerializeField] Mini02_Player mini02_Player;      // �÷��̾� ��ũ��Ʈ

    public bool isFryerInput = false;         // �巡�� �̹����� Ƣ��� �ȿ� �־����� ���� ����

    public GameObject fryerPanel;          // Ƣ�� �г�

    bool isCorrect01 = false;     // �ո��� �����ߴ��� ���� ����
    bool isCorrect02 = false;     // �޸��� �����ߴ��� ���� ����

    bool isFront = true;              // ������ �ո����� ���� ����
    bool isInput = false;             // �巡�� ������ Ƣ��⿡ �־����� ���� ����

    [SerializeField] Image right_Image;
    [SerializeField] Sprite[] sprite_Array;          // 0 : Ƣ��, 1 : ��

    IEnumerator coroutine01;                 // �ڷ�ƾ �ʱ�ȭ
    IEnumerator coroutine02;                 // �ڷ�ƾ �ʱ�ȭ

    [SerializeField] Button mainButton;        // Ƣ�� �гο� �ִ� ��ư

    bool isSuccess = false;               // ��, �� �������� �ÿ� ���� ����ȭ ����

    [SerializeField] GameObject One_Donut;     // �������� �ȿ� ���� ����
    [SerializeField] Renderer One_Donut_01;    // �������� �ȿ� ���� ������ ����
    [SerializeField] Renderer One_Donut_02;    // �������� �ȿ� ���� ������ �޸�
    [SerializeField] Animator One_Donut_Anim;

    [SerializeField] GameObject Star_Donut;    // �������� �ȿ� ��Ÿ ����
    [SerializeField] Renderer Star_Donut_01;   // �������� �ȿ� ��Ÿ ������ ����
    [SerializeField] Renderer Star_Donut_02;   // �������� �ȿ� ��Ÿ ������ �޸�
    [SerializeField] Animator Star_Donut_Anim;

    Material One_Donut_Fry_Mat_01;     // ���� ���� ���� ������ ���׸���
    Material One_Donut_Fry_Mat_02;     // ���� ���� �޸� ������ ���׸���
    Material Star_Donut_Fry_Mat_01;    // ��Ÿ ���� ���� ������ ���׸���
    Material Star_Donut_Fry_Mat_02;    // ��Ÿ ���� �޸� ������ ���׸���

    Material One_Donut_Origin_Mat;       // ���� ���� �⺻ ���׸���
    Material Star_Donut_Origin_Mat;      // ��Ÿ ���� �⺻ ���׸���

    Animator anim;          // �ش� ������ �ִϸ��̼��� �޴� ����
    Vector3 originPos;      // ���� ���� ��ġ
    Quaternion originRot;   // ���� ���� ȸ��

    WaitForSeconds delay01;     // ������Ʈ �ڷ�ƾ
    WaitForSeconds delay02;     // ���� �������� �ڷ�ƾ
    WaitForSeconds delay03;     // ȸ���� ��ư�� ���� �ڷ�ƾ

    string anim01;    // �ִϸ����� ����ȭ
    string anim02;
    string anim03;

    void Awake()
	{
        anim01 = "Mini02_Fry_01";       // �E������ ���� �ִϸ��̼�
        anim02 = "Mini02_Fry_02";          // �ո����� ���� �ִϸ��̼�
        anim03 = "Mini02_Fry_03";     // ���ư��� �ִϸ��̼�

        One_Donut_Fry_Mat_01 = Mat_Array[0];
        One_Donut_Fry_Mat_02 = Mat_Array[1];
        Star_Donut_Fry_Mat_01 = Mat_Array[2];
        Star_Donut_Fry_Mat_02 = Mat_Array[3];
        One_Donut_Origin_Mat = Mat_Array[4];
        Star_Donut_Origin_Mat = Mat_Array[5];

        delay01 = new WaitForSeconds(0.1f);
        delay02 = new WaitForSeconds(2.5f);      // �ڷ�ƾ�� ����ǰ� 5�� �� �� ���� ���� ������, ���� �������� ��ȭ
        delay03 = new WaitForSeconds(0.5f);
    }

	void OnEnable()       // ���� ��...
    {
        if (mini02_Player.isHoleOrStar.Equals(false))
        {
            anim = One_Donut_Anim;      // ���� ������ �ִϸ��̼��� �ѱ��.
            originPos = One_Donut.transform.position;       // ���� ���� ��ġ�� �ѱ��.
            originRot = One_Donut.transform.rotation;
        }
        else
        {
            anim = Star_Donut_Anim;     // ��Ÿ ������ �ִϸ��̼��� �ѱ��.
            originPos = Star_Donut.transform.position;      // ��Ÿ ���� ��ġ�� �ѱ��.
            originRot = Star_Donut.transform.rotation;
        }

        coroutine01 = StartFry();     // �ڷ�ƾ �ʱ�ȭ
        coroutine02 = FirstPress();   // �ڷ�ƾ �ʱ�ȭ

        StartCoroutine(updateCoroutine());    // ����ȭ?
    }

    void OnDisable()      // ���� ��..
    {
        mainButton.interactable = true;    // ���� ���� ��ư Ȱ��ȭ
        anim.Play("New State");  // �ִϸ��̼� �ʱ�ȭ

        if (mini02_Player.isHoleOrStar.Equals(false))      // ���� �̶��..
        {
            One_Donut.transform.position = originPos;      // ���� ���� ��ġ �ʱ�ȭ
            One_Donut.transform.rotation = originRot;

            One_Donut_01.material = One_Donut_Origin_Mat;   // ������ ���׸��� �ʱ�ȭ
            One_Donut_02.material = One_Donut_Origin_Mat;

            One_Donut.SetActive(false);                     // ���� ���� ��Ȱ��ȭ
        }
        else      // ��Ÿ���...
        {
            Star_Donut.transform.position = originPos;      // ��Ÿ ���� ��ġ �ʱ�ȭ
            Star_Donut.transform.rotation = originRot;

            Star_Donut_01.material = Star_Donut_Origin_Mat;   // ������ ���׸��� �ʱ�ȭ
            Star_Donut_02.material = Star_Donut_Origin_Mat;

            Star_Donut.SetActive(false);                    // ��Ÿ ���� ��Ȱ��ȭ
        }

        right_Image.sprite = sprite_Array[0];
        isCorrect01 = false;         // �ʱ�ȭ
        isCorrect02 = false;
        isFront = true;
        isInput = false;
        isSuccess = false;                      // ��, �� �������� �� ���� �ʱ�ȭ

        StopCoroutine(coroutine01);
        StopCoroutine(coroutine02);
    }

    IEnumerator updateCoroutine()           // ����ȭ?
    {
        while (true)
        {
            if (isFryerInput.Equals(true))         // ������ ó�� Ƣ��⸦ �־��� ��
            {
                isFryerInput = false;        // �ѹ��� �Ҳ��� �ٷ� ��
                isInput = true;
                StartCoroutine(coroutine01);                // �ո鿡�� Ƣ��� ����!!
            }

            if (isCorrect01.Equals(true) && isCorrect02.Equals(true) && isSuccess.Equals(false))          //  �ո�, �޸� �� �� �����ߴٸ�...
            {
                isSuccess = true;                    // ��, �� ������, ���� ����ȭ
                right_Image.sprite = sprite_Array[1];

                AudioMng.ins.LoopEffect(false);
                AudioMng.ins.PlayEffect("SpeedUp");      // Ƣ�� ��

                mini02_Player.isOvenOrFryer = true;          // true�� Ƣ��, false�� ���쿡�� �ߴٴ� ��
                mini02_Player.playerCompleteInt = 3;     // ���� �гη� ����� �˸�

                anim.Play(anim03);  // ���ư��� �ִϸ��̼�
            }

            yield return delay01;
        }
    }


    IEnumerator StartFry()    // �ո鿡�� ����, 01
    {
        yield return delay02;      // �ڷ�ƾ�� ����ǰ� 5�� �� �� ���� ���� ������, ���� �������� ��ȭ


        if (mini02_Player.isHoleOrStar.Equals(false))      // �����̶��...
        {
            One_Donut_01.material = One_Donut_Fry_Mat_01;         // ���� ���� ���鿡, ������ ���׸����� �ִ´�.
        }
        else         // ��Ÿ���...
        {
            Star_Donut_01.material = Star_Donut_Fry_Mat_01;       // ��Ÿ ���� ���鿡, ������ ���׸����� �ִ´�.
        }

        isCorrect01 = true;                                // �ո� ����
    }

    IEnumerator FirstPress()   // �޸鿡�� ����, 02
    {
        yield return delay02;        // �ڷ�ƾ�� ����ǰ� 5�� �� �� ���� ���� ������, ���� �������� ��ȭ


        if (mini02_Player.isHoleOrStar.Equals(false))   // �����̶��...
        {
            One_Donut_02.material = One_Donut_Fry_Mat_02;         // ���� ���� �޸鿡, ������ ���׸����� �ִ´�.
        }
        else           // ��Ÿ���...
        {
            Star_Donut_02.material = Star_Donut_Fry_Mat_02;       // ��Ÿ ���� �޸鿡, ������ ���׸����� �ִ´�.
        }

        isCorrect02 = true;                                     // �޸� ����
    }


    IEnumerator Press_Wait()       // ���� ȸ���� ������ ��ư ���� �� �ֵ���
    {
        yield return delay03;               // 

        mainButton.interactable = true;     // ȸ���� ������ ��, ��ư�� ���� �� �ֵ��� ��
    }

    public void EndButton()   // ������ ��ư
    {
        fryerPanel.SetActive(false);      
        mini02_Player.Origin_Panel();  // �г��� ��Ȱ��ȭ �ϴ� �Լ�
    }

    public void Press_MainButton()   // Ƣ�� �г� �ȿ� �ִ� ���� ��ư�� ���� ��..
    {
        if (isSuccess.Equals(true))             // ��, �� ��� ������...
        {
            EndButton();      // ������ �Լ�

            return;
        }

        if (isInput.Equals(false))           // �巡�� ������ ���� �̹����� ���� �ʾҴٸ�..
        {
            return;
        }

        mainButton.interactable = false;      // ������ ȸ���� �� ���� ��ư�� �� ������ �ϱ�.
        StartCoroutine(Press_Wait());

        if (isFront.Equals(true) && (isCorrect02.Equals(false) || isCorrect01.Equals(false)))
        // ���� �ո��� ���¿���, (�޸��� ���� ������ ���߰ų�, �ո��� ���� ������ ���߰ų�) -> ������ �� ���� ���¶� ȸ���� �����ϵ���....
        {
            anim.Play(anim01);  // �E������ ���� �ִϸ��̼�

            isFront = false;               // ���� �޸�����!

            if (isCorrect01.Equals(false))         // �ո��� ���� �������� �ʴ� ���...(�ո鿡�� ����� �ڷ�ƾ�� �ʱ�ȭ �ؾ���!!!)
            {
                StopCoroutine(coroutine01);      // �ո鿡�� Ƣ��� ���߱�!!
                coroutine01 = StartFry();        // �ڷ�ƾ �ʱ�ȭ
            }
            StartCoroutine(coroutine02);         // �޸鿡�� Ƣ��� ����!!
        }
        else if (isFront.Equals(false) && (isCorrect02.Equals(false) || isCorrect01.Equals(false)))
        // ���� �޸��� ���¿���, (�޸��� ���� ������ ���߰ų�, �ո��� ���� ������ ���߰ų�) -> ������ �� ���� ���¶� ȸ���� �����ϵ���....
        {
            anim.Play(anim02);    // �ո����� ���� �ִϸ��̼�

            isFront = true;           // ���� �ո�����!

            if (isCorrect02.Equals(false))       // �޸��� ���� �������� �ʴ� ���...(�޸鿡�� ����� �ڷ�ƾ�� �ʱ�ȭ �ؾ���!!!)
            {
                StopCoroutine(coroutine02);      // �޸鿡�� Ƣ��� ����!!
                coroutine02 = FirstPress();      // �ڷ�ƾ �ʱ�ȭ
            }
            StartCoroutine(coroutine01);        // �ո鿡�� Ƣ��� ����!!
        }
    }
}
