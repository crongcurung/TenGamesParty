using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mini02_FryDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler     // 도넛 반죽에 등록
{
    [SerializeField] Sprite One_Donut_Sprite;
    [SerializeField] Sprite Star_Donut_Sprite;

    [SerializeField] Mini02_Player mini02_Player;       // 플레이어 스크립트
    [SerializeField] Mini02_FryInput mini02_FryInput;   // 튀김 안 스크립트


    bool isInFryer = false;               // 튀김기에 도넛이 닿았는지 묻는 변수
    bool isInput = false;                 // 튀김기에 도넛을 넣었는지 묻는 변수

    Sprite One_Image;        // 구멍 도넛 드래그 이미지
    Sprite Star_Image;       // 스타 도넛 드래그 이미지

    [SerializeField] GameObject One_Donut;    // 프라이팬 안에 구멍 도넛
    [SerializeField] GameObject Star_Donut;   // 프라이팬 안에 스타 도넛

    Image thisImage;        // 드래그 이미지를 받는 변수


    void Awake()
	{
        One_Image = One_Donut_Sprite;
        Star_Image = Star_Donut_Sprite;

        thisImage = gameObject.GetComponent<Image>();
    }


	void OnEnable()
    {
        if (mini02_Player.isHoleOrStar.Equals(false))  // 구멍이라면...
        {
            thisImage.sprite = One_Image;      // 드래그 이미지를 구멍 도넛으로 한다.
        }
        else          // 스타라면...
        {
            thisImage.sprite = Star_Image;      // 드래그 이미지를 스타 도넛으로 한다.
        }
    }

    void OnDisable()    // 끝날때......
    {
        mini02_FryInput.isFryerInput = false;     // 드래그 이미지를 튀김기 안에 초기화
        isInFryer = false;     
        isInput = false;
    }

    public void OnBeginDrag(PointerEventData eventData)    // 드래그가 시작될 때
    {
        AudioMng.ins.PlayEffect("Click03");      // 도넛 드래그 시작
    }

    public void OnEndDrag(PointerEventData eventData)      // 드래그가 끝날 떄
    {
        if (isInFryer.Equals(true))                                    // 드래그가 끝났을 떄 도넛이 튀김기에 닿았는지..?
        {
            thisImage.sprite = null;            // 도넛이 튀김기에 있다면 도넛 반죽에 있는 이미지는 없앤다.

            AudioMng.ins.LoopEffect(true);
            AudioMng.ins.PlayEffect("Fry");      // 튀김이 들어가는 상황

            if (mini02_Player.isHoleOrStar.Equals(false))
            {
                One_Donut.SetActive(true);   // 구멍 도넛 활성화
            }
            else
            {
                Star_Donut.SetActive(true);   // 스타 도넛 활성화
            }

            mini02_FryInput.isFryerInput = true;                     // 튀김기에 도넛이 있다고 알려줌
            isInput = true;                                        // 튀김기에 도넛이 있다는 것을 알리는 변수를 또 true로 바꾼다.
        }
        else                 // 드래그가 끝났을때 도넛이 튀김기에 없다...
        {

        }

        this.transform.localPosition = Vector3.zero;                                // 최종적으로 원래 위치로 되돌리도록 함
    }

    public void OnDrag(PointerEventData eventData)        // 드래그 하는 중...
    {
        if (isInput.Equals(true))               // 튀김기에 도넛이 있다면 드래그 안되도록 함
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
            isInFryer = true;          // 닿았다고 함


        }
        else
        {
            isInFryer = false;         // 안 닿았다고 함

        }
    }
}
