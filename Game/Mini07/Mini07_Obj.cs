using UnityEngine;

public class Mini07_Obj : MonoBehaviour
{
    public Mini07_Spawn mini07_Spawn;
    int tagInt;

    void Awake()
    {
        if (mini07_Spawn == null)
        {
            mini07_Spawn = GameObject.FindGameObjectWithTag("Finish").GetComponent<Mini07_Spawn>();
        }

        if (gameObject.CompareTag("Note"))          // 1ĭ ��ֹ�   note
        {
            tagInt = 0;
        }
        else if (gameObject.CompareTag("Monster"))   // 2ĭ ��ֹ�   monster
        {
            tagInt = 1;
        }
        else if (gameObject.CompareTag("Cushion"))        // 3ĭ ��ֹ�
        {
            tagInt = 2;
        }
        else                                   // 4ĭ ��ֹ�  (���)
        {
            tagInt = 3;
        }
    }


	public void End_Area()
	{
		switch (tagInt)
		{
			case 0:          // 1ĭ ��ֹ�
				mini07_Spawn.InsertQueue_StopSign(transform.gameObject);
				break;
			case 1:          // 2ĭ ��ֹ�
				mini07_Spawn.InsertQueue_Ban(transform.gameObject);
				break;
			case 2:          // 3ĭ ��ֹ�
				mini07_Spawn.InsertQueue_Container(transform.gameObject);
				break;
			default:         // 4Ų ��ֹ�
				mini07_Spawn.InsertQueue_Sandwich(transform.gameObject);
				break;
		}

	}


	//void OnDisable()
	//{
	//	switch (tagInt)
	//	{
	//		case 0:          // 1ĭ ��ֹ�
	//			mini07_Spawn.InsertQueue_StopSign(transform.gameObject);
	//			break;
	//		case 1:          // 2ĭ ��ֹ�
	//			mini07_Spawn.InsertQueue_Ban(transform.gameObject);
	//			break;
	//		case 2:          // 3ĭ ��ֹ�
	//			mini07_Spawn.InsertQueue_Container(transform.gameObject);
	//			break;
	//		default:         // 4Ų ��ֹ�
	//			mini07_Spawn.InsertQueue_Sandwich(transform.gameObject);
	//			break;
	//	}
	//}
}
