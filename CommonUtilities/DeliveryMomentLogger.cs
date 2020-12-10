using System;
using System.Collections.Generic;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.CommonUtilities
{
    public class DeliveryMomentLogger
    {
        public static List<LogModel> _logModelList;

        public static void StartLogs()
        {
            _logModelList = new List<LogModel>();
        }

        public static List<LogModel> GetLogs()
        {
          return  _logModelList;
        }

        public static void AddToLog(LogModel logModel)
        {
            if (_logModelList == null)
            {
                _logModelList = new List<LogModel>();
            }
            _logModelList.Add(logModel);
        }

        public static void ClearLogs()
        {
            if (_logModelList != null)
            {
                _logModelList.Clear();
            }
        }
    }

    public class LogModel
    {
        public string Id { get; set; }
        public string StoreNumber { get; set; }
        public string Opration { get; set; }
        public string Data { get; set; }

        public LogModel(string id, string operation, string data, string storeNumber)
        {
            StoreNumber = storeNumber;
            Id = id;
            Opration = operation;
            Data = data;
        }
    }
}
