using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mini02_DoughPanel : MonoBehaviour
{
	[SerializeField] Sprite LeftArrow;
	[SerializeField] Sprite RightArrow;

	[SerializeField] Mini02_Player mini02_Player;   // �÷��̾� ��ũ��Ʈ

    [SerializeField] Image[] Arrow_Array;     // �Ǻ��� �ִ� 10���� ȭ�� �ִ� �� 10��
    List<int> saveint = new List<int>();   // ���� ������ ����(��������, ����������)�� ��� ����Ʈ
    int countInt = 0;                     // ���� ī��Ʈ

    Sprite leftArrow;        // ���� ȭ��
    Sprite rightArrow;       // ������ ȭ��

	[SerializeField] Button leftButton;         // ���� ���� ��ư
	[SerializeField] Button righrButton;        // ������ ���� ��ư

	[SerializeField] Image Right_Image;
	[SerializeField] Sprite[] RightSprite_Array;      // ������, ��

	[SerializeField] Animator anim_Dough;          // ���� �ִϸ��̼�
    [SerializeField] GameObject doughInBow;        // �׸� �ȿ� �ִ� ����
    [SerializeField] Animator anim_Player;         // ������ �ϴ� �ִϸ��̼�
    [SerializeField] GameObject Throw_Donut;       // ������ ���� �����°�
	[SerializeField] Animator Throw_Donut_Anim;    // ������ ���ư��� ����

	[SerializeField] Transform dough_Player;       // ���� �гο� �ִ� �÷��̾� ��ġ
	Vector3 player_Trans;
	Quaternion player_Quater;

	WaitForSeconds delay;  // Ʋ���� �� ��ٸ��� �ڷ�ƾ

	int doughId;     // �ִϸ����� ����ȭ
	int doughId_P;
	int throwId;

	string invoke_Text;


	// player : 9.946518, -0.0952649, 4.553154                 0, 184.403, 0
	// bowl   : 9.935, 1.81, 1.818                             -90.91, 181.09, -0.242
	// dough  : 9.985, 0.011, 4.028                            0, -179.982, 0

	void Awake()
	{
		doughId = Animator.StringToHash("isDough");
		doughId_P = Animator.StringToHash("isDough");
		throwId = Animator.StringToHash("isThrow");

		leftButton.interactable = true;      // ���� �г� ��ư Ȱ��ȭ
		righrButton.interactable = true;

		leftArrow = LeftArrow;           // ���� ȭ��ǥ �̹��� ������
		rightArrow = RightArrow;         // ������ ȭ��ǥ �̹��� ������

		invoke_Text = "Final_Success";

		player_Trans = dough_Player.position;
		player_Quater = dough_Player.rotation;

		delay = new WaitForSeconds(1.0f);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Press_LeftMainButton();
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Press_RightMainButton();
		}
	}

	void OnEnable()         // ���� �г��� �����ٸ�....
	{
        for (int i = 0; i < Arrow_Array.Length; i++)        // 10�� ����
        {
            int randInt = Random.Range(0, 2);   // 0�̸� left, 1�̸� right

            if (randInt.Equals(0))                       // ����
            {
                Arrow_Array[i].sprite = leftArrow;   // ���������� ���� ȭ��ǥ ���
            }
            else                                   // ������
            {
                Arrow_Array[i].sprite = rightArrow;   // ���������� ������ ȭ��ǥ ���
            }
            saveint.Add(randInt);                 // ���ڸ� ���������� ����
        }

		Right_Image.sprite = RightSprite_Array[0];
	}

	void OnDisable()
	{
		saveint.Clear();                             // ���� ����Ʈ�� �ʱ�ȭ
		for (int i = 0; i < Arrow_Array.Length; i++)
		{
			Arrow_Array[i].gameObject.SetActive(true);         //  �÷��̾ ������ ȭ��ǥ���� ���� �ѵα�...
		}
		countInt = 0;                                 // �÷��̾ ����� �������� �˷��ִ� ���� �ʱ�ȭ

		anim_Dough.SetBool(doughId, false);        // �ִϸ��̼� �ʱ�ȭ
		anim_Player.SetBool(doughId_P, false);   

		doughInBow.SetActive(true);     // �ٽ� �׸��ȿ� �ִ� ������ Ų��.

		leftButton.interactable = true;      // ���� �г� ��ư Ȱ��ȭ
		righrButton.interactable = true;

		dough_Player.position = player_Trans;
		dough_Player.rotation = player_Quater;

		Throw_Donut_Anim.SetBool(throwId, false);    // �ִϸ��̼� �ʱ�ȭ
		Throw_Donut.SetActive(false);            // ���ƴٴϴ� ���� ��Ȱ��ȭ
	}



	void CheckArrow(int countInt, bool isLeftButton)    // �������� �������� �����ϴ� �Լ�
	{
		if (isLeftButton.Equals(true))                      // �÷��̾ ���� ���� ��ư�� �����ٸ�
		{
			if (saveint[countInt].Equals(0))         // ����
			{
				Arrow_Array[countInt].gameObject.SetActive(false);    // ���������� �տ� �ϳ��� ����
				this.countInt++;                // ���������� ī��Ʈ �ö�

				AudioMng.ins.PlayEffect("Dough");     // ���� �����ϴ� �Ҹ�

				if (this.countInt < 10)         // ���� ī��Ʈ�� 10�� �̸��̸�
				{
					anim_Dough.SetBool(doughId, true);     // ���� �ϴ� �ִϸ��̼� �ߵ�
					anim_Player.SetBool(doughId_P, true);
				}
			}
			else         // ����
			{
				StartCoroutine(WaitKneading_Fail());          // ���� �̹��� �ڷ�ƾ ����
			}
		}
		else                                             // �÷��̾ ������ ���� ��ư�� �����ٸ�
		{
			if (saveint[countInt].Equals(1))               // ����
			{
				Arrow_Array[countInt].gameObject.SetActive(false);    // ���������� �տ� �ϳ��� ����
				this.countInt++;                      // ���������� ī��Ʈ �ö�

				AudioMng.ins.PlayEffect("Dough");     // ���� �����ϴ� �Ҹ�

				if (this.countInt < 10)            // ���� ī��Ʈ�� 10�� �̸��̸�
				{
					anim_Dough.SetBool(doughId, true);     // ���� �ϴ� �ִϸ��̼� �ߵ�
					anim_Player.SetBool(doughId_P, true);
				}
			}
			else                                // ����
			{
				StartCoroutine(WaitKneading_Fail());      // ���� �̹��� �ڷ�ƾ ����
			}
		}



		if (this.countInt.Equals(10))         // ���� ī��Ʈ�� 10�� ���...               ���� �ܰ� ����!!!!!!!!!!!!!!!!
		{
			Right_Image.sprite = RightSprite_Array[1];

			AudioMng.ins.PlayEffect("SpeedUp");       // ���� �ִϸ��̼� ����

			mini02_Player.playerCompleteInt = 1;     // Ʋ �гη� ����� �˸�
			doughInBow.SetActive(false);     // �׸� �ȿ� ����ִ� ������ ����
			anim_Player.SetBool(doughId_P, true);  // �÷��̾ �����ϴ� �ִϸ��̼�(�ݺ�)
			Invoke(invoke_Text, 0.3f);         // �����°Ŷ� �κ�ũ�� ��
			Throw_Donut.SetActive(true);           // ���ư��� ���� Ȱ��ȭ
			Throw_Donut_Anim.SetBool(throwId, true);    // ���ư��� ���� �ִϸ��̼� �ߵ�!
		}
	}

	void Final_Success()  // ������ ����
	{
		anim_Player.SetBool(doughId_P, false);  //  0.3�� �� �ִϸ��̼� ������
	}


	public void Press_LeftMainButton()    // ���� ��ư�� ���� ��
	{
		if (countInt < 10)          // ���� ī��Ʈ�� 10�� �̸��̸� ȭ��ǥ üũ�� ��
		{
			CheckArrow(countInt, true);

			
		}
		else                      // ���� ī��Ʈ�� 10���� �� ������ �ϱ�
		{
			return;
		}
	}


	public void Press_RightMainButton()   // ������ ��ư�� ���� ��
	{
		if (countInt < 10)          // ���� ī��Ʈ�� 10�� �̸��̸� ȭ��ǥ üũ�� ��
		{
			CheckArrow(countInt, false);    // �°� �ߴ��� üũ

		}
		else if (countInt.Equals(10))    // ���� ī��Ʈ�� 10���� ������ ��ư���� ������
		{
			EndButton();     // ���� �������� ���� �Լ�
		}
		else                       // ���� ī��Ʈ�� 10 �ʰ��̸� �� ������ �ϱ�
		{
			return;
		}
	}


	public void EndButton()         // ���� �������� ���� �Լ�
	{
		mini02_Player.Origin_Panel();         // �г��� ��Ȱ��ȭ �ϴ� �Լ�
		
		transform.gameObject.SetActive(false);    // �� �г��� ��Ȱ��ȭ
	}


	/////////////////////////////// �ڷ�ƾ ����

	IEnumerator WaitKneading_Fail()          // �÷��̾ Ʋ���� ���
	{
		leftButton.interactable = false;     // ��ư ����
		righrButton.interactable = false;

		AudioMng.ins.PlayEffect("Meow");     // ���� �����ϴ� �Ҹ�

		anim_Dough.SetBool(doughId, false);           // ���� �ִϸ��̼� ������
		anim_Player.SetBool(doughId_P, false);

		yield return delay;     // Ʋ���� ��, 1�� ��ٷ��� ��...

		leftButton.interactable = true;     // 1�� ��, ��ư Ǯ��
		righrButton.interactable = true;
	}
}
