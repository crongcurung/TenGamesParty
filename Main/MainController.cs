using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoogleMobileAds.Api;


public class MainController : MonoBehaviour
{
	string adUnitId;      // 전면 광고 아이디

	private InterstitialAd interstitialAd;

	public void LoadInterstitialAd() //광고 로드
	{
		if (interstitialAd != null)
		{
			interstitialAd.Destroy();
			interstitialAd = null;
		}

		var adRequest = new AdRequest();
		adRequest.Keywords.Add("unity-admob-sample");

		InterstitialAd.Load(adUnitId, adRequest,
			(InterstitialAd ad, LoadAdError error) =>
			{
				if (error != null || ad == null)
				{

					return;
				}

				interstitialAd = ad;

				RegisterEventHandlers(interstitialAd); //이벤트 등록
			});
	}

	public void ShowAd()                //광고 보기
	{
		if (interstitialAd != null && interstitialAd.CanShowAd())
		{
			interstitialAd.Show();
		}
		else
		{
			//LoadInterstitialAd(); //광고 재로드
		}
	}



	private void RegisterEventHandlers(InterstitialAd ad) //광고 이벤트
	{
		ad.OnAdFullScreenContentClosed += () =>
		{
			Debug.Log("광고를 닫았을 시.... ");          // 광고를 닫았을 시.... 

			AudioMng.ins.PlayEffect("Meow");
			Main.ins.PressButton(mini_Int);


			interstitialAd.Destroy();
		};
	}

	void ResetAdmob()      // 광고 초기화
	{
		MobileAds.Initialize((InitializationStatus initStatus) =>
		{
			//초기화 완료
		});
	}


	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[SerializeField] GameObject Mini_Panel;         // 미니 게임 패널
	[SerializeField] GameObject Closed_Panel;       // 광고 닫기 누를떄 패널 가릴 패널

	[SerializeField] GameObject Option_Panel;       // 옵션 패널
	[SerializeField] GameObject Press_Option_Panel;       // 옵션 뒷 패널

	[SerializeField] GameObject Play_Panel;         // 플레이 패널
	[SerializeField] GameObject Press_Panel;        // 뒷 패널 

	[SerializeField] GameObject LeaderBoard_Panel;       // 리더보드 패널
	[SerializeField] GameObject Press_LeaderBoard_Panel;       // 리더보드 뒷패널


	[SerializeField] Image play_Image;
	[SerializeField] Sprite[] Sprite_MiniGame;        // 미니게임 인게임 스프라이트 배열


	[SerializeField] TextMeshProUGUI socreText;     // 최대 스코어를 나타낼 텍스트(리본 아래)


	[SerializeField] GameObject[] Guide_Array;

	[SerializeField] Sprite[] Ope_Sprite;         // 사칙연산 스프라이트        0 : 플러스, 1 : 마이너스, 2 : 곱하기
	[SerializeField] Sprite[] OX_Sprite;          // OX 스프라이트              0 : O, 1 : X
	[SerializeField] Sprite[] Guide_Sprite;       // 나머지 가이드 스프라이트    

	// 0 : 도넛, 1 : 캐릭터, 2 : 시계, 3 : 미니01 물, 4 : 검, 5 : 회전, 6 : 도넛 o, 7 : 도넛 x, 8 : 몬스터, 9 : 초록 연기
	// 10 타워: , 11 : 도넛 사라지기, 12 : 캔 날라가기, 13 : 성벽, 14 : 파이어볼, 15 : 마녀, 16 : 화난 곰, 17 : 깨진 꿀통, 18 : 꿀통, 19 : 벌
	// 20 : 숫자 7, 21 : ?(오브젝트), 22 : 유령, 23 : 숫자 60, 24 : 땅, 25 : 비행거리, 26 : 미니10 도착지, 27 : 추락, 28 : 비행기 , 29 : 

	[SerializeField] Image[] Ex_Sprite_01;        // 설명 패널 이미지 배열
	[SerializeField] Image[] Ex_Sprite_02;
	[SerializeField] Image[] Ex_Sprite_03;
	[SerializeField] Image[] Ex_Sprite_04;



