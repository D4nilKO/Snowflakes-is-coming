using System;
using System.Text;

namespace Project.Components.Scripts.Utility
{
    [Serializable]
    public class SurviveTimer
    {
        public int Minute => minute;

        public int Second => second;

        public int Millisecond => millisecond;

        public float CompareValue => compareValue;

        private int minute;
        private int second;
        private int millisecond;
        private float time;
        private float compareValue;
        private StringBuilder formattedMinute = new();
        private StringBuilder formattedSecond = new();
        private StringBuilder formattedMillisecond = new();

        public static SurviveTimer Compare(SurviveTimer a, SurviveTimer b)
        {
            return a.CompareValue < b.CompareValue ? a : b;
        }

        public void AddTime(float deltaTime)
        {
            compareValue += deltaTime;
            time += deltaTime;
            if (time >= 0.01f)
            {
                while (time >= 0.01f)
                {
                    time -= 0.01f;
                    millisecond++;
                }
            }

            if (millisecond >= 100)
            {
                millisecond = 0;
                second++;
            }

            if (second < 60) return;
            second = 0;
            minute++;
        }

        public string FormattedTime()
        {
            return String.Format("{0}:{1}",
                ZeroAdder(ref formattedMinute, Minute),
                ZeroAdder(ref formattedSecond, Second));
            //ZeroAdder(ref formattedMillisecond, Millisecond));
        }

        private StringBuilder ZeroAdder(ref StringBuilder builder, int value)
        {
            builder.Remove(0, builder.Length);
            return (value >= 10) ? builder.Append(value) : builder.Append("0" + value);
        }
    }
}