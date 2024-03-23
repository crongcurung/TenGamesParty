using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mini02_OvenDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler     // ���� ���׿� ���
{
    [SerializeField] Sprite One_Donut_Sprite;
    [SerializeField] Sprite Star_Donut_Sprite;

    [SerializeField] Mini02_Player mini02_Player;         // �÷��̾� ��ũ��Ʈ
    [SerializeField] Mini02_OvenInput mini02_OvenInput;   // ���쿡 ���� ��ũ��Ʈ


    bool isInOven = false;             // ������ ���쿡 ��Ҵ��� ���� ����
    bool isInput = false;

    Sprite One_Image;        // ���� ���� �巡�� �̹���
    Sprite Star_Image;       // ��Ÿ ���� �巡�� �̹���

    [SerializeField] GameObject One_Donut;    // ��� �ȿ� ���� ����
    [SerializeField] GameObject Star_Donut;   // ��� �ȿ� ��Ÿ ����

    Image thisImage;     // �巡�� �̹���


    void Awake()
	{
        One_Image = One_Donut_Sprite;
        Star_Image = Star_Donut_Sprite;

        thisImage = gameObject.GetComponent<Image>();
    }


	void OnEnable()   // ���� ��...
    {
        if (mini02_Player.isHoleOrStar.Equals(false))   // ����
        {
            thisImage.sprite = One_Image;      // �巡�� �̹����� ���� �������� �Ѵ�.
        }
        else         // ��Ÿ���
        {
            thisImage.sprite = Star_Image;      // �巡�� �̹����� ��Ÿ �������� �Ѵ�.
        }
    }

    void OnDisable()    // ������..
    {
        mini02_OvenInput.isOvenInput = false;      // �ʱ�ȭ

        isInOven = false;
        isInput = false;
    }

    public void OnBeginDrag(PointerEventData eventData)        // �巡�װ� ���۵Ǿ��� ��,
    {

        AudioMng.ins.PlayEffect("Click03");      // ���� �巡�� ����
    }

    public void OnEndDrag(PointerEventData eventData)         // �巡�װ� ������ ��,
    {
        if (isInOven.Equals(true))            // ������ ���쿡 ��Ҵٸ�
        {
            mini02_OvenInput.isOvenInput = true;           // ������ ���쿡 ��Ҵٰ� �˷���
            thisImage.sprite = null;    // ������ ����ִ� ���� �̹����� ���ֹ���

            AudioMng.ins.LoopEffect(true);
            AudioMng.ins.PlayEffect("Oven");      // ���찡 �������� ��Ȳ


            if (mini02_Player.isHoleOrStar.Equals(false))   // ���� ������ ���
            {
                One_Donut.SetActive(true);   // ���� ���� Ȱ��ȭ
            }
            else                        // ��Ÿ ������ ���
            {
                Star_Donut.SetActive(true);  // ��Ÿ ���� Ȱ��ȭ
            }
            isInput = true;      // ������ �ȿ� �־��ٰ� �˸�
        }

        this.transform.localPosition = Vector3.zero;                                // ���������� ���� ��ġ�� �ǵ������� ��
    }

    public void OnDrag(PointerEventData eventData)   // �巡�� �ϴ� ��...
    {
        if (isInput.Equals(true))
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
        if (transform.localPosition.y < 750 && transform.localPosition.y > 200 && transform.localPosition.x > 600 && transform.localPosition.x < 1600)
        {
            isInOven = true;          // ��Ҵٰ� ��

        }
        else
        {
            isInOven = false;         // �� ��Ҵٰ� ��

        }
    }




}
