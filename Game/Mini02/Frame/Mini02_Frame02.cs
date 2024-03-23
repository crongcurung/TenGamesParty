using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mini02_Frame02 : MonoBehaviour    // 두번쨰 틀에 부착됨
{
	[SerializeField] Mini02_FramePanel mini02_FramePanel;       // 틀 패널 스크립트
	[SerializeField] Mini02_Player mini02_Player;               // 플레이어 스크립트

	[SerializeField] Button frameButton;       // 틀 패널에 있는 메인버튼

	[SerializeField] Image Right_Image;
	[SerializeField] Sprite[] sprite_Array;    // 0 : 흔들(원), 1 : 흔들(스타) 2 : 뺵

	[SerializeField] Slider slider;           // 연타 누르면 올라가는 슬라이더

	Animator anim;           // 뒤집어진 틀이 흔들거리는 애니메이터 받는 변수
	bool isSuccess = false;    // 성공했는지 묻는 변수

	[SerializeField] Sprite holeDonut01;         // 완료 이미지에 구멍 도넛 이미지 넣을려는 변수
	[SerializeField] Sprite starDonut01;         // 완료 이미지에 스타 도넛 이미지 넣을려는 변수

	[SerializeField] GameObject Spread_Dough;    // 펴진 반죽 오브젝트
	[SerializeField] GameObject One_Frame;       // 구멍 도넛 틀 오브젝트
	[SerializeField] GameObject Star_Frame;      // 스타 도넛 틀 오브젝트

	[SerializeField] Animator One_Frame_Anim;       // 구멍 도넛 틀 오브젝트
	[SerializeField] Animator Star_Frame_Anim;      // 스타 도넛 틀 오브젝트

	[SerializeField] GameObject One_Complete;    // 날아다니는 구멍 도넛
	[SerializeField] GameObject Star_Complete;   // 날아다니는 스타 도넛

	[SerializeField] Animator One_Complete_Anim;    // 날아다니는 구멍 도넛
	[SerializeField] Animator Star_Complete_Anim;   // 날아다니는 스타 도넛

	WaitForSeconds delay01;   // 도마 위에 틀이 움직이는 애니메이션

	int frameId;   // 애니메이터 최적화
	int endId;

	void Awake()
	{
		frameId = Animator.StringToHash("isFrameShake");
		endId = Animator.StringToHash("isEnd");

		delay01 = new WaitForSeconds(0.1f);
	}

	void OnEnable()      // 켜질떄..
	{
		Spread_Dough.SetActive(true);
		


		if (mini02_FramePanel.isHoleOrStar.Equals(false))      //페이지01에서 구멍 틀을 클릭했다면...
		{
			One_Frame.SetActive(true);    // 구멍 틀 활성화
			anim = One_Frame_Anim;        // 구멍 애니메이션을 담는다.

			Right_Image.sprite = sprite_Array[0];
		}
		else                                             // 페이지01에서 별 틀을 클릭했다면...
		{
			Star_Frame.SetActive(true);    // 스타 틀 활성화
			anim = Star_Frame_Anim;        // 스타 애니메이션을 담는다.

			Right_Image.sprite = sprite_Array[1];
		}
		frameButton.interactable = true;             // 틀 패널 메인버튼 활성화(이때 누를 수 있음)
	}


	void OnDisable()      // 꺼질떄...
	{
		slider.value = 0;                      // 슬라이더 초기화
		isSuccess = false;                     // 성공 여부 초기화

		frameButton.interactable = false;             // 틀 패널 메인버튼 활성화(이때 누를 수 있음)

		Spread_Dough.SetActive(false);         // 펴진 도넛 오브젝트 비활성화
		One_Frame.SetActive(false);            // 구멍 도넛 틀 오브젝트 비활성화
		Star_Frame.SetActive(false);           // 스타 도넛 틀 오브젝트 비활성화

		One_Complete.SetActive(false);         // 날아다니는 구멍 도넛 비활성화
		Star_Complete.SetActive(false);        // 날아다니는 스타 도넛 비활성화

		Right_Image.sprite = sprite_Array[0];

		Invoke("FinalInvoke", 0.1f);   // 인보크 실행
	}

	void FinalInvoke()   // 마지막에 애니메이션을 끄는 인보크?(이거 해야할지 의문...)
	{
		anim.SetBool(frameId, false);         // 애니메이션 바로 끄기(반죽)
	}


	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Press_FrameButton();
		}

		if (slider.value.Equals(1) && isSuccess.Equals(false))             // 슬라이더의 값이 1이면 끝내기, 완료 상태...
		{
			isSuccess = true;               // 완료 되었다고 알림
			Right_Image.sprite = sprite_Array[2];

			AudioMng.ins.PlayEffect("SpeedUp");      // 틀 도우 애니메이션 실행

			if (mini02_FramePanel.isHoleOrStar.Equals(false))       // 구멍 도넛                       
			{
				One_Complete.SetActive(true);      // 신작
				One_Complete_Anim.SetBool(endId, true);   // 신작

				mini02_Player.isHoleOrStar = false;                              // 구멍 틀을 만들었다고하고 끝내기
			}
			else                        // 스타 도넛
			{
				Star_Complete.SetActive(true);      // 신작
				Star_Complete_Anim.SetBool(endId, true);   // 신작

				mini02_Player.isHoleOrStar = true;                             // 스타 틀을 만들었다고하고 끝내기
			}

			mini02_Player.playerCompleteInt = 2;     // 오븐, 튀김 패널로 가라고 알림

			return;
		}

		if (slider.value < 1.0f)       // 슬라이더 값이 1 미만이면
		{
			slider.value -= 0.1f * Time.deltaTime;     // 버튼을 누르지 않는다면 슬라이더의 값 감소
		}
	}


	public void Press_FrameButton()    // 계속 누르면 슬라이더가 채워진다.
	{
		if (slider.value.Equals(1.0f))        // 슬라이더 값이 1이면
		{
			
			mini02_FramePanel.EndButton();

			return;
		}

		AudioMng.ins.PlayEffect("Dough");     // 틀 슬라이더 상승
		slider.value += 0.05f;                // 슬라이더 값 상승
		anim.SetBool(frameId, true);   // 버튼을 누를때마다 틀 애니메이션 발동(반죽)
		
		StartCoroutine(WaitShake());           // 애니메이션 발동 시간(반죽)
	}

	IEnumerator WaitShake()                       // 도마 위에 틀이 움직이는 애니메이션
	{
		yield return delay01;
		anim.SetBool(frameId, false);         // 애니메이션 바로 끄기(반죽)
	}
}