	[SerializeField] Image[] graphic_Array;          // 0 : low, 1 : mid, 2 : high
	[SerializeField] Sprite[] graphic_OnOff;         // 0 : On, 1 : Off


	[SerializeField] Slider BackGround_Slider;              // 배경음 슬라이더
	[SerializeField] Slider Effect_Slider;                  // 효과음 슬라이더

	[SerializeField] TextMeshProUGUI BackGround_Text;       // 배경음 숫자받는 변수
	[SerializeField] TextMeshProUGUI Effect_Text;           // 효과음 숫자받는 변수


	int graphic_Int = 0;    // 현재 그래픽이 몇인가?


	void Awake()
	{
		Time.timeScale = 1;
	}

	void Start()
	{
		AudioMng.ins.Play_BG("Main");


		BackGround_Slider.value = AudioMng.ins.GetBackGroundVolume();
		Effect_Slider.value = AudioMng.ins.GetEffectVolume();

		Effect_Text.text = (Effect_Slider.value * 100).ToString("N0");


		if (Main.ins.nowPlayer.qualityLevel.Equals(0))     // 그래픽이 0이라면...
		{
			graphic_Array[0].sprite = graphic_OnOff[0];    // 온으로 이미지 바꿈

			graphic_Array[1].sprite = graphic_OnOff[1];    // 오프으로 이미지 바꿈
			graphic_Array[2].sprite = graphic_OnOff[1];
		}
		else if (Main.ins.nowPlayer.qualityLevel.Equals(1))     // 그래픽이 1이라면...
		{
			graphic_Array[1].sprite = graphic_OnOff[0];    // 온으로 이미지 바꿈

			graphic_Array[0].sprite = graphic_OnOff[1];    // 오프으로 이미지 바꿈
			graphic_Array[2].sprite = graphic_OnOff[1];
		}
		else               // 그래픽이 2이라면...
		{
			graphic_Array[2].sprite = graphic_OnOff[0];    // 온으로 이미지 바꿈

			graphic_Array[0].sprite = graphic_OnOff[1];    // 오프으로 이미지 바꿈
			graphic_Array[1].sprite = graphic_OnOff[1];
		}




		//adUnitId = "ca-app-pub-3940256099942544/1033173712";   // 테스트용 전면
		//adUnitId = "ca-app-pub-8055963595822882/4583813111";   // 전면광고(이전꺼)
		//adUnitId = "ca-app-pub-4231934681802344/5067718171";   // 전면광고(현재꺼)

		//ResetAdmob();                     
		//LoadInterstitialAd();

	}





	public void Press_01()              // 1칸, GameStart
	{
		Mini_Panel.SetActive(true);

		AudioMng.ins.PlayEffect("Click");
	}

	public void Press_02()              // 2칸, Option
	{
		Option_Panel.SetActive(true);
		Press_Option_Panel.SetActive(true);

		AudioMng.ins.PlayEffect("Click02");
	}

	public void Press_03()              // 3칸, Game End
	{

		AudioMng.ins.Pause_BG();
		AudioMng.ins.PlayEffect("Fail02");

		Application.Quit();
	}

	public void Press_04()                   // 리더보드 클릭 버튼(오른쪽)
	{
		LeaderBoard_Panel.SetActive(true);
		Press_LeaderBoard_Panel.SetActive(true);

		AudioMng.ins.PlayEffect("Click02");
	}




