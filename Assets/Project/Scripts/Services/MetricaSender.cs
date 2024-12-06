using System.Collections.Generic;
using UnityEngine;
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

        // Отправка без группировки по целям.
        // public void Send(string id)
        // {
        //     YandexMetrica.Send(id);
        // }
    }
}