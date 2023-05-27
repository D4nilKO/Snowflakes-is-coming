using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Project.Components.Scripts
{
    public class TimerViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text enemyTimerText;
        [SerializeField] private TMP_Text mainTimerText;

        private GameStateMachine gameStateMachine;

        private void Awake()
        {
            gameStateMachine = FindObjectOfType<GameStateMachine>();
        }

        public void UpdateEnemyTimerText(float value)
        {
            enemyTimerText.text = $"{value}";
        }

        public void UpdateMainTimerText(string value)
        {
            mainTimerText.text = value;
        }
    }
}