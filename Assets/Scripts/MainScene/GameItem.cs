using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace MainScene
{
	public class GameItem : MonoBehaviour
	{
		[SerializeField] private Image background;
		[SerializeField] private Image icon;
		[SerializeField] private Sprite[] backgrounds;
		[SerializeField] private Sprite[] icons;
		[SerializeField] private Animator anim;
		private string animOnWin = "onWin";
		private void Awake()
		{
			background.ThrowIfNull();
			icon.ThrowIfNull();
		}
		public void SetDogWin()
		{
			icon.gameObject.SetActive(true);
			background.sprite = backgrounds[0];
			icon.sprite = icons[0];
		}
		public void SetCatWin()
		{
			icon.gameObject.SetActive(true);
			background.sprite = backgrounds[1];
			icon.sprite = icons[1];
		}
		public void SetDraw()
		{
			icon.gameObject.SetActive(false);
			background.sprite = backgrounds[2];
		}
		public void Reset()
		{
			background.sprite = backgrounds[3];
			icon.gameObject.SetActive(false);
		}
		public void RunAnimOnWin()
		{
			anim.ResetTrigger(animOnWin);
			anim.SetTrigger(animOnWin);
		}
	}
}
