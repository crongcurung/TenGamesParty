using System;
using System.Collections;
using UnityEngine;

public class Mini07_Area : MonoBehaviour
{
    public Transform playerTrans;               // 플레이어 위치
    public Mini07_Spawn mini07_Spawn;           // 스폰 스크립트

    float destroyDistance = 100.0f;             // 이 바닥을 없애기 위한 거리 값

    WaitForSeconds waitCoroutine;               // 코루틴 최적화 변수

    string invoke_Text;

    public Action area_Action;

    void Start()
    {
        invoke_Text = "wait_Spawn";
        waitCoroutine = new WaitForSeconds(0.1f);                 // 코루틴 최적화
    }

    void OnEnable()             // 활성화될때...
    {
        StartCoroutine(coroutine_Update());         // 플레이어와 바닥과의 거리를 알아보는 코루틴(매번 실행)
    }


    IEnumerator coroutine_Update()            // 플레이어와 바닥과의 사이를 확인하기 위한 코루틴(매번 실행)
    {
        yield return waitCoroutine;
        while (true)
        {
            if (playerTrans.position.z - transform.position.z >= destroyDistance)    // 플레이어가 설정한 값보다는 플레이어가 이 바닥과 멀어지는 경우
            {
                Invoke(invoke_Text, 0.3f);                   // 0.3초 후에 다음 바닥을 스폰하라고 알림

                area_Action?.Invoke();
                area_Action = null;

                mini07_Spawn.InsertQueue_Ground(transform.gameObject);
            }

            yield return waitCoroutine;           // 코루틴 최적화
        }
    }

    void wait_Spawn()         // 다음 바닥을 스폰하기 위한 함수
    {
        mini07_Spawn.SpawnArea();       // 스폰하라고 알린다. (이 바닥으로 함)
    }

}
