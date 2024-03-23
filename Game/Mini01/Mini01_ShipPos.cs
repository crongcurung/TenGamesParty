using System.Collections;
using UnityEngine;

public class Mini01_ShipPos : MonoBehaviour
{
    [SerializeField] GameObject ship;

    GameObject shipPrefab;         // 똇목 프리펩을 받을 변수
    Transform shipPos;             // 처음 뗏목이 나오는 지점을 받는 변수
    int shipCount = 0;             // 지금까지 뗏목이 총 몇개 나왔냐 묻는 변수(3개까지만 할려고...)

    [SerializeField] Transform waterTiles;

    [SerializeField] GameObject playerTrans;                   // 플레이어를 받는 변수
    [SerializeField] Mini01_Player mini01_Player;         // 플레이어의 스크립트를 받는 변수

    WaitForSeconds delay;          // 코루틴 최적화 변수


    void Start()
    {
        shipPrefab = ship;                  // 프리팹 매니져에서 똇목을 찾아서 가져온다.
        shipPos = transform;    // 처음 시작지점의 위치를 받아온다.

        delay = new WaitForSeconds(14.0f);                                // 14초 마다 똇목이 나오도록 한다.
        StartCoroutine(SpawnShip());                                      // 코루틴 시작!
    }


    IEnumerator SpawnShip()           // 똇목을 나오게 하는 코루틴 (3번 밖에 안하긴 한다..)
    {
        shipCount++;                  // 뗏목이 나올때마다 카운트를 하나 올린다.
        if (shipCount.Equals(4))           // 카운트가 4개가 되면
        {
            yield break;              // 뗏목을 더 이상 생성하지 않고 코루틴을 종료시킨다.
        }
        
        GameObject ship =  Instantiate(shipPrefab, shipPos.transform.position, Quaternion.identity);      // 뗏목 생성!
        Mini01_Ship mini01_Ship = ship.GetComponent<Mini01_Ship>();
        ship.SetActive(true);

        mini01_Ship.waterTiles = waterTiles;
        mini01_Ship.player = playerTrans;
        mini01_Ship.mini01_Player = mini01_Player;

        yield return delay;                // 14초 마다....
        StartCoroutine(SpawnShip());       // 14초가 끝나면 다시 코루틴 시작!
    }

}
