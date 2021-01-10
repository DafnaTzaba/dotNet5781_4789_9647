﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class WrongLicenceException : Exception //when licence is wrong 
    {
        public int Licence;
        public WrongLicenceException() : base() { }
        public WrongLicenceException(int licence) : base() => Licence = licence;
        public WrongLicenceException(int licence, string messege) : base(messege) => Licence = licence;
        public WrongLicenceException(int licence, string messege, Exception innerException) : base(messege, innerException) => Licence = licence;
        public override string ToString()
        {
            return base.ToString() + $",{Licence} מספר רישוי לא תקין";
        }
    }

    [Serializable]
    public class WrongIDExeption:Exception //when the id number is wrong.. use at line, station, trip,linestation..
    {
        public int ID;
        public WrongIDExeption() : base() { }

        public WrongIDExeption(int idline) : base() => ID = idline;
        public WrongIDExeption(int idline, string messege) : base(messege) => ID = idline;
        public WrongIDExeption(int idline, string messege, Exception innerException) : base(messege, innerException) => ID = idline;
        public override string ToString()
        {
            return base.ToString() + $",{ID} מספר הקו לא תקין";
        }

    }

    [Serializable]
    public class WrongNameExeption :Exception //if name user wrong
    {
        public string Name;
        public WrongNameExeption() : base() { }

        public WrongNameExeption(string name) : base() => Name = name;
        public WrongNameExeption(string name, string messege) : base(messege) => Name = name;
        public WrongNameExeption(string name, string messege, Exception innerException) : base(messege, innerException) => Name = name;
        public override string ToString()
        {
            return base.ToString() + $",{Name} השם שהוכנס לא תקין";
        }
    }

    [Serializable]
    public class WrongLineTripExeption : Exception //if name user wrong
    {
        public int ID;
        public WrongLineTripExeption() : base() { }

        public WrongLineTripExeption(int idline) : base() => ID = idline;
        public WrongLineTripExeption(int idline, string messege) : base(messege) => ID = idline;
        public WrongLineTripExeption(int idline, string messege, Exception innerException) : base(messege, innerException) => ID = idline;
        public override string ToString()
        {
            return base.ToString() + $"{ID} מצטערים, לא נמצאו זמני נסיעה עבור ";
        }
    }




}
