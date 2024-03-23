using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mini09_Player : MonoBehaviour
{
    [SerializeField] Sprite[] sprite_Array;

    [SerializeField] AudioSource audio_Run;

    [SerializeField] Transform PlainPos;                  // �÷��̾�� ����⸦ ������ ��ġ
    [SerializeField] Mini09_Camera mini09_Camera;         // ī�޶� ��ũ��Ʈ

    [SerializeField] GameObject Slider_Obj;

    [SerializeField] Button Left_Button;
    [SerializeField] Button Right_Button;
    [SerializeField] Image Right_Image;
    [SerializeField] Sprite[] spriteButton_Array;         // 0 : ������, 1 : �Ŀ�, 2 : ����

    Animator anim;                       // �ִϸ����ʹ� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����
    Rigidbody rigid;                 // ������ٵ�� �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    [SerializeField] float speed;        // �̵� �ӵ� ������ �÷��̾�� ���Ͱ� �������� ���� ���ɼ� ����

    [SerializeField] Image speed_Image;                   // UI �Ʒ��� ���ǵ� �̹���
    Sprite[] speedSprite_Array = new Sprite[3];          // 100 �̸� ���ǵ� ��������Ʈ
    [SerializeField] TextMeshProUGUI[] speedText_Array;   // ���ǵ� �ؽ�Ʈ �迭
    float speedInt;                      // ���� ���ǵ� ������ �ؽ�Ʈ�� �ű���� ����

    [SerializeField] GameObject powerCircle;              // �Ŀ� ��Ŭ ������Ʈ(�г�)
    [SerializeField] Transform redLine;                   // �Ŀ� ��Ŭ���� ȸ���ϴ� �������
    [SerializeField] TextMeshProUGUI[] powerCircle_Text;  // �Ŀ� ��Ŭ �ؽ�Ʈ �迭
    [SerializeField] Image power_Image;                   // �Ŀ� �̹���
    Sprite[] power_Sprite = new Sprite[5];               // �Ŀ� ��Ŭ ��������Ʈ �迭

    [SerializeField] GameObject angle;                 // �����Ⱑ ��� ������Ʈ
    public float angle_Float;            // �������� ������ �޴� ����

    [SerializeField] GameObject plain_01;                 // �տ� ��� �ִ� �����
    [SerializeField] GameObject plain_02;                 // ������ �����

    [SerializeField] TextMeshProUGUI distanceText;        // �Ÿ� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI coinText;            // ���� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI heightText;          // ���� �ؽ�Ʈ
    [SerializeField] Transform startPos;                  // �÷��̾� ��ŸƮ ��ġ(���⼭���� 0m)
    [SerializeField] Slider slider;                       // �����̴�

    float z = 0;                     // ȸ�� ����
    int powerInt;                    // �Ŀ� ��Ŭ ȸ�� ��
    int completeInt = 0;             // ���� ȸ�� ���� �޴� ����

    bool pressRun = false;                    // �����̽��ٸ� ������ ���� ����...
    bool leftBool = false;                   // ���� ��ư�� ������ ���� ����...

    int rightButtonCount = 0;       // ������ ��ư�� ��� ������ ���� ����

    bool shooting = false;          // ���� �������� �Ӹ��� ���ϵ��� �ϴ� ����
    bool isFloor = false;           // �ٴڿ� ��Ҵ��� ���� ����

    bool isend = false;         // �÷��̾ �������ٰ� �˸��� ����(�̰͵� ����Ѵ�...)
    bool isFall = false;        // �÷��̾ ��������.

    bool isInvoke = false;      // �κ�ũ�� �����ϰ� �ִ��� ���� ����

    public float distance = 0;         // �Ÿ��� ��Ÿ���� ����
    float height = 0;           // ���̸� ��Ÿ���� ����
    float temp = 0.0f;          // �ִ� ���̸� ��Ÿ���� ����
    public int coinInt = 0;            // ���� ���� ������ ��Ÿ���� ����

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
        if (pressRun.Equals(true) && isend.Equals(false))        // �����̽��ٸ� ������, ���� �÷��̾ �������� �ʾҴٸ�...
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

        if (isend.Equals(false))     // �÷��̾ �������� �ʾҴٸ�..
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
        else                          // �÷��̾ �������ٸ�...
        {
            if (isFallFail.Equals(false))
            {
                Text_Fuction();           // �ؽ�Ʈ�� ǥ���ϴ� �Լ� ����
            }

            if (transform.position.y < -50.0f && isFallFail.Equals(false))        // �÷��̾ ���� �κ� �������ٸ�..
            {
                isFallFail = true;

                mini09_Camera.EndGame();
                StartCoroutine(End_Coroutine());
            }

            if (shooting.Equals(true))                // 
            {
                transform.up = rigid.velocity;        // �÷��̾� ȸ�� ��ġ�� ������ ���� ������ ����.
            }

            if (isInvoke.Equals(true))                // �κ�ũ�� ����ǰ� �ִٸ�..
            {
                if (rigid.velocity != Vector3.zero)       // �ٽ� �����δ�!
                {
                    isInvoke = false;                     // �κ�ũ �ߴ�
                    CancelInvoke(invoke_Text);
                }
                return;        // �ؿ� ���� ���ϰ� �Ѵ�.
            }

            if (rigid.velocity.Equals(Vector3.zero) && isFloor.Equals(true))      // �ӵ��� 0�̰�, �ٴڿ� ��´ٸ�..
            {
                isInvoke = true;                    // �κ�ũ�� �����Ų��.
                Invoke(invoke_Text, 2.0f);
            }
        }
    }

    void Invoke_EndGame()         // ī�޶� ����(���� ��)
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
        distance = transform.position.z - startPos.transform.position.z;    // �Ÿ��� ��Ÿ����.
        height = transform.position.y - startPos.transform.position.y;      // ���̸� ��Ÿ����.
        distanceText.text = distance.ToString("N0");                  // �Ÿ� �ؽ�Ʈ�� ��Ÿ����.

        if (height <= 0.0f)     // ���̰� 0 ���̸�
        {
            height = 0.0f;      // ���̸� 0���� ����
        }

        if (temp <= height)     // ���� ���̰� �� ���̺��� ���ٸ�
        {
            temp = height;              // ���� ���̸� �ִ� ���̷� �Ѵ�.
            slider.maxValue = temp;
            heightText.text = temp.ToString("N1");   // �ִ� ���̸� �ؽ�Ʈ�� �ѱ��.
        }

        slider.value = height;        // �����̴��� ���� ���̷� �����ش�.
    }

    void Move()         // �̵� �Լ��� �θ�ɷ� ��
    {
        transform.position += vector01.normalized * Time.fixedDeltaTime * speed * 0.5f;     // �÷��̾ ������
    }



    public void Press_LeftButton()       // ���� ��ư�� ������
    {
        if (leftBool.Equals(false))     // �ѹ��� �����ҷ��� �ٷ� false�� �ٲ۴�.
        {
            audio_Run.pitch += 0.07f;
            audio_Run.Play();

            speed += Random.Range(2.5f, 4.3f);     // �������� �ӵ��� ���δ�.
            pressRun = true;                       // �����̽��ٸ� �����ٰ� �˸���.
            anim.SetBool(runId, true);           // �޸��� �ִϸ��̼��� ����
            anim.speed += speed * 0.001f;          // �ִϸ��̼� �ӵ��� �ø���.

            speedInt = Mathf.Round(speed * 10) * 0.09f;     // ���ǵ带 �ؽ�Ʈ�� ǥ���Ѵ�.

            if (speedInt < 50.0f)         // ���ǵ尡 50 �̸��̸�
            {
                speedText_Array[0].text = $"<color=white>{speedInt.ToString("N1")} km/h</color>";   // ���ǵ� ������ �Ͼ������...
            }
            else if (speedInt < 100.0f)   // ���ǵ尡 100 �̸��̸�
            {
                speedText_Array[0].text = $"<color=blue>{speedInt.ToString("N1")} km/h</color>";     // ���ǵ� ������ �Ķ�������...
            }
            else                          // ���ǵ尡 100�̻��̸�
            {
                speedText_Array[0].text = $"<color=red>{speedInt.ToString("N1")} km/h</color>";     // ���ǵ� ������ ����������...
            }
        }
    }

    public void Press_RightButton()      // ������ ��ư�� �����ٸ�..
    {
        switch (rightButtonCount)        // ������ ��ư�� �� �� ��������..
        {
            case 0:           // �Ŀ� ��Ŭ�� ����
                AudioMng.ins.LoopEffect(true);
                AudioMng.ins.PlayEffect("Circle");    // �Ŀ� ��Ŭ

                Left_Button.interactable = false;      // ��ư �� ������ �ϱ�...
                
                Right_Image.sprite = spriteButton_Array[1];

                PowerCircle_RightButton();
                break;
            case 1:           // �Ŀ� ��Ŭ�� �������, �����Ⱑ ����
                AudioMng.ins.LoopEffect(false);
                AudioMng.ins.StopEffect();

                Right_Image.sprite = spriteButton_Array[2];

                Angle_RightButton();
                break;
            case 2:           // �����Ⱑ �����
                AudioMng.ins.PlayEffect("SpeedUp");    // ����� �߻�
                ThrowPlain_RightButton();

                Right_Button.interactable = false;      // ��ư �� ������ �ϱ�...

                Left_Button.gameObject.SetActive(false);
                Right_Button.gameObject.SetActive(false);

                Slider_Obj.SetActive(true);

                break;
            default:          // ������ �� ����
                return;
        }
        rightButtonCount++;               // �ѹ� ���������� �ϳ��� �ø�
    }


    void PowerCircle_RightButton()    // �Ŀ���Ŭ�� ��Ÿ���� �Լ�
    {
        if (speedInt < 100.0f)      // ���ǵ尡 100 �̸��̸� 
        {
            speed_Image.sprite = speedSprite_Array[1];    // ��ӱ� �̹��� 1�� ����
        }
        else                        // ���ǵ尡 100 �̻��̸�
        {
            speed_Image.sprite = speedSprite_Array[2];    // ��� �̹��� 2�� ����
        }

        speedText_Array[1].text = speedText_Array[0].text;

        leftBool = true;                      
        pressRun = false;
        anim.SetBool(attackId, true);         // ������ ����� ���Ѵ�.
        anim.SetBool(runId, false);           // �޸��� �ִϸ��̼� ��
        transform.rotation = Quaternion.Euler(new Vector3(0, 45.0f, 0));   // ������ ����� ����� �̷��� �ؾ��Ѵ�..
        powerCircle.SetActive(true);   // �Ŀ� ��Ŭ Ȱ��ȭ
    }

    void Angle_RightButton()      // ������ ��Ÿ���� �Լ�
    {
        powerCircle_Text[0].text = powerCircle_Text[1].text;
        switch (completeInt)
        {
            case 0:       // low
                power_Image.sprite = power_Sprite[1];          // �̹��� �ٲ�
                break;
            case 1:       // normal
                power_Image.sprite = power_Sprite[2];         // �̹��� �ٲ�
                break;
            case 2:       // high
                power_Image.sprite = power_Sprite[3];             // �̹��� �ٲ�
                break;
            default:      // supreme
                power_Image.sprite = power_Sprite[4];           // �̹��� �ٲ�
                break;
        }
        powerCircle.SetActive(false);    // �Ŀ� ��Ŭ�� ��Ȱ��ȭ
        angle.SetActive(true);           // ������� Ȱ��ȭ
    }


    void ThrowPlain_RightButton()        // ���󰡴� ����� ��Ÿ���� �Լ�
    {
        angle.SetActive(false);          // ������ ��Ȱ��ȭ
        transform.rotation = Quaternion.Euler(Vector3.zero);   // 45�� ��￴�� ȸ���� 0���� �����.
        anim.SetBool(throwId, true);                         // ������ �ִϸ��̼� ����

        plain_01.SetActive(false);         // ��� �ִ� ����� ��Ȱ��ȭ
        plain_02.SetActive(true);          // ���󰡴� ����� Ȱ��ȭ
        plain_02.transform.position = PlainPos.position;   // ���󰡴� ����⸦ ������ ��ġ�� ���´�.

        plain_02.GetComponent<Mini09_Plain>().Shotting_Plain(speedInt, completeInt, angle_Float);   // ���󰡴� ����⸦ ������.
        mini09_Camera.Target_Change();       // ī�޶� Ÿ���� ������ �ٲ۴�.
    }


    void PowerCurcle()       // �Ŀ� ��Ŭ ȸ���ϴ� �Լ�
    {
        z += Time.deltaTime * 600;                // ������� ȸ�� �ӵ�
        powerInt = (int)(z * 0.1f) % 36;           // ���� �� ����
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



    void Falling_Player()       // �÷��̾ ������ ��ư�� �� �����ٸ�..
    {
        anim.SetBool(runId, false);    // �޸��� �ִϸ��̼��� ����.
        rigid.constraints &= ~RigidbodyConstraints.FreezePositionY & ~RigidbodyConstraints.FreezePositionZ & ~RigidbodyConstraints.FreezeRotationX;

        AudioMng.ins.LoopEffect(false);

        mini09_Camera.Camera_Chage();

        Left_Button.interactable = false;      // ��ư �� ������ �ϱ�...
        Right_Button.interactable = false;

        if (speedInt < 50.0f)    // ���ǵ尡 50���� ���ٸ�
        {
            transform.position += new Vector3(0, 0, 1);
            rigid.AddForce(new Vector3(0, -1, 1) * 5.0f * speedInt);            // �÷��̾� ����Ʈ����

            //AudioMng.ins.PlayEffect("Fail02");    // �ٴ� ���ۿ� ������
        }
        else                     // ���ǵ尡 50���� ���ٸ�
        {
            transform.position += new Vector3(0, 3, 0);
            transform.rotation = Quaternion.Euler(Vector3.zero);
            rigid.AddForce(new Vector3(0, 1, 1) * 20.0f * speedInt);            // �÷��̾� ���󰡱�

            AudioMng.ins.PlayEffect("Meow");    // �÷��̾� ���󰡱�
        }
    }



    /////////////////////   Ʈ���� ����

    void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.layer.Equals(4))       // �޸��� �ٴ� ���� ������.. Water
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
            other.gameObject.SetActive(false);       // �Դ� ������ ��´ٸ�..
            coinInt++;
            coinText.text = coinInt.ToString();

            AudioMng.ins.PlayEffect("Score_Up");    // ���� ����
        }
        else if (other.gameObject.layer.Equals(8))       // Object
        {
            rigid.AddForce(vector02 * 10000.0f);


            AudioMng.ins.PlayEffect("HitApple");    // ����̵��� ����
        }
        else if (other.gameObject.layer.Equals(1))      // ����
        {
            shooting = false;
            rigid.AddForce(vector02 * 1000.0f);

            AudioMng.ins.PlayEffect("Bomb");    // ����̵� ����
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (isend.Equals(true))
        {
            if (collision.gameObject.layer.Equals(3))   // �÷��̾ �ٴڿ� ��Ҵٸ�    WALL
            {
                shooting = false;
                isFloor = true;                             // �ٴڿ� ��Ҵٸ� �˷���

                AudioMng.ins.PlayEffect("Meow");    // �ٴڿ� ����
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer.Equals(3))       // �÷��̾ �ٴڿ��� �����ٸ�     WALL
        {
            isFloor = false;                           // �ٴڿ��� �������ٰ� �˷���
        }
    }
}
