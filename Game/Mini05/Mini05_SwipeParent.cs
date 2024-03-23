using UnityEngine;
using UnityEngine.EventSystems;

public class Mini05_SwipeParent : MonoBehaviour, IPointerDownHandler             // ĳ���� �߾ӿ� ������
{
    [SerializeField] Mini05_Swipe miniGame06_SwipeRight;      // ������ ���������� ��ũ��Ʈ
    [SerializeField] Mini05_Swipe miniGame06_SwipeLeft;       // ���� ���������� ��ũ��Ʈ
    [SerializeField] Mini05_Swipe miniGame06_SwipeButton;     // ������ ��ư ���������� ��ũ��Ʈ

    public void OnPointerDown(PointerEventData eventData)         // ĳ���� �߾��ʿ��� �����ٸ�..
    {
        miniGame06_SwipeRight.isTouch = true;      // �����ٰ� �˷���
        miniGame06_SwipeLeft.isTouch = true;      // �����ٰ� �˷���

        miniGame06_SwipeButton.isTouch = true;      // �����ٰ� �˷���
    }
}
