﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineTrip
    {
        public int KeyId { get; set; }
        public TimeSpan StartAt { get; set; }
        public TimeSpan Frequency { get; set; } //if 0 so its mean single exit 
        public TimeSpan FinishAt { get; set; } //It is possible to have several end times per hour

    }
}