using UnityEngine;
using TMPro;

public class Mini09_Angle : MonoBehaviour               // ������ UI�� ������
{
    [SerializeField] TextMeshProUGUI currentText;       // ������ ������ ���� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI completeText;      // �ϼ��� �ؽ�Ʈ

    [SerializeField] Mini09_Player mini09_Player;       // �÷��̾� ��ũ��Ʈ

    float z;                        // �⺻ ����
    bool changeBool = false;        // ������ ���� �ٲٴ� ����
    float angle_Float = 0;          // ����

    string tag01;

    void Awake()
    {
        tag01 = "Note";
    }


    void OnEnable()     // ������ ��...
    {
        z = 45.0f;      // 45�� ���� �����ؾ���, ������ ��������.
    }

	void OnDisable()    // ������ ��....
	{
        completeText.text = currentText.text;            // �ϼ��� �ؽ�Ʈ�� �� ����
        angle_Float = Mathf.Round(z * 10) * 0.1f;        // �̷��� �ؾ� �Ҽ������� ��
        mini09_Player.angle_Float = angle_Float;         // �÷��̾����� �Ϸ�� ���� ���ڸ� ������.
    }

	void Update()
    {
        if (changeBool.Equals(false))         // ����
        {
            z += Time.deltaTime * 100;
        }
        else                                  // ������
        {
            z -= Time.deltaTime * 100;
        }
        transform.rotation = Quaternion.Euler(0, 0, z);          // ������ ȸ��
        currentText.text = z.ToString("N1");              // ������ ���� ���� ǥ��
    }


    //////////////// Ʈ���� ����

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tag01))           // ����ٲٴ� Ʈ����
        {
            changeBool = !changeBool;          // ���� �ٲٱ�

            AudioMng.ins.PlayEffect("Check");    // ������ ����
        }
    }
}
