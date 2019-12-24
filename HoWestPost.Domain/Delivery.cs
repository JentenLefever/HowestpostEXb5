using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HoWestPost.Domain
{
    public class Delivery
    {
        public event DeliveryProgressHandler DeliveryProgress;

        private DateTime starttijd;
        public DateTime StartTijd
        {
            get { return starttijd; }
        }
        
        private double afstandTijd = 0;
        public double AfstandTijd
        {
            get { return afstandTijd; }
        }

        private Thread DeliveryThread = null;

        public void StartDelivery(double LeverTijd)
        {
            starttijd = DateTime.Now;
            this.afstandTijd = LeverTijd / 10;

            try
            {
                DeliveryThread = new Thread(new ThreadStart(DeliveryThreadProc));
                DeliveryThread.Start();
            }
            catch (ThreadAbortException tae)
            {
                //niets doen
            }

        }       

        private void DeliveryThreadProc()
        {
            
            
                double afgelegdeTijd = 0;

                while (afgelegdeTijd < this.afstandTijd)
                {                    
                    TimeSpan verstreken = DateTime.Now - starttijd;
                    TimeSpan resterend = TimeSpan.FromSeconds((double)this.afstandTijd);
                    resterend = resterend - verstreken;
                    afgelegdeTijd = (double)verstreken.TotalSeconds;
                    Thread.Sleep(1);

                    if (DeliveryProgress != null)
                    {
                        DeliveryProgress(this, new DeliveryEventArgs(   
                            resterend
                            ));
                    }
                }
                if (DeliveryProgress != null)
                {
                    DeliveryProgress(this, new DeliveryEventArgs(                        
                        new TimeSpan()
                        ));
                }
            
        }
    }
}
