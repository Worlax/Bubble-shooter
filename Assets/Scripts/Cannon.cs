using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
	[SerializeField] KeyCode fireButton = KeyCode.Mouse1;
    [SerializeField] float maxAngle = 45;
    [SerializeField] Transform ballSpawnPosition;
    [SerializeField] PhysicsMaterial2D ballPhysicsMaterial;
	[SerializeField] List<Ball> ballsPrefabs;

    [System.NonSerialized] public bool Pause = false;

    Ball activeBall;

    void LookAtMouse()
	{
        Vector3 mousePosition = Input.mousePosition;
        Vector3 cannonPosition = Camera.main.WorldToScreenPoint(transform.position);

        mousePosition.x = mousePosition.x - cannonPosition.x;
        mousePosition.y = mousePosition.y - cannonPosition.y;

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        angle -= 90;
        angle = Mathf.Clamp(angle, maxAngle * -1, maxAngle);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void SpawnRandomBall()
	{
        if (activeBall != null)
		{
            Destroy(activeBall.gameObject);
		}

        Ball prefab = ballsPrefabs[Random.Range(0, ballsPrefabs.Count)];
        activeBall = Instantiate(prefab, transform, true);
        activeBall.transform.position = ballSpawnPosition.position;

        Rigidbody2D rigidbody = activeBall.gameObject.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        rigidbody.sharedMaterial = ballPhysicsMaterial;
    }

    void FireOnPress()
	{
        if (activeBall == null ||
            DemonstrationUi.IsMouseOver) { return; }

        if (Input.GetKeyDown(fireButton))
		{
            activeBall.transform.SetParent(null);
            activeBall.Fire(transform.up);
            activeBall = null;
        }
    }

	// Unity
	private void Start()
	{
        SpawnRandomBall();
        Ball.OnBallConnected += BallConnected;
        DemonstrationUi.OnPause += (value) => Pause = value;
    }

	private void Update()
	{
        if (!Pause)
		{
            LookAtMouse();
            FireOnPress();
        }
    }

    // Events
    void BallConnected()
	{
        SpawnRandomBall();
    }
}
