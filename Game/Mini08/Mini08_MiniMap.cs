using UnityEngine;

public class Mini08_MiniMap : MonoBehaviour
{
    [SerializeField] bool x;
    [SerializeField] bool y;
    [SerializeField] bool z;

    [SerializeField] Transform target;                                     // 플레이어 위치

    void Update()
    {
        if (!target)
        {
            return;
        }

        transform.position = new Vector3(                           // 플레이어 위치에 따라 미니맵 카메라 이동
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z));
    }
}
