using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mini02_DoughPanel : MonoBehaviour
{
	[SerializeField] Sprite LeftArrow;
	[SerializeField] Sprite RightArrow;

	[SerializeField] Mini02_Player mini02_Player;   // 플레이어 스크립트

    [SerializeField] Image[] Arrow_Array;     // 악보에 있는 10개의 화살 넣는 곳 10개
    List<int> saveint = new List<int>();   // 랜덤 생성된 숫자(왼쪽인지, 오른쪽인지)를 담는 리스트
    int countInt = 0;                     // 성공 카운트

    Sprite leftArrow;        // 왼쪽 화살
    Sprite rightArrow;       // 오른쪽 화살

	[SerializeField] Button leftButton;         // 왼쪽 도우 버튼
	[SerializeField] Button righrButton;        // 오른쪽 도우 버튼

	[SerializeField] Image Right_Image;
	[SerializeField] Sprite[] RightSprite_Array;      // 오른쪽, 뺵

	[SerializeField] Animator anim_Dough;          // 반죽 애니메이션
    [SerializeField] GameObject doughInBow;        // 그릇 안에 있는 반죽
    [SerializeField] Animator anim_Player;         // 반죽을 하는 애니메이션
    [SerializeField] GameObject Throw_Donut;       // 마지막 반죽 날리는거
	[SerializeField] Animator Throw_Donut_Anim;    // 끝날때 날아가는 도넛

	[SerializeField] Transform dough_Player;       // 도우 패널에 있는 플레이어 위치
	Vector3 player_Trans;
	Quaternion player_Quater;

	WaitForSeconds delay;  // 틀렸을 떄 기다리는 코루틴

	int doughId;     // 애니메이터 최적화
	int doughId_P;
	int throwId;

	string invoke_Text;


	// player : 9.946518, -0.0952649, 4.553154                 0, 184.403, 0
	// bowl   : 9.935, 1.81, 1.818                             -90.91, 181.09, -0.242
	// dough  : 9.985, 0.011, 4.028                            0, -179.982, 0

	void Awake()
	{
		doughId = Animator.StringToHash("isDough");
		doughId_P = Animator.StringToHash("isDough");
		throwId = Animator.StringToHash("isThrow");

		leftButton.interactable = true;      // 도우 패널 버튼 활성화
		righrButton.interactable = true;

		leftArrow = LeftArrow;           // 왼쪽 화살표 이미지 가져옴
		rightArrow = RightArrow;         // 오른쪽 화살표 이미지 가져옴

		invoke_Text = "Final_Success";

		player_Trans = dough_Player.position;
		player_Quater = dough_Player.rotation;

		delay = new WaitForSeconds(1.0f);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Press_LeftMainButton();
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Press_RightMainButton();
		}
	}

	void OnEnable()         // 도우 패널이 열린다면....
	{
        for (int i = 0; i < Arrow_Array.Length; i++)        // 10번 돌림
        {
            int randInt = Random.Range(0, 2);   // 0이면 left, 1이면 right

            if (randInt.Equals(0))                       // 왼쪽
            {
                Arrow_Array[i].sprite = leftArrow;   // 순차적으로 왼쪽 화살표 등록
            }
            else                                   // 오른쪽
            {
                Arrow_Array[i].sprite = rightArrow;   // 순차적으로 오른쪽 화살표 등록
            }
            saveint.Add(randInt);                 // 숫자를 순차적으로 저장
        }

		Right_Image.sprite = RightSprite_Array[0];
	}

	void OnDisable()
	{
		saveint.Clear();                             // 저장 리스트를 초기화
		for (int i = 0; i < Arrow_Array.Length; i++)
		{
			Arrow_Array[i].gameObject.SetActive(true);         //  플레이어가 지웠던 화살표들을 전부 켜두기...
		}
		countInt = 0;                                 // 플레이어가 몇번쨰 눌렀는지 알려주는 변수 초기화

		anim_Dough.SetBool(doughId, false);        // 애니메이션 초기화
		anim_Player.SetBool(doughId_P, false);   

		doughInBow.SetActive(true);     // 다시 그릇안에 있는 반죽을 킨다.

		leftButton.interactable = true;      // 도우 패널 버튼 활성화
		righrButton.interactable = true;

		dough_Player.position = player_Trans;
		dough_Player.rotation = player_Quater;

		Throw_Donut_Anim.SetBool(throwId, false);    // 애니메이션 초기화
		Throw_Donut.SetActive(false);            // 날아다니는 도넛 비활성화
	}



	void CheckArrow(int countInt, bool isLeftButton)    // 성공인지 실패인지 구분하는 함수
	{
		if (isLeftButton.Equals(true))                      // 플레이어가 왼쪽 메인 버튼을 눌렀다면
		{
			if (saveint[countInt].Equals(0))         // 성공
			{
				Arrow_Array[countInt].gameObject.SetActive(false);    // 성공했으니 앞에 하나를 없앰
				this.countInt++;                // 성공했으니 카운트 올라감

				AudioMng.ins.PlayEffect("Dough");     // 도우 반죽하는 소리

				if (this.countInt < 10)         // 성공 카운트가 10개 미만이면
				{
					anim_Dough.SetBool(doughId, true);     // 반죽 하는 애니메이션 발동
					anim_Player.SetBool(doughId_P, true);
				}
			}
			else         // 실패
			{
				StartCoroutine(WaitKneading_Fail());          // 실패 이미지 코루틴 실행
			}
		}
		else                                             // 플레이어가 오른쪽 메인 버튼을 눌렀다면
		{
			if (saveint[countInt].Equals(1))               // 성공
			{
				Arrow_Array[countInt].gameObject.SetActive(false);    // 성공했으니 앞에 하나를 없앰
				this.countInt++;                      // 성공했으니 카운트 올라감

				AudioMng.ins.PlayEffect("Dough");     // 도우 반죽하는 소리

				if (this.countInt < 10)            // 성공 카운트가 10개 미만이면
				{
					anim_Dough.SetBool(doughId, true);     // 반죽 하는 애니메이션 발동
					anim_Player.SetBool(doughId_P, true);
				}
			}
			else                                // 실패
			{
				StartCoroutine(WaitKneading_Fail());      // 실패 이미지 코루틴 실행
			}
		}



		if (this.countInt.Equals(10))         // 성공 카운트가 10개 라면...               반죽 단계 성공!!!!!!!!!!!!!!!!
		{
			Right_Image.sprite = RightSprite_Array[1];

			AudioMng.ins.PlayEffect("SpeedUp");       // 도우 애니메이션 실행

			mini02_Player.playerCompleteInt = 1;     // 틀 패널로 가라고 알림
			doughInBow.SetActive(false);     // 그릇 안에 들어있는 반죽을 없앰
			anim_Player.SetBool(doughId_P, true);  // 플레이어가 반죽하는 애니메이션(반복)
			Invoke(invoke_Text, 0.3f);         // 끝나는거라 인보크로 함
			Throw_Donut.SetActive(true);           // 날아가는 도넛 활성화
			Throw_Donut_Anim.SetBool(throwId, true);    // 날아가는 도넛 애니메이션 발동!
		}
	}

	void Final_Success()  // 마지막 손짓
	{
		anim_Player.SetBool(doughId_P, false);  //  0.3초 후 애니메이션 끝내기
	}


	public void Press_LeftMainButton()    // 왼쪽 버튼을 누를 때
	{
		if (countInt < 10)          // 성공 카운트가 10개 미만이면 화살표 체크로 감
		{
			CheckArrow(countInt, true);

			
		}
		else                      // 성공 카운트가 10개면 못 누르게 하기
		{
			return;
		}
	}


	public void Press_RightMainButton()   // 오른쪽 버튼을 누를 때
	{
		if (countInt < 10)          // 성공 카운트가 10개 미만이면 화살표 체크로 감
		{
			CheckArrow(countInt, false);    // 맞게 했는지 체크

		}
		else if (countInt.Equals(10))    // 성공 카운트가 10개면 나가는 버튼으로 변신함
		{
			EndButton();     // 완전 메인으로 가는 함수
		}
		else                       // 성공 카운트가 10 초과이면 못 누르게 하기
		{
			return;
		}
	}


	public void EndButton()         // 완전 메인으로 가는 함수
	{
		mini02_Player.Origin_Panel();         // 패널을 비활성화 하는 함수
		
		transform.gameObject.SetActive(false);    // 이 패널을 비활성화
	}


	/////////////////////////////// 코루틴 구역

	IEnumerator WaitKneading_Fail()          // 플레이어가 틀렸을 경우
	{
		leftButton.interactable = false;     // 버튼 막기
		righrButton.interactable = false;

		AudioMng.ins.PlayEffect("Meow");     // 도우 반죽하는 소리

		anim_Dough.SetBool(doughId, false);           // 반죽 애니메이샨 끝내기
		anim_Player.SetBool(doughId_P, false);

		yield return delay;     // 틀렸을 때, 1초 기다려야 함...

		leftButton.interactable = true;     // 1초 후, 버튼 풀기
		righrButton.interactable = true;
	}
}
