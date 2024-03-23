using UnityEngine;
using TMPro;

public class Mini09_Angle : MonoBehaviour               // 각도기 UI에 부착됨
{
    [SerializeField] TextMeshProUGUI currentText;       // 각도기 오른쪽 위에 텍스트
    [SerializeField] TextMeshProUGUI completeText;      // 완성된 텍스트

    [SerializeField] Mini09_Player mini09_Player;       // 플레이어 스크립트

    float z;                        // 기본 각도
    bool changeBool = false;        // 각도기 방향 바꾸는 변수
    float angle_Float = 0;          // 현재

    string tag01;

    void Awake()
    {
        tag01 = "Note";
    }


    void OnEnable()     // 켜졌을 때...
    {
        z = 45.0f;      // 45도 부터 시작해야지, 오류가 적어진다.
    }

	void OnDisable()    // 꺼졌을 때....
	{
        completeText.text = currentText.text;            // 완성된 텍스트에 값 넣음
        angle_Float = Mathf.Round(z * 10) * 0.1f;        // 이렇게 해야 소수점으로 들어감
        mini09_Player.angle_Float = angle_Float;         // 플레이어한테 완료된 각도 숫자를 보낸다.
    }

	void Update()
    {
        if (changeBool.Equals(false))         // 왼쪽
        {
            z += Time.deltaTime * 100;
        }
        else                                  // 오른쪽
        {
            z -= Time.deltaTime * 100;
        }
        transform.rotation = Quaternion.Euler(0, 0, z);          // 각도기 회전
        currentText.text = z.ToString("N1");              // 오른쪽 위에 각도 표시
    }


    //////////////// 트리거 구역

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tag01))           // 방향바꾸는 트리거
        {
            changeBool = !changeBool;          // 방향 바꾸기

            AudioMng.ins.PlayEffect("Check");    // 각도기 변경
        }
    }
}
