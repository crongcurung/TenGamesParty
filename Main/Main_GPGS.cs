using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_GPGS : MonoBehaviour
{



    public void Press_GPGS_Button(int num)    // ���ο��� ���õ� �������带 ��������...
    {
        switch (num)
        {
            case 0:       // �̴ϰ���01 �������� ���
                GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no1);
                break;
			case 1:
				GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no2);
				break;
			case 2:
				GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no3);
				break;
			case 3:
				GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no4);
				break;
			case 4:
				GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no5);
				break;
			case 5:
				GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no6);
				break;
			case 6:
				GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no7);
				break;
			case 7:
				GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no8);
				break;
			case 8:
				GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no9);
				break;
			case 9:
				GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no10);
				break;
		}
    }
}
