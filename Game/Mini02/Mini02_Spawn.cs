using System.Collections;
using UnityEngine;

public class Mini02_Spawn : MonoBehaviour
{
    [SerializeField] GameObject Guest01;
    [SerializeField] GameObject Guest02;
    [SerializeField] GameObject Guest03;

    [SerializeField] Transform goalTarget;            // ī���� ����
    [SerializeField] Transform spawnTarget;           // �մ� ���� ����
    [SerializeField] Mini02_Player mini02_Player;     // �÷��̾� ��ũ��Ʈ
    [SerializeField] Mini02_CountLine mini02_CountLine;

    GameObject prefab;          // ������ �Ŵ������� ������Ʈ�� �޾ƿ� ����

    GameObject[] array_Guest = new GameObject[15];

    WaitForSeconds delay01;
    WaitForSeconds delay02;
    WaitForSeconds delay03;
    WaitForSeconds delay04;
    WaitForSeconds delay05;
    WaitForSeconds delay06;
    WaitForSeconds delay07;
    WaitForSeconds delay08;
    WaitForSeconds delay09;
    WaitForSeconds delay10;
	WaitForSeconds delay11;
	WaitForSeconds delay12;

	int scoreCount = 0;     // ���ھ� ����
    int spawnLevel = 0;     // ������ ���� ����

    

    void Awake()
    {
        GameObject p_object;
        Mini02_Guest Mini02_Guest;

        prefab = Guest01;               // �մ� ������Ʈ..

        for (int i = 0; i < 5; i++)       // �մ� 20���� ���� �� ����
        {
            p_object = Instantiate(prefab);      // �մ� ����
            Mini02_Guest = p_object.GetComponent<Mini02_Guest>();        // �մ��� ��ũ��Ʈ�� ������ �ͼ�

            Mini02_Guest.target = goalTarget;               // ī���� ������ ������.
            Mini02_Guest.mini02_Player = mini02_Player;     // �÷��̾� ��ũ��Ʈ�� ������.
            Mini02_Guest.mini02_Spawn = this;               // ���� ��ũ��Ʈ�� ������.

            array_Guest[i] = p_object;
            p_object.SetActive(false);                  // ��Ȱ��ȭ
        }

        prefab = Guest02;               // �մ� ������Ʈ..
        prefab = Guest02;               // �մ� ������Ʈ..

        for (int i = 0; i < 5; i++)       // �մ� 20���� ���� �� ����
        {
            p_object = Instantiate(prefab);      // �մ� ����
            Mini02_Guest = p_object.GetComponent<Mini02_Guest>();        // �մ��� ��ũ��Ʈ�� ������ �ͼ�

            Mini02_Guest.target = goalTarget;               // ī���� ������ ������.
            Mini02_Guest.mini02_Player = mini02_Player;     // �÷��̾� ��ũ��Ʈ�� ������.
            Mini02_Guest.mini02_Spawn = this;               // ���� ��ũ��Ʈ�� ������.
            array_Guest[i + 5] = p_object;
            p_object.SetActive(false);                  // ��Ȱ��ȭ
        }

        prefab = Guest03;               // �մ� ������Ʈ..

        for (int i = 0; i < 5; i++)       // �մ� 20���� ���� �� ����
        {
            p_object = Instantiate(prefab);      // �մ� ����
            Mini02_Guest = p_object.GetComponent<Mini02_Guest>();        // �մ��� ��ũ��Ʈ�� ������ �ͼ�

            Mini02_Guest.target = goalTarget;               // ī���� ������ ������.
            Mini02_Guest.mini02_Player = mini02_Player;     // �÷��̾� ��ũ��Ʈ�� ������.
            Mini02_Guest.mini02_Spawn = this;               // ���� ��ũ��Ʈ�� ������.

            array_Guest[i + 10] = p_object;
            p_object.SetActive(false);                  // ��Ȱ��ȭ
        }
    }

    void Start()
    {
        delay01 = new WaitForSeconds(58.0f);          // �ڷ�ƾ ����ȭ
        delay02 = new WaitForSeconds(55.0f);
        delay03 = new WaitForSeconds(52.0f);
        delay04 = new WaitForSeconds(49.0f);
        delay05 = new WaitForSeconds(46.0f);
        delay06 = new WaitForSeconds(43.0f);
        delay07 = new WaitForSeconds(40.0f);
        delay08 = new WaitForSeconds(37.0f);
        delay09 = new WaitForSeconds(34.0f);
        delay10 = new WaitForSeconds(31.0f);
		delay11 = new WaitForSeconds(28.0f);
		delay12 = new WaitForSeconds(23.0f);          // ���⸸ 5�ʷ� ����

		StartCoroutine(SpawnCoroutine());
    }

    public int spawnInt = 0;


    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            spawnInt++;

            if (spawnInt.Equals(10))             // ���� ������ ���Ͱ� 10�������..
            {
                mini02_CountLine.GameOver();          // ���ӿ���...


                yield return null;
            }


            int randInt = Random.Range(0, 15);
               
            while (array_Guest[randInt].activeSelf.Equals(true))           // �ߺ� ����..
            {
                randInt = Random.Range(0, 15);
            }
            GameObject geust = array_Guest[randInt];
            geust.SetActive(true);
            geust.transform.position = spawnTarget.position; // ���� �������� �մ��� ������.

            switch (spawnLevel)     // ������ ������ ����
            {
                case 0:
                    yield return delay01;

                    break;
                case 1:
                    yield return delay02;

                    break;
                case 2:
                    yield return delay03;

                    break;
                case 3:
                    yield return delay04;

                    break;
                case 4:
                    yield return delay05;

                    break;
                case 5:
                    yield return delay06;

                    break;
                case 6:
                    yield return delay07;

                    break;
                case 7:
                    yield return delay08;

                    break;
                case 8:
                    yield return delay09;

                    break;
				case 9:
					yield return delay10;

					break;
				case 10:
					yield return delay11;

					break;
				default:                 // 11
                    yield return delay12;

                    break;
            }
            
        }
    }


    public void ScoreUp()
    {
        scoreCount++;       // ���ھ �þ��

        switch (scoreCount)        
        {
            case 3:               // 3���� �Ǹ� �����Ѵ�.
                spawnLevel++;
                break;
            case 6:
                spawnLevel++;
                break;
            case 9:
                spawnLevel++;
                break;
            case 12:
                spawnLevel++;
                break;
            case 15:
                spawnLevel++;
                break;
            case 18:
                spawnLevel++;
                break;
            case 21:
                spawnLevel++;
                break;
            case 24:
                spawnLevel++;
                break;
            case 27:
                spawnLevel++;
                break;
            case 30:
                spawnLevel++;
                break;
			case 33:
				spawnLevel++;
				break;
			case 36:
				spawnLevel++;
				break;
			default:
                break;
        }
    }
}
