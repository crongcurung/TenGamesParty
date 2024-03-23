using UnityEngine;
using UnityEngine.UI;

public class Mini02_ToppingPanel : MonoBehaviour       // ���� �гο� ������
{
    [SerializeField] Material[] Mat_Array;

    [SerializeField] Mini02_Player mini02_Player;      // �÷��̾� ��ũ��Ʈ

    [SerializeField] Button redButton;                 // ���� ���� �̹��� ��ư
    [SerializeField] Button chocoButton;               // ���� ���� �̹��� ��ư

    [SerializeField] Button mainButton;               // ���� �гο� �ִ� ���� ��ư

    [SerializeField] GameObject bic_Circle;            //  ȸ�� ��Ŭ ������Ʈ

    bool isHoleOrStar = true;                // true�� ���� ����(������), false�� ��Ÿ ����(������)

    [SerializeField] GameObject One_Donut;            // ������ �ѷ��� ���� ���� ������Ʈ
    [SerializeField] GameObject Star_Donut;           // ������ �ѷ��� ��Ÿ ���� ������Ʈ

    [SerializeField] GameObject Bottle;               // ������ ������Ʈ
    [SerializeField] Renderer Bottle_Render;          // ������ ������(���׸��� ������)

    Material strow_Mat;              // ���� ������ ���׸���
    Material choco_Mat;              // ���� ������ ���׸���
    Material out_Mat;      // �迭������ ��¿�� ���� ������(������ ������Ʈ)

    public bool isPinkOrChoco = false;

    void Awake()
	{
        strow_Mat = Mat_Array[0];
        choco_Mat = Mat_Array[1];
        out_Mat = Mat_Array[2];
    }

	void OnEnable()         // ���� ��...
    {
        redButton.gameObject.SetActive(true);        // ���� ��ư Ȱ��ȭ
        chocoButton.gameObject.SetActive(true);      // ���� ��ư Ȱ��ȭ

        isHoleOrStar = mini02_Player.isHoleOrStar;
        mainButton.interactable = false;    // �̰� �� ����?
    }

    void OnDisable()        // ������...
    {
        bic_Circle.SetActive(false);          // ���� �г��� ���� ��� ��Ŭ�� ����.

        redButton.interactable = true;        // �̰� �� ����?
        chocoButton.interactable = true;       // �̰� �� ����?

        One_Donut.SetActive(false);     // ���� ������Ʈ ��Ȱ��ȭ
        Star_Donut.SetActive(false);

        Bottle.SetActive(false);        // ������ ������Ʈ ��Ȱ��ȭ
    }

    public void Press_RedButton()               // ���� �гο� �ִ� '���� ����' ��ư�� ������ ���
    {
        isPinkOrChoco = false;      // ���� ����
        bic_Circle.SetActive(true);             // ȸ�� ��Ŭ�� Ų��.

        AudioMng.ins.PlayEffect("Click03");      // ���� �巡�� ����

        if (isHoleOrStar.Equals(false))
        {
            One_Donut.SetActive(true);     // ���� ���� ������Ʈ Ȱ��ȭ
        }
        else
        {
            Star_Donut.SetActive(true);    // ��Ÿ ���� ������Ʈ Ȱ��ȭ
        }
        Bottle.SetActive(true);  // ������ Ȱ��ȭ
        Bottle_Render.materials = new Material[2] { out_Mat, strow_Mat };   // ���� ������ ���׸���� ��ü

        redButton.interactable = false;
        chocoButton.interactable = false;        // �� ��ư ��� �� ������ ��

        mainButton.interactable = true;

        redButton.gameObject.SetActive(false);     // ��ư �Ѵ� ��Ȱ��ȭ
        chocoButton.gameObject.SetActive(false);
    }

    public void Press_ChocoButton()              // ���� �гο� �ִ� '���� ����' ��ư�� ������ ���
    {
        isPinkOrChoco = true;       // ���� ����
        bic_Circle.SetActive(true);              // ȸ�� ��Ŭ�� Ų��.

        AudioMng.ins.PlayEffect("Click03");      // ���� �巡�� ����

        if (isHoleOrStar.Equals(false))
        {
            One_Donut.SetActive(true);     // ���� ���� Ȱ��ȭ
        }
        else
        {
            Star_Donut.SetActive(true);    // ��Ÿ ���� Ȱ��ȭ
        }

        Bottle.SetActive(true);  // ����
        Bottle_Render.materials = new Material[2] { out_Mat, choco_Mat };   // ���� ������ ���׸���� ��ü

        redButton.interactable = false;
        chocoButton.interactable = false;        // �� ��ư ��� �� ������ ��

        mainButton.interactable = true;

        redButton.gameObject.SetActive(false);     // ��ư �Ѵ� ��Ȱ��ȭ
        chocoButton.gameObject.SetActive(false);
    }



    public void Press_BackButton()         // ���� �гο� �ִ� ����ư�� ������ ���
    {
        gameObject.SetActive(false);     // ���� ���� �г��� ����.
        mini02_Player.Origin_Panel();
    }
}
