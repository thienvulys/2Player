using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Extensions;
using TMPro;

namespace MainScene
{
	public class ScrollViewEx : MonoBehaviour
	{
		[Header("SCROLL PREFERENCE")]
		[SerializeField] private RectTransform rectTransform;
		[SerializeField] private RectTransform mainCanvas;
		[SerializeField] private RectTransform screenPanel;
		[SerializeField] private RectTransform mainScrollContentPanel;
		[SerializeField] private RectTransform parentPanel;
		[SerializeField] private Scrollbar scrollbar;
		[SerializeField] private GameObject gameItemPrefab;
		#region SCROLL CONFIG
		[Header("SCROLL CONFIG")]
		public Action<MainSceneModel.GameItem , bool> OnItemClick;
		public Action<MainSceneModel.GameItem,bool> OnFavoriteChange;
		[SerializeField] private float spacing = 40f;
		[SerializeField] private float paddingTop = 20f;
		[SerializeField] private float paddingBottom = 20f;
		[SerializeField] private float paddingLeftRight = 40f;
		//[SerializeField] private float autoScrollSpeed = 5f;
		#endregion

		private Dictionary<int, GameItemBtn> items = new Dictionary<int, GameItemBtn>();
		private List<MainSceneModel.GameItem> listItems;
		private Rect safeArea;

		private int totalDisplayItems = 0;
		private int col = 1;
		private int row = 0;
		private float itemScaler;
		private float mainPanelHeight;
		private float contentPanelHeight;
		private float itemHeight;
		private float itemWidth;
		private float destinationPosY = Mathf.Infinity;
		private bool isWideScreen = false;
		private bool isSroll = false;
		private bool isRebuild = false;

		private int theme = 0;
		private void Awake()
		{
			//Stop when any preference is Null
			mainScrollContentPanel.ThrowIfNull();
			parentPanel.ThrowIfNull();
			screenPanel.ThrowIfNull();
			mainScrollContentPanel.ThrowIfNull();
			parentPanel.ThrowIfNull();
			scrollbar.ThrowIfNull();
			gameItemPrefab.ThrowIfNull();
		}

		private void Start()
		{
			spacing /= mainCanvas.localScale.x;
			//paddingLeftRight /= mainCanvas.localScale.x;
			//paddingTop /= mainCanvas.localScale.x;
			//paddingBottom /= mainCanvas.localScale.x;
		}

		public void Init(List<MainSceneModel.GameItem> listItems ,string noContentText , Rect safeArea ,bool isWideScreen , bool isReverse = false)
		{
			this.listItems = listItems;
			this.safeArea = safeArea;
			this.isWideScreen = isWideScreen;

			this.listItems = listItems
				.FindAll((item) => item.isDisplayOnScreen).ToList();

			if (isReverse) this.listItems.Reverse();
			ClearAllItem();

			StartCoroutine(OnInit());
		}

		private IEnumerator OnInit()
		{
			//Initial Item display calculation and scroll event setting
			scrollbar.value = 1f;
			CalculatorItemOnScreen();
			isRebuild = false;
			scrollbar.value = 1f;
			OnScroll();
			yield return new WaitForSeconds(Time.deltaTime);
			isRebuild = true;
			OnScroll();
		}

		private void Update()
		{
			if(isSroll) AutoScroll();
		}

		public void ResetScroll()
		{
			isSroll = false;
		}

		//Slide to Game Item by Index + 1
		public void ScrollToGame(int i)
		{
			isSroll = true;
			//Calculate position of GameItem relative to MainScrollContentPanelHeight
			destinationPosY =(Mathf.CeilToInt(i / col) * (itemHeight + spacing));
			//Limit the position of the MainScrollContentPanelHeight from leaving the Main Panel
			destinationPosY = Mathf.Clamp(destinationPosY, 0, contentPanelHeight - rectTransform.sizeDelta.y);
		}

		//Slide to RectTransform
		private void ScrollToObject(RectTransform rect)
		{
			isSroll = true;
			//Calculate position of RectTransform relative to MainScrollContentPanelHeight
			destinationPosY = rect.anchoredPosition.y - contentPanelHeight;
			//Limit the position of the MainScrollContentPanelHeight from leaving the Main Panel
			destinationPosY = Mathf.Clamp(destinationPosY, 0, contentPanelHeight - rectTransform.sizeDelta.y);
		}

		//Slide to the destination position and stop when it has reached
		private void AutoScroll()
		{
			//Auto scroll

			//if (destinationPosY == Mathf.Infinity) return;
			//float tempY = Mathf.Lerp(mainScrollContentPanel.anchoredPosition.y, destinationPosY, Time.deltaTime * autoScrollSpeed);
			//mainScrollContentPanel.anchoredPosition = new Vector2(mainScrollContentPanel.anchoredPosition.x, tempY);
			//if (Mathf.Abs(mainScrollContentPanel.anchoredPosition.y - destinationPosY) < 2f)
			//{
			//	mainScrollContentPanel.anchoredPosition = new Vector2(mainScrollContentPanel.anchoredPosition.x, destinationPosY);
			//	destinationPosY = Mathf.Infinity;
			//	isSroll = false;
			//}

			mainScrollContentPanel.anchoredPosition = new Vector2(mainScrollContentPanel.anchoredPosition.x, destinationPosY);
			isSroll = false;
		}

