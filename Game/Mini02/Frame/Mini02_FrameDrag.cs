using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mini02_FrameDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler     // 도넛 반죽에 등록
{
    [SerializeField] Mini02_FramePanel mini02_FramePanel;

    [SerializeField] GameObject spreadDough;                // 펴진 도넛 오브젝트
    bool isDomaInput = false;                     // 드래그 도넛 이미지가 도마에 닿았는지 묻는 변수

    Sprite originSprite;                     // 드래그 도넛 이미지를 담는 변수
    bool isInput = false;                    // 드래그 이미지를 중앙에 넣었냐?

    Image thisImage;        // 드래그 이미지를 받는 변수


	void Awake()
	{
        thisImage = gameObject.GetComponent<Image>();     // 드래그 이미지를 가져온다.
    }

	void OnEnable()
    {
        originSprite = thisImage.sprite;    // 이미지 담아두기(초기화를 위한...)
    }

    void OnDisable()
    {
        thisImage.sprite = originSprite;     // 이미지 초기화
        spreadDough.SetActive(false);                               // 패널에서 나갈때 펴진 반죽을 끈다.
        mini02_FramePanel.isFrameInput = false;               // 도우가 도마에 들어갔다는 변수를 초기화
        isDomaInput = false;                                        // 도우가 도마에 닿았다는 변수를 초기화
        isInput = false;                                            // 넣었냐는 변수를 초기화
    }

    public void OnBeginDrag(PointerEventData eventData)    // 드래그가 시작될 때
    {

        AudioMng.ins.PlayEffect("Click03");      // 도넛 드래그 시작

    }

    public void OnEndDrag(PointerEventData eventData)      // 드래그가 끝날 떄
    {
        if (isDomaInput.Equals(true))                     // 드래그 도넛 이미지가 도마에 닿았다면 (드래그가 끝난 시점이니 드래그 도넛이 도마에 넣어졌다 볼 수 있다.)
        {
            mini02_FramePanel.isFrameInput = true;      // 드래그 도넛이 도마에 놓여졌다고 알려줌
            spreadDough.SetActive(true);                      // 펴진 도넛 오브젝트를 활성화

            AudioMng.ins.PlayEffect("Cloud");      // 도우가 펼쳐지는 상황

            thisImage.sprite = null;    // 도넛이 담겨있는 곳은 이미지를 없애버림

            isInput = true;                                   // 이미지를 안에 넣었다고 알림
        }

        this.transform.localPosition = Vector3.zero;                                // 최종적으로 원래 위치로 되돌리도록 함
    }

    public void OnDrag(PointerEventData eventData)        // 드래그 하는 중...
    {
        if (isInput.Equals(true))      // 이미 안에 넣었다면 실행 안함
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
            isDomaInput = true;          // 닿았다고 함

        }
        else
        {
            isDomaInput = false;         // 안 닿았다고 함

        }
    }
}
