using System;

using UnityEngine;

using Extensions;

namespace HappyBirds
{
	public class Ship : MonoBehaviour
	{
		[SerializeField] private Transform spawner;
		[SerializeField] private GameObject normalShip;
		[SerializeField] private GameObject brokenShip;
		[SerializeField] private Rigidbody2D rb;
		[SerializeField] private Animator animator;
		[SerializeField] private bool isDogSize;
		[SerializeField] private float gravity;
		[SerializeField] private float flyForce;
		[SerializeField] private float flyDistance;

		private const string boundaryTag = "Barrier";
		private const string stoneTag = "Stone";
		private const string idleAnim = "Idle";
		private const string dieAnim = "Die";
		private string currentAnim;
		private Vector2 flyVector;
		private bool isLeft;
		private bool isDead;
		private bool isFly;
		private bool isStop = true;

		public bool IsLeft => isLeft;
		private Action OnHitWall;
		private Action<GameSound,bool> OnPlaySound;
		private Action<Ship> OnDead;

		private void Awake()
		{
			spawner.ThrowIfNull();
			brokenShip.ThrowIfNull();
			normalShip.ThrowIfNull();
			rb.ThrowIfNull();
			animator.ThrowIfNull();
		}

		public void Initialize(float gravity , float flyForce , float flyDistance, Action onHitWall , Action<Ship> onDead , Action<GameSound, bool> onPlaySound)
		{
			this.gravity  = gravity;
			this.flyForce = flyForce;
			this.flyDistance = flyDistance;
			OnHitWall = onHitWall;
			OnDead = onDead;
			OnPlaySound = onPlaySound;
			OnInit();
		}

		public void OnInit()
		{
			transform.position = spawner.position;
			transform.rotation = spawner.rotation;
			ChangeAnim(idleAnim);
			isDead = false;
			isStop = true;
			flyVector = isDogSize ? Vector2.up : Vector2.down;
			rb.velocity = Vector2.zero;
			rb.gravityScale = 0;
			isLeft = !isDogSize;
		}

		public void OnStart()
		{
			isStop = false;
			rb.gravityScale = isDogSize ? this.gravity : -this.gravity;
		}

		public void Fly()
		{
			if (isDead || isStop) return;
			OnPlaySound?.Invoke(GameSound.Jump,false);
			rb.velocity = (flyVector + (isLeft ? Vector2.left : Vector2.right)) * flyForce;
		}

		public void Stop()
		{
			isDead = true;
			rb.gravityScale = 0f;
			rb.velocity = Vector2.zero;
		}

		private void Update()
		{
			if (isDead || isStop) return;
			isFly = rb.velocity.y > 0 && !isDogSize || rb.velocity.y < 0 && isDogSize;
			rb.velocity = new Vector2( isLeft ? -flyDistance : flyDistance, rb.velocity.y );
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (isDead) return;
			if (collision.transform.name.Contains(boundaryTag))
			{
				isLeft = !isLeft;
				transform.rotation = Quaternion.Euler(new Vector3( 0f , isLeft ? 180f : 0f , 0f ));
				if (!isDogSize) OnHitWall?.Invoke();
				OnPlaySound?.Invoke(GameSound.Wall,false);
			}
			else if (collision.transform.name.Contains(stoneTag))
			{
				isDead = true;
				ChangeAnim(dieAnim);
				OnPlaySound?.Invoke(GameSound.Crash,true);
				OnDead.Invoke(this);
			}
		}

		public void ChangeAnim(string anim)
		{
			if (anim != null && anim.Equals(currentAnim)) return;
			animator.ResetTrigger(anim);
			currentAnim = anim;
			animator.SetTrigger(currentAnim);
		}
	}
}

