using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini09_Camera : MonoBehaviour         // �̴� 09 ī�޶� ������
{
	[SerializeField] Material Mini09_SkyBox;

	[SerializeField] GameObject Result_Panel;
	[SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // ������ ������ ��Ͻ�ų �� �ִ� ��ư

	[SerializeField] Transform playerTrans;             // �÷��̾� ������Ʈ
	[SerializeField] Transform paperPlain;              // ���� ����� ������Ʈ
	[SerializeField] Vector3 cameraPos;         // �̴� ���� ���� ȭ�鿡 ������ ī�޶� ��ġ ���� ��

	Transform target;        // ī�޶� ��� Ÿ�� ������Ʈ
	bool isEnd = false;      // ������ ���� ���...
	Vector3 completePos;     // ������ ������ ��, ī�޶� ���� ����

	void Awake()
	{
		Material skyBox_Mini09 = Mini09_SkyBox;      // ��ī�� �ڽ��� �����´�.
		RenderSettings.skybox = skyBox_Mini09;       // ��ī�� �ڽ� ��ü

		cameraPos = new Vector3(7.5f, 2.5f, 2.0f);                    // ī�޶� ��ġ ����
		transform.rotation = Quaternion.Euler(new Vector3(25.0f, -90.0f, 0));  // ī�޶� �ʱ� ȸ�� �� ����
		target = playerTrans;     // ó������ �÷��̾ ��´�.

		completePos = new Vector3(2.0f, 1.0f, 0.3f);   // �������� ī�޶� ���� ����
	}


	void LateUpdate()     // ��� lateUpdate�� �ؾ���..
	{
		if (endBool.Equals(false))
		{
			CameraPos();
		}
	}

	void CameraPos()
	{
		if (isEnd.Equals(false))     // ������ ������ �ʴ� ���...
		{
			transform.position = target.position + cameraPos;       // Ÿ���� ��ġ�� ���� ī�޶� ��ġ
		}
		else                         // ������ ���� ���...(�����̵�, �÷��̾ �ٴڿ��� ���� ���)
		{
			transform.position = Vector3.Lerp(transform.position, target.transform.position + completePos, Time.unscaledDeltaTime * 2.0f);  // �������� ī�޶� �ε巴�� �̵�
			transform.LookAt(transform.position);  // ī�޶� ���� ������ Ÿ������ ����

			if ((target.transform.position + completePos - transform.position).magnitude < 0.1f)          // ������ �� �� ���..
			{

				if (target.Equals(paperPlain))
				{
					End_Game(true);     // ������ϋ�,
				}
				else
				{
					End_Game(false);         // �÷��̾�
				}
				return;
			}
		}
	}

	bool endBool = false;
	int thisInt = 0;

	public void End_Game(bool isTarget)                   // ������ ������ �� �Լ�   true�� �����, false�� �÷��̾�
	{
		endBool = true;
		playerTrans.parent.GetComponent<Mini09_Player>().isEnd = true;
		paperPlain.GetComponent<Mini09_Plain>().isEnd = true;

		Result_Panel.SetActive(true);
		Game_Panel.SetActive(true);

		int scoreInt = 0;

		if (isTarget.Equals(true))         // ������ϋ�...
		{
			Mini09_Plain mini09_Plain = paperPlain.GetComponent<Mini09_Plain>();
			scoreInt = mini09_Plain.coinInt * 10 + (int)(Mathf.Floor(mini09_Plain.distance * 0.01f));
		}
		else                     // �÷��̾� �ϋ�..
		{
			Mini09_Player mini09_Player = playerTrans.parent.GetComponent<Mini09_Player>();
			scoreInt = mini09_Player.coinInt * 10 + (int)(Mathf.Floor(mini09_Player.distance * 0.01f));
		}

		Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + scoreInt.ToString();

		if (Main.ins.nowPlayer.maxScore_List[8] >= scoreInt)    // �ְ����� �� �����ٸ�...
		{
			Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[8].ToString();
		}
		else        // �ְ����� ���� ��� (�ű��)
		{
			Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + scoreInt.ToString();

			Main.ins.nowPlayer.maxScore_List[8] = scoreInt;
			Main.ins.SaveData();

			thisInt = scoreInt;
			GPGS_Button.interactable = true;        // �ű���� ����� ���� ���� ��� ��ư�� Ȱ��ȭ�Ѵ�. /////////////////////////////////////////////////////////////////////////////
		}

		AudioMng.ins.Pause_BG();              // ��������� ����.
		AudioMng.ins.PlayEffect("Fail02");

		Time.timeScale = 0;
	}


	public void Press_GPGS_09()   // �ű�� ��, ������ ���
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no9, thisInt);   // ������ ������ ���
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no9);          // �������带 ����.
	}



	public void Camera_Chage()      // ī�޶� �ణ ����
	{
		cameraPos = new Vector3(7.5f, 3.5f, 0.0f);
	}

	public void Target_Change()   // �÷��̾� ��ũ��Ʈ���� ����⸦ ������ ����ȴ�.
	{
		target = paperPlain;      // Ÿ���� ������ ������ �ٲ۴�.
		Camera_Chage();      // ī�޶� �ణ ����
	}

	public void EndGame()         // ������ ������ ���..(�÷��̾�, ����� ��ũ��Ʈ���� ���)
	{
		isEnd = true;             // ������ �����ٰ� �Ѵ�.
	}
}
