using System.Collections;
using UnityEngine;

public class Mini08_Cemetry : MonoBehaviour          // 묘지에 부착됨
{
	[SerializeField] Mini08_Player mini08_Player;    // 플레이어 스크립트를 가져온다.
	[SerializeField] Mini08_Spawn mini08_Spawn;      // 스폰 스크립트를 가져온다.
	[SerializeField] Mesh[] Cemetry_Mesh;            // 묘지 상태 변화에 따른 메쉬를 가져온다(총 5개)

	[SerializeField] SpriteRenderer spriteRander;
	[SerializeField] Sprite[] sprite_Array;

    MeshFilter meshFilter;                           // 이 묘지의 상태를 변화해줄 메쉬 컴포넌트

	[SerializeField] int thisInt;                    // 이 묘지의 번호(묘지를 아이템에 의해 삭제되었을때를 위해...)

	Coroutine coroutine;                             // 상태 변화 코루틴을 받을 변수

	int meshInt = 0;                                 // 상태 변화에 따른 번호 부여 변수

	WaitForSeconds delay01;
	WaitForSeconds delay02;


	Transform top_Child;

	Vector3 oriPos;
	Vector3 pos05;

	Quaternion[] Rot_Array = new Quaternion[5];

	Coroutine ghost_Coroutine;

	void Awake()
    {
		top_Child = transform.GetChild(0).transform;

		oriPos = new Vector3(0, 1.976761f, 1.673394f);

		pos05 = new Vector3(-3.64f, 0.94f, 1.673396f);


		Rot_Array[0] = top_Child.localRotation;
		Rot_Array[1] = Quaternion.Euler(new Vector3(-90, 10, 0));
		Rot_Array[2] = Quaternion.Euler(new Vector3(-90, 20, 0));
		Rot_Array[3] = Quaternion.Euler(new Vector3(-90, 30, 0));
		Rot_Array[4] = Quaternion.Euler(new Vector3(-30.922f, -90, 90));

		meshFilter = transform.GetChild(2).GetComponent<MeshFilter>();
		delay01 = new WaitForSeconds(20.0f);
		delay02 = new WaitForSeconds(30.0f);
	}



	void OnEnable()          // 켜질 떄..
	{
		mini08_Player.CemetryText_Fuction(1);                 // 묘지 텍스트에 하나 올린다.
		coroutine = StartCoroutine(Chage_Coroutine());        // 상태 변화 코루틴을 실행시킨다..
	}

	void OnDisable()         // 꺼질 떄...
	{
		mini08_Player.CemetryText_Fuction(-1);              // 묘지가 하나 줄었다고 알린다.
		mini08_Player.Cemetry_Minus(thisInt);               // 묘지 리스트에 넣는다.(삭제 스킬에 씌일 곳)

		meshInt = 0;                                        // 메쉬 초기화한다.
		meshFilter.sharedMesh = Cemetry_Mesh[meshInt];      // 메쉬를 초기화한다.
		spriteRander.sprite = sprite_Array[meshInt];

		top_Child.localPosition = oriPos;
		top_Child.localRotation = Rot_Array[0];
	}


	public int Check_Cemetry()     // 묘지 상태 확인
	{
		if (meshInt.Equals(0))     // 아직 이 묘지를 상태 변화를 안해서 그냥 리턴해버림
		{
			return 10;             // 아무 숫자나 리턴하면 된다.
		}
		else
		{
			return meshInt;                // 아까 담아뒀던 숫자를 넘긴다.(숫자에 따라 사용 자원이 다르기 때문에...)
		}
	}


	public void Player_Fixed()      // 플레이어가 이 묘지를 고치는 중이라면..
	{
		if (meshInt.Equals(4))     // 만약 이 묘지의 상태가 완전 파괴 상태였다면...
		{
			StopCoroutine(ghost_Coroutine);
			mini08_Player.BrokenText_Fuction(-1);   // 부서진 텍스트를 하나 줄인다..
		}

		StopCoroutine(coroutine);  // 상태 변화 코루틴을 중단시킨다.

		int tempInt = meshInt;         // 현재 상태를 잠시 담아두고
		meshInt = 0;                   // 현재 상태를 초기화한다..
		meshFilter.sharedMesh = Cemetry_Mesh[meshInt];      // 초기화 상태로 형태를 바꿈
		spriteRander.sprite = sprite_Array[meshInt];

		top_Child.localRotation = Rot_Array[meshInt];
		top_Child.localPosition = oriPos;

		coroutine = StartCoroutine(Chage_Coroutine());      // 다시 상태 변화 코루틴 실행
	}



	/////////////////////////  코루틴 구역..


	IEnumerator Chage_Coroutine()
	{
		while (!meshInt.Equals(4))         // 마지막 부서진 상태가 아니라면...
		{
			yield return delay01;          // 20초간 쉰 다음..
			meshInt++;                     // 상태를 변화한다..
			meshFilter.sharedMesh = Cemetry_Mesh[meshInt];
			spriteRander.sprite = sprite_Array[meshInt];
			top_Child.localRotation = Rot_Array[meshInt];

			if (meshInt.Equals(4))         // 완전 부서진 상태라면..
			{
				AudioMng.ins.LoopEffect(false);    // 흙, 돌 소리 무한루프 종료
				AudioMng.ins.PlayEffect("Cemetry");    // 묘지 소리
				top_Child.localPosition = pos05;

				ghost_Coroutine = StartCoroutine(Ghost_Coroutine());      // 유령 스폰 코루틴을 실행한다...
				mini08_Player.BrokenText_Fuction(1);    // 부서진 텍스트를 하나 올린다.
			}
		}
	}


	IEnumerator Ghost_Coroutine()      // 유령 스폰 코루틴..
	{
		while (true)
		{
			GameObject obj = mini08_Spawn.GetQueue_Chost();    // 유령 하나를 가져온다.

			if (obj != null)      // null 이면 60마리 이상이라는 뜻!!!
			{
				obj.transform.position = transform.position;    // 유령을 이 묘지 위치로 가져온다.
			}

			yield return delay02;       // 30초 간 쉰다.
		}
	}
}
