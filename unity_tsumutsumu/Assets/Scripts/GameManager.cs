using System.Collections.Generic;
using UnityEngine;

namespace TsumTsumu
{
    public class GameManager : MonoBehaviour
    {
        [Header("Prefabs & References")]
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private UIManager uiManager;

        [Header("Settings")]
        [SerializeField] private int initialBallCount = 30;
        [SerializeField] private float maxConnectDistance = 1.5f;
        [SerializeField] private float spawnHeight = 4.0f;

        private readonly List<TsumBall> connectedBalls = new List<TsumBall>();
        private BallType currentType;
        private bool isDragging;
        private int score;
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
            SpawnInitialBalls();
            UpdateUI();
        }

        private void Update()
        {
            HandleInput();
        }

        private void SpawnInitialBalls()
        {
            if (ballPrefab == null) return;
            for (int i = 0; i < initialBallCount; i++)
            {
                Vector2 spawnPos = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(1.0f, spawnHeight));
                SpawnSingleBall(spawnPos);
            }
        }

        private TsumBall SpawnSingleBall(Vector2 position)
        {
            GameObject obj = Instantiate(ballPrefab, position, Quaternion.identity);
            TsumBall ball = obj.GetComponent<TsumBall>();
            if (ball != null)
            {
                BallType randomType = (BallType)Random.Range(0, 3);
                ball.Setup(randomType);
            }
            return ball;
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                TsumBall ball = GetBallAtPosition(mousePos);
                if (ball != null)
                {
                    isDragging = true;
                    connectedBalls.Clear();
                    currentType = ball.Type;
                    ConnectBall(ball);
                }
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                TsumBall ball = GetBallAtPosition(mousePos);
                if (ball != null && ball.Type == currentType && !ball.IsConnected)
                {
                    TsumBall lastBall = connectedBalls[connectedBalls.Count - 1];
                    if (Vector2.Distance(lastBall.transform.position, ball.transform.position) <= maxConnectDistance)
                    {
                        ConnectBall(ball);
                    }
                }
                UpdateLine(mousePos);
            }
            else if (Input.GetMouseButtonUp(0) && isDragging)
            {
                EndDrag();
            }
        }

        private void ConnectBall(TsumBall ball)
        {
            connectedBalls.Add(ball);
            ball.SetHighlight(true);
        }

        private void UpdateLine(Vector2 currentMousePos)
        {
            if (lineRenderer == null) return;
            lineRenderer.positionCount = connectedBalls.Count + 1;
            for (int i = 0; i < connectedBalls.Count; i++)
            {
                lineRenderer.SetPosition(i, connectedBalls[i].transform.position);
            }
            lineRenderer.SetPosition(connectedBalls.Count, currentMousePos);
        }

        private void EndDrag()
        {
            isDragging = false;
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 0;
            }

            int count = connectedBalls.Count;
            if (count >= 3)
            {
                foreach (TsumBall ball in connectedBalls)
                {
                    Destroy(ball.gameObject);
                }
                score += count * 100;
                UpdateUI();

                for (int i = 0; i < count; i++)
                {
                    Vector2 spawnPos = new Vector2(Random.Range(-2.0f, 2.0f), spawnHeight);
                    SpawnSingleBall(spawnPos);
                }
            }
            else
            {
                foreach (TsumBall ball in connectedBalls)
                {
                    ball.SetHighlight(false);
                }
            }
            connectedBalls.Clear();
        }

        private TsumBall GetBallAtPosition(Vector2 pos)
        {
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null)
            {
                return hit.collider.GetComponent<TsumBall>();
            }
            return null;
        }

        private void UpdateUI()
        {
            if (uiManager != null)
            {
                uiManager.SetScore(score);
            }
        }
    }
}
