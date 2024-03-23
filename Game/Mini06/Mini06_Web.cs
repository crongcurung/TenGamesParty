using UnityEngine;

public class Mini06_Web : MonoBehaviour              // �׹��� ������...
{
    public Mini06_Spawn mini06_Spawn;                // ���� ��ũ��Ʈ�� �޾ƿ´�..
    Rigidbody rigid;

    string invoke_Text;

	void Awake()
	{
        rigid = transform.GetComponent<Rigidbody>();

        invoke_Text = "Invoke_Destroy_Web";
    }

	void OnEnable()     // ������...
    {
        Invoke(invoke_Text, 5.0f);
    }

	void OnDisable()    // ������...
	{
        rigid.velocity = Vector3.zero;
    }

    void Invoke_Destroy_Web()
    {
        mini06_Spawn.InsertQueue_Web(gameObject);        // �� �׹� �ݳ�
    }

}
