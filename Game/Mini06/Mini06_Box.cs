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

		if (hp_Int.Equals(1))           // �� �� ���� 70%
		{
			slider.value = 0.7f;
		}
		else if (hp_Int.Equals(2))      // �� �� ���� 40%
		{
			slider.value = 0.4f;
		}
		else                             // �� �� ���� 0%
		{
			AudioMng.ins.PlayEffect("Bomb");    // �ڽ� ����
			slider.value = 0.0f;

			mini06_Spawn.End_Box();
		}
	}


	


	////////////// �ڷ�ƾ ����...


	IEnumerator Score_Box()             // ���ھ� �ø��� �ڷ�ƾ
	{
		while (hp_Int < 3)
		{
			yield return delay;          // 1�� ����..

			mini06_Spawn.Score_Box();    // ���� ��ũ��Ʈ �ȿ� ���ھ� �ø��� �Լ��� ����
		}
	}
}
