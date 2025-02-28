using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using YG;

namespace Project.Services.MessageSystem
{
    public class ScreenMessageHandler : MonoBehaviour, IScreenMessageHandler
    {
        private List<ScreenMessage> _messages = new();

        [SerializeField]
        private float _messageDuration = 3f;

        [SerializeField]
        private float _fadeDuration = 1f;

        private Queue<ScreenMessage> _messageQueue = new();
        private Coroutine _displayCoroutine;
        private WaitForSecondsRealtime _waitDuration;

        private void Awake()
        {
            _waitDuration = new WaitForSecondsRealtime(_messageDuration);
            FillMessagesList();
            HideAllMessages();
        }

        private void FillMessagesList()
        {
            _messages = GetComponentsInChildren<ScreenMessage>(true).ToList();

            if (_messages.Count == 0)
                Debug.LogError("Не найдено сообщений для добавления в массив");
        }

        private void HideAllMessages()
        {
            foreach (var message in _messages)
            {
                message.gameObject.SetActive(false);
                message.CanvasGroup.alpha = 0f;
            }
        }

        public void ShowMessage(string key, string additionalText = "")
        {
            ScreenMessage message = _messages.Find(m => m.Key == key);

            if (message == null)
            {
                Debug.LogError($"Сообщение с ключом '{key}' не найдено!");
                return;
            }

            if (additionalText != "" && message.gameObject.TryGetComponent(out LangYGAdditionalText tmpText))
            {
                tmpText.additionalText = additionalText;
            }

            _messageQueue.Enqueue(message);

            _displayCoroutine ??= StartCoroutine(DisplayMessages());
        }

        private IEnumerator DisplayMessages()
        {
            while (_messageQueue.Count > 0)
            {
                var message = _messageQueue.Dequeue();
                message.gameObject.SetActive(true);
                message.CanvasGroup.alpha = 1f;

                yield return _waitDuration;

                yield return message.CanvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.Linear).SetUpdate(true)
                    .OnComplete(() => { message.gameObject.SetActive(false); }).WaitForCompletion();
            }

            _displayCoroutine = null;
        }
    }
}