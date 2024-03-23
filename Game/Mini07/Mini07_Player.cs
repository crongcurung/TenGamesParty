using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini07_Player : MonoBehaviour
{
    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // ������ ������ ��Ͻ�ų �� �ִ� ��ư

	[SerializeField] Button left_Button;
    [SerializeField] Button rigit_Button;

    [SerializeField] Mini07_Spawn mini07_Spawn;
    [SerializeField] Transform rail_SpawnPos;

    Animator anim;                       // �ִϸ����ʹ� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����
    [SerializeField] float speed;        // �̵� �ӵ� ������ �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    [SerializeField] GameObject Straight_Player;

    [SerializeField] TextMeshProUGUI speedText;            // ���ǵ带 ��Ÿ���� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI coinText;

    [SerializeField] ParticleSystem particle_Left;        // ���� ��ƼŬ
    [SerializeField] ParticleSystem particle_Right;       // ������ ��ƼŬ

    ParticleSystem.MainModule tempParticle_Left;    // ���� ��ƼŬ�� �ӵ��� �޾��� ����
    ParticleSystem.MainModule tempParticle_Right;   // ������ ��ƼŬ�� �ӵ��� �޾��� ����

    int palyer_Pos = 0;             // ���� �÷��̾�� ��� ������ ��ġ�� �ֳĴ� ����
    int coinInt;
    bool isEnd = false;

    Vector3 railOffSet;


    WaitForSeconds delay_01;

    int attackId;

    void Start()
    {
        anim = transform.GetChild(1).GetComponent<Animator>();

        attackId = Animator.StringToHash("isAttack");

        anim.SetBool(attackId, true);

        railOffSet = new Vector3(1.5f, 0, 0);

        speedText.text = (speed * 2).ToString("N0");

        tempParticle_Left = particle_Left.main;     // ������ ��ƼŬ�� ��Ƶ�(��ƼŬ�� �̷��� �ؾ���..)
        tempParticle_Right = particle_Right.main;

        tempParticle_Left.simulationSpeed = 2.0f;     // ó���� �� �ӵ���
        tempParticle_Right.simulationSpeed = 2.0f;

        delay_01 = new WaitForSeconds(0.2f);
        StartCoroutine(Rail_StratCoroutine());

        AudioMng.ins.Play_BG("Mini07_B");
    }

    


    void Update()
    {
        if (isEnd.Equals(true))
        {
            return;
        }

        transform.position += Vector3.forward * Time.deltaTime * speed;        // ������ ��� ������ ����.

        if (Input.GetKeyDown(KeyCode.D) == true)       // ������ ��ư�� �����ٸ�...
        {
            Press_RightButton();
        }
        else if (Input.GetKeyDown(KeyCode.A) == true)       // ���� ��ư�� �����ٸ�...
        {
            Press_LeftButton();
        }
    }




    public void Press_RightButton()
    {
        if (palyer_Pos <= 2)       // �÷��̾ 2 ���� ���Ͽ� �ִٸ�..
        {
            palyer_Pos++;          // ���� ��ȣ�� �߰���

            if (palyer_Pos >= 3)   // �÷��̾ 3 �̻� ���Ͽ� �ִٸ�..
            {
                palyer_Pos = 2;    // 2�� ����
            }
            else             // �÷��̾ 3 �̸� ���Ͽ� �ִٸ�..
            {
                GameObject t_object = mini07_Spawn.GetQueue_Right();          // ������ ������ ����
                t_object.transform.position = rail_SpawnPos.position + railOffSet;  // ������ ���� ��ġ ����

                transform.position = new Vector3(palyer_Pos * 3, 0, transform.position.z);     // �÷��̾ ������ ���Ϸ� �̵�
            }
        }
    }

    public void Press_LeftButton()
    {
        if (palyer_Pos >= -2)         // �÷��̾ -2 �̻� ���Ͽ� �ִٸ�...
        {
            palyer_Pos--;             // ���� ��ȣ�� ��

            if (palyer_Pos <= -3)     // �÷��̾ -3 ���� ���Ͽ� �ִٸ�...
            {
                palyer_Pos = -2;      // -2�� ����
            }
            else               // �÷��̾ -3 �ʰ� ���Ͽ� �ִٸ�..
            {
                GameObject t_object = mini07_Spawn.GetQueue_Left();             // ���� ������ ����
                t_object.transform.position = rail_SpawnPos.position - railOffSet;   // ���� ���� ��ġ ���� 

                transform.position = new Vector3(palyer_Pos * 3, 0, transform.position.z);  // �÷��̾ ���� ���Ϸ� �̵�
            }
        }
    }



    bool isParticle = false;

    public void SpeedUp()         // ���ǵ� �� �Լ�
    {
        speed += 0.5f;            // �Լ��� ����ɶ����� ���ǵ带 �ø���.
        anim.speed += 0.01f;      // ���� ������ �ִϸ��̼� �ӵ��� �ø���.
        

        if (isParticle.Equals(false) && speed > 40)
        {
            isParticle = true;
            particle_Left.gameObject.SetActive(true);
            particle_Right.gameObject.SetActive(true);
        }

        speedText.text = (speed * 2).ToString("N0");
    }

    

    /////////////////////////////// �ڷ�ƾ ����
    IEnumerator Rail_StratCoroutine()   // ���� ������ ��� �ڷ�ƾ
    {
        while (isEnd.Equals(false))
        {
            GameObject t_object = mini07_Spawn.GetQueue_Straight();     // ���� ������ �������� �Լ�
            t_object.transform.position = rail_SpawnPos.position;
            t_object.transform.rotation = rail_SpawnPos.rotation;

            yield return delay_01;
        }
    }


    IEnumerator End_Coroutine()
    {
        yield return new WaitForSeconds(5.0f);

        Result_Panel.SetActive(true);
        Game_Panel.SetActive(true);

        Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + coinInt.ToString();

        if (Main.ins.nowPlayer.maxScore_List[6] >= coinInt)    // �ְ����� �� �����ٸ�...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[6].ToString();
        }
        else        // �ְ����� ���� ��� (�ű��)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + coinInt.ToString();

            Main.ins.nowPlayer.maxScore_List[6] = coinInt;
            Main.ins.SaveData();

			GPGS_Button.interactable = true;        // �ű���� ����� ���� ���� ��� ��ư�� Ȱ��ȭ�Ѵ�. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // ��������� ����.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;
    }


	public void Press_GPGS_07()   // �ű�� ��, ������ ���
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no7, coinInt);   // ������ ������ ���
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no7);          // �������带 ����.
	}


	///////////////////////////////  Ʈ���� ����..



	void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(8))    // ��ֹ� ������Ʈ�� ������..
        {
            isEnd = true;     // ���� ����� �ٲ�
            Straight_Player.SetActive(false);

            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;       // �÷��̾��� ������ٵ��� ������ ���� Ǭ��.

            transform.GetComponent<Rigidbody>().velocity = Vector3.up * 15.0f;     // �浹 ǥ��

            left_Button.interactable = false;
            rigit_Button.interactable = false;

            AudioMng.ins.PlayEffect("Bomb");    // �浹�ϴ� �Ҹ�
            anim.SetBool(attackId, false);               // ������ ��� �ִϸ��̼��� ����.

            StartCoroutine(End_Coroutine());
        }
    }


    void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.layer.Equals(4))
        {
            other.gameObject.SetActive(false);

            AudioMng.ins.PlayEffect("Score_Up");    // ���� �Դ� �Ҹ�
            coinInt++;
            coinText.text = coinInt.ToString("N0");
        }
	}
}
