using UnityEngine;
using UnityEngine.EventSystems;

public class Mini05_SwipeParent : MonoBehaviour, IPointerDownHandler             // 캐릭터 중앙에 부착됨
{
    [SerializeField] Mini05_Swipe miniGame06_SwipeRight;      // 오른쪽 스와이프의 스크립트
    [SerializeField] Mini05_Swipe miniGame06_SwipeLeft;       // 왼쪽 스와이프의 스크립트
    [SerializeField] Mini05_Swipe miniGame06_SwipeButton;     // 오른쪽 버튼 스와이프의 스크립트

    public void OnPointerDown(PointerEventData eventData)         // 캐릭터 중앙쪽에서 눌렀다면..
    {
        miniGame06_SwipeRight.isTouch = true;      // 눌렀다고 알려줌
        miniGame06_SwipeLeft.isTouch = true;      // 눌렀다고 알려줌

        miniGame06_SwipeButton.isTouch = true;      // 눌렀다고 알려줌
    }
}
