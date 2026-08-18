using System.Collections.Generic;
using UnityEngine;

namespace TsumTsumu
{
    public class GameManager : MonoBehaviour
    {
        private GameObject ballPrefab;
        private LineRenderer lineRenderer;
        private UIManager uiManager;

        private int initialBallCount = 30;
        private float maxConnectDistance = 1.5f;
        private float spawnHeight = 4.0f;

        private readonly List<TsumBall> connectedBalls = new List<TsumBall>();
        private BallType currentType;
        private bool isDragging;
        private int score;
        private Camera mainCamera;

        private void Awake()
        {
            EnsureCamera();
            EnsureWalls();
            EnsureLineRenderer();
            EnsureUI();
            CreateBallPrefab();
        }

        private void Start()
        {
            mainCamera = Camera.main;
            SpawnInitialBalls();
            UpdateUI();
            Invoke(nameof(TakeScreenshot), 1.5f);
        }

        private void TakeScreenshot()
        {
            ScreenCapture.CaptureScreenshot("unity_screenshot.png");
            Debug.Log("Saved unity_screenshot.png");
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        private void EnsureCamera()
        {
            if (Camera.main == null)
            {
                GameObject camObj = new GameObject("Main Camera");
                camObj.tag = "MainCamera";
                Camera cam = camObj.AddComponent<Camera>();
                cam.orthographic = true;
                cam.orthographicSize = 5.0f;
                camObj.transform.position = new Vector3(0, 0, -10);
            }
        }

        private void EnsureWalls()
        {
            // 下壁
            CreateWall("BottomWall", new Vector2(0, -5), new Vector2(10, 1));
            // 左壁
            CreateWall("LeftWall", new Vector2(-3, 0), new Vector2(1, 10));
            // 右壁
            CreateWall("RightWall", new Vector2(3, 0), new Vector2(1, 10));
        }

        private void CreateWall(string name, Vector2 pos, Vector2 size)
        {
            GameObject wall = new GameObject(name);
            wall.transform.position = pos;
            BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
            col.size = size;
        }

        private void EnsureLineRenderer()
        {
            GameObject lineObj = new GameObject("LineRenderer");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.15f;
            lineRenderer.endWidth = 0.15f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
            lineRenderer.positionCount = 0;
        }

        private void EnsureUI()
        {
            GameObject uiObj = new GameObject("UIManager");
            uiManager = uiObj.AddComponent<UIManager>();
        }

        private void CreateBallPrefab()
        {
            ballPrefab = new GameObject("BallTemplate");
            ballPrefab.SetActive(false);

            Rigidbody2D rb = ballPrefab.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1.0f;

            CircleCollider2D col = ballPrefab.AddComponent<CircleCollider2D>();
            col.radius = 0.4f;

            SpriteRenderer sr = ballPrefab.AddComponent<SpriteRenderer>();
            sr.sprite = CreateCircleSprite();

            ballPrefab.AddComponent<TsumBall>();
        }

        private Sprite CreateCircleSprite()
        {
            int texSize = 64;
            Texture2D tex = new Texture2D(texSize, texSize);
            float radius = texSize / 2.0f;
            Vector2 center = new Vector2(radius, radius);

            for (int y = 0; y < texSize; y++)
            {
                for (int x = 0; x < texSize; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), center);
                    if (dist <= radius)
                    {
                        tex.SetPixel(x, y, Color.white);
                    }
                    else
                    {
                        tex.SetPixel(x, y, Color.clear);
                    }
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, texSize, texSize), new Vector2(0.5f, 0.5f), 64.0f);
        }

        private void Update()
        {
            HandleInput();
        }

        private void SpawnInitialBalls()
        {
            for (int i = 0; i < initialBallCount; i++)
            {
                Vector2 spawnPos = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-2.0f, spawnHeight));
                SpawnSingleBall(spawnPos);
            }
        }

        private TsumBall SpawnSingleBall(Vector2 position)
        {
            GameObject obj = Instantiate(ballPrefab, position, Quaternion.identity);
            obj.SetActive(true);
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
                    Vector2 spawnPos = new Vector2(Random.Range(-1.5f, 1.5f), spawnHeight);
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
