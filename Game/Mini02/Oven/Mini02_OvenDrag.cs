using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mini02_OvenDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler     // 도넛 반죽에 등록
{
    [SerializeField] Sprite One_Donut_Sprite;
    [SerializeField] Sprite Star_Donut_Sprite;

    [SerializeField] Mini02_Player mini02_Player;         // 플레이어 스크립트
    [SerializeField] Mini02_OvenInput mini02_OvenInput;   // 오븐에 넣은 스크립트


    bool isInOven = false;             // 도넛이 오븐에 닿았는지 묻는 변수
    bool isInput = false;

    Sprite One_Image;        // 구멍 도넛 드래그 이미지
    Sprite Star_Image;       // 스타 도넛 드래그 이미지

    [SerializeField] GameObject One_Donut;    // 쟁반 안에 구멍 도넛
    [SerializeField] GameObject Star_Donut;   // 쟁반 안에 스타 도넛

    Image thisImage;     // 드래그 이미지


    void Awake()
	{
        One_Image = One_Donut_Sprite;
        Star_Image = Star_Donut_Sprite;

        thisImage = gameObject.GetComponent<Image>();
    }


	void OnEnable()   // 켜질 떄...
    {
        if (mini02_Player.isHoleOrStar.Equals(false))   // 구멍
        {
            thisImage.sprite = One_Image;      // 드래그 이미지를 구멍 도넛으로 한다.
        }
        else         // 스타라면
        {
            thisImage.sprite = Star_Image;      // 드래그 이미지를 스타 도넛으로 한다.
        }
    }

    void OnDisable()    // 끝날떄..
    {
        mini02_OvenInput.isOvenInput = false;      // 초기화

        isInOven = false;
        isInput = false;
    }

    public void OnBeginDrag(PointerEventData eventData)        // 드래그가 시작되었을 떄,
    {

        AudioMng.ins.PlayEffect("Click03");      // 도넛 드래그 시작
    }

    public void OnEndDrag(PointerEventData eventData)         // 드래그가 끝났을 떄,
    {
        if (isInOven.Equals(true))            // 도넛이 오븐에 닿았다면
        {
            mini02_OvenInput.isOvenInput = true;           // 도넛이 오븐에 닿았다고 알려줌
            thisImage.sprite = null;    // 도넛이 담겨있는 곳은 이미지를 없애버림

            AudioMng.ins.LoopEffect(true);
            AudioMng.ins.PlayEffect("Oven");      // 도우가 펼쳐지는 상황


            if (mini02_Player.isHoleOrStar.Equals(false))   // 구멍 도넛일 경우
            {
                One_Donut.SetActive(true);   // 구멍 도넛 활성화
            }
            else                        // 스타 도넛이 경우
            {
                Star_Donut.SetActive(true);  // 스타 도넛 활성화
            }
            isInput = true;      // 도넛을 안에 넣었다고 알림
        }

        this.transform.localPosition = Vector3.zero;                                // 최종적으로 원래 위치로 되돌리도록 함
    }

    public void OnDrag(PointerEventData eventData)   // 드래그 하는 중...
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
            isInOven = true;          // 닿았다고 함

        }
        else
        {
            isInOven = false;         // 안 닿았다고 함

        }
    }




}
