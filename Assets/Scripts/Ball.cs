using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField] float speed = 300;
	[SerializeField] float overlapArea = 1;
	[SerializeField] int ballsToMatch = 3;
	[field: SerializeField] public BallType ThisBallType { get; private set; }

	bool firedBall;

	public enum BallType
	{
		Blue,
		Green,
		Orange
	}

	public void Fire(Vector2 direction)
	{
		firedBall = true;
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

	// Unity
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// This executes only on static balls
		// And if type is matched - deletes all connected ballss


		Ball otherBall = collision.collider.GetComponent<Ball>();
		// test
		if (otherBall != null && firedBall)
		{
			firedBall = false;
			BallSpawner.Instance.ConnectBallToTarget(this, otherBall);
		}

		return;
		// end test
		if (otherBall == null || firedBall) { return; }

		if (otherBall.ThisBallType == ThisBallType)
		{
			GetConnectedBalls().ForEach(x => Destroy(x.gameObject));
			Destroy(gameObject);
		}
	}
}
