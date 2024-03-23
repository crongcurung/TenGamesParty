using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Mini01_Player : MonoBehaviour            // �÷��̾� �θ�Ŭ������ ���
{
	[SerializeField] Joystic joystic;

	[SerializeField] GameObject Result_Panel;
	[SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // ������ ������ ��Ͻ�ų �� �ִ� ��ư

	public bool isShip = false;
	[SerializeField] GameObject[] Hp_Array;

	[SerializeField] float dig_x;      // ���� x ���� ��
	[SerializeField] float dig_z;      // ���� y ���� ��

	[SerializeField] Mini01_Donut mini01_Donut;

	bool isShield = false;             // ü���� ��ų�, ���� ���¶�� true..
	bool isHit = false;                // �̹� �°� 4�� �̳��̸� true..

	int shovel_Int;                    // �÷��̾ ��� ������ �� ���� ��Ʈ ��
	Coroutine muzukCor;                // ���� �ð� �ڷ�ƾ�� �޴� ����
	Coroutine speedCor;                // ���ǵ� �ð� �ڷ�ƾ�� �޴� ����

	GameObject starCube;               // ���� ������ ���� �� �÷��̾� ������ ���� ��Ÿ ť��

	Vector3 digRot;

	[SerializeField] Mini01_Spawn mini01_Spawn;
	[SerializeField] TextMeshProUGUI right_Text;
	[SerializeField] TextMeshProUGUI score_Text;

	Vector3 moveDir;                     // �̵� ������ �÷��̾ ���� ���ɼ� ����
	Vector3 rotBan = Vector3.zero;                      // ������ٵ�� �̵� ��, ĳ���Ͱ� ��� ȸ���ϴ� ������ ���� ���� ����

	int playerHp;                        // �÷��̾��� ü�� ����
	int playerScore;                        // �÷��̾� ���ھ�

	Rigidbody rigid;                 // ������ٵ�� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����
	Animator anim;                       // �ִϸ����ʹ� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

	[SerializeField] float speed;        // �̵� �ӵ� ������ �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

	WaitForSeconds delay_01;
	WaitForSeconds delay_02;
	WaitForSeconds delay_03;

	string hor_Text;
	string ver_Text;

	int run_Id;
	int attack_Id;

	string tag_Text01;
	string tag_Text02;
	string tag_Text03;
	string tag_Text04;

	string invoke_Text;

	Renderer render;

	void Start()
	{
		playerHp = 3;         // �÷��̾��� ü���� 3���� �����Ѵ�.(�� �̻� �� ������.)
		playerScore = 0;      // ��Ʈ ������ ������ ���ھ �ö󰣴�.(���� ����)

		hor_Text = "Horizontal";
		ver_Text = "Vertical";

		run_Id = Animator.StringToHash("isRun");
		attack_Id = Animator.StringToHash("isAttack");

		tag_Text01 = "Spring";          // ���� ����
		tag_Text02 = "Bear";           // �� ����
		tag_Text03 = "Cushion";        // ���ǵ� ����
		tag_Text04 = "Coin";           // ��Ʈ ����

		invoke_Text = "Invoke_Shove";

		right_Text.text = "3";           // ���� ó���� 3���̶� 3�̶� ���´�.

		rigid = GetComponent<Rigidbody>();              // �÷��̾��� ������ �ٵ� ������ �´�.
		anim = GetComponent<Animator>();                // �÷��̾��� �ִϸ��̼��� ������ �´�.
		starCube = transform.GetChild(2).gameObject;    // ��Ÿ ť�긦 ������ �´�.

		shovel_Int = 3;            // ó������ ���� 3�� ��
		digRot = new Vector3(-90, 0, 0);     // ���� ȸ�� ���� �� ����

		delay_01 = new WaitForSeconds(10.0f);   // ����, ���ǵ� �ڷ�ƾ �ð� ����
		delay_02 = new WaitForSeconds(4.0f);    // �¾��� �� ��� ���� �ð� ����
		delay_03 = new WaitForSeconds(0.25f);

		render = transform.GetChild(1).GetComponent<Renderer>();

		StartCoroutine(rayCoroutine());     // ���̴� ó������ �����Ѵ�.

		AudioMng.ins.Play_BG("Mini01_B");
	}

	void Move()                 // �θ� Ŭ�������� �̵� �Լ��� �ֱ� ������ ������ �Ѵ�!
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

	void FixedUpdate()
    {
        rigid.velocity = moveDir * speed;          // ���ν�Ƽ�� �̵�

		if (!moveDir.x.Equals(0) || !moveDir.z.Equals(0))               // wasd �ƹ��ų��� �����δٸ�..
        {
            anim.SetBool(run_Id, true);         // �����̴� �ִϸ��̼� ����
			isShip = false;
			
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z)), 0.2f);
            // �÷��̾��� ȸ�� ��
        }
        else
        {
            anim.SetBool(run_Id, false);       // ���ߴ� �ִϸ��̼� ����
            rigid.angularVelocity = rotBan;          // �÷��̾ ���𰡿� �浹�� ��� ȸ���ϴ� ������ �־ ���ƹ���
			isShip = true;                    
		}
    }

    void Update()
    {
        Move();

		if (Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			Press_RightButton();     // ������ ��ư�� �����ٸ�..
		}
    }


	IEnumerator Render_Hp()
	{
		int num = 16;
		while (!num.Equals(0))
		{

			render.enabled = !render.enabled;

			yield return delay_03;
			num--;
		}
	}


	void MinusHp()  // ���������� �� �Լ��� ����Ǿ���� ü���� ��´�.
	{
		playerHp--;                  // �÷��̾��� ü���� ��� �Ѵ�.

		Hp_Array[playerHp].SetActive(false);

		StartCoroutine(Render_Hp());

		if (playerHp.Equals(0))                 // ü���� �� ������...
		{
			Result_Panel.SetActive(true);       // ��� â�� ����.
			Game_Panel.SetActive(true);         // ������ â�� ����.

			Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + playerScore.ToString();

			if (Main.ins.nowPlayer.maxScore_List[0] >= playerScore)    // �ְ����� �� �����ٸ�...
			{
				Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[0].ToString();
			}
			else        // �ְ����� ���� ��� (�ű��)
			{
				Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + playerScore.ToString();

				Main.ins.nowPlayer.maxScore_List[0] = playerScore;
				Main.ins.SaveData();

				GPGS_Button.interactable = true;        // �ű���� ����� ���� ���� ��� ��ư�� Ȱ��ȭ�Ѵ�. /////////////////////////////////////////////////////////////////////////////

			}

			AudioMng.ins.Pause_BG();              // ��������� ����.
			AudioMng.ins.PlayEffect("Fail02");

			Time.timeScale = 0;                 // ������ ������...

		}
		else if (isHit.Equals(false))         // �̹� ���� ���°� �ƴ϶��...
		{
			AudioMng.ins.PlayEffect("Meow");     // ü���� ������ �Ҹ���
			StartCoroutine(HitCoroutine());
			return;
		}
	}

	public void Press_GPGS_01()   // �ű�� ��, ������ ���
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no1, playerScore);   // ������ ������ ���
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no1);          // �������带 ����.
	}

	public void PlayerHp()          // public���� ���Ƽ� �ٸ� �������� �� �Լ��� ���� �����ϵ��� �Ѵ�.
	{
		if (isShield.Equals(false) && isHit.Equals(false))   // ��Ÿ ť�갡 �۵��ϰ� ���� ������..
		{
			MinusHp();                // ü���� ��´�.
		}
	}


	public void Press_RightButton()          // ������ ��ư Ȥ�� ���͸� �����ٸ�..
	{
		if (shovel_Int > 0 && transform.position.y < 1.93f)     // �� ��Ʈ�� 0���� ũ��, ���� �÷��̾��� ���̰� ���� ���̺��� ���ٸ�...
		{
			shovel_Int--;       // �� ���ڸ� ���δ�.
			right_Text.text = shovel_Int.ToString();           // �� ��ȯ �����...

			anim.SetBool(attack_Id, true);
			Invoke(invoke_Text, 0.5f);

			int ceil_x = Vector3Int.CeilToInt(transform.position).x;        // �÷��̾��� ���� ��ġ�� ���������� ��ȯ�Ѵ�.
			int ceil_z = Vector3Int.CeilToInt(transform.position).z;

			GameObject dig_Prefab = mini01_Spawn.GetQueue_Digged();      // ���� ������Ʈ�� �����´�.
			if (dig_Prefab == null)    // ���� ���� ������Ʈ�� �� �����´ٸ�...(���� ������Ʈ�� ���� ������ ����)
			{
				return;      // ���� ����
			}

			AudioMng.ins.PlayEffect("Shovel");       // �� �Ĵ� �Ҹ�
			dig_Prefab.transform.position = new Vector3(ceil_x + dig_x, transform.position.y, ceil_z + dig_z);   // ���� ������Ʈ�� ��ġ�� ȸ�� ����
			dig_Prefab.transform.rotation = Quaternion.Euler(digRot);
		}
	}

	void Invoke_Shove()
	{
		anim.SetBool(attack_Id, false);
	}

	void Muzuk_Potion()             // ���� ���ǿ� �÷��̾ ������...
	{
		if (isShield.Equals(true))        // ���� ���� �ð� �ڷ�ƾ�� �������̶��..
		{
			StopCoroutine(muzukCor);      // ����ǰ� �ִ� ���� �ð� �ڷ�ƾ �ߴ�
		}
		muzukCor = StartCoroutine(MuzukCoroutine());   // ���� �ð� �ڷ�ƾ�� �����Ѵ�.
	}

	void Shovel_Potion()            // �� ���ǿ� �÷��̾ ������...
	{
		if (shovel_Int.Equals(5))     // ���� �÷��̾ �� ��Ʈ�� 5���� ��� �ִٸ� �� �̻� �߰����� �ʴ´�.
		{
			return;
		}

		shovel_Int++;               // �� ��Ʈ�� ������Ų��.
		right_Text.text = shovel_Int.ToString();     // ������ ��ư �ؽ�Ʈ�� ��������� �˸���.
	}

	void Speed_Potion()             // ���ǵ� ���ǿ� �÷��̾ ������...
	{
		if (speed > 6)              // �÷��̾��� ���ǵ尡 �̹� �ö� ���¶��..
		{
			StopCoroutine(speedCor);    // ���� ���� ���ǵ� �ð� �ڷ�ƾ�� �ߴܽ�Ų��.
		}
		speedCor = StartCoroutine(SpeedCoroutine());      // ���ǵ� �ð� �ڷ�ƾ�� �����Ų��.
	}

	void Donut_Score()          // ��Ʈ ���ӿ� ��´ٸ�..
	{
		playerScore++;          // ���ھ �ø���.
		score_Text.text = playerScore.ToString();      // ���ھ� ��������� �˸���.
	}





	////////////////////////////////////////////////////////////
	// �ڷ�ƾ ����

	IEnumerator HitCoroutine()
	{
		isHit = true;
		yield return delay_02;
		isHit = false;
	}

	IEnumerator MuzukCoroutine()                    // ���� �ð� �ڷ�ƾ
	{
		isShield = true;                            // ���� ���¶�� �˸���.
		starCube.SetActive(true);                   // ��Ÿť�긦 Ų��.
		yield return delay_01;                  // �ð��� �����ٸ�...
		starCube.SetActive(false);                  // ��Ÿť�긦 ����.
		isShield = false;                           // ���� ���°� �����ٰ� �˸���.
	}

	IEnumerator SpeedCoroutine()                     // ���ǵ� �ð� �ڷ�ƾ
	{
		speed = 13;                                  // �÷��̾��� ���ǵ带 ���δ�.
		yield return delay_01;                   // �ð��� �����ٸ�...
		speed = 5;                                   // �÷��̾��� ���ǵ带 �ٽ� �������´�.
	}


	IEnumerator rayCoroutine()
	{
		WaitForSeconds delay = new WaitForSeconds(0.05f);             // �ڷ�ƾ ����ȭ

		LayerMask mask = LayerMask.GetMask("Water");                  // �� ���̾ �̸� �޾ƿ�
		Vector3 down = transform.TransformDirection(Vector3.down);    // �÷��̾� ������(���� ����) ���� ������ ����
		RaycastHit hitInfo;                                           // hit�� ������ �޾ƿ��� ����

		while (true)
		{
			if (Physics.Raycast(this.transform.position + Vector3.up * 0.5f, down, out hitInfo, 1.0f, mask))      // ���� ������
			{
				PlayerHp();         // ���� ����� �� ó�� �Լ�(ü���� ��´�.)
			}

			yield return delay;
		}
	}


	///////////////////////////////////////////////
	// Ʈ���� ����

	void OnCollisionEnter(Collision collision)          // ���ع� ������Ʈ�� ������..,  Object
	{
		if (collision.gameObject.layer.Equals(8))
		{
			PlayerHp();           // ü���� ��� �Լ�
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer.Equals(1))        //  TransparentFX
		{
			if (other.CompareTag(tag_Text01))      // ���� ���ǿ� ������    
			{
				other.GetComponent<Mini01_Potion>().Spawn_Potion();   // ���� �����ϱ�
				mini01_Spawn.InsertQueue_Potion(other.gameObject, 0);    // ���� ���� ������ �ݳ��Ѵ�.

				AudioMng.ins.PlayEffect("Enter");      // ���� ������ �Դ� �Ҹ�
				Muzuk_Potion();     // ���� �ð� �ڷ�ƾ ����
			}
			else if (other.CompareTag(tag_Text02))   // �� ���ǿ� ������..
			{
				other.GetComponent<Mini01_Potion>().Spawn_Potion();   // ���� �����ϱ�
				mini01_Spawn.InsertQueue_Potion(other.gameObject, 1);    // ���� �� ������ �ݳ��Ѵ�.

				AudioMng.ins.PlayEffect("Dough");       // �� ������ �Դ� �Ҹ�
				Shovel_Potion();    // �� �ð� �ڷ�ƾ ����
			}
			else if (other.CompareTag(tag_Text03))       // ���ǵ� ���ǿ� ������
			{
				other.GetComponent<Mini01_Potion>().Spawn_Potion();   // ���� �����ϱ�
				mini01_Spawn.InsertQueue_Potion(other.gameObject, 2);    // ���� ���ǵ� ������ �ݳ��Ѵ�.

				AudioMng.ins.PlayEffect("SpeedUp");     // ���ǵ� ������ �Դ� �Ҹ�
				Speed_Potion();     // ���ǵ� �ð� �ڷ�ƾ ����
			}
			else if (other.CompareTag(tag_Text04))          // ��Ʈ ���ӿ� ������
			{
				mini01_Donut.ResetItemPos();    // ���� ��ġ ������

				AudioMng.ins.PlayEffect("Score_Up");      // ���ھ� �ö󰡴� �Ҹ�
				Donut_Score();        // ���ھ �ø��� �Լ�
			}
		}
		
	}
}
