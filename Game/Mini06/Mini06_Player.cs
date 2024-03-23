using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mini06_Player : MonoBehaviour
{
    [SerializeField] Joystic joystic;

    [SerializeField] GameObject[] Hp_Array;

    Rigidbody rigid;                 // 리지드바디는 플레이어와 몬스터가 공통으로 지닐 가능성 높음
    Animator anim;                       // 애니메이터는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    [SerializeField] float speed;        // 이동 속도 변수는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    [SerializeField] Image rightButtonImage;    // 오른쪽 버튼 이미지
    [SerializeField] Sprite[] sprite_Array;           // 0 : 잠자리채, 1 : 그물, 2 : 아이템 교체

    public bool isAlive;              // 플레이어가 살아 있는지 묻는 변수

    public bool isTouch_Bear = false;    // 플레이어가 곰에 닿았냐 묻는 변수

    bool isRun = false;         // 잠자리 채 혹은 그물을 휘두르고 있냐?
    bool isTable = false;                  // 책상에 닿았냐?

    [SerializeField] Mini06_Spawn mini06_Spawn;
    [SerializeField] Mini06_Net mini06_Net;        // 잠자리 채 스크립트
    [SerializeField] Transform tableObject;        // 테이블의 위치(바라볼때 사용)

    [SerializeField] GameObject net_Player;           // 플레이어가 들고 있는 잠자리채 오브젝트
    [SerializeField] GameObject web_player;           // 플레이어가 들고 있는 그물 오브젝트


    int currentObject = 0;             // 현재 들고 있는 것이 무엇인가? 0번 : 잠자리채, 1번 : 그물

    [SerializeField] GameObject net_Table;            // 테이블에 올려져 있는 잠자리채 오브젝트
    [SerializeField] GameObject web_Table;            // 테이블에 올려져 있는 그물 오브젝트

    [SerializeField] Transform middle;             // 그물을 던질 위치

    int playerHp;                        // 플레이어의 체력 변수

    Vector3 moveDir;                     // 이동 변수는 플레이어가 지닐 가능성 높음

    int runId;
    int attackId01;
    int attackId02;

    string hor_Text;
    string ver_Text;

    string invoke_Text01;
    string invoke_Text02;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        isAlive = true;                   // 플레이어가 살아있다고 함
        playerHp = 5;                     // 플레이어 체력은 5부터...

        runId = Animator.StringToHash("isRun");
        attackId01 = Animator.StringToHash("isAttack");
        attackId02 = Animator.StringToHash("isAttack02");

        hor_Text = "Horizontal";
        ver_Text = "Vertical";

        invoke_Text01 = "Invoke_Wait_BearAttack";
        invoke_Text02 = "Invoke_Press";
    }

	void Start()
	{
        AudioMng.ins.Play_BG("Mini06_B");
    }

	void FixedUpdate()
    {
        if (!moveDir.x.Equals(0) || !moveDir.z.Equals(0))               // wasd 아무거나라도 움직인다면..
        {
            anim.SetBool(runId, true);         // 움직이는 애니메이션 실행
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z)), 0.5f);
            // 플레이어 회전

            transform.position += new Vector3(moveDir.x, 0, moveDir.z).normalized * Time.fixedDeltaTime * speed;      // 플레이어 이동
        }
        else
        {
            anim.SetBool(runId, false);       // 멈추는 애니메이션 실행
        }
    }

    void Update()
    {
        if (isTouch_Bear.Equals(true))        // 곰에 닿았다면...
        {
            isTouch_Bear = false;        // 중복 방지

            Invoke(invoke_Text01, 1.0f);
        }

        Move();

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Press_RightButton();     // 오른쪽 버튼을 누른다면..
        }
    }



    void Move()         // 이동 함수를 부모걸로 씀
    {
#if (UNITY_EDITOR)
        float hor = Input.GetAxis(hor_Text);
        float ver = Input.GetAxis(ver_Text);
#elif (UNITY_IOS || UNITY_ANDROID)
		float hor = joystic.Horizontal;
        float ver = joystic.Vertical;
#endif




        moveDir.x = hor;
        moveDir.z = ver;
    }

    void MinusHp()        // 플레이어의 체력을 깍는다..
    {
        playerHp--;
        Hp_Array[playerHp].SetActive(false);

        AudioMng.ins.PlayEffect("Meow");    // 고양이 다침
        if (playerHp <= 0)
        {
            isAlive = false;

            mini06_Spawn.End_Game();
        }
    }

    public void MinusHp_Mini06()
    {
        MinusHp();
    }

    public void Before_Attack()         // 지금 공격 버튼을 눌렀다.
    {
        mini06_Net.isAttack = true;
    }

    public void After_Attack()          // 공격 애니메이션 끝
    {
        mini06_Net.isAttack = false;
    }


    public void Press_RightButton()        // 오른쪽 버튼을 누른다면...
    {
        Invoke(invoke_Text02, 0.1f);

        if (isRun.Equals(false))                // 공격을 하고 있지 않다면...
        {
            if (currentObject.Equals(0))        // 잠자리채를 들고 있다면..
            {
                if (isTable.Equals(true))         // 책상에 닿았으면
                {
                    transform.LookAt(tableObject);      // 테이블을 바라본다.(중앙)

                    AudioMng.ins.PlayEffect("TrainSide");    // 교체 소리
                    rightButtonImage.sprite = sprite_Array[2];

                    net_Player.SetActive(false);        // 잠자리채 비활성화
                    web_player.SetActive(true);         // 그물 활성화

                    net_Table.SetActive(true);          // 테이블 잠자리채 활성화
                    web_Table.SetActive(false);         // 테이블 그물 비활성화

                    currentObject = 1;                  // 이제는 그물을 들고 있다고 알림

                    mini06_Net.isAttack = false;        // 잠자리 채에 붙어 있는 트리거를 비활성화(혹시 몰라서..)
                }
                else                        // 책상에 닿지 않다면...
                {
                    if (isRun.Equals(false))
                    {
                        AudioMng.ins.PlayEffect("Sword");    // 채 휘두르는 소리
                        StartCoroutine(netCoroutine());      // 잠자리 채를 휘두르는 코루틴 실행
                    }
                }
            }
            else                        // 그물을 들고 있다면...
            {
                if (isTable.Equals(true))         // 책상에 닿았으면
                {
                    transform.LookAt(tableObject);      // 테이블을 바라본다.(중앙)

                    AudioMng.ins.PlayEffect("TrainSide");    // 교체 소리
                    rightButtonImage.sprite = sprite_Array[2];

                    web_player.SetActive(false);        // 그물 비활성화
                    net_Player.SetActive(true);         // 잠자리채 활성화

                    net_Table.SetActive(false);         // 테이블 그물 비활성화
                    web_Table.SetActive(true);          // 테이블 잠자리채 활성화

                    currentObject = 0;                // 이제는 잠자리 채를 들고 있다고 알림
                }
                else                          // 책상에 닿지 않았다면...
                {
                    AudioMng.ins.PlayEffect("SpeedUp");    // 채 휘두르는 소리
                    StartCoroutine(Shot_Web_Coroutine());
                }
            }
        }
    }

    void Invoke_Press()
    {
        if (currentObject.Equals(0))
        {
            anim.SetBool(attackId01, false);
        }
        else
        {
            anim.SetBool(attackId02, false);
        }
    }



    ////////////////  코루틴 관련....

    void Invoke_Wait_BearAttack()
    {
        rigid.velocity = Vector3.zero;
    }
     



    IEnumerator netCoroutine()      // 잠자리 채를 휘두르는 코루틴
    {
        isRun = true;                // 잠자리 채를 휘두르고 있다고 알림
        anim.SetBool(attackId01, true);   // 잠자리 채를 휘두르는 애니메이션 실행

		rightButtonImage.fillAmount = 0.0f;     // 도넛폭탄 오른쪽 버튼 이미지
		while (rightButtonImage.fillAmount <= 0.9999f)          // 1로 하면 안된다;;;
		{
			rightButtonImage.fillAmount += Time.deltaTime;

			yield return null;
		}

		yield return null;
        isRun = false;               // 코루틴이 끝났다고 알림
    }


    IEnumerator Shot_Web_Coroutine()        // 그물을 던지는 코루틴
    {
        isRun = true;                // 그물을 휘두르고 있다고 알림

        anim.SetBool(attackId02, true);         // 그물을 던지는 애니메이션 실행

        GameObject t_object = mini06_Spawn.GetQueue_Web();      // 그물 오브젝트 풀링에서 가져옴
        t_object.transform.position = middle.position;     // 발사대에서 시작
        t_object.transform.rotation = middle.rotation;     // 현재 발사대 회전으로 맞춤

        Rigidbody bulletRigid = t_object.GetComponent<Rigidbody>();
        bulletRigid.velocity = transform.forward * 10.0f;               // 그물을 던짐

		rightButtonImage.fillAmount = 0.0f;     // 도넛폭탄 오른쪽 버튼 이미지
		while (rightButtonImage.fillAmount <= 0.9999f)          // 1로 하면 안된다;;;
		{
			rightButtonImage.fillAmount += Time.deltaTime;

			yield return null;
		}

		yield return null;

        isRun = false;               // 코루틴이 끝났다고 알림
    }


    /////////////// 콜리젼 구역...


	void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.layer.Equals(3))      // 테이블에 닿았다면...   WALL
        {
            isTable = true;      // 닿았다고 알려줌

            
            rightButtonImage.sprite = sprite_Array[2];
            
        }
        else if (collision.gameObject.layer.Equals(8))   // 나무에 닿았다면...  Object
        {
            AudioMng.ins.PlayEffect("HitApple");    // 나무 장풍 소리
            rigid.velocity = (transform.position - collision.transform.position).normalized * 50.0f; // 장풍 효과..
        }
    }

	void OnCollisionExit(Collision collision)
	{
        if (collision.gameObject.layer.Equals(3))      // 테이블에서 떨어졌다면....   WALL
        {
            isTable = false;    // 떨어졌다고 알려줌

            if (currentObject.Equals(0))       // 플레이어가 현재 잠자리채를 들고 있다면..
            {
                rightButtonImage.sprite = sprite_Array[0];
            }
            else                  // 플레이어가 현재 그물을 들고 있다면..
            {
                rightButtonImage.sprite = sprite_Array[1];
            }
        }
    }
}
