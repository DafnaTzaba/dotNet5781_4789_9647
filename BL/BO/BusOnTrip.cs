﻿using System;

namespace BO
{
    public class BusOnTrip
    {
        public int KeyId { get; set; } //identity of bus at travel
        public int LicenceNum { get; set; }
        public int LineId { get; set; }//???????????
        public TimeSpan PlannedTakeOff { get; set; }
        public TimeSpan ActualTakeOff { get; set; }
        public int PrevStation { get; set; }
        public TimeSpan TimeAtPrevStation { get; set; }
        public TimeSpan NextStation { get; set; }
    }
}
