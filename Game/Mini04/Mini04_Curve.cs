using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Mini04_Curve : MonoBehaviour
{
    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // ������ ������ ��Ͻ�ų �� �ִ� ��ư

	[SerializeField] AudioSource audio_Bowl;

    [SerializeField] TextMeshProUGUI scoreText;   // ���ھ� �ؽ�Ʈ
    [SerializeField] GameObject[] Hp_List;

    [SerializeField] Image donut_Image;
    [SerializeField] Sprite[] sprite_Array;      // 0 : ����, 1 : ����

    int scoreInt = 0;              // ���ھ� ������ �ø��� ����
    int failInt = 0;               // �й� ������ �ø��� ����

    [SerializeField] Transform[] cups;       // �׸� 5���� ���� ����

    [SerializeField] Transform[] Pos_3x3;    // �׸� 3���ϋ� ��ġ ����
    [SerializeField] Transform[] Pos_4x4;    // �׸� 4���ϋ� ��ġ ����
    [SerializeField] Transform[] Pos_5x5;    // �׸� 5���ϋ� ��ġ ����

    [SerializeField] GameObject[] donut_Array;     // �׸� �ȿ� �� ����(�ΰ�)

    float speed;               // �׸� ������ �ӵ�

    [Range(0, 1)]             
    public float Test = 0;     // �׸� ������ ���� ��(����?)

    Animator anim;           // �׸� ��ü�� 90���� ���� �ִϸ��̼��� �ޱ� ����...

    int randInt;             // ���⵵�Ӹ� ���Ë� ������ ��ġ�� �ٲٴ� ����?

    bool isSecondTouch = false;                // �÷��̾ ��ġ�� �ѹ� �߳� ���� ����(�׸� �� ��...)
    bool isCurveEnd = false;                   // �׸����� ��� ��������?(�÷��̾ �׸��� �� �غ� �Ǿ�����..?)
    bool isCorrect = false;                    // �������� ���� ����..

    WaitForEndOfFrame delay_01;          // ���� ������ ������..
    WaitForSeconds delay_02;             // 2�� ������..

    [SerializeField] Animator anim_Player;               // �÷��̾��� �ִϸ����͸� �����´�.
    [SerializeField] Animator anim_Monster;              // ������ �ִϸ����͸� �����´�.

    [SerializeField] GameObject[] donutThrow_Array;      // ������ ���� �迭(�ΰ�)

    [SerializeField] Transform Enemy_HeadPos;            // ���� �Ӹ� ��ġ(������ ���� ��������)

    Vector3 originPos_ThrowDonut;                        // ������ ���� ó����ġ(������ �� ��, �ٽ� �������� �ǵ��ƿ;� �Ѵ�)

    Mini04_Bowl mini04_Bowl;                             // �׸� �ȿ� �پ� �ִ� ��ũ��Ʈ

    [SerializeField] GameObject enermy_Bat;              // ���Ͱ� ��� �ִ� �и�ġ
    bool isShield = false;                               // ���� �÷��̾ �Ӹ��� ���� �����ִ���?

    [SerializeField] GameObject[] head_Helmet;            // 0 : �׸�, 1 : ����
    GameObject temp_Helmet;                               // �Ӹ� ���� ���� ������Ʈ �ӽ÷� �޴� ����

    [SerializeField] Transform HeadThrow_Pos;            // ƨ���� ���� ����� ƨ�� ���� ���� ����

    GameObject touch_Bowl;                               // ù���� ��ġ�Ҷ� �׸��� �޴� ����
    int bowlInt;                                         // �̹� ���������� �� �׸��� ���;��ϴ��� �˾ƺ��� ����

    [SerializeField] GameObject fly_Obj;                 // ���ƴٴϴ� ������Ʈ
    [SerializeField] Collider[] cloud_Col;               // ���� ������Ʈ(5��)

    bool isCloud = false;        // �̹� ������������ ������ ���Գ�?
    bool isFly = false;          // �̹� ������������ ���ƴٴϴ� ������Ʈ�� ���Գ�?

    int firstInt = 0;            // ���� ������ ��ġ�� �޴� ����(��������4 �̻�)
    int secondInt = 0;           // ���� ������ ��ġ�� �޴� ����(��������4 �̻�)
    bool isDonutStrow = true;    // true�� ����, false�� ����
    bool isRotEnd = false;            // true�� 90���� ���� ȸ���� ������, false�� ���� �� ������..(��Ȯ���� �׸��� �����ϋ� true�� ��)

    string tag01;
    string tag02;
    string tag03;
    string tag04;

    int anim_Id01;
    int anim_Id02;
    int anim_Id03;
    int anim_Id04;
    int anim_Id05;

    Camera cameraMain;

    void Start()
    {
        tag01 = "Cushion";
        tag02 = "Note";
        tag03 = "Honey";
        tag04 = "Spring";

        audio_Bowl.volume = AudioMng.ins.GetEffectVolume();

        anim_Id01 = Animator.StringToHash("New State");
        anim_Id02 = Animator.StringToHash("isRot");
        anim_Id03 = Animator.StringToHash("isAttack");
        anim_Id04 = Animator.StringToHash("Enemy_Attack");
        anim_Id05 = Animator.StringToHash("Enemy_Damage");

        failInt = 3;

        cameraMain = Camera.main;

        anim = GetComponent<Animator>();         // �׸����� 90���� ������ �� �ִ� �ִϸ��̼��� ������
        originPos_ThrowDonut = donutThrow_Array[0].transform.position;      // ������ ������ ���� ��ġ�� ������(�ϳ��� �ص� �ȴ�)
        Stage_fuction(0);                                                   // ���������� �����Ѵ�.
        delay_01 = new WaitForEndOfFrame();        // ���� ������
        delay_02 = new WaitForSeconds(2.0f);       // 2�� ��

        AudioMng.ins.Play_BG("Mini04_B");
    }

    

	void Update()
	{
        if (Input.GetMouseButtonDown(0))          // �÷��̾ �����ٸ�?
        {
            RaycastHit hit;      
            Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (isRotEnd.Equals(true) && hit.collider != null)      // 90�� ȸ���� ������ ���� ���� �ݶ��̴��� �ִٸ�..
            {
                if (hit.transform.CompareTag(tag01))          // ���� ��信 ������...
                {
                    hit.transform.gameObject.SetActive(false);    // ������ ������ �����.

                    AudioMng.ins.PlayEffect("Click02");    // ���� �׸� Ŭ��
                    Shield_Active(1);    // ���� ����� �����..
                }
            }

            if (isSecondTouch.Equals(false) && isCurveEnd.Equals(true) && hit.collider != null)  // ���� ó�� ������, ��� �׸��� ������, �ݶ��̴��� �ִٸ�..
            {
                if (hit.transform.CompareTag(tag02))       // �׸��� �����ٸ�..
                {
                    AudioMng.ins.PlayEffect("Click02");    // ���� �׸� Ŭ��
                    mini04_Bowl = hit.transform.GetComponent<Mini04_Bowl>();      // ���� �׸��� ��ũ��Ʈ�� �����´�.

                    if (hit.transform.childCount.Equals(2))     // �ڽ��� �ΰ���� ���� �׸��ȿ� ���� �ִٴ� ��..
                    {
                        if (isDonutStrow.Equals(true))          // ������ ���� �����̶�� ��
                        {
                            if (hit.transform.GetChild(1).CompareTag(tag03))     // ����            honey ����, spring�� ����
                            {
                                isCorrect = true;                           // �����̶�� �˸�
                            }
                            else      // �߸��� ������ �־��ٴ� ��....
                            {
                                StartCoroutine(Attack_Monster());
                            }
                        }
                        else                                    // ������ ���� �����̶�� ��
                        {
                            if (hit.transform.GetChild(1).CompareTag(tag04))     // ����            honey ����, spring�� ����
                            {
                                isCorrect = true;                           // �����̶�� �˸�
                            }
                            else      // �߸��� ������ �־��ٴ� ��....
                            {
                                StartCoroutine(Attack_Monster());      // ���� �и�ġ ���� ����..
                            }
                        }
                        hit.transform.GetChild(1).parent = null;        // ���� �׸��� ������ ����Ʈ����.(�̹� �׸��ȿ� ������ �ִٰ� ����)
                    }
                    else                   // �� �׸��� ��������...
                    {
                        StartCoroutine(Attack_Monster());      // ���� �и�ġ ���� ����..
                    }
                    mini04_Bowl.Buddle_Bowl();         // ������ �׸��� �ε�Ÿ��� �Ѵ�.

                    isSecondTouch = true;              // �ѹ� �����ٰ� �˷���
                    touch_Bowl = hit.transform.gameObject;  // ������ �׸��� �ӽ÷� ��´�.
                }
            }
            else if(isSecondTouch.Equals(true) && hit.collider != null)     // �� ��° �����Ű�, �ݶ��̴��� �ִٸ�...
            {
                if (isCorrect.Equals(true))               // �÷��̾ ������ ����ٸ�...(������ 2������, �����̶�� ���� ������ ����ٴ� ���̴�..)
                {
                    if (hit.transform.gameObject.layer.Equals(4))        // �ٽ� ���� ���� �����̶��..
                    {
                        hit.transform.gameObject.SetActive(false);        // ���� ������ ��Ȱ��ȭ �Ѵ�.
                        StartCoroutine(Attack_Player());      // �÷��̾� ������ ���� ����..
                    }
                }
                else                                      // �÷��̾ Ʋ�ȴٸ�...
                {
                    if (mini04_Bowl.isBuddle_End.Equals(true))    // ù��° ������ �׸��� �ε� �Ÿ��� �ִϸ��̼��� �����ٸ�...(�÷��̾�� ���� ���� �� ����..)
                    {
                        if (hit.transform.CompareTag(tag02))     // �׸��� �����ٸ�.. (ù ��° ���� �����̶� �ٸ����� �ִ�)
                        {
                            if (hit.transform.gameObject.Equals(touch_Bowl))     // �ٽ� ���� ���� ƨ���� �׸��̶� ���ٸ�...
                            {
                                hit.transform.gameObject.SetActive(false);       // ƨ���� �׸�(���� ����)�� ��Ȱ��ȭ

                                AudioMng.ins.PlayEffect("Click02");    // ƨ�� �׸� Ŭ��
                                Shield_Active(0);    // �׸� ����� �����..
                            }
                        }
                    }
                }
            }
        }
	}

    void Shield_Active(int helmetInt)    // ����� ���� ���������� ���� �Լ�, 0 : �׸�, 1 : ����
    {
        if (isShield.Equals(false))                      // ����(���)�� �����ٸ�..
        {
            isShield = true;                             // ����(���)�� �Ǿ��ٰ� �˸���.
            temp_Helmet = head_Helmet[helmetInt];        // �׸� ����� ������ ��´�.
            temp_Helmet.SetActive(true);                 // �׸� ����� Ȱ��ȭ ��Ų��.
        }
    }

    void Fly_isActive(bool isFlyActive)   // ���ƴٴϴ� ������Ʈ�� Ȱ��ȭ����, ��Ȱ��ȭ���� ���� �Լ�. true�� Ű��, false�� ����.
    {
        isFly = isFlyActive;              // Ȱ��ȭ�� Ȱ��ȭ �ƴٰ�, ��Ȱ��ȭ�� ��Ȱ��ȭ �ƴٰ� �˷���
        fly_Obj.SetActive(isFlyActive);   // ���ƴٴϴ� ������Ʈ�� Ȱ��ȭ ��Ȱ��ȭ �Ѵ�.
    }

    void Cloud_isActive(bool isCloudActive)  // ������ Ȱ��ȭ����, ��Ȱ��ȭ���� ���� �Լ�. true�� Ű��, false�� ����.
    {
        isCloud = isCloudActive;          // Ȱ��ȭ�� Ȱ��ȭ �ƴٰ�, ��Ȱ��ȭ�� ��Ȱ��ȭ �ƴٰ� �˷���
        for (int i = 0; i < 5; i++)       // ������ �ټ����� 5�� ��� �ؾ���
        {
            cloud_Col[i].gameObject.SetActive(isCloudActive);   // ���� Ȱ��ȭ, ��Ȱ��ȭ �Ѵ�.
        }
    }

    void Cloud_isTouch(bool isTouch)    // ������ ��ġ�� �� �ִ��� ���� �Լ�
    {
        for (int i = 0; i < 5; i++)     // ������ �� 5��
        {
            cloud_Col[i].enabled = isTouch;     // ���� �ݶ��̴��� Ȱ��ȭ�� Ȱ��ȭ��, ��Ȱ��ȭ�� ��Ȱ��ȭ�� �Ѵ�.
        }
    }

    void Setting_Fuction(int scoreInt)    // �������� ���� �� �ʱ�ȭ(�ƴ� �͵� ����)�ϴ� �Լ�(ù��°�� ������� �ʴ´�..)
    {
        isSecondTouch = false;            // ��ġ�� ����ߴ��� ���� ���� �ʱ�ȭ
        isCurveEnd = false;               // �׸��� �� �������� ���� ���� �ʱ�ȭ
        isCorrect = false;                // �����̶�� �˸� ���� �ʱ�ȭ
        isRotEnd = false;                 // �׸��� 90���� ȸ���Ǿ����� ���� ���� �ʱ�ȭ

        donut_Array[0].transform.parent = null;                              // ���� �θ� �ʱ�ȭ(Ȥ�� ����..)
        donut_Array[0].transform.rotation = Quaternion.Euler(Vector3.zero);
        donut_Array[1].transform.parent = null;
        donut_Array[1].transform.rotation = Quaternion.Euler(Vector3.zero);

        anim_Monster.Play(anim_Id01);      // ���� �ٽ� �Ͼ�� ��...(Ȥ�� ����...)
        anim_Monster.speed = 1.0f;           // �ӵ� ����ġ (Ȥ�� ����...)
        enermy_Bat.SetActive(false);         // �и�ġ ��Ȱ��ȭ (Ȥ�� ����...)
        touch_Bowl.transform.rotation = Quaternion.Euler(Vector3.zero);   // �׸� ȸ�� �ʱ�ȭ...

        int stageLevel = 0;      // �������� ������ ������ ���� ����
        if (scoreInt < 5)        // ���ھ 5 �̸��̶��..
        {
            stageLevel = 0;      // �������� ������ 0�̴�..
        }
        else if (scoreInt < 10)    // ���ھ 10 �̸��̶��..
        {
            stageLevel = 1;      // �������� ������ 1�̴�..
        }
        else if (scoreInt < 15)    // ���ھ 15 �̸��̶��..
        {
            stageLevel = 2;      // �������� ������ 2�̴�..
        }
        else if (scoreInt < 30)    // ���ھ 30 �̸��̶��..
        {
            stageLevel = 3;      // �������� ������ 3�̴�..
        }
        else if (scoreInt < 45)    // ���ھ 45 �̸��̶��..
        {
            stageLevel = 4;      // �������� ������ 4�̴�..
        }
        else                     // ���ھ 45 �̻��̶��..
        {
            stageLevel = 5;      // �������� ������ 5�̴�..
        }


        if (isCloud.Equals(true))       // �̹� ������������ ������ ���Ծ��ٸ�..(Ȥ�� �𸣴� ��Ȱ��ȭ)
        {
            Cloud_isActive(false);      // ������ ��Ȱ��ȭ
        }

        if ((scoreInt % 5).Equals(0) && !scoreInt.Equals(0))      // �¸� 5���� ���� ��������....
        {
            Cloud_isActive(true);   // ������ Ȱ��ȭ
            Cloud_isTouch(false);   // ������ ������ ��ġ ���ϵ��� �Ѵ�...
        }

        if ((scoreInt % 20).Equals(4) && scoreInt > 43)             // ���ƴٴϴ� ������Ʈ �������� ��
        {
            Fly_isActive(true);     // ���ƴٴϴ� ������Ʈ�� Ȱ��ȭ ��
        }

        Stage_fuction(stageLevel);    // ���� �������� ����
    }


    void Stage_fuction(int stageLevel)          // ���� �������� ���� �Լ�
    {
        audio_Bowl.pitch = 0.1f;


        if (stageLevel >= 3)   // ���� 2���� ���;� ��..
        {
            donut_Array[0].SetActive(true);    // ���� �ΰ� Ȱ��ȭ
            donut_Array[1].SetActive(true);

            int randInt_Two = Random.Range(0, 2);    // ������ �̸� ����
            if (randInt_Two.Equals(0))               // ���� ������ �ɸ���...
            {
                donut_Image.sprite = sprite_Array[0];
                isDonutStrow = true;                       // ���� ������ �ɷȴٰ� �˷���
            }
            else                                      // ���� ������ �ɸ���...
            {
                donut_Image.sprite = sprite_Array[1];
                isDonutStrow = false;                       // ���� ������ �ɷȴٰ� �˷���
            }
        }
        else                   // ���� 1���� ���;� �Ѵ�.
        {
            donut_Array[0].SetActive(true);      // ���� ���Ӹ� Ȱ��ȭ
        }

        donut_Image.gameObject.SetActive(false);
        switch (stageLevel)             // �������� ������ ����~
        {
            case 0:            // �������� ���� 1.. ���� 3��
                bowlInt = 3;   // �׸� ������ 3���� ��
                speed = 1.0f;  // �׸� ������ ���ǵ带 1�� �ʱ�ȭ
                break;
            case 1:            // �������� ���� 2.. ���� 4��
                bowlInt = 4;   // �׸� ������ 4���� ��
                speed = 1.0f;  // �׸� ������ ���ǵ带 1�� �ʱ�ȭ
                break;
            case 2:            // �������� ���� 3.. ���� 5��
                bowlInt = 5;   // �׸� ������ 5���� ��
                speed = 1.0f;  // �׸� ������ ���ǵ带 1�� �ʱ�ȭ
                break;
            case 3:            // �������� ���� 4.. ���� 3��
                bowlInt = 3;   // �׸� ������ 3���� ��
                speed = 1.5f;  // �׸� ������ ���ǵ带 1.5�� �ʱ�ȭ
                break;
            case 4:            // �������� ���� 5.. ���� 4��
                bowlInt = 4;   // �׸� ������ 4���� ��
                speed = 1.8f;  // �׸� ������ ���ǵ带 1.8�� �ʱ�ȭ
                break;
            default:            // �������� ���� 6.. ���� 5��
                bowlInt = 5;   // �׸� ������ 5���� ��
                speed = 2.1f;  // �׸� ������ ���ǵ带 2.1�� �ʱ�ȭ
                break;
        }


        randInt = Random.Range(0, bowlInt);    // �׸� ��ġ�� �������� �̾ƿ´�.
        for (int i = 0; i < 5; i++)            // Ȱ��ȭ�� �׸��� ������� ������ 5�� ��� �׳� ��Ȱ��ȭ ��Ŵ
        {
            cups[i].gameObject.SetActive(false);
        }


        if (bowlInt.Equals(3))     // �׸� ������ 3�����..
        {
            Donut_Setting(stageLevel, Pos_3x3, randInt);       // ���� ��ġ ����
        }
        else if (bowlInt.Equals(4))    // �׸� ������ 4�����..
        {
            Donut_Setting(stageLevel, Pos_4x4, randInt);       // ���� ��ġ ����
        }
        else                           // �׸� ������ 5�����..
        {
            Donut_Setting(stageLevel, Pos_5x5, randInt);       // ���� ��ġ ����
        }
        anim.SetBool(anim_Id02, true);       // �׸� 90�� ȸ����Ű�� �ִϸ��̼� ����
    }


    void Donut_Setting(int stageLevel, Transform[] pos_Array, int randInt)      // ���� ��ġ ���� �Լ�
    {
        if (stageLevel >= 3)                  // �������� ������ 3�̻��̶��...
        {
            int first = Random.Range(0, bowlInt);
            int second = Random.Range(0, bowlInt);

            while (first.Equals(second))         // �ߺ� ����..
            {
                second = Random.Range(0, bowlInt);
            }
            
            firstInt = first;       // ���� ������ ��ġ�� ��´�.
            secondInt = second;     // ���� ������ ��ġ�� ��´�.

            donut_Array[0].transform.position = pos_Array[first].position;     // ���� ������ ��ġ�� �����Ѵ�.
            donut_Array[1].transform.position = pos_Array[second].position;     // ���� ������ ��ġ�� �����Ѵ�.
        }
        else                                   // �������� ������ 3 �̸��̶��..
        {
            donut_Array[0].transform.position = pos_Array[randInt].position;     // ���� ������ ��ġ�� �����Ѵ�.
        }

        for (int i = 0; i < pos_Array.Length; i++)      // �迭�� ���̿� ����..
        {
            cups[i].gameObject.SetActive(true);         // �տ��� ���� Ȱ��ȭ
            cups[i].position = pos_Array[i].position;   // ��ġ�� �տ��� ����..
        }
    }


    void Score_Up()         // ���ھ �ø���. (�������� ���)
    {
        scoreInt++;         // ���ھ �ø���
        scoreText.text = scoreInt.ToString();   // �ؽ�Ʈ�� �ݿ��Ѵ�.
    }

    void Fail_Up()          // �й踦 �ø���. (2������ ����)
    {
        failInt--;          // �й� ������ �ø���

        Hp_List[failInt].SetActive(false);

        if (failInt.Equals(0))
        {
            Result_Panel.SetActive(true);
            Game_Panel.SetActive(true);

            Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + scoreInt.ToString();

            if (Main.ins.nowPlayer.maxScore_List[3] >= scoreInt)    // �ְ����� �� �����ٸ�...
            {
                Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[3].ToString();
            }
            else        // �ְ����� ���� ��� (�ű��)
            {
                Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + scoreInt.ToString();

                Main.ins.nowPlayer.maxScore_List[3] = scoreInt;
                Main.ins.SaveData();

				GPGS_Button.interactable = true;        // �ű���� ����� ���� ���� ��� ��ư�� Ȱ��ȭ�Ѵ�. /////////////////////////////////////////////////////////////////////////////
			}

            AudioMng.ins.Pause_BG();              // ��������� ����.
            AudioMng.ins.PlayEffect("Fail02");

            Time.timeScale = 0;
        }
    }

	public void Press_GPGS_04()   // �ű�� ��, ������ ���
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no4, scoreInt);   // ������ ������ ���
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no4);          // �������带 ����.
	}


	public void Bowl_RotEnd()           // �ִϸ��̼ǿ� ������ ....90�� ȸ���� ������...
    {
        if (scoreInt < 15)     // ���ھ 15�̸��̶��..(�������� ���� 3 �̸�)
        {
            donut_Array[0].transform.parent = cups[randInt];           // ���� ������ �θ�
            donut_Array[0].transform.rotation = Quaternion.Euler(Vector3.zero);   // ȸ���� 0����..
        }
        else                   // ���ھ 15�̻��̶��..(�������� ���� 3 �̻�)
        {
            donut_Array[0].transform.parent = cups[firstInt];              // ����, ���� ���� ����
            donut_Array[0].transform.rotation = Quaternion.Euler(Vector3.zero);
            donut_Array[1].transform.parent = cups[secondInt];
            donut_Array[1].transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        anim.SetBool(anim_Id02, false);         // 90�� ȸ�� �ִϸ��̼��� ����
        //StartCoroutine(Wait_Bowl());          //
        StartCoroutine(Shake(30));    // �̶�, �׸��� ���´�..
    }

    Vector3 BezierTest(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4, float Value)   // ������ � �Լ�
    {
        Vector3 A = Vector3.Lerp(P_1, P_2, Value);
        Vector3 B = Vector3.Lerp(P_2, P_3, Value);
        Vector3 C = Vector3.Lerp(P_3, P_4, Value);

        Vector3 D = Vector3.Lerp(A, B, Value);
        Vector3 E = Vector3.Lerp(B, C, Value);

        Vector3 F = Vector3.Lerp(D, E, Value);

        return F;
    }

    public void HitAndShield_Check()   // ���Ϳ� ������ �ִϸ��̼ǿ� ������. �÷��̾ ������ ���� ���� üũ �Լ�
    {
        if (isShield.Equals(true))     // ���� ���� ���¶��..
        {
            AudioMng.ins.PlayEffect("HitBowl");     // �׸����� �и�ġ�� ����
            StartCoroutine(Fly_Coroutine());
        }
        else                           // ���� ���� ���°� �ƴ϶��..
        {
            AudioMng.ins.PlayEffect("HitHammer");     // �и�ġ�� ����
            StartCoroutine(Fail_Wait());
        }
    }


    //////////////////   �ڷ�ƾ ����

    IEnumerator Attack_Monster()             // ���Ͱ� �÷��̷��� ���� ����(�и�ġ)�� �ϴ� �ڷ�ƾ
    {
        while (mini04_Bowl.isBuddle_End.Equals(false))    // �ش� �׸��� �ε� �Ÿ��� �����ٸ�..
        {
            yield return delay_01;    // �ε� �Ÿ� ���������� ��� ������..
        }

        enermy_Bat.SetActive(true);             // �� ��ġ ����
        anim_Monster.Play(anim_Id04, -1, 0.0f);     // ������ �÷��̾�� �и�ġ�� ������ �ִϸ��̼�
        anim_Monster.speed = 0.3f;             // �ӵ� ����(�������� ���� �ӵ��� �÷��� ��)

        yield return delay_02;    // �� �ڷ�ƾ�̶� �����
    }


    IEnumerator Fail_Wait()             // �й����� �� fail �Լ� ����� ���� ���������� ���� �� ����
    {
        Fail_Up();                  // �й踦 �ø�
        yield return delay_02;      // 2�� ��ٸ���..
        Setting_Fuction(scoreInt);  // �������� �ʱ�ȭ ���� ����
    }


    IEnumerator Fly_Coroutine()          // �Ӹ��� ���� ����� ƨ���� ������ �ڷ�ƾ
    {
        float time = 0;         // �� �ð����� ƨ���� ����
        isShield = false;       // ���尡 �������ٰ� �˷���
        Vector3 originPos_Head = temp_Helmet.transform.position;      // ����� ó�� ��ġ�� ����
        Quaternion originRot_Head = temp_Helmet.transform.rotation;   // ����� ó�� ȸ���� ����

        while (time < 2.0f)   // 2�ʵ���
        {
            time += Time.deltaTime;     
            temp_Helmet.transform.position = Vector3.MoveTowards(temp_Helmet.transform.position, HeadThrow_Pos.position, Time.deltaTime * 4.0f);
            temp_Helmet.transform.rotation = Quaternion.Euler(new Vector3(time * 500.0f, temp_Helmet.transform.rotation.y, temp_Helmet.transform.rotation.z));
            // ����� ƨ�� ���� ���̵��� �Ѵ�.
            yield return delay_01;   // 2�� �� ���� ��� ����
        }

        temp_Helmet.SetActive(false);         // ƨ���� ��� ��Ȱ��ȭ.
        temp_Helmet.transform.position = originPos_Head;  // ƨ���� ����� �ٽ� �� ��ġ��..
        temp_Helmet.transform.rotation = originRot_Head;

        Setting_Fuction(scoreInt);           // �������� �ʱ�ȭ ���� ����

        yield return delay_01;
    }




    IEnumerator Attack_Player()                      // �÷��̾ ������ ���� ������ �ڷ�ƾ
    {
        AudioMng.ins.PlayEffect("SpeedUp");    // ��� ������ �Ҹ�

        GameObject tempThrow_Donut;                  // ������ ������ �ӽ÷� �޴� ����
        if (isDonutStrow.Equals(true))               // ������ ������ ���� �����̶��..
        {
            donutThrow_Array[0].SetActive(true);     // ������ ���� ���� Ȱ��ȭ..
            tempThrow_Donut = donutThrow_Array[0];   // ���� ������ �ӽ� ������ ��´�..
        }
        else                                          // ������ ������ ���� �����̶��...
        {
            donutThrow_Array[1].SetActive(true);     // ������ ���� ���� Ȱ��ȭ..
            tempThrow_Donut = donutThrow_Array[1];   // ���� ������ �ӽ� ������ ��´�.
        }

        anim_Player.SetBool(anim_Id03, true);       // �÷��̾� ���� �ִϸ��̼� ����(������ ���)
        bool isTouch = false;                        // ������ ������ ���Ϳ� �¾ҳ� ���� ����

        while (isTouch.Equals(false))
        {
            tempThrow_Donut.transform.position = Vector3.MoveTowards(tempThrow_Donut.transform.position, Enemy_HeadPos.position, 4.0f * Time.deltaTime);
            // ������ ������ ���� �Ӹ������� ������.
            if ((Enemy_HeadPos.position - tempThrow_Donut.transform.position).magnitude < 0.1f)  // ������ ������ ���Ϳ� ���������..
            {
                isTouch = true;                            // �¾Ҵٰ� �˷���(�ݺ� �ߴ�)
                tempThrow_Donut.SetActive(false);          // ������ ���� ��Ȱ��ȭ..
                tempThrow_Donut.transform.position = originPos_ThrowDonut;  // ������ ���� ��ġ �ʱ�ȭ..
                anim_Monster.Play(anim_Id05, -1, 0.0f);         // �¾� �������� �ִϸ��̼� ����
                anim_Player.SetBool(anim_Id03, false);              // �÷��̾� ���� �ִϸ��̼� �ߴ�

                AudioMng.ins.PlayEffect("HitApple");    // ���� ����� �°� �������� �Ҹ�
                Score_Up();                     // ���ھ �ø���..
                yield return delay_02;          // 2�ʰ� ����..

                anim_Monster.Play(anim_Id01);      // ���͸� �ٽ� �Ͼ�� ��...

                yield return delay_02;          // �ٽ� 2�ʰ� ����..
                Setting_Fuction(scoreInt);      // �������� �ʱ�ȭ ���� ����
            }

            yield return delay_01;
        }

        yield return delay_02;
    }


    IEnumerator Shake(int count)        // �׸��� ���� �ڷ�ƾ..
    {
        yield return delay_02;     // 2�ʰ� ��ٸ���.
        isRotEnd = true;           // �̋� 90�� ȸ���� �����ٰ� �˸���.

        if (isCloud.Equals(true))  // ������ ���� �־��ٸ�..
        {
            Cloud_isTouch(true);   // ������ ��ġ �� �� �ִٰ� �˸���..
        }

        while (count > 0)        // ������ ���� ��ŭ �ݺ�...
        {
            int first = Random.Range(0, bowlInt);
            int second = Random.Range(0, bowlInt);

            audio_Bowl.Play();
            audio_Bowl.pitch += 0.1f;

            while (first.Equals(second))           // �ߺ� ����..
            {
                second = Random.Range(0, bowlInt);
            }

            Transform first_Obj = cups[first];
            Transform second_Obj = cups[second];

            Vector3 origin_01 = first_Obj.position;
            Vector3 origin_02 = second_Obj.position;

            Vector3 P1_1 = first_Obj.position;
            Vector3 P2_1 = new Vector3(first_Obj.position.x, -1.15f , 1.55f);   // -0.5f
            Vector3 P3_1 = new Vector3(second_Obj.position.x, -1.15f, 1.55f);   // -0.5f
            Vector3 P4_1 = second_Obj.position;
             
            Vector3 P1_2 = first_Obj.position;
            Vector3 P2_2 = new Vector3(first_Obj.position.x, -1.15f, -0.5f);   // -0.5f
            Vector3 P3_2 = new Vector3(second_Obj.position.x, -1.15f, -0.5f);   // -0.5f
            Vector3 P4_2 = second_Obj.position;


            while (Test <= 1)
            {
                Test += Time.deltaTime * speed;

                first_Obj.position = BezierTest(P1_1, P2_1, P3_1, P4_1, Test);
                second_Obj.position = BezierTest(P4_2, P3_2, P2_2, P1_2, Test);

                yield return delay_01;
            }
            speed += 0.1f;

            first_Obj.position = origin_02;
            second_Obj.position = origin_01;
            Test = 0;
            count--;
            yield return delay_01;
        }

        

        isCurveEnd = true;           // �׸��� ���� �����ٰ� �˸���.
        donut_Image.gameObject.SetActive(true);

        if (isCloud.Equals(true))    // ������ Ȱ��ȭ �Ǿ� �ִٸ�..
        {
            Cloud_isTouch(false);    // ���� ��ġ�� ���ذ� �Ѵ�..
        }

        if (isFly.Equals(true))      // ���ƴٴϴ� ������Ʈ�� Ȱ��ȭ �Ǿ� �ִٸ�..
        {
            Fly_isActive(false);     // ���ƴٴϴ� ������Ʈ�� ��Ȱ��ȭ..
        }
    }
}
