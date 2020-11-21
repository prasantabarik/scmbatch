using System;
using System.Collections.Generic;
using System.Text;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageGenerator
{
    class DateTimeUtilities
    {
        public static DateTime GetEndDate(string endDatetime)
        {
            DateTime newEndDate = DateTime.Now;
            if (endDatetime == null || endDatetime == "null" || endDatetime == "BsonNull")
            {
                newEndDate = DateTime.Now.AddDays(42).Date;
            }
            else
            {
                newEndDate = Convert.ToDateTime(endDatetime).Date;
            }
            return newEndDate;
        }
        public static List<DateTime> GetDeliveryMomentBatchDatesToRun()
        {
            var deliveryMomentBatchDatesList = new List<DateTime>();
            for (int iIndex = 2; iIndex <= 42; iIndex++)
            {
                var deliveryMomentDate = DateTime.Now.AddDays(iIndex).Date;
                deliveryMomentBatchDatesList.Add(deliveryMomentDate);
            }

            return deliveryMomentBatchDatesList;
        }
        public static DateTime GetNextDateTimeFromDayCode(DateTime dateTime, string weekCode)
        {
            DayOfWeek dayName = DayOfTheWeekName(weekCode);

            int daysToAdd = ((int)dayName - (int)dateTime.DayOfWeek + 7) % 7;
            return dateTime.AddDays(daysToAdd);
        }
        public static string DayOfTheWeek(string weekCode)
        {
            string weekName = string.Empty;
            switch (weekCode)
            {
                case "MA":
                    weekName = "Monday";
                    break;
                case "DI":
                    weekName = "Tuesday";
                    break;
                case "WO":
                    weekName = "Wednesday";
                    break;
                case "DO":
                    weekName = "Thursday";
                    break;
                case "VR":
                    weekName = "Friday";
                    break;
                case "ZA":
                    weekName = "Saturday";
                    break;
                case "ZO":
                    weekName = "Sunday";
                    break;
                default:
                    break;
            }
            return weekName;
        }
        public static DayOfWeek DayOfTheWeekName(string weekCode)
        {
            DayOfWeek weekName = DayOfWeek.Sunday;
            switch (weekCode)
            {
                case "MA":
                    weekName = DayOfWeek.Monday;
                    break;
                case "DI":
                    weekName = DayOfWeek.Tuesday;
                    break;
                case "WO":
                    weekName = DayOfWeek.Wednesday;
                    break;
                case "DO":
                    weekName = DayOfWeek.Thursday;
                    break;
                case "VR":
                    weekName = DayOfWeek.Friday;
                    break;
                case "ZA":
                    weekName = DayOfWeek.Saturday;
                    break;
                case "ZO":
                    weekName = DayOfWeek.Sunday;
                    break;
                default:
                    break;
            }
            return weekName;
        }
    }
}