	public void Press_BackButton(int num)           // 빽버튼 함수
	{
		AudioMng.ins.PlayEffect("Back");

		if (num.Equals(0))                   // 미니 게임 패널
		{
			Mini_Panel.SetActive(false);
		}
		else if (num.Equals(1))              // 플레이 패널(최종)
		{
			Press_Panel.SetActive(false);
			Play_Panel.SetActive(false);
		}
		else if (num.Equals(2))              // 옵션 패널에서 뺵 버튼
		{
			Option_Panel.SetActive(false);
			Press_Option_Panel.SetActive(false);

			Main.ins.nowPlayer.qualityLevel = graphic_Int;

			Main.ins.nowPlayer.Volume_BackGround = AudioMng.ins.GetBackGroundVolume();
			Main.ins.nowPlayer.Volume_Effect = AudioMng.ins.GetEffectVolume();
			Main.ins.SaveData();      // 세이브 함수
		}
		else                                 // 리더보드 뺵버튼
		{
			LeaderBoard_Panel.SetActive(false);
			Press_LeaderBoard_Panel.SetActive(false);
		}
	}




	public void Press_Graphic(int num)           // 그래픽 0 : low, 1 : mid, 2 : high
	{
		AudioMng.ins.PlayEffect("Click02");

		if (num.Equals(0))        // 0 : low
		{
			QualitySettings.SetQualityLevel(0);
			Main.ins.nowPlayer.qualityLevel = 0;

			graphic_Int = 0;

			graphic_Array[0].sprite = graphic_OnOff[0];    // 온으로 이미지 바꿈

			graphic_Array[1].sprite = graphic_OnOff[1];    // 오프으로 이미지 바꿈
			graphic_Array[2].sprite = graphic_OnOff[1];
		}
		else if (num.Equals(1))     // 1 : mid
		{
			QualitySettings.SetQualityLevel(1);
			Main.ins.nowPlayer.qualityLevel = 1;

			graphic_Int = 1;

			graphic_Array[1].sprite = graphic_OnOff[0];    // 온으로 이미지 바꿈

			graphic_Array[0].sprite = graphic_OnOff[1];    // 오프으로 이미지 바꿈
			graphic_Array[2].sprite = graphic_OnOff[1];
		}
		else                        // 2 : high
		{
			QualitySettings.SetQualityLevel(2);
			Main.ins.nowPlayer.qualityLevel = 2;

			graphic_Int = 2;

			graphic_Array[2].sprite = graphic_OnOff[0];    // 온으로 이미지 바꿈

			graphic_Array[0].sprite = graphic_OnOff[1];    // 오프으로 이미지 바꿈
			graphic_Array[1].sprite = graphic_OnOff[1];
		}
	}





	public void Change_Slider_BG()            // 배경음악 오디오 볼륨
	{
		float BG_Float = BackGround_Slider.value;

		BackGround_Text.text = (BG_Float * 100).ToString("N0");

		AudioMng.ins.SetBackGroundVolume(BackGround_Slider.value);
	}


	public void Change_Slider_EffectText()
	{
		float Effect_Float = Effect_Slider.value;

		Effect_Text.text = (Effect_Float * 100).ToString("N0");
	}

	public void Change_Slider_Effect()            // 효과음 오디오 볼륨
	{
		float Effect_Float = Effect_Slider.value;

		//Effect_Text.text = (Effect_Float * 100).ToString("N0");

		
		AudioMng.ins.SetEffectVolume(Effect_Slider.value);
		AudioMng.ins.PlayEffect("Click");
	}




	int mini_Int = 0;       // 플레이 버튼을 눌렀다면 숫자 넣기

