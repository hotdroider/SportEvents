using Newtonsoft.Json;
using SportEvents.Core.Entities;
using SportEvents.MQ.Messages;
using System.Text;

namespace SportEvents.MQ
{
    public static class Helpers
    {
        /// <summary>
        /// Сериализовать в JSON и вернуть в виде байтового массива
        /// </summary>
        public static byte[] ToMessageBody(this object packMe)
        {
            var json = JsonConvert.SerializeObject(packMe);
            return Encoding.UTF8.GetBytes(json);
        }

        public static T FromMessageBody<T>(this byte[] body)
        {
            var json = Encoding.UTF8.GetString(body);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Обновить событие по сообщению
        /// </summary>
        public static void UpdateByMessage(this Event evnt, EventMessage message)
        {
            //маппер для бедных
            evnt.EventDate = message.EventDate;
            evnt.Name = message.Name;
            evnt.SportId = message.SportId;
            evnt.Status = message.Status;
            evnt.Team1Price = message.Team1Price;
            evnt.Team2Price = message.Team2Price;
            evnt.DrawPrice = message.DrawPrice;
        }
    }
}
