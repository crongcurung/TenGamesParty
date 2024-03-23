using System.Collections;
using UnityEngine;

public class Mini02_Spawn : MonoBehaviour
{
    [SerializeField] GameObject Guest01;
    [SerializeField] GameObject Guest02;
    [SerializeField] GameObject Guest03;

    [SerializeField] Transform goalTarget;            // 카운터 지점
    [SerializeField] Transform spawnTarget;           // 손님 스폰 지점
    [SerializeField] Mini02_Player mini02_Player;     // 플레이어 스크립트
    [SerializeField] Mini02_CountLine mini02_CountLine;

    GameObject prefab;          // 프리팹 매니져에서 오브젝트를 받아올 변수

    GameObject[] array_Guest = new GameObject[15];

    WaitForSeconds delay01;
    WaitForSeconds delay02;
    WaitForSeconds delay03;
    WaitForSeconds delay04;
    WaitForSeconds delay05;
    WaitForSeconds delay06;
    WaitForSeconds delay07;
    WaitForSeconds delay08;
    WaitForSeconds delay09;
    WaitForSeconds delay10;
	WaitForSeconds delay11;
	WaitForSeconds delay12;

	int scoreCount = 0;     // 스코어 변수
    int spawnLevel = 0;     // 갱신한 레벨 변수

    

    void Awake()
    {
        GameObject p_object;
        Mini02_Guest Mini02_Guest;

        prefab = Guest01;               // 손님 오브젝트..

        for (int i = 0; i < 5; i++)       // 손님 20정도 생성 및 보관
        {
            p_object = Instantiate(prefab);      // 손님 생성
            Mini02_Guest = p_object.GetComponent<Mini02_Guest>();        // 손님의 스크립트를 가지고 와서

            Mini02_Guest.target = goalTarget;               // 카운터 라인을 보낸다.
            Mini02_Guest.mini02_Player = mini02_Player;     // 플레이어 스크립트를 보낸다.
            Mini02_Guest.mini02_Spawn = this;               // 스폰 스크립트를 보낸다.

            array_Guest[i] = p_object;
            p_object.SetActive(false);                  // 비활성화
        }

        prefab = Guest02;               // 손님 오브젝트..
        prefab = Guest02;               // 손님 오브젝트..

        for (int i = 0; i < 5; i++)       // 손님 20정도 생성 및 보관
        {
            p_object = Instantiate(prefab);      // 손님 생성
            Mini02_Guest = p_object.GetComponent<Mini02_Guest>();        // 손님의 스크립트를 가지고 와서

            Mini02_Guest.target = goalTarget;               // 카운터 라인을 보낸다.
            Mini02_Guest.mini02_Player = mini02_Player;     // 플레이어 스크립트를 보낸다.
            Mini02_Guest.mini02_Spawn = this;               // 스폰 스크립트를 보낸다.
            array_Guest[i + 5] = p_object;
            p_object.SetActive(false);                  // 비활성화
        }

        prefab = Guest03;               // 손님 오브젝트..

        for (int i = 0; i < 5; i++)       // 손님 20정도 생성 및 보관
        {
            p_object = Instantiate(prefab);      // 손님 생성
            Mini02_Guest = p_object.GetComponent<Mini02_Guest>();        // 손님의 스크립트를 가지고 와서

            Mini02_Guest.target = goalTarget;               // 카운터 라인을 보낸다.
            Mini02_Guest.mini02_Player = mini02_Player;     // 플레이어 스크립트를 보낸다.
            Mini02_Guest.mini02_Spawn = this;               // 스폰 스크립트를 보낸다.

            array_Guest[i + 10] = p_object;
            p_object.SetActive(false);                  // 비활성화
        }
    }

    void Start()
    {
        delay01 = new WaitForSeconds(58.0f);          // 코루틴 최적화
        delay02 = new WaitForSeconds(55.0f);
        delay03 = new WaitForSeconds(52.0f);
        delay04 = new WaitForSeconds(49.0f);
        delay05 = new WaitForSeconds(46.0f);
        delay06 = new WaitForSeconds(43.0f);
        delay07 = new WaitForSeconds(40.0f);
        delay08 = new WaitForSeconds(37.0f);
        delay09 = new WaitForSeconds(34.0f);
        delay10 = new WaitForSeconds(31.0f);
		delay11 = new WaitForSeconds(28.0f);
		delay12 = new WaitForSeconds(23.0f);          // 여기만 5초로 낮춤

		StartCoroutine(SpawnCoroutine());
    }

    public int spawnInt = 0;


    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            spawnInt++;

            if (spawnInt.Equals(10))             // 현재 스폰된 몬스터가 10마리라면..
            {
                mini02_CountLine.GameOver();          // 게임오버...


                yield return null;
            }


            int randInt = Random.Range(0, 15);
               
            while (array_Guest[randInt].activeSelf.Equals(true))           // 중복 방지..
            {
                randInt = Random.Range(0, 15);
            }
            GameObject geust = array_Guest[randInt];
            geust.SetActive(true);
            geust.transform.position = spawnTarget.position; // 스폰 지점으로 손님을 보낸다.

            switch (spawnLevel)     // 갱신한 레벨에 따라
            {
                case 0:
                    yield return delay01;

                    break;
                case 1:
                    yield return delay02;

                    break;
                case 2:
                    yield return delay03;

                    break;
                case 3:
                    yield return delay04;

                    break;
                case 4:
                    yield return delay05;

                    break;
                case 5:
                    yield return delay06;

                    break;
                case 6:
                    yield return delay07;

                    break;
                case 7:
                    yield return delay08;

                    break;
                case 8:
                    yield return delay09;

                    break;
				case 9:
					yield return delay10;

					break;
				case 10:
					yield return delay11;

					break;
				default:                 // 11
                    yield return delay12;

                    break;
            }
            
        }
    }


    public void ScoreUp()
    {
        scoreCount++;       // 스코어가 늘어나면

        switch (scoreCount)        
        {
            case 3:               // 3번씩 되면 갱신한다.
                spawnLevel++;
                break;
            case 6:
                spawnLevel++;
                break;
            case 9:
                spawnLevel++;
                break;
            case 12:
                spawnLevel++;
                break;
            case 15:
                spawnLevel++;
                break;
            case 18:
                spawnLevel++;
                break;
            case 21:
                spawnLevel++;
                break;
            case 24:
                spawnLevel++;
                break;
            case 27:
                spawnLevel++;
                break;
            case 30:
                spawnLevel++;
                break;
			case 33:
				spawnLevel++;
				break;
			case 36:
				spawnLevel++;
				break;
			default:
                break;
        }
    }
}
