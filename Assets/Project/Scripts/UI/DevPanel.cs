using Project.Data;
using Project.GameState;
using Project.LevelSystem;
using TMPro;
using UnityEngine;
using VContainer;

namespace Project.Services
{
    public class DevPanel : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _inputField;

        private GameOutcome _gameOutcome;
        private LevelInitializer _levelInitializer;
        private LevelLoader _levelLoader;
        private ProgressData _progressData;

        [Inject]
        private void Construct(GameOutcome gameOutcome, LevelInitializer levelInitializer, LevelLoader levelLoader,
            ProgressData progressData)
        {
            _gameOutcome = gameOutcome;
            _levelInitializer = levelInitializer;
            _levelLoader = levelLoader;
            _progressData = progressData;
        }

        private void Awake()
        {
            this.ValidateSerializedFields();
        }

        public void ClearSaves()
        {
            _progressData.SetStartValues();
        }

        public void UnlockAllLevels()
        {
            _progressData.SetUnlockedLevel(_progressData.MaxLevelsCount);
        }

        public void LoadEnteredLevel()
        {
            if (int.TryParse(_inputField.text, out int level) == false)
            {
                Debug.LogError("Неверное значение");
                return;
            }

            _progressData.SetUnlockedLevel(level);
            _progressData.SetCurrentLevel(level);

            _levelLoader.FetchCurrentLevelSettings();
            _levelInitializer.RestartLevel();
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