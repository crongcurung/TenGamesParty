using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini10_Stage : MonoBehaviour         // ������ ������
{
    [SerializeField] GameObject[] Stage_Array01;
    [SerializeField] GameObject[] Stage_Array02;
    [SerializeField] GameObject[] Stage_Array03;
    [SerializeField] GameObject[] Stage_Array04;
    [SerializeField] GameObject[] Stage_Array05;
    [SerializeField] GameObject[] Stage_Array06;
    [SerializeField] GameObject[] Stage_Array07;
    [SerializeField] GameObject[] Stage_Array08;
    [SerializeField] GameObject[] Stage_Array09;
    [SerializeField] GameObject[] Stage_Array10;

    [SerializeField] Mini10_Player mini10_Player;      // �÷��̾� ��ũ��Ʈ

    [SerializeField] TextMeshProUGUI stageText;        // ����� ������������ �˸��� �ؽ�Ʈ
    [SerializeField] Image timeImage02;                     // �� �̹���

    GameObject prefab;          // ������ �Ŵ������� ������Ʈ�� �޾ƿ� ����

    List<int> list_Int01 = new List<int>();      // �ߺ� ���� ����Ʈ
    List<int> list_Int02 = new List<int>();
    List<int> list_Int03 = new List<int>();
    List<int> list_Int04 = new List<int>();
    List<int> list_Int05 = new List<int>();
    List<int> list_Int06 = new List<int>();
    List<int> list_Int07 = new List<int>();
    List<int> list_Int08 = new List<int>();
    List<int> list_Int09 = new List<int>();
    List<int> list_Int10 = new List<int>();

    GameObject p_object;

    int timerInt = 0;                   // ���̵� �� �ð� ����

    Coroutine coroutine_Comment07;      // ���_07�� Ȱ���ϱ� ���� �ڷ�ƾ ����

    int stageLevel;
    int stageCount = 0;
    public int stageInt = 0;

    bool isFirst = false;

    GameObject prevStage;
    GameObject tempStage;

    WaitForSeconds[] delay_Array = new WaitForSeconds[10];

    string invoke_Text;

    bool nextStage = true;                        // ���� ���������� ���Ĵ� ����

    void Awake()
    {
        invoke_Text = "Invoke_Stage";

        delay_Array[0] = new WaitForSeconds(45.0f);      // 45, 60, 75, 90, 105, 120, 135, 150, 165, 180
        delay_Array[1] = new WaitForSeconds(60.0f);
        delay_Array[2] = new WaitForSeconds(75.0f);
        delay_Array[3] = new WaitForSeconds(90.0f);
        delay_Array[4] = new WaitForSeconds(105.0f);
        delay_Array[5] = new WaitForSeconds(120.0f);
        delay_Array[6] = new WaitForSeconds(135.0f);
        delay_Array[7] = new WaitForSeconds(150.0f);
        delay_Array[8] = new WaitForSeconds(165.0f);
        delay_Array[9] = new WaitForSeconds(180.0f);

        List_Setting();       // �ߺ����� ����Ʈ ����
        Stage_Spawn();
    }

    


    public Vector3 Stage_Spawn()
    {
        GameObject stagePrefab;

        if (stageInt.Equals(0))       // ó������ �������� �ʴ´�.
        {

        }
        else
        {
            StopCoroutine(coroutine_Comment07);            // ���� �ð� �ڷ�ƾ ����
        }

        //stageLevel = 3;
        switch (stageLevel)       // �������� ������ ����..
        {
            case 0:
                stagePrefab = Stage01_Spawn();
                break;
            case 1:
                stagePrefab = Stage02_Spawn();
                break;
            case 2:
                stagePrefab = Stage03_Spawn();
                break;
            case 3:
                stagePrefab = Stage04_Spawn();
                break;
            case 4:
                stagePrefab = Stage05_Spawn();
                break;
            case 5:
                stagePrefab = Stage06_Spawn();
                break;
            case 6:
                stagePrefab = Stage07_Spawn();
                break;
            case 7:
                stagePrefab = Stage08_Spawn();
                break;
            case 8:
                stagePrefab = Stage09_Spawn();
                break;
            default:
                stagePrefab = Stage10_Spawn();
                break;
        }

        p_object = Instantiate(stagePrefab);


        if ((stageCount % 2).Equals(0))                     // �������� ��, ������ ��ġ
        {
            p_object.transform.position = new Vector3(0, stageCount * 10, 0);
        }
        else
        {
            p_object.transform.position = new Vector3(30, stageCount * 10, 0);
        }

        stageInt++;   
        stageText.text = stageInt.ToString();      // �� ��° ������������ �ؽ�Ʈ�� �˸�

        stageCount++;
        stageLevel++;

        if (stageLevel.Equals(10))        // �������� ������ 10�̶�� 0���� �ٽ� ����
        {
            stageLevel = 0;
        }

        if (stageCount.Equals(100))       // ���������� 100���� �Ǹ� 
        {
            stageCount = 0;
            List_Setting();       // �ߺ����� ����Ʈ ����
        }

        tempStage = p_object;

        nextStage = true;                                                // ���� ���������� ���ٰ� �˷���
        Invoke(invoke_Text, 1.0f);                                    // 1�� �� ���� ���������� ����
        return p_object.transform.position;
    }


    void Update()
    {
        SliderCount(stageLevel - 1);       // �ð� ǥ��
    }

    

    void SliderCount(int stageInt)      // �������� �� �����̴� ī��Ʈ �Լ�
    {
        if (nextStage.Equals(true))    // ���� ���������� ����
        {
            nextStage = false;    // ���ƹ���

            switch (stageInt)
            {
                case 0:
                    timerInt = 0;           // �� ���̵��� �� �ð�
                    break;
                case 1:
                    timerInt = 1;         
                    break;
                case 2:
                    timerInt = 2;
                    break;
                case 3:
                    timerInt = 3;
                    break;
                case 4:
                    timerInt = 4;
                    break;
                case 5:
                    timerInt = 5;
                    break;
                case 6:
                    timerInt = 6;
                    break;
                case 7:
                    timerInt = 7;
                    break;
                case 8:
                    timerInt = 8;
                    break;
                default:
                    timerInt = 9;
                    break;
            }

            timeImage02.fillAmount = 0;                 // �ð� �̹��� �ʱ�ȭ
            coroutine_Comment07 = StartCoroutine(RemainTime(timerInt));       // ���� �ð� üũ �ڷ�ƾ...
        }


        timeImage02.fillAmount += Time.deltaTime / (45.0f + timerInt * 15.0f);       // ���� �ð� Ÿ�̸Ӹ� ���� 
    }

    

    IEnumerator RemainTime(int stageTime)            // ���������� ���� �ð� üũ �ڷ�ƾ (���������� �ð�, �����ð� ����)
    {
        switch (stageTime)       // 1 ~ 3�ܰ� = 10��, 4 ~ 6�ܰ� = 20��, 7 ~ 10�ܰ� = 30��
        {
            case 0:
                yield return delay_Array[0];          
                break;
            case 1:
                yield return delay_Array[1];
                break;
            case 2:
                yield return delay_Array[2];
                break;
            case 3:
                yield return delay_Array[3];
                break;
            case 4:
                yield return delay_Array[4];
                break;
            case 5:
                yield return delay_Array[5];
                break;
            case 6:
                yield return delay_Array[6];
                break;
            case 7:
                yield return delay_Array[7];
                break;
            case 8:
                yield return delay_Array[8];
                break;
            default:
                yield return delay_Array[9];
                break;
        }

        mini10_Player.MinusHp(true);
    }

    void Invoke_Stage()
    {
        if (isFirst.Equals(true))
        {
            Destroy(prevStage);
        }
        else
        {
            isFirst = true;
        }

        prevStage = tempStage;
    }

    GameObject Stage01_Spawn()         // �������� ���� 1�� �� ����
    {
        int randInt = Random.Range(0, list_Int01.Count);
        int stageInt = list_Int01[randInt];

        //stageInt = 9;
        prefab = Stage_Array01[stageInt];
        
        list_Int01.RemoveAt(randInt);

        return prefab;
    }

    GameObject Stage02_Spawn()         // �������� ���� 2�� �� ����
    {
        int randInt = Random.Range(0, list_Int02.Count);
        int stageInt = list_Int02[randInt];

        //stageInt = 1;
        prefab = Stage_Array02[stageInt];

        list_Int02.RemoveAt(randInt);

        return prefab;
    }

    GameObject Stage03_Spawn()         // �������� ���� 3�� �� ����
    {
        int randInt = Random.Range(0, list_Int03.Count);
        int stageInt = list_Int03[randInt];

        //stageInt = 9;
        prefab = Stage_Array03[stageInt];

        list_Int03.RemoveAt(randInt);

        return prefab;
    }

    GameObject Stage04_Spawn()         // �������� ���� 4�� �� ����
    {
        int randInt = Random.Range(0, list_Int04.Count);
        int stageInt = list_Int04[randInt];

        //stageInt = 0;
        prefab = Stage_Array04[stageInt];

        list_Int04.RemoveAt(randInt);

        return prefab;
    }

    GameObject Stage05_Spawn()         // �������� ���� 5�� �� ����
    {
        int randInt = Random.Range(0, list_Int05.Count);
        int stageInt = list_Int05[randInt];

        //stageInt = 9;
        prefab = Stage_Array05[stageInt];

        list_Int05.RemoveAt(randInt);

        return prefab;
    }

    GameObject Stage06_Spawn()         // �������� ���� 6�� �� ����
    {
        int randInt = Random.Range(0, list_Int06.Count);
        int stageInt = list_Int06[randInt];

        //stageInt = 9;
        prefab = Stage_Array06[stageInt];

        list_Int06.RemoveAt(randInt);

        return prefab;
    }

    GameObject Stage07_Spawn()         // �������� ���� 7�� �� ����
    {
        int randInt = Random.Range(0, list_Int07.Count);
        int stageInt = list_Int07[randInt];

        //stageInt = 9;
        prefab = Stage_Array07[stageInt];

        list_Int07.RemoveAt(randInt);

        return prefab;
    }

    GameObject Stage08_Spawn()         // �������� ���� 8�� �� ����
    {
        int randInt = Random.Range(0, list_Int08.Count);
        int stageInt = list_Int08[randInt];

        //stageInt = 9;
        prefab = Stage_Array08[stageInt];

        list_Int08.RemoveAt(randInt);

        return prefab;
    }

    GameObject Stage09_Spawn()         // �������� ���� 9�� �� ����
    {
        int randInt = Random.Range(0, list_Int09.Count);
        int stageInt = list_Int09[randInt];

        //stageInt = 9;
        prefab = Stage_Array09[stageInt];

        list_Int09.RemoveAt(randInt);

        return prefab;
    }

    GameObject Stage10_Spawn()         // �������� ���� 10�� �� ����
    {
        int randInt = Random.Range(0, list_Int10.Count);
        int stageInt = list_Int10[randInt];

        //stageInt = 9;
        prefab = Stage_Array10[stageInt];

        list_Int10.RemoveAt(randInt);

        return prefab;
    }

    

    void List_Setting()       // �ߺ����� ����Ʈ ����
    {
        for (int i = 0; i < 10; i++)
        {
            list_Int01.Add(i);
            list_Int02.Add(i);
            list_Int03.Add(i);
            list_Int04.Add(i);
            list_Int05.Add(i);
            list_Int06.Add(i);
            list_Int07.Add(i);
            list_Int08.Add(i);
            list_Int09.Add(i);
            list_Int10.Add(i);
        }
    }
}
