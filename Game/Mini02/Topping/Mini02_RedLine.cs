using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini02_RedLine : MonoBehaviour             // 레드라인에 부착됨
{
	[SerializeField] Material[] Mat_Array;

	[SerializeField] Mini02_Player mini02_Player;        // 플레이어 스크립트
	[SerializeField] Mini02_CountLine mini02_CountLine;  // 카운트라인 스크립트
	[SerializeField] Mini02_Spawn mini02_Spawn;          // 스폰 스크립트

	[SerializeField] GameObject toppingPanel;     // 토핑 패널
	[SerializeField] Mini02_ToppingPanel mini02_ToppingPanel;         // 토핑 패널 스크립트

	[SerializeField] Button topping_Button;      // 토핑 패널 안에 있는 메인 버튼
	[SerializeField] Image topping_ButtonImage;                 // 토핑 메인 버튼 이미지 (색깔용)
	[SerializeField] Sprite[] sprite_Array;              // 0 : 뿌림(딸기), 1 : 뿌림(초코), 1 : 성공, 2 :실패, 3 : 없음

	[SerializeField] TextMeshProUGUI score_Text;      // 스코어 텍스트(점수)

	Color originalColor;               // 라인의 색깔 변환시 다시 되돌아 올 색깔을 받는 변수

	float z = 0;                       // 라인의 회전을 담당하는 변수
	bool changeBool = false;           // 왼쪽, 오른쪽으로 상태가 어떤지 보는 변수

	bool isGreenLine01 = false;          // 레드라인이 현재 성공 판정에 있는지 묻는 변수
	int succeess = 0;                   // 성공 카운트
	bool isSuccess = false;

	[SerializeField] TextMeshProUGUI countText;   // 서클 중앙에 있는 숫자 카운트가 나오는 텍스트

	bool isHoleOrStar = true;       // 플레이어가 구멍인지, 스타인지
	bool isOvenOrFryer = true;      // 플레이어가 오븐인지, 튀김인지
	bool isPinkOrChoco = true;      // 플레이어가 딸기인지, 초코인지

	int completeDonutInt = 10;     // 구멍, 오븐, 딸기를 집어넣어 숫자를 받아오는 변수

	[SerializeField] GameObject One_Donut;    // 날아가는 도넛
	[SerializeField] GameObject Star_Donut;   // 
	Vector3 originPos;      // 날아가는 도넛의 처음 위치
	Quaternion originRot;   

	[SerializeField] Animator Shake_Anim;     // 신작

	Material[] Donut_Mat_Array = new Material[4];   // 0 : 구멍 오븐, 1 : 구멍 튀김, 2 : 스타 오븐, 3 : 스타 튀김

	Material[] OneOvenStrow_Mat_List = new Material[5];        // 구멍 오븐 딸기
	Material[] OneOvenChoco_Mat_List = new Material[5];        // 구멍 오븐 초코
	Material[] OneFryStrow_Mat_List = new Material[5];         // 구멍 튀김 딸기
	Material[] OneFryChoco_Mat_List = new Material[5];         // 구멍 튀김 초코
	
	Material[] StarOvenStrow_Mat_List = new Material[5];       // 스타 오븐 딸기
	Material[] StarOvenChoco_Mat_List = new Material[5];       // 스타 오븐 초코
	Material[] StarFryStrow_Mat_List = new Material[5];        // 스타 튀김 딸기
	Material[] StarFryChoco_Mat_List = new Material[5];        // 스타 튀김 초코

	Renderer One_Donut_Render;
	Renderer Star_Donut_Render;

	Animator One_Donut_Ani;          // 날아다니는 구멍 도넛 애니메이션
	Animator Star_Donut_Ani;          // 날아다니는 스타 도넛 애니메이션

	[SerializeField] GameObject Complete_One;     // 완성된 구멍 도넛
	[SerializeField] GameObject Complete_Star;    // 완성된 스타 도넛

	[SerializeField] Renderer Complete_One_Render;     // 완성된 구멍 도넛
	[SerializeField] Renderer Complete_Star_Render;    // 완성된 스타 도넛

	[SerializeField] Animator Complete_One_Anim;     // 완성된 구멍 도넛
	[SerializeField] Animator Complete_Star_Anim;    // 완성된 스타 도넛

	Image thisImage;   // 레드라인 이미지(색깔용)

	WaitForSeconds delay01;           // 토핑통 쉐이크 코루틴
	WaitForSeconds delay02;           // 버튼 막기 코루탄

	int shakeId;                    // 달리는 애니메이터를 받는 변수
	int finalId;
	int completeId;



	void Awake()
	{
		One_Donut_Render = One_Donut.GetComponent<Renderer>();
		Star_Donut_Render = Star_Donut.GetComponent<Renderer>();

		One_Donut_Ani = One_Donut.GetComponent<Animator>();
		Star_Donut_Ani = Star_Donut.GetComponent<Animator>();

		thisImage = transform.GetComponent<Image>();

		delay01 = new WaitForSeconds(0.2f);
		delay02 = new WaitForSeconds(0.5f);

		shakeId = Animator.StringToHash("isShake");
		finalId = Animator.StringToHash("isFinal");
		completeId = Animator.StringToHash("isComplete");

		Donut_Mat_Array[0] = Mat_Array[0];
		Donut_Mat_Array[1] = Mat_Array[1];
		Donut_Mat_Array[2] = Mat_Array[2];
		Donut_Mat_Array[3] = Mat_Array[3];


		OneOvenStrow_Mat_List[0] = Mat_Array[4];
		OneOvenStrow_Mat_List[1] = Mat_Array[5];
		OneOvenStrow_Mat_List[2] = Mat_Array[6];
		OneOvenStrow_Mat_List[3] = Mat_Array[7];
		OneOvenStrow_Mat_List[4] = Mat_Array[8];

		OneOvenChoco_Mat_List[0] = Mat_Array[9];
		OneOvenChoco_Mat_List[1] = Mat_Array[10];
		OneOvenChoco_Mat_List[2] = Mat_Array[11];
		OneOvenChoco_Mat_List[3] = Mat_Array[12];
		OneOvenChoco_Mat_List[4] = Mat_Array[13];

		OneFryStrow_Mat_List[0] = Mat_Array[14];
		OneFryStrow_Mat_List[1] = Mat_Array[15];
		OneFryStrow_Mat_List[2] = Mat_Array[16];
		OneFryStrow_Mat_List[3] = Mat_Array[17];
		OneFryStrow_Mat_List[4] = Mat_Array[18];

		OneFryChoco_Mat_List[0] = Mat_Array[19];
		OneFryChoco_Mat_List[1] = Mat_Array[20];
		OneFryChoco_Mat_List[2] = Mat_Array[21];
		OneFryChoco_Mat_List[3] = Mat_Array[22];
		OneFryChoco_Mat_List[4] = Mat_Array[23];


		StarOvenStrow_Mat_List[0] = Mat_Array[24];
		StarOvenStrow_Mat_List[1] = Mat_Array[25];
		StarOvenStrow_Mat_List[2] = Mat_Array[26];
		StarOvenStrow_Mat_List[3] = Mat_Array[27];
		StarOvenStrow_Mat_List[4] = Mat_Array[28];

		StarOvenChoco_Mat_List[0] = Mat_Array[29];
		StarOvenChoco_Mat_List[1] = Mat_Array[30];
		StarOvenChoco_Mat_List[2] = Mat_Array[31];
		StarOvenChoco_Mat_List[3] = Mat_Array[32];
		StarOvenChoco_Mat_List[4] = Mat_Array[33];

		StarFryStrow_Mat_List[0] = Mat_Array[34];
		StarFryStrow_Mat_List[1] = Mat_Array[35];
		StarFryStrow_Mat_List[2] = Mat_Array[36];
		StarFryStrow_Mat_List[3] = Mat_Array[37];
		StarFryStrow_Mat_List[4] = Mat_Array[38];

		StarFryChoco_Mat_List[0] = Mat_Array[39];
		StarFryChoco_Mat_List[1] = Mat_Array[40];
		StarFryChoco_Mat_List[2] = Mat_Array[41];
		StarFryChoco_Mat_List[3] = Mat_Array[42];
		StarFryChoco_Mat_List[4] = Mat_Array[43];
	}


	void OnEnable()       // 시작할떄..
	{
		isHoleOrStar = mini02_Player.isHoleOrStar;        // 구멍인지 스타인지
		isOvenOrFryer = mini02_Player.isOvenOrFryer;      // 오븐인지 튀김인지
		isPinkOrChoco = mini02_ToppingPanel.isPinkOrChoco;      // 딸기인지 초코인지

		if (isHoleOrStar.Equals(false))    // 구멍 도넛
		{
			if (isOvenOrFryer.Equals(false))    // 구멍 오븐
			{
				One_Donut_Render.material = Donut_Mat_Array[0];      // 구멍 도넛에 기본 구멍/오븐 메테리얼을 넣는다.
			}
			else                          // 구멍 튀김
			{
				One_Donut_Render.material = Donut_Mat_Array[1];      // 구멍 도넛에 기본 구멍/튀김 메테리얼을 넣는다.
			}

			originPos = One_Donut.transform.position;    // 처음 구멍 도넛의 위치를 저장
			originRot = One_Donut.transform.rotation;
		}
		else                          // 스타 도넛
		{
			if (isOvenOrFryer.Equals(false))   // 스타 오븐
			{
				Star_Donut_Render.material = Donut_Mat_Array[2];      // 스타 도넛에 기본 스타/오븐 메테리얼을 넣는다.
			}
			else                         // 스타 튀김
			{
				Star_Donut_Render.material = Donut_Mat_Array[3];      // 스타 도넛에 기본 스타/튀김 메테리얼을 넣는다.
			}

			originPos = Star_Donut.transform.position;    // 처음 구멍 도넛의 위치를 저장
			originRot = Star_Donut.transform.rotation;
		}

		if (isPinkOrChoco.Equals(false))   // 딸기
		{
			topping_ButtonImage.sprite = sprite_Array[0];
		}
		else    // 초코
		{
			topping_ButtonImage.sprite = sprite_Array[1];
		}

		originalColor = thisImage.color;    // 레드라인에 있는 색깔을 변수에 저장함

		int startRandInt = Random.Range(0, 2);   // 회전 방향 랜덤으로..
		if (startRandInt.Equals(0))
		{
			changeBool = false;
		}
		else
		{
			changeBool = true;
		}

		z = 0.0f;                     // 회전 초기화
	}

	void OnDisable()              // 끝날 때..
	{
		isGreenLine01 = false;        // 초기화
		isSuccess = false;
		succeess = 0;
		countText.text = succeess.ToString();


		if (isHoleOrStar.Equals(false))    // 신작
		{
			One_Donut_Ani.SetBool(finalId, false);  // 구멍 도넛 애니메이션 초기화
			One_Donut.transform.position = originPos;    // 구멍 도넛 위치 초기화
			One_Donut.transform.rotation = originRot;

			One_Donut.SetActive(false);    // 구멍 도넛 비활성화
			Complete_One_Anim.SetBool(completeId, false);
			Complete_One.SetActive(false);
		}
		else
		{
			Star_Donut_Ani.SetBool(finalId, false);  // 스타 도넛 애니메이션 초기화
			Star_Donut.transform.position = originPos;    // 스타 도넛 위치 초기화
			Star_Donut.transform.rotation = originRot;

			Star_Donut.SetActive(false);    // 스타 도넛 비활성화
			Complete_Star_Anim.SetBool(completeId, false);
			Complete_Star.SetActive(false);
		}
	}

	// 왼쪽에 있는 성공 영역을 터치하거나, 끝에 닿으면
	// 오른쪽에서만 터치되도록 하고 회전을 바꾼다.

	void Update()
	{
		if (isSuccess.Equals(false))
		{
			if (changeBool.Equals(false))         // 왼쪽
			{
				z += Time.deltaTime * 200;
			}
			else                             // 오른쪽
			{
				z -= Time.deltaTime * 200;
			}


			if (transform.localRotation.z > 0.72f)
			{
				if (transform.localRotation.z > 0.73f)
				{
					changeBool = true;
				}

				isGreenLine01 = true;
			}
			else if (transform.localRotation.z < -0.72f)
			{
				if (transform.localRotation.z < -0.73f)
				{
					changeBool = false;
				}

				isGreenLine01 = true;
			}
			else
			{
				isGreenLine01 = false;
			}
		}

		transform.localRotation = Quaternion.Euler(0, 0, z);    // 레드라인 회전(z축)을 z에 따라 변환

	}


	IEnumerator WaitSuccess()       // 끝났을떄!!!!!!!!!
	{
		yield return delay01;

		Shake_Anim.SetBool(shakeId, false);    // 토핑통 애니메이션 끔

		if (isHoleOrStar.Equals(false))    // 신작
		{
			One_Donut_Ani.SetBool(finalId, true);      // 구멍 도넛 애니메이션 실행(넘기기)
		}
		else
		{
			Star_Donut_Ani.SetBool(finalId, true);     // 스타 도넛 애니메이션 실행(넘기기)
		}
	}




	int scoreInt = 0;

	public void Press_RedLine()       // 토핑 패널에 있는 메인 버튼을 눌렀을 경우
	{
		if (succeess.Equals(5))             // 성공 카운트가 5개면 바로 리턴 시킴
		{
			toppingPanel.SetActive(false);     // 토핑 패널 비활성화
			mini02_Player.Origin_Panel();
			return;
		}

		if (isGreenLine01.Equals(true))    // 성공 카운트가 5개 이하이고, 레드 라인이 성공 판정에 있다면
		{
			succeess++;                            // 성공 카운트에 1 더하기(실패 시, 카운트 안함)
			countText.text = succeess.ToString();  // 

			Shake_Anim.SetBool(shakeId, true);    // 신작

			if (isHoleOrStar.Equals(false))        // 구멍 도넛               ///////////////////////////////////////////////////////////////
			{
				AudioMng.ins.PlayEffect("Topping");      // 토핑 쉐이크

				if (isOvenOrFryer.Equals(false))        // 구멍 오븐 도넛
				{
					if (isPinkOrChoco.Equals(false))    // 구멍 오븐 딸기 도넛
					{
						One_Donut_Render.material = OneOvenStrow_Mat_List[succeess - 1];  // 메테리얼 교체
					}
					else                          // 구멍 오븐 초코 도넛
					{
						One_Donut_Render.material = OneOvenChoco_Mat_List[succeess - 1];  // 메테리얼 교체
					}
				}
				else                              // 구멍 튀김 도넛
				{
					if (isPinkOrChoco.Equals(false))    // 구멍 튀김 딸기 도넛
					{
						One_Donut_Render.material = OneFryStrow_Mat_List[succeess - 1];  // 메테리얼 교체
					}
					else                          // 구멍 튀김 초코 도넛
					{
						One_Donut_Render.material = OneFryChoco_Mat_List[succeess - 1];  // 메테리얼 교체
					}
				}
			}
			else                              // 스타 도넛
			{
				AudioMng.ins.PlayEffect("Topping");      // 토핑 쉐이크

				if (isOvenOrFryer.Equals(false))     // 스타 오븐 도넛
				{
					if (isPinkOrChoco.Equals(false))     // 스타 오븐 딸기 도넛
					{
						Star_Donut_Render.material = StarOvenStrow_Mat_List[succeess - 1];  // 메테리얼 교체
					}
					else                           // 스타 오븐 초코 도넛
					{
						Star_Donut_Render.material = StarOvenChoco_Mat_List[succeess - 1];  // 메테리얼 교체
					}
				}
				else                           // 스타 튀김 도넛
				{
					if (isPinkOrChoco.Equals(false))     // 스타 튀김 딸기 도넛
					{
						Star_Donut_Render.material = StarFryStrow_Mat_List[succeess - 1];  // 메테리얼 교체
					}
					else                           // 스타 튀김 초코 도넛
					{
						Star_Donut_Render.material = StarFryChoco_Mat_List[succeess - 1];  // 메테리얼 교체
					}
				}
			}

			if (succeess.Equals(5) && isSuccess.Equals(false))   // 성공 카운트가 5개라면 -> 성공
			{
				isSuccess = true;     // 성공했다고 알림
				mini02_Player.playerCompleteInt = 0;     // 도우 패널로 가라고 알림

				if (mini02_CountLine.newMonster.Equals(true))
				{
					completeDonutInt = CheckCompleteDonut(isHoleOrStar, isOvenOrFryer, isPinkOrChoco);     // 완료 도넛이 뭔지 알려줌

					if (isHoleOrStar.Equals(false))
					{
						Complete_One.SetActive(true);
						Complete_One_Anim.SetBool(completeId, true);
					}
					else
					{
						Complete_Star.SetActive(true);
						Complete_Star_Anim.SetBool(completeId, true);
					}


					if (completeDonutInt.Equals(mini02_CountLine.menuInt))               // 도넛을 원하는 것으로 줄때...
					{
						mini02_CountLine.GoHome_Guest(false);

						AudioMng.ins.PlayEffect("Success");      // 성공 환-호

						topping_ButtonImage.sprite = sprite_Array[2];

						scoreInt++;  // 스코어 올리기
						score_Text.text = scoreInt.ToString();    // 스코어를 텍스트로 표현
						mini02_Spawn.ScoreUp();                       // 스폰 스크립트에서 스코어 올린다.(스폰 시간 단축을 위해...)
						mini02_Player.scoreInt++;
					}
					else                                   // 주문한 도넛과 다른 것일떄....
					{
						completeDonutInt = -2;

						AudioMng.ins.PlayEffect("DdiYong");      // 실패 띠-용

						topping_ButtonImage.sprite = sprite_Array[3];
					}
				}
				else
				{
					completeDonutInt = -2;
					topping_ButtonImage.sprite = sprite_Array[4];
				}

				StartCoroutine(WaitSuccess());

				return;
			}

			StartCoroutine(SuccessCoroutine());     // 성공 시, 레드라인을 파란색으로 바꾸고 0.2초 후 다시 원래대로(빨강색)으로 바꾸기
			StartCoroutine(ClickWait());            // 버튼을 누를 시, 0.5초 동안 버튼을 못 누르게 하기...
		}
		else                          // 성공 카운트가 5개 이하이고, 레드 라인이 성공 판정에 없다면
		{
			Shake_Anim.SetBool(shakeId, false);    // 토핑통 애니메이션 끔

			AudioMng.ins.PlayEffect("Fail03");      // 토핑 실패

			StartCoroutine(FailCoroutine());        // 성공 시, 레드라인을 노란색으로 바꾸고 0.2초 후 다시 원래대로(빨강색)으로 바꾸기
			StartCoroutine(ClickWait());            // 버튼을 누를 시, 0.5초 동안 버튼을 못 누르게 하기...
		}
	}

	IEnumerator SuccessCoroutine()      // 성공시 하는 코루틴
	{
		thisImage.color = Color.blue;

		yield return delay01;
		thisImage.color = originalColor;
	}

	IEnumerator FailCoroutine()          // 실패시 하는 코루틴
	{
		thisImage.color = Color.yellow;

		yield return delay01;
		thisImage.color = originalColor;
	}

	IEnumerator ClickWait()              // 버튼을 누를 때마다 호출하는 코루틴
	{
		topping_Button.interactable = false;     // 버튼을 누르자마자 버튼을 멈춤
		topping_ButtonImage.color = Color.red;         // 버튼을 누르자마자 버튼을 빨간색으로 바꿈
		yield return delay02;                      // 버튼을 못 누르는 시간
		topping_ButtonImage.color = Color.white;       // 시간이 지난 뒤, 버튼을 색깔을 되돌림
		topping_Button.interactable = true;      // 시간이 지난 뒤, 버튼을 클릭할 수 있도록 함
	}


	int CheckCompleteDonut(bool isHoleOrStar, bool isOvenOrFryer, bool isPinkOrChoco)
	{
		int playerDonutInt = -1;

		if (isHoleOrStar.Equals(false))             // 틀 패널에서 구멍을 선택했을 경우
		{
			if (isOvenOrFryer.Equals(false))        // 오븐에서 구워졌을 경우
			{
				if (isPinkOrChoco.Equals(false))    // 딸기 토핑을 선택했을 경우
				{
					playerDonutInt = 0;        // 구멍 / 오븐 / 딸기
					Complete_One_Render.material = OneOvenStrow_Mat_List[4];
				}
				else                          // 초코 토핑을 선택했을 경우
				{
					playerDonutInt = 1;        // 구멍 / 오븐 / 초코
					Complete_One_Render.material = OneOvenChoco_Mat_List[4];
				}
			}
			else                              // 튀김기에서 튀겼을 경우
			{
				if (isPinkOrChoco.Equals(false))    // 딸기 토핑을 선택했을 경우
				{
					playerDonutInt = 2;        // 구멍 / 튀김 / 딸기
					Complete_One_Render.material = OneFryStrow_Mat_List[4];
				}
				else                          // 초코 토핑을 선택했을 경우
				{
					playerDonutInt = 3;        // 구멍 / 튀김 / 초코
					Complete_One_Render.material = OneFryChoco_Mat_List[4];
				}
			}
		}
		else                                 // 틀 패널에서 스타를 선택했을 경우
		{
			if (isOvenOrFryer.Equals(false))        // 오븐에서 구워졌을 경우
			{
				if (isPinkOrChoco.Equals(false))     // 딸기 토핑을 선택했을 경우
				{
					playerDonutInt = 4;         // 스타 / 오븐 / 딸기
					Complete_Star_Render.material = StarOvenStrow_Mat_List[4];
				}
				else                           // 초코 토핑을 선택했을 경우
				{
					playerDonutInt = 5;         // 스타 / 오븐 / 초코
					Complete_Star_Render.material = StarOvenChoco_Mat_List[4];
				}
			}
			else                               // 튀김기에서 튀겼을 경우
			{
				if (isPinkOrChoco.Equals(false))     // 딸기 토핑을 선택했을 경우
				{
					playerDonutInt = 6;         // 스타 / 튀김 / 딸기
					Complete_Star_Render.material = StarFryStrow_Mat_List[4];
				}
				else                           // 초코 토핑을 선택했을 경우
				{
					playerDonutInt = 7;         // 스타 / 튀김 / 초코
					Complete_Star_Render.material = StarFryChoco_Mat_List[4];
				}
			}
		}

		return playerDonutInt;
	}
}
