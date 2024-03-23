using UnityEngine;

public class Mini04_Monster : MonoBehaviour        // 몬스터에 부착됨..
{
	[SerializeField] Material Default_SkyBox;

	[SerializeField] Mini04_Curve mini04_Curve;


	void Awake()
	{
		Material skyBox_Mini04 = Default_SkyBox;      // 스카이 박스를 가져온다.
		RenderSettings.skybox = skyBox_Mini04;       // 스카이 박스 교체
	}

	public void HitAndShield_Check()                   // (애니메이션에 부착됨) 애니메이션 막판에 뿅망치가 플레이어에 닿았을 때, 플레이어가 헬맷을 썻는지, 안써는지 체크함
	{
        
        mini04_Curve.HitAndShield_Check();
    }
}
