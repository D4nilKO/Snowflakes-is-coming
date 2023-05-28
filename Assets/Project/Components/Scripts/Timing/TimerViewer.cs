using TMPro;
using UnityEngine;

namespace Project.Components.Scripts
{
    public class TimerViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text enemyTimerText;
        [SerializeField] private TMP_Text mainTimerText;

        //private GameStateMachine gameStateMachine;

        private void Awake()
        {
            //gameStateMachine = FindObjectOfType<GameStateMachine>();
        }

        public void UpdateEnemyTimerText(float value)
        {
            enemyTimerText.text = $"{value:f2}";
            enemyTimerText.color = value - float.Epsilon < 0.1f ? Color.red : Color.white;

            if (value == 0f)
            {
                enemyTimerText.text = "";
            }
        }

        public void UpdateMainTimerText(string value)
        {
            mainTimerText.text = value;
        }
    }
}