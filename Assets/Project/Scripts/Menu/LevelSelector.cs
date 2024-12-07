using System.Collections.Generic;
using Project.Data;
using Project.LevelSystem;
using Project.LevelSystem.LevelStructure;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Project.Menu
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField, Header("Dependencies")]
        private JsonLevelParser _jsonLevelParser;

        [FormerlySerializedAs("_sceneLoadButtons")]
        [SerializeField]
        private SceneLoader _sceneLoader;

        [SerializeField]
        private LevelButton _buttonPrefab;

        [SerializeField]
        private int _buttonsPerPage = 10;

        [SerializeField, Header("UI Elements")]
        private Transform _buttonContainer;

        [SerializeField]
        private Button _nextButton;

        [SerializeField]
        private Button _prevButton;

        private int _totalLevels;
        private int _unlockedLevels;

        private int _currentPage;

        private List<LevelButton> _pageButtons = new();
        private LevelDataList _levelDataList;

        private bool _isInitialized;

        private int TotalPages => Mathf.CeilToInt((float)_totalLevels / _buttonsPerPage);

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (_isInitialized)
                return;

            this.ValidateSerializedFields();

            GetData();
            CreatePageButtons();
            UpdatePage();
            SetupNavigationButtons();

            _isInitialized = true;
        }

        private void GetData()
        {
            _levelDataList = _jsonLevelParser.GetLevelDataList();
            ProgressData.SetMaxLevelsCount(_levelDataList.Count);

            _totalLevels = _levelDataList.Count;
            _unlockedLevels = ProgressData.UnlockedLevelNumber;

            if (_unlockedLevels > _totalLevels)
                _unlockedLevels = _totalLevels;
        }

        private void CreatePageButtons()
        {
            foreach (Transform child in _buttonContainer)
                Destroy(child.gameObject);

            for (int i = 0; i < _buttonsPerPage; i++)
            {
                LevelButton levelButton = Instantiate(_buttonPrefab, _buttonContainer);
                levelButton.gameObject.SetActive(false);
                levelButton.Initialize(_sceneLoader);
                _pageButtons.Add(levelButton);
            }
        }

        private void UpdatePage()
        {
            int startLevel = _currentPage * _buttonsPerPage + 1;
            int endLevel = Mathf.Min(startLevel + _buttonsPerPage - 1, _totalLevels);

            int buttonIndex = 0;

            foreach (LevelButton levelButton in _pageButtons)
            {
                int levelIndex = startLevel + buttonIndex;

                if (levelIndex <= endLevel)
                {
                    bool isUnlocked = levelIndex <= _unlockedLevels;
                    levelButton.Setup(levelIndex, isUnlocked);
                    levelButton.gameObject.SetActive(true);
                }
                else
                {
                    levelButton.gameObject.SetActive(false);
                }

                buttonIndex++;
            }

            _prevButton.gameObject.SetActive(_currentPage > 0);
            _nextButton.gameObject.SetActive(_currentPage < TotalPages - 1);
        }

        private void SetupNavigationButtons()
        {
            _nextButton.onClick.RemoveAllListeners();
            _nextButton.onClick.AddListener(OnNextPage);

            _prevButton.onClick.RemoveAllListeners();
            _prevButton.onClick.AddListener(OnPrevPage);
        }

        private void OnNextPage()
        {
            if (_currentPage < TotalPages - 1)
            {
                _currentPage++;
                UpdatePage();
            }
        }

        private void OnPrevPage()
        {
            if (_currentPage > 0)
            {
                _currentPage--;
                UpdatePage();
            }
        }
    }
}