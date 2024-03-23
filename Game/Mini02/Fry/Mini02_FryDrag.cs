using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mini02_FryDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler     // ���� ���׿� ���
{
    [SerializeField] Sprite One_Donut_Sprite;
    [SerializeField] Sprite Star_Donut_Sprite;

    [SerializeField] Mini02_Player mini02_Player;       // �÷��̾� ��ũ��Ʈ
    [SerializeField] Mini02_FryInput mini02_FryInput;   // Ƣ�� �� ��ũ��Ʈ


    bool isInFryer = false;               // Ƣ��⿡ ������ ��Ҵ��� ���� ����
    bool isInput = false;                 // Ƣ��⿡ ������ �־����� ���� ����

    Sprite One_Image;        // ���� ���� �巡�� �̹���
    Sprite Star_Image;       // ��Ÿ ���� �巡�� �̹���

    [SerializeField] GameObject One_Donut;    // �������� �ȿ� ���� ����
    [SerializeField] GameObject Star_Donut;   // �������� �ȿ� ��Ÿ ����

    Image thisImage;        // �巡�� �̹����� �޴� ����


    void Awake()
	{
        One_Image = One_Donut_Sprite;
        Star_Image = Star_Donut_Sprite;

        thisImage = gameObject.GetComponent<Image>();
    }


	void OnEnable()
    {
        if (mini02_Player.isHoleOrStar.Equals(false))  // �����̶��...
        {
            thisImage.sprite = One_Image;      // �巡�� �̹����� ���� �������� �Ѵ�.
        }
        else          // ��Ÿ���...
        {
            thisImage.sprite = Star_Image;      // �巡�� �̹����� ��Ÿ �������� �Ѵ�.
        }
    }

    void OnDisable()    // ������......
    {
        mini02_FryInput.isFryerInput = false;     // �巡�� �̹����� Ƣ��� �ȿ� �ʱ�ȭ
        isInFryer = false;     
        isInput = false;
    }

    public void OnBeginDrag(PointerEventData eventData)    // �巡�װ� ���۵� ��
    {
        AudioMng.ins.PlayEffect("Click03");      // ���� �巡�� ����
    }

    public void OnEndDrag(PointerEventData eventData)      // �巡�װ� ���� ��
    {
        if (isInFryer.Equals(true))                                    // �巡�װ� ������ �� ������ Ƣ��⿡ ��Ҵ���..?
        {
            thisImage.sprite = null;            // ������ Ƣ��⿡ �ִٸ� ���� ���׿� �ִ� �̹����� ���ش�.

            AudioMng.ins.LoopEffect(true);
            AudioMng.ins.PlayEffect("Fry");      // Ƣ���� ���� ��Ȳ

            if (mini02_Player.isHoleOrStar.Equals(false))
            {
                One_Donut.SetActive(true);   // ���� ���� Ȱ��ȭ
            }
            else
            {
                Star_Donut.SetActive(true);   // ��Ÿ ���� Ȱ��ȭ
            }

            mini02_FryInput.isFryerInput = true;                     // Ƣ��⿡ ������ �ִٰ� �˷���
            isInput = true;                                        // Ƣ��⿡ ������ �ִٴ� ���� �˸��� ������ �� true�� �ٲ۴�.
        }
        else                 // �巡�װ� �������� ������ Ƣ��⿡ ����...
        {

        }

        this.transform.localPosition = Vector3.zero;                                // ���������� ���� ��ġ�� �ǵ������� ��
    }

    public void OnDrag(PointerEventData eventData)        // �巡�� �ϴ� ��...
    {
        if (isInput.Equals(true))               // Ƣ��⿡ ������ �ִٸ� �巡�� �ȵǵ��� ��
        {
            return;
        }

        Vector3 vec = Camera.main.WorldToScreenPoint(transform.position);
        vec.x += eventData.delta.x;
        vec.y += eventData.delta.y;
        transform.position = Camera.main.ScreenToWorldPoint(vec);
    }


    void Update()
    {
        if (transform.localPosition.y < 550 && transform.localPosition.y > 0 && transform.localPosition.x > 400 && transform.localPosition.x < 1500)
        {
            isInFryer = true;          // ��Ҵٰ� ��


        }
        else
        {
            isInFryer = false;         // �� ��Ҵٰ� ��

        }
    }
}
