using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPGS_Init : MonoBehaviour
{

	public void Start()
	{
		LogIn_Button();         // 처음 시작할떄 로그인 해버리기
	}



    public void LogIn_Button()   // 로그인 하기
    {
		GPGS_Binder.Inst.Login();
	}

}
