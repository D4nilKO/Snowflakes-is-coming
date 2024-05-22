using System;
using System.Text;

namespace Project.Components.Scripts.Utility
{
    [Serializable]
    public class SurviveTimer
    {
        private StringBuilder _formattedMinute = new();
        private StringBuilder _formattedSecond = new();

        private int _millisecond;
        private float _time;

        public int Minute { get; private set; }
        public int Second { get; private set; }
        public float CompareValue { get; private set; }

        public static SurviveTimer Compare(SurviveTimer a, SurviveTimer b)
        {
            return a.CompareValue < b.CompareValue ? a : b;
        }

        public void AddTime(float deltaTime)
        {
            CompareValue += deltaTime;
            _time += deltaTime;

            if (_time >= 0.01f)
            {
                while (_time >= 0.01f)
                {
                    _time -= 0.01f;
                    _millisecond++;
                }
            }

            if (_millisecond >= 100)
            {
                _millisecond = 0;
                Second++;
            }

            if (Second < 60)
                return;

            Second = 0;
            Minute++;
        }

        public string FormattedTime()
        {
            return $"{ZeroAdder(ref _formattedMinute, Minute)}:{ZeroAdder(ref _formattedSecond, Second)}";
        }

        private StringBuilder ZeroAdder(ref StringBuilder builder, int value)
        {
            builder.Remove(0, builder.Length);

            return (value >= 10)
                ? builder.Append(value)
                : builder.Append("0" + value);
        }
    }
}