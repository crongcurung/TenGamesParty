using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini09_Plain : MonoBehaviour       // 날아가는 비행기에 부착됨
{
    [SerializeField] Mini09_Camera mini09_Camera;      // 카메라 스크립트

    [SerializeField] Transform startPos;               // 처음 플레이어가 시작하는 위치
    [SerializeField] TextMeshProUGUI distanceText;     // 거리가 표현되는 텍스트
    [SerializeField] TextMeshProUGUI coinText;         // 먹은 코인을 표현하는 텍스트
    [SerializeField] TextMeshProUGUI heightText;       // 높이가 표현되는 텍스트
    [SerializeField] GameObject dirCube;               // 비행기 앞에 방향을 맞춰주는 큐브
    [SerializeField] Slider slider;                    // 높이 슬라이드

    Rigidbody rigid;            // 비행기의 리지드 바디

    float angle = 0.0f;         // 비행기의 발사 회전을 받는 변수

    bool endBool = false;       // 비행기가..
    bool isFloor = false;       // 비행기가 바닥에 닿았는지 알리는 변수
    bool isInvoke = false;      // 인보크가 실행됐는지 묻는 변수

    public float distance = 0;     // 거리를 나타내는 변수
    float height = 0;       // 높이를 나타내는 변수
    float temp = 0.0f;      // 최대 높이를 나타내는 변수(높이가 더 높을 시 갱신)
    public int coinInt = 0;        // 먹는 코인을 나타내는 변수

    string invoke_Text;


    Vector3 vector01;
    Vector3 vector02;

    bool isFallFail = false;

    public bool isEnd = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        invoke_Text = "Invoke_EndGame";

        vector01 = new Vector3(0, 1, 0);
        vector02 = new Vector3(0, 1, 0.3f);

    }

    void Update()
    {
        if (isEnd.Equals(true))
        {
            return;
        }


        if (transform.position.y < -50.0f)        // 비행기가 어느 정도 바닥 아래로 떨어진다면 끝냄
        {
            mini09_Camera.EndGame();
            StartCoroutine(End_Coroutine());
        }

        if (isFallFail.Equals(false))
        {
            Text_Fuction();                           // 텍스트를 갱신하는 함수
        }

        if (transform.position.y < -30.0f && isFallFail.Equals(false))        // 플레이어가 일정 부분 떨어진다면..
        {
            isFallFail = true;

            //AudioMng.ins.PlayEffect("Fail02");    // 바닥 구멍에 떨어짐

            mini09_Camera.EndGame();
        }

        if (endBool.Equals(false))                
        {
            transform.up = rigid.velocity;        // 비행기 앞쪽을 보여주기 위해 이렇게 해야함(바닥에 닿으면 true가 되서 회전이 막 됨)
        }

        if (isInvoke.Equals(true))                // 인보크가 실행되고 있다면...
        {
            if (rigid.velocity != Vector3.zero)       // 다시 움직인다!
            {
                isInvoke = false;                 // 인보크가 끝났다고 한다.
                CancelInvoke(invoke_Text);   // 인보크 중단
            }
            return;     // 아래 실행 못하게 한다.
        }

        if (rigid.velocity.Equals(Vector3.zero) && isFloor.Equals(true))  // 속도가 0이고, 바닥에 닿고 있다면..
        {
            isInvoke = true;                     // 인보크를 실행한다.
            Invoke(invoke_Text, 2.0f);      // 2초 후에 게임 끝났다고 알린다.
        }
    }

    IEnumerator End_Coroutine()
    {
        yield return new WaitForSeconds(2.0f);
        mini09_Camera.End_Game(true);
    }

    void Invoke_EndGame()      // 바닥에 닿고, 속도가 0이면 끝난다고 알리는 인보크 변수
    {
        mini09_Camera.EndGame();   // 카메라 줌인 함수 실행
    }


    void Text_Fuction()        // 텍스트를 표현하는 함수
    {
        distance = transform.position.z - startPos.transform.position.z;    // 거리를 나타낸다.
        height = transform.position.y - startPos.transform.position.y;      // 높이를 나타낸다.
        distanceText.text = distance.ToString("N0");                  // 거리 텍스트로 나타낸다.

        if (height <= 0.0f)     // 높이가 0 밑이면
        {
            height = 0.0f;      // 높이를 0으로 고정
        }

        if (temp <= height)     // 현재 높이가 전 높이보다 높다면
        {
            temp = height;              // 현재 높이를 최대 높이로 한다.
            slider.maxValue = temp;     
            heightText.text = temp.ToString("N1");   // 최대 높이를 텍스트로 넘긴다.
        }

        slider.value = height;        // 슬라이더는 현재 높이로 보여준다.
    }


    public void Shotting_Plain(float speedFloat, int powerInt, float angleFloat)       // 플레이어에서 받아온 변수로 비행기를 날린다.
    {
        angle = 90.0f - angleFloat;          // 각도는 반대로 해야한다...
        transform.Rotate(new Vector3(angle, 0, 0));       // 비행기의 각도를 맞춘다.
        Vector3 dir = dirCube.transform.position - transform.position;      // 비행기의 방향을 앞쪽으로 향하게 한다.
        rigid.AddForce(dir.normalized * speedFloat * (powerInt + 1.5f) * 30.0f);       // 이 파워로 발사한다.
    }



    /////////////////////// 트리거 구역...

    void OnCollisionEnter(Collision collision)        // 콜라이더 엔터
    {
		if (collision.gameObject.layer.Equals(3))     // 바닥에 닿았으면
		{
			endBool = true;       // 게임 끝내자고 알려줌
			isFloor = true;       // 바닥에 닿았다고 알려줌

            AudioMng.ins.PlayEffect("Cloud");    // 바닥에 닿음
        }
	}

	void OnCollisionExit(Collision collision)        // 콜라이더 탈출
    {
		if (collision.gameObject.layer.Equals(3))     // 바닥에서 빠져나왔다면..
        {
			isFloor = false;      // 바닥하고 안 닿았다고 알려줌
		}
	}

    

    void OnTriggerEnter(Collider other)         // 트리거 엔터
    {
        if (other.gameObject.layer.Equals(7))       // Monster
        {
            other.gameObject.SetActive(false);       // 먹는 코인을 닿는다면..
            coinInt++;
            coinText.text = coinInt.ToString();

            AudioMng.ins.PlayEffect("Score_Up");    // 코인 먹음
        }
        else if (other.gameObject.layer.Equals(8))       // Object
        {
            AudioMng.ins.PlayEffect("HitApple");    // 토네이도 닿음

            if (rigid.velocity.y < -50.0f && rigid.velocity.y > -75.0f)
            {
                rigid.AddForce(vector01 * 3000.0f);

                return;
            }
            else if (rigid.velocity.y <= -75.0f)
            {
                rigid.AddForce(vector01 * 3500.0f);

                return;
            }
            rigid.AddForce(vector01 * 1000.0f);

        }
        else if (other.gameObject.layer.Equals(1))
        {
            endBool = false;                 // 게임 끝내자는걸 없던 일로 함
            rigid.AddForce(vector02 * 1000.0f);

            AudioMng.ins.PlayEffect("Bomb");    // 토네이도 닿음
        }
    }
}