	public void Press_Check(int num)            // 미니 패널에서 버튼을 클릭했으면...
	{
		Press_Panel.SetActive(true);            // 뒤에 배경 패널 활성화
		Play_Panel.SetActive(true);             // 플레이 설명 패널 활성화

		AudioMng.ins.PlayEffect("Click03");     


		switch (num)        // 미니 게임에 따라
		{
			case 0:            // 미니 01
				mini_Int = 0;        // 미니 게임 01을 눌렀다고 알린다.

				Guide_ImageChange(4, 0);       // Play_Penel 켜질떄 이미지 바꾸기(슬라이더 이미지 배열01, 사용 설명서 4개, 미니게임01)

				play_Image.sprite = Sprite_MiniGame[0];
				socreText.text = Main.ins.nowPlayer.maxScore_List[0].ToString();  // 미니 게임01 최고 스코어
				break;
			case 1:            // 미니 02
				mini_Int = 1;        // 미니 게임 02을 눌렀다고 알린다.

				Guide_ImageChange(2, 1);       // Play_Penel 켜질떄 이미지 바꾸기(슬라이더 이미지 배열02, 사용 설명서 2개, 미니게임02)

				play_Image.sprite = Sprite_MiniGame[1];
				socreText.text = Main.ins.nowPlayer.maxScore_List[1].ToString();  // 미니 게임02 최고 스코어
				break;
			case 2:            // 미니 03
				mini_Int = 10;        // 미니 게임 03을 눌렀다고 알린다.

				Guide_ImageChange(4, 2);       // Play_Penel 켜질떄 이미지 바꾸기(슬라이더 이미지 배열03, 사용 설명서 4개, 미니게임03)

				play_Image.sprite = Sprite_MiniGame[2];
				socreText.text = Main.ins.nowPlayer.maxScore_List[2].ToString();  // 미니 게임03 최고 스코어
				break;
			case 3:            // 미니 04
				mini_Int = 2;        // 미니 게임 04을 눌렀다고 알린다.

				Guide_ImageChange(2, 3);       // Play_Penel 켜질떄 이미지 바꾸기(슬라이더 이미지 배열04, 사용 설명서 2개, 미니게임04)

				play_Image.sprite = Sprite_MiniGame[3];
				socreText.text = Main.ins.nowPlayer.maxScore_List[3].ToString();  // 미니 게임04 최고 스코어
				break;
			case 4:            // 미니 05
				mini_Int = 3;        // 미니 게임 05을 눌렀다고 알린다.

				Guide_ImageChange(3, 4);       // Play_Penel 켜질떄 이미지 바꾸기(슬라이더 이미지 배열05, 사용 설명서 3개, 미니게임05)

				play_Image.sprite = Sprite_MiniGame[4];
				socreText.text = Main.ins.nowPlayer.maxScore_List[4].ToString();  // 미니 게임05 최고 스코어
				break;
			case 5:            // 미니 06
				mini_Int = 4;        // 미니 게임 06을 눌렀다고 알린다.

				Guide_ImageChange(4, 5);       // Play_Penel 켜질떄 이미지 바꾸기(슬라이더 이미지 배열06, 사용 설명서 4개, 미니게임06)

				play_Image.sprite = Sprite_MiniGame[5];
				socreText.text = Main.ins.nowPlayer.maxScore_List[5].ToString();  // 미니 게임06 최고 스코어
				break;
			case 6:            // 미니 07
				mini_Int = 5;        // 미니 게임 07을 눌렀다고 알린다.

				Guide_ImageChange(2, 6);       // Play_Penel 켜질떄 이미지 바꾸기(슬라이더 이미지 배열07, 사용 설명서 2개, 미니게임07)

				play_Image.sprite = Sprite_MiniGame[6];
				socreText.text = Main.ins.nowPlayer.maxScore_List[6].ToString();  // 미니 게임07 최고 스코어
				break;
			case 7:            // 미니 08
				mini_Int = 6;        // 미니 게임 08을 눌렀다고 알린다.

				Guide_ImageChange(3, 7);       // Play_Penel 켜질떄 이미지 바꾸기(슬라이더 이미지 배열08, 사용 설명서 3개, 미니게임08)

				play_Image.sprite = Sprite_MiniGame[7];
				socreText.text = Main.ins.nowPlayer.maxScore_List[7].ToString();  // 미니 게임08 최고 스코어
				break;
			case 8:            // 미니 09
				mini_Int = 7;        // 미니 게임 09을 눌렀다고 알린다.

				Guide_ImageChange(3, 8);       // Play_Penel 켜질떄 이미지 바꾸기(슬라이더 이미지 배열09, 사용 설명서 3개, 미니게임09)

				play_Image.sprite = Sprite_MiniGame[8];
				socreText.text = Main.ins.nowPlayer.maxScore_List[8].ToString();  // 미니 게임09 최고 스코어
				break;
			case 9:            // 미니 10
				mini_Int = 8;        // 미니 게임 10을 눌렀다고 알린다.

				Guide_ImageChange(4, 9);       // Play_Penel 켜질떄 이미지 바꾸기(슬라이더 이미지 배열10, 사용 설명서 4개, 미니게임10)

				play_Image.sprite = Sprite_MiniGame[9];
				socreText.text = Main.ins.nowPlayer.maxScore_List[9].ToString();  // 미니 게임10 최고 스코어
				break;
		}
	}


