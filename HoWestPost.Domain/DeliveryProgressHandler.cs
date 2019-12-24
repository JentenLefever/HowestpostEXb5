using System;

namespace HoWestPost.Domain
{
    public delegate void DeliveryProgressHandler(object sender, DeliveryEventArgs e);

    public class DeliveryEventArgs
    {
        public TimeSpan ResterendeTijd { get; set; }

        public DeliveryEventArgs(TimeSpan resterendeTijd)
        {
            this.ResterendeTijd = resterendeTijd;
        }
    }
}
