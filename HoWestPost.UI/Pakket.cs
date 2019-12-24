using HoWestPost.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoWestPost.UI
{
    public class Pakket
    {
        public bool Prior { get; set; }
        public PakketType PakketType { get; set; }
        public double Reistijd { get; set; }
    }
}
