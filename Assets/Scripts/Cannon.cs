using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Cannon controlls (rotation, fire, ball spawn)
public class Cannon : MonoBehaviour
{
	[SerializeField] Button fireButton;
    [SerializeField] float maxAngle = 45;
    [SerializeField] Transform ballSpawnPosition;
    [SerializeField] PhysicsMaterial2D ballPhysicsMaterial;
	[SerializeField] List<Ball> ballsPrefabs;

    Ball activeBall;
    bool gamePaused;
    bool touchOverUi;

    // Rotate cannon to cursor/touch position
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

    void CheckTouchOverUi()
	{
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Ended) { return; }
            touchOverUi = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
        }
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

    void Fire()
	{
        if (activeBall == null ||
            gamePaused == true) { return; }

        activeBall.transform.SetParent(null);
        activeBall.Fire(transform.up);
        activeBall = null;
    }

    // Unity
    private void Start()
	{
        SpawnRandomBall();

        Ball.OnBallConnected += BallConnected;
        fireButton.onClick.AddListener(Fire);
        DemonstrationUi.OnPause += (value) => gamePaused = value;
    }

	private void Update()
	{
        CheckTouchOverUi();

        if (!gamePaused && !touchOverUi)
		{
            LookAtMouse();
        }
    }

    // Events
    void BallConnected()
	{
        SpawnRandomBall();
    }
}
