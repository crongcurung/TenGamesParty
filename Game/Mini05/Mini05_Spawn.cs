using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini05_Spawn : MonoBehaviour
{
    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // 점수를 서버에 등록시킬 수 있는 버튼

	[SerializeField] GameObject Plunger;
    [SerializeField] GameObject Bomb;
    [SerializeField] GameObject Tornado;

    [SerializeField] GameObject knight;
    [SerializeField] GameObject archer;
    [SerializeField] GameObject which;

    [SerializeField] Transform player;                   // 플레이어 위치
    [SerializeField] Transform failPos;                  // 화살 실패 장소

    [SerializeField] TextMeshProUGUI time_M;
    [SerializeField] TextMeshProUGUI time_S;

    [SerializeField] TextMeshProUGUI hp_Text;

    int scoreInt = 0;

    GameObject prefab;          // 프리팹 매니져에서 오브젝트를 받아올 변수
    GameObject mon_Prefab;      // 위에 프리팹 변수에서 몬스터 프리팹으로 뽑을 변수

    Transform[] spawnTrans = new Transform[4];    // 스폰위치 4개

    List<int> knightPosList;               // 검사가 이동할 Z값 위치(랜덤 중복 방지용)
    List<int> archerPosList;               // 궁수가 이동할 Z값 위치(랜덤 중복 방지용)
    List<int> witchPosList;                // 마녀가 이동할 Z값 위치(랜덤 중복 방지용)

    Queue<GameObject> queue_BreakThrough = new Queue<GameObject>();     // 뚫어뻥 오브젝트 풀링
    Queue<GameObject> queue_DonutBomb = new Queue<GameObject>();        // 도넛 폭탄 오브젝트 풀링
    Queue<GameObject> queue_Tonedo = new Queue<GameObject>();           // 토네이도 오브젝트 풀링

    public List<GameObject> list_Knight = new List<GameObject>();     // 뚫어뻥 오브젝트 풀링
    public List<GameObject> list_Archer = new List<GameObject>();        // 도넛 폭탄 오브젝트 풀링
    public List<GameObject> list_Witch = new List<GameObject>();           // 토네이도 오브젝트 풀링

    WaitForSeconds delay;
    WaitForSeconds delay_Time;        // 시간 변화 코루틴 시간
    WaitForSeconds[] delay_Array = new WaitForSeconds[14];             // 시간마다 빠르게 스폰하기 위한 딜레이 9개

    int Time_Level;      // 시간 변화 레벨 변수

    float timeInt = 0;
    float tempInt = 0;
    float minute = 0;
    float second = 0;

    int hp_Int = 0;

    void Awake()
    {
        for (int i = 0; i < 4; i++)
		{
            spawnTrans[i] = transform.GetChild(i).transform;       // 자식 오브젝트 담기(몬스터 스폰 위치)
        }

        knightPosList = new List<int>() { -145, -135, -115, -105, -85, -75, -55, -45, -25, -15, 5, 15, 35, 45, 65, 75, 95, 105, 125, 135 };
        archerPosList = new List<int>() { -145, -125, -105, -85, -65, -45, -25, -5, 15, 35, 55, 75, 95, 115, 135 };
        witchPosList = new List<int>() { -125, -95, -65, -35, -5, 25, 55, 85, 115, 145 };

        GameObject p_object;

        prefab = Plunger;
        for (int i = 0; i < 4; i++)       // 뚫어뻥 20정도 생성 및 보관
        {
            p_object = Instantiate(prefab);  // 뚫어뻥 생성
            p_object.GetComponent<Mini05_Weapon>().mini05_Spawn = this;
            queue_BreakThrough.Enqueue(p_object);       // 오브젝트 풀링
        }

        prefab = Bomb;

        p_object = Instantiate(prefab);  // 도넛 폭탄 생성
        p_object.GetComponent<Mini05_Weapon>().mini05_Spawn = this;
        queue_DonutBomb.Enqueue(p_object);       // 오브젝트 풀링
        

        prefab = Tornado;

        p_object = Instantiate(prefab);  // 토네이도 생성
        p_object.GetComponent<Mini05_Weapon>().mini05_Spawn = this;
        queue_Tonedo.Enqueue(p_object);       // 오브젝트 풀링


        int randInt;

        prefab = knight;               // 검사...........................
        for (int i = 0; i < 20; i++)       // 검사 30정도 생성 및 보관
        {
            mon_Prefab = Instantiate(prefab);// 몬스터 생성
            randInt = Random.Range(0, knightPosList.Count);

            Mini05_Knight mini05_Knight = mon_Prefab.GetComponent<Mini05_Knight>();
            mini05_Knight.thisInt = knightPosList[randInt];
            mini05_Knight.thisStartInt = StartInt(knightPosList[randInt])[0];
            mini05_Knight.mini05_Spawn = this;

            mon_Prefab.transform.position = spawnTrans[StartInt(knightPosList[randInt])[1]].position;
            
            list_Knight.Add(mon_Prefab);       // 오브젝트 풀링

            knightPosList.RemoveAt(randInt);
        }

        prefab = archer;               // 궁수...........................
        for (int i = 0; i < 15; i++)       // 궁수 30정도 생성 및 보관
        {
            mon_Prefab = Instantiate(prefab);// 몬스터 생성
            randInt = Random.Range(0, archerPosList.Count);

            Mini05_Archer mini05_Archer = mon_Prefab.GetComponent<Mini05_Archer>();
            mini05_Archer.thisInt = archerPosList[randInt];
            mini05_Archer.thisStartInt = StartInt(archerPosList[randInt])[0];
            mini05_Archer.player = player;
            mini05_Archer.FailPos = failPos;
            mini05_Archer.mini05_Spawn = this;

            mon_Prefab.transform.position = spawnTrans[StartInt(archerPosList[randInt])[1]].position;

            list_Archer.Add(mon_Prefab);       // 오브젝트 풀링

            archerPosList.RemoveAt(randInt);
        }


		prefab = which;               // 마녀...........................
        for (int i = 0; i < 10; i++)       // 몬스터 50정도 생성 및 보관
		{
			mon_Prefab = Instantiate(prefab);// 몬스터 생성
            randInt = Random.Range(0, witchPosList.Count);

            Mini05_Witch mini05_Witch = mon_Prefab.GetComponent<Mini05_Witch>();
            mini05_Witch.thisInt = witchPosList[randInt];
            mini05_Witch.thisStartInt = StartInt(witchPosList[randInt])[0];
            mini05_Witch.player = player;
            mini05_Witch.mini05_Spawn = this;

            mon_Prefab.transform.position = spawnTrans[StartInt(witchPosList[randInt])[1]].position;

            list_Witch.Add(mon_Prefab);       // 오브젝트 풀링

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

        StartCoroutine(SpawnCoroutine_Knight());   // 검사 스폰 코루틴..(30개 반복)
        StartCoroutine(SpawnCoroutine_Archer());   // 궁수 스폰 코루틴..(30개 반복)
        StartCoroutine(SpawnCoroutine_Witch());   // 마녀 스폰 코루틴..(30개 반복)

        StartCoroutine(TimerChange());
        StartCoroutine(Time_Coroutine_S());
        
        hp_Int = 100;
    }


	void Update()
	{
        timeInt += Time.deltaTime;                // 지나간 시간
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

        if (Main.ins.nowPlayer.maxScore_List[4] >= scoreInt)    // 최고점을 못 넘은다면...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[4].ToString();
        }
        else        // 최고점을 넘은 경우 (신기록)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + scoreInt.ToString();

            Main.ins.nowPlayer.maxScore_List[4] = scoreInt;
            Main.ins.SaveData();

			GPGS_Button.interactable = true;        // 신기록을 세우면 점수 서버 등록 버튼을 활성화한다. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // 배경음악을 끈다.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;

    }


	public void Press_GPGS_05()   // 신기록 시, 서버에 등록
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no5, scoreInt);   // 점수를 서버에 등록
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no5);          // 리더보드를 띄운다.
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


    IEnumerator Time_Coroutine_S()       // 초 시간
    {
        time_S.text = timeInt_S.ToString();
        
        while (true)
        {
            yield return delay;
            timeInt_S++;

            if (timeInt_S.Equals(30))
            {
                scoreInt++;                  // 스코어 올리기
            }
            else if (timeInt_S.Equals(60))
            {
                timeInt_S = 0;
                timeInt_M++;
                time_M.text = timeInt_M.ToString();

                scoreInt++;                  // 스코어 올리기
            }

            time_S.text = timeInt_S.ToString();
        }
    }


    /////// 뚫어뻥 관련...

    public void InsertQueue_BreakThrough(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(뚫어뻥)
    {
        queue_BreakThrough.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_BreakThrough()           // 큐에서 객체를 빌려오는 함수(뚫어뻥)
    {
        GameObject t_object = queue_BreakThrough.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    //////////////// 도넛 폭탄 관련..

    public void InsertQueue_DonutBomb(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(도넛 폭탄)
    {
        queue_DonutBomb.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_DonutBomb()           // 큐에서 객체를 빌려오는 함수(도넛 폭탄)
    {
        GameObject t_object = queue_DonutBomb.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }


    //////////////// 토네이도 관련..

    public void InsertQueue_Tonedo(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(토네이도)
    {
        queue_Tonedo.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Tonedo()           // 큐에서 객체를 빌려오는 함수(토네이도)
    {
        GameObject t_object = queue_Tonedo.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }



    int[] StartInt(int randInt)
    {
        int tempInt = randInt;                      // 순서때문에 임시로 잠깐 받는다.
        int startInt = 0;     // 문 근처 위치로 갈때 쓰인다.
        int spawnInt = 0;     // 소환될 곳을 지정하는 변수

        if (tempInt < -104)     // 랜덤으로 뽑은 것이 -105 이하라면...
        {
            startInt = -130;         // 0번 스폰 근처로 이동
            spawnInt = 0;            // 0번 스폰지역으로 배정
        }
        else if (tempInt < -74)     // 랜덤으로 뽑은 것이 -75 이하라면...
        {
            startInt = -65;         // 0번 스폰 근처로 이동
            spawnInt = 0;            // 0번 스폰지역으로 배정
        }
        else if (tempInt < -44)     // 랜덤으로 뽑은 것이 -45 이하라면...
        {
            startInt = -65;         // 1번 스폰 근처로 이동
            spawnInt = 1;            // 1번 스폰지역으로 배정
        }
        else if (tempInt < -4)     // 랜덤으로 뽑은 것이 -5 이하라면...
        {
            startInt = 0;         // 1번 스폰 근처로 이동
            spawnInt = 1;            // 1번 스폰지역으로 배정
        }
        else if (tempInt < 36)     // 랜덤으로 뽑은 것이 35 이하라면...
        {
            startInt = 0;         // 2번 스폰 근처로 이동
            spawnInt = 2;            // 2번 스폰지역으로 배정
        }
        else if (tempInt < 66)     // 랜덤으로 뽑은 것이 65 이하라면...
        {
            startInt = 65;         // 2번 스폰 근처로 이동
            spawnInt = 2;            // 2번 스폰지역으로 배정
        }
        else if (tempInt < 96)     // 랜덤으로 뽑은 것이 95 이하라면...
        {
            startInt = 65;         // 3번 스폰 근처로 이동
            spawnInt = 3;            // 3번 스폰지역으로 배정
        }
        else                       // 랜덤으로 뽑은 것이 96 이상라면...
        {
            startInt = 130;         // 3번 스폰 근처로 이동
            spawnInt = 3;            // 3번 스폰지역으로 배정
        }

        int[] arr = new int[2];     // 배열 형태로 넘겨줌
        arr[0] = startInt;          // 위치..
        arr[1] = spawnInt;          // 스폰 지역 위치

        return arr;          // 리턴 값이 2개임...
    }



    /////////////////////// 코루틴 영역............

    IEnumerator SpawnCoroutine_Knight()   // 검사 스폰 코루틴..(30개 반복)
    {
        WaitForSeconds delay;

        while (true)
        {
            switch (Time_Level)
            {
                case 0:
                    delay = delay_Array[4];      // 7초
                    break;
                case 1:
                    delay = delay_Array[3];      // 6초
                    break;
                case 2:
                    delay = delay_Array[2];      // 5초
                    break;
                case 3:
                    delay = delay_Array[1];      // 4초
                    break;
                default:
                    delay = delay_Array[0];      // 3초
                    break;
            }

            if (!list_Knight.Count.Equals(0))                       // 검사 숫자 리스트에 하나 이상 있다면?
            {
                int randInt = Random.Range(0, list_Knight.Count);   // 랜덤으로 뽑고
                list_Knight[randInt].SetActive(true);               // 랜덤으로 뽑은 숫자를 활성화 시킨다.
                list_Knight.RemoveAt(randInt);                      // 중복 방지
            }

            yield return delay;
        }
    }

    IEnumerator SpawnCoroutine_Archer()   // 궁수 스폰 코루틴..(30개 반복)
    {
        WaitForSeconds delay;

        while (true)
        {
            switch (Time_Level)
            {
                case 0:
                    delay = delay_Array[9];     // 12초
                    break;
                case 1:
                    delay = delay_Array[8];     // 11초
                    break;
                case 2:
                    delay = delay_Array[7];     // 10초
                    break;
                case 3:
                    delay = delay_Array[6];     // 9초
                    break;
                default:
                    delay = delay_Array[5];     // 8초
                    break;
            }

            if (!list_Archer.Count.Equals(0))                       // 궁수 숫자 리스트에 하나 이상 있다면?
            {
                int randInt = Random.Range(0, list_Archer.Count);   // 랜덤으로 뽑고
                list_Archer[randInt].SetActive(true);               // 랜덤으로 뽑은 숫자를 활성화 시킨다.
                list_Archer.RemoveAt(randInt);                      // 중복 방지
            }

            yield return delay;
        }
    }

    IEnumerator SpawnCoroutine_Witch()   // 마녀 스폰 코루틴..(30개 반복)
    {
        WaitForSeconds delay;

        while (true)
        {
            switch (Time_Level)
            {
                case 0:
                    delay = delay_Array[13];     // 16초
                    break;
                case 1:
                    delay = delay_Array[12];     // 15초
                    break;
                case 2:
                    delay = delay_Array[11];     // 14초
                    break;
                case 3:
                    delay = delay_Array[10];     // 13초
                    break;
                default:
                    delay = delay_Array[9];     // 12초
                    break;
            }

            if (!list_Witch.Count.Equals(0))                       // 마녀 숫자 리스트에 하나 이상 있다면?
            {
                int randInt = Random.Range(0, list_Witch.Count);   // 랜덤으로 뽑고
                list_Witch[randInt].SetActive(true);               // 랜덤으로 뽑은 숫자를 활성화 시킨다.
                list_Witch.RemoveAt(randInt);                      // 중복 방지
            }

            yield return delay;
        }
    }


    IEnumerator TimerChange()       // 스폰 시간 조정 코루틴
    {
        Time_Level = 0;
        yield return delay_Time;   // 1분 후

        Time_Level = 1;

        yield return delay_Time;   // 1분 후
        yield return delay_Time;   // 1분 후
        yield return delay_Time;   // 1분 후
        Time_Level = 2;

        yield return delay_Time;   // 1분 후
        yield return delay_Time;   // 1분 후
        yield return delay_Time;   // 1분 후
        Time_Level = 3;

        yield return delay_Time;   // 1분 후
        yield return delay_Time;   // 1분 후
        Time_Level = 4;
    }
}
