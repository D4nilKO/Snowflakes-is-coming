using System;
using Project.UI;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Architecture.UI
{
    public class LosePanel : OutcomeUiPanel
    {
        [field: SerializeField]
        public Button ShowAdButton { get; private set; }

        protected override void AddListeners()
        {
            base.AddListeners();

            ShowAdButton.onClick.RemoveAllListeners();
            ShowAdButton.onClick.AddListener(ShowRewardAd);
        }

        private void ShowRewardAd()
        {
            YandexGame.RewVideoShow(0);
        }
    }
}