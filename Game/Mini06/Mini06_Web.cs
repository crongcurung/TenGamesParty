using UnityEngine;

public class Mini06_Web : MonoBehaviour              // 그물에 부착됨...
{
    public Mini06_Spawn mini06_Spawn;                // 스폰 스크립트를 받아온다..
    Rigidbody rigid;

    string invoke_Text;

	void Awake()
	{
        rigid = transform.GetComponent<Rigidbody>();

        invoke_Text = "Invoke_Destroy_Web";
    }

	void OnEnable()     // 켜질떄...
    {
        Invoke(invoke_Text, 5.0f);
    }

	void OnDisable()    // 끝날떄...
	{
        rigid.velocity = Vector3.zero;
    }

    void Invoke_Destroy_Web()
    {
        mini06_Spawn.InsertQueue_Web(gameObject);        // 이 그물 반납
    }

}
