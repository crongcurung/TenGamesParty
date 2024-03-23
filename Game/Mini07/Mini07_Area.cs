using System;
using System.Collections;
using UnityEngine;

public class Mini07_Area : MonoBehaviour
{
    public Transform playerTrans;               // �÷��̾� ��ġ
    public Mini07_Spawn mini07_Spawn;           // ���� ��ũ��Ʈ

    float destroyDistance = 100.0f;             // �� �ٴ��� ���ֱ� ���� �Ÿ� ��

    WaitForSeconds waitCoroutine;               // �ڷ�ƾ ����ȭ ����

    string invoke_Text;

    public Action area_Action;

    void Start()
    {
        invoke_Text = "wait_Spawn";
        waitCoroutine = new WaitForSeconds(0.1f);                 // �ڷ�ƾ ����ȭ
    }

    void OnEnable()             // Ȱ��ȭ�ɶ�...
    {
        StartCoroutine(coroutine_Update());         // �÷��̾�� �ٴڰ��� �Ÿ��� �˾ƺ��� �ڷ�ƾ(�Ź� ����)
    }


    IEnumerator coroutine_Update()            // �÷��̾�� �ٴڰ��� ���̸� Ȯ���ϱ� ���� �ڷ�ƾ(�Ź� ����)
    {
        yield return waitCoroutine;
        while (true)
        {
            if (playerTrans.position.z - transform.position.z >= destroyDistance)    // �÷��̾ ������ �����ٴ� �÷��̾ �� �ٴڰ� �־����� ���
            {
                Invoke(invoke_Text, 0.3f);                   // 0.3�� �Ŀ� ���� �ٴ��� �����϶�� �˸�

                area_Action?.Invoke();
                area_Action = null;

                mini07_Spawn.InsertQueue_Ground(transform.gameObject);
            }

            yield return waitCoroutine;           // �ڷ�ƾ ����ȭ
        }
    }

    void wait_Spawn()         // ���� �ٴ��� �����ϱ� ���� �Լ�
    {
        mini07_Spawn.SpawnArea();       // �����϶�� �˸���. (�� �ٴ����� ��)
    }

}
