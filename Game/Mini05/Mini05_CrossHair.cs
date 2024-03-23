using UnityEngine;

public class Mini05_CrossHair : MonoBehaviour       // 뚫어뻥, 도넛 폭탄 크로스 헤어에 부착됨
{
	Animator anim;

	int moveId;
	int shotId;

	void Start()
	{
		anim = transform.GetComponent<Animator>();         // 크로스를 축소, 확대하는 애니메이션을 받는다.

		moveId = Animator.StringToHash("isMove");              
		shotId = Animator.StringToHash("isShot");
	}

	public void MovingState(bool state)        // 플레이어의 이동에 따라 애니메이션 발동 함수
	{
		anim.SetBool(moveId, state);
	}

	public void Shooting(bool state)        // 플레이어의 공격에 따라 애니메이션 발동 함수
	{
		anim.SetBool(shotId, state);
	}
}
