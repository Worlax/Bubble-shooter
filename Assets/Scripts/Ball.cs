using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField] float speed = 300;
	[SerializeField] int ballsToMatch = 3;
	[SerializeField] float overlapArea = 0.7f;
	[field: SerializeField] public BallType ThisBallType { get; private set; }

	public static Ball FiredBall { get; private set; }
	static Vector2 pausedVelocity;

	public static event Action OnBallConnected;

	public enum BallType
	{
		Blue,
		Green,
		Orange
	}

	public void Fire(Vector2 direction)
	{
		FiredBall = this;
		Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
		rigidBody.AddForce(direction * speed);
	}

	// Breadth first search algorithm
	List<Ball> GetConnectedBalls()
	{
		List<Ball> searchResult = new List<Ball>();
		Queue<Ball> searchQueue = new Queue<Ball>();
		Ball current;
		List<Ball> ballsNearCurrent;

		searchResult.Add(this);
		searchQueue.Enqueue(this);

		while (searchQueue.Count != 0)
		{
			current = searchQueue.Dequeue();
			ballsNearCurrent = current.GetNearbyBalls();
			ballsNearCurrent = current.RemoveWrongType(ballsNearCurrent);
			ballsNearCurrent = current.RemoveDuplicates(ballsNearCurrent, searchResult);

			searchResult.AddRange(ballsNearCurrent);

			foreach (Ball ball in ballsNearCurrent)
			{
				searchQueue.Enqueue(ball);
			}
		}

		searchResult.Remove(this);
		return searchResult;
	}

	List<Ball> GetNearbyBalls()
	{
		Collider2D[] nearColliders = Physics2D.OverlapCircleAll(transform.position, overlapArea);
		List<Ball> nearBalls = new List<Ball>();

		foreach (Collider2D collider in nearColliders)
		{
			Ball ball = collider.GetComponent<Ball>();

			if (ball != null)
			{
				nearBalls.Add(ball);
			}
		}

		nearBalls.Remove(this);
		return nearBalls;
	}

	List<Ball> RemoveWrongType(IEnumerable<Ball> balls)
	{
		return balls.Where(x => x.ThisBallType == ThisBallType).ToList();
	}

	List<Ball> RemoveDuplicates(IEnumerable<Ball> ballsContent, IEnumerable<Ball> matchesToRemove)
	{
		return ballsContent.Where(x => !matchesToRemove.Contains(x)).ToList();
	}

	void CheckForMatch()
	{
		List<Ball> connectedBalls = GetConnectedBalls();
		if (connectedBalls.Count >= ballsToMatch - 1)
		{
			connectedBalls.ForEach(x => Destroy(x.gameObject));
			Destroy(gameObject);
		}
	}

	// Unity
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Ball otherBall = collision.collider.GetComponent<Ball>();
		if (otherBall == null) { return; }

		if (FiredBall == this)
		{
			FiredBall = null;
			BallSpawner.Instance.ConnectBallToTarget(this, otherBall);
			OnBallConnected?.Invoke();
			CheckForMatch();
		}
	}

	// Events
	static void Pause(bool value)
	{
		if (FiredBall == null) { return; }
		Rigidbody2D rigidBody = FiredBall.GetComponent<Rigidbody2D>();

		if (value)
		{
			pausedVelocity = rigidBody.velocity;
			rigidBody.velocity = Vector2.zero;
		}
		else
		{
			rigidBody.velocity = pausedVelocity;
			pausedVelocity = Vector2.zero;
		}
	}

	// Constructor
	static Ball()
	{
		DemonstrationUi.OnPause += Pause;
	}
}
