using UnityEngine;

using Extensions;

namespace HappyBirds
{
	public class Bot : MonoBehaviour
	{
		[SerializeField] private Ship ship;
		[SerializeField] private bool isBotActive = false;
		[SerializeField] private float botWinRate = 0f;
		[SerializeField] private float flyForce = 0f;
		[SerializeField] private float flyDistance = 0f;
		[SerializeField] private float flyDelay = 0f;
		[SerializeField] private float minY = 6f;
		[SerializeField] private Vector2 endPos;
		[SerializeField] private float timer = 0f;

		public bool IsStart { get; set; }

		private void Awake()
		{
			// Check for reference errors
			ship.ThrowIfNull();
		}

		public void BotInit(Ship ship , float botWinRate , float flyForce , float flyDistance , float delay)
		{
			this.ship = ship;
			this.botWinRate = botWinRate;
			this.flyForce = flyForce;
			this.flyDistance = flyDistance;
			this.flyDelay = delay;
			isBotActive = true;
		}

		public void SetEndPos(Vector2 endPos)
		{
			this.endPos = endPos;
			float posY = 0.5f * (100 - botWinRate);
			this.endPos.y = Mathf.Clamp(this.endPos.y, this.endPos.y - posY, this.endPos.y + posY);
			//this.endPos = Vector2.zero;
		}

		private void Update()
		{
			if (!isBotActive || !IsStart) return;
			timer += Time.deltaTime;

			if (ship.transform.position.y > minY)
			{
				ship.Fly();
				return;
			}

			if (timer < flyDelay) return;
			if (ship.transform.position.y  > endPos.y + flyForce * flyDelay)
			{
				ship.Fly();
				timer = 0;
			}
		}
	}
}


