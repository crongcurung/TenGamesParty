using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini10_Player : MonoBehaviour
{
    [SerializeField] Joystic joystic;

    [SerializeField] AudioSource audio_Black;

    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // ������ ������ ��Ͻ�ų �� �ִ� ��ư

	[SerializeField] Mini10_Stage mini10_Stage;
    [SerializeField] Mini10_Camera mini10_Camera;

    [SerializeField] Image Right_Image;
    [SerializeField] Sprite[] sprite_Array;   // 0 : ���ø���, 1 : �ε�����, 2 : ���� ���� ����, 4 : ����, 5 : ���� Ǯ��

    Vector3 moveDir;                     // �̵� ������ �÷��̾ ���� ���ɼ� ����
    Animator anim;                       // �ִϸ����ʹ� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����
    Rigidbody rigid;                 // ������ٵ�� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����
    Vector3 rotBan = Vector3.zero;                      // ������ٵ�� �̵� ��, ĳ���Ͱ� ��� ȸ���ϴ� ������ ���� ���� ����

    [SerializeField] float speed;        // �̵� �ӵ� ������ �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    GameObject nearObject;          // �÷��̾�� ����� ���� �������� �޴� ����
    Transform upTrans;              // �÷��̾� ���� �������� ��� ���� ��ġ
    Transform nearNote;             // �÷��̾ ��� �ִ� ����

    bool isItem;                    // �÷��̾ �������� ��� �ִ��� üũ
    bool isDonut;                   // �÷��̾ ������ ��� �ִ��� üũ


    public bool isCameraMove = false;      // ī�޶� �����̳İ� ���� ����
    public bool isMapCamera = false;

    bool stopPlayer = true;            // �÷��̾ �����ִ���, �����̰� �ִ� ���� ����

    bool startCube = false;          // ó�� ���ǿ��� ������ false�� �ٲ�, �ʺ��� ���� ����
    bool isFirst = false;            // ó�� ������ ���� �� ����

    bool isEnd = false;              // �÷��̾ �������� ���� ����

    Transform blackHoleTrans;      // ��Ȧ ����(9ĭ)�� ��Ҵ��� ���� ����

    int runId;
    int attackId;

    string tag01;
    string tag02;
    string tag03;
    string tag04;
    string tag05;
    string tag06;
    string tag07;
    string tag08;
    string tag09;
    string tag10;

    string hor_Text;
    string ver_Text;

    bool endBool = false;
    bool isSpring = false;    // �������� ��ҳ�?

    void Start()
    {
        isItem = false;
        isDonut = false;

        runId = Animator.StringToHash("isRun");
        attackId = Animator.StringToHash("isAttack");

        tag01 = "Coin";
        tag02 = "Respawn";
        tag03 = "Spring";
        tag04 = "Cushion";
        tag05 = "Bear";
        tag06 = "Honey";
        tag07 = "BlackHole";
        tag08 = "Monster";
        tag09 = "Note";
        tag10 = "Finish";

        hor_Text = "Horizontal";
        ver_Text = "Vertical";

        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        nearObject = null;
        upTrans = transform.GetChild(2).transform;

        StartCoroutine(rayCoroutine());                // ������ ���̸� ��� �ڷ�ƾ ����

        AudioMng.ins.Play_BG("Mini10_B");
    }



	void FixedUpdate()
    {
        rigid.velocity = moveDir * speed;          // ���ν�Ƽ�� �̵�

        if (!moveDir.x.Equals(0) || !moveDir.z.Equals(0))               // wasd �ƹ��ų��� �����δٸ�..
        {
            anim.SetBool(runId, true);         // �����̴� �ִϸ��̼� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z)), 0.2f);
            // �÷��̾��� ȸ�� ��

            stopPlayer = false;                              // �÷��̾ �����̰� �ִٰ� �˷���
        }
        else
        {
            anim.SetBool(runId, false);       // ���ߴ� �ִϸ��̼� ����
            rigid.angularVelocity = rotBan;          // �÷��̾ ���𰡿� �浹�� ��� ȸ���ϴ� ������ �־ ���ƹ���

            stopPlayer = true;                 // �÷��̾ �����ִٰ� �˷���
        }
    }

    void Update()
    {
        if (isEnd.Equals(true))         // �÷��̾ �׾��ٸ� 
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Press_RightButton();   
        }

        if (isCameraMove.Equals(false))         // ī�޶� �̵� ���� �ƴ϶��...
        {
            if (isMapCamera.Equals(false))
            {
                Move();
            }
        }
        else                       // ī�޶� �̵� ���̶��..
        {
            moveDir.x = 0;         // �÷��̾ �����..
            moveDir.z = 0;

        }

        if (blackHoleTrans != null && stopPlayer.Equals(true) && isCameraMove.Equals(false) && isMapCamera.Equals(false))   // ��Ȧ ������Ʈ�� ������� �ʰ�, �÷��̾ �����ְ�, �� ī�޶� �۵������� ������...
        {
            transform.position += (blackHoleTrans.position - transform.position).normalized * Time.deltaTime * 0.5f;
            // �÷��̾ ��Ȧ ������ �̵�
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
    }

    

    public void MinusHp(bool failBool)                  ////////////////////////////////////////////////////////////////////////////////////////////////
    {
        if (failBool.Equals(false))      // ��������.
        {
            transform.GetComponent<Collider>().isTrigger = true;
            rigid.constraints = RigidbodyConstraints.None;                            // ������ �ٵ� ������ ���� ����
            rigid.AddForce(Vector3.down * 5000f);        // �÷��̾ ������ ��������

            speed = 0.0f;
            isEnd = true;

            StartCoroutine(EndGame_Falling());
        }
        else                             // �������� �� �´� ���
        {
            speed = 0.0f;
            isEnd = true;

            End_Game();
        }
        

        
    }

    IEnumerator EndGame_Falling()
    {
        yield return new WaitForSeconds(2.0f);
        End_Game();
    }

	int thisInt = 0;

	public void End_Game()                   // ������ ������ �� �Լ�
    {
        Result_Panel.SetActive(true);
        Game_Panel.SetActive(true);

        int scoreInt = mini10_Stage.stageInt;

        Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + scoreInt.ToString();

        if (Main.ins.nowPlayer.maxScore_List[9] >= scoreInt)    // �ְ����� �� �����ٸ�...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[9].ToString();
        }
        else        // �ְ����� ���� ��� (�ű��)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + scoreInt.ToString();

            Main.ins.nowPlayer.maxScore_List[9] = scoreInt;
            Main.ins.SaveData();

            thisInt = scoreInt;
			GPGS_Button.interactable = true;        // �ű���� ����� ���� ���� ��� ��ư�� Ȱ��ȭ�Ѵ�. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // ��������� ����.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;
    }


	public void Press_GPGS_10()   // �ű�� ��, ������ ���
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no10, thisInt);   // ������ ������ ���
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no10);          // �������带 ����.
	}


	///////////////////////////////
	// ���� �Լ� ����

	public void Press_RightButton()             // ���͸� �����ٸ�..
    {
        if (isMapCamera.Equals(true))
        {
            mini10_Camera.StopMapCamera();

            moveDir.x = 0;         // �÷��̾ �����..
            moveDir.z = 0;


            Right_Image.sprite = sprite_Array[0];   // ���ø��� �̹���
        }

        if (isItem.Equals(true))         // �������� ��� �ִٸ�..
        {
            

            if (isDonut.Equals(true))
            {
                

                if (nearNote != null && nearNote.childCount.Equals(1))       // ���� �÷��̾ ������ ��� �ְ�, ��� �ִ� ������ �������� ���ٸ�
                {
                    

                    anim.SetBool(attackId, false);
                    isItem = false;     // �������� ��� ���� �ʴٰ� �˸���.
                    upTrans.GetChild(0).position = nearNote.position;            // ���ǿ� �ִ� ������ ���� ��ġ�� ��� �ִ� ������ �ִ´�.
                    upTrans.GetChild(0).rotation = Quaternion.Euler(Vector3.zero);
                    upTrans.GetChild(0).parent = nearNote.transform;             // ��� �ִ� ������ �θ� ���� ������ ������ �ٲ۴�.

                    Right_Image.sprite = sprite_Array[0]; 

                    isDonut = false;        // ������ ��� ���� �ʴٰ� �˸���.
                }
            }
            else
            {

            }
        }
        else                            // �������� ���� ���� �ʴٸ�..       isItem == false
        {

            if (nearObject != null)
            {

                isItem = true;              // �������� ��� �ִٰ� �˸�
                anim.SetBool(attackId, true);

                AudioMng.ins.PlayEffect("Cloud");    // ������ ���

                nearObject.transform.position = upTrans.position;     // �÷��̾� ���� �������� �ø���.
                nearObject.transform.parent = upTrans;                // �������� �θ� �ٲ۴�.

                if (nearObject.CompareTag(tag01))    // ��� �� �������� �����̶��..
                {
                    isDonut = true;                   // ������ ��� �ִٰ� �˸�
                    Right_Image.sprite = sprite_Array[2];     // �������� �̹���
                }
                else
                {
                    Right_Image.sprite = sprite_Array[1];      // �浹 �̹���
                }
            }


            if (startCube.Equals(false))        // ��ŸƮ ť�갡 ��Ҵٸ�..
            {
                if (isCameraMove.Equals(false) && nearNote.CompareTag(tag02))     // ī�޶� �̵����� �ʰ�, ��� �ִ� ���� ��ŸƮ ť����..
                {
                    isFirst = true;
                    startCube = true;

                    moveDir.x = 0;         // �÷��̾ �����..
                    moveDir.z = 0;


                    AudioMng.ins.PlayEffect("Click");    // ���� ��ħ
                    mini10_Camera.MapCamera(nearNote.GetChild(0).transform);          // �� ī�޶� �ߵ�

                    Right_Image.sprite = sprite_Array[4];   // ���� Ǯ�� �̹���
                }
            }
        }
    }


    void StepCube(Transform stepCube)             // ���� ���� ������ üũ�Ѵ�.
    {
        int cubeCount = stepCube.GetComponent<Mini10_Cube>().StepPlayer();      // ���� ������ ���ڸ� ���δ�...
        if (cubeCount.Equals(0))                                                // ������ �޾ƿ� ���ڰ� 0�̸� 
        {
            nearNote = null;    // ����� ������ ���ٰ� �˸�
        }


        if (stepCube.childCount.Equals(2))   // ���ع� Ȥ�� �������� �ִ� ���̴�..
        {
            if (stepCube.GetChild(1).gameObject.layer.Equals(8))      // �����۵�...
            {
                if (isItem.Equals(false))       // �������� �� ��� �־��ٸ�,,
                {
                    nearObject = stepCube.GetChild(1).gameObject;      // ����� ������Ʈ�� �������� �ϴ� �ִ´�!
                }
            }
            else         // ���ع�...
            {
                if (stepCube.GetChild(1).CompareTag(tag03))         // �������� ������...
                {
                    CheckItem(stepCube, tag04, 0);    // �������̶� ���� ������Ʈ ��
                }
                else if (stepCube.GetChild(1).CompareTag(tag05))      // ���� ������...
                {
                    CheckItem(stepCube, tag06, 1);    // �������̶� ���� ������Ʈ ��
                }
                else if (stepCube.GetChild(1).CompareTag(tag07)) // ��Ȧ�� ������...
                {
                    CheckItem(stepCube, tag08, 2);    // �������̶� ���� ������Ʈ ��

                    blackHoleTrans = null;     // ���� ��Ȧ ������ ���ٰ� �˸�(��Ȧ�� ������ ���� �ϴ� ����)
                    speed = 3.5f;              // �ӵ��� ������� �ٲ�
                }
            }
        }
        else     // �ƹ� �������̶� ���ع��� ���� ť����..
        {
            if (isDonut.Equals(false))   // ������ �ȵ�� �ִٸ�
            {
                nearObject = null;        // ����� ������Ʈ�� ���ٰ� �˸���.
            }
        }
    }

    void CheckItem(Transform stepCube, string tagString, int itemId)       // �������� ���ϴ� �Լ�
    {
        

        if (isItem.Equals(true) && upTrans.GetChild(0).CompareTag(tagString))   // �÷��̾ �������� ��� �ְ�, �±׳��� �´´ٸ�..
        {
            Destroy(upTrans.GetChild(0).gameObject);     // �÷��̾ ��� �ִ� �������� ���ش�.
            Destroy(stepCube.GetChild(1).gameObject);    // ���� ���� �ִ� ���ع��� ���ش�.

            AudioMng.ins.PlayEffect("Score_Up");    // ������ ����
            Right_Image.sprite = sprite_Array[0];   // ���ø��� �̹���

            isItem = false;                              // �������� ��� ���� �ʴٰ� �˸���.
            nearObject = null;

            anim.SetBool(attackId, false);             // ��� �ٴϴ� �ִϸ��̼� �ߴ�
        }
        else       // �÷��̾ �������� ��� ���� �ʰų�, �±װ� �ȸ´´ٸ�..
        {
            if (itemId.Equals(0))       // ������
            {
                isSpring = true;
                rigid.AddForce(Vector3.up * 100000.0f);     // ����

                AudioMng.ins.PlayEffect("DdiYong");    // �������� ����
            }
            else if (itemId.Equals(1))  // ��
            {
                transform.GetChild(1).GetComponent<Renderer>().enabled = false;      // �÷��̾� ���� ��Ȱ��ȭ
                transform.position = stepCube.GetChild(1).transform.position;
                isEnd = true;    // �����ٰ� �˸�

                moveDir.x = 0;   // �÷��̾� ����
                moveDir.z = 0;

                AudioMng.ins.PlayEffect("BearGrowl");    // ���� ����
            }
            else                        // ��Ȧ
            {
                transform.GetChild(1).GetComponent<Renderer>().enabled = false;      // �÷��̾� ���� ��Ȱ��ȭ
                isEnd = true;    // �����ٰ� �˸�
                moveDir.x = 0;   // �÷��̾� ����
                moveDir.z = 0;

                //AudioMng.ins.Effect_Speed(0.3f);
                //AudioMng.ins.PlayEffect("Meow");    // ��Ȧ�� ����

                audio_Black.Play();
            }

            StartCoroutine(FailCoroutine(true));
        }
    }

    /////////////////////////////////////////////////////////
    // �ڷ�ƾ ����


    IEnumerator FailCoroutine(bool isBool)          // ������ �������� ���� �Ҹ�
    {
        endBool = true;
        yield return new WaitForSeconds(3.0f);

        MinusHp(isBool);
    }


    IEnumerator rayCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.03f);             // �ڷ�ƾ ����ȭ

        LayerMask mask = LayerMask.GetMask("WALL");                  // 
        Vector3 down = transform.TransformDirection(Vector3.down);    // �÷��̾� ������(���� ����) ���� ������ ����
        RaycastHit hitInfo;                                           // hit�� ������ �޾ƿ��� ����
        GameObject PrevObj = null;

        while (isSpring.Equals(false) || endBool.Equals(false))
        {
            if (Physics.Raycast(this.transform.position + Vector3.up * 0.5f, down, out hitInfo, 1.0f, mask)) // ���̸� �Ʒ��� ���� �������� üũ
            {
                if (!hitInfo.transform.gameObject.Equals(PrevObj))        // �̸����� ���� ť�갡 �ٲ������ �Ǵ�...(����ȭ�ε� ��Ʈ���ϴ°� ���� �ɸ�...)
                {
                    nearNote = hitInfo.transform;
                    if (nearNote.CompareTag(tag09))      // �Ϲ� ť��
                    {
                        StepCube(hitInfo.transform);       // ���� üũS
                        isFirst = true;   // ��ŸƮ ���ǿ��� �������Դٰ� �˸�

                        if (startCube.Equals(false))
                        {
                            startCube = true;

                            Right_Image.sprite = sprite_Array[0];   // ���ø��� �̹���
                        }
                    }
                    else if (nearNote.CompareTag(tag02))   // ��ŸƮ ť��
                    {
                        if (isFirst.Equals(false))  // ???
                        {
                            isFirst = true;
                            startCube = false;
                        }
                        

                        if (isItem.Equals(false))     // �������� ��� ���� �ʴٸ�..
                        {
                            nearObject = null;        // ����� ������Ʈ ����
                        }


                        if (nearNote.childCount.Equals(2))   // ���ع� Ȥ�� �������� �ִ� ���̴�..
                        {
                            if (nearNote.GetChild(1).gameObject.layer.Equals(8))      // �����۵�...
                            {
                                if (isItem.Equals(false))       // �������� �� ��� �־��ٸ�,,
                                {
                                    nearObject = nearNote.GetChild(1).gameObject;      // ����� ������Ʈ�� �������� �ϴ� �ִ´�!

                                }
                            }
                        }

                    }
                    else if (nearNote.CompareTag(tag10))   // �ص� ť��
                    {
                        if (isItem.Equals(false))
                        {
                            nearObject = null;
                        }

                        isFirst = true;   // ��ŸƮ ���ǿ��� �������Դٰ� �˸�
                        startCube = true;
                        if (isDonut.Equals(true))       // ������ �˰� �ְ�,
                        {
                            bool checkBool = nearNote.GetComponent<Mini10_End>().CheckEnd();    // �ص� üũ�� �ؼ�
                             
                            if (checkBool.Equals(true))  // true���, ���� ����������!!
                            {
                                Destroy(upTrans.GetChild(0).gameObject);   // ��� �ִ� ���� ����

                                isItem = false;
                                isDonut = false;

                                isFirst = false;
                                startCube = false;

                                nearObject = null;
                                nearNote = null;

                                isCameraMove = true;

                                AudioMng.ins.PlayEffect("SpeedUp");    // �������� �̵�
                                transform.position = mini10_Stage.Stage_Spawn();       // ���� ���������� �����Ѵ�.
                                
                                mini10_Camera.NextStageMove();         // ī�޶� �̵� �Լ� ����
                                anim.SetBool(attackId, false);       // ��� �ִ� �ִϸ��̼� �ߴ�

                                Right_Image.sprite = sprite_Array[3];   // ���� �̹���
                            }
                        }
                    }

                    PrevObj = hitInfo.transform.gameObject;     // ���� ��ü
                }
            }
            else            // ť�갡 �ƴ� ��(����)�� ���� ���
            {
                if (isCameraMove.Equals(false))
                {
                    nearObject = null;
                    nearNote = null;

                    if (endBool.Equals(false))
                    {
                        endBool = true;

                        MinusHp(false);
                    }
                    
                }
            }

            yield return delay;
        }
    }

    

	/////////////////////////    Ʈ���� ����


	void OnTriggerStay(Collider other)
	{
        if (other.gameObject.layer.Equals(7))       // ��Ȧ�� ��´ٸ�
        {
            blackHoleTrans = other.transform;    // ��Ȧ ������ ��Ҵٰ� �˸�
            speed = 2.8f;                        // �ӵ��� ����
        }
	}


	void OnTriggerExit(Collider other)
	{
        if (other.gameObject.layer.Equals(7))       // ��Ȧ���� �������´ٸ�..
        {
            blackHoleTrans = null;               // ��Ȧ �������� �������Դٰ� �˸�
            speed = 3.5f;                        // �ӵ��� ������� �ٲ۴�
        }
    }
}
