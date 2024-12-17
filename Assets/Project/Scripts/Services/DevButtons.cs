using Project.Data;
using Project.GameState;
using Project.LevelSystem;
using TMPro;
using UnityEngine;

namespace Project.Services
{
    public class DevButtons : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _inputField;

        [SerializeField]
        private GameOutcome _gameOutcome;

        [SerializeField]
        private LevelLoader _levelLoader;

        [SerializeField]
        private Game _game;

        private void Awake()
        {
            this.ValidateSerializedFields();
        }

        public void ClearSaves()
        {
            ProgressData.SetStartValues();
        }

        public void UnlockAllLevels()
        {
            ProgressData.SetUnlockedLevel(ProgressData.MaxLevelsCount);
        }

        public void LoadEnteredLevel()
        {
            if (int.TryParse(_inputField.text, out int level) == false)
            {
                Debug.LogError("Неверное значение");
                return;
            }

            ProgressData.SetUnlockedLevel(level);
            ProgressData.SetCurrentLevel(level);

            _game.FetchCurrentLevelSettings();
            _levelLoader.RestartLevel();
        }

        public void WonLevel()
        {
            _gameOutcome.WonGame();
        }

        public void LostLevel()
        {
            _gameOutcome.LostGame();
        }
    }
}