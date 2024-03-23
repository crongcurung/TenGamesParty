using System.Collections;
using UnityEngine;

public class Mini04_Bowl : MonoBehaviour       // 그릇 5개에 부착됨...
{
    WaitForSeconds delay_01;
    WaitForSeconds delay_02;
    WaitForEndOfFrame delay_03;

    public bool isBuddle_End = false;      // 이 그릇이 부들거리는 지 묻는 변수

    Vector3 rot01;
    Vector3 rot02;
    Vector3 rot03;
    Vector3 rot04;

    Vector3 fly_Pos01;
    Vector3 fly_Pos02;
    Vector3 fly_Pos03;
    Vector3 fly_Pos04;


    void Start()
	{
        rot01 = new Vector3(10, 0, 10);
        rot02 = new Vector3(10, 0, -10);
        rot03 = new Vector3(-10, 0, 10);
        rot04 = new Vector3(-10, 0, -10);

        fly_Pos01 = new Vector3(2, 2, 2);
        fly_Pos02 = new Vector3(2, 2, -2);
        fly_Pos03 = new Vector3(-2, 2, 2);
        fly_Pos04 = new Vector3(-2, 2, -2);

        delay_01 = new WaitForSeconds(0.02f);
        delay_02 = new WaitForSeconds(0.1f);
        delay_03 = new WaitForEndOfFrame();
    }

	void OnDisable()                 // 스테이지 셋팅할떄 모음 그릇이 비활성화됨
	{
        isBuddle_End = false;        // 부들 거림이 끝났는지 묻는 변수 초기화....(튕겨져 나가는 그릇을 중간에 없앨 수 있기 떄문에 여기서도 초기화 한다..)
    }

	public void Buddle_Bowl()       // 부들거리는 코루틴 실행.. 커브 스크립트에서 접근할 수 있도록 public으로 함
    {
        StartCoroutine(Buddle_Coroutine());    // 부들거리는 코루틴 실행
    }

    IEnumerator Buddle_Coroutine()       // 부들거리는 코루틴..
    {
        int count = 30;      // 30번 정도 부들거림...
        Vector3 tempRot;

        while (count > 0)
        {
            switch (count % 4)   
            {
                case 0:
                    tempRot = rot01;
                    break;
                case 1:
                    tempRot = rot02;
                    break;
                case 2:
                    tempRot = rot03;
                    break;
                default:
                    tempRot = rot04;
                    break;
            }

            transform.rotation = Quaternion.Euler(tempRot);     // 순서대로 회전값을 바꿈

            count--;
            yield return delay_01;
        }

        yield return delay_01;
        isBuddle_End = true;        // 부들거리는 것이 끝났다고 알림


        int randInt = Random.Range(0, 4);     // 위 4개 위치를 랜덤으로 하나 뽑는다..
        Vector3 tempPos;

        switch (randInt)
        {
            case 0:
                tempPos = fly_Pos01;
                break;
            case 1:
                tempPos = fly_Pos02;
                break;
            case 2:
                tempPos = fly_Pos03;
                break;
            default:
                tempPos = fly_Pos04;
                break;
        }
        float time = 0;       // 2초를 받는다..

        while (time < 2)
        {
            time += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, tempPos, Time.deltaTime * 4.0f);  // 랜덤으로 뽑은 위치를 향해 튕겨져 나가게 한다..
            transform.rotation = Quaternion.Euler(new Vector3(time * 500.0f, transform.rotation.y, transform.rotation.z));
            yield return delay_03;
        }

        isBuddle_End = false;      // 부들거리는게 끝났다는 사실을 초기화 한다..
        yield return delay_02;
    }

}