		//Event occurs when scroll
		public void OnScroll()
		{
			if (!isRebuild) return;
			//Get and limit the output of the current position of the mainScrollContentPanel
			float startViewY = Mathf.Clamp(mainScrollContentPanel.anchoredPosition.y, 0, contentPanelHeight);
			//Get the index of each displayed row based on the number of columns
			// (panel height /item height) * number of columns
			int startIndex = (int)(startViewY / (itemHeight + spacing)) * col;
			//returns item not in range
			int[] keys = new int[items.Keys.Count];
			items.Keys.CopyTo(keys, 0);
			foreach (int index in keys)
			{
				if (index < startIndex || index > startIndex + totalDisplayItems - 1)
				{
					SimplePool.Despawn(items[index].gameObject);
					items.Remove(index);
				}
			}

			//spawn new items put into position displayed on the screen
			for (int i = startIndex; i < startIndex + totalDisplayItems - 1 && i < listItems.Count; i++)
			{
				if (items.ContainsKey(i))
				{
					continue;
				}
				SetGameItem(i);
			}
		}

		public void ClearAllItem()
		{
			//Remove all game item when change
			int[] keys = new int[items.Keys.Count];
			items.Keys.CopyTo(keys, 0);
			foreach (int index in keys)
			{
				SimplePool.Despawn(items[index].gameObject);
				items.Remove(index);
			}
		}

		public void PreloadAndSetItemInfo()
		{
			//Make available items to display in Pool
			SimplePool.Preload(gameItemPrefab, totalDisplayItems);
			GameObject[] obj = new GameObject[totalDisplayItems];
			for (int i = 0; i < totalDisplayItems; ++i)
			{
				obj[i] = SimplePool.Spawn(gameItemPrefab, Vector3.zero, Quaternion.identity);
				GameItemBtn gameItemBtn = obj[i].GetComponent<GameItemBtn>();
				gameItemBtn.OnCLick = OnItemClick;
				gameItemBtn.OnToggleChange = OnFavoriteChange;
				gameItemBtn.SetTheme(theme);
			}

			// Now despawn them all.
			for (var i = 0; i < totalDisplayItems; ++i)
			{
				SimplePool.Despawn(obj[i]);
			}
		}

		//function to calculate initial parameters used to display items on Main Panel
		private void CalculatorItemOnScreen()
		{
			//Get the height and width of the item + spacing
			Vector2 size = gameItemPrefab.GetComponent<RectTransform>().sizeDelta;

			// isWideScreen is 3 colum , else 2 col
			col = isWideScreen ? 3 : 2;

			// scaler of item
			itemWidth = (parentPanel.rect.width - (spacing * (col - 1)) - paddingLeftRight * 2f) / col ;
			itemScaler = itemWidth / size.x ;
			itemHeight = size.y * itemScaler;

			//Get Main Panel size when scaled to screen size
			mainPanelHeight = Screen.height;

			//Maximum number of rows that can be displayed
			// number of complete items displayed on MainPanel + 2 items from the top and bottom of the main panel)
			row = (int)(mainPanelHeight / (itemHeight + spacing) + 4);
			//Maximum number of items displayed 
			//number of rows * number of columns
			totalDisplayItems = row * col;
			//The size of the ContentPanel will be based on the number of rows of items displayed + padding top + padding bottom
			contentPanelHeight = paddingTop + Mathf.CeilToInt((float)listItems.Count / col) * (itemHeight + spacing) - spacing + paddingBottom;
			mainScrollContentPanel.sizeDelta = new Vector2(mainScrollContentPanel.sizeDelta.x, contentPanelHeight);

			//Make the screen display to the top
			mainScrollContentPanel.anchoredPosition = new Vector2(0f, -contentPanelHeight / 2f);

			PreloadAndSetItemInfo();
		}

		//Setting and displaying the Item on the screen
		private void SetGameItem(int i)
		{
			GameObject gameObject = SimplePool.Spawn(gameItemPrefab, mainScrollContentPanel.transform.position, Quaternion.identity);
			GameItemBtn gameBtn = gameObject.GetComponent<GameItemBtn>();
			//Calculate the display position of the item
			float spacingColTemp = i % col == 0 ? 0 : spacing * ( i % col );
			float spacingRowTemp = spacing * (i / col);
			//float posX = paddingLeftRight + itemWidth / 2f + itemWidth * (i % col) + spacingColTemp;
			float posX = -(itemWidth * col + spacing *(col-1))/2 + itemWidth / 2f + itemWidth * (i % col) + spacingColTemp;
			float posY = paddingTop + itemHeight/2f + (i / col * itemHeight) + spacingRowTemp;
			gameBtn.SetParentAndPosition(mainScrollContentPanel.transform, new Vector2(posX, - posY));
			gameBtn.SetGameItem(listItems[i]);
			gameBtn.SetScaler(itemScaler);
			items.Add(i, gameBtn);
		}
		public void SetTheme(int theme)
		{
			this.theme = theme;
		}
	}
}

