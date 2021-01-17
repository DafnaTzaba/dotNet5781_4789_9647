﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BL
{
     internal  class ClockS
    {
        #region singelton
        static readonly ClockS instance = new ClockS();
        static ClockS() { }
        public static ClockS Instance { get => instance; }
        #endregion

        internal class Clock
        {
            public TimeSpan Time;
            internal Clock(TimeSpan time) => Time = time;//constructor
        }
        internal volatile bool Cancel;
        private volatile Clock sClock = null;
        internal  Clock SClock => sClock;
        private int rate;
        internal int Rate => rate;
        private Stopwatch stoper = new Stopwatch();
        private Action<TimeSpan> observerClock = null;
        internal event Action<TimeSpan>ObserverClock
        {
            add => observerClock = value;
            remove => observerClock = null;
        }
        private TimeSpan startTime;
        internal void Start(TimeSpan mstartTime,int mrate)
        {
            startTime = mstartTime;
            sClock = new Clock(startTime);
            rate = mrate;
            Cancel = false;
            stoper.Restart();
            new Thread(clockThread).Start();
          ///  TripSimulator.Instance.Start();

        }

        void clockThread()//update the clock when not cancle
        {
            while (!Cancel)
            {
                sClock = new Clock(startTime + new TimeSpan(stoper.ElapsedTicks * rate));
                observerClock(new TimeSpan(sClock.Time.Hours, sClock.Time.Minutes, sClock.Time.Seconds));//actuator the event
                Thread.Sleep(100);
            }
            observerClock = null;
        }
        internal void Stop() => Cancel = true;
    }
}

    
