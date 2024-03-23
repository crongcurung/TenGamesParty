using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini06_Spawn : MonoBehaviour                          // ���� ������ ������
{
    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // ������ ������ ��Ͻ�ų �� �ִ� ��ư

	[SerializeField] GameObject bee;
    [SerializeField] GameObject web;

    GameObject prefab;          // ������ �Ŵ������� ������Ʈ�� �޾ƿ� ����
    GameObject mon_Prefab;      // ���� ������ �������� ���� ���������� ���� ����

    Vector3[] pos_Array = new Vector3[4];                       // ���� ���� ��ġ

    WaitForSeconds delay_Time;                                  // �ð� ��ȭ �ð�
    WaitForSeconds[] delay_Array = new WaitForSeconds[4];       // ���� �ð� �迭

    int Time_Level;                                             // �ð� ��ȭ ����
    [SerializeField] Mini06_Player mini06_Player;                         // �÷��̾� ��ũ��Ʈ
    [SerializeField] Mini06_Bear mini06_Bear;                             // �� ��ũ��Ʈ

    public List<GameObject> list_Bee = new List<GameObject>();     // �վ ������Ʈ Ǯ��
    Queue<GameObject> queue_Web = new Queue<GameObject>();     // �վ ������Ʈ Ǯ��

    int scoreInt = 0;

    [SerializeField] TextMeshProUGUI scoreText;                 // �ڽ��� ���� ���ھ� ������ ���� ��ũ��Ʈ���� ó���Ѵ�.
    [SerializeField] TextMeshProUGUI hive_Text;                 // ����ִ� ������ ������ �˸��� �ؽ�Ʈ
    int hive_Int = 0;

    void Awake()
    {
        Vector3 pos01 = new Vector3(-20.0f, 2.0f, -15.0f);     // ���� ��ġ
        pos_Array[0] = pos01;
        pos01 = new Vector3(-20.0f, 2.0f, 15.0f);
        pos_Array[1] = pos01;
        pos01 = new Vector3(20.0f, 2.0f, -15.0f);
        pos_Array[2] = pos01;
        pos01 = new Vector3(20.0f, 2.0f, 15.0f);
        pos_Array[3] = pos01;


        delay_Time = new WaitForSeconds(60.0f);                // �ð� ��ȭ(1��)

        delay_Array[0] = new WaitForSeconds(5.0f);
        delay_Array[1] = new WaitForSeconds(4.0f);
        delay_Array[2] = new WaitForSeconds(3.0f);
        delay_Array[3] = new WaitForSeconds(2.0f);

        Mini06_Bee mini06_Bee;

        prefab = bee;               // ��...........................
        for (int i = 0; i < 30; i++)       // �� 30���� ���� �� ����
        {
            mon_Prefab = Instantiate(prefab);      // �� ����

            mini06_Bee = mon_Prefab.GetComponent<Mini06_Bee>();
            mini06_Bee.mini06_Spawn = this;   // �� ��ũ��Ʈ�� �ѱ��.(�ݳ�������...)
            mini06_Bee.mini06_Bear = mini06_Bear;
            mini06_Bee.mini06_Player = mini06_Player;

            list_Bee.Add(mon_Prefab);       // ������Ʈ Ǯ��
        }

        
         
        prefab = web;               // �׹�...........................
        for (int i = 0; i < 5; i++)       // �׹� 5���� ���� �� ����
        {
            mon_Prefab = Instantiate(prefab);// �׹� ����
            mon_Prefab.GetComponent<Mini06_Web>().mini06_Spawn = this;   // �� ��ũ��Ʈ�� �ѱ��.(�ݳ�������...)
            queue_Web.Enqueue(mon_Prefab);       // ������Ʈ Ǯ��
        }

        StartCoroutine(TimerChange());                 // �ð� ��ȭ �ڷ�ƾ ����
        StartCoroutine(SpawnCoroutine_Bee());          // �� ���� �ڷ�ƾ ����


        hive_Int = 12;
    }



    ///////////////////////// �Ϲ� �Լ� ����


    public void InsertQueue_Web(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(�׹�)
    {
        queue_Web.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Web()           // ť���� ��ü�� �������� �Լ�(�׹�)
    {
        GameObject t_object = queue_Web.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }


    public void End_Box()
    {

        hive_Int--;
        hive_Text.text = hive_Int.ToString();

        if (hive_Int.Equals(6))
        {
            End_Game();
        }
    }

    public void End_Game()
    {
        Result_Panel.SetActive(true);
        Game_Panel.SetActive(true);


        Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + scoreInt.ToString();

        if (Main.ins.nowPlayer.maxScore_List[5] >= scoreInt)    // �ְ����� �� �����ٸ�...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[5].ToString();
        }
        else        // �ְ����� ���� ��� (�ű��)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + scoreInt.ToString();

            Main.ins.nowPlayer.maxScore_List[5] = scoreInt;
            Main.ins.SaveData();

			GPGS_Button.interactable = true;        // �ű���� ����� ���� ���� ��� ��ư�� Ȱ��ȭ�Ѵ�. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // ��������� ����.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;
    }

	public void Press_GPGS_06()   // �ű�� ��, ������ ���
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no6, scoreInt);   // ������ ������ ���
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no6);          // �������带 ����.
	}



	public void Score_Box()       // ���ھ� �ø��� �Լ�(�ڽ��ʿ��� ���� ��)
    {
        scoreInt++;
        scoreText.text = scoreInt.ToString("N0") ;     // �� �Լ��� ����ɶ����� ���ھ� �ؽ�Ʈ �ö�
    }



    ///////////////////////  �ڷ�ƾ ����...


    IEnumerator TimerChange()       // ���� �ð� ���� �ڷ�ƾ                5, 4, 3, 2...?
    {
        yield return delay_Time;   // 1�� ��

        Time_Level = 1;            // �ð� ���� 1�� �ٲ�

        yield return delay_Time;   // 2�� ��
        yield return delay_Time;

        Time_Level = 2;            // �ð� ���� 2�� �ٲ�

        yield return delay_Time;   // 2�� ��
        yield return delay_Time;

        Time_Level = 3;            // �ð� ���� 3�� �ٲ�
    }



    IEnumerator SpawnCoroutine_Bee()   // �� ���� �ڷ�ƾ..(30�� �ݺ�)
    {
        WaitForSeconds delay;
        int randInt;

        while (true)
        {
            switch (Time_Level)                 // �ð� ������ ����..
            {
                case 0:
                    delay = delay_Array[0];      // 5��
                    break;
                case 1:
                    delay = delay_Array[1];      // 4��
                    break;
                case 2:
                    delay = delay_Array[2];      // 3��
                    break;
                default:
                    delay = delay_Array[2];      // 2��
                    break;
            }

            if (!list_Bee.Count.Equals(0))       // �� ���� ����Ʈ�� �ϳ��� ���� ���...
            {
                int randInt_Bee = Random.Range(0, list_Bee.Count);    // �� ����Ʈ ������ ����..
                randInt = Random.Range(0, pos_Array.Length);          // 4?
                GameObject spawn = list_Bee[randInt_Bee];             // 
                spawn.transform.position = pos_Array[randInt];        // ���� ��ġ ����
                spawn.SetActive(true);

                list_Bee.RemoveAt(randInt_Bee);   // �� ����
            }

            yield return delay;
        }
    }
}
