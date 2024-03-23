using UnityEngine;

public class Mini01_Donut : MonoBehaviour       // 하트 도넛에 부착됨
{
    [SerializeField] Transform itemPos;                   // 아이템은 보통 랜덤 위치 또는 특정 어딘가에 스폰하기 때문에 스폰 위치를 받기 위한 변수
    [SerializeField] float speedRot = 10.0f;     // 회전의 속도를 받는 변수(아이템 마다 회전 속도가 다를 수 있어서 시리얼 라이즈 필드를 씀...)
    [SerializeField] Mini01_Spawn mini01_Spawn;

    int randInt = 0;
    int prevInt = 0;           // 방금 위치가 나왔던 랜덤으로 뽑은 숫자

    void Start()
    {
        ResetItemPos();      // 처음부터 도넛이 어느에 생길지 실행
    }

    void Update()
    {
        ItemRot();
    }

    void ItemRot()               // 아이템의 회전을 담당하는 함수
    {
        transform.Rotate(0, Time.deltaTime * speedRot, 0);
    }


    public void ResetItemPos()             // 플레이어쪽에서 접근할 수 있게 public으로 함
    {
        randInt = Random.Range(0, itemPos.transform.childCount);        // 랜덤으로 하트 도넛 위치를 바꾼다.


        while (prevInt.Equals(randInt))
        {
            randInt = Random.Range(0, itemPos.transform.childCount);        // 랜덤으로 하트 도넛 위치를 바꾼다.
        }

        prevInt = randInt;

        transform.position = itemPos.transform.GetChild(randInt).transform.position;   // 도넛의 위치를 바꿈
        mini01_Spawn.GetQueue_Monster();       // 하트 도넛을 먹으면 몬스터가 나오도록 한다.
    }
}
