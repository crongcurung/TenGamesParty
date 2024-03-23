using UnityEngine;
using UnityEngine.EventSystems;

//[System.Serializable]
//public enum JoysticDirection { Horizontal, Vertical, Both }

public class Joystic : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	[SerializeField] RectTransform Background;
	//public JoysticDirection joysticDirection = JoysticDirection.Both;
	[SerializeField] RectTransform Handle;
	//public float HandleLimit = 1.0f;
	Vector2 input = Vector2.zero;

	public float Vertical { get { return input.y; } }
	public float Horizontal { get { return input.x; } }
	Vector2 JoyPosition = Vector2.zero;


	public bool isZero = true;

	public void OnPointerDown(PointerEventData eventData)
	{
		//
		Background.gameObject.SetActive(true);
		OnDrag(eventData);
		JoyPosition = eventData.position;
		Background.position = eventData.position;
		Handle.anchoredPosition = Vector2.zero;
		input = Vector3.zero;
	}


	public void OnDrag(PointerEventData eventData)
	{
		isZero = false;

		Vector2 JoyDriection = eventData.position - JoyPosition;
		input = (JoyDriection.magnitude > Background.sizeDelta.x / 2.0f) ? JoyDriection.normalized :
			JoyDriection / (Background.sizeDelta.x / 2.0f);

		//if (joysticDirection == JoysticDirection.Horizontal)
		//{
		//	input = new Vector2(input.x, 0.0f);
		//}
		//if (joysticDirection == JoysticDirection.Vertical)
		//{
		//	input = new Vector2(0.0f, input.y);
		//}

		//Handle.anchoredPosition = (input * Background.sizeDelta.x / 2.0f) * HandleLimit;

		Handle.anchoredPosition = input * Background.sizeDelta.x / 2.0f;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Background.gameObject.SetActive(false);
		input = Vector2.zero;
		Handle.anchoredPosition = Vector2.zero;

		isZero = true;
	}
}
