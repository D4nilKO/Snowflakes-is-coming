using UnityEngine;
using YG;

namespace Project.Services
{
    public class VersionNumber : MonoBehaviour
    {
        private LangYGAdditionalText _text;

        private void Start()
        {
            _text = GetComponent<LangYGAdditionalText>();
            _text.additionalText = $" {Application.version}";
        }
    }
}