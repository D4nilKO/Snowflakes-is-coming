using Project.LevelSystem.LevelStructure;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.GameState.View
{
    public class LevelInfoView : MonoBehaviour
    {
        [SerializeField, Header("Данные для ознакомления")]
        private LevelData _currentLevelData;

        [ShowInInspector]
        private int TimeToWin => _timeToWin;

        private int _timeToWin;

        public void SetLevelInfo(LevelData levelData)
        {
            _currentLevelData = levelData;
            _timeToWin = _currentLevelData?.GetTimeToSurvive() ?? 0;
        }
    }
}