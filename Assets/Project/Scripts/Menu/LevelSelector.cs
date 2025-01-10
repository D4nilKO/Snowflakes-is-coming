using System.Collections.Generic;
using Project.Data;
using Project.LevelSystem;
using Project.LevelSystem.LevelStructure;
using Project.WindowSystem;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Project.Menu
{
    public class LevelSelector : BaseWindow, IStartable
    {
        [SerializeField]
        private int _buttonsPerPage = 10;

        [SerializeField]
        private LevelButton _buttonPrefab;

        [SerializeField, Header("UI Elements")]
        private Transform _buttonContainer;

        [SerializeField]
        private Button _nextButton;

        [SerializeField]
        private Button _prevButton;

        [SerializeField]
        private Button _backButton;

        private int _totalLevels;
        private int _unlockedLevels;

        private int _currentPage;

        private SceneLoader _sceneLoader;
        private JsonLevelParser _jsonLevelParser;
        private ProgressData _progressData;

        private List<LevelButton> _pageButtons = new();
        private LevelDataList _levelDataList;

        private int TotalPages => Mathf.CeilToInt((float)_totalLevels / _buttonsPerPage);

        [Inject]
        public void Construct(SceneLoader sceneLoader, JsonLevelParser jsonLevelParser, ProgressData progressData)
        {
            this.ValidateSerializedFields();

            _sceneLoader = sceneLoader;
            _jsonLevelParser = jsonLevelParser;
            _progressData = progressData;
        }

        public void Start()
        {
            SetupNavigationButtons();
            Initialize();
        }

        private void Initialize()
        {
            GetData();
            CreateAndSetup();
        }

        private void GetData()
        {
            _levelDataList = _jsonLevelParser.GetLevelDataList();

            _totalLevels = _levelDataList.Count;
            _unlockedLevels = _progressData.UnlockedLevelNumber;

            if (_unlockedLevels > _totalLevels)
                _unlockedLevels = _totalLevels;
        }

        private void CreateAndSetup()
        {
            ClearButtons();
            CreatePageButtons();
            UpdatePage();
        }

        private void ClearButtons()
        {
            foreach (Transform child in _buttonContainer)
            {
                Destroy(child.gameObject);
            }

            _pageButtons.Clear();
        }

        private void CreatePageButtons()
        {
            for (int i = 0; i < _buttonsPerPage; i++)
            {
                LevelButton levelButton = Instantiate(_buttonPrefab, _buttonContainer);
                levelButton.Initialize(_sceneLoader, _progressData);
                levelButton.gameObject.SetActive(false);
                _pageButtons.Add(levelButton);
            }
        }

        private void UpdatePage()
        {
            int startLevel = _currentPage * _buttonsPerPage + 1;
            int endLevel = Mathf.Min(startLevel + _buttonsPerPage - 1, _totalLevels);

            for (int i = 0; i < _pageButtons.Count; i++)
            {
                int levelIndex = startLevel + i;

                if (levelIndex <= endLevel)
                {
                    bool isUnlocked = levelIndex <= _unlockedLevels;
                    _pageButtons[i].Setup(levelIndex, isUnlocked);
                    _pageButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    _pageButtons[i].gameObject.SetActive(false);
                }
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

            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(OnBackButtonClicked);
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

        private void OnBackButtonClicked()
        {
            WindowManager.ShowWindow("Main");
        }
    }
}