using System;
using System.Collections.Generic;
using System.Text;

namespace TCS.MVP.DeliveryMoment.DeliveryMoment.Batch.DeliveryMomentMessageHandler
{
   public interface IDeliveryMomentMessageHandler
    {
        void PublishDeliveryMomentMessageAsync(string message);
    }
}
