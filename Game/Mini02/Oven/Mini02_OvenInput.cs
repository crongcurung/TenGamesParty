using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mini02_OvenInput : MonoBehaviour
{
    [SerializeField] Material[] Mat_Array;

    [SerializeField] Mini02_Player mini02_Player;

    public bool isOvenInput = false;

    [SerializeField] GameObject ovenPanel;            // ���� �г�

    [SerializeField] Image right_Image;
    [SerializeField] Sprite[] sprite_Array;      // 0 : ���� ��, ���� �ƿ�, ��

    IEnumerator coroutine01;               // �ڷ�ƾ �ʱ�ȭ

    bool isInput = false;              // ���쿡 ������ �־����� ���� ����
    bool isSuccess = false;             // ������ ���쿡 �ְ� �ڷ�ƾ�� ��������(����) ���� ����

    bool failTry = false;              // �Դٰ��� �ҷ��� ������ ��
    
    bool endButton = false;            // �����ٰ� �˸��� ����

    [SerializeField] GameObject dragDough;          // �巡�� ����(�̹���) 

    [SerializeField] Transform Oven_Door;        // ������ �� ��ġ
    [SerializeField] Transform Tray;             // ���� ��� ��ġ
    [SerializeField] Transform Tray_Pos_One;     // ó�� ��� ��ġ
    [SerializeField] Transform Tray_Pos_Two;     // �ι�° ��� ��ġ

    [SerializeField] GameObject One_Donut;          // ���ƴٴϴ� ���� ����
    [SerializeField] GameObject Star_Donut;         // ���ƴٴϴ� ��Ÿ ����

    [SerializeField] Renderer One_Donut_Render;
    [SerializeField] Renderer Star_Donut_Render;

    [SerializeField] Animator One_Donut_Anim;
    [SerializeField] Animator Star_Donut_Anim;

    [SerializeField] MeshRenderer Oven_Renderer;        // ���� ���׸����� �ٲٱ� ���� ������
    Material Oven_Mat_Yellow;          // ����� ���� ���׸���
    Material Oven_Mat_Red;             // ������ ���� ���׸���
    Material Oven_Mat_Green;           // �ʷϺ� ���� ���׸���

    Material Donut_One_Toast_Mat;      // ����, ���� ���� ���׸���
    Material Donut_Star_Toast_Mat;     // ��Ÿ, ���� ���� ���׸���

    Material origin_Mat;         // ���� ���׸����� ���� ��
    Renderer origin_Render;      // ���� ������ ���� ��

    Animator anim;                // ���ƴٴϴ� ������ �ִϸ��̼��� �޴� ����
    Vector3 origin_Donut_Pos;     // ���� ���� ��ġ
    Quaternion origin_Donut_Rot;  // ���� ���� ȸ��

    [SerializeField] GameObject LightInOven;     // ���� �ȿ� �ִ� �Һ�

    bool isInDonut = false;        // ���쿡 ������ �־����� ���� ����
    float x = 0.0f;

    WaitForSeconds delay01;
    WaitForSeconds delay02;
    WaitForSeconds delay03;

    int endId;             // �ִϸ��̼� ����ȭ

    Quaternion doorRot;      // ���� �� ȸ��


    void Awake()
	{
        endId = Animator.StringToHash("isEnd");
        doorRot = Quaternion.Euler(Vector3.zero);      // ���� �� ȸ��

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
        if (isInDonut.Equals(true))     // ���쿡 ������ �־��ٸ�..
        {
            if (x <= 90.0f)
            {
                x += Time.deltaTime * 500.0f;
                Oven_Door.rotation = Quaternion.Euler(new Vector3(x, 0, 0));      // ���� ��� �������� �Ѵ�.
            }
            else
            {
                isInDonut = false;   // �ʱ�ȭ

                if (LightInOven.activeSelf.Equals(false))     // ����Ʈ�� Ȱ��ȭ���� �ʾҴٸ�..
                {
                    LightInOven.SetActive(true);         // ����Ʈ Ȱ��ȭ
                }
            }
        }
    }

    void OnEnable()        // ������...
    {
        dragDough.SetActive(true);           // �巡�� �ٽ� Ű��
        coroutine01 = WaitOvenInput();       // �ڷ�ƾ �ʱ�ȭ

        Oven_Renderer.material = Oven_Mat_Yellow;                    // ���� ���׸����� �ٽ� ����ҷ� �ٲ۴�.(�ʱ�ȭ)

        if (mini02_Player.isHoleOrStar.Equals(false))       // ���� ����
        {
            anim = One_Donut_Anim;               // ���� ���� �ִϸ��̼� �ѱ�
            origin_Render = One_Donut_Render;      // ���� ���� �������� �ѱ�
            origin_Mat = origin_Render.material;                     // ���� ����(����) ���׸����� �ѱ�

            origin_Donut_Pos = One_Donut.transform.localPosition;    // ���� ���� ���� ��ġ�� �ѱ�
            origin_Donut_Rot = One_Donut.transform.localRotation;
        }
        else                            // ��Ÿ ����
        {

            anim = Star_Donut_Anim;               // ��Ÿ ���� �ִϸ��̼� �ѱ�
            origin_Render = Star_Donut_Render;      // ��Ÿ ���� �������� �ѱ�
            origin_Mat = origin_Render.material;                      // ��Ÿ ����(����) ���׸����� �ѱ�

            origin_Donut_Pos = Star_Donut.transform.localPosition;    // ��Ÿ ���� ���� ��ġ�� �ѱ�
            origin_Donut_Rot = Star_Donut.transform.localRotation;
        }
        StartCoroutine(updateCoroutine());
    }

    void OnDisable()      // ������..
    {
        isInput = false;       // �ʱ�ȭ
        failTry = false;
        isSuccess = false;
        endButton = false;                     // �����ٴ� ���� �ʱ�ȭ
        StopCoroutine(coroutine01);

        if (mini02_Player.isHoleOrStar.Equals(false))           // ���� ����
        {
            origin_Render.material = origin_Mat;       // ���� ���׸����� �ʱ�ȭ�Ѵ�.

            One_Donut.transform.localPosition = origin_Donut_Pos;       // ���� ������ �ʱ�ȭ �Ѵ�.
            One_Donut.transform.localRotation = origin_Donut_Rot;
        }
        else                                 // ��Ÿ ����
        {
            origin_Render.material = origin_Mat;       // ���� ���׸����� �ʱ�ȭ�Ѵ�.

            Star_Donut.transform.localPosition = origin_Donut_Pos;      // ��Ÿ ������ �ʱ�ȭ �Ѵ�.
            Star_Donut.transform.localRotation = origin_Donut_Rot;
        }

        right_Image.sprite = sprite_Array[0];

        isInDonut = false;     // �ʱ�ȭ(����)
        Oven_Door.rotation = doorRot;     // ȸ�� �ʱ�ȭ
        x = 0.0f;
        Tray.position = Tray_Pos_One.position;    // ��� ��ġ�� �ʱ�ȭ
        One_Donut.SetActive(false);          // ���ƴٴϴ� ���� ��Ȱ��ȭ
        Star_Donut.SetActive(false);

        LightInOven.SetActive(false);        // ����Ʈ�� ����.
        
        anim.SetBool(endId, false);        // �ִϸ��̼� �ʱ�ȭ
    }

    IEnumerator updateCoroutine()                         // ����ȭ �ڷ�ƾ
    {
        while (true)
        {
            if (isOvenInput.Equals(true))      // ���쿡 ������ �־�����..?
            {
                isOvenInput = false;      // �ѹ��� �ϱ� ������ �ٷ� ��������.
                
                isInDonut = true;                                 // ���쿡 ��Ҵٰ� �˷���(����)
                Tray.position = Tray_Pos_Two.position;            // ��� ��ġ�� ���� ������

                isInput = true;                      // ���쿡 ������ �־��ٰ� �˷���
                StartCoroutine(coroutine01);         // �ٷ� 5�ʸ� ����� �ڷ�ƾ ����!!
            }

            yield return delay01;
        }
    }

    IEnumerator WaitOvenInput()                 // ������ ���쿡 �ְ� �� ���� �ڷ�ƾ 
    {
        right_Image.sprite = sprite_Array[1];

        yield return delay02;

        isSuccess = true;        // �ڷ�ƾ ���� ��, 5�ʰ� ������ �������� ���� ��.
        Oven_Renderer.material = Oven_Mat_Green;                // ���� ���׸����� �ʷϺ� ���׸���� �ٲ۴�.
        
        if (mini02_Player.isHoleOrStar.Equals(false))         // ���� ����
        {
            origin_Render.material = Donut_One_Toast_Mat;       // ����, ���� ���׸����� �ѱ�
        }
        else                              // ��Ÿ ����
        {
            origin_Render.material = Donut_Star_Toast_Mat;      // ��Ÿ, ���� ���׸����� �ѱ�
        }
    }


    public void EndButton()
    {
        ovenPanel.SetActive(false);       // ���� �г� ��Ȱ��ȭ
        mini02_Player.Origin_Panel();
    }

    public void Press_Button()            // ���� �гο� �ִ� ���� ��ư�� ���
    {
        if (endButton.Equals(true))                 // �������� ��������..
        {
            EndButton();
            return;
        }

        if (isInput.Equals(true) && failTry.Equals(true))   // ��ư�� ������ ��, ������ ���� �ȿ� �ְ�. failTry�� true�� ���
        {
            AudioMng.ins.LoopEffect(true);
            AudioMng.ins.PlayEffect("Oven");

            

            isInDonut = true;     // ���쿡 ��Ҵٰ� �˷���(����)
            Tray.position = Tray_Pos_Two.position;       // ��� ��ġ�� ���� ������
            Oven_Renderer.material = Oven_Mat_Yellow;    // ���� ���׸����� ����ҷ� �ٲ۴�.

            failTry = false;

            StartCoroutine(coroutine01);         // �ٽ� �ڷ�ƾ ����!
            return;
        }


        if (isInput.Equals(true) && failTry.Equals(false))   // ��ư�� ������ ��, ������ ���� �ȿ� �ְ�. failTry�� false�� ���
        {
            AudioMng.ins.StopEffect();

            if (isSuccess.Equals(false))          // ���� �������� �ʴ� ���...
            {
                StopCoroutine(coroutine01);        // �ڷ�ƾ �ʱ�ȭ
                coroutine01 = WaitOvenInput();     // �ڷ�ƾ �ʱ�ȭ

                Oven_Door.rotation = doorRot;    // ȸ�� 90��
                
                x = 0.0f;
                Tray.position = Tray_Pos_One.position;                   // ��� ��ġ �ʱ�ȭ
                Oven_Renderer.material = Oven_Mat_Red;                   // ���� ���׸����� �����ҷ� �ٲ۴�.

                right_Image.sprite = sprite_Array[0];

                failTry = true;                // �������� ���ߴٰ� �˷���
                LightInOven.SetActive(false);    // ����Ʈ�� ��Ȱ��ȭ

                return;
            }

            AudioMng.ins.LoopEffect(false);
            AudioMng.ins.PlayEffect("SpeedUp");       // �̵� �ִϸ��̼� ����

            right_Image.sprite = sprite_Array[2];

            mini02_Player.playerCompleteInt = 3;     // ���� �гη� ����� �˸�

            mini02_Player.isOvenOrFryer = false;          // true�� Ƣ��, false�� ���쿡�� �ߴٴ� ��
            //
            Oven_Door.rotation = doorRot;    // ȸ�� 90��

            LightInOven.SetActive(false);    // ����Ʈ�� ��Ȱ��ȭ
            Tray.position = Tray_Pos_One.position;          // ��� ��ġ �ʱ�ȭ
            endButton = true;                         // �����ٰ� �˷���
            StartCoroutine(WaitComplete());
        }
    }

    IEnumerator WaitComplete()
    {
        yield return delay03;

        anim.SetBool(endId, true);
    }
}
