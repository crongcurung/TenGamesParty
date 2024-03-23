using UnityEngine;

public class Mini02_FramePanel : MonoBehaviour     // Ʋ �гο� ������
{
    [SerializeField] Mini02_Player mini02_Player;

    [SerializeField] GameObject page01Panel;    // ������01 �г�
    [SerializeField] GameObject page02Panel;    // ������02 �г�


    public bool isFrameInput = false;           // Ʋ�� �־����� �˸��� ����
    public bool isHoleOrStar = false;           // ���� Ȥ�� ��Ÿ�� Ŭ���ߴ��� �˸��� ����

    void OnEnable()      // ������ ��..
    {
        page01Panel.SetActive(true);                               // Ʋ �гο� ������ ������01���� �����ϵ���! 
    }

	void OnDisable()
	{
        page02Panel.SetActive(false);         // ������02 ��Ȱ��ȭ
    }


    public void Press_HoleButton()        // ���� Ʋ �̹����� ���
    {
        if (isFrameInput.Equals(false))   // �巡�� ���� �̹����� ������ ���� �ʾҴ°�?
        {
            return;                               // �׷� ����
        }

        AudioMng.ins.PlayEffect("Click03");      // ���� �巡�� ����

        isHoleOrStar = false;

        page01Panel.SetActive(false);            // ������02�� ��
        page02Panel.SetActive(true);
    }


    public void Press_StarButton()           // �� Ʋ �̹����� ���
    {
        if (isFrameInput.Equals(false))   // �巡�� ���� �̹����� ������ ���� �ʾҴ°�?
        {
            return;                                  // �׷� ����
        }

        AudioMng.ins.PlayEffect("Click03");      // ���� �巡�� ����

        isHoleOrStar = true;

        page01Panel.SetActive(false);         // ������02�� ��
        page02Panel.SetActive(true);
    }

    public void EndButton()         // ���� �������� ���� �Լ�
    {
        mini02_Player.Origin_Panel();                // �г��� ��Ȱ��ȭ �ϴ� �Լ�
        transform.gameObject.SetActive(false);       // �� �г��� ��Ȱ��ȭ
    }
}
