using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini09_Player : MonoBehaviour
{
    [SerializeField] Sprite[] sprite_Array;

    [SerializeField] AudioSource audio_Run;

    [SerializeField] Transform PlainPos;                  // 플레이어에서 비행기를 던지는 위치
    [SerializeField] Mini09_Camera mini09_Camera;         // 카메라 스크립트

    [SerializeField] GameObject Slider_Obj;

    [SerializeField] Button Left_Button;
    [SerializeField] Button Right_Button;
    [SerializeField] Image Right_Image;
    [SerializeField] Sprite[] spriteButton_Array;         // 0 : 날리기, 1 : 파워, 2 : 각도

    Animator anim;                       // 애니메이터는 플레이어와 몬스터가 공통으로 지닐 가능성 높음
    Rigidbody rigid;                 // 리지드바디는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    [SerializeField] float speed;        // 이동 속도 변수는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    [SerializeField] Image speed_Image;                   // UI 아래에 스피드 이미지
    Sprite[] speedSprite_Array = new Sprite[3];          // 100 미만 스피드 스프라이트
    [SerializeField] TextMeshProUGUI[] speedText_Array;   // 스피드 텍스트 배열
    float speedInt;                      // 위에 스피드 변수를 텍스트로 옮길려는 변수

    [SerializeField] GameObject powerCircle;              // 파워 서클 오브젝트(패널)
    [SerializeField] Transform redLine;                   // 파워 서클에서 회전하는 레드라인
    [SerializeField] TextMeshProUGUI[] powerCircle_Text;  // 파워 서클 텍스트 배열
    [SerializeField] Image power_Image;                   // 파워 이미지
    Sprite[] power_Sprite = new Sprite[5];               // 파워 서클 스프라이트 배열

    [SerializeField] GameObject angle;                 // 각도기가 담긴 오브젝트
    public float angle_Float;            // 각도기의 각도를 받는 변수

    [SerializeField] GameObject plain_01;                 // 손에 들고 있는 비행기
    [SerializeField] GameObject plain_02;                 // 던지는 비행기

    [SerializeField] TextMeshProUGUI distanceText;        // 거리 텍스트
    [SerializeField] TextMeshProUGUI coinText;            // 코인 텍스트
    [SerializeField] TextMeshProUGUI heightText;          // 높이 텍스트
    [SerializeField] Transform startPos;                  // 플레이어 스타트 위치(여기서부터 0m)
    [SerializeField] Slider slider;                       // 슬라이더

    float z = 0;                     // 회전 숫자
    int powerInt;                    // 파워 서클 회전 값
    int completeInt = 0;             // 끝난 회전 값을 받는 변수

    bool pressRun = false;                    // 스페이스바를 눌렀냐 묻는 변수...
    bool leftBool = false;                   // 왼쪽 버튼을 눌렀냐 묻는 변수...

    int rightButtonCount = 0;       // 오른쪽 버튼을 몇번 눌렀냐 보는 변수

    bool shooting = false;          // 가는 방향으로 머리가 향하도록 하는 변수
    bool isFloor = false;           // 바닥에 닿았는지 묻는 변수

    bool isend = false;         // 플레이어가 떨어진다고 알리는 변수(이것도 써야한다...)
    bool isFall = false;        // 플레이어가 떨어진다.

    bool isInvoke = false;      // 인보크를 실행하고 있는지 묻는 변수

    public float distance = 0;         // 거리를 나타내는 변수
    float height = 0;           // 높이를 나타내는 변수
    float temp = 0.0f;          // 최대 높이를 나타내는 변수
    public int coinInt = 0;            // 먹은 도넛 코인을 나타내는 변수

    Vector3 vector01;
    Vector3 vector02;

    string invoke_Text;

    int runId;
    int attackId;
    int throwId;

    public bool isEnd = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        audio_Run.volume = AudioMng.ins.GetEffectVolume();

        vector01 = new Vector3(0, 0, 1.0f);
        vector02 = new Vector3(0, 1, 0.3f);
        invoke_Text = "Invoke_EndGame";

        runId = Animator.StringToHash("isRun");
        attackId = Animator.StringToHash("isAttack");
        throwId = Animator.StringToHash("isThrow");


        speedSprite_Array[0] = sprite_Array[0];
        speedSprite_Array[1] = sprite_Array[1];
        speedSprite_Array[2] = sprite_Array[2];

        power_Sprite[0] = sprite_Array[3];
        power_Sprite[1] = sprite_Array[4];
        power_Sprite[2] = sprite_Array[5];
        power_Sprite[3] = sprite_Array[6];
        power_Sprite[4] = sprite_Array[7];

        AudioMng.ins.Play_BG("Mini09_B");
    }


    void FixedUpdate()
    {
        if (pressRun.Equals(true) && isend.Equals(false))        // 스페이스바를 눌렀고, 아직 플레이어가 떨어지지 않았다면...
        {
            Move();
        }
    }

    bool isFallFail = false;

    void Update()
    {
        if (isEnd.Equals(true))
        {
            return;
        }

        if (isend.Equals(false))     // 플레이어가 떨어지지 않았다면..
        {
            if (rightButtonCount.Equals(1))
            {
                PowerCurcle();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Press_LeftButton();
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Press_RightButton();
            }
        }
        else                          // 플레이어가 떨어진다면...
        {
            if (isFallFail.Equals(false))
            {
                Text_Fuction();           // 텍스트를 표현하는 함수 실행
            }

            if (transform.position.y < -50.0f && isFallFail.Equals(false))        // 플레이어가 일정 부분 떨어진다면..
            {
                isFallFail = true;

                mini09_Camera.EndGame();
                StartCoroutine(End_Coroutine());
            }

            if (shooting.Equals(true))                // 
            {
                transform.up = rigid.velocity;        // 플레이어 회전 위치를 앞으로 가는 쪽으로 본다.
            }

            if (isInvoke.Equals(true))                // 인보크가 실행되고 있다면..
            {
                if (rigid.velocity != Vector3.zero)       // 다시 움직인다!
                {
                    isInvoke = false;                     // 인보크 중단
                    CancelInvoke(invoke_Text);
                }
                return;        // 밑에 실행 못하게 한다.
            }

            if (rigid.velocity.Equals(Vector3.zero) && isFloor.Equals(true))      // 속도가 0이고, 바닥에 닿는다면..
            {
                isInvoke = true;                    // 인보크를 실행시킨다.
                Invoke(invoke_Text, 2.0f);
            }
        }
    }

    void Invoke_EndGame()         // 카메라 줌인(게임 끝)
    {
        mini09_Camera.EndGame();
    }

    IEnumerator End_Coroutine()
    {
        yield return new WaitForSeconds(2.0f);
        mini09_Camera.End_Game(false);
    }

    void Text_Fuction()
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

    void Move()         // 이동 함수를 부모걸로 씀
    {
        transform.position += vector01.normalized * Time.fixedDeltaTime * speed * 0.5f;     // 플레이어를 앞으로
    }



    public void Press_LeftButton()       // 왼쪽 버튼을 누르면
    {
        if (leftBool.Equals(false))     // 한번만 실행할려고 바로 false로 바꾼다.
        {
            audio_Run.pitch += 0.07f;
            audio_Run.Play();

            speed += Random.Range(2.5f, 4.3f);     // 랜덤으로 속도를 높인다.
            pressRun = true;                       // 스페이스바를 눌렀다고 알린다.
            anim.SetBool(runId, true);           // 달리는 애니메이션을 실행
            anim.speed += speed * 0.001f;          // 애니메이션 속도를 올린다.

            speedInt = Mathf.Round(speed * 10) * 0.09f;     // 스피드를 텍스트로 표현한다.

            if (speedInt < 50.0f)         // 스피드가 50 미만이면
            {
                speedText_Array[0].text = $"<color=white>{speedInt.ToString("N1")} km/h</color>";   // 스피드 색깔을 하얀색으로...
            }
            else if (speedInt < 100.0f)   // 스피드가 100 미만이면
            {
                speedText_Array[0].text = $"<color=blue>{speedInt.ToString("N1")} km/h</color>";     // 스피드 색깔을 파란색으로...
            }
            else                          // 스피드가 100이상이면
            {
                speedText_Array[0].text = $"<color=red>{speedInt.ToString("N1")} km/h</color>";     // 스피드 색깔을 빨강색으로...
            }
        }
    }

    public void Press_RightButton()      // 오른쪽 버튼을 누른다면..
    {
        switch (rightButtonCount)        // 오른쪽 버튼을 몇 번 눌렀는지..
        {
            case 0:           // 파워 서클이 나옴
                AudioMng.ins.LoopEffect(true);
                AudioMng.ins.PlayEffect("Circle");    // 파워 서클

                Left_Button.interactable = false;      // 버튼 못 누르게 하기...
                
                Right_Image.sprite = spriteButton_Array[1];

                PowerCircle_RightButton();
                break;
            case 1:           // 파워 서클이 사라지고, 각도기가 나옴
                AudioMng.ins.LoopEffect(false);
                AudioMng.ins.StopEffect();

                Right_Image.sprite = spriteButton_Array[2];

                Angle_RightButton();
                break;
            case 2:           // 각도기가 사라짐
                AudioMng.ins.PlayEffect("SpeedUp");    // 비행기 발사
                ThrowPlain_RightButton();

                Right_Button.interactable = false;      // 버튼 못 누르게 하기...

                Left_Button.gameObject.SetActive(false);
                Right_Button.gameObject.SetActive(false);

                Slider_Obj.SetActive(true);

                break;
            default:          // 이쪽은 다 무시
                return;
        }
        rightButtonCount++;               // 한번 누를때마다 하나씩 올림
    }


    void PowerCircle_RightButton()    // 파워서클이 나타내는 함수
    {
        if (speedInt < 100.0f)      // 스피드가 100 미만이면 
        {
            speed_Image.sprite = speedSprite_Array[1];    // 계속기 이미지 1번 넣음
        }
        else                        // 스피드가 100 이상이면
        {
            speed_Image.sprite = speedSprite_Array[2];    // 계속 이미지 2번 넣음
        }

        speedText_Array[1].text = speedText_Array[0].text;

        leftBool = true;                      
        pressRun = false;
        anim.SetBool(attackId, true);         // 던지는 모션을 취한다.
        anim.SetBool(runId, false);           // 달리는 애니메이션 끝
        transform.rotation = Quaternion.Euler(new Vector3(0, 45.0f, 0));   // 던지는 모션을 쓸라면 이렇게 해야한다..
        powerCircle.SetActive(true);   // 파워 서클 활성화
    }

    void Angle_RightButton()      // 각도기 나타내는 함수
    {
        powerCircle_Text[0].text = powerCircle_Text[1].text;
        switch (completeInt)
        {
            case 0:       // low
                power_Image.sprite = power_Sprite[1];          // 이미지 바꿈
                break;
            case 1:       // normal
                power_Image.sprite = power_Sprite[2];         // 이미지 바꿈
                break;
            case 2:       // high
                power_Image.sprite = power_Sprite[3];             // 이미지 바꿈
                break;
            default:      // supreme
                power_Image.sprite = power_Sprite[4];           // 이미지 바꿈
                break;
        }
        powerCircle.SetActive(false);    // 파워 서클은 비활성화
        angle.SetActive(true);           // 각도기는 활성화
    }


    void ThrowPlain_RightButton()        // 날라가는 비행기 나타내는 함수
    {
        angle.SetActive(false);          // 각도기 비활성화
        transform.rotation = Quaternion.Euler(Vector3.zero);   // 45도 기울였던 회전을 0으로 맞춘다.
        anim.SetBool(throwId, true);                         // 던지는 애니메이션 실행

        plain_01.SetActive(false);         // 들고 있던 비행기 비활성화
        plain_02.SetActive(true);          // 날라가는 비행기 활성화
        plain_02.transform.position = PlainPos.position;   // 날라가는 비행기를 던지는 위치에 놓는다.

        plain_02.GetComponent<Mini09_Plain>().Shotting_Plain(speedInt, completeInt, angle_Float);   // 날라가는 비행기를 날린다.
        mini09_Camera.Target_Change();       // 카메라 타켓을 비행기로 바꾼다.
    }


    void PowerCurcle()       // 파워 서클 회전하는 함수
    {
        z += Time.deltaTime * 600;                // 레드라인 회전 속도
        powerInt = (int)(z * 0.1f) % 36;           // 비율 값 조정
        redLine.rotation = Quaternion.Euler(0, 0, z);

        if (powerInt > 31)                            // supreme
        {
            powerCircle_Text[1].text = "<color=red>Supreme</color>";
            powerCircle_Text[1].fontSize = 50.0f;
            completeInt = 3;
        }
        else if (powerInt > 25)                         // high
        {
            powerCircle_Text[1].text = "<color=orange>High</color>";
            powerCircle_Text[1].fontSize = 100.0f;
            completeInt = 2;
        }
        else if (powerInt > 12)                           // normal
        {
            powerCircle_Text[1].text = "<color=yellow>Normal</color>";
            powerCircle_Text[1].fontSize = 60.0f;
            completeInt = 1;
        }
        else                                              // low
        {
            powerCircle_Text[1].text = "<color=green>Low</color>";
            powerCircle_Text[1].fontSize = 100.0f;
            completeInt = 0;
        }
    }



    void Falling_Player()       // 플레이어가 오른쪽 버튼을 못 누른다면..
    {
        anim.SetBool(runId, false);    // 달리는 애니메이션을 끊다.
        rigid.constraints &= ~RigidbodyConstraints.FreezePositionY & ~RigidbodyConstraints.FreezePositionZ & ~RigidbodyConstraints.FreezeRotationX;

        AudioMng.ins.LoopEffect(false);

        mini09_Camera.Camera_Chage();

        Left_Button.interactable = false;      // 버튼 못 누르게 하기...
        Right_Button.interactable = false;

        if (speedInt < 50.0f)    // 스피드가 50보다 낮다면
        {
            transform.position += new Vector3(0, 0, 1);
            rigid.AddForce(new Vector3(0, -1, 1) * 5.0f * speedInt);            // 플레이어 떨어트리기

            //AudioMng.ins.PlayEffect("Fail02");    // 바닥 구멍에 떨어짐
        }
        else                     // 스피드가 50보다 높다면
        {
            transform.position += new Vector3(0, 3, 0);
            transform.rotation = Quaternion.Euler(Vector3.zero);
            rigid.AddForce(new Vector3(0, 1, 1) * 20.0f * speedInt);            // 플레이어 날라가기

            AudioMng.ins.PlayEffect("Meow");    // 플레이어 날라가기
        }
    }



    /////////////////////   트리거 구역

    void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.layer.Equals(4))       // 달리는 바닥 끝에 닿으면.. Water
        {
            if (isFall.Equals(false))
            {
                isFall = true;
                isend = true;
                shooting = true;

                plain_01.SetActive(false);
                Falling_Player();
            }
        }
        else if (other.gameObject.layer.Equals(7))       // Monster
        {
            other.gameObject.SetActive(false);       // 먹는 코인을 닿는다면..
            coinInt++;
            coinText.text = coinInt.ToString();

            AudioMng.ins.PlayEffect("Score_Up");    // 코인 먹음
        }
        else if (other.gameObject.layer.Equals(8))       // Object
        {
            rigid.AddForce(vector02 * 10000.0f);


            AudioMng.ins.PlayEffect("HitApple");    // 토네이도에 닿음
        }
        else if (other.gameObject.layer.Equals(1))      // 지뢰
        {
            shooting = false;
            rigid.AddForce(vector02 * 1000.0f);

            AudioMng.ins.PlayEffect("Bomb");    // 토네이도 닿음
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (isend.Equals(true))
        {
            if (collision.gameObject.layer.Equals(3))   // 플레이어가 바닥에 닿았다면    WALL
            {
                shooting = false;
                isFloor = true;                             // 바닥에 닿았다면 알려줌

                AudioMng.ins.PlayEffect("Meow");    // 바닥에 닿음
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer.Equals(3))       // 플레이어가 바닥에서 나갔다면     WALL
        {
            isFloor = false;                           // 바닥에서 떨어졌다고 알려줌
        }
    }
}
