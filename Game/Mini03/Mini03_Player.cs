using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class Mini03_Player : MonoBehaviour         // �̴�03 �÷��̾� ������
{
    [SerializeField] Joystic joystic;

    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // ������ ������ ��Ͻ�ų �� �ִ� ��ư

	Animator anim;                       // �ִϸ����ʹ� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����
    Rigidbody rigid;                 // ������ٵ�� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    NavMeshAgent nma;
    [SerializeField] protected float speed;        // �̵� �ӵ� ������ �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����
    Vector3 rotBan = Vector3.zero;                      // ������ٵ�� �̵� ��, ĳ���Ͱ� ��� ȸ���ϴ� ������ ���� ���� ����
    Vector3 moveDir;                     // �̵� ������ �÷��̾ ���� ���ɼ� ����

    Renderer playerRenderer;
    Collider playerCol;

    GameObject playerOfBin;
    Vector3 originPos;

    [SerializeField] Mini03_Spawn mini03_Spawn;

    [SerializeField] Button Right_Button;
    [SerializeField] Image Right_image;
    [SerializeField] Sprite[] sprite_Array;       // 0 : �� ��, 1 : �� ��


    public bool isInBin = false;            // ���� �÷��̾ �������뿡 �� �ֳİ� ����� ����

    bool isMonLight = false;

    GameObject player_Light;
    Vector3 open_Pos;
    Quaternion open_Rot;
    Vector3 closed_Pos;
    Quaternion closed_Rot;

    string hor_Text;
    string ver_Text;

    string tag01;
    string tag02;
    string tag03;
    string tag04;
    string tag05;
    string tag06;
    string tag07;

    int runId;

    void Start()
    {
        playerCol = GetComponent<Collider>();      // �÷��̾��� �ݶ��̴��� �޴´�.
        playerRenderer = transform.GetChild(1).GetComponent<Renderer>();       // �÷��̾��� �������� �޴´�.
        nma = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();              // �÷��̾��� ������ �ٵ� ������ �´�.
        anim = GetComponent<Animator>();                // �÷��̾��� �ִϸ��̼��� ������ �´�.

        hor_Text = "Horizontal";
        ver_Text = "Vertical";

        tag01 = "Note";
        tag02 = "Player";
        tag03 = "Spring";
        tag04 = "Monster";
        tag05 = "Coin";
        tag06 = "BlackHole";             //
        tag07 = "Bear";

        runId = Animator.StringToHash("isRun");

        Right_Button.interactable = false;              // ó������ �������뿡 ������ ������ ��ư�� ��Ȱ��ȭ ��Ŵ

        Right_image.sprite = sprite_Array[0];

        player_Light = transform.GetChild(2).gameObject;
        open_Pos = new Vector3(-0.3f, 1.6f, 0);
        open_Rot = Quaternion.Euler(new Vector3(-75, -90, 90));
        closed_Pos = new Vector3(0, 1.6f, 0);
        closed_Rot = Quaternion.Euler(new Vector3(-90, 0, 0));

        Stage_Setting();

        AudioMng.ins.Play_BG("Mini03_B");
    }

    void FixedUpdate()
    {
        rigid.velocity = moveDir * speed;          // ���ν�Ƽ�� �̵�

        if (!moveDir.x.Equals(0) || !moveDir.z.Equals(0))               // wasd �ƹ��ų��� �����δٸ�..
        {
            anim.SetBool(runId, true);         // �����̴� �ִϸ��̼� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z)), 0.2f);
            // �÷��̾��� ȸ�� ��
        }
        else
        {
            anim.SetBool(runId, false);       // ���ߴ� �ִϸ��̼� ����
            rigid.angularVelocity = rotBan;          // �÷��̾ ���𰡿� �浹�� ��� ȸ���ϴ� ������ �־ ���ƹ���
        }
    }

    void Update()
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

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
			Press_RightButton();
		}
    }


	public void Press_RightButton()         // ���� ��ư, �������뿡 ����
	{
        if (playerOfBin != null)          // ������ �뿡 ��� �ִٸ�..
        {
            moveDir.x = 0.0f;
            moveDir.z = 0.0f;

            if (isInBin.Equals(false))           // ���� �÷��̾ �������뿡 �� �� ���¶��..(���� ���� �Ѵ�!)
            {
                if (isMonLight.Equals(true))     // ���� �Һ��� ��� ���¶��..
                {
                    return;
                }

                AudioMng.ins.PlayEffect("Click02");

                isInBin = true;

                gameObject.tag = tag01;         // �÷��̾� �ױ� �ٲٱ�
                gameObject.layer = 1;            // �÷��̾� ���̾� �ٲٱ�

                Right_image.sprite = sprite_Array[1];
                originPos = transform.position;   // ���� �÷��̾��� ��ġ�� ��Ƶα�

                transform.position = new Vector3(playerOfBin.transform.position.x, 0.0f, playerOfBin.transform.position.z);
                playerRenderer.enabled = false;   // �÷��̾� �������� ���ֹ���

                nma.enabled = false;         // �׺�޽� ����
                playerCol.enabled = false;           // �ݶ��̴� ����

                playerOfBin.transform.GetChild(1).transform.localPosition = closed_Pos;
                playerOfBin.transform.GetChild(1).transform.localRotation = closed_Rot;
                player_Light.SetActive(false);

                speed = 0.0f;
            }
            else                                // ���� �÷��̾ �������뿡 �� ���¶��..(���� ���;� �Ѵ�!)
            {
                AudioMng.ins.PlayEffect("Back");

                isInBin = false;

                gameObject.tag = tag02;        // �±� ������� �ϱ�
                gameObject.layer = 6;             // ���̾� ������� �ϱ�

                Right_image.sprite = sprite_Array[0];

                transform.position = originPos;   // ��Ƶξ��� ��ġ�� �÷��̾� ��ġ�� �ű�
                playerRenderer.enabled = true;   // �÷��̾� �������� �ٽ� �츲

                nma.enabled = true;          // �׺�޽� �ѱ�
                playerCol.enabled = true;            // �ݶ��̴� �ѱ�

                playerOfBin.transform.GetChild(1).transform.localPosition = open_Pos;
                playerOfBin.transform.GetChild(1).transform.localRotation = open_Rot;
                player_Light.SetActive(true);

                speed = 4.0f;
            }
        }
        else                                   // ������ �뿡 ���� �ʾҴٸ�...(�׳� ����)
        {

        }
	}

    void Stage_Setting()        // ���������� �����ϰ�, �÷��̾� ĳ���͸� �̵���Ű�� �Լ�
    {
        nma.enabled = false;                // �׺�޽� ����(�÷��̾ �ٸ� ���������� �Űܾ� �ؼ�)
        Transform stageTrans = mini03_Spawn.StageSpawn();     // ���������� �����Ѵ�.
        transform.position = stageTrans.GetChild(2).position;      // �÷��̾��� ��ġ�� ���� ������ ���������� start point�� �̵���Ų��.
        nma.enabled = true;                 // �׺�Ž� �ٽ� �ѱ�(�÷��̾� �� �����̸� �ٸ� ���������� �Ű��� �־ �ٽ� �ѱ�)
    }


    ///////////////// Ʈ���� ����......


    public void GameOver()
    {
        Result_Panel.SetActive(true);     // ��� â�� ��� �Ѵ�.
        Game_Panel.SetActive(true);

        Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + mini03_Spawn.stageInt.ToString();

        if (Main.ins.nowPlayer.maxScore_List[2] >= mini03_Spawn.stageInt)    // �ְ����� �� �����ٸ�...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[2].ToString();
        }
        else        // �ְ����� ���� ��� (�ű��)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + mini03_Spawn.stageInt.ToString();

            Main.ins.nowPlayer.maxScore_List[2] = mini03_Spawn.stageInt;
            Main.ins.SaveData();

			GPGS_Button.interactable = true;        // �ű���� ����� ���� ���� ��� ��ư�� Ȱ��ȭ�Ѵ�. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // ��������� ����.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;     // ����
    }

	public void Press_GPGS_03()   // �ű�� ��, ������ ���
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no3, mini03_Spawn.stageInt);   // ������ ������ ���
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no3);          // �������带 ����.
	}


	void OnTriggerEnter(Collider other)
    {
        if (isInBin.Equals(false))                     // �÷��̾ �������뿡 �� ���ٸ�..
        {
            if (other.CompareTag(tag03))              // ���� ���������� ���� �ٴ��� ������...
            {

                AudioMng.ins.PlayEffect("Score_Up");
                Stage_Setting();                        // ���� �������� ���� ����
            }

            if (other.CompareTag(tag04))                    // ���Ϳ� ������...
            {

                if (other.name.Equals("Pipe02"))         // Ÿ���� �ɸ���...
                {
                    other.transform.parent.LookAt(transform.position);   // Ÿ���� �÷��̾ ������ �Ѵ�.
                }


                GameOver();
            }

            if (other.CompareTag(tag01))    // ���� ���� �ٷ� �տ� ������..
            {
                other.transform.GetChild(0).gameObject.SetActive(true);        // �ʷϻ� ���� ���� �ö󰡴� ��ƼŬ ������ �Ѵ�.
            }

            if (other.CompareTag(tag05))    // �Ҹ� ������ ������...
            {
                AudioMng.ins.PlayEffect("Check");         // ���� �Ҹ� ���� �Ѵ�.
            }
        }
    }

    void OnTriggerStay(Collider other)          // �������� ������ ����
    {
        if (isInBin.Equals(false))                     // �÷��̾ �������뿡 �� ���ٸ�..
        {
            if (other.CompareTag(tag06))
            {
                playerOfBin = other.gameObject;     // ���� ���������� ��� ������Ʈ�� �ֱ�

                Right_Button.interactable = true;
            }
            
            if (other.CompareTag(tag07))    // ���� ����� ��´ٸ�..
            {
                isMonLight = true;                   // ���� ����� ��Ҵٰ� �˸�
            }
        }
    }

    void OnTriggerExit(Collider other)          // �������� ������ ������
    {
        if (other.CompareTag(tag06))    // ��������� ���� �ʴ´ٸ�..
        {
            playerOfBin = null;             // �������� Ʈ���ſ��� �������Դٸ� ���� ���������� ��� ������Ʈ�� null �ֱ�

            Right_Button.interactable = false;
        }

        if (other.CompareTag(tag07))    // ���� ����� �� ��´ٸ�..
        {
            isMonLight = false;                   // ���� ����� �� ��Ҵٰ� �˸�
        }
    }
}