	void Guide_ImageChange(int contentNum, int MiniNum)       // Play_Penel 켜질떄 이미지 바꾸기
	{

		for (int i = 0; i < 4; i++)      
		{
			Guide_Array[i].SetActive(false);                       // 미니게임마다 사용 패널 개수가 달라서 일단 다 끔
		}


		for (int i = 0; i < contentNum; i++)
		{
			Guide_Array[i].SetActive(true);             // 미니게임에 맞게 사용 패널 개수를 활성화 함
		}


		switch (MiniNum)      // 미니게임 버튼을 무엇을 눌렀느냐애 따라
		{
			case 0:        // 4          캐릭터 + 도넛 = O                        미니게임01
				Ex_Sprite_01[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[0];   // 도넛
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       캐릭터 + 검 = x
				Ex_Sprite_02[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[4];   // 검
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // x

				//                       캐릭터 + 물 = x
				Ex_Sprite_03[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_03[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_03[2].sprite = Guide_Sprite[3];   // 물
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // x

				//                       캐릭터 + 트랩 = x
				Ex_Sprite_04[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_04[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_04[2].sprite = Guide_Sprite[5];   // 트랩
				Ex_Sprite_04[3].sprite = OX_Sprite[1];      // x

				break;
			case 1:        // 2          도넛O + 몬스터 = o                                          미니게임02
				Ex_Sprite_01[0].sprite = Guide_Sprite[6];   // 도넛O
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[29];   // 몬스터
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // o

				//                       도넛X + 몬스터가 오른쪽으로 지나감 = x
				Ex_Sprite_02[0].sprite = Guide_Sprite[7];   // 도넛X
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[8];   // 몬스터가 오른쪽으로 지나감      ????????????????????????????????????????
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // x


				break;
			case 2:        // 4          캐릭터 + 초록색연기 = o                                          미니게임03
				Ex_Sprite_01[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[9];   // 초록색연기
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // o

				//                       캐릭터 + 몬스터 = x
				Ex_Sprite_02[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[29];   // 몬스터
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // x

				//                       캐릭터 + 타워불빛(하늘) = x
				Ex_Sprite_03[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_03[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_03[2].sprite = Guide_Sprite[10];   // 타워불빛(하늘)
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // x

				//                       캐릭터 - 시계 = X
				Ex_Sprite_04[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_04[1].sprite = Ope_Sprite[1];     // -
				Ex_Sprite_04[2].sprite = Guide_Sprite[2];   // 시계
				Ex_Sprite_04[3].sprite = OX_Sprite[1];      // X



				break;
			case 3:        // 2          날아가는캔 + 도넛 = o                                          미니게임04
				Ex_Sprite_01[0].sprite = Guide_Sprite[12];   // 날아가는캔
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[0];   // 도넛
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // o

				//                       날아가는캔 + 도넛없음 = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[12];   // 날아가는캔
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[7];   // 도넛없음
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X



				break;
			case 4:        // 3          캐릭터 x 시계 = O                                          미니게임05
				Ex_Sprite_01[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_01[1].sprite = Ope_Sprite[2];     // x
				Ex_Sprite_01[2].sprite = Guide_Sprite[2];   // 시계
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       성벽 + 검 = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[13];   // 성벽
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[4];   // 검
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X

				//                       마녀 + 파이어볼 = X
				Ex_Sprite_03[0].sprite = Guide_Sprite[15];   // 마녀
				Ex_Sprite_03[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_03[2].sprite = Guide_Sprite[14];   // 파이어볼
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // X


				break;
			case 5:        // 4          꿀통 x 시계 = O                                          미니게임06
				Ex_Sprite_01[0].sprite = Guide_Sprite[18];   // 꿀통
				Ex_Sprite_01[1].sprite = Ope_Sprite[2];     // x
				Ex_Sprite_01[2].sprite = Guide_Sprite[2];   // 시계
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       캐릭터 + 화난 곰 = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[16];   // 화난 곰
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X

				//                       캐릭터 + 벌 = X
				Ex_Sprite_03[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_03[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_03[2].sprite = Guide_Sprite[19];   // 벌
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // X

				//                       파괴된 꿀통 x 7 = X
				Ex_Sprite_04[0].sprite = Guide_Sprite[17];   // 파괴된 꿀통
				Ex_Sprite_04[1].sprite = Ope_Sprite[2];     // x
				Ex_Sprite_04[2].sprite = Guide_Sprite[20];   // 7
				Ex_Sprite_04[3].sprite = OX_Sprite[1];      // X



				break;
			case 6:        // 2          캐릭터 + 도넛 = O                                          미니게임07
				Ex_Sprite_01[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[0];   // 도넛
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       캐릭터 + 장애물 (검정색사각형 물음표) = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[21];   // 장애물 (검정색사각형 물음표)
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X



				break;
			case 7:        // 3          캐릭터 X 시계 = O                                          미니게임08
				Ex_Sprite_01[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_01[1].sprite = Ope_Sprite[2];     // X
				Ex_Sprite_01[2].sprite = Guide_Sprite[2];   // 시계
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       캐릭터 + 유령 = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[22];   // 유령
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X

				//                       유령 X 60 = X
				Ex_Sprite_03[0].sprite = Guide_Sprite[22];   // 유령
				Ex_Sprite_03[1].sprite = Ope_Sprite[2];     // X
				Ex_Sprite_03[2].sprite = Guide_Sprite[23];   // 60
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // X



				break;
			case 8:        // 3          도넛 + 비행거리 = O                                          미니게임09
				Ex_Sprite_01[0].sprite = Guide_Sprite[0];   // 도넛
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[25];   // 비행거리
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       비행기 + 땅 = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[28];   // 비행기
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[24];   // 땅
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X

				//                       캐릭터 + 땅 = X
				Ex_Sprite_03[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_03[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_03[2].sprite = Guide_Sprite[24];   // 땅
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // X



				break;
			case 9:        // 4          캐릭터 + 도착지 = O                                          미니게임10
				Ex_Sprite_01[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[26];   // 도착지
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       캐릭터 + 추락 = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[27];   // 추락
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X

				//                       캐릭터 + 장애물 = X
				Ex_Sprite_03[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_03[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_03[2].sprite = Guide_Sprite[21];   // 장애물
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // X

				//                       캐릭터 - 시계 = X
				Ex_Sprite_04[0].sprite = Guide_Sprite[1];   // 캐릭터
				Ex_Sprite_04[1].sprite = Ope_Sprite[1];     // -
				Ex_Sprite_04[2].sprite = Guide_Sprite[2];   // 시계
				Ex_Sprite_04[3].sprite = OX_Sprite[1];      // X



				break;
		}
	}



	public void Press_Button()        // 미니 게임 플레이 버튼
    {
		Closed_Panel.SetActive(true);

		//int randInt = Random.Range(0, 2);
		//randInt = 1;

		//if (randInt.Equals(0))
		//{
		//	ShowAd();
		//}
		//else
		//{
		//	AudioMng.ins.PlayEffect("Meow");                 
		//	Main.ins.PressButton(mini_Int);
		//}


		AudioMng.ins.PlayEffect("Meow");
		Main.ins.PressButton(mini_Int);
	}



	public void Press_URLButton()     // 노 광고 어플 버튼
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.KBJH.TenGamesParty_Pro");
	}

}


