using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Mini01_Player : MonoBehaviour            // 플레이어 부모클래스를 상속
{
	[SerializeField] Joystic joystic;

	[SerializeField] GameObject Result_Panel;
	[SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // 점수를 서버에 등록시킬 수 있는 버튼

	public bool isShip = false;
	[SerializeField] GameObject[] Hp_Array;

	[SerializeField] float dig_x;      // 함정 x 조정 값
	[SerializeField] float dig_z;      // 함정 y 조정 값

	[SerializeField] Mini01_Donut mini01_Donut;

	bool isShield = false;             // 체력이 닳거나, 무적 상태라면 true..
	bool isHit = false;                // 이미 맞고 4초 이내이면 true..

	int shovel_Int;                    // 플레이어가 사용 가능한 삽 개수 인트 값
	Coroutine muzukCor;                // 무적 시간 코루틴을 받는 변수
	Coroutine speedCor;                // 스피드 시간 코루틴을 받는 변수

	GameObject starCube;               // 무적 포션을 먹을 때 플레이어 주위에 도는 스타 큐브

	Vector3 digRot;

	[SerializeField] Mini01_Spawn mini01_Spawn;
	[SerializeField] TextMeshProUGUI right_Text;
	[SerializeField] TextMeshProUGUI score_Text;

	Vector3 moveDir;                     // 이동 변수는 플레이어가 지닐 가능성 높음
	Vector3 rotBan = Vector3.zero;                      // 리지드바디로 이동 시, 캐릭터가 계속 회전하는 문제를 막기 위한 변수

	int playerHp;                        // 플레이어의 체력 변수
	int playerScore;                        // 플레이어 스코어

	Rigidbody rigid;                 // 리지드바디는 플레이어와 몬스터가 공통으로 지닐 가능성 높음
	Animator anim;                       // 애니메이터는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

	[SerializeField] float speed;        // 이동 속도 변수는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

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
		playerHp = 3;         // 플레이어의 체력은 3부터 시작한다.(더 이상 안 오른다.)
		playerScore = 0;      // 하트 도넛을 먹으면 스코어가 올라간다.(제한 없음)

		hor_Text = "Horizontal";
		ver_Text = "Vertical";

		run_Id = Animator.StringToHash("isRun");
		attack_Id = Animator.StringToHash("isAttack");

		tag_Text01 = "Spring";          // 무적 포션
		tag_Text02 = "Bear";           // 삽 포션
		tag_Text03 = "Cushion";        // 스피드 포션
		tag_Text04 = "Coin";           // 하트 도넛

		invoke_Text = "Invoke_Shove";

		right_Text.text = "3";           // 삽은 처음에 3번이라 3이라 적는다.

		rigid = GetComponent<Rigidbody>();              // 플레이어의 리지드 바디를 가지고 온다.
		anim = GetComponent<Animator>();                // 플레이어의 애니메이션을 가지고 온다.
		starCube = transform.GetChild(2).gameObject;    // 스타 큐브를 가지고 온다.

		shovel_Int = 3;            // 처음에는 삽을 3번 줌
		digRot = new Vector3(-90, 0, 0);     // 함정 회전 고정 값 지정

		delay_01 = new WaitForSeconds(10.0f);   // 무적, 스피드 코루틴 시간 설정
		delay_02 = new WaitForSeconds(4.0f);    // 맞았을 떄 잠시 무적 시간 설정
		delay_03 = new WaitForSeconds(0.25f);

		render = transform.GetChild(1).GetComponent<Renderer>();

		StartCoroutine(rayCoroutine());     // 레이는 처음부터 실행한다.

		AudioMng.ins.Play_BG("Mini01_B");
	}

	void Move()                 // 부모 클래스에서 이동 함수가 있기 때문에 재정의 한다!
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
        rigid.velocity = moveDir * speed;          // 벨로시티로 이동

		if (!moveDir.x.Equals(0) || !moveDir.z.Equals(0))               // wasd 아무거나라도 움직인다면..
        {
            anim.SetBool(run_Id, true);         // 움직이는 애니메이션 실행
			isShip = false;
			
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z)), 0.2f);
            // 플레이어의 회전 값
        }
        else
        {
            anim.SetBool(run_Id, false);       // 멈추는 애니메이션 실행
            rigid.angularVelocity = rotBan;          // 플레이어가 무언가에 충돌시 계속 회전하는 문제가 있어서 막아버림
			isShip = true;                    
		}
    }

    void Update()
    {
        Move();

		if (Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			Press_RightButton();     // 오른쪽 버튼을 누른다면..
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


	void MinusHp()  // 실질적으로 이 함수가 실행되어야지 체력이 닳는다.
	{
		playerHp--;                  // 플레이어의 체력을 닳게 한다.

		Hp_Array[playerHp].SetActive(false);

		StartCoroutine(Render_Hp());

		if (playerHp.Equals(0))                 // 체력이 다 닿으면...
		{
			Result_Panel.SetActive(true);       // 결과 창을 띄운다.
			Game_Panel.SetActive(true);         // 반투명 창을 띄운다.

			Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + playerScore.ToString();

			if (Main.ins.nowPlayer.maxScore_List[0] >= playerScore)    // 최고점을 못 넘은다면...
			{
				Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[0].ToString();
			}
			else        // 최고점을 넘은 경우 (신기록)
			{
				Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + playerScore.ToString();

				Main.ins.nowPlayer.maxScore_List[0] = playerScore;
				Main.ins.SaveData();

				GPGS_Button.interactable = true;        // 신기록을 세우면 점수 서버 등록 버튼을 활성화한다. /////////////////////////////////////////////////////////////////////////////

			}

			AudioMng.ins.Pause_BG();              // 배경음악을 끈다.
			AudioMng.ins.PlayEffect("Fail02");

			Time.timeScale = 0;                 // 게임을 끝낸다...

		}
		else if (isHit.Equals(false))         // 이미 맞은 상태가 아니라면...
		{
			AudioMng.ins.PlayEffect("Meow");     // 체력이 닳으면 소리냄
			StartCoroutine(HitCoroutine());
			return;
		}
	}

	public void Press_GPGS_01()   // 신기록 시, 서버에 등록
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no1, playerScore);   // 점수를 서버에 등록
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no1);          // 리더보드를 띄운다.
	}

	public void PlayerHp()          // public으로 놓아서 다른 곳에서도 이 함수에 접근 가능하도록 한다.
	{
		if (isShield.Equals(false) && isHit.Equals(false))   // 스타 큐브가 작동하고 있지 않으면..
		{
			MinusHp();                // 체력을 깍는다.
		}
	}


	public void Press_RightButton()          // 오른쪽 버튼 혹은 엔터를 누른다면..
	{
		if (shovel_Int > 0 && transform.position.y < 1.93f)     // 삽 인트가 0보다 크고, 현재 플레이어의 높이가 일정 높이보다 낮다면...
		{
			shovel_Int--;       // 삽 숫자를 줄인다.
			right_Text.text = shovel_Int.ToString();           // 형 변환 물어보기...

			anim.SetBool(attack_Id, true);
			Invoke(invoke_Text, 0.5f);

			int ceil_x = Vector3Int.CeilToInt(transform.position).x;        // 플레이어의 현재 위치를 정수값으로 변환한다.
			int ceil_z = Vector3Int.CeilToInt(transform.position).z;

			GameObject dig_Prefab = mini01_Spawn.GetQueue_Digged();      // 함정 오브젝트를 가져온다.
			if (dig_Prefab == null)    // 만약 함정 오브젝트를 못 가져온다면...(함정 오브젝트를 개수 제한이 있음)
			{
				return;      // 끝내 버림
			}

			AudioMng.ins.PlayEffect("Shovel");       // 삽 파는 소리
			dig_Prefab.transform.position = new Vector3(ceil_x + dig_x, transform.position.y, ceil_z + dig_z);   // 함정 오브젝트의 위치와 회전 조절
			dig_Prefab.transform.rotation = Quaternion.Euler(digRot);
		}
	}

	void Invoke_Shove()
	{
		anim.SetBool(attack_Id, false);
	}

	void Muzuk_Potion()             // 무적 포션에 플레이어가 닿으면...
	{
		if (isShield.Equals(true))        // 현재 무적 시간 코루틴이 실행중이라면..
		{
			StopCoroutine(muzukCor);      // 실행되고 있는 무적 시간 코루틴 중단
		}
		muzukCor = StartCoroutine(MuzukCoroutine());   // 무적 시간 코루틴을 실행한다.
	}

	void Shovel_Potion()            // 삽 포션에 플레이어가 닿으면...
	{
		if (shovel_Int.Equals(5))     // 현재 플레이어가 삽 인트를 5개를 들고 있다면 더 이상 추가하지 않는다.
		{
			return;
		}

		shovel_Int++;               // 삽 인트를 증가시킨다.
		right_Text.text = shovel_Int.ToString();     // 오른쪽 버튼 텍스트에 변경사항을 알린다.
	}

	void Speed_Potion()             // 스피드 포션에 플레이어가 닿으면...
	{
		if (speed > 6)              // 플레이어의 스피드가 이미 올라간 상태라면..
		{
			StopCoroutine(speedCor);    // 실행 중인 스피드 시간 코루틴을 중단시킨다.
		}
		speedCor = StartCoroutine(SpeedCoroutine());      // 스피드 시간 코루틴을 실행시킨다.
	}

	void Donut_Score()          // 하트 도넛에 닿는다면..
	{
		playerScore++;          // 스코어를 올린다.
		score_Text.text = playerScore.ToString();      // 스코어 변경사항을 알린다.
	}





	////////////////////////////////////////////////////////////
	// 코루틴 구역

	IEnumerator HitCoroutine()
	{
		isHit = true;
		yield return delay_02;
		isHit = false;
	}

	IEnumerator MuzukCoroutine()                    // 무적 시간 코루틴
	{
		isShield = true;                            // 무적 상태라고 알린다.
		starCube.SetActive(true);                   // 스타큐브를 킨다.
		yield return delay_01;                  // 시간이 끝난다면...
		starCube.SetActive(false);                  // 스타큐브를 끈다.
		isShield = false;                           // 무적 상태가 끝났다고 알린다.
	}

	IEnumerator SpeedCoroutine()                     // 스피드 시간 코루틴
	{
		speed = 13;                                  // 플레이어의 스피드를 높인다.
		yield return delay_01;                   // 시간이 끝난다면...
		speed = 5;                                   // 플레이어의 스피드를 다시 돌려놓는다.
	}


	IEnumerator rayCoroutine()
	{
		WaitForSeconds delay = new WaitForSeconds(0.05f);             // 코루틴 최적화

		LayerMask mask = LayerMask.GetMask("Water");                  // 물 레이어를 미리 받아옴
		Vector3 down = transform.TransformDirection(Vector3.down);    // 플레이어 밑으로(로컬 기준) 레이 방향을 설정
		RaycastHit hitInfo;                                           // hit된 정보를 받아오는 변수

		while (true)
		{
			if (Physics.Raycast(this.transform.position + Vector3.up * 0.5f, down, out hitInfo, 1.0f, mask))      // 물에 닿으면
			{
				PlayerHp();         // 물에 닿았을 때 처리 함수(체력이 닿는다.)
			}

			yield return delay;
		}
	}


	///////////////////////////////////////////////
	// 트리거 구역

	void OnCollisionEnter(Collision collision)          // 방해물 오브젝트에 닿으면..,  Object
	{
		if (collision.gameObject.layer.Equals(8))
		{
			PlayerHp();           // 체력을 깍는 함수
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer.Equals(1))        //  TransparentFX
		{
			if (other.CompareTag(tag_Text01))      // 무적 포션에 닿으면    
			{
				other.GetComponent<Mini01_Potion>().Spawn_Potion();   // 포션 스폰하기
				mini01_Spawn.InsertQueue_Potion(other.gameObject, 0);    // 닿은 무적 포션을 반납한다.

				AudioMng.ins.PlayEffect("Enter");      // 무적 아이템 먹는 소리
				Muzuk_Potion();     // 무적 시간 코루틴 실행
			}
			else if (other.CompareTag(tag_Text02))   // 삽 포션에 닿으면..
			{
				other.GetComponent<Mini01_Potion>().Spawn_Potion();   // 포션 스폰하기
				mini01_Spawn.InsertQueue_Potion(other.gameObject, 1);    // 닿은 삽 포션을 반납한다.

				AudioMng.ins.PlayEffect("Dough");       // 삽 아이템 먹는 소리
				Shovel_Potion();    // 삽 시간 코루틴 실행
			}
			else if (other.CompareTag(tag_Text03))       // 스피드 포션에 닿으면
			{
				other.GetComponent<Mini01_Potion>().Spawn_Potion();   // 포션 스폰하기
				mini01_Spawn.InsertQueue_Potion(other.gameObject, 2);    // 닿은 스피드 포션을 반납한다.

				AudioMng.ins.PlayEffect("SpeedUp");     // 스피드 아이템 먹는 소리
				Speed_Potion();     // 스피드 시간 코루틴 실행
			}
			else if (other.CompareTag(tag_Text04))          // 하트 도넛에 닿으면
			{
				mini01_Donut.ResetItemPos();    // 도넛 위치 재조정

				AudioMng.ins.PlayEffect("Score_Up");      // 스코어 올라가는 소리
				Donut_Score();        // 스코어를 올리는 함수
			}
		}
		
	}
}
