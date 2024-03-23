using UnityEngine;

public class Mini01_Ship : MonoBehaviour
{
    public Transform waterTiles;               // �� Ÿ���� ��� �ִ� ������Ʈ�� �޴� ����
    Transform nextTile;                 // ���� �� Ÿ���� �������� �޴� ����
    public GameObject player;                   // �÷��̾ �޴� ����
    public Mini01_Player mini01_Player;         // �÷��̾��� ��ũ��Ʈ�� �޴� ����

    Vector3 originalPos;                 // �H���� ó�� �����Ǵ� ��ġ ����
    float speed = 2.5f;                  // �H�� ���ǵ� ����

    int childCount = 0;                  // ���� �� Ÿ���� ���°���� ���� ����

    bool isPlayer = false;               // �� �¸� �÷��̾ ��Ҵ��� ���� ����

    int layerInt01;
    int layerInt02;

    string Compare01;
    string Compare02;
    string Compare03;

    void Start()
    {
        originalPos = this.gameObject.transform.position;                           // ó�� ������ ���� ��ġ�� ��´�.
        nextTile = waterTiles.GetChild(childCount).transform;                       // ī��Ʈ�� 0�̴�, 0��° �� Ÿ�� ��ġ�� ������ �´�.

        layerInt01 = LayerMask.NameToLayer("Water");
        layerInt02 = LayerMask.NameToLayer("WALL");

        Compare01 = "Player";
        Compare02 = "BlackHole";
        Compare03 = "Cushion";
    }

    void FixedUpdate()
    {
        if (!this.gameObject.transform.position.Equals(nextTile.position))         // ���� Ÿ�ϰ� ���� Ÿ���� �ٸ��ٸ�???????????
        {
            MoveShip();
        }
        else
        {
            childCount++;        // ���� Ÿ�� ��ȣ�� �ٲ�
            MoveShip();
        }
    }

    void MoveShip()
    {
        nextTile = waterTiles.GetChild(childCount).transform;                  // �̰� ����ȭ �ʿ�!!!!!!!!!!

        Vector3 dir = -1 * (nextTile.position - transform.position);

        transform.position = Vector3.MoveTowards(gameObject.transform.position, nextTile.position, speed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.fixedDeltaTime);
        // �H���� �̵��� ȸ���� �����Ѵ�...

        if (isPlayer.Equals(true))           // �÷��̾ �H��� ��Ұ�, �¸��� �������� ���� ���� ���� �ƴ϶��...
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, nextTile.position, speed * Time.fixedDeltaTime);
            // �� �¸� �߾ӿ� �÷��̾ ���� �Ѵ�.(������...)
        }
    }



    //////////////////////////////////////
    // Ʈ���� ����


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(layerInt01))          // ���� ��� �� Ÿ���� ���̾ �ٸ� ������ �ٲ۴�.
        {
            other.gameObject.layer = layerInt02;           // �̷��� �÷��̾��� ����ĳ��Ʈ�� ���̶�� ���� ���Ѵ�.(����� ���� �ʿ�!!)
        }

        if (other.gameObject.CompareTag(Compare02))           // Water52..    �������� ��°�!
        {
            isPlayer = false;
            transform.position = originalPos;
            childCount = 0;
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Compare01))               // �÷��̾ ������...
        {
            if (mini01_Player.isShip.Equals(true))       // �迡 Ÿ�� �ִٰ� �˷���
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
        if (other.gameObject.layer.Equals(layerInt02))    // ���� �ٲ� ���̾ ������
        {
            if (other.CompareTag(Compare03))
            {
                other.gameObject.layer = layerInt01;     // �ٽ� �� ���̾�� ��ȯ
            }
        }
    }
}
