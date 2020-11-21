using System;
using System.Collections.Generic;
using System.Linq;
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

        public static DateTime GetEndDateForLogisticChannel(string endDatetime)
        {
            DateTime newEndDate = DateTime.Now;
            if (endDatetime == null || endDatetime == "null" || endDatetime == "BsonNull")
            {
                newEndDate = DateTime.Now.AddDays(42).Date;
            }
            else
            {
                try
                {
                    newEndDate = Convert.ToDateTime(endDatetime).Date;
                }
                catch (Exception ex)
                {
                    newEndDate = GetDate(endDatetime).Date;
                }

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

        public static DateTime GetDate(string strDateTime)
        {
            DateTime dateTime = DateTime.Now;
            try
            {
                dateTime = Convert.ToDateTime(strDateTime).Date;
            }
            catch (Exception ex)
            {
                string newstrDateTime = strDateTime;
                string[] dateValues = strDateTime.Split('-');
                if (dateValues != null && dateValues.Count() > 2)
                {
                    string month = dateValues[1];
                    if (month.Length == 1)
                    {
                        month = string.Concat("0", dateValues[1]);
                    }
                    newstrDateTime = $"{dateValues[0]}-{month}-{dateValues[2]}";
                }
                dateTime = DateTime.ParseExact(newstrDateTime, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            return dateTime;
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
