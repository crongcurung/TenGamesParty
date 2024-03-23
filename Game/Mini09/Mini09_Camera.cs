using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini09_Camera : MonoBehaviour         // 미니 09 카메라에 부착됨
{
	[SerializeField] Material Mini09_SkyBox;

	[SerializeField] GameObject Result_Panel;
	[SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // 점수를 서버에 등록시킬 수 있는 버튼

	[SerializeField] Transform playerTrans;             // 플레이어 오브젝트
	[SerializeField] Transform paperPlain;              // 종이 비행기 오브젝트
	[SerializeField] Vector3 cameraPos;         // 미니 게임 마다 화면에 보여줄 카메라 위치 조정 값

	Transform target;        // 카메라가 잡는 타켓 오브젝트
	bool isEnd = false;      // 게임이 끝난 경우...
	Vector3 completePos;     // 게임이 끝났을 때, 카메라 시점 조절

	void Awake()
	{
		Material skyBox_Mini09 = Mini09_SkyBox;      // 스카이 박스를 가져온다.
		RenderSettings.skybox = skyBox_Mini09;       // 스카이 박스 교체

		cameraPos = new Vector3(7.5f, 2.5f, 2.0f);                    // 카메라 위치 조정
		transform.rotation = Quaternion.Euler(new Vector3(25.0f, -90.0f, 0));  // 카메라 초기 회전 값 조절
		target = playerTrans;     // 처음에는 플레이어를 잡는다.

		completePos = new Vector3(2.0f, 1.0f, 0.3f);   // 마지막에 카메라 시점 벡터
	}


	void LateUpdate()     // 요거 lateUpdate로 해야함..
	{
		if (endBool.Equals(false))
		{
			CameraPos();
		}
	}

	void CameraPos()
	{
		if (isEnd.Equals(false))     // 게임이 끝나지 않는 경우...
		{
			transform.position = target.position + cameraPos;       // 타켓의 위치에 따른 카메라 위치
		}
		else                         // 게임이 끝난 경우...(비행이든, 플레이어가 바닥에서 멈춘 경우)
		{
			transform.position = Vector3.Lerp(transform.position, target.transform.position + completePos, Time.unscaledDeltaTime * 2.0f);  // 마지막에 카메라를 부드럽게 이동
			transform.LookAt(transform.position);  // 카메라가 보는 시점을 타겟으로 삼음

			if ((target.transform.position + completePos - transform.position).magnitude < 0.1f)          // 줌인이 다 된 경우..
			{

				if (target.Equals(paperPlain))
				{
					End_Game(true);     // 비행기일떄,
				}
				else
				{
					End_Game(false);         // 플레이어
				}
				return;
			}
		}
	}

	bool endBool = false;
	int thisInt = 0;

	public void End_Game(bool isTarget)                   // 게임이 끝났을 때 함수   true면 비행기, false면 플레이어
	{
		endBool = true;
		playerTrans.parent.GetComponent<Mini09_Player>().isEnd = true;
		paperPlain.GetComponent<Mini09_Plain>().isEnd = true;

		Result_Panel.SetActive(true);
		Game_Panel.SetActive(true);

		int scoreInt = 0;

		if (isTarget.Equals(true))         // 비행기일떄...
		{
			Mini09_Plain mini09_Plain = paperPlain.GetComponent<Mini09_Plain>();
			scoreInt = mini09_Plain.coinInt * 10 + (int)(Mathf.Floor(mini09_Plain.distance * 0.01f));
		}
		else                     // 플레이어 일떄..
		{
			Mini09_Player mini09_Player = playerTrans.parent.GetComponent<Mini09_Player>();
			scoreInt = mini09_Player.coinInt * 10 + (int)(Mathf.Floor(mini09_Player.distance * 0.01f));
		}

		Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + scoreInt.ToString();

		if (Main.ins.nowPlayer.maxScore_List[8] >= scoreInt)    // 최고점을 못 넘은다면...
		{
			Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[8].ToString();
		}
		else        // 최고점을 넘은 경우 (신기록)
		{
			Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + scoreInt.ToString();

			Main.ins.nowPlayer.maxScore_List[8] = scoreInt;
			Main.ins.SaveData();

			thisInt = scoreInt;
			GPGS_Button.interactable = true;        // 신기록을 세우면 점수 서버 등록 버튼을 활성화한다. /////////////////////////////////////////////////////////////////////////////
		}

		AudioMng.ins.Pause_BG();              // 배경음악을 끈다.
		AudioMng.ins.PlayEffect("Fail02");

		Time.timeScale = 0;
	}


	public void Press_GPGS_09()   // 신기록 시, 서버에 등록
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no9, thisInt);   // 점수를 서버에 등록
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no9);          // 리더보드를 띄운다.
	}



	public void Camera_Chage()      // 카메라 약간 변경
	{
		cameraPos = new Vector3(7.5f, 3.5f, 0.0f);
	}

	public void Target_Change()   // 플레이어 스크립트에서 비행기를 날리면 실행된다.
	{
		target = paperPlain;      // 타켓을 날리는 비행기로 바꾼다.
		Camera_Chage();      // 카메라 약간 변경
	}

	public void EndGame()         // 게임이 끝났을 경우..(플레이어, 비행기 스크립트에서 사용)
	{
		isEnd = true;             // 게임이 끝났다고 한다.
	}
}
