using UnityEngine;

public class Mini04_Monster : MonoBehaviour        // ���Ϳ� ������..
{
	[SerializeField] Material Default_SkyBox;

	[SerializeField] Mini04_Curve mini04_Curve;


	void Awake()
	{
		Material skyBox_Mini04 = Default_SkyBox;      // ��ī�� �ڽ��� �����´�.
		RenderSettings.skybox = skyBox_Mini04;       // ��ī�� �ڽ� ��ü
	}

	public void HitAndShield_Check()                   // (�ִϸ��̼ǿ� ������) �ִϸ��̼� ���ǿ� �и�ġ�� �÷��̾ ����� ��, �÷��̾ ����� ������, �Ƚ���� üũ��
	{
        
        mini04_Curve.HitAndShield_Check();
    }
}
