using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini07_Player : MonoBehaviour
{
    [SerializeField] GameObject Result_Panel;
    [SerializeField] GameObject Game_Panel;

	[SerializeField] Button GPGS_Button;         // 점수를 서버에 등록시킬 수 있는 버튼

	[SerializeField] Button left_Button;
    [SerializeField] Button rigit_Button;

    [SerializeField] Mini07_Spawn mini07_Spawn;
    [SerializeField] Transform rail_SpawnPos;

    Animator anim;                       // 애니메이터는 플레이어와 몬스터가 공통으로 지닐 가능성 높음
    [SerializeField] float speed;        // 이동 속도 변수는 플레이어와 몬스터가 공통으로 지닐 가능성 높음

    [SerializeField] GameObject Straight_Player;

    [SerializeField] TextMeshProUGUI speedText;            // 스피드를 나타내는 텍스트
    [SerializeField] TextMeshProUGUI coinText;

    [SerializeField] ParticleSystem particle_Left;        // 왼쪽 파티클
    [SerializeField] ParticleSystem particle_Right;       // 오른쪽 파티클

    ParticleSystem.MainModule tempParticle_Left;    // 왼쪽 파티클의 속도를 받아줄 변수
    ParticleSystem.MainModule tempParticle_Right;   // 오른쪽 파티클의 속도를 받아줄 변수

    int palyer_Pos = 0;             // 현재 플레이어는 어느 레일이 위치해 있냐는 변수
    int coinInt;
    bool isEnd = false;

    Vector3 railOffSet;


    WaitForSeconds delay_01;

    int attackId;

    void Start()
    {
        anim = transform.GetChild(1).GetComponent<Animator>();

        attackId = Animator.StringToHash("isAttack");

        anim.SetBool(attackId, true);

        railOffSet = new Vector3(1.5f, 0, 0);

        speedText.text = (speed * 2).ToString("N0");

        tempParticle_Left = particle_Left.main;     // 변수에 파티클을 담아둠(파티클은 이렇게 해야함..)
        tempParticle_Right = particle_Right.main;

        tempParticle_Left.simulationSpeed = 2.0f;     // 처음은 이 속도로
        tempParticle_Right.simulationSpeed = 2.0f;

        delay_01 = new WaitForSeconds(0.2f);
        StartCoroutine(Rail_StratCoroutine());

        AudioMng.ins.Play_BG("Mini07_B");
    }

    


    void Update()
    {
        if (isEnd.Equals(true))
        {
            return;
        }

        transform.position += Vector3.forward * Time.deltaTime * speed;        // 기차는 계속 앞으로 간다.

        if (Input.GetKeyDown(KeyCode.D) == true)       // 오른쪽 버튼을 누른다면...
        {
            Press_RightButton();
        }
        else if (Input.GetKeyDown(KeyCode.A) == true)       // 왼쪽 버튼을 누른다면...
        {
            Press_LeftButton();
        }
    }




    public void Press_RightButton()
    {
        if (palyer_Pos <= 2)       // 플레이어가 2 이하 레일에 있다면..
        {
            palyer_Pos++;          // 레일 번호를 추가함

            if (palyer_Pos >= 3)   // 플레이어가 3 이상 레일에 있다면..
            {
                palyer_Pos = 2;    // 2로 고정
            }
            else             // 플레이어가 3 미만 레일에 있다면..
            {
                GameObject t_object = mini07_Spawn.GetQueue_Right();          // 오른쪽 레일을 담음
                t_object.transform.position = rail_SpawnPos.position + railOffSet;  // 오른쪽 레일 위치 조정

                transform.position = new Vector3(palyer_Pos * 3, 0, transform.position.z);     // 플레이어를 오른쪽 레일로 이동
            }
        }
    }

    public void Press_LeftButton()
    {
        if (palyer_Pos >= -2)         // 플레이어가 -2 이상 레일에 있다면...
        {
            palyer_Pos--;             // 레일 번호를 뺌

            if (palyer_Pos <= -3)     // 플레이어가 -3 이하 레일에 있다면...
            {
                palyer_Pos = -2;      // -2로 고정
            }
            else               // 플레이어가 -3 초과 레일에 있다면..
            {
                GameObject t_object = mini07_Spawn.GetQueue_Left();             // 왼쪽 레일을 담음
                t_object.transform.position = rail_SpawnPos.position - railOffSet;   // 왼쪽 레일 위치 조정 

                transform.position = new Vector3(palyer_Pos * 3, 0, transform.position.z);  // 플레이어를 왼쪽 레일로 이동
            }
        }
    }



    bool isParticle = false;

    public void SpeedUp()         // 스피드 업 함수
    {
        speed += 0.5f;            // 함수가 실행될때마다 스피드를 올린다.
        anim.speed += 0.01f;      // 레일 던지는 애니메이션 속도도 올린다.
        

        if (isParticle.Equals(false) && speed > 40)
        {
            isParticle = true;
            particle_Left.gameObject.SetActive(true);
            particle_Right.gameObject.SetActive(true);
        }

        speedText.text = (speed * 2).ToString("N0");
    }

    

    /////////////////////////////// 코루틴 구역
    IEnumerator Rail_StratCoroutine()   // 직선 레일을 까는 코루틴
    {
        while (isEnd.Equals(false))
        {
            GameObject t_object = mini07_Spawn.GetQueue_Straight();     // 직선 레일을 가져오는 함수
            t_object.transform.position = rail_SpawnPos.position;
            t_object.transform.rotation = rail_SpawnPos.rotation;

            yield return delay_01;
        }
    }


    IEnumerator End_Coroutine()
    {
        yield return new WaitForSeconds(5.0f);

        Result_Panel.SetActive(true);
        Game_Panel.SetActive(true);

        Result_Panel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score : " + coinInt.ToString();

        if (Main.ins.nowPlayer.maxScore_List[6] >= coinInt)    // 최고점을 못 넘은다면...
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + Main.ins.nowPlayer.maxScore_List[6].ToString();
        }
        else        // 최고점을 넘은 경우 (신기록)
        {
            Result_Panel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Record : " + coinInt.ToString();

            Main.ins.nowPlayer.maxScore_List[6] = coinInt;
            Main.ins.SaveData();

			GPGS_Button.interactable = true;        // 신기록을 세우면 점수 서버 등록 버튼을 활성화한다. /////////////////////////////////////////////////////////////////////////////
		}

        AudioMng.ins.Pause_BG();              // 배경음악을 끈다.
        AudioMng.ins.PlayEffect("Fail02");

        Time.timeScale = 0;
    }


	public void Press_GPGS_07()   // 신기록 시, 서버에 등록
	{
		GPGS_Binder.Inst.ReportLeaderboard(GPGSIds.leaderboard_minigame_no7, coinInt);   // 점수를 서버에 등록
		GPGS_Binder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_minigame_no7);          // 리더보드를 띄운다.
	}


	///////////////////////////////  트리거 구역..



	void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(8))    // 장애물 오브젝트에 닿으면..
        {
            isEnd = true;     // 게임 종료로 바꿈
            Straight_Player.SetActive(false);

            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;       // 플레이어의 리지드바디의 고정된 것을 푼다.

            transform.GetComponent<Rigidbody>().velocity = Vector3.up * 15.0f;     // 충돌 표현

            left_Button.interactable = false;
            rigit_Button.interactable = false;

            AudioMng.ins.PlayEffect("Bomb");    // 충돌하는 소리
            anim.SetBool(attackId, false);               // 레일을 까는 애니메이션을 끊다.

            StartCoroutine(End_Coroutine());
        }
    }


    void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.layer.Equals(4))
        {
            other.gameObject.SetActive(false);

            AudioMng.ins.PlayEffect("Score_Up");    // 코인 먹는 소리
            coinInt++;
            coinText.text = coinInt.ToString("N0");
        }
	}
}
