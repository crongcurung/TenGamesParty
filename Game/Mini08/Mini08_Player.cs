using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Mini08_Player : MonoBehaviour     // �÷��̾ ������
{
    [SerializeField] Joystic joystic;

    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // ������ ������ ��Ͻ�ų �� �ִ� ��ư

	[SerializeField] GameObject hammer;
    [SerializeField] GameObject gang;

    [SerializeField] Image timeImage;
    [SerializeField] TextMeshProUGUI[] Text_Array;     // 0 : ��, 1 : ��, 2 : ����, 3 : �μ��� ����, 4 : ���� ����

    int timeInt = 0;

    Animator anim;                       // �ִϸ����ʹ� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    [SerializeField] Image Right_Image;
    [SerializeField] Button Right_Button;
    [SerializeField] Sprite[] sprite_Array;       // 0 : �ƹ��͵� �� �������, 1 : ����, 2 : ��, 3 : ��, 4 : ��Ż

    [SerializeField] GameObject Item_Book;              // �߾ӿ��� ���۵��� ������ ������Ʈ
    [SerializeField] Image Item_Image;
    [SerializeField] Sprite[] spriteItem_Array;   // 0 : �� �̹���, 1 : ���� �̹���, 2 : ��� �̹���, 3 : ���� �̹���, 4 : ä�� �̹���, 5 : ��Ż �̹���, 6 : ???

    [SerializeField] Transform[] potal_Array;           // ��Ż 4���� �޴´�.
    [SerializeField] GameObject startCube;              // �� �����Ҷ� ��Ÿ���� ��Ÿ ������Ʈ..

    [SerializeField] GameObject[] cemetry_Array;     // ���� ����Ʈ
    List<int> cemetry_ListInt = new List<int>();        // ���� ��Ȱ��ȭ ��Ʈ ����Ʈ

    [SerializeField] float speed;        // �̵� �ӵ� ������ �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    Vector3 moveDir;                     // �̵� ������ �÷��̾ ���� ���ɼ� ����

    bool isPotal = false;   // �÷��̾ ��Ż�� ��ҳİ� ���� ����
    int potalInt = 0;       // �÷��̾ ��� ��Ż�� �� �� �ִ��� ���� ����

    int potal_OneTwoInt = 0;         // 1�� ��Ʈ �̵� Ƚ�� ����
    int potal_ThreeFourInt = 0;      // 2�� ��Ʈ �̵� Ƚ�� ����

    int stoneInt = 0;       // ���� ����
    int soilInt = 0;        // ���� ����

    bool isExorcism = false;         // ���� �� �������� ���� ����

    public Action followAction;      // ���� �Լ��� �޴� �׼� ����(�ѹ��� �Ѹ�����...)

    WaitForSeconds delay_03;         // �Ĺ��� ������

    bool moveLock = false;      // �÷��̾� �̵� ���ϰ� �ϴ� ����..

    int countInt = 0;           // Ȱ��ȭ�� ���� ������ �޴� ����
    int BrokenInt = 0;          // �μ��� ���� ������ �޴� ����
    int GhostInt = 0;           // Ȱ��ȭ�� ���� ������ �޴� ���� 

    int potal_LimitInt = 0;          // ��Ż �ִ� �̵� Ƚ��(������, �Ĺ���)
    int star_LimitInt = 0;           // �� ��Ÿ�� �����ð�(������, �Ĺ���)
    int up_Int = 0;                  // ��� ��ų �ߵ���, �ڿ� ��� ����(������, �Ĺ���)
    int repair_Skill = 0;            // ���� �ð� ��ų �ߵ���, ���� �ð� ����(������, �Ĺ���)
    int extra_Skill = 0;             // �߰� �ڿ� ��ų �ߵ���, �߰� �ڿ� ����(������, �Ĺ���)

    int repairTime = 0;   // ���� �ð�
    int extraInt = 0;   // ä�� ����

    int cemetryInt = 0;   // ���� Ȱ��ȭ ����

    bool isCemetry = false;     // ������ ��ҳ�?
    bool isStone = false;       // �� ���� ��ҳ�?
    bool isSoil = false;        // �� ���� ��ҳ�?

    Transform current_This;      // ���� �÷��̾�� �پ� �ִ� ��ȣ�ۿ� ������Ʈ

    WaitForSeconds delay_01;
    WaitForSeconds delay_02;

    int runId;
    int cemetryId;
    int stoneId;
    int soilId;

    string invoke_Text01;
    string invoke_Text02;
    string invoke_Text03;
    string invoke_Text04;
    string invoke_Text05;
    string invoke_Text06;

    string hor_Text;
    string ver_Text;

    string tag01;
    string tag02;
    string tag03;

    

    void Start()
    {
        anim = GetComponent<Animator>();

        runId = Animator.StringToHash("isRun");
        cemetryId = Animator.StringToHash("isCemetry");
        stoneId = Animator.StringToHash("isStone");
        soilId = Animator.StringToHash("isSoil");

        invoke_Text01 = "Invoke_Fixed";
        invoke_Text02 = "Invoke_Stone";
        invoke_Text03 = "Invoke_Soil";
        invoke_Text04 = "Invoke_Exorcism";
        invoke_Text05 = "Invoke_Repair";
        invoke_Text06 = "Invoke_Gather";

        hor_Text = "Horizontal";
        ver_Text = "Vertical";

        tag01 = "Honey";
        tag02 = "Spring";
        tag03 = "Bear";


        stoneInt = 20;             // �� ����
        soilInt = 20;              // �� ����
        Text_Array[0].text = stoneInt.ToString();  // ��, �� ���� �ؽ�Ʈ�� ǥ��
        Text_Array[1].text = soilInt.ToString();

        repairTime = 3;     // ���� �ð�

        for (int i = 0; i < 20; i++)     // ���� ��Ȱ��ȭ ����Ʈ ����
		{
            cemetry_ListInt.Add(i);      
        }

        delay_01 = new WaitForSeconds(60.0f);
        delay_02 = new WaitForSeconds(30.0f);
        delay_03 = new WaitForSeconds(240.0f);

        Start_Spawn();

        StartCoroutine(Item_Coroutine());        // ������ Ȱ��ȭ �ڷ�ƾ
        StartCoroutine(Cemetry_Spawn());         // ���� Ȱ��ȭ �ڷ�ƾ
        StartCoroutine(After_Coroutine());       // �Ĺ������� ���� �ڷ�ƾ ����

        timeImage.fillAmount = 0;

		repair_Skill = 3;      // ó�� �����ð�

		AudioMng.ins.Play_BG("Mini08_B");
    }

    

    void FixedUpdate()
    {
        if (!moveDir.x.Equals(0) || !moveDir.z.Equals(0))               // wasd �ƹ��ų��� �����δٸ�..
        {
            anim.SetBool(runId, true);         // �����̴� �ִϸ��̼� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z)), 0.5f);
            // �÷��̾� ȸ��

            transform.position += new Vector3(moveDir.x, 0, moveDir.z).normalized * Time.fixedDeltaTime * speed;      // �÷��̾� �̵�
        }
        else
        {
            anim.SetBool(runId, false);       // ���ߴ� �ִϸ��̼� ����
        }
    }

    bool scoreBool = false;

    void Update()
    {
        timeImage.fillAmount += Time.deltaTime / 60.0f;

        if (timeImage.fillAmount >= 0.5f && scoreBool.Equals(false))
        {
            scoreBool = true;
            timeInt++;
        }


        if (timeImage.fillAmount.Equals(1))
        {
            timeImage.fillAmount = 0;
            timeInt++;
            scoreBool = false;
        }

        if (moveLock.Equals(true))     // ��, ��, ���� ��ġ�� �� �� �����̴� ���¸�...
        {
            moveDir.x = 0;     // �̰� �ؾ� �Ѵ�.
            moveDir.z = 0;
            return;            // �Ʒ� ���� ���ϰ� �Ѵ�.
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) == true)       // ������ ��ư�� �����ٸ�...
        {
            Press_RightButton();
        }

        Move();
    }

    void Start_Spawn()
    {
		for (int i = 0; i < 3; i++)
		{
            int randInt = UnityEngine.Random.Range(0, cemetry_ListInt.Count);

            cemetry_Array[cemetry_ListInt[randInt]].SetActive(true);
            cemetry_ListInt.RemoveAt(randInt);                           // �ߺ� ����
            cemetryInt++;
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

    


    public void Press_RightButton()        // ������ ��ư�� �����ٸ�..
    {
        if (isSoil.Equals(true))           // ���� �뿡 ��� �ִٸ�...
        {
            Soil_Gather();
        }
        else if (isStone.Equals(true))     // ���� ���� ��� �ִٸ�..
        {
            Stone_Gather();
        }
        else if (isCemetry.Equals(true))   // ���� ������ ��� �ִٸ�...
        {
            Cemetry_Fixed();
        }
        else if (isPotal.Equals(true))     // ���� ��Ż�� ��� �ִٸ�...
        {
            transform.position = new Vector3(potal_Array[potalInt].position.x, transform.position.y, potal_Array[potalInt].position.z);    // ������ ��Ż�� ��

            AudioMng.ins.PlayEffect("SpeedUp");    // ��Ż ����

            if (potalInt.Equals(0) || potalInt.Equals(1))           // �÷��̾ ��Ż01�̶� 02�� �ִٸ�..
            {
                potal_OneTwoInt++;                                  // ��Ż ��Ʈ 1���� �̵�Ƚ���� �ø���.

                if (potal_OneTwoInt.Equals(potal_LimitInt))         // ��Ż ��Ʈ 1���� �̵� ���ѿ� �ɸ��ٸ�...
                {
                    potal_Array[0].gameObject.SetActive(false);     // ��Ż 01, 02�� ��Ȱ��ȭ�Ѵ�.
                    potal_Array[1].gameObject.SetActive(false);

                    isPotal = false;                                // ��Ż�� �������, �� ��Ҵٰ� ����� �Ѵ�.
                }
            }
            else                                                    // �÷��̾ ��Ż03�̶� 04�� �ִٸ�...
            {
                potal_ThreeFourInt++;                               // ��Ż ��Ʈ 2���� �̵�Ƚ���� �ø���.

                if (potal_ThreeFourInt.Equals(potal_LimitInt))      // ��Ż ��Ʈ 2���� �̵� ���ѿ� �ɸ��ٸ�...
                {
                    potal_Array[2].gameObject.SetActive(false);     // ��Ż 03, 04�� ��Ȱ��ȭ�Ѵ�.
                    potal_Array[3].gameObject.SetActive(false);

                    isPotal = false;                                // ��Ż�� �������, �� ��Ҵٰ� ����� �Ѵ�.
                }
            }
        }
    }

    

    public void CemetryText_Fuction(int upDownInt)          // ���� Ȱ��ȭ�� ������ ������ �˷��ִ� �Լ�
    {
        countInt = countInt + upDownInt;
        Text_Array[2].text = countInt.ToString();
    }

    public void BrokenText_Fuction(int upDownInt)           // ���� ���� �μ��� ������ ������ �˷��ִ� �Լ�
    {
        BrokenInt = BrokenInt + upDownInt;
        Text_Array[3].text = BrokenInt.ToString();  
    }

    public void GhostText_Fuction(int upDownInt)            // ���� ���� ������ ������ �˷��ִ� �Լ�
    {
        GhostInt = GhostInt + upDownInt;
        Text_Array[4].text = GhostInt.ToString();


        if (GhostInt.Equals(60))
        {

            End_Game();
        }
    }


    void End_Game()                   // ������ ������ �� �Լ�
    {
        Result_Panel.SetActive(true);
        Game_Panel.SetActive(true);

        AudioMng.ins.LoopEffect(false);    // ���ѷ��� ����


        Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + timeInt.ToString();

        if (Main.ins.nowPlayer.maxScore_List[7] >= timeInt)    // �ְ����� �� �����ٸ�...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[7].ToString();
        }
        else        // �ְ����� ���� ��� (�ű��)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + timeInt.ToString();

            Main.ins.nowPlayer.maxScore_List[7] = timeInt;
            Main.ins.SaveData();

			GPGS_Button.interactable = true;        // �ű���� ����� ���� ���� ��� ��ư�� Ȱ��ȭ�Ѵ�. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // ��������� ����.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;
    }


	public void Press_GPGS_08()   // �ű�� ��, ������ ���
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no8, timeInt);   // ������ ������ ���
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no8);          // �������带 ����.
	}


	void Cemetry_Fixed()      // ������ ��ġ�� �Լ�
    {
        int cemetryInt = current_This.GetComponent<Mini08_Cemetry>().Check_Cemetry();     // ���� ��ġ�� �������� ������ ���¸� �����´�.
        


        switch (cemetryInt)      // ������ ���¿� ����...
        {
            case 1:              // ������ ���°� �ν� 1�ܰ���..
                if (stoneInt > 0 && soilInt > 0)    // ���� ���� 1���� �̻��̶��..
                {
                    Right_Button.interactable = false;

                    stoneInt--;                          // �ϳ��� ����
                    soilInt--;

                    current_This.GetComponent<Mini08_Cemetry>().Player_Fixed();
                }
                else
                {
                    return;
                }
                break;
            case 2:              // ������ ���°� �ν� 2�ܰ���..
                if (stoneInt > 1 && soilInt > 1)
                {
                    Right_Button.interactable = false;

                    stoneInt -= 2;                       // �ΰ��� ����
                    soilInt -= 2;

                    current_This.GetComponent<Mini08_Cemetry>().Player_Fixed();
                }
                else
                {
                    return;
                }
                break;
            case 3:              // ������ ���°� �ν� 3�ܰ���..
                if (stoneInt > 2 && soilInt > 2)          // ������ ����
                {
                    Right_Button.interactable = false;

                    stoneInt -= 3;
                    soilInt -= 3;

                    current_This.GetComponent<Mini08_Cemetry>().Player_Fixed();
                }
                else
                {
                    return;
                }
                break;
            case 4:              // ������ ���°� �ν� 4�ܰ���..(���� �μ���)
                if (stoneInt > 4 && soilInt > 4)             // �ټ����� ����
                {
                    Right_Button.interactable = false;

                    stoneInt -= 5;
                    soilInt -= 5;

                    current_This.GetComponent<Mini08_Cemetry>().Player_Fixed();
                }
                else
                {
                    return;
                }
                break;
            default:              // ������ ���°� �ν� 0�ܰ���..
                return;
        }

        Text_Array[0].text = stoneInt.ToString();     // ������ ���ƴٸ� ���� ��Ḧ �ؽ�Ʈ�� ��Ÿ����.
        Text_Array[1].text = soilInt.ToString();

        moveLock = true;                                           // ������ ��ģ�ٸ� �� �����̰� �ؾ��Ѵ�.
        anim.SetBool(runId, false);                              // �޸��� �ִϸ��̼��� ����
        anim.SetBool(cemetryId, true);                           // ������ ��ġ�� �ִϸ��̼��� Ų��.
        hammer.SetActive(true);
        transform.LookAt(current_This);                            // �ش� ������ �ٶ󺸵��� �Ѵ�.


		//if (!repair_Skill.Equals(3))                                 // ���� �ð��� 3�ʰ� �ƴ϶��(���� �ð� ��ų�� �ߵ��Ǿ��ٴ� ��)
		//      {
		//          repairTime = repair_Skill;                             // ���� �ð� ��ų �ð��� �޾ƿ´�.(������, �Ĺ����� �־ �̷��� �Ѵ�...)
		//      }

		repairTime = repair_Skill;
		AudioMng.ins.PlayEffect("Fix");    // ���� ��ġ�� �Ҹ�
        Invoke(invoke_Text01, repairTime);                        // ���� �ϴ� ���� ������ ���� �κ�ũ�� �Ѵ�.

    }

    void Invoke_Fixed()               // ������ ������ �κ�ũ..
    {
        Right_Button.interactable = true;

        AudioMng.ins.LoopEffect(false);    // ���� �Ҹ� ���ѷ��� ����
        AudioMng.ins.StopEffect();

        moveLock = false;                   // �ٽ� �����̰� �Ѵ�.
        anim.SetBool(cemetryId, false);   // ���� ��ġ�� �ִϸ��̼��� ����.
        hammer.SetActive(false);
    }



    void Stone_Gather()       // ���� ĳ�� �Լ�
    {
        moveLock = true;      // �÷��̾ �� �����̰� �Ѵ�.
        Right_Button.interactable = false;

        anim.SetBool(runId, false);     // �޸��� �ִϸ��̼��� ����.
        anim.SetBool(stoneId, true);    // ���� ĳ�� �ִϸ��̼��� Ų��.
        gang.SetActive(true);
        transform.LookAt(current_This);   // �� ���� ����� ������ �Ѵ�.

        AudioMng.ins.LoopEffect(true);
        AudioMng.ins.PlayEffect("Pickaxe");    //  �� ĳ�� �Ҹ�

        followAction?.Invoke();           // Ȱ��ȭ�� ���ɵ��� �ڱ������� ������ �Ѵ�.
        Invoke(invoke_Text02, 3.0f);     // �� ĳ�� �� ������ �κ�ũ ����
    }

    void Invoke_Stone()       // ���� Ķ�� �κ�ũ
    {
        moveLock = false;       // �ٽ� �����̰� �Ѵ�.
        Right_Button.interactable = true;

        anim.SetBool(stoneId, false);   // �� ĳ�� �ִϸ��̼��� ����.
        gang.SetActive(false);

        AudioMng.ins.LoopEffect(false);

        Stone_Up();          // �� ���ھ �ø��� �Լ��� ����
    }

    void Stone_Up()                 // ���� ���ھ �ø��� �Լ�
    {
        if (!extraInt.Equals(0))      // �߰� �ڿ� ä�밡 0�� �ƴ϶��...(�ڿ� ä�� ��ų�� �ߵ��Ǿ��ٴ� ��)
        {
            extraInt = extra_Skill;   // �߰� �ڿ� ä�� ���ڸ� �޾ƿ´�.(������, �Ĺ����� �־ �̷��� �Ѵ�...)
        }

        stoneInt += 4 + extraInt;     // �� ���ڸ� �ø���.

        if (stoneInt > 99)            // �ø� �� ���ڰ� 99�� �Ѿ�ٸ�..
        {
            stoneInt = 99;            // 99�� �����.
        }

        Text_Array[0].text = stoneInt.ToString();      // �ٲ� �� �ؽ�Ʈ�� �ø���.
    }





    void Soil_Gather()        // ���� ĳ�� �Լ�
    {
        moveLock = true;      // �÷��̾ �� �����̰� �Ѵ�.
        Right_Button.interactable = false;

        anim.SetBool(runId, false);     // �޸��� �ִϸ��̼��� ����.
        anim.SetBool(soilId, true);     // ���� ĳ�� �ִϸ��̼��� Ų��.
        transform.LookAt(current_This);   // �� ���� ����� ������ �Ѵ�.

        AudioMng.ins.LoopEffect(true);
        AudioMng.ins.PlayEffect("Shovel");    //  �� ĳ�� �Ҹ�

        followAction?.Invoke();           // Ȱ��ȭ�� ���ɵ��� �ڱ������� ������ �Ѵ�.
        Invoke(invoke_Text03, 3.0f);      // �� ĳ�� �� ������ �κ�ũ ����
    }

    void Invoke_Soil()       // ���� Ķ�� �κ�ũ
    {
        moveLock = false;       // �ٽ� �����̰� �Ѵ�.
        Right_Button.interactable = true;

        anim.SetBool(soilId, false);   // �� ĳ�� �ִϸ��̼��� ����.

        AudioMng.ins.LoopEffect(false);

        Soil_Up();          // �� ���ھ �ø��� �Լ��� ����
    }

    void Soil_Up()                 // ���� ���ھ �ø��� �Լ�
    {
        if (!extraInt.Equals(0))      // �߰� �ڿ� ä�밡 0�� �ƴ϶��...(�ڿ� ä�� ��ų�� �ߵ��Ǿ��ٴ� ��)
        {
            extraInt = extra_Skill;   // �߰� �ڿ� ä�� ���ڸ� �޾ƿ´�.(������, �Ĺ����� �־ �̷��� �Ѵ�...)
        }

        soilInt += 4 + extraInt;     // �� ���ڸ� �ø���.

        if (soilInt > 99)            // �ø� �� ���ڰ� 99�� �Ѿ�ٸ�..
        {
            soilInt = 99;            // 99�� �����.
        }

        Text_Array[1].text = soilInt.ToString();      // �ٲ� �� �ؽ�Ʈ�� �ø���.
    }






    void Potal_Check(GameObject potal)   // ���� �÷��̾ ��� ��Ż�� �ְ�, ���� �� �� �ִ��� üũ���ִ� �Լ�
    {
        if (potal.CompareTag(tag01))          // ��Ż 01�� ������..
        {
            potalInt = 1;      // 2�� ��Ż�� �� �� �ִٰ� �˸�
        }
        else if (potal.CompareTag(tag02))      // ��Ż 02�� ������..
        {
            potalInt = 0;      // 1�� ��Ż�� �� �� �ִٰ� �˸�
        }
        else if (potal.CompareTag(tag03))      // ��Ż 03�� ������..
        {
            potalInt = 3;      // 4�� ��Ż�� �� �� �ִٰ� �˸�
        }
        else                                 // ��Ż 04�� ������..
        {
            potalInt = 2;      // 3�� ��Ż�� �� �� �ִٰ� �˸�
        }
    }



    // 1�̸�      �� : �����ð�(10��) ���� �ͽſ��� �ǰݵ� ���, �ͽ��� �Ҹ���.
    // 2�̸�      ���� : �ʵ� �� ������ ���� 5���� ���ش�.
    // 3�̸�      ��� : �� �� ���� ��� 30���� ȹ���Ѵ�.
    // 4�̸�      ���� : �����ð�(30��)���� ���� ���� �ð��� 1�ʷ� �����Ѵ�.
    // 5�̸�      ä�� : �����ð�(30��) ���� �� �� �� ȹ����� 2���� �� ȹ���Ѵ�.
    // 6�̸�      ��Ż : 10ȸ ����� �� �ִ� ��Ż�� �����Ѵ�.

    int rantInt;

    void Item_Check()      // �÷��̾ �������� �Ծ��� �� ��ų�� �������� �ߵ���Ű�� �Լ�
    {
        rantInt = UnityEngine.Random.Range(0, 6);      // 6���� ��ų
        //rantInt = 3;

        switch (rantInt)          // �������� ���� ���ڿ� ����..
        {
            case 0:
                AudioMng.ins.PlayEffect("Score_Up");    // �� ����
                Exorcism_01();    // ��

                Item_Image.sprite = spriteItem_Array[0];       // �� �̹���
                break;
            case 1:
                AudioMng.ins.PlayEffect("Bomb");    // ���� ����
                Delete_02();      // ����

                Item_Image.sprite = spriteItem_Array[1];       // ���� �̹���
                break;
            case 2:
                AudioMng.ins.PlayEffect("Cloud");    // ��� ����
                Rich_03();        // ���

                Item_Image.sprite = spriteItem_Array[2];       // ��� �̹���
                break;
            case 3:
                AudioMng.ins.PlayEffect("Fix");    // ���� ����
                Repair_04();      // ����

                Item_Image.sprite = spriteItem_Array[3];       // ���� �̹���
                break;
            case 4:
                AudioMng.ins.PlayEffect("Click02");    // ä�� ����
                Gather_05();      // ä��

                Item_Image.sprite = spriteItem_Array[4];       // ä�� �̹���
                break;
            default:
                AudioMng.ins.PlayEffect("SpeedUp");    // ��Ż ����
                Potal_06();       // ��Ż

                Item_Image.sprite = spriteItem_Array[5];       // ��Ż �̹���
                break;
        }
    }


    void Exorcism_01()    // ��
    {
        isExorcism = true;                     // ��(��Ÿ)�� �ߵ��ǰ� �ִٰ� �˸���.
        startCube.SetActive(true);             // �÷��̾� ��Ÿ ������Ʈ�� Ų��.

        CancelInvoke(invoke_Text04);       // �� �κ�ũ�� ����ǰ� �ִٰ� �ߴܽ�Ų��.
        Invoke(invoke_Text04, star_LimitInt);    // �� �κ�ũ�� �����Ѵ�.
    }

    void Invoke_Exorcism()    // �� �κ�ũ
    {
        startCube.SetActive(false);       // �ð��� �Ǹ� ��Ÿ ������Ʈ�� ����.

        if (rantInt.Equals(0))
        {
            Item_Image.sprite = spriteItem_Array[6];
        }

        isExorcism = false;               // �𸶰� �����ٰ� �˸���.
    }


    void Delete_02()      // ����
    {
        int randInt = 20 - cemetry_ListInt.Count;    // 20���� Ȱ��ȭ�� ���� ���� ����Ʈ�� ������ �� ���ڸ� �ѱ��.
        int tempInt = Mathf.Clamp(randInt, 0, 5);    // ���� ���ڸ� �ּ� 0���� �ִ� 5���� �ȿ��ٰ� ���д�.
        randInt--;                                   // ����Ʈ�� 0���Ͷ� ���� 1�� ���δ�.

        for (int i = 0; i < tempInt; i++)    // ���� ���� ��ŭ ������.
		{
            bool isEnd = false;              // ���� �ݺ� ������ ����

			while (isEnd.Equals(false))      // true�� �ɶ����� ��� ������.
			{
                if(cemetry_Array[randInt].activeSelf.Equals(true)) // �������� ���� ������ Ȱ��ȭ���...
                {
                    isEnd = true;     // ������ ������..
                }
                else
                {
                    randInt = UnityEngine.Random.Range(0, 20);     // �ٽ� ������ �̴´�.
                }
			}
            cemetry_Array[randInt].SetActive(false);      // Ȱ��ȭ�� ������ ��Ȱ��ȭ �Ѵ�.
            randInt = UnityEngine.Random.Range(0, 20);   // ������ �̴´�.
        }
	}


    void Rich_03()        // ���
    {
        stoneInt += up_Int;                         // 20���� �ø�
        soilInt += up_Int;

        if (stoneInt > 99)      // 99�� �����ٸ�..
        {
            stoneInt = 99;      // 99�� ������Ų��.
        }

        if (soilInt > 99)
        {
            soilInt = 99;
        }

        Text_Array[0].text = stoneInt.ToString();   // �ٲ� ��Ḧ �ؽ�Ʈ�� �ø���.
        Text_Array[1].text = soilInt.ToString();
    }

    

    void Repair_04()      // ���� �ð�
    {
        CancelInvoke(invoke_Text05);     // ���� �ð� �κ�ũ�� �ߴܽ�Ų��.

        if (potal_LimitInt.Equals(7))
        {
			repair_Skill = 2;       // ��, �Ĺ��� �����ð��� ������ �´�.
        }
        else
        {
			repair_Skill = 1;

		}

        Invoke(invoke_Text05, 30.0f);  // 30�� ���� ���� �ð��� ���δ�.
    }

    void Invoke_Repair()    // ���� �ð� �κ�ũ
    {
        repair_Skill = 3;

        if (rantInt.Equals(3))
        {
            Item_Image.sprite = spriteItem_Array[6];       // ���� �̹���
        }
    }

    

    void Gather_05()      // ä��
    {
        CancelInvoke(invoke_Text06);

        extraInt = extra_Skill;      // ��, �Ĺ��� �߰� �ڿ� ���ڸ� ������ �´�.

        Invoke(invoke_Text06, 30);  // 30�� ���� �߰� ä���� �Ѵ�.
    }

    void Invoke_Gather()   // ä�� �κ�ũ
    {
        extraInt = 0;      // 30�� �Ŀ�, �߰� ä���� ���ش�.

        if (rantInt.Equals(4))
        {
            Item_Image.sprite = spriteItem_Array[6];       // ���� �̹���
        }
    }


    void Potal_06()       // ��Ż
    {
        int randInt = UnityEngine.Random.Range(0, 2);          // 1, 2�� ��Ʈ �� ��� ��Ʈ�� ���� ���ϴ� ���� ����

        if (randInt.Equals(0))                     // 1�� ��Ʈ���...
        {
            potal_OneTwoInt = 0;                   // (1�� ��Ʈ)��Ż Ÿ�� ī��Ʈ�� 0���� ����(�ʱ�ȭ)

            potal_Array[0].gameObject.SetActive(true);     // ��Ż 1�� ��Ʈ�� Ų��.
            potal_Array[1].gameObject.SetActive(true);
        }
        else                                        // 2�� ��Ʈ���...
        {
            potal_ThreeFourInt = 0;                  // (2�� ��Ʈ) ��Ż Ÿ�� ī��Ʈ�� 0���� ����(�ʱ�ȭ)

            potal_Array[2].gameObject.SetActive(true);     // ��Ż 2�� ��Ʈ�� Ų��.
            potal_Array[3].gameObject.SetActive(true);
        }
    }


    ////////////////////// Ʈ���� ����.........

    

    void OnTriggerEnter(Collider other)        // Ʈ���ſ� ��Ҵٸ�...
    {
        switch (other.gameObject.layer)      // ���� ���̾..
        {
            case 4:            // ��� å�� ����� ���...  Water
                other.gameObject.SetActive(false);      // ������ å�� ��Ȱ��ȭ�Ѵ�.
                Item_Check();
                break;
            case 3:            // �� ���� ����� ���...   wall
                isSoil = true;         // �� ���� ��Ҵٰ� �˸���.
                current_This = other.transform;   // �뺮�� �ѱ��.

                Right_Image.sprite = sprite_Array[3];     // �뿡 ����� ���...
                break;
            case 1:            // �� ���� ����� ���...    Transparentfx
                isStone = true;        // �� ���� ��Ҵٰ� �˸���.
                current_This = other.transform;   // �� ���� �ѱ��.

                Right_Image.sprite = sprite_Array[2];     // ���� ����� ���...
                break;
            case 7:         // ���Ϳ� ������...    monster
                Touch_Monster(other);
                break;
            case 6:           // �ƹ� ��Ż�̳� ������..    player
                isPotal = true;        // ��Ż�� ��Ҵٰ� �˸���.
                Potal_Check(other.gameObject);

                Right_Image.sprite = sprite_Array[4];     // ��Ż�� ����� ���...
                break;
            case 8:          // ������ ����� ���..      object
                isCemetry = true;      // ������ ��Ҵٰ� �˸���.
                current_This = other.transform;   // ������ �ѱ��.

                Right_Image.sprite = sprite_Array[1];     // ������ ����� ���...
                break;
        }
    }


	void OnTriggerExit(Collider other)        // Ʈ���ſ��� ���� ���Դٸ�...
    {
        switch (other.gameObject.layer)    // ���� ���� ���̾�..
        {
            case 3:        // �� ������ �������Դٸ�...      wall
                isSoil = false;     // �� ������ �������Դٰ� �˸���.
                current_This = null;

                Right_Image.sprite = sprite_Array[0];     // �뿡�� ���� ���Դٸ�...
                break;
            case 1:        // �� ������ �������Դٸ�...    Transparentfx
                isStone = false;    // �� ������ �������Դٰ� �˸���.
                current_This = null;

                Right_Image.sprite = sprite_Array[0];     // ������ ���� ���Դٸ�...
                break;
            case 6:       // ��Ż���� �������Դٸ�...   player
                isPotal = false;    // ��Ż���� �������Դٰ� �˸���.

                Right_Image.sprite = sprite_Array[0];     // ��Ż���� ���� ���Դٸ�...
                break;
            case 8:       // �������� �������Դٸ�...    object
                isCemetry = false;  // �������� �������Դٰ� �˸���.
                current_This = null;

                Right_Image.sprite = sprite_Array[0];     // ��Ż���� ���� ���Դٸ�...
                break;
        }
    }

    void Touch_Monster(Collider other)    // ���Ϳ� ��Ҵٸ�..
    {
        if (isExorcism.Equals(true))      // �÷��̾ �� ���¶��..
        {
            AudioMng.ins.LoopEffect(false);    // ��, �� �Ҹ� ���ѷ��� ����
            AudioMng.ins.PlayEffect("Score_Up");    // �� ����

            other.gameObject.SetActive(false);   // ���� ���͸� ��Ȱ��ȭ ��Ų��.
        }
        else                              // �� ���°� �ƴ϶��...
        {
            End_Game();
        }
    }


    

    public void Cemetry_Minus(int listInt)      // ���� ��ũ��Ʈ���� �ڽ��� ��Ȱ��ȭ �Ǿ��ٰ� �Ѵٸ�..          ///////////////////////////////////////////////////////////////////////////////////
    {
        cemetryInt--;                           // ���� ���ڸ� ���δ�.

        cemetry_ListInt.Add(listInt);           // ���� ���� ����Ʈ�� �ش� ������ ��ȣ�� ��´�.
    }

    ////////////////////////   �ڷ�ƾ ����...

    IEnumerator Cemetry_Spawn()           // ���� Ȱ��ȭ �ڷ�ƾ
    {
        while (true)
        {
            yield return delay_02;   // 30�� ���� ���

            if (!cemetryInt.Equals(20))       // ���� ���� ����Ʈ�� 20�� �ƴ϶��
            {
                int randInt = UnityEngine.Random.Range(0, cemetry_ListInt.Count);
                cemetry_Array[cemetry_ListInt[randInt]].SetActive(true);      // �������� ������ Ȱ��ȭ
                cemetry_ListInt.RemoveAt(randInt);                           // �ߺ� ����
                cemetryInt++;
            }
        }
    }


    IEnumerator Item_Coroutine()           // ������ Ȱ��ȭ �ڷ�ƾ
    {
        while (true)
        {
            yield return delay_01;         // 1�� ���� ���
            AudioMng.ins.LoopEffect(false);
            AudioMng.ins.PlayEffect("Enter");    // ������ �Դ� �Ҹ�
            Item_Book.SetActive(true);     // ������ å Ȱ��ȭ
        }
    }


    IEnumerator After_Coroutine()        // �Ĺ������� ���� �ڷ�ƾ
    {
        potal_LimitInt = 7;     // �������� ��Ż ���� 7����
        star_LimitInt = 15;     // �������� �� ���ӽð� 15�ʷ�
        up_Int = 20;            // �������� ��� ��� 20����
        extra_Skill = 2;        // �������� �߰� �ڿ� 2����

        yield return delay_03;       // 240�� ��...(4�� ��...)

        potal_LimitInt = 12;    // �Ĺ����� ��Ż ���� 12����
        star_LimitInt = 25;     // �Ĺ����� �� ���ӽð� 25�ʷ�
        up_Int = 30;            // �Ĺ����� ��� ��� 30����
        extra_Skill = 3;        // �Ĺ����� �߰� �ڿ� 3����
    }
}
