using UnityEngine;
using UnityEngine.UI;

public class Mini02_Player : MonoBehaviour
{
    [SerializeField] Joystic joystic;

    public bool isHoleOrStar = false;       // ���� ���� �������, ��Ÿ ������� ���� ����
    public bool isOvenOrFryer = false;      // ���� ������ ���� ���¿�����, Ƣ�� ���¿����� ���� ����

    public int playerCompleteInt = 0;           // 0 : ����, 1 : Ʋ, 2 : ����, Ƣ��, 3 : ����

    bool isMoving = false;        // ���� �÷��̾ ����Ű�� �����̰� �ִ��� �˾ƺ��� ����
    bool isMove = false;

    Vector3 rayPos;         // ���� ũ�⸦ �����ϴ� ����
    Vector3 posUp;          // �÷��̾ ���̻� ���� �� �ö󰡰� �ϴ� ����
    Vector3 posDown;        // �÷��̾ ���̻� �Ʒ��� �� �������� �ϴ� ����

    Vector3 dough_Pos;       // Űģ ī�޶��� ��ġ ����, ȸ�� ��
    Quaternion dough_Rot;
    Vector3 frame_Pos;
    Quaternion frame_Rot;
    Vector3 oven_Pos;
    Quaternion oven_Rot;
    Vector3 fry_Pos;
    Quaternion fry_Rot;
    Vector3 topping_Pos;
    Quaternion topping_Rot;

    [SerializeField] Transform kitchenCamera;     // Űģ ī�޶�
    int shelfNum;                   // ���� �÷��̾ ��� ī���Ϳ� �ִ��� �˸��� ����

    bool isPanel = false;              // �÷��̾ �ٸ� ���� �ϰ� �ִ��� ���� ����

    [SerializeField] GameObject RightButton;              // ������ ��ư(����)

    [SerializeField] Image Right_Image;

    [SerializeField] GameObject ScorePanel;               // ���ھ�� �ð��� ǥ���Ǵ� �г�(����)
    [SerializeField] GameObject[] Donut_Panel;            // �� ī��Ʈ���� �г� 0 : , 1 : , 2 : , 3 : , 4 :

    public int scoreInt = 0;

    Vector3 origin_CameraPos;
    Vector3 moveDir;                     // �̵� ������ �÷��̾ ���� ���ɼ� ����

    string tag01;
    string tag02;
    string tag03;
    string tag04;
    string tag05;

    string hor_Text;
    string ver_Text;

    void Start()
	{
        playerCompleteInt = 0;

        tag01 = "BlackHole";
        tag02 = "Bear";
        tag03 = "Spring";
        tag04 = "Honey";
        tag05 = "Cushion";

        hor_Text = "Horizontal";
        ver_Text = "Vertical";

        dough_Pos = new Vector3(-139.9f, 4.2f, 0.83f);
        dough_Rot = Quaternion.Euler(new Vector3(56.742f, 0, 0));
        frame_Pos = new Vector3(-100.62f, 8.66f, -5.55f);
        frame_Rot = Quaternion.Euler(new Vector3(46.458f, 0, 0));
        oven_Pos = new Vector3(-65.39f, 2.57f, 16.04f);
        oven_Rot = Quaternion.Euler(new Vector3(18.61f, 0, 0));
        fry_Pos = new Vector3(-64.98f, 6.203f, 21.787f);
        fry_Rot = Quaternion.Euler(new Vector3(60.858f, 0, 0));
        topping_Pos = new Vector3(-9.91f, 8.54f, 42.39f);
        topping_Rot = Quaternion.Euler(new Vector3(90.0f, 0, 0));

        origin_CameraPos = kitchenCamera.position;

        rayPos = new Vector3(0, 0.5f, 0);       // ���� ���� ���� ��

        posUp = new Vector3(0, 0, 2);           // �̵� ���� ��
        posDown = new Vector3(0, 0, -2);        
        OneShotRay();                        // ó�� ����� ����� �˾ƺ���.

        AudioMng.ins.Play_BG("Mini02_B");
    }


