using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Extensions;
using DG.Tweening;

namespace HappyBirds
{
	public class Environment : MonoBehaviour
	{
		[SerializeField] private Transform des;
		[SerializeField] private Transform boundaryTopRight;
		[SerializeField] private Transform boundaryBotLeft;
		[SerializeField] private Transform leftStone;
		[SerializeField] private Transform rightStone;
		[SerializeField] private Transform topStone;
		[SerializeField] private Transform bottomStone;
		[SerializeField] private Collider2D redDogColl;
		[SerializeField] private Collider2D blueCatColl;
		[SerializeField] private float offset;
		[SerializeField] private List<Transform> leftThorns;
		[SerializeField] private List<Transform> rightThorns;
		[SerializeField] private Animator animator;
		[SerializeField] private float changeTime = 1.5f;
		[SerializeField] private float stonePosY = 1.6f;
		[SerializeField] private float stonePrePosY = 2f;

		private const string QuakeAnim = "Quake";
		private HashSet<int> set = new HashSet<int>();
		private int minThorns = 1;
		private float botWinRate;
		private int maxThorns = 3;
		private int flyFree;
		private Vector3 lastThorn ;
		private bool isFirst = true;

		public UnityAction OnStoneChange;

		private void Awake()
		{
			boundaryTopRight.ThrowIfNull();
			boundaryBotLeft.ThrowIfNull();
			redDogColl.ThrowIfNull();
			blueCatColl.ThrowIfNull();
			leftStone.ThrowIfNull();
			rightStone.ThrowIfNull();
			topStone.ThrowIfNull();
			bottomStone.ThrowIfNull();
		}

		// Start is called before the first frame update
		void Start()
		{
			SetupBoundary();
			IgnoreBirdCollision();
		}

		public void Initialized(int min, int max, float changeTime, float stonePosY, float botWinRate)
		{
			this.minThorns = min;
			this.maxThorns = max;
			this.changeTime = changeTime;
			this.stonePosY = stonePosY;
			this.botWinRate = botWinRate;
		}

		public void OnInit()
		{
			isFirst = true;
			flyFree = Mathf.CeilToInt(botWinRate/25f);
			lastThorn = Vector3.zero;
			HideStone();
			StartCoroutine(ChangeStones());
		}

		public void HideStone()
		{
			for (int i = 0; i < leftThorns.Count; i++)
			{
				leftThorns[i].transform.localPosition = new Vector2(leftThorns[i].transform.localPosition.x, stonePosY);
				rightThorns[i].transform.localPosition = new Vector2(rightThorns[i].transform.localPosition.x, stonePosY);
			}
		}

		public IEnumerator ChangeStones()
		{
			int rdThorn = Random.Range(minThorns,maxThorns+1);
			for (int i = 0; i < leftThorns.Count; i++)
			{
				leftThorns[i].transform.DOLocalMoveY(stonePosY - stonePrePosY, changeTime * 0.5f);
				rightThorns[i].transform.DOLocalMoveY(stonePosY - stonePrePosY, changeTime * 0.5f);
			}
			set.Clear();
			while (set.Count != rdThorn)
			{
				int rd = Random.Range(0, leftThorns.Count);
				if (isFirst && rd == 4) continue;
				set.Add(rd);
			}
			OnStoneChange?.Invoke();

			yield return new WaitForSeconds(0.5f);
			for (int i=0; i< leftThorns.Count; i++)
			{
				leftThorns[i].gameObject.SetActive(false);
				rightThorns[i].gameObject.SetActive(false);
				if (set.Contains(i))
				{
					leftThorns[i].gameObject.SetActive(true);
					leftThorns[i].localPosition = new Vector3(leftThorns[i].localPosition.x, stonePosY - stonePrePosY, leftThorns[i].localPosition.x);
					int id = i;
					leftThorns[i].transform.DOLocalMoveY(stonePosY - stonePrePosY * 0.3f , changeTime * 0.7f).OnComplete(() => {
						animator.ResetTrigger(QuakeAnim);
						animator.SetTrigger(QuakeAnim);
						leftThorns[id].transform.DOLocalMoveY(stonePosY, changeTime * 0.3f);
					});

					rightThorns[i].gameObject.SetActive(true);
					rightThorns[i].localPosition = new Vector3(rightThorns[i].localPosition.x, stonePosY - stonePrePosY, rightThorns[i].localPosition.x);
					rightThorns[i].transform.DOLocalMoveY(stonePosY - stonePrePosY * 0.3f, changeTime * 0.7f).OnComplete(() => {
						rightThorns[id].transform.DOLocalMoveY(stonePosY, changeTime * 0.3f);
					}); ;
				}
			}
		}

		public Vector2 RandomMissingStone(bool left)
		{
			List<Transform> temp = new List<Transform>();
			for (int i = 1 ; i < leftThorns.Count - 1; i++)
			{
				if (!set.Contains(i))
				{
					temp.Add(left ? leftThorns[i] : rightThorns[i]);
				}
			}

			Vector3 rd;
			if (isFirst)
			{
				rd = left ? leftThorns[4].position : rightThorns[4].position;
				flyFree--;
			}
			else if (Random.Range(0f,100f) < botWinRate || flyFree > 0)
			{
				rd = temp[Random.Range(0, temp.Count)].position;
				flyFree--;
			}
			else
			{
				int rdIndex = Random.Range(0, rightThorns.Count);
				rd = left ? leftThorns[rdIndex].position : rightThorns[rdIndex].position;
			}
			
			lastThorn = rd;
			des.position = rd;
			isFirst = false;
			return lastThorn;
		}

		public void SetupBoundary()
		{
			Vector3 point;
			Vector3 offsetVector = new Vector3(offset/2,offset/2,0);

			point = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane)) + offsetVector;
			boundaryTopRight.position = point;

			point = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)) - offsetVector;
			boundaryBotLeft.position = point;

			leftStone.position = new Vector2(boundaryBotLeft.position.x, leftStone.position.y);
			rightStone.position = new Vector2(boundaryTopRight.position.x, rightStone.position.y);
			topStone.position = new Vector2(topStone.position.x, boundaryTopRight.position.y);
			bottomStone.position = new Vector2(bottomStone.position.x, boundaryBotLeft.position.y);
		}

		public void IgnoreBirdCollision()
		{
			Physics2D.IgnoreCollision(redDogColl,blueCatColl,true);
		}

		private void OnDisable()
		{
			transform.DOKill();
			for (int i = 0; i < leftThorns.Count; i++)
			{
				leftThorns[i].transform.DOKill();
				rightThorns[i].transform.DOKill();
			}
		}
	}
}
