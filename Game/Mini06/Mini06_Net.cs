using UnityEngine;

public class Mini06_Net : MonoBehaviour
{
    public bool isAttack = false;         // 플레이어가 오른쪽 버튼을 눌렀냐?

    string tag01;

	void Awake()
	{
        tag01 = "Monster";
    }

	void OnTriggerEnter(Collider other)
    {
        if (isAttack.Equals(true))          // 플레이어가 오른쪽 버튼을 눌렀냐? (버튼을 안 눌렀을 때를 발동 안하게 할려고 이렇게 함)
        {
            if (other.gameObject.CompareTag(tag01))       // 벌에 닿으면...
            {
                other.gameObject.SetActive(false);       // 나중에 반납으로 바꿔라
                AudioMng.ins.PlayEffect("Score_Up");    // 벌 죽는 소리
            }
        }
    }
}
