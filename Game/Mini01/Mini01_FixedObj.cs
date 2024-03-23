using UnityEngine;

public class Mini01_FixedObj : MonoBehaviour
{
    [SerializeField] float speedRot = 10.0f;     // 회전의 속도를 받는 변수(아이템 마다 회전 속도가 다를 수 있어서 시리얼 라이즈 필드를 씀...)

    void Update()
    {
        ItemRot();
    }


    void ItemRot()               // 아이템의 회전을 담당하는 함수
    {
        transform.Rotate(0, Time.deltaTime * speedRot, 0);
    }
}
