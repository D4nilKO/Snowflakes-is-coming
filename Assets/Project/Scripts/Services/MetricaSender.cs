using System.Collections.Generic;
using YG;

namespace Project.Services
{
    public static class MetricaSender
    {
        public static void SendWithId(string id, string value)
        {
            Dictionary<string, string> eventParams = new()
            {
                { id, value }
            };

            YandexMetrica.Send(id, eventParams);
        }
    }
}