using UnityEngine;

public class Mini08_MiniMap : MonoBehaviour
{
    [SerializeField] bool x;
    [SerializeField] bool y;
    [SerializeField] bool z;

    [SerializeField] Transform target;                                     // �÷��̾� ��ġ

    void Update()
    {
        if (!target)
        {
            return;
        }

        transform.position = new Vector3(                           // �÷��̾� ��ġ�� ���� �̴ϸ� ī�޶� �̵�
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z));
    }
}
