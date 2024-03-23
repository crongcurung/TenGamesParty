using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mini02_FrameDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler     // ���� ���׿� ���
{
    [SerializeField] Mini02_FramePanel mini02_FramePanel;

    [SerializeField] GameObject spreadDough;                // ���� ���� ������Ʈ
    bool isDomaInput = false;                     // �巡�� ���� �̹����� ������ ��Ҵ��� ���� ����

    Sprite originSprite;                     // �巡�� ���� �̹����� ��� ����
    bool isInput = false;                    // �巡�� �̹����� �߾ӿ� �־���?

    Image thisImage;        // �巡�� �̹����� �޴� ����


	void Awake()
	{
        thisImage = gameObject.GetComponent<Image>();     // �巡�� �̹����� �����´�.
    }

	void OnEnable()
    {
        originSprite = thisImage.sprite;    // �̹��� ��Ƶα�(�ʱ�ȭ�� ����...)
    }

    void OnDisable()
    {
        thisImage.sprite = originSprite;     // �̹��� �ʱ�ȭ
        spreadDough.SetActive(false);                               // �гο��� ������ ���� ������ ����.
        mini02_FramePanel.isFrameInput = false;               // ���찡 ������ ���ٴ� ������ �ʱ�ȭ
        isDomaInput = false;                                        // ���찡 ������ ��Ҵٴ� ������ �ʱ�ȭ
        isInput = false;                                            // �־��Ĵ� ������ �ʱ�ȭ
    }

    public void OnBeginDrag(PointerEventData eventData)    // �巡�װ� ���۵� ��
    {

        AudioMng.ins.PlayEffect("Click03");      // ���� �巡�� ����

    }

    public void OnEndDrag(PointerEventData eventData)      // �巡�װ� ���� ��
    {
        if (isDomaInput.Equals(true))                     // �巡�� ���� �̹����� ������ ��Ҵٸ� (�巡�װ� ���� �����̴� �巡�� ������ ������ �־����� �� �� �ִ�.)
        {
            mini02_FramePanel.isFrameInput = true;      // �巡�� ������ ������ �������ٰ� �˷���
            spreadDough.SetActive(true);                      // ���� ���� ������Ʈ�� Ȱ��ȭ

            AudioMng.ins.PlayEffect("Cloud");      // ���찡 �������� ��Ȳ

            thisImage.sprite = null;    // ������ ����ִ� ���� �̹����� ���ֹ���

            isInput = true;                                   // �̹����� �ȿ� �־��ٰ� �˸�
        }

        this.transform.localPosition = Vector3.zero;                                // ���������� ���� ��ġ�� �ǵ������� ��
    }

    public void OnDrag(PointerEventData eventData)        // �巡�� �ϴ� ��...
    {
        if (isInput.Equals(true))      // �̹� �ȿ� �־��ٸ� ���� ����
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
        if (transform.localPosition.y < -300 && transform.localPosition.y > -850 && transform.localPosition.x > -150 && transform.localPosition.x < 950)
        {
            isDomaInput = true;          // ��Ҵٰ� ��

        }
        else
        {
            isDomaInput = false;         // �� ��Ҵٰ� ��

        }
    }
}
