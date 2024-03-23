
using UnityEngine;
using GoogleMobileAds.Api;

public class BottomBanner : MonoBehaviour
{
	string adUnitId;

	BannerView _bannerView;

	public void Start()
	{
		MobileAds.Initialize((InitializationStatus initStatus) =>
		{
			//초기화 완료
		});

		//adUnitId = "ca-app-pub-3940256099942544/6300978111";     // 테스트용 배너
		//adUnitId = "ca-app-pub-8055963595822882/3533675071";       // 배너광고(이전꺼)  
		//adUnitId = "ca-app-pub-4231934681802344/7904854256";       // 배너광고(현재꺼)

		LoadAd();
	}

	public void OnDisable()         // 메인씬이 꺼지면..
	{
		DestroyAd();

	}

	public void LoadAd() //광고 로드
	{
		if (_bannerView == null)
		{
			CreateBannerView();
		}
		var adRequest = new AdRequest.Builder().Build();

		_bannerView.LoadAd(adRequest);
	}

	public void CreateBannerView() //광고 보여주기
	{

		if (_bannerView != null)
		{
			DestroyAd();
		}

		_bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

		//_bannerView = new BannerView(_adUnitId, AdSize.Banner, 0, 50);
	}



	public void DestroyAd() //광고 제거
	{
		if (_bannerView != null)
		{
			_bannerView.Destroy();
			_bannerView = null;
		}
	}
}



















