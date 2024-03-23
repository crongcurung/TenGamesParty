using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini02_CountLine : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Mini02_Player mini02_Player;

    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // 점수를 서버에 등록시킬 수 있는 버튼

	[SerializeField] GameObject menuPanel;          // 말풍선


    // 0 : 구멍/오븐/핑크
    // 1 : 구멍/오븐/초코
    // 2 : 구멍/튀김/핑크
    // 3 : 구멍/튀김/초코
    // 4 : 스타/오븐/핑크
    // 5 : 스타/오븐/초코
    // 6 : 스타/튀김/핑크
    // 7 : 스타/튀김/초코
    [SerializeField] Image menu_Image;
    [SerializeField] Sprite[] Sprite_Array;        // 말풍선 이미지
    

    [SerializeField] GameObject[] Hp_Array;      // 체력 이미지 오브젝트 리스트

    public int menuInt = 0;         // 손님에게서 랜덤 변수를 받는 변수

    Mini02_Guest mini02_Guest;      // 닿은 손님의 스크립트를 받는 변수

    public bool newMonster = false;     // 닿은 손님이 있냐 묻는 변수

    int hpCount = 0;                    // 체력 카운트 

    void Start()
    {
        hpCount = Hp_Array.Length;
    }


    void Update()
    {

        if (newMonster.Equals(true))       // 닿은 손님이 있다면 기다린 시간 알려준다.
        {
            slider.value -= Time.deltaTime;
        }
    }

    

    public void SettingMenu(int menuInt, Mini02_Guest mini02_Guest, float timeFloat)       // 닿은 손님의 스크립트에서 랜덤으로 뽑은 숫자를 가져와서 메뉴를 판단해보는 함수
    {
        this.menuInt = menuInt;             // 손님한테 랜덤으로 뽑은 숫자
        this.mini02_Guest = mini02_Guest;   // 닿은 손님의 스크립트
        slider.maxValue = timeFloat;
        slider.value = timeFloat;

        newMonster = true;                // 손님이 닿았다고 알림
        menuPanel.SetActive(true);        // 손님이 카운터 앞에 왔기 때문에 말풍선 나오기

        switch (menuInt)     // 손님의 숫자를 판단
        {
            case 0:              // 0 = 구멍/오븐/핑크
                menu_Image.sprite = Sprite_Array[0];
                break; 
            case 1:              // 1 = 구멍/오븐/초코
                menu_Image.sprite = Sprite_Array[1];
                break; 
            case 2:              // 2 = 구멍/튀김/핑크
                menu_Image.sprite = Sprite_Array[2];
                break;
            case 3:              // 3 = 구멍/튀김/초코
                menu_Image.sprite = Sprite_Array[3];
                break;
            case 4:              // 4 = 스타/오븐/핑크
                menu_Image.sprite = Sprite_Array[4];
                break;
            case 5:              // 5 = 스타/오븐/초코
                menu_Image.sprite = Sprite_Array[5];
                break;
            case 6:              // 6 = 스타/튀김/핑크
                menu_Image.sprite = Sprite_Array[6];
                break;
            default:             // 7 = 스타/튀김/초코
                menu_Image.sprite = Sprite_Array[7]; 
                break;
        }
    }

    public void GoHome_Guest(bool isMinus)        // 닿은 손님에게 다시 돌아가라고 알린다..
    {
        if (newMonster.Equals(true))              // 닿은 손님이 있었을 경우...
        {
            newMonster = false;                   // 돌아가라고 했기 때문에 닿은 손님이 업다고 알린다.
            mini02_Guest.GoBack();                // 손님에게 돌아가라고 한다.
            menuPanel.SetActive(false);           // 말풍선 없애기

            if (isMinus.Equals(true))             // 마이너스가 켜져 있다면..(이 함수는 성공할떄와 그냥 돌아갈때 두개 모두 함수 실행이라 손님이 그냥 돌아갈 경우 마이너스를 킨다)
            {
                hpCount--;                        // 체력 카운트를 줄인다.

                Hp_Array[hpCount].SetActive(false);    // 체력 이미지를 비활성화한다.

                if (hpCount.Equals(0))                // 체력 카운트가 0이라면 게임 종료(총 5개)
                {
                    GameOver();
                }
            }
        }
    }


    public void GameOver()
    {
        Result_Panel.SetActive(true);
        Game_Panel.SetActive(true);

        Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + mini02_Player.scoreInt.ToString();

        if (Main.ins.nowPlayer.maxScore_List[1] >= mini02_Player.scoreInt)    // 최고점을 못 넘은다면...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[1].ToString();
        }
        else        // 최고점을 넘은 경우 (신기록)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + mini02_Player.scoreInt.ToString();

            Main.ins.nowPlayer.maxScore_List[1] = mini02_Player.scoreInt;
            Main.ins.SaveData();

			GPGS_Button.interactable = true;        // 신기록을 세우면 점수 서버 등록 버튼을 활성화한다. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // 배경음악을 끈다.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;
    }

	public void Press_GPGS_02()   // 신기록 시, 서버에 등록
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no2, mini02_Player.scoreInt);   // 점수를 서버에 등록
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no2);          // 리더보드를 띄운다.
	}
}
