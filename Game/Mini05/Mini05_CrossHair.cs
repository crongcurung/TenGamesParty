using UnityEngine;

public class Mini05_CrossHair : MonoBehaviour       // �վ, ���� ��ź ũ�ν� �� ������
{
	Animator anim;

	int moveId;
	int shotId;

	void Start()
	{
		anim = transform.GetComponent<Animator>();         // ũ�ν��� ���, Ȯ���ϴ� �ִϸ��̼��� �޴´�.

		moveId = Animator.StringToHash("isMove");              
		shotId = Animator.StringToHash("isShot");
	}

	public void MovingState(bool state)        // �÷��̾��� �̵��� ���� �ִϸ��̼� �ߵ� �Լ�
	{
		anim.SetBool(moveId, state);
	}

	public void Shooting(bool state)        // �÷��̾��� ���ݿ� ���� �ִϸ��̼� �ߵ� �Լ�
	{
		anim.SetBool(shotId, state);
	}
}
