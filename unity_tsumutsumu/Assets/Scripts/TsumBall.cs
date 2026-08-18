using UnityEngine;

namespace TsumTsumu
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class TsumBall : MonoBehaviour
    {
        [SerializeField] private BallType type;
        private SpriteRenderer spriteRenderer;

        public BallType Type => type;
        public bool IsConnected { get; private set; }

        private static readonly Color RedColor = new Color(0.95f, 0.25f, 0.25f);
        private static readonly Color BlueColor = new Color(0.25f, 0.55f, 0.95f);
        private static readonly Color YellowColor = new Color(0.95f, 0.85f, 0.25f);

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }
            spriteRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        public void Setup(BallType ballType)
        {
            type = ballType;
            UpdateVisuals();
        }

        public void SetHighlight(bool highlight)
        {
            IsConnected = highlight;
        }

        private void UpdateVisuals()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
            if (spriteRenderer == null) return;

            switch (type)
            {
                case BallType.Red:
                    spriteRenderer.color = RedColor;
                    break;
                case BallType.Blue:
                    spriteRenderer.color = BlueColor;
                    break;
                case BallType.Yellow:
                    spriteRenderer.color = YellowColor;
                    break;
            }
        }
    }
}
