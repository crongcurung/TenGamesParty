using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class MiniPause : MonoBehaviour
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
			if (isBool.Equals(false))
			{
				Main.ins.LoadScene("MiniGame");
			}
			else
			{
				Main.ins.LoadScene("Mini_03");
			}

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


	void Start()
	{
		//adUnitId = "ca-app-pub-3940256099942544/1033173712";   // �׽�Ʈ�� ����
		//adUnitId = "ca-app-pub-8055963595822882/4583813111";   // ���鱤��(������)
		//adUnitId = "ca-app-pub-4231934681802344/5067718171";   // ���鱤��(���粨)

		//ResetAdmob();
		//LoadInterstitialAd();
	}



	[SerializeField] GameObject Closed_Panel;       // ���� �ݱ� ������ �г� ���� �г�


	[SerializeField] GameObject Pause_Panel;        // �Ͻ����� �г�
    [SerializeField] GameObject Game_Panel;         // ������ �г�
    [SerializeField] GameObject Wanning_Panel;      // �� �� �� ���� �г�



    public void Press_Pause()              // �Ͻ����� ��ư�� ������ ��...
    {
        Pause_Panel.SetActive(true);
        Game_Panel.SetActive(true);

        AudioMng.ins.LoopEffect(false);
        AudioMng.ins.PlayEffect("Click");

        AudioMng.ins.Pause_BG();

        Time.timeScale = 0;
    }


    public void Press_ToMain()              // �Ͻ� ���� �гο��� �������� ���� ��ư�� ������ ���..
    {
        AudioMng.ins.PlayEffect("Click");
        Wanning_Panel.SetActive(true);
    }

	bool isBool = false;


	public void Press_OneMore(bool isMini03)             // �Ͻ� ���� �гο��� �� �� �� �ϴ� ��ư�� ������ ���...
    {
        AudioMng.ins.StopEffect();
        AudioMng.ins.Stop_BG();

		isBool = isMini03;

		Closed_Panel.SetActive(true);

		//int randInt = Random.Range(0, 2);

		//if (randInt.Equals(0))                             // ���� ����
		//{
		//	ShowAd();
		//}
		//else
		//{
		//	if (isMini03.Equals(false))                       
		//	{
		//		Main.ins.LoadScene("MiniGame");
		//	}
		//	else
		//	{
		//		Main.ins.LoadScene("Mini_03");
		//	}
		//}



		if (isMini03.Equals(false))                      // ���� ����
		{
			Main.ins.LoadScene("MiniGame");
		}
		else
		{
			Main.ins.LoadScene("Mini_03");
		}
	}

    public void Press_ReturnGame()          // �Ͻ� ���� �гο��� �ٽ� �ǵ��ư��� ��ư�� ������ ���...
    {
        Time.timeScale = 1;

        AudioMng.ins.PlayEffect("Back");

        AudioMng.ins.UnPause_BG();

        Pause_Panel.SetActive(false);
        Game_Panel.SetActive(false);
    }





    public void Press_Yes()        // �� �� �� ���� �гο��� yes�� �����ٸ�..
    {
		SceneManager.LoadScene("Main");
    }

    public void Press_No()        // �� �� �� ���� �гο��� no�� �����ٸ�..
    {
        AudioMng.ins.PlayEffect("Back");
        Wanning_Panel.SetActive(false);
    }

}
