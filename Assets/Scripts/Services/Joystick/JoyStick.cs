using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Services
{
	[System.Serializable]
	public enum JoyStickDirection
	{
		Horizontal,
		Vetical,
		Both,
	}
	[System.Serializable]
	public enum Side
	{
		Red,
		Blue
	}
	public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		public JoyStickDirection joyStickDirection = JoyStickDirection.Both;
		[SerializeField] public RectTransform background;
		[SerializeField] public RectTransform handler;
		[SerializeField] public bool IsFloat;
		[Range(0, 2f)] public float HandleLimit = 1f;
		[Space(8.0f)]
		[SerializeField] private Side side = Side.Red;
		[Space(8.0f)]
		[SerializeField] private Sprite[] spriteLegs;
		[SerializeField] private Sprite[] spriteBackgrounds;
		[Space(8.0f)]
		[SerializeField] private Image backgroundImg;
		[SerializeField] private Image handlerImg;
		public float Vertical { get { return input.y; } }
		public float Horizontal { get { return input.x; } }

		Vector2 joyPosition = Vector2.zero;
		private Vector2 input = Vector2.zero;
		private int touchId;
		private bool isDragging = false;

		private void Start()
		{
			if (IsFloat)
			{
				background.gameObject.SetActive(false);
			}
			if(side == Side.Red)
			{
				backgroundImg.sprite = spriteBackgrounds[0];
				handlerImg.sprite = spriteLegs[0];
			}
			else
			{
				backgroundImg.sprite = spriteBackgrounds[1];
				handlerImg.sprite = spriteLegs[1];
			}
		}
		public void OnPointerDown(PointerEventData eventData)
		{
			if(isDragging == false)
			{
				isDragging = true;
				touchId = eventData.pointerId;

				background.gameObject.SetActive(true);
				OnDrag(eventData);
				joyPosition = eventData.position;
				if (IsFloat)
				{
					background.position = eventData.position;
					handler.anchoredPosition = Vector2.zero;
				}
			}
		}
		public void OnPointerUp(PointerEventData eventData)
		{
			if(isDragging == true && touchId == eventData.pointerId)
			{
				isDragging = false;
				if (IsFloat)
				{
					background.gameObject.SetActive(false);
				}
				input = Vector2.zero;
				handler.anchoredPosition = Vector2.zero;
			}
		}
		public void OnDrag(PointerEventData eventData)
		{
			if(touchId == eventData.pointerId)
			{
				Vector2 joyDirection = eventData.position - joyPosition;
				input = (joyDirection.magnitude > background.sizeDelta.x / 2f) ? joyDirection.normalized : joyDirection / (background.sizeDelta.x / 2f);
				if (joyStickDirection == JoyStickDirection.Horizontal)
					input = new Vector2(input.x, 0f);
				if (joyStickDirection == JoyStickDirection.Vetical)
					input = new Vector2(0f, input.y);
				handler.anchoredPosition = (input * background.sizeDelta.x / 2f) * HandleLimit;
				handler.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(-Horizontal, Vertical) * Mathf.Rad2Deg);
			}
		}
	}
}
