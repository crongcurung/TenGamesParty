using System.Collections;
using UnityEngine;

public class Mini05_Weapon : MonoBehaviour         // 뚫어뻥, 도넛폭탄, 토네이도에 붙어 있다.
{
	public Mini05_Spawn mini05_Spawn;      // 스폰 스크립트에서 각 무기의 리스트를 가져와야 해서 public으로 둔다.

	int weaponInt = 0;

	Rigidbody rigid;                
	Collider col;
	Renderer render_ChildD;
	Collider col_ChildD;
	GameObject child_Donut;

	string invoke_Text01;
	string invoke_Text02;

	float delayFloat;
	WaitForSeconds delay;

	void Awake()
	{
		invoke_Text01 = "Invoke_Weapon";

		if (transform.CompareTag("Spring"))         // 뚫어뻥
		{
			weaponInt = 0;                          // 뚫어뻥이라고 알린다.
			delayFloat = 2.0f;
		}
		else if (transform.CompareTag("Bear"))      // 도넛 폭탄
		{
			invoke_Text02 = "Invoke_Donut";

			weaponInt = 1;                          // 도넛 폭탄이라고 알린다.
			col = transform.GetComponent<Collider>();
			render_ChildD = transform.GetChild(0).GetComponent<Renderer>();
			col_ChildD = transform.GetChild(0).GetComponent<Collider>();
			child_Donut = transform.GetChild(1).gameObject;

			delayFloat = 4.0f;
		}
		else                                         // 토네이도
		{
			weaponInt = 2;                          // 토네이도라고 알린다.
			delayFloat = 6.0f;
		}

		delay = new WaitForSeconds(delayFloat);
		rigid = GetComponent<Rigidbody>();            // 이 무기의 리지드바디을 받는다.
	}

	void OnEnable()        // 활성화 될떄...
	{
		if (weaponInt.Equals(0))
		{
			StartCoroutine(Coroutine_0());
		}
		else if (weaponInt.Equals(1))
		{
			StartCoroutine(Coroutine_1());
		}
		else
		{
			StartCoroutine(Coroutine_2());
		}
	}

	void OnDisable()        // 비활성화 될때...
	{
		if (weaponInt.Equals(1))    // 도넛 폭탄일때...
		{
			render_ChildD.enabled = true;     // 도넛 폭탄 자식의 렌더(걍 폭탄 렌더)를 킨다(도넛 폭탄은 폭발하면 폭탄 렌더가 꺼져서...)
			col.enabled = true;               // 도넛 폭탄의 콜라이더(기본 폭탄 사이즈)을 킨다.(도넛 폭탄은 폭발하면 기본 폭탄 콜라이더가 꺼진다...)
			child_Donut.SetActive(false);     // 도넛 폭탄의 자식의 자식?(파티클)을 끈다.
			col_ChildD.enabled = false;       // 도넛 폭탄의 자식 콜라이더를 끈다.(큰 사이즈 범위)
		}

		rigid.velocity = Vector3.zero;   // 혹시 모르니 벨로시티를 제로로 맞춘다.
	}



	//////////////// 트리거 구역....

	void OnTriggerEnter(Collider other)        
	{
		if (other.gameObject.layer.Equals(3))        // 바닥, 성벽, 바위에 닿았을 경우    WALL
		{
			if (weaponInt.Equals(0))           // 뚫어뻥
			{
				mini05_Spawn.InsertQueue_BreakThrough(transform.gameObject);
			}
			else if (weaponInt.Equals(1))      // 도넛 폭탄
			{
				rigid.velocity = Vector3.zero;     // 일단 멈춤
				render_ChildD.enabled = false;     // 폭탄 렌더 지움
				col.enabled = false;               // 기본 폭탄 콜라이더를 끔
				child_Donut.SetActive(true);       // 파티클이랑 큰 폭탄 킴
				col_ChildD.enabled = true;         // 큰 폭탄 콜라이더 킴

				AudioMng.ins.PlayEffect("Bomb");    // 폭탄 터지는 소리

				CancelInvoke(invoke_Text01);    // 시간 재는 코루틴 끔
				Invoke(invoke_Text02, 1.0f);    // 도넛 폭탄 코루틴 킴
			}
		}
		else if (other.gameObject.layer.Equals(7))          // 몬스터에 닿았을 경우(기본 사이즈 폭탄)
		{
			if (weaponInt.Equals(0))          // 뚫어뻥
			{
				mini05_Spawn.InsertQueue_BreakThrough(transform.gameObject);
			}
			else if (weaponInt.Equals(1))     // 도넛 폭탄
			{
				rigid.velocity = Vector3.zero;     // 일단 멈춤
				render_ChildD.enabled = false;     // 도넛 폭탄 렌더를 지운다.
				col.enabled = false;               // 기본 폭탄 콜라이더를 끔
				child_Donut.SetActive(true);       // 큰 사이즈 폭탄 파티클을 킨다.
				col_ChildD.enabled = true;         // 큰 폭탄 콜라이더 킴 

				AudioMng.ins.PlayEffect("Bomb");    // 폭탄 터지는 소리

				CancelInvoke(invoke_Text01);     // 시간 재는 코루틴 끔
				Invoke(invoke_Text02, 1.0f);      // 도넛 폭탄 코루틴 킴
			}

			other.gameObject.SetActive(false);        // 몬스터를 없앤다.
		}

	}


	void OnTriggerStay(Collider other)          // 도넛 폭탄만...
	{
		if (weaponInt.Equals(1))     // 도넛 폭탄
		{
			if (other.gameObject.layer.Equals(7))     // 몬스터가 큰 폭탄 안에 있다면...
			{
				other.gameObject.SetActive(false);   // 몬스터를 없앰
			}
		}
	}



	////////////////////////// 인보크 구역...

	void Invoke_Donut()
	{
		mini05_Spawn.InsertQueue_DonutBomb(transform.gameObject);       // 일정 시간이 지나면 도넛 폭탄 반납
	}


	IEnumerator Coroutine_0()
	{
		yield return delay;
		mini05_Spawn.InsertQueue_BreakThrough(transform.gameObject);      // 뚫어뻥 반납
	}


	IEnumerator Coroutine_1()
	{
		yield return delay;
		mini05_Spawn.InsertQueue_DonutBomb(transform.gameObject);         // 도넛 폭탄 반납
	}


	IEnumerator Coroutine_2()
	{
		yield return delay;
		mini05_Spawn.InsertQueue_Tonedo(transform.gameObject);            // 토네이도 반납
	}

}
