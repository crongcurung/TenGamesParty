using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mini06_Box : MonoBehaviour
{
	[SerializeField] Mini06_Spawn mini06_Spawn;

	Slider slider;
	public int hp_Int;


	WaitForSeconds delay;

	void Start()
	{
		slider = transform.GetChild(0).GetChild(0).GetComponent<Slider>();

		slider.value = 1.0f;
		hp_Int = 0;

		delay = new WaitForSeconds(30.0f);
		StartCoroutine(Score_Box());
	}


	public void Hit_Fuction()
	{
		hp_Int++;

		if (hp_Int.Equals(1))           // 한 대 맞음 70%
		{
			slider.value = 0.7f;
		}
		else if (hp_Int.Equals(2))      // 두 대 맞음 40%
		{
			slider.value = 0.4f;
		}
		else                             // 세 대 맞음 0%
		{
			AudioMng.ins.PlayEffect("Bomb");    // 박스 폭발
			slider.value = 0.0f;

			mini06_Spawn.End_Box();
		}
	}


	


	////////////// 코루틴 구역...


	IEnumerator Score_Box()             // 스코어 올리는 코루틴
	{
		while (hp_Int < 3)
		{
			yield return delay;          // 1초 마다..

			mini06_Spawn.Score_Box();    // 스폰 스크립트 안에 스코어 올리는 함수를 실행
		}
	}
}
