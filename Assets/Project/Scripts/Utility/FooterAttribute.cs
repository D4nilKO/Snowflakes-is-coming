using UnityEngine;

namespace Project.Utility
{
    public class FooterAttribute : PropertyAttribute
    {
        public string Text;

        public FooterAttribute(string text)
        {
            Text = text;
        }
    }
}