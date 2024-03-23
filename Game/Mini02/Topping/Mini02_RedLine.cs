using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini02_RedLine : MonoBehaviour             // ������ο� ������
{
	[SerializeField] Material[] Mat_Array;

	[SerializeField] Mini02_Player mini02_Player;        // �÷��̾� ��ũ��Ʈ
	[SerializeField] Mini02_CountLine mini02_CountLine;  // ī��Ʈ���� ��ũ��Ʈ
	[SerializeField] Mini02_Spawn mini02_Spawn;          // ���� ��ũ��Ʈ

	[SerializeField] GameObject toppingPanel;     // ���� �г�
	[SerializeField] Mini02_ToppingPanel mini02_ToppingPanel;         // ���� �г� ��ũ��Ʈ

	[SerializeField] Button topping_Button;      // ���� �г� �ȿ� �ִ� ���� ��ư
	[SerializeField] Image topping_ButtonImage;                 // ���� ���� ��ư �̹��� (�����)
	[SerializeField] Sprite[] sprite_Array;              // 0 : �Ѹ�(����), 1 : �Ѹ�(����), 1 : ����, 2 :����, 3 : ����

	[SerializeField] TextMeshProUGUI score_Text;      // ���ھ� �ؽ�Ʈ(����)

	Color originalColor;               // ������ ���� ��ȯ�� �ٽ� �ǵ��� �� ������ �޴� ����

	float z = 0;                       // ������ ȸ���� ����ϴ� ����
	bool changeBool = false;           // ����, ���������� ���°� ��� ���� ����

	bool isGreenLine01 = false;          // ��������� ���� ���� ������ �ִ��� ���� ����
	int succeess = 0;                   // ���� ī��Ʈ
	bool isSuccess = false;

	[SerializeField] TextMeshProUGUI countText;   // ��Ŭ �߾ӿ� �ִ� ���� ī��Ʈ�� ������ �ؽ�Ʈ

	bool isHoleOrStar = true;       // �÷��̾ ��������, ��Ÿ����
	bool isOvenOrFryer = true;      // �÷��̾ ��������, Ƣ������
	bool isPinkOrChoco = true;      // �÷��̾ ��������, ��������

	int completeDonutInt = 10;     // ����, ����, ���⸦ ����־� ���ڸ� �޾ƿ��� ����

	[SerializeField] GameObject One_Donut;    // ���ư��� ����
	[SerializeField] GameObject Star_Donut;   // 
	Vector3 originPos;      // ���ư��� ������ ó�� ��ġ
	Quaternion originRot;   

	[SerializeField] Animator Shake_Anim;     // ����

	Material[] Donut_Mat_Array = new Material[4];   // 0 : ���� ����, 1 : ���� Ƣ��, 2 : ��Ÿ ����, 3 : ��Ÿ Ƣ��

	Material[] OneOvenStrow_Mat_List = new Material[5];        // ���� ���� ����
	Material[] OneOvenChoco_Mat_List = new Material[5];        // ���� ���� ����
	Material[] OneFryStrow_Mat_List = new Material[5];         // ���� Ƣ�� ����
	Material[] OneFryChoco_Mat_List = new Material[5];         // ���� Ƣ�� ����
	
	Material[] StarOvenStrow_Mat_List = new Material[5];       // ��Ÿ ���� ����
	Material[] StarOvenChoco_Mat_List = new Material[5];       // ��Ÿ ���� ����
	Material[] StarFryStrow_Mat_List = new Material[5];        // ��Ÿ Ƣ�� ����
	Material[] StarFryChoco_Mat_List = new Material[5];        // ��Ÿ Ƣ�� ����

	Renderer One_Donut_Render;
	Renderer Star_Donut_Render;

	Animator One_Donut_Ani;          // ���ƴٴϴ� ���� ���� �ִϸ��̼�
	Animator Star_Donut_Ani;          // ���ƴٴϴ� ��Ÿ ���� �ִϸ��̼�

	[SerializeField] GameObject Complete_One;     // �ϼ��� ���� ����
	[SerializeField] GameObject Complete_Star;    // �ϼ��� ��Ÿ ����

	[SerializeField] Renderer Complete_One_Render;     // �ϼ��� ���� ����
	[SerializeField] Renderer Complete_Star_Render;    // �ϼ��� ��Ÿ ����

	[SerializeField] Animator Complete_One_Anim;     // �ϼ��� ���� ����
	[SerializeField] Animator Complete_Star_Anim;    // �ϼ��� ��Ÿ ����

	Image thisImage;   // ������� �̹���(�����)

	WaitForSeconds delay01;           // ������ ����ũ �ڷ�ƾ
	WaitForSeconds delay02;           // ��ư ���� �ڷ�ź

	int shakeId;                    // �޸��� �ִϸ����͸� �޴� ����
	int finalId;
	int completeId;



	void Awake()
	{
		One_Donut_Render = One_Donut.GetComponent<Renderer>();
		Star_Donut_Render = Star_Donut.GetComponent<Renderer>();

		One_Donut_Ani = One_Donut.GetComponent<Animator>();
		Star_Donut_Ani = Star_Donut.GetComponent<Animator>();

		thisImage = transform.GetComponent<Image>();

		delay01 = new WaitForSeconds(0.2f);
		delay02 = new WaitForSeconds(0.5f);

		shakeId = Animator.StringToHash("isShake");
		finalId = Animator.StringToHash("isFinal");
		completeId = Animator.StringToHash("isComplete");

		Donut_Mat_Array[0] = Mat_Array[0];
		Donut_Mat_Array[1] = Mat_Array[1];
		Donut_Mat_Array[2] = Mat_Array[2];
		Donut_Mat_Array[3] = Mat_Array[3];


		OneOvenStrow_Mat_List[0] = Mat_Array[4];
		OneOvenStrow_Mat_List[1] = Mat_Array[5];
		OneOvenStrow_Mat_List[2] = Mat_Array[6];
		OneOvenStrow_Mat_List[3] = Mat_Array[7];
		OneOvenStrow_Mat_List[4] = Mat_Array[8];

		OneOvenChoco_Mat_List[0] = Mat_Array[9];
		OneOvenChoco_Mat_List[1] = Mat_Array[10];
		OneOvenChoco_Mat_List[2] = Mat_Array[11];
		OneOvenChoco_Mat_List[3] = Mat_Array[12];
		OneOvenChoco_Mat_List[4] = Mat_Array[13];

		OneFryStrow_Mat_List[0] = Mat_Array[14];
		OneFryStrow_Mat_List[1] = Mat_Array[15];
		OneFryStrow_Mat_List[2] = Mat_Array[16];
		OneFryStrow_Mat_List[3] = Mat_Array[17];
		OneFryStrow_Mat_List[4] = Mat_Array[18];

		OneFryChoco_Mat_List[0] = Mat_Array[19];
		OneFryChoco_Mat_List[1] = Mat_Array[20];
		OneFryChoco_Mat_List[2] = Mat_Array[21];
		OneFryChoco_Mat_List[3] = Mat_Array[22];
		OneFryChoco_Mat_List[4] = Mat_Array[23];


		StarOvenStrow_Mat_List[0] = Mat_Array[24];
		StarOvenStrow_Mat_List[1] = Mat_Array[25];
		StarOvenStrow_Mat_List[2] = Mat_Array[26];
		StarOvenStrow_Mat_List[3] = Mat_Array[27];
		StarOvenStrow_Mat_List[4] = Mat_Array[28];

		StarOvenChoco_Mat_List[0] = Mat_Array[29];
		StarOvenChoco_Mat_List[1] = Mat_Array[30];
		StarOvenChoco_Mat_List[2] = Mat_Array[31];
		StarOvenChoco_Mat_List[3] = Mat_Array[32];
		StarOvenChoco_Mat_List[4] = Mat_Array[33];

		StarFryStrow_Mat_List[0] = Mat_Array[34];
		StarFryStrow_Mat_List[1] = Mat_Array[35];
		StarFryStrow_Mat_List[2] = Mat_Array[36];
		StarFryStrow_Mat_List[3] = Mat_Array[37];
		StarFryStrow_Mat_List[4] = Mat_Array[38];

		StarFryChoco_Mat_List[0] = Mat_Array[39];
		StarFryChoco_Mat_List[1] = Mat_Array[40];
		StarFryChoco_Mat_List[2] = Mat_Array[41];
		StarFryChoco_Mat_List[3] = Mat_Array[42];
		StarFryChoco_Mat_List[4] = Mat_Array[43];
	}


	void OnEnable()       // �����ҋ�..
	{
		isHoleOrStar = mini02_Player.isHoleOrStar;        // �������� ��Ÿ����
		isOvenOrFryer = mini02_Player.isOvenOrFryer;      // �������� Ƣ������
		isPinkOrChoco = mini02_ToppingPanel.isPinkOrChoco;      // �������� ��������

		if (isHoleOrStar.Equals(false))    // ���� ����
		{
			if (isOvenOrFryer.Equals(false))    // ���� ����
			{
				One_Donut_Render.material = Donut_Mat_Array[0];      // ���� ���ӿ� �⺻ ����/���� ���׸����� �ִ´�.
			}
			else                          // ���� Ƣ��
			{
				One_Donut_Render.material = Donut_Mat_Array[1];      // ���� ���ӿ� �⺻ ����/Ƣ�� ���׸����� �ִ´�.
			}

			originPos = One_Donut.transform.position;    // ó�� ���� ������ ��ġ�� ����
			originRot = One_Donut.transform.rotation;
		}
		else                          // ��Ÿ ����
		{
			if (isOvenOrFryer.Equals(false))   // ��Ÿ ����
			{
				Star_Donut_Render.material = Donut_Mat_Array[2];      // ��Ÿ ���ӿ� �⺻ ��Ÿ/���� ���׸����� �ִ´�.
			}
			else                         // ��Ÿ Ƣ��
			{
				Star_Donut_Render.material = Donut_Mat_Array[3];      // ��Ÿ ���ӿ� �⺻ ��Ÿ/Ƣ�� ���׸����� �ִ´�.
			}

			originPos = Star_Donut.transform.position;    // ó�� ���� ������ ��ġ�� ����
			originRot = Star_Donut.transform.rotation;
		}

		if (isPinkOrChoco.Equals(false))   // ����
		{
			topping_ButtonImage.sprite = sprite_Array[0];
		}
		else    // ����
		{
			topping_ButtonImage.sprite = sprite_Array[1];
		}

		originalColor = thisImage.color;    // ������ο� �ִ� ������ ������ ������

		int startRandInt = Random.Range(0, 2);   // ȸ�� ���� ��������..
		if (startRandInt.Equals(0))
		{
			changeBool = false;
		}
		else
		{
			changeBool = true;
		}

		z = 0.0f;                     // ȸ�� �ʱ�ȭ
	}

	void OnDisable()              // ���� ��..
	{
		isGreenLine01 = false;        // �ʱ�ȭ
		isSuccess = false;
		succeess = 0;
		countText.text = succeess.ToString();


		if (isHoleOrStar.Equals(false))    // ����
		{
			One_Donut_Ani.SetBool(finalId, false);  // ���� ���� �ִϸ��̼� �ʱ�ȭ
			One_Donut.transform.position = originPos;    // ���� ���� ��ġ �ʱ�ȭ
			One_Donut.transform.rotation = originRot;

			One_Donut.SetActive(false);    // ���� ���� ��Ȱ��ȭ
			Complete_One_Anim.SetBool(completeId, false);
			Complete_One.SetActive(false);
		}
		else
		{
			Star_Donut_Ani.SetBool(finalId, false);  // ��Ÿ ���� �ִϸ��̼� �ʱ�ȭ
			Star_Donut.transform.position = originPos;    // ��Ÿ ���� ��ġ �ʱ�ȭ
			Star_Donut.transform.rotation = originRot;

			Star_Donut.SetActive(false);    // ��Ÿ ���� ��Ȱ��ȭ
			Complete_Star_Anim.SetBool(completeId, false);
			Complete_Star.SetActive(false);
		}
	}

	// ���ʿ� �ִ� ���� ������ ��ġ�ϰų�, ���� ������
	// �����ʿ����� ��ġ�ǵ��� �ϰ� ȸ���� �ٲ۴�.

	void Update()
	{
		if (isSuccess.Equals(false))
		{
			if (changeBool.Equals(false))         // ����
			{
				z += Time.deltaTime * 200;
			}
			else                             // ������
			{
				z -= Time.deltaTime * 200;
			}


			if (transform.localRotation.z > 0.72f)
			{
				if (transform.localRotation.z > 0.73f)
				{
					changeBool = true;
				}

				isGreenLine01 = true;
			}
			else if (transform.localRotation.z < -0.72f)
			{
				if (transform.localRotation.z < -0.73f)
				{
					changeBool = false;
				}

				isGreenLine01 = true;
			}
			else
			{
				isGreenLine01 = false;
			}
		}

		transform.localRotation = Quaternion.Euler(0, 0, z);    // ������� ȸ��(z��)�� z�� ���� ��ȯ

	}


	IEnumerator WaitSuccess()       // ��������!!!!!!!!!
	{
		yield return delay01;

		Shake_Anim.SetBool(shakeId, false);    // ������ �ִϸ��̼� ��

		if (isHoleOrStar.Equals(false))    // ����
		{
			One_Donut_Ani.SetBool(finalId, true);      // ���� ���� �ִϸ��̼� ����(�ѱ��)
		}
		else
		{
			Star_Donut_Ani.SetBool(finalId, true);     // ��Ÿ ���� �ִϸ��̼� ����(�ѱ��)
		}
	}




	int scoreInt = 0;

	public void Press_RedLine()       // ���� �гο� �ִ� ���� ��ư�� ������ ���
	{
		if (succeess.Equals(5))             // ���� ī��Ʈ�� 5���� �ٷ� ���� ��Ŵ
		{
			toppingPanel.SetActive(false);     // ���� �г� ��Ȱ��ȭ
			mini02_Player.Origin_Panel();
			return;
		}

		if (isGreenLine01.Equals(true))    // ���� ī��Ʈ�� 5�� �����̰�, ���� ������ ���� ������ �ִٸ�
		{
			succeess++;                            // ���� ī��Ʈ�� 1 ���ϱ�(���� ��, ī��Ʈ ����)
			countText.text = succeess.ToString();  // 

			Shake_Anim.SetBool(shakeId, true);    // ����

			if (isHoleOrStar.Equals(false))        // ���� ����               ///////////////////////////////////////////////////////////////
			{
				AudioMng.ins.PlayEffect("Topping");      // ���� ����ũ

				if (isOvenOrFryer.Equals(false))        // ���� ���� ����
				{
					if (isPinkOrChoco.Equals(false))    // ���� ���� ���� ����
					{
						One_Donut_Render.material = OneOvenStrow_Mat_List[succeess - 1];  // ���׸��� ��ü
					}
					else                          // ���� ���� ���� ����
					{
						One_Donut_Render.material = OneOvenChoco_Mat_List[succeess - 1];  // ���׸��� ��ü
					}
				}
				else                              // ���� Ƣ�� ����
				{
					if (isPinkOrChoco.Equals(false))    // ���� Ƣ�� ���� ����
					{
						One_Donut_Render.material = OneFryStrow_Mat_List[succeess - 1];  // ���׸��� ��ü
					}
					else                          // ���� Ƣ�� ���� ����
					{
						One_Donut_Render.material = OneFryChoco_Mat_List[succeess - 1];  // ���׸��� ��ü
					}
				}
			}
			else                              // ��Ÿ ����
			{
				AudioMng.ins.PlayEffect("Topping");      // ���� ����ũ

				if (isOvenOrFryer.Equals(false))     // ��Ÿ ���� ����
				{
					if (isPinkOrChoco.Equals(false))     // ��Ÿ ���� ���� ����
					{
						Star_Donut_Render.material = StarOvenStrow_Mat_List[succeess - 1];  // ���׸��� ��ü
					}
					else                           // ��Ÿ ���� ���� ����
					{
						Star_Donut_Render.material = StarOvenChoco_Mat_List[succeess - 1];  // ���׸��� ��ü
					}
				}
				else                           // ��Ÿ Ƣ�� ����
				{
					if (isPinkOrChoco.Equals(false))     // ��Ÿ Ƣ�� ���� ����
					{
						Star_Donut_Render.material = StarFryStrow_Mat_List[succeess - 1];  // ���׸��� ��ü
					}
					else                           // ��Ÿ Ƣ�� ���� ����
					{
						Star_Donut_Render.material = StarFryChoco_Mat_List[succeess - 1];  // ���׸��� ��ü
					}
				}
			}

			if (succeess.Equals(5) && isSuccess.Equals(false))   // ���� ī��Ʈ�� 5����� -> ����
			{
				isSuccess = true;     // �����ߴٰ� �˸�
				mini02_Player.playerCompleteInt = 0;     // ���� �гη� ����� �˸�

				if (mini02_CountLine.newMonster.Equals(true))
				{
					completeDonutInt = CheckCompleteDonut(isHoleOrStar, isOvenOrFryer, isPinkOrChoco);     // �Ϸ� ������ ���� �˷���

					if (isHoleOrStar.Equals(false))
					{
						Complete_One.SetActive(true);
						Complete_One_Anim.SetBool(completeId, true);
					}
					else
					{
						Complete_Star.SetActive(true);
						Complete_Star_Anim.SetBool(completeId, true);
					}


					if (completeDonutInt.Equals(mini02_CountLine.menuInt))               // ������ ���ϴ� ������ �ٶ�...
					{
						mini02_CountLine.GoHome_Guest(false);

						AudioMng.ins.PlayEffect("Success");      // ���� ȯ-ȣ

						topping_ButtonImage.sprite = sprite_Array[2];

						scoreInt++;  // ���ھ� �ø���
						score_Text.text = scoreInt.ToString();    // ���ھ �ؽ�Ʈ�� ǥ��
						mini02_Spawn.ScoreUp();                       // ���� ��ũ��Ʈ���� ���ھ� �ø���.(���� �ð� ������ ����...)
						mini02_Player.scoreInt++;
					}
					else                                   // �ֹ��� ���Ӱ� �ٸ� ���ϋ�....
					{
						completeDonutInt = -2;

						AudioMng.ins.PlayEffect("DdiYong");      // ���� ��-��

						topping_ButtonImage.sprite = sprite_Array[3];
					}
				}
				else
				{
					completeDonutInt = -2;
					topping_ButtonImage.sprite = sprite_Array[4];
				}

				StartCoroutine(WaitSuccess());

				return;
			}

			StartCoroutine(SuccessCoroutine());     // ���� ��, ��������� �Ķ������� �ٲٰ� 0.2�� �� �ٽ� �������(������)���� �ٲٱ�
			StartCoroutine(ClickWait());            // ��ư�� ���� ��, 0.5�� ���� ��ư�� �� ������ �ϱ�...
		}
		else                          // ���� ī��Ʈ�� 5�� �����̰�, ���� ������ ���� ������ ���ٸ�
		{
			Shake_Anim.SetBool(shakeId, false);    // ������ �ִϸ��̼� ��

			AudioMng.ins.PlayEffect("Fail03");      // ���� ����

			StartCoroutine(FailCoroutine());        // ���� ��, ��������� ��������� �ٲٰ� 0.2�� �� �ٽ� �������(������)���� �ٲٱ�
			StartCoroutine(ClickWait());            // ��ư�� ���� ��, 0.5�� ���� ��ư�� �� ������ �ϱ�...
		}
	}

	IEnumerator SuccessCoroutine()      // ������ �ϴ� �ڷ�ƾ
	{
		thisImage.color = Color.blue;

		yield return delay01;
		thisImage.color = originalColor;
	}

	IEnumerator FailCoroutine()          // ���н� �ϴ� �ڷ�ƾ
	{
		thisImage.color = Color.yellow;

		yield return delay01;
		thisImage.color = originalColor;
	}

	IEnumerator ClickWait()              // ��ư�� ���� ������ ȣ���ϴ� �ڷ�ƾ
	{
		topping_Button.interactable = false;     // ��ư�� �����ڸ��� ��ư�� ����
		topping_ButtonImage.color = Color.red;         // ��ư�� �����ڸ��� ��ư�� ���������� �ٲ�
		yield return delay02;                      // ��ư�� �� ������ �ð�
		topping_ButtonImage.color = Color.white;       // �ð��� ���� ��, ��ư�� ������ �ǵ���
		topping_Button.interactable = true;      // �ð��� ���� ��, ��ư�� Ŭ���� �� �ֵ��� ��
	}


	int CheckCompleteDonut(bool isHoleOrStar, bool isOvenOrFryer, bool isPinkOrChoco)
	{
		int playerDonutInt = -1;

		if (isHoleOrStar.Equals(false))             // Ʋ �гο��� ������ �������� ���
		{
			if (isOvenOrFryer.Equals(false))        // ���쿡�� �������� ���
			{
				if (isPinkOrChoco.Equals(false))    // ���� ������ �������� ���
				{
					playerDonutInt = 0;        // ���� / ���� / ����
					Complete_One_Render.material = OneOvenStrow_Mat_List[4];
				}
				else                          // ���� ������ �������� ���
				{
					playerDonutInt = 1;        // ���� / ���� / ����
					Complete_One_Render.material = OneOvenChoco_Mat_List[4];
				}
			}
			else                              // Ƣ��⿡�� Ƣ���� ���
			{
				if (isPinkOrChoco.Equals(false))    // ���� ������ �������� ���
				{
					playerDonutInt = 2;        // ���� / Ƣ�� / ����
					Complete_One_Render.material = OneFryStrow_Mat_List[4];
				}
				else                          // ���� ������ �������� ���
				{
					playerDonutInt = 3;        // ���� / Ƣ�� / ����
					Complete_One_Render.material = OneFryChoco_Mat_List[4];
				}
			}
		}
		else                                 // Ʋ �гο��� ��Ÿ�� �������� ���
		{
			if (isOvenOrFryer.Equals(false))        // ���쿡�� �������� ���
			{
				if (isPinkOrChoco.Equals(false))     // ���� ������ �������� ���
				{
					playerDonutInt = 4;         // ��Ÿ / ���� / ����
					Complete_Star_Render.material = StarOvenStrow_Mat_List[4];
				}
				else                           // ���� ������ �������� ���
				{
					playerDonutInt = 5;         // ��Ÿ / ���� / ����
					Complete_Star_Render.material = StarOvenChoco_Mat_List[4];
				}
			}
			else                               // Ƣ��⿡�� Ƣ���� ���
			{
				if (isPinkOrChoco.Equals(false))     // ���� ������ �������� ���
				{
					playerDonutInt = 6;         // ��Ÿ / Ƣ�� / ����
					Complete_Star_Render.material = StarFryStrow_Mat_List[4];
				}
				else                           // ���� ������ �������� ���
				{
					playerDonutInt = 7;         // ��Ÿ / Ƣ�� / ����
					Complete_Star_Render.material = StarFryChoco_Mat_List[4];
				}
			}
		}

		return playerDonutInt;
	}
}
