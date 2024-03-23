using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mini02_Frame02 : MonoBehaviour    // �ι��� Ʋ�� ������
{
	[SerializeField] Mini02_FramePanel mini02_FramePanel;       // Ʋ �г� ��ũ��Ʈ
	[SerializeField] Mini02_Player mini02_Player;               // �÷��̾� ��ũ��Ʈ

	[SerializeField] Button frameButton;       // Ʋ �гο� �ִ� ���ι�ư

	[SerializeField] Image Right_Image;
	[SerializeField] Sprite[] sprite_Array;    // 0 : ���(��), 1 : ���(��Ÿ) 2 : ��

	[SerializeField] Slider slider;           // ��Ÿ ������ �ö󰡴� �����̴�

	Animator anim;           // �������� Ʋ�� ���Ÿ��� �ִϸ����� �޴� ����
	bool isSuccess = false;    // �����ߴ��� ���� ����

	[SerializeField] Sprite holeDonut01;         // �Ϸ� �̹����� ���� ���� �̹��� �������� ����
	[SerializeField] Sprite starDonut01;         // �Ϸ� �̹����� ��Ÿ ���� �̹��� �������� ����

	[SerializeField] GameObject Spread_Dough;    // ���� ���� ������Ʈ
	[SerializeField] GameObject One_Frame;       // ���� ���� Ʋ ������Ʈ
	[SerializeField] GameObject Star_Frame;      // ��Ÿ ���� Ʋ ������Ʈ

	[SerializeField] Animator One_Frame_Anim;       // ���� ���� Ʋ ������Ʈ
	[SerializeField] Animator Star_Frame_Anim;      // ��Ÿ ���� Ʋ ������Ʈ

	[SerializeField] GameObject One_Complete;    // ���ƴٴϴ� ���� ����
	[SerializeField] GameObject Star_Complete;   // ���ƴٴϴ� ��Ÿ ����

	[SerializeField] Animator One_Complete_Anim;    // ���ƴٴϴ� ���� ����
	[SerializeField] Animator Star_Complete_Anim;   // ���ƴٴϴ� ��Ÿ ����

	WaitForSeconds delay01;   // ���� ���� Ʋ�� �����̴� �ִϸ��̼�

	int frameId;   // �ִϸ����� ����ȭ
	int endId;

	void Awake()
	{
		frameId = Animator.StringToHash("isFrameShake");
		endId = Animator.StringToHash("isEnd");

		delay01 = new WaitForSeconds(0.1f);
	}

	void OnEnable()      // ������..
	{
		Spread_Dough.SetActive(true);
		


		if (mini02_FramePanel.isHoleOrStar.Equals(false))      //������01���� ���� Ʋ�� Ŭ���ߴٸ�...
		{
			One_Frame.SetActive(true);    // ���� Ʋ Ȱ��ȭ
			anim = One_Frame_Anim;        // ���� �ִϸ��̼��� ��´�.

			Right_Image.sprite = sprite_Array[0];
		}
		else                                             // ������01���� �� Ʋ�� Ŭ���ߴٸ�...
		{
			Star_Frame.SetActive(true);    // ��Ÿ Ʋ Ȱ��ȭ
			anim = Star_Frame_Anim;        // ��Ÿ �ִϸ��̼��� ��´�.

			Right_Image.sprite = sprite_Array[1];
		}
		frameButton.interactable = true;             // Ʋ �г� ���ι�ư Ȱ��ȭ(�̶� ���� �� ����)
	}


	void OnDisable()      // ������...
	{
		slider.value = 0;                      // �����̴� �ʱ�ȭ
		isSuccess = false;                     // ���� ���� �ʱ�ȭ

		frameButton.interactable = false;             // Ʋ �г� ���ι�ư Ȱ��ȭ(�̶� ���� �� ����)

		Spread_Dough.SetActive(false);         // ���� ���� ������Ʈ ��Ȱ��ȭ
		One_Frame.SetActive(false);            // ���� ���� Ʋ ������Ʈ ��Ȱ��ȭ
		Star_Frame.SetActive(false);           // ��Ÿ ���� Ʋ ������Ʈ ��Ȱ��ȭ

		One_Complete.SetActive(false);         // ���ƴٴϴ� ���� ���� ��Ȱ��ȭ
		Star_Complete.SetActive(false);        // ���ƴٴϴ� ��Ÿ ���� ��Ȱ��ȭ

		Right_Image.sprite = sprite_Array[0];

		Invoke("FinalInvoke", 0.1f);   // �κ�ũ ����
	}

	void FinalInvoke()   // �������� �ִϸ��̼��� ���� �κ�ũ?(�̰� �ؾ����� �ǹ�...)
	{
		anim.SetBool(frameId, false);         // �ִϸ��̼� �ٷ� ����(����)
	}


	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Press_FrameButton();
		}

		if (slider.value.Equals(1) && isSuccess.Equals(false))             // �����̴��� ���� 1�̸� ������, �Ϸ� ����...
		{
			isSuccess = true;               // �Ϸ� �Ǿ��ٰ� �˸�
			Right_Image.sprite = sprite_Array[2];

			AudioMng.ins.PlayEffect("SpeedUp");      // Ʋ ���� �ִϸ��̼� ����

			if (mini02_FramePanel.isHoleOrStar.Equals(false))       // ���� ����                       
			{
				One_Complete.SetActive(true);      // ����
				One_Complete_Anim.SetBool(endId, true);   // ����

				mini02_Player.isHoleOrStar = false;                              // ���� Ʋ�� ������ٰ��ϰ� ������
			}
			else                        // ��Ÿ ����
			{
				Star_Complete.SetActive(true);      // ����
				Star_Complete_Anim.SetBool(endId, true);   // ����

				mini02_Player.isHoleOrStar = true;                             // ��Ÿ Ʋ�� ������ٰ��ϰ� ������
			}

			mini02_Player.playerCompleteInt = 2;     // ����, Ƣ�� �гη� ����� �˸�

			return;
		}

		if (slider.value < 1.0f)       // �����̴� ���� 1 �̸��̸�
		{
			slider.value -= 0.1f * Time.deltaTime;     // ��ư�� ������ �ʴ´ٸ� �����̴��� �� ����
		}
	}


	public void Press_FrameButton()    // ��� ������ �����̴��� ä������.
	{
		if (slider.value.Equals(1.0f))        // �����̴� ���� 1�̸�
		{
			
			mini02_FramePanel.EndButton();

			return;
		}

		AudioMng.ins.PlayEffect("Dough");     // Ʋ �����̴� ���
		slider.value += 0.05f;                // �����̴� �� ���
		anim.SetBool(frameId, true);   // ��ư�� ���������� Ʋ �ִϸ��̼� �ߵ�(����)
		
		StartCoroutine(WaitShake());           // �ִϸ��̼� �ߵ� �ð�(����)
	}

	IEnumerator WaitShake()                       // ���� ���� Ʋ�� �����̴� �ִϸ��̼�
	{
		yield return delay01;
		anim.SetBool(frameId, false);         // �ִϸ��̼� �ٷ� ����(����)
	}
}