	void Update()
	{
        if (isPanel.Equals(false))
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) && isPanel.Equals(false))
        {
            PressShelf();
        }
    }

    

    void Move()         // �̵� �Լ��� �θ�ɷ� ��
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

        isMoving = (!moveDir.z.Equals(0) || !moveDir.x.Equals(0));   // ���̽�ƽ�� �����̸� ���ڸ�(0 ~ 1) ������ ���

        if (isMoving.Equals(false))   //  �������� �ʾҴٸ�...
        {
            isMove = true;       // �ʱ�ȭ...?
        }
        else
        {
            if (moveDir.z > 0.3f && isMove.Equals(true) && transform.position.z <= 3.5f)   // �� ����Ű�� �����̰�, �����̴� ������ true�̸�, �׸��� �÷��̾� �̵� ����
            {
                isMove = false;                   // �Ҷ� ���� ���� �����̴� ������ false�� �д�.(�ᱹ �ѹ� �����̸�, ������ �ȴ�....)

                transform.position += posUp;    // �� ĭ�� �����̵��� �̵�
                OneShotRay();
            }
            else if (moveDir.z < -0.3f && isMove.Equals(true) && transform.position.z >= -3.5f)           // �Ʒ� ����Ű�� �����̰�, �����̴� ������  true�̸�
            {
                isMove = false;                    // �Ҷ� ���� ���� �����̴� ������ false�� �д�.(�ᱹ �ѹ� �����̸�, ������ �ȴ�....)

                transform.position += posDown;    // �� ĭ�� �����̵��� �̵�
                OneShotRay();
            }
        }
    }


    void OneShotRay()            // ���̸� �ѹ��� ����� ����
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(this.transform.position + rayPos, this.transform.forward, out hitInfo))      // ������ ���̸� ���.
        {
            if (hitInfo.transform.CompareTag(tag01))        // ���� ����
            {
                shelfNum = 0;
            }
            else if (hitInfo.transform.CompareTag(tag02))             // Ʋ ����
            {
                shelfNum = 1;
            }
            else if (hitInfo.transform.CompareTag(tag03))           // ���� ����
            {
                shelfNum = 2;
            }
            else if (hitInfo.transform.CompareTag(tag04))            // Ƣ�� ����
            {
                shelfNum = 3;
            }
            else if (hitInfo.transform.CompareTag(tag05))          // ���� ����
            {
                shelfNum = 4;
            }

        }
    }

    public void PressShelf()         // ��ư�� ������ ���
    {
        switch (shelfNum)                  // ���� ī��Ʈ�� ����
        {
            case 0:        // ���� ����
                isPanel = true;                      // �г��� ���ȴٰ� �˸�
                ScorePanel.SetActive(false);         // ���ھ� �г��� ��
                RightButton.SetActive(false);        // ������ ��ư�� ��

                AudioMng.ins.PlayEffect("Enter");    // �г� ���� �Ҹ�

                kitchenCamera.GetComponent<Camera>().depth = 1;

                kitchenCamera.position = dough_Pos;    // Űģ ī�޶� ���� Űģ������ �̵���Ŵ
                kitchenCamera.rotation = dough_Rot;
                Donut_Panel[0].SetActive(true);        // ���� �г��� ����.
                break;
            case 1:             // Ʋ ����
                if (playerCompleteInt.Equals(1))
                {
                    isPanel = true;                      // �г��� ���ȴٰ� �˸�
                    ScorePanel.SetActive(false);         // ���ھ� �г��� ��
                    RightButton.SetActive(false);        // ������ ��ư�� ��

                    AudioMng.ins.PlayEffect("Enter");    // �г� ���� �Ҹ�

                    kitchenCamera.position = frame_Pos;    // Űģ ī�޶� Ʋ Űģ������ �̵���Ŵ
                    kitchenCamera.rotation = frame_Rot;
                    Donut_Panel[1].SetActive(true);        // Ʋ �г��� ����.
                }
                
                break;
            case 2:           // ���� ����
                if (playerCompleteInt.Equals(0) || playerCompleteInt.Equals(1) || playerCompleteInt.Equals(3))
                {
                    break;
                }
                isPanel = true;                      // �г��� ���ȴٰ� �˸�
                ScorePanel.SetActive(false);         // ���ھ� �г��� ��
                RightButton.SetActive(false);        // ������ ��ư�� ��

                AudioMng.ins.PlayEffect("Enter");    // �г� ���� �Ҹ�

                kitchenCamera.position = oven_Pos;    // Űģ ī�޶� ���� Űģ������ �̵���Ŵ
                kitchenCamera.rotation = oven_Rot;
                Donut_Panel[3].SetActive(true);        // ���� �г��� ����.
                break;
            case 3:            // Ƣ�� ����
                if (playerCompleteInt.Equals(0) || playerCompleteInt.Equals(1) || playerCompleteInt.Equals(3))
                {
                    break;
                }
                isPanel = true;                      // �г��� ���ȴٰ� �˸�
                ScorePanel.SetActive(false);         // ���ھ� �г��� ��
                RightButton.SetActive(false);        // ������ ��ư�� ��

                AudioMng.ins.PlayEffect("Enter");    // �г� ���� �Ҹ�

                kitchenCamera.position = fry_Pos;    // Űģ ī�޶� Ƣ�� Űģ������ �̵���Ŵ
                kitchenCamera.rotation = fry_Rot;
                Donut_Panel[2].SetActive(true);        // Ƣ�� �г��� ����.
                break;
            default:          // ���� ����
                if (playerCompleteInt.Equals(3))
                {
                    isPanel = true;                      // �г��� ���ȴٰ� �˸�
                    ScorePanel.SetActive(false);         // ���ھ� �г��� ��
                    RightButton.SetActive(false);        // ������ ��ư�� ��

                    AudioMng.ins.PlayEffect("Enter");    // �г� ���� �Ҹ�

                    kitchenCamera.position = topping_Pos;    // Űģ ī�޶� ���� Űģ������ �̵���Ŵ
                    kitchenCamera.rotation = topping_Rot;
                    Donut_Panel[4].SetActive(true);        // ���� �г��� ����.
                }
                break;
        }


    }

    public void Origin_Panel()         // �ٸ� �гο��� ���� ���´ٸ�...
    {
        AudioMng.ins.LoopEffect(false);
        AudioMng.ins.PlayEffect("Back");      // ���� ����

        RightButton.SetActive(true);                        // ������ ��ư�� Ȱ��ȭ
        kitchenCamera.position = origin_CameraPos;          // Űģ ī�޶� ����Ʈ������ �ű�
        ScorePanel.SetActive(true);                         // ���ھ� �г��� Ȱ��ȭ
        isPanel = false;                                    // �г��� ������ �ִٰ� �˸�
    }

}


