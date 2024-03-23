using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoogleMobileAds.Api;


public class MainController : MonoBehaviour
{
	string adUnitId;      // ���� ���� ���̵�

	private InterstitialAd interstitialAd;

	public void LoadInterstitialAd() //���� �ε�
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

				RegisterEventHandlers(interstitialAd); //�̺�Ʈ ���
			});
	}

	public void ShowAd()                //���� ����
	{
		if (interstitialAd != null && interstitialAd.CanShowAd())
		{
			interstitialAd.Show();
		}
		else
		{
			//LoadInterstitialAd(); //���� ��ε�
		}
	}



	private void RegisterEventHandlers(InterstitialAd ad) //���� �̺�Ʈ
	{
		ad.OnAdFullScreenContentClosed += () =>
		{
			Debug.Log("���� �ݾ��� ��.... ");          // ���� �ݾ��� ��.... 

			AudioMng.ins.PlayEffect("Meow");
			Main.ins.PressButton(mini_Int);


			interstitialAd.Destroy();
		};
	}

	void ResetAdmob()      // ���� �ʱ�ȭ
	{
		MobileAds.Initialize((InitializationStatus initStatus) =>
		{
			//�ʱ�ȭ �Ϸ�
		});
	}


	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[SerializeField] GameObject Mini_Panel;         // �̴� ���� �г�
	[SerializeField] GameObject Closed_Panel;       // ���� �ݱ� ������ �г� ���� �г�

	[SerializeField] GameObject Option_Panel;       // �ɼ� �г�
	[SerializeField] GameObject Press_Option_Panel;       // �ɼ� �� �г�

	[SerializeField] GameObject Play_Panel;         // �÷��� �г�
	[SerializeField] GameObject Press_Panel;        // �� �г� 

	[SerializeField] GameObject LeaderBoard_Panel;       // �������� �г�
	[SerializeField] GameObject Press_LeaderBoard_Panel;       // �������� ���г�


	[SerializeField] Image play_Image;
	[SerializeField] Sprite[] Sprite_MiniGame;        // �̴ϰ��� �ΰ��� ��������Ʈ �迭


	[SerializeField] TextMeshProUGUI socreText;     // �ִ� ���ھ ��Ÿ�� �ؽ�Ʈ(���� �Ʒ�)


	[SerializeField] GameObject[] Guide_Array;

	[SerializeField] Sprite[] Ope_Sprite;         // ��Ģ���� ��������Ʈ        0 : �÷���, 1 : ���̳ʽ�, 2 : ���ϱ�
	[SerializeField] Sprite[] OX_Sprite;          // OX ��������Ʈ              0 : O, 1 : X
	[SerializeField] Sprite[] Guide_Sprite;       // ������ ���̵� ��������Ʈ    

	// 0 : ����, 1 : ĳ����, 2 : �ð�, 3 : �̴�01 ��, 4 : ��, 5 : ȸ��, 6 : ���� o, 7 : ���� x, 8 : ����, 9 : �ʷ� ����
	// 10 Ÿ��: , 11 : ���� �������, 12 : ĵ ���󰡱�, 13 : ����, 14 : ���̾, 15 : ����, 16 : ȭ�� ��, 17 : ���� ����, 18 : ����, 19 : ��
	// 20 : ���� 7, 21 : ?(������Ʈ), 22 : ����, 23 : ���� 60, 24 : ��, 25 : ����Ÿ�, 26 : �̴�10 ������, 27 : �߶�, 28 : ����� , 29 : 

	[SerializeField] Image[] Ex_Sprite_01;        // ���� �г� �̹��� �迭
	[SerializeField] Image[] Ex_Sprite_02;
	[SerializeField] Image[] Ex_Sprite_03;
	[SerializeField] Image[] Ex_Sprite_04;



	[SerializeField] Image[] graphic_Array;          // 0 : low, 1 : mid, 2 : high
	[SerializeField] Sprite[] graphic_OnOff;         // 0 : On, 1 : Off


	[SerializeField] Slider BackGround_Slider;              // ����� �����̴�
	[SerializeField] Slider Effect_Slider;                  // ȿ���� �����̴�

	[SerializeField] TextMeshProUGUI BackGround_Text;       // ����� ���ڹ޴� ����
	[SerializeField] TextMeshProUGUI Effect_Text;           // ȿ���� ���ڹ޴� ����


	int graphic_Int = 0;    // ���� �׷����� ���ΰ�?


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


		if (Main.ins.nowPlayer.qualityLevel.Equals(0))     // �׷����� 0�̶��...
		{
			graphic_Array[0].sprite = graphic_OnOff[0];    // ������ �̹��� �ٲ�

			graphic_Array[1].sprite = graphic_OnOff[1];    // �������� �̹��� �ٲ�
			graphic_Array[2].sprite = graphic_OnOff[1];
		}
		else if (Main.ins.nowPlayer.qualityLevel.Equals(1))     // �׷����� 1�̶��...
		{
			graphic_Array[1].sprite = graphic_OnOff[0];    // ������ �̹��� �ٲ�

			graphic_Array[0].sprite = graphic_OnOff[1];    // �������� �̹��� �ٲ�
			graphic_Array[2].sprite = graphic_OnOff[1];
		}
		else               // �׷����� 2�̶��...
		{
			graphic_Array[2].sprite = graphic_OnOff[0];    // ������ �̹��� �ٲ�

			graphic_Array[0].sprite = graphic_OnOff[1];    // �������� �̹��� �ٲ�
			graphic_Array[1].sprite = graphic_OnOff[1];
		}




		//adUnitId = "ca-app-pub-3940256099942544/1033173712";   // �׽�Ʈ�� ����
		//adUnitId = "ca-app-pub-8055963595822882/4583813111";   // ���鱤��(������)
		//adUnitId = "ca-app-pub-4231934681802344/5067718171";   // ���鱤��(���粨)

		//ResetAdmob();                     
		//LoadInterstitialAd();

	}





	public void Press_01()              // 1ĭ, GameStart
	{
		Mini_Panel.SetActive(true);

		AudioMng.ins.PlayEffect("Click");
	}

	public void Press_02()              // 2ĭ, Option
	{
		Option_Panel.SetActive(true);
		Press_Option_Panel.SetActive(true);

		AudioMng.ins.PlayEffect("Click02");
	}

	public void Press_03()              // 3ĭ, Game End
	{

		AudioMng.ins.Pause_BG();
		AudioMng.ins.PlayEffect("Fail02");

		Application.Quit();
	}

	public void Press_04()                   // �������� Ŭ�� ��ư(������)
	{
		LeaderBoard_Panel.SetActive(true);
		Press_LeaderBoard_Panel.SetActive(true);

		AudioMng.ins.PlayEffect("Click02");
	}




	public void Press_BackButton(int num)           // ����ư �Լ�
	{
		AudioMng.ins.PlayEffect("Back");

		if (num.Equals(0))                   // �̴� ���� �г�
		{
			Mini_Panel.SetActive(false);
		}
		else if (num.Equals(1))              // �÷��� �г�(����)
		{
			Press_Panel.SetActive(false);
			Play_Panel.SetActive(false);
		}
		else if (num.Equals(2))              // �ɼ� �гο��� �� ��ư
		{
			Option_Panel.SetActive(false);
			Press_Option_Panel.SetActive(false);

			Main.ins.nowPlayer.qualityLevel = graphic_Int;

			Main.ins.nowPlayer.Volume_BackGround = AudioMng.ins.GetBackGroundVolume();
			Main.ins.nowPlayer.Volume_Effect = AudioMng.ins.GetEffectVolume();
			Main.ins.SaveData();      // ���̺� �Լ�
		}
		else                                 // �������� ����ư
		{
			LeaderBoard_Panel.SetActive(false);
			Press_LeaderBoard_Panel.SetActive(false);
		}
	}




	public void Press_Graphic(int num)           // �׷��� 0 : low, 1 : mid, 2 : high
	{
		AudioMng.ins.PlayEffect("Click02");

		if (num.Equals(0))        // 0 : low
		{
			QualitySettings.SetQualityLevel(0);
			Main.ins.nowPlayer.qualityLevel = 0;

			graphic_Int = 0;

			graphic_Array[0].sprite = graphic_OnOff[0];    // ������ �̹��� �ٲ�

			graphic_Array[1].sprite = graphic_OnOff[1];    // �������� �̹��� �ٲ�
			graphic_Array[2].sprite = graphic_OnOff[1];
		}
		else if (num.Equals(1))     // 1 : mid
		{
			QualitySettings.SetQualityLevel(1);
			Main.ins.nowPlayer.qualityLevel = 1;

			graphic_Int = 1;

			graphic_Array[1].sprite = graphic_OnOff[0];    // ������ �̹��� �ٲ�

			graphic_Array[0].sprite = graphic_OnOff[1];    // �������� �̹��� �ٲ�
			graphic_Array[2].sprite = graphic_OnOff[1];
		}
		else                        // 2 : high
		{
			QualitySettings.SetQualityLevel(2);
			Main.ins.nowPlayer.qualityLevel = 2;

			graphic_Int = 2;

			graphic_Array[2].sprite = graphic_OnOff[0];    // ������ �̹��� �ٲ�

			graphic_Array[0].sprite = graphic_OnOff[1];    // �������� �̹��� �ٲ�
			graphic_Array[1].sprite = graphic_OnOff[1];
		}
	}





	public void Change_Slider_BG()            // ������� ����� ����
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

	public void Change_Slider_Effect()            // ȿ���� ����� ����
	{
		float Effect_Float = Effect_Slider.value;

		//Effect_Text.text = (Effect_Float * 100).ToString("N0");

		
		AudioMng.ins.SetEffectVolume(Effect_Slider.value);
		AudioMng.ins.PlayEffect("Click");
	}




	int mini_Int = 0;       // �÷��� ��ư�� �����ٸ� ���� �ֱ�

	public void Press_Check(int num)            // �̴� �гο��� ��ư�� Ŭ��������...
	{
		Press_Panel.SetActive(true);            // �ڿ� ��� �г� Ȱ��ȭ
		Play_Panel.SetActive(true);             // �÷��� ���� �г� Ȱ��ȭ

		AudioMng.ins.PlayEffect("Click03");     


		switch (num)        // �̴� ���ӿ� ����
		{
			case 0:            // �̴� 01
				mini_Int = 0;        // �̴� ���� 01�� �����ٰ� �˸���.

				Guide_ImageChange(4, 0);       // Play_Penel ������ �̹��� �ٲٱ�(�����̴� �̹��� �迭01, ��� ���� 4��, �̴ϰ���01)

				play_Image.sprite = Sprite_MiniGame[0];
				socreText.text = Main.ins.nowPlayer.maxScore_List[0].ToString();  // �̴� ����01 �ְ� ���ھ�
				break;
			case 1:            // �̴� 02
				mini_Int = 1;        // �̴� ���� 02�� �����ٰ� �˸���.

				Guide_ImageChange(2, 1);       // Play_Penel ������ �̹��� �ٲٱ�(�����̴� �̹��� �迭02, ��� ���� 2��, �̴ϰ���02)

				play_Image.sprite = Sprite_MiniGame[1];
				socreText.text = Main.ins.nowPlayer.maxScore_List[1].ToString();  // �̴� ����02 �ְ� ���ھ�
				break;
			case 2:            // �̴� 03
				mini_Int = 10;        // �̴� ���� 03�� �����ٰ� �˸���.

				Guide_ImageChange(4, 2);       // Play_Penel ������ �̹��� �ٲٱ�(�����̴� �̹��� �迭03, ��� ���� 4��, �̴ϰ���03)

				play_Image.sprite = Sprite_MiniGame[2];
				socreText.text = Main.ins.nowPlayer.maxScore_List[2].ToString();  // �̴� ����03 �ְ� ���ھ�
				break;
			case 3:            // �̴� 04
				mini_Int = 2;        // �̴� ���� 04�� �����ٰ� �˸���.

				Guide_ImageChange(2, 3);       // Play_Penel ������ �̹��� �ٲٱ�(�����̴� �̹��� �迭04, ��� ���� 2��, �̴ϰ���04)

				play_Image.sprite = Sprite_MiniGame[3];
				socreText.text = Main.ins.nowPlayer.maxScore_List[3].ToString();  // �̴� ����04 �ְ� ���ھ�
				break;
			case 4:            // �̴� 05
				mini_Int = 3;        // �̴� ���� 05�� �����ٰ� �˸���.

				Guide_ImageChange(3, 4);       // Play_Penel ������ �̹��� �ٲٱ�(�����̴� �̹��� �迭05, ��� ���� 3��, �̴ϰ���05)

				play_Image.sprite = Sprite_MiniGame[4];
				socreText.text = Main.ins.nowPlayer.maxScore_List[4].ToString();  // �̴� ����05 �ְ� ���ھ�
				break;
			case 5:            // �̴� 06
				mini_Int = 4;        // �̴� ���� 06�� �����ٰ� �˸���.

				Guide_ImageChange(4, 5);       // Play_Penel ������ �̹��� �ٲٱ�(�����̴� �̹��� �迭06, ��� ���� 4��, �̴ϰ���06)

				play_Image.sprite = Sprite_MiniGame[5];
				socreText.text = Main.ins.nowPlayer.maxScore_List[5].ToString();  // �̴� ����06 �ְ� ���ھ�
				break;
			case 6:            // �̴� 07
				mini_Int = 5;        // �̴� ���� 07�� �����ٰ� �˸���.

				Guide_ImageChange(2, 6);       // Play_Penel ������ �̹��� �ٲٱ�(�����̴� �̹��� �迭07, ��� ���� 2��, �̴ϰ���07)

				play_Image.sprite = Sprite_MiniGame[6];
				socreText.text = Main.ins.nowPlayer.maxScore_List[6].ToString();  // �̴� ����07 �ְ� ���ھ�
				break;
			case 7:            // �̴� 08
				mini_Int = 6;        // �̴� ���� 08�� �����ٰ� �˸���.

				Guide_ImageChange(3, 7);       // Play_Penel ������ �̹��� �ٲٱ�(�����̴� �̹��� �迭08, ��� ���� 3��, �̴ϰ���08)

				play_Image.sprite = Sprite_MiniGame[7];
				socreText.text = Main.ins.nowPlayer.maxScore_List[7].ToString();  // �̴� ����08 �ְ� ���ھ�
				break;
			case 8:            // �̴� 09
				mini_Int = 7;        // �̴� ���� 09�� �����ٰ� �˸���.

				Guide_ImageChange(3, 8);       // Play_Penel ������ �̹��� �ٲٱ�(�����̴� �̹��� �迭09, ��� ���� 3��, �̴ϰ���09)

				play_Image.sprite = Sprite_MiniGame[8];
				socreText.text = Main.ins.nowPlayer.maxScore_List[8].ToString();  // �̴� ����09 �ְ� ���ھ�
				break;
			case 9:            // �̴� 10
				mini_Int = 8;        // �̴� ���� 10�� �����ٰ� �˸���.

				Guide_ImageChange(4, 9);       // Play_Penel ������ �̹��� �ٲٱ�(�����̴� �̹��� �迭10, ��� ���� 4��, �̴ϰ���10)

				play_Image.sprite = Sprite_MiniGame[9];
				socreText.text = Main.ins.nowPlayer.maxScore_List[9].ToString();  // �̴� ����10 �ְ� ���ھ�
				break;
		}
	}


	void Guide_ImageChange(int contentNum, int MiniNum)       // Play_Penel ������ �̹��� �ٲٱ�
	{

		for (int i = 0; i < 4; i++)      
		{
			Guide_Array[i].SetActive(false);                       // �̴ϰ��Ӹ��� ��� �г� ������ �޶� �ϴ� �� ��
		}


		for (int i = 0; i < contentNum; i++)
		{
			Guide_Array[i].SetActive(true);             // �̴ϰ��ӿ� �°� ��� �г� ������ Ȱ��ȭ ��
		}


		switch (MiniNum)      // �̴ϰ��� ��ư�� ������ �������ľ� ����
		{
			case 0:        // 4          ĳ���� + ���� = O                        �̴ϰ���01
				Ex_Sprite_01[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[0];   // ����
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       ĳ���� + �� = x
				Ex_Sprite_02[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[4];   // ��
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // x

				//                       ĳ���� + �� = x
				Ex_Sprite_03[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_03[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_03[2].sprite = Guide_Sprite[3];   // ��
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // x

				//                       ĳ���� + Ʈ�� = x
				Ex_Sprite_04[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_04[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_04[2].sprite = Guide_Sprite[5];   // Ʈ��
				Ex_Sprite_04[3].sprite = OX_Sprite[1];      // x

				break;
			case 1:        // 2          ����O + ���� = o                                          �̴ϰ���02
				Ex_Sprite_01[0].sprite = Guide_Sprite[6];   // ����O
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[29];   // ����
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // o

				//                       ����X + ���Ͱ� ���������� ������ = x
				Ex_Sprite_02[0].sprite = Guide_Sprite[7];   // ����X
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[8];   // ���Ͱ� ���������� ������      ????????????????????????????????????????
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // x


				break;
			case 2:        // 4          ĳ���� + �ʷϻ����� = o                                          �̴ϰ���03
				Ex_Sprite_01[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[9];   // �ʷϻ�����
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // o

				//                       ĳ���� + ���� = x
				Ex_Sprite_02[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[29];   // ����
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // x

				//                       ĳ���� + Ÿ���Һ�(�ϴ�) = x
				Ex_Sprite_03[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_03[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_03[2].sprite = Guide_Sprite[10];   // Ÿ���Һ�(�ϴ�)
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // x

				//                       ĳ���� - �ð� = X
				Ex_Sprite_04[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_04[1].sprite = Ope_Sprite[1];     // -
				Ex_Sprite_04[2].sprite = Guide_Sprite[2];   // �ð�
				Ex_Sprite_04[3].sprite = OX_Sprite[1];      // X



				break;
			case 3:        // 2          ���ư���ĵ + ���� = o                                          �̴ϰ���04
				Ex_Sprite_01[0].sprite = Guide_Sprite[12];   // ���ư���ĵ
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[0];   // ����
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // o

				//                       ���ư���ĵ + ���Ӿ��� = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[12];   // ���ư���ĵ
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[7];   // ���Ӿ���
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X



				break;
			case 4:        // 3          ĳ���� x �ð� = O                                          �̴ϰ���05
				Ex_Sprite_01[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_01[1].sprite = Ope_Sprite[2];     // x
				Ex_Sprite_01[2].sprite = Guide_Sprite[2];   // �ð�
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       ���� + �� = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[13];   // ����
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[4];   // ��
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X

				//                       ���� + ���̾ = X
				Ex_Sprite_03[0].sprite = Guide_Sprite[15];   // ����
				Ex_Sprite_03[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_03[2].sprite = Guide_Sprite[14];   // ���̾
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // X


				break;
			case 5:        // 4          ���� x �ð� = O                                          �̴ϰ���06
				Ex_Sprite_01[0].sprite = Guide_Sprite[18];   // ����
				Ex_Sprite_01[1].sprite = Ope_Sprite[2];     // x
				Ex_Sprite_01[2].sprite = Guide_Sprite[2];   // �ð�
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       ĳ���� + ȭ�� �� = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[16];   // ȭ�� ��
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X

				//                       ĳ���� + �� = X
				Ex_Sprite_03[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_03[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_03[2].sprite = Guide_Sprite[19];   // ��
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // X

				//                       �ı��� ���� x 7 = X
				Ex_Sprite_04[0].sprite = Guide_Sprite[17];   // �ı��� ����
				Ex_Sprite_04[1].sprite = Ope_Sprite[2];     // x
				Ex_Sprite_04[2].sprite = Guide_Sprite[20];   // 7
				Ex_Sprite_04[3].sprite = OX_Sprite[1];      // X



				break;
			case 6:        // 2          ĳ���� + ���� = O                                          �̴ϰ���07
				Ex_Sprite_01[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[0];   // ����
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       ĳ���� + ��ֹ� (�������簢�� ����ǥ) = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[21];   // ��ֹ� (�������簢�� ����ǥ)
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X



				break;
			case 7:        // 3          ĳ���� X �ð� = O                                          �̴ϰ���08
				Ex_Sprite_01[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_01[1].sprite = Ope_Sprite[2];     // X
				Ex_Sprite_01[2].sprite = Guide_Sprite[2];   // �ð�
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       ĳ���� + ���� = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[22];   // ����
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X

				//                       ���� X 60 = X
				Ex_Sprite_03[0].sprite = Guide_Sprite[22];   // ����
				Ex_Sprite_03[1].sprite = Ope_Sprite[2];     // X
				Ex_Sprite_03[2].sprite = Guide_Sprite[23];   // 60
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // X



				break;
			case 8:        // 3          ���� + ����Ÿ� = O                                          �̴ϰ���09
				Ex_Sprite_01[0].sprite = Guide_Sprite[0];   // ����
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[25];   // ����Ÿ�
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       ����� + �� = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[28];   // �����
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[24];   // ��
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X

				//                       ĳ���� + �� = X
				Ex_Sprite_03[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_03[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_03[2].sprite = Guide_Sprite[24];   // ��
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // X



				break;
			case 9:        // 4          ĳ���� + ������ = O                                          �̴ϰ���10
				Ex_Sprite_01[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_01[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_01[2].sprite = Guide_Sprite[26];   // ������
				Ex_Sprite_01[3].sprite = OX_Sprite[0];      // O

				//                       ĳ���� + �߶� = X
				Ex_Sprite_02[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_02[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_02[2].sprite = Guide_Sprite[27];   // �߶�
				Ex_Sprite_02[3].sprite = OX_Sprite[1];      // X

				//                       ĳ���� + ��ֹ� = X
				Ex_Sprite_03[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_03[1].sprite = Ope_Sprite[0];     // +
				Ex_Sprite_03[2].sprite = Guide_Sprite[21];   // ��ֹ�
				Ex_Sprite_03[3].sprite = OX_Sprite[1];      // X

				//                       ĳ���� - �ð� = X
				Ex_Sprite_04[0].sprite = Guide_Sprite[1];   // ĳ����
				Ex_Sprite_04[1].sprite = Ope_Sprite[1];     // -
				Ex_Sprite_04[2].sprite = Guide_Sprite[2];   // �ð�
				Ex_Sprite_04[3].sprite = OX_Sprite[1];      // X



				break;
		}
	}



	public void Press_Button()        // �̴� ���� �÷��� ��ư
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



	public void Press_URLButton()     // �� ���� ���� ��ư
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.KBJH.TenGamesParty_Pro");
	}

}


