using UnityEngine;

public class Mini01_Ship : MonoBehaviour
{
    public Transform waterTiles;               // 물 타일이 들어 있는 오브젝트를 받는 변수
    Transform nextTile;                 // 다음 물 타일이 무엇인지 받는 변수
    public GameObject player;                   // 플레이어를 받는 변수
    public Mini01_Player mini01_Player;         // 플레이어의 스크립트를 받는 변수

    Vector3 originalPos;                 // 똇목이 처음 생성되는 위치 저장
    float speed = 2.5f;                  // 똇목 스피드 변수

    int childCount = 0;                  // 현재 물 타일이 몇번째인지 묻는 변수

    bool isPlayer = false;               // 이 뗏목에 플레이어가 닿았는지 묻는 변수

    int layerInt01;
    int layerInt02;

    string Compare01;
    string Compare02;
    string Compare03;

    void Start()
    {
        originalPos = this.gameObject.transform.position;                           // 처음 생성된 곳의 위치를 담는다.
        nextTile = waterTiles.GetChild(childCount).transform;                       // 카운트가 0이니, 0번째 물 타일 위치를 가지고 온다.

        layerInt01 = LayerMask.NameToLayer("Water");
        layerInt02 = LayerMask.NameToLayer("WALL");

        Compare01 = "Player";
        Compare02 = "BlackHole";
        Compare03 = "Cushion";
    }

    void FixedUpdate()
    {
        if (!this.gameObject.transform.position.Equals(nextTile.position))         // 현재 타일과 다음 타일이 다르다면???????????
        {
            MoveShip();
        }
        else
        {
            childCount++;        // 다음 타일 번호로 바꿈
            MoveShip();
        }
    }

    void MoveShip()
    {
        nextTile = waterTiles.GetChild(childCount).transform;                  // 이거 최적화 필요!!!!!!!!!!

        Vector3 dir = -1 * (nextTile.position - transform.position);

        transform.position = Vector3.MoveTowards(gameObject.transform.position, nextTile.position, speed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.fixedDeltaTime);
        // 똇목의 이동과 회전을 실행한다...

        if (isPlayer.Equals(true))           // 플레이어가 똇목과 닿았고, 뗏목이 마지막을 향해 가는 중이 아니라면...
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, nextTile.position, speed * Time.fixedDeltaTime);
            // 이 뗏목 중앙에 플레이어를 오게 한다.(서서히...)
        }
    }



    //////////////////////////////////////
    // 트리거 구역


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(layerInt01))          // 현재 닿는 물 타일의 레이어를 다른 것으로 바꾼다.
        {
            other.gameObject.layer = layerInt02;           // 이러면 플레이어의 레이캐스트는 물이라고 인지 못한다.(강사님 도움 필요!!)
        }

        if (other.gameObject.CompareTag(Compare02))           // Water52..    마지막에 닿는곳!
        {
            isPlayer = false;
            transform.position = originalPos;
            childCount = 0;
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Compare01))               // 플레이어에 닿으면...
        {
            if (mini01_Player.isShip.Equals(true))       // 배에 타고 있다고 알려줌
            {
                
                isPlayer = true;
            }
            else
            {
                isPlayer = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(layerInt02))    // 물이 바뀐 레이어에 닿으면
        {
            if (other.CompareTag(Compare03))
            {
                other.gameObject.layer = layerInt01;     // 다시 물 레이어로 변환
            }
        }
    }
}
