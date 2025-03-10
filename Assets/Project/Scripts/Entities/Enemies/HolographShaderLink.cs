using UnityEngine;

namespace Project.Entities.Enemies
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class HolographShaderLink : MonoBehaviour
    {
        private const string MainPropertyName = "_InputValue";
        private readonly int _inputValueID = Shader.PropertyToID(MainPropertyName);

        private float _inputValue;
        private float _speed = 10;

        private Material _material;

        private void Start()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        private void Update()
        {
            SetShaderProperties();
        }

        private void SetShaderProperties()
        {
            _inputValue += Time.deltaTime * _speed;
            _material.SetFloat(_inputValueID, _inputValue);
        }
    }
}