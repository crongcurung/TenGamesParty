using System.Collections;
using UnityEngine;

public class Mini04_Bowl : MonoBehaviour       // �׸� 5���� ������...
{
    WaitForSeconds delay_01;
    WaitForSeconds delay_02;
    WaitForEndOfFrame delay_03;

    public bool isBuddle_End = false;      // �� �׸��� �ε�Ÿ��� �� ���� ����

    Vector3 rot01;
    Vector3 rot02;
    Vector3 rot03;
    Vector3 rot04;

    Vector3 fly_Pos01;
    Vector3 fly_Pos02;
    Vector3 fly_Pos03;
    Vector3 fly_Pos04;


    void Start()
	{
        rot01 = new Vector3(10, 0, 10);
        rot02 = new Vector3(10, 0, -10);
        rot03 = new Vector3(-10, 0, 10);
        rot04 = new Vector3(-10, 0, -10);

        fly_Pos01 = new Vector3(2, 2, 2);
        fly_Pos02 = new Vector3(2, 2, -2);
        fly_Pos03 = new Vector3(-2, 2, 2);
        fly_Pos04 = new Vector3(-2, 2, -2);

        delay_01 = new WaitForSeconds(0.02f);
        delay_02 = new WaitForSeconds(0.1f);
        delay_03 = new WaitForEndOfFrame();
    }

	void OnDisable()                 // �������� �����ҋ� ���� �׸��� ��Ȱ��ȭ��
	{
        isBuddle_End = false;        // �ε� �Ÿ��� �������� ���� ���� �ʱ�ȭ....(ƨ���� ������ �׸��� �߰��� ���� �� �ֱ� ������ ���⼭�� �ʱ�ȭ �Ѵ�..)
    }

	public void Buddle_Bowl()       // �ε�Ÿ��� �ڷ�ƾ ����.. Ŀ�� ��ũ��Ʈ���� ������ �� �ֵ��� public���� ��
    {
        StartCoroutine(Buddle_Coroutine());    // �ε�Ÿ��� �ڷ�ƾ ����
    }

    IEnumerator Buddle_Coroutine()       // �ε�Ÿ��� �ڷ�ƾ..
    {
        int count = 30;      // 30�� ���� �ε�Ÿ�...
        Vector3 tempRot;

        while (count > 0)
        {
            switch (count % 4)   
            {
                case 0:
                    tempRot = rot01;
                    break;
                case 1:
                    tempRot = rot02;
                    break;
                case 2:
                    tempRot = rot03;
                    break;
                default:
                    tempRot = rot04;
                    break;
            }

            transform.rotation = Quaternion.Euler(tempRot);     // ������� ȸ������ �ٲ�

            count--;
            yield return delay_01;
        }

        yield return delay_01;
        isBuddle_End = true;        // �ε�Ÿ��� ���� �����ٰ� �˸�


        int randInt = Random.Range(0, 4);     // �� 4�� ��ġ�� �������� �ϳ� �̴´�..
        Vector3 tempPos;

        switch (randInt)
        {
            case 0:
                tempPos = fly_Pos01;
                break;
            case 1:
                tempPos = fly_Pos02;
                break;
            case 2:
                tempPos = fly_Pos03;
                break;
            default:
                tempPos = fly_Pos04;
                break;
        }
        float time = 0;       // 2�ʸ� �޴´�..

        while (time < 2)
        {
            time += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, tempPos, Time.deltaTime * 4.0f);  // �������� ���� ��ġ�� ���� ƨ���� ������ �Ѵ�..
            transform.rotation = Quaternion.Euler(new Vector3(time * 500.0f, transform.rotation.y, transform.rotation.z));
            yield return delay_03;
        }

        isBuddle_End = false;      // �ε�Ÿ��°� �����ٴ� ����� �ʱ�ȭ �Ѵ�..
        yield return delay_02;
    }

}
