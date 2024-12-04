using Project.LevelSystem;
using UnityEngine;
using YG;

namespace Project.Services
{
    public class FinalPanelView : MonoBehaviour
    {
        [SerializeField]
        private LevelLoader _levelLoader;

        [SerializeField]
        private GameObject _finalCanvas;

        [SerializeField]
        private GameObject _reviewPanel;

        [SerializeField]
        private GameObject _outOfLevelsPanel;

        private void Awake()
        {
            _levelLoader.LastLevelReached += OnLastLevelReached;
        }

        private void OnDestroy()
        {
            _levelLoader.LastLevelReached -= OnLastLevelReached;
        }

        private void OnLastLevelReached()
        {
            _finalCanvas.SetActive(true);
            ShowRightOnePanel();
        }

        private void ShowRightOnePanel()
        {
            if (YandexGame.EnvironmentData.reviewCanShow)
            {
                _reviewPanel.SetActive(true);
                _outOfLevelsPanel.SetActive(false);
            }
            else
            {
                _reviewPanel.SetActive(false);
                _outOfLevelsPanel.SetActive(true);
            }
        }
    }
}