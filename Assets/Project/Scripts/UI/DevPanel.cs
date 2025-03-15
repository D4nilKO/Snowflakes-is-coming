using Project.Data;
using Project.Data.Collections;
using Project.Entities.Character;
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
        private CharacterCollisionHandler _characterCollisionHandler;
        private CollectionManager _collectionManager;

        [Inject]
        private void Construct(GameOutcome gameOutcome, LevelInitializer levelInitializer, LevelLoader levelLoader,
            ProgressData progressData, CharacterCollisionHandler characterCollisionHandler, CollectionManager collectionManager)
        {
            _gameOutcome = gameOutcome;
            _levelInitializer = levelInitializer;
            _levelLoader = levelLoader;
            _progressData = progressData;
            _characterCollisionHandler = characterCollisionHandler;
            _collectionManager = collectionManager;
        }

        private void Awake()
        {
            this.ValidateSerializedFields();
        }

        public void ClearSaves()
        {
            _progressData.SetStartValues();
        }

        public void UnlockAll()
        {
            _progressData.SetUnlockedLevel(_progressData.MaxLevelsCount);
            _collectionManager.UnlockAllAbilities();
        }

        public void LoadEnteredLevel()
        {
            if (int.TryParse(_inputField.text, out int level) == false)
            {
                Debug.LogError("Неверное значение");
                return;
            }

            if (level > _progressData.MaxLevelsCount)
            {
                Debug.LogError($"Уровень {level} не существует, максимальный уровень {_progressData.MaxLevelsCount}");
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

        public void LoseLevel()
        {
            _gameOutcome.LostGame();
        }

        public void ImmortalMode()
        {
            _characterCollisionHandler.ActivateInvincibility();
        }

        public void UnImmortalMode()
        {
            _characterCollisionHandler.DeactivateInvincibility();
        }
    }
}