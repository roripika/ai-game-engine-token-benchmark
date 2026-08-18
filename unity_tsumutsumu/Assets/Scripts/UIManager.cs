using UnityEngine;

namespace TsumTsumu
{
    public class UIManager : MonoBehaviour
    {
        private int currentScore = 0;

        public void SetScore(int score)
        {
            currentScore = score;
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 40;
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(30, 40, 400, 100), $"SCORE: {currentScore}", style);
        }
    }
}
