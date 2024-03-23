using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini06_Spawn : MonoBehaviour                          // 스폰 포스에 부착됨
{
    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // 점수를 서버에 등록시킬 수 있는 버튼

	[SerializeField] GameObject bee;
    [SerializeField] GameObject web;

    GameObject prefab;          // 프리팹 매니져에서 오브젝트를 받아올 변수
    GameObject mon_Prefab;      // 위에 프리팹 변수에서 몬스터 프리팹으로 뽑을 변수

    Vector3[] pos_Array = new Vector3[4];                       // 벌의 스폰 위치

    WaitForSeconds delay_Time;                                  // 시간 변화 시간
    WaitForSeconds[] delay_Array = new WaitForSeconds[4];       // 스폰 시간 배열

    int Time_Level;                                             // 시간 변화 레벨
    [SerializeField] Mini06_Player mini06_Player;                         // 플레이어 스크립트
    [SerializeField] Mini06_Bear mini06_Bear;                             // 곰 스크립트

    public List<GameObject> list_Bee = new List<GameObject>();     // 뚫어뻥 오브젝트 풀링
    Queue<GameObject> queue_Web = new Queue<GameObject>();     // 뚫어뻥 오브젝트 풀링

    int scoreInt = 0;

    [SerializeField] TextMeshProUGUI scoreText;                 // 박스에 의한 스코어 점수를 여기 스크립트에서 처리한다.
    [SerializeField] TextMeshProUGUI hive_Text;                 // 살아있는 꿀통의 개수를 알리는 텍스트
    int hive_Int = 0;

    void Awake()
    {
        Vector3 pos01 = new Vector3(-20.0f, 2.0f, -15.0f);     // 스폰 위치
        pos_Array[0] = pos01;
        pos01 = new Vector3(-20.0f, 2.0f, 15.0f);
        pos_Array[1] = pos01;
        pos01 = new Vector3(20.0f, 2.0f, -15.0f);
        pos_Array[2] = pos01;
        pos01 = new Vector3(20.0f, 2.0f, 15.0f);
        pos_Array[3] = pos01;


        delay_Time = new WaitForSeconds(60.0f);                // 시간 변화(1분)

        delay_Array[0] = new WaitForSeconds(5.0f);
        delay_Array[1] = new WaitForSeconds(4.0f);
        delay_Array[2] = new WaitForSeconds(3.0f);
        delay_Array[3] = new WaitForSeconds(2.0f);

        Mini06_Bee mini06_Bee;

        prefab = bee;               // 벌...........................
        for (int i = 0; i < 30; i++)       // 벌 30정도 생성 및 보관
        {
            mon_Prefab = Instantiate(prefab);      // 벌 생성

            mini06_Bee = mon_Prefab.GetComponent<Mini06_Bee>();
            mini06_Bee.mini06_Spawn = this;   // 이 스크립트를 넘긴다.(반납때문에...)
            mini06_Bee.mini06_Bear = mini06_Bear;
            mini06_Bee.mini06_Player = mini06_Player;

            list_Bee.Add(mon_Prefab);       // 오브젝트 풀링
        }

        
         
        prefab = web;               // 그물...........................
        for (int i = 0; i < 5; i++)       // 그물 5정도 생성 및 보관
        {
            mon_Prefab = Instantiate(prefab);// 그물 생성
            mon_Prefab.GetComponent<Mini06_Web>().mini06_Spawn = this;   // 이 스크립트를 넘긴다.(반납때문에...)
            queue_Web.Enqueue(mon_Prefab);       // 오브젝트 풀링
        }

        StartCoroutine(TimerChange());                 // 시간 변화 코루틴 실행
        StartCoroutine(SpawnCoroutine_Bee());          // 벌 스폰 코루틴 실행


        hive_Int = 12;
    }



    ///////////////////////// 일반 함수 구역


    public void InsertQueue_Web(GameObject p_object)     // 사용한 객체를 큐에 다시 반납시키는 함수(그물)
    {
        queue_Web.Enqueue(p_object);
        p_object.SetActive(false);
    }

    public GameObject GetQueue_Web()           // 큐에서 객체를 빌려오는 함수(그물)
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

        if (Main.ins.nowPlayer.maxScore_List[5] >= scoreInt)    // 최고점을 못 넘은다면...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[5].ToString();
        }
        else        // 최고점을 넘은 경우 (신기록)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + scoreInt.ToString();

            Main.ins.nowPlayer.maxScore_List[5] = scoreInt;
            Main.ins.SaveData();

			GPGS_Button.interactable = true;        // 신기록을 세우면 점수 서버 등록 버튼을 활성화한다. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // 배경음악을 끈다.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;
    }

	public void Press_GPGS_06()   // 신기록 시, 서버에 등록
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no6, scoreInt);   // 점수를 서버에 등록
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no6);          // 리더보드를 띄운다.
	}



	public void Score_Box()       // 스코어 올리는 함수(박스쪽에서 접근 함)
    {
        scoreInt++;
        scoreText.text = scoreInt.ToString("N0") ;     // 이 함수가 실행될때마다 스코어 텍스트 올라감
    }



    ///////////////////////  코루틴 구역...


    IEnumerator TimerChange()       // 스폰 시간 조정 코루틴                5, 4, 3, 2...?
    {
        yield return delay_Time;   // 1분 후

        Time_Level = 1;            // 시간 레벨 1로 바꿈

        yield return delay_Time;   // 2분 후
        yield return delay_Time;

        Time_Level = 2;            // 시간 레벨 2로 바꿈

        yield return delay_Time;   // 2분 후
        yield return delay_Time;

        Time_Level = 3;            // 시간 레벨 3로 바꿈
    }



    IEnumerator SpawnCoroutine_Bee()   // 벌 스폰 코루틴..(30개 반복)
    {
        WaitForSeconds delay;
        int randInt;

        while (true)
        {
            switch (Time_Level)                 // 시간 레벨에 따라..
            {
                case 0:
                    delay = delay_Array[0];      // 5초
                    break;
                case 1:
                    delay = delay_Array[1];      // 4초
                    break;
                case 2:
                    delay = delay_Array[2];      // 3초
                    break;
                default:
                    delay = delay_Array[2];      // 2초
                    break;
            }

            if (!list_Bee.Count.Equals(0))       // 벌 숫자 리스트에 하나도 없을 경우...
            {
                int randInt_Bee = Random.Range(0, list_Bee.Count);    // 벌 리스트 개수에 따라..
                randInt = Random.Range(0, pos_Array.Length);          // 4?
                GameObject spawn = list_Bee[randInt_Bee];             // 
                spawn.transform.position = pos_Array[randInt];        // 스폰 위치 조정
                spawn.SetActive(true);

                list_Bee.RemoveAt(randInt_Bee);   // 벌 지움
            }

            yield return delay;
        }
    }
}
