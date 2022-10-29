using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : Singleton<BallSpawner>
{
	[SerializeField] int width;
	[SerializeField] int height;
	[SerializeField] float horizontalSpacing;
	[SerializeField] float verticalSpacing;
	[SerializeField] [Range(1, 100)] int density;
	[SerializeField] Transform content;
	[SerializeField] List<Ball> ballPrefabs = new List<Ball>();

	// Setters
	public void SetHeight(int value)
	{
		height = Mathf.Clamp(value, 1, 14);
	}

	public void SetDebsity(int value)
	{
		density = Mathf.Clamp(value, 1, 100);
	}
	// ...

	public void RespawnLevel()
	{
		foreach (Transform tr in content)
		{
			Destroy(tr.gameObject);
		}
		SpawnLevel();
	}

	public void ConnectBallToTarget(Ball ball, Ball target)
	{
		ball.transform.rotation = Quaternion.identity;
		Rigidbody2D rigitBody = ball.GetComponent<Rigidbody2D>();
		if (rigitBody != null)
		{
			Destroy(rigitBody);
		}

		List<Vector2> possibleSlots = GetPossibleSlotsToConnect(ball, target);
		Vector2 closestSlot = GetClosestSlot(ball.transform.position, possibleSlots);
		ball.transform.position = closestSlot;
		ball.transform.SetParent(content);
	}

	void SpawnLevel()
	{
		Vector2 startPosition = transform.position;
		Vector2 currentPosition;

		for (int row = 0; row < height; ++row)
		{
			for (int column = 0; column < width; ++column)
			{
				// Every second row will have 1 less ball
				// due to the offset
				if (row % 2 != 0 && column == width - 1) { break; }

				currentPosition = GetSpawnPosition(startPosition, column, row);
				TryToSpawnBall(currentPosition);
			}
		}
	}

	List<Vector2> GetPossibleSlotsToConnect(Ball ball, Ball target)
	{
		List<Vector2> slots = new List<Vector2>();

		slots.Add(target.transform.position + new Vector3(horizontalSpacing, 0));
		slots.Add(target.transform.position - new Vector3(horizontalSpacing, 0));
		slots.Add(target.transform.position + new Vector3(-horizontalSpacing / 2, verticalSpacing));
		slots.Add(target.transform.position + new Vector3(horizontalSpacing / 2, verticalSpacing));
		slots.Add(target.transform.position - new Vector3(-horizontalSpacing / 2, verticalSpacing));
		slots.Add(target.transform.position - new Vector3(horizontalSpacing / 2, verticalSpacing));

		slots = RemoveSlotsWithBalls(slots, ball);

		return slots;
	}

	List<Vector2> RemoveSlotsWithBalls(List<Vector2> slots, Ball ignoreThisTarget)
	{
		foreach (Vector2 slot in slots.ToList())
		{
			Collider2D[] colliders = Physics2D.OverlapPointAll(slot);
			foreach (Collider2D collider in colliders)
			{
				Ball ball = collider.GetComponent<Ball>();
				if (ball != null && ball != ignoreThisTarget)
				{
					slots.Remove(slot);
				}
			}
		}

		return slots;
	}

	Vector2 GetClosestSlot(Vector2 ball, List<Vector2> slots)
	{
		slots = slots.OrderBy(x => Vector2.Distance(ball, x)).ToList();
		return slots[0];
	}

	Vector2 GetSpawnPosition(Vector2 startPosition, int column, int row)
	{
		float x = startPosition.x + horizontalSpacing * column;
		float y = startPosition.y - verticalSpacing * row;

		// Every second row will be spaced a bit to the right
		if (row % 2 != 0)
		{
			x += horizontalSpacing / 2;
		}

		return new Vector2(x, y);
	}

	Ball TryToSpawnBall(Vector2 position)
	{
		// Dont spawn if not lucky enough
		// (luck depends on density)
		bool shouldSpawn = Random.Range(1, 101) <= density;
		if (!shouldSpawn) { return null; }

		// Spawn random ball from prefab
		int prefabIndex = Random.Range(0, ballPrefabs.Count);
		Ball ball = Instantiate(ballPrefabs[prefabIndex], content);
		ball.transform.position = position;

		return ball;
	}
}
