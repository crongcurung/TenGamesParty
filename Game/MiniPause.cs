using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class MiniPause : MonoBehaviour
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

	void ResetAdmob()      // 광고 초기화
	{
		MobileAds.Initialize((InitializationStatus initStatus) =>
		{
			//초기화 완료
		});
	}


	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


	void Start()
	{
		//adUnitId = "ca-app-pub-3940256099942544/1033173712";   // 테스트용 전면
		//adUnitId = "ca-app-pub-8055963595822882/4583813111";   // 전면광고(이전꺼)
		//adUnitId = "ca-app-pub-4231934681802344/5067718171";   // 전면광고(현재꺼)

		//ResetAdmob();
		//LoadInterstitialAd();
	}



	[SerializeField] GameObject Closed_Panel;       // 광고 닫기 누를떄 패널 가릴 패널


	[SerializeField] GameObject Pause_Panel;        // 일시정지 패널
    [SerializeField] GameObject Game_Panel;         // 반투명 패널
    [SerializeField] GameObject Wanning_Panel;      // 한 번 더 묻는 패널



    public void Press_Pause()              // 일시정지 버튼을 눌렀을 때...
    {
        Pause_Panel.SetActive(true);
        Game_Panel.SetActive(true);

        AudioMng.ins.LoopEffect(false);
        AudioMng.ins.PlayEffect("Click");

        AudioMng.ins.Pause_BG();

        Time.timeScale = 0;
    }


    public void Press_ToMain()              // 일시 정지 패널에서 메인으로 가는 버튼을 눌렀을 경우..
    {
        AudioMng.ins.PlayEffect("Click");
        Wanning_Panel.SetActive(true);
    }

	bool isBool = false;


	public void Press_OneMore(bool isMini03)             // 일시 정지 패널에서 한 번 더 하는 버튼을 눌렀을 경우...
    {
        AudioMng.ins.StopEffect();
        AudioMng.ins.Stop_BG();

		isBool = isMini03;

		Closed_Panel.SetActive(true);

		//int randInt = Random.Range(0, 2);

		//if (randInt.Equals(0))                             // 광고 버전
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



		if (isMini03.Equals(false))                      // 프로 버전
		{
			Main.ins.LoadScene("MiniGame");
		}
		else
		{
			Main.ins.LoadScene("Mini_03");
		}
	}

    public void Press_ReturnGame()          // 일시 정지 패널에서 다시 되돌아가는 버튼을 눌렀을 경우...
    {
        Time.timeScale = 1;

        AudioMng.ins.PlayEffect("Back");

        AudioMng.ins.UnPause_BG();

        Pause_Panel.SetActive(false);
        Game_Panel.SetActive(false);
    }





    public void Press_Yes()        // 한 번 더 묻는 패널에서 yes를 누른다면..
    {
		SceneManager.LoadScene("Main");
    }

    public void Press_No()        // 한 번 더 묻는 패널에서 no를 누른다면..
    {
        AudioMng.ins.PlayEffect("Back");
        Wanning_Panel.SetActive(false);
    }

}
