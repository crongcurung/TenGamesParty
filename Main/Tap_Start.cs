using UnityEngine;
using UnityEngine.UI;

public class Tap_Start : MonoBehaviour     // 스타트 씬에서 깜빡이는 문구
{
    float time;
    Image image;


    void Start()
    {
        image = GetComponent<Image>();       // 스타트 씬 아래 문구 이미지
    }

    void Update()
    {
        if (time < 0.9f)                             // 깜빡이기
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
