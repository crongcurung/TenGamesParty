using UnityEngine;
using UnityEngine.UI;

public class Tap_Start : MonoBehaviour     // ��ŸƮ ������ �����̴� ����
{
    float time;
    Image image;


    void Start()
    {
        image = GetComponent<Image>();       // ��ŸƮ �� �Ʒ� ���� �̹���
    }

    void Update()
    {
        if (time < 0.9f)                             // �����̱�
        {
            image.color = new Color(1, 1, 1, time);

            time += Time.deltaTime * 0.7f;              
        }
        else
        {
            image.color = new Color(1, 1, 1, 1 - time);

            if (time > 1.0f)
            {
                time = 0;
            }

            time += Time.deltaTime;
        }
    }
}
