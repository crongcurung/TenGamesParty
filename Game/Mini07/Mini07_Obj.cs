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

        if (gameObject.CompareTag("Note"))          // 1Ä­ Àå¾Ö¹°   note
        {
            tagInt = 0;
        }
        else if (gameObject.CompareTag("Monster"))   // 2Ä­ Àå¾Ö¹°   monster
        {
            tagInt = 1;
        }
        else if (gameObject.CompareTag("Cushion"))        // 3Ä­ Àå¾Ö¹°
        {
            tagInt = 2;
        }
        else                                   // 4Ä­ Àå¾Ö¹°  (Çã´Ï)
        {
            tagInt = 3;
        }
    }


	public void End_Area()
	{
		switch (tagInt)
		{
			case 0:          // 1Ä­ Àå¾Ö¹°
				mini07_Spawn.InsertQueue_StopSign(transform.gameObject);
				break;
			case 1:          // 2Ä­ Àå¾Ö¹°
				mini07_Spawn.InsertQueue_Ban(transform.gameObject);
				break;
			case 2:          // 3Ä­ Àå¾Ö¹°
				mini07_Spawn.InsertQueue_Container(transform.gameObject);
				break;
			default:         // 4Å² Àå¾Ö¹°
				mini07_Spawn.InsertQueue_Sandwich(transform.gameObject);
				break;
		}

	}


	//void OnDisable()
	//{
	//	switch (tagInt)
	//	{
	//		case 0:          // 1Ä­ Àå¾Ö¹°
	//			mini07_Spawn.InsertQueue_StopSign(transform.gameObject);
	//			break;
	//		case 1:          // 2Ä­ Àå¾Ö¹°
	//			mini07_Spawn.InsertQueue_Ban(transform.gameObject);
	//			break;
	//		case 2:          // 3Ä­ Àå¾Ö¹°
	//			mini07_Spawn.InsertQueue_Container(transform.gameObject);
	//			break;
	//		default:         // 4Å² Àå¾Ö¹°
	//			mini07_Spawn.InsertQueue_Sandwich(transform.gameObject);
	//			break;
	//	}
	//}
}
