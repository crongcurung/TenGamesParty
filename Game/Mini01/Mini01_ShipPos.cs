using System.Collections;
using UnityEngine;

public class Mini01_ShipPos : MonoBehaviour
{
    [SerializeField] GameObject ship;

    GameObject shipPrefab;         // �H�� �������� ���� ����
    Transform shipPos;             // ó�� �¸��� ������ ������ �޴� ����
    int shipCount = 0;             // ���ݱ��� �¸��� �� � ���Գ� ���� ����(3�������� �ҷ���...)

    [SerializeField] Transform waterTiles;

    [SerializeField] GameObject playerTrans;                   // �÷��̾ �޴� ����
    [SerializeField] Mini01_Player mini01_Player;         // �÷��̾��� ��ũ��Ʈ�� �޴� ����

    WaitForSeconds delay;          // �ڷ�ƾ ����ȭ ����


    void Start()
    {
        shipPrefab = ship;                  // ������ �Ŵ������� �H���� ã�Ƽ� �����´�.
        shipPos = transform;    // ó�� ���������� ��ġ�� �޾ƿ´�.

        delay = new WaitForSeconds(14.0f);                                // 14�� ���� �H���� �������� �Ѵ�.
        StartCoroutine(SpawnShip());                                      // �ڷ�ƾ ����!
    }


    IEnumerator SpawnShip()           // �H���� ������ �ϴ� �ڷ�ƾ (3�� �ۿ� ���ϱ� �Ѵ�..)
    {
        shipCount++;                  // �¸��� ���ö����� ī��Ʈ�� �ϳ� �ø���.
        if (shipCount.Equals(4))           // ī��Ʈ�� 4���� �Ǹ�
        {
            yield break;              // �¸��� �� �̻� �������� �ʰ� �ڷ�ƾ�� �����Ų��.
        }
        
        GameObject ship =  Instantiate(shipPrefab, shipPos.transform.position, Quaternion.identity);      // �¸� ����!
        Mini01_Ship mini01_Ship = ship.GetComponent<Mini01_Ship>();
        ship.SetActive(true);

        mini01_Ship.waterTiles = waterTiles;
        mini01_Ship.player = playerTrans;
        mini01_Ship.mini01_Player = mini01_Player;

        yield return delay;                // 14�� ����....
        StartCoroutine(SpawnShip());       // 14�ʰ� ������ �ٽ� �ڷ�ƾ ����!
    }

}
