using UnityEngine;
using UnityEngine.UI;

namespace TsumTsumu
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Text scoreText;

        public void SetScore(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"SCORE: {score}";
            }
        }
    }
}
