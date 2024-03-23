using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini05_Spawn : MonoBehaviour
{
    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // ������ ������ ��Ͻ�ų �� �ִ� ��ư

	[SerializeField] GameObject Plunger;
    [SerializeField] GameObject Bomb;
    [SerializeField] GameObject Tornado;

    [SerializeField] GameObject knight;
    [SerializeField] GameObject archer;
    [SerializeField] GameObject which;

    [SerializeField] Transform player;                   // �÷��̾� ��ġ
    [SerializeField] Transform failPos;                  // ȭ�� ���� ���

    [SerializeField] TextMeshProUGUI time_M;
    [SerializeField] TextMeshProUGUI time_S;

    [SerializeField] TextMeshProUGUI hp_Text;

    int scoreInt = 0;

    GameObject prefab;          // ������ �Ŵ������� ������Ʈ�� �޾ƿ� ����
    GameObject mon_Prefab;      // ���� ������ �������� ���� ���������� ���� ����

    Transform[] spawnTrans = new Transform[4];    // ������ġ 4��

    List<int> knightPosList;               // �˻簡 �̵��� Z�� ��ġ(���� �ߺ� ������)
    List<int> archerPosList;               // �ü��� �̵��� Z�� ��ġ(���� �ߺ� ������)
    List<int> witchPosList;                // ���డ �̵��� Z�� ��ġ(���� �ߺ� ������)

    Queue<GameObject> queue_BreakThrough = new Queue<GameObject>();     // �վ ������Ʈ Ǯ��
    Queue<GameObject> queue_DonutBomb = new Queue<GameObject>();        // ���� ��ź ������Ʈ Ǯ��
    Queue<GameObject> queue_Tonedo = new Queue<GameObject>();           // ����̵� ������Ʈ Ǯ��

    public List<GameObject> list_Knight = new List<GameObject>();     // �վ ������Ʈ Ǯ��
    public List<GameObject> list_Archer = new List<GameObject>();        // ���� ��ź ������Ʈ Ǯ��
    public List<GameObject> list_Witch = new List<GameObject>();           // ����̵� ������Ʈ Ǯ��

    WaitForSeconds delay;
    WaitForSeconds delay_Time;        // �ð� ��ȭ �ڷ�ƾ �ð�
    WaitForSeconds[] delay_Array = new WaitForSeconds[14];             // �ð����� ������ �����ϱ� ���� ������ 9��

    int Time_Level;      // �ð� ��ȭ ���� ����

    float timeInt = 0;
    float tempInt = 0;
    float minute = 0;
    float second = 0;

    int hp_Int = 0;

    void Awake()
    {
        for (int i = 0; i < 4; i++)
		{
            spawnTrans[i] = transform.GetChild(i).transform;       // �ڽ� ������Ʈ ���(���� ���� ��ġ)
        }

        knightPosList = new List<int>() { -145, -135, -115, -105, -85, -75, -55, -45, -25, -15, 5, 15, 35, 45, 65, 75, 95, 105, 125, 135 };
        archerPosList = new List<int>() { -145, -125, -105, -85, -65, -45, -25, -5, 15, 35, 55, 75, 95, 115, 135 };
        witchPosList = new List<int>() { -125, -95, -65, -35, -5, 25, 55, 85, 115, 145 };

        GameObject p_object;

        prefab = Plunger;
        for (int i = 0; i < 4; i++)       // �վ 20���� ���� �� ����
        {
            p_object = Instantiate(prefab);  // �վ ����
            p_object.GetComponent<Mini05_Weapon>().mini05_Spawn = this;
            queue_BreakThrough.Enqueue(p_object);       // ������Ʈ Ǯ��
        }

        prefab = Bomb;

        p_object = Instantiate(prefab);  // ���� ��ź ����
        p_object.GetComponent<Mini05_Weapon>().mini05_Spawn = this;
        queue_DonutBomb.Enqueue(p_object);       // ������Ʈ Ǯ��
        

        prefab = Tornado;

        p_object = Instantiate(prefab);  // ����̵� ����
        p_object.GetComponent<Mini05_Weapon>().mini05_Spawn = this;
        queue_Tonedo.Enqueue(p_object);       // ������Ʈ Ǯ��


        int randInt;

        prefab = knight;               // �˻�...........................
        for (int i = 0; i < 20; i++)       // �˻� 30���� ���� �� ����
        {
            mon_Prefab = Instantiate(prefab);// ���� ����
            randInt = Random.Range(0, knightPosList.Count);

            Mini05_Knight mini05_Knight = mon_Prefab.GetComponent<Mini05_Knight>();
            mini05_Knight.thisInt = knightPosList[randInt];
            mini05_Knight.thisStartInt = StartInt(knightPosList[randInt])[0];
            mini05_Knight.mini05_Spawn = this;

            mon_Prefab.transform.position = spawnTrans[StartInt(knightPosList[randInt])[1]].position;
            
            list_Knight.Add(mon_Prefab);       // ������Ʈ Ǯ��

            knightPosList.RemoveAt(randInt);
        }

        prefab = archer;               // �ü�...........................
        for (int i = 0; i < 15; i++)       // �ü� 30���� ���� �� ����
        {
            mon_Prefab = Instantiate(prefab);// ���� ����
            randInt = Random.Range(0, archerPosList.Count);

            Mini05_Archer mini05_Archer = mon_Prefab.GetComponent<Mini05_Archer>();
            mini05_Archer.thisInt = archerPosList[randInt];
            mini05_Archer.thisStartInt = StartInt(archerPosList[randInt])[0];
            mini05_Archer.player = player;
            mini05_Archer.FailPos = failPos;
            mini05_Archer.mini05_Spawn = this;

            mon_Prefab.transform.position = spawnTrans[StartInt(archerPosList[randInt])[1]].position;

            list_Archer.Add(mon_Prefab);       // ������Ʈ Ǯ��

            archerPosList.RemoveAt(randInt);
        }


		prefab = which;               // ����...........................
        for (int i = 0; i < 10; i++)       // ���� 50���� ���� �� ����
		{
			mon_Prefab = Instantiate(prefab);// ���� ����
            randInt = Random.Range(0, witchPosList.Count);

            Mini05_Witch mini05_Witch = mon_Prefab.GetComponent<Mini05_Witch>();
            mini05_Witch.thisInt = witchPosList[randInt];
            mini05_Witch.thisStartInt = StartInt(witchPosList[randInt])[0];
            mini05_Witch.player = player;
            mini05_Witch.mini05_Spawn = this;

            mon_Prefab.transform.position = spawnTrans[StartInt(witchPosList[randInt])[1]].position;

            list_Witch.Add(mon_Prefab);       // ������Ʈ Ǯ��

            witchPosList.RemoveAt(randInt);
        }

        delay_Time = new WaitForSeconds(60.0f);
        delay = new WaitForSeconds(1.0f);


        for (int i = 0; i < 14; i++)
		{                                                  // 0, 1, 2, 3, 4, 5, 6, 7, 8
                                                           // 2, 3, 4, 5, 6, 7, 8, 9, 10

            //new    // 0, 1, 2, 3, 4, 5, 6, 7,  8,  9,  10, 11, 12, 13
                     // 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16

            delay_Array[i] = new WaitForSeconds(i + 3);
        }

        StartCoroutine(SpawnCoroutine_Knight());   // �˻� ���� �ڷ�ƾ..(30�� �ݺ�)
        StartCoroutine(SpawnCoroutine_Archer());   // �ü� ���� �ڷ�ƾ..(30�� �ݺ�)
        StartCoroutine(SpawnCoroutine_Witch());   // ���� ���� �ڷ�ƾ..(30�� �ݺ�)

        StartCoroutine(TimerChange());
        StartCoroutine(Time_Coroutine_S());
        
        hp_Int = 100;
    }


	void Update()
	{
        timeInt += Time.deltaTime;                // ������ �ð�
        tempInt += Time.deltaTime;
        minute = Mathf.Floor(timeInt / 60);
        second = Mathf.Floor(tempInt);

        if (second.Equals(60))
        {
            tempInt = 0;
            second = 0;
        }
    }


    public void End_Game()
    {
        Result_Panel.SetActive(true);
        Game_Panel.SetActive(true);


        Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + scoreInt.ToString();

        if (Main.ins.nowPlayer.maxScore_List[4] >= scoreInt)    // �ְ����� �� �����ٸ�...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[4].ToString();
        }
        else        // �ְ����� ���� ��� (�ű��)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + scoreInt.ToString();

            Main.ins.nowPlayer.maxScore_List[4] = scoreInt;
            Main.ins.SaveData();

			GPGS_Button.interactable = true;        // �ű���� ����� ���� ���� ��� ��ư�� Ȱ��ȭ�Ѵ�. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // ��������� ����.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;

    }


	public void Press_GPGS_05()   // �ű�� ��, ������ ���
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no5, scoreInt);   // ������ ������ ���
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no5);          // �������带 ����.
	}


	public void Hp_WallMinus()
    {
        hp_Int--;
        hp_Text.text = hp_Int.ToString();

        if (hp_Int.Equals(0))
        {
            End_Game();
            
            return;
        }
    }

    int timeInt_M = 0;
    int timeInt_S = 0;


    IEnumerator Time_Coroutine_S()       // �� �ð�
    {
        time_S.text = timeInt_S.ToString();
        
        while (true)
        {
            yield return delay;
            timeInt_S++;

            if (timeInt_S.Equals(30))
            {
                scoreInt++;                  // ���ھ� �ø���
            }
            else if (timeInt_S.Equals(60))
            {
                timeInt_S = 0;
                timeInt_M++;
                time_M.text = timeInt_M.ToString();

                scoreInt++;                  // ���ھ� �ø���
            }

            time_S.text = timeInt_S.ToString();
        }
    }


    /////// �վ ����...

    public void InsertQueue_BreakThrough(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(�վ)
    {
        queue_BreakThrough.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_BreakThrough()           // ť���� ��ü�� �������� �Լ�(�վ)
    {
        GameObject t_object = queue_BreakThrough.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    //////////////// ���� ��ź ����..

    public void InsertQueue_DonutBomb(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(���� ��ź)
    {
        queue_DonutBomb.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_DonutBomb()           // ť���� ��ü�� �������� �Լ�(���� ��ź)
    {
        GameObject t_object = queue_DonutBomb.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }


    //////////////// ����̵� ����..

    public void InsertQueue_Tonedo(GameObject p_object)     // ����� ��ü�� ť�� �ٽ� �ݳ���Ű�� �Լ�(����̵�)
    {
        queue_Tonedo.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Tonedo()           // ť���� ��ü�� �������� �Լ�(����̵�)
    {
        GameObject t_object = queue_Tonedo.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    int[] StartInt(int randInt)
    {
        int tempInt = randInt;                      // ���������� �ӽ÷� ��� �޴´�.
        int startInt = 0;     // �� ��ó ��ġ�� ���� ���δ�.
        int spawnInt = 0;     // ��ȯ�� ���� �����ϴ� ����

        if (tempInt < -104)     // �������� ���� ���� -105 ���϶��...
        {
            startInt = -130;         // 0�� ���� ��ó�� �̵�
            spawnInt = 0;            // 0�� ������������ ����
        }
        else if (tempInt < -74)     // �������� ���� ���� -75 ���϶��...
        {
            startInt = -65;         // 0�� ���� ��ó�� �̵�
            spawnInt = 0;            // 0�� ������������ ����
        }
        else if (tempInt < -44)     // �������� ���� ���� -45 ���϶��...
        {
            startInt = -65;         // 1�� ���� ��ó�� �̵�
            spawnInt = 1;            // 1�� ������������ ����
        }
        else if (tempInt < -4)     // �������� ���� ���� -5 ���϶��...
        {
            startInt = 0;         // 1�� ���� ��ó�� �̵�
            spawnInt = 1;            // 1�� ������������ ����
        }
        else if (tempInt < 36)     // �������� ���� ���� 35 ���϶��...
        {
            startInt = 0;         // 2�� ���� ��ó�� �̵�
            spawnInt = 2;            // 2�� ������������ ����
        }
        else if (tempInt < 66)     // �������� ���� ���� 65 ���϶��...
        {
            startInt = 65;         // 2�� ���� ��ó�� �̵�
            spawnInt = 2;            // 2�� ������������ ����
        }
        else if (tempInt < 96)     // �������� ���� ���� 95 ���϶��...
        {
            startInt = 65;         // 3�� ���� ��ó�� �̵�
            spawnInt = 3;            // 3�� ������������ ����
        }
        else                       // �������� ���� ���� 96 �̻���...
        {
            startInt = 130;         // 3�� ���� ��ó�� �̵�
            spawnInt = 3;            // 3�� ������������ ����
        }

        int[] arr = new int[2];     // �迭 ���·� �Ѱ���
        arr[0] = startInt;          // ��ġ..
        arr[1] = spawnInt;          // ���� ���� ��ġ

        return arr;          // ���� ���� 2����...
    }



    /////////////////////// �ڷ�ƾ ����............

    IEnumerator SpawnCoroutine_Knight()   // �˻� ���� �ڷ�ƾ..(30�� �ݺ�)
    {
        WaitForSeconds delay;

        while (true)
        {
            switch (Time_Level)
            {
                case 0:
                    delay = delay_Array[4];      // 7��
                    break;
                case 1:
                    delay = delay_Array[3];      // 6��
                    break;
                case 2:
                    delay = delay_Array[2];      // 5��
                    break;
                case 3:
                    delay = delay_Array[1];      // 4��
                    break;
                default:
                    delay = delay_Array[0];      // 3��
                    break;
            }

            if (!list_Knight.Count.Equals(0))                       // �˻� ���� ����Ʈ�� �ϳ� �̻� �ִٸ�?
            {
                int randInt = Random.Range(0, list_Knight.Count);   // �������� �̰�
                list_Knight[randInt].SetActive(true);               // �������� ���� ���ڸ� Ȱ��ȭ ��Ų��.
                list_Knight.RemoveAt(randInt);                      // �ߺ� ����
            }

            yield return delay;
        }
    }

    IEnumerator SpawnCoroutine_Archer()   // �ü� ���� �ڷ�ƾ..(30�� �ݺ�)
    {
        WaitForSeconds delay;

        while (true)
        {
            switch (Time_Level)
            {
                case 0:
                    delay = delay_Array[9];     // 12��
                    break;
                case 1:
                    delay = delay_Array[8];     // 11��
                    break;
                case 2:
                    delay = delay_Array[7];     // 10��
                    break;
                case 3:
                    delay = delay_Array[6];     // 9��
                    break;
                default:
                    delay = delay_Array[5];     // 8��
                    break;
            }

            if (!list_Archer.Count.Equals(0))                       // �ü� ���� ����Ʈ�� �ϳ� �̻� �ִٸ�?
            {
                int randInt = Random.Range(0, list_Archer.Count);   // �������� �̰�
                list_Archer[randInt].SetActive(true);               // �������� ���� ���ڸ� Ȱ��ȭ ��Ų��.
                list_Archer.RemoveAt(randInt);                      // �ߺ� ����
            }

            yield return delay;
        }
    }

    IEnumerator SpawnCoroutine_Witch()   // ���� ���� �ڷ�ƾ..(30�� �ݺ�)
    {
        WaitForSeconds delay;

        while (true)
        {
            switch (Time_Level)
            {
                case 0:
                    delay = delay_Array[13];     // 16��
                    break;
                case 1:
                    delay = delay_Array[12];     // 15��
                    break;
                case 2:
                    delay = delay_Array[11];     // 14��
                    break;
                case 3:
                    delay = delay_Array[10];     // 13��
                    break;
                default:
                    delay = delay_Array[9];     // 12��
                    break;
            }

            if (!list_Witch.Count.Equals(0))                       // ���� ���� ����Ʈ�� �ϳ� �̻� �ִٸ�?
            {
                int randInt = Random.Range(0, list_Witch.Count);   // �������� �̰�
                list_Witch[randInt].SetActive(true);               // �������� ���� ���ڸ� Ȱ��ȭ ��Ų��.
                list_Witch.RemoveAt(randInt);                      // �ߺ� ����
            }

            yield return delay;
        }
    }


    IEnumerator TimerChange()       // ���� �ð� ���� �ڷ�ƾ
    {
        Time_Level = 0;
        yield return delay_Time;   // 1�� ��

        Time_Level = 1;

        yield return delay_Time;   // 1�� ��
        yield return delay_Time;   // 1�� ��
        yield return delay_Time;   // 1�� ��
        Time_Level = 2;

        yield return delay_Time;   // 1�� ��
        yield return delay_Time;   // 1�� ��
        yield return delay_Time;   // 1�� ��
        Time_Level = 3;

        yield return delay_Time;   // 1�� ��
        yield return delay_Time;   // 1�� ��
        Time_Level = 4;
    }
}
