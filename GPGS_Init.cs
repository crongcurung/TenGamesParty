using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPGS_Init : MonoBehaviour
{

	public void Start()
	{
		LogIn_Button();         // ó�� �����ҋ� �α��� �ع�����
	}



    public void LogIn_Button()   // �α��� �ϱ�
    {
		GPGS_Binder.Inst.Login();
	}

}
