
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
			//�ʱ�ȭ �Ϸ�
		});

		//adUnitId = "ca-app-pub-3940256099942544/6300978111";     // �׽�Ʈ�� ���
		//adUnitId = "ca-app-pub-8055963595822882/3533675071";       // ��ʱ���(������)  
		//adUnitId = "ca-app-pub-4231934681802344/7904854256";       // ��ʱ���(���粨)

		LoadAd();
	}

	public void OnDisable()         // ���ξ��� ������..
	{
		DestroyAd();

	}

	public void LoadAd() //���� �ε�
	{
		if (_bannerView == null)
		{
			CreateBannerView();
		}
		var adRequest = new AdRequest.Builder().Build();

		_bannerView.LoadAd(adRequest);
	}

	public void CreateBannerView() //���� �����ֱ�
	{

		if (_bannerView != null)
		{
			DestroyAd();
		}

		_bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

		//_bannerView = new BannerView(_adUnitId, AdSize.Banner, 0, 50);
	}



	public void DestroyAd() //���� ����
	{
		if (_bannerView != null)
		{
			_bannerView.Destroy();
			_bannerView = null;
		}
	}
}



















