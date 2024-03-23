using UnityEngine;

public class Mini03_Tower : MonoBehaviour
{
    Transform patrol;          // ���� ���������� ��Ʈ�� ������Ʈ�� �޴� ����

    bool patrolBool = false;     // ��Ʈ�� ����ȭ ����

    [SerializeField] float speed = 7.0f;          // Ÿ�� ȸ�� ���ǵ�

       
    Vector3 randPos;

    void Start()
    {
        patrol = transform.parent.GetChild(4).transform;     // ���� 
    }

    void Update()
    {
        if (patrolBool.Equals(false))       // Ÿ�� �Һ��� ���� ��Ʈ�� ���°� �ƴ� ���
        {
            patrolBool = true;          // ��Ʈ�� ���·� �ٲ�
            
            int randInt = randInt = Random.Range(0, patrol.childCount);   // ���� ��Ʈ�� ��η�!
            randPos = patrol.GetChild(randInt).position;
        }

        Vector3 dir = randPos - this.transform.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);
        // ���� ��η� �̵�

        float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir));

        if (angle <= 1.0f)       // ��ΰ� ������ ���ٸ�...
        {
            patrolBool = false;      // ���� ��Ʈ���� �����ٰ� �˸�
        }
    }
}
