using UnityEngine;

public class Mini07_Coin : MonoBehaviour       // 코인에 부착됨
{
	public Mini07_Spawn mini07_Spawn;      // 스폰 스크립트(스폰할때 미리 받아온다...)

    [SerializeField] protected float speedRot = 10.0f;     // 회전의 속도를 받는 변수(아이템 마다 회전 속도가 다를 수 있어서 시리얼 라이즈 필드를 씀...)


    void Update()
    {
        ItemRot();
    }

    void ItemRot()               // 아이템의 회전을 담당하는 함수
    {
        transform.Rotate(0, Time.deltaTime * speedRot, 0);
    }



    public void End_Area()
    {
        mini07_Spawn.InsertQueue_Coin(transform.gameObject);       // 코인을 반납한다..
    }



 //   void OnDisable()          // 비활성화는 플레이어에서 한다.
	//{
	//	mini07_Spawn.InsertQueue_Coin(transform.gameObject);       // 코인을 반납한다..
	//}
}
