using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini02_CountLine : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Mini02_Player mini02_Player;

    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // ������ ������ ��Ͻ�ų �� �ִ� ��ư

	[SerializeField] GameObject menuPanel;          // ��ǳ��


    // 0 : ����/����/��ũ
    // 1 : ����/����/����
    // 2 : ����/Ƣ��/��ũ
    // 3 : ����/Ƣ��/����
    // 4 : ��Ÿ/����/��ũ
    // 5 : ��Ÿ/����/����
    // 6 : ��Ÿ/Ƣ��/��ũ
    // 7 : ��Ÿ/Ƣ��/����
    [SerializeField] Image menu_Image;
    [SerializeField] Sprite[] Sprite_Array;        // ��ǳ�� �̹���
    

    [SerializeField] GameObject[] Hp_Array;      // ü�� �̹��� ������Ʈ ����Ʈ

    public int menuInt = 0;         // �մԿ��Լ� ���� ������ �޴� ����

    Mini02_Guest mini02_Guest;      // ���� �մ��� ��ũ��Ʈ�� �޴� ����

    public bool newMonster = false;     // ���� �մ��� �ֳ� ���� ����

    int hpCount = 0;                    // ü�� ī��Ʈ 

    void Start()
    {
        hpCount = Hp_Array.Length;
    }


    void Update()
    {

        if (newMonster.Equals(true))       // ���� �մ��� �ִٸ� ��ٸ� �ð� �˷��ش�.
        {
            slider.value -= Time.deltaTime;
        }
    }

    

    public void SettingMenu(int menuInt, Mini02_Guest mini02_Guest, float timeFloat)       // ���� �մ��� ��ũ��Ʈ���� �������� ���� ���ڸ� �����ͼ� �޴��� �Ǵ��غ��� �Լ�
    {
        this.menuInt = menuInt;             // �մ����� �������� ���� ����
        this.mini02_Guest = mini02_Guest;   // ���� �մ��� ��ũ��Ʈ
        slider.maxValue = timeFloat;
        slider.value = timeFloat;

        newMonster = true;                // �մ��� ��Ҵٰ� �˸�
        menuPanel.SetActive(true);        // �մ��� ī���� �տ� �Ա� ������ ��ǳ�� ������

        switch (menuInt)     // �մ��� ���ڸ� �Ǵ�
        {
            case 0:              // 0 = ����/����/��ũ
                menu_Image.sprite = Sprite_Array[0];
                break; 
            case 1:              // 1 = ����/����/����
                menu_Image.sprite = Sprite_Array[1];
                break; 
            case 2:              // 2 = ����/Ƣ��/��ũ
                menu_Image.sprite = Sprite_Array[2];
                break;
            case 3:              // 3 = ����/Ƣ��/����
                menu_Image.sprite = Sprite_Array[3];
                break;
            case 4:              // 4 = ��Ÿ/����/��ũ
                menu_Image.sprite = Sprite_Array[4];
                break;
            case 5:              // 5 = ��Ÿ/����/����
                menu_Image.sprite = Sprite_Array[5];
                break;
            case 6:              // 6 = ��Ÿ/Ƣ��/��ũ
                menu_Image.sprite = Sprite_Array[6];
                break;
            default:             // 7 = ��Ÿ/Ƣ��/����
                menu_Image.sprite = Sprite_Array[7]; 
                break;
        }
    }

    public void GoHome_Guest(bool isMinus)        // ���� �մԿ��� �ٽ� ���ư���� �˸���..
    {
        if (newMonster.Equals(true))              // ���� �մ��� �־��� ���...
        {
            newMonster = false;                   // ���ư���� �߱� ������ ���� �մ��� ���ٰ� �˸���.
            mini02_Guest.GoBack();                // �մԿ��� ���ư���� �Ѵ�.
            menuPanel.SetActive(false);           // ��ǳ�� ���ֱ�

            if (isMinus.Equals(true))             // ���̳ʽ��� ���� �ִٸ�..(�� �Լ��� �����ҋ��� �׳� ���ư��� �ΰ� ��� �Լ� �����̶� �մ��� �׳� ���ư� ��� ���̳ʽ��� Ų��)
            {
                hpCount--;                        // ü�� ī��Ʈ�� ���δ�.

                Hp_Array[hpCount].SetActive(false);    // ü�� �̹����� ��Ȱ��ȭ�Ѵ�.

                if (hpCount.Equals(0))                // ü�� ī��Ʈ�� 0�̶�� ���� ����(�� 5��)
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

        if (Main.ins.nowPlayer.maxScore_List[1] >= mini02_Player.scoreInt)    // �ְ����� �� �����ٸ�...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[1].ToString();
        }
        else        // �ְ����� ���� ��� (�ű��)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + mini02_Player.scoreInt.ToString();

            Main.ins.nowPlayer.maxScore_List[1] = mini02_Player.scoreInt;
            Main.ins.SaveData();

			GPGS_Button.interactable = true;        // �ű���� ����� ���� ���� ��� ��ư�� Ȱ��ȭ�Ѵ�. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // ��������� ����.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;
    }

	public void Press_GPGS_02()   // �ű�� ��, ������ ���
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no2, mini02_Player.scoreInt);   // ������ ������ ���
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no2);          // �������带 ����.
	}
}
