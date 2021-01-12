﻿using DALAPI;
using DO;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
//https://www.gov.il/he/Departments/General/gtfs_general_transit_feed_specifications

namespace DL
{
    sealed class DalXml : IDAL
    {
        private XElement serials;

        #region singelton
        static readonly DalXml instance = new DalXml();
        static DalXml() { }// static ctor to ensure instance init is done just before first usage
        DalXml() { }   // default => private
        public static DalXml Instance { get => instance; }// The public Instance property to use
        #endregion

        #region DS XML Files
        string busPath = @"BusXML.xml"; //XElement        
        string linePath = @"LineXml.xml"; //XMLSerializer
        string stationPath = @"StationXml.xml"; //XMLSerializer
                                        //string lecturersPath = @"LecturersXml.xml"; //XMLSerializer
                                        //string lectInCoursesPath = @"LecturerInCourseXml.xml"; //XMLSerializer
                                        //string studInCoursesPath = @"StudentInCoureseXml.xml"; //XMLSerializer
        #endregion


        #region Bus
        public Bus GetBus(string licence)
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath); //get the data from xml
            DO.Bus b = (from bus in busRootElem.Elements()
                        where bus.Element("Licence").Value == licence && Convert.ToBoolean(bus.Element("BusExist").Value) == true
                        select new DO.Bus()
                        {
                            Licence = bus.Element("Licence").Value,
                            StartingDate = DateTime.Parse(bus.Element("StartingDate").Value),
                            Kilometrz = Double.Parse(bus.Element("Kilometrz").Value),
                            KilometrFromLastTreat = Double.Parse(bus.Element("KilometrFromLastTreat").Value),
                            FuellAmount = Double.Parse(bus.Element("FuellAmount").Value),
                            StatusBus = (STUTUS)Enum.Parse(typeof(STUTUS), bus.Element("StatusBus").Value),
                            BusExist = Boolean.Parse(bus.Element("BusExist").Value),
                            LastTreatment = DateTime.Parse(bus.Element("LastTreatment").Value)
                        }
            ).FirstOrDefault();

            if (b == null)
                throw new DO.WrongLicenceException(Convert.ToInt32(licence), "האוטובוס המבוקש לא נמצא במערכת");
            return b;
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath); //get the data from xml

            return (from bus in busRootElem.Elements()
                    where Convert.ToBoolean(bus.Element("BusExist").Value) == true
                    select new DO.Bus()
                    {
                        Licence = bus.Element("Licence").Value,
                        StartingDate = DateTime.Parse(bus.Element("StartingDate").Value),
                        Kilometrz = Double.Parse(bus.Element("Kilometrz").Value),
                        KilometrFromLastTreat = Double.Parse(bus.Element("KilometrFromLastTreat").Value),
                        FuellAmount = Double.Parse(bus.Element("FuellAmount").Value),
                        StatusBus = (STUTUS)Enum.Parse(typeof(STUTUS), bus.Element("StatusBus").Value),
                        BusExist = Boolean.Parse(bus.Element("BusExist").Value),
                        LastTreatment = DateTime.Parse(bus.Element("LastTreatment").Value)
                    }
              );

        }

        public IEnumerable<Bus> GetAllBusesStusus(STUTUS stusus)
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath); //get the data from xml

            return (from bus in busRootElem.Elements()
                    where (STUTUS)Enum.Parse(typeof(STUTUS), bus.Element("StatusBus").Value) == stusus && Convert.ToBoolean(bus.Element("BusExist").Value) == true
                    select new DO.Bus()
                    {
                        Licence = bus.Element("Licence").Value,
                        StartingDate = DateTime.Parse(bus.Element("StartingDate").Value),
                        Kilometrz = Double.Parse(bus.Element("Kilometrz").Value),
                        KilometrFromLastTreat = Double.Parse(bus.Element("KilometrFromLastTreat").Value),
                        FuellAmount = Double.Parse(bus.Element("FuellAmount").Value),
                        StatusBus = (STUTUS)Enum.Parse(typeof(STUTUS), bus.Element("StatusBus").Value),
                        BusExist = Boolean.Parse(bus.Element("BusExist").Value),
                        LastTreatment = DateTime.Parse(bus.Element("LastTreatment").Value)
                    }
              );
        }

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> buscondition)
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath); //get the data from xml
            return from bus in busRootElem.Elements()
                   let b1 = new DO.Bus()
                   {
                       Licence = bus.Element("Licence").Value,
                       StartingDate = DateTime.Parse(bus.Element("StartingDate").Value),
                       Kilometrz = Double.Parse(bus.Element("Kilometrz").Value),
                       KilometrFromLastTreat = Double.Parse(bus.Element("KilometrFromLastTreat").Value),
                       FuellAmount = Double.Parse(bus.Element("FuellAmount").Value),
                       StatusBus = (STUTUS)Enum.Parse(typeof(STUTUS), bus.Element("StatusBus").Value),
                       BusExist = Boolean.Parse(bus.Element("BusExist").Value),
                       LastTreatment = DateTime.Parse(bus.Element("LastTreatment").Value)
                   }
                   where buscondition(b1) && Convert.ToBoolean(bus.Element("BusExist").Value) == true
                   select b1;
        }

        public int AddBus(Bus bus)
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath); //get the data from xml
            XElement findBus = (from b in busRootElem.Elements()
                                where b.Element("Licence").Value == bus.Licence && Convert.ToBoolean(b.Element("BusExist").Value) == true
                                select b).FirstOrDefault();
            if (findBus != null)
                throw new DO.WrongLicenceException(Convert.ToInt32(bus.Licence), "אוטובוס זה כבר קיים במערכת, באפשרותך לעדכן נתונים עליו במקום המתאים");
            XElement busToAdd = new XElement("Bus",
                     new XElement("Licence", bus.Licence),
                     new XElement("StartingDate", bus.StartingDate),
                     new XElement("Kilometrz", bus.Kilometrz),
                     new XElement("KilometrFromLastTreat", bus.KilometrFromLastTreat),
                     new XElement("FuellAmount", bus.FuellAmount),
                     new XElement("StatusBus", bus.StatusBus.ToString()),
                     new XElement("BusExist", bus.BusExist),
                     new XElement("LastTreatment", bus.LastTreatment));

            busRootElem.Add(busToAdd);
            XMLTools.SaveListToXMLElement(busRootElem, busPath);
            return 1;

        }

        public bool DeleteBus(string licence)
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath); //get the data from xml
            XElement bus= (from b in busRootElem.Elements()
                           where b.Element("Licence").Value == licence && Convert.ToBoolean(b.Element("BusExist").Value) == true
                           select b).FirstOrDefault();
            if (bus != null)
            {
                bus.Element("BusExist").Value = "false";
                XMLTools.SaveListToXMLElement(busRootElem, busPath);
                return true;
            }
            else
                throw new DO.WrongLicenceException(Convert.ToInt32(licence), "האוטובוס המבוקש אינו קיים במערכת");


        }

        public bool UpdateBus(Bus bus)
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath); //get the data from xml
            XElement findBus = (from b in busRootElem.Elements()
                                where b.Element("Licence").Value == bus.Licence && Convert.ToBoolean(b.Element("BusExist").Value) == true
                                select b).FirstOrDefault();
            if (findBus == null) //we not find the bus
                throw new DO.WrongLicenceException(Convert.ToInt32(bus.Licence), "אוטובוס זה כבר קיים במערכת, באפשרותך לעדכן נתונים עליו במקום המתאים");
            else
            {
                findBus.Element("Licence").Value = bus.Licence;
                findBus.Element("StartingDate").Value = bus.StartingDate.ToString();
                findBus.Element("Kilometrz").Value = bus.Kilometrz.ToString();
                findBus.Element("KilometrFromLastTreat").Value = bus.KilometrFromLastTreat.ToString();
                    findBus.Element("FuellAmount").Value = bus.FuellAmount.ToString();
                findBus.Element("StatusBus").Value = bus.StatusBus.ToString();
                findBus.Element("BusExist").Value = bus.BusExist.ToString();
                findBus.Element("LastTreatment").Value = bus.LastTreatment.ToString();

                XMLTools.SaveListToXMLElement(busRootElem, busPath);


                return true;

            }

        }



        #endregion

        #region Line

      

        public Line GetLine(int idline)
        {
            XElement lineRootElem = XMLTools.LoadListFromXMLElement(linePath); //get the data from xml
            DO.Line b = (from line in lineRootElem.Elements()
                        where line.Element("IdNumber").Value == idline.ToString() && Convert.ToBoolean(line.Element("LineExist").Value) == true
                        select new DO.Line()
                        {
                            IdNumber = Convert.ToInt32(line.Element("IdNumber").Value),
                            NumberLine = Convert.ToInt32(line.Element("NumberLine").Value),
                            FirstStationCode = Convert.ToInt32(line.Element("FirstStationCode").Value),
                            LastStationCode = Convert.ToInt32(line.Element("LastStationCode").Value),
                            Area = (AREA) Enum.Parse(typeof(AREA),line.Element("Area").Value),
                            LineExist = Convert.ToBoolean(line.Element("LineExist").Value)

                        }
            ).FirstOrDefault();

            if (b == null)
                throw new DO.WrongIDExeption(idline, "  הקו המבוקש לא נמצא במערכת");
            return b;
        }

        public IEnumerable<Line> GetAllLineBy(Predicate<Line> linecondition)
        {


            XElement lineRootElem = XMLTools.LoadListFromXMLElement(linePath); //get the data from xml
            return from line in lineRootElem.Elements()
                   let l1 = new DO.Line()
                   {
                       IdNumber = Convert.ToInt32(line.Element("IdNumber").Value),
                       NumberLine = Convert.ToInt32(line.Element("NumberLine").Value),
                       FirstStationCode = Convert.ToInt32(line.Element("FirstStationCode").Value),
                       LastStationCode = Convert.ToInt32(line.Element("LastStationCode").Value),
                       Area = (AREA)Enum.Parse(typeof(AREA), line.Element("Area").Value),
                       LineExist = Convert.ToBoolean(line.Element("LineExist").Value)

                     
                   }
                   where linecondition(l1) && Convert.ToBoolean(line.Element("LineExist").Value) == true
                   select l1;

        }

        public IEnumerable<Line> GetAllLine()
        {

            XElement lineRootElem = XMLTools.LoadListFromXMLElement(linePath); //get the data from xml

            return (from line in lineRootElem.Elements()
                    where Convert.ToBoolean(line.Element("LineExist").Value) == true
                    select new DO.Line()
                    {
                        IdNumber = Convert.ToInt32(line.Element("IdNumber").Value),
                        NumberLine = Convert.ToInt32(line.Element("NumberLine").Value),
                        FirstStationCode = Convert.ToInt32(line.Element("FirstStationCode").Value),
                        LastStationCode = Convert.ToInt32(line.Element("LastStationCode").Value),
                        Area = (AREA)Enum.Parse(typeof(AREA), line.Element("Area").Value),
                        LineExist = Convert.ToBoolean(line.Element("LineExist").Value)

                    }
              );
        }

        public IEnumerable<Line> GetAllLinesArea(AREA area)
        {
            XElement lineRootElem = XMLTools.LoadListFromXMLElement(linePath); //get the data from xml

            return (from line in lineRootElem.Elements()
                    where (AREA)Enum.Parse(typeof(AREA), line.Element("Area").Value) == area && Convert.ToBoolean(line.Element("LineExist").Value) == true
                    select new DO.Line()
                    {
                        IdNumber = Convert.ToInt32(line.Element("IdNumber").Value),
                        NumberLine = Convert.ToInt32(line.Element("NumberLine").Value),
                        FirstStationCode = Convert.ToInt32(line.Element("FirstStationCode").Value),
                        LastStationCode = Convert.ToInt32(line.Element("LastStationCode").Value),
                        Area = (AREA)Enum.Parse(typeof(AREA), line.Element("Area").Value),
                        LineExist = Convert.ToBoolean(line.Element("LineExist").Value)
                    }
              );
        }

        public int AddLine(Line line)
        {  //eliezer told us that we can do here not checking because check according to idnumber is meaningless

            serials = XElement.Load(@"Serials.xml");
            int idLine = int.Parse(serials.Element("LineCounter").Value);

            serials.Element("LineCounter").Value = (++idLine).ToString();

            List<Line> lines = XMLTools.LoadListFromXMLSerializer<Line>(linePath);
            line.IdNumber = idLine;

            lines.Add(line);
            XMLTools.SaveListToXMLSerializer(lines, linePath);

            serials.Save(@"Serials.xml");
            return idLine;
        }

        public void UpdateLine(Line line)
        {
            XElement lineRootElem = XMLTools.LoadListFromXMLElement(linePath); //get the data from xml
            XElement findLine = (from l in lineRootElem.Elements()
                                where l.Element("IdNumber").Value == line.IdNumber.ToString() && Convert.ToBoolean(l.Element("LineExist").Value) == true
                                select l).FirstOrDefault();
            if (findLine == null) //we not find the bus
                throw new DO.WrongLicenceException(Convert.ToInt32(line.IdNumber), "קו זה כבר קיים במערכת, באפשרותך לעדכן נתונים עליו במקום המתאים");
            else
            {
                findLine.Element("IdNumber").Value = line.IdNumber.ToString();
                findLine.Element("NumberLine").Value = line.NumberLine.ToString();
                findLine.Element("FirstStationCode").Value = line.FirstStationCode.ToString();
                findLine.Element("LastStationCode").Value = line.LastStationCode.ToString();
                findLine.Element("Area").Value = line.Area.ToString();
                findLine.Element("LineExist").Value = line.LineExist.ToString();
               

                XMLTools.SaveListToXMLElement(lineRootElem, linePath);


            }

        }

        public void DeleteLine(int idnumber)
        {
            XElement lineRootElem = XMLTools.LoadListFromXMLElement(linePath); //get the data from xml
            XElement line = (from l in lineRootElem.Elements()
                            where l.Element("IdNumber").Value == idnumber.ToString() && Convert.ToBoolean(l.Element("LineExist").Value) == true
                            select l).FirstOrDefault();
            if (line != null)
            {
                line.Element("LineExist").Value = "false";
                XMLTools.SaveListToXMLElement(lineRootElem, linePath);
                
            }
            else
                throw new DO.WrongLicenceException(idnumber, "הקו המבוקש אינו קיים במערכת");



        }



        #endregion


        #region Station

        /*
       

          public int Code { get; set; }
        public string Name { get; set; }
         public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public string Address { get; set; }
        public bool StationExist { get; set; }
         
   
    


        public void DeleteStations(int code)
        {
            DO.Stations stations = DataSource.ListStations.Find(b => b.Code == code && b.StationExist);
            if (stations != null)
            {
                stations.StationExist = false;

            }
            else
                throw new DO.WrongIDExeption(code, "התחנה לא קיימת במערכת");//////////////////////////////////////////////////////
        }
        

         */
        public Stations GetStations(int code)
        {
            XElement stationRootElem = XMLTools.LoadListFromXMLElement(stationPath); //get the data from xml
            DO.Stations b = (from station in stationRootElem.Elements()
                             where station.Element("Code").Value == code.ToString() && Convert.ToBoolean(station.Element("StationExist").Value) == true
                             select new DO.Stations()
                             {
                                 Code = Convert.ToInt32(station.Element("Code").Value),
                                 Name = station.Element("Name").Value,
                                 Address = station.Element("Address").Value,
                                 Latitude= Convert.ToDouble(station.Element("Latitude").Value),
                                 Longtitude = Convert.ToDouble(station.Element("Longtitude").Value),
                                 StationExist = Boolean.Parse(station.Element("StationExist").Value)

                             }
            ) .FirstOrDefault();

            if (b == null)
                throw new DO.WrongLicenceException(code, "התחנה המבוקש לא נמצא במערכת");
            return b;


        }

        public IEnumerable<Stations> GetAllStations()
        {
            XElement stationRootElem = XMLTools.LoadListFromXMLElement(stationPath); //get the data from xml

            return (from station in stationRootElem.Elements()
                    where Convert.ToBoolean(station.Element("StationExist").Value) == true
                    select new DO.Stations()
                    {
                        Code = Convert.ToInt32(station.Element("Code").Value),
                        Name = station.Element("Name").Value,
                        Address = station.Element("Address").Value,
                        Latitude = Convert.ToDouble(station.Element("Latitude").Value),
                        Longtitude = Convert.ToDouble(station.Element("Longtitude").Value),
                        StationExist = Boolean.Parse(station.Element("StationExist").Value)

                    }
              );

        }

        public IEnumerable<Stations> GetAllStationsBy(Predicate<Stations> Stationscondition)
        {
            XElement stationRootElem = XMLTools.LoadListFromXMLElement(stationPath); //get the data from xml
            return from station in stationRootElem.Elements()
                   let b1 = new DO.Stations()
                   {
                       Code = Convert.ToInt32(station.Element("Code").Value),
                       Name = station.Element("Name").Value,
                       Address = station.Element("Address").Value,
                       Latitude = Convert.ToDouble(station.Element("Latitude").Value),
                       Longtitude = Convert.ToDouble(station.Element("Longtitude").Value),
                       StationExist = Boolean.Parse(station.Element("StationExist").Value)
                   }
                   where Stationscondition(b1) && Convert.ToBoolean(station.Element("StationExist").Value) == true
                   select b1;
        }

        public void AddStations(Stations station)
        {
            XElement stationRootElem = XMLTools.LoadListFromXMLElement(stationPath); //get the data from xml
            XElement stationBus = (from b in stationRootElem.Elements()
                                   where b.Element("Code").Value == station.Code.ToString() && Convert.ToBoolean(b.Element("StationExist").Value) == true
                                   select b).FirstOrDefault();
            if (stationBus != null)
                throw new DO.WrongIDExeption(station.Code, "תחנה זו כבר קיימת במערכת, באפשרותך לעדכן נתונים עליו במקום המתאים");

            XElement stationToAdd = new XElement("Stations",
                    new XElement("Code", station.Code),
                    new XElement("Name", station.Name),
                    new XElement("Address", station.Address),
                    new XElement("Latitude", station.Latitude),
                    new XElement("Longtitude", station.Longtitude),
                    new XElement("StationExist", station.StationExist));


            stationRootElem.Add(stationToAdd);
            XMLTools.SaveListToXMLElement(stationRootElem, stationPath);
 
        }

        #endregion

        public void AddLineStations(LineStation station)
        {
            throw new NotImplementedException();
        }

        public void AddLineStations(AdjacentStations adjacentStations)
        {
            throw new NotImplementedException();
        }

        public void AddLineTrip(LineTrip lineTrip)
        {
            throw new NotImplementedException();
        }

        

        public void AddTrip(Trip trip)
        {
            throw new NotImplementedException();
        }

        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteAdjacentStationse(int Scode1, int Scode2)
        {
            throw new NotImplementedException();
        }

        public void DeleteAdjacentStationseBStation(int Scode1)
        {
            throw new NotImplementedException();
        }

      
     
        public void DeleteLineTrip(int idline)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineTrip1(LineTrip lineTrip)
        {
            throw new NotImplementedException();
        }

        public void DeleteStations(int code)
        {
            throw new NotImplementedException();
        }

        public int DeleteStationsFromLine(int Scode, int idline)
        {
            throw new NotImplementedException();
        }

        public void DeleteStationsFromLine(int Scode)
        {
            throw new NotImplementedException();
        }

        public void DeleteStationsOfLine(int idline)
        {
            throw new NotImplementedException();
        }

        public void DeleteTrip(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string name)
        {
            throw new NotImplementedException();
        }

        public AdjacentStations GetAdjacentStations(int Scode1, int Scode2)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AdjacentStations> GetAllAdjacentStations(int stationCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AdjacentStations> GetAllAdjacentStationsBy(Predicate<AdjacentStations> StationsLinecondition)
        {
            throw new NotImplementedException();
        }




  
        public IEnumerable<LineStation> GetAllLineAt2Stations(int code1, int cod2)
        {
            throw new NotImplementedException();
        }

      
  

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> StationsLinecondition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LineTrip> GetAllLineTripsBy(Predicate<LineTrip> StationsLinecondition)
        {
            throw new NotImplementedException();
        }

      

   

        public IEnumerable<LineStation> GetAllStationsCode(int code)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LineStation> GetAllStationsLine(int idline)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Trip> GetAllTrip()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LineTrip> GetAllTripline(int idline)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Trip> GetAllTripLine(int line)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Trip> GetAllTripsBy(Predicate<Trip> Tripcondition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAlluser()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAlluserAdmin()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAlluserBy(Predicate<User> userConditions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAlluserNAdmin()
        {
            throw new NotImplementedException();
        }


     

        public IEnumerable<object> GetLineFields(Func<int, bool, object> generate)
        {
            throw new NotImplementedException();
        }

        public LineStation GetLineStation(int Scode, int idline)
        {
            throw new NotImplementedException();
        }

        public LineTrip GetLineTrip(TimeSpan start, int idline)
        {
            throw new NotImplementedException();
        }

       

        public Trip GetTrip(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string name)
        {
            throw new NotImplementedException();
        }

        public User getUserBy(Predicate<User> userConditions)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdjacentStations(AdjacentStations adjacentStations)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdjacentStations(int code1, int code2, int codeChange, int oldCode)
        {
            throw new NotImplementedException();
        }

    
      

        public void UpdateLineStations(LineStation linestations)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineStationsCode(LineStation linestations, int oldCode)
        {
            throw new NotImplementedException();
        }

        public void UpdatelineTrip(LineTrip lineTrip)
        {
            throw new NotImplementedException();
        }

        public void UpdateStations(Stations stations, int oldCode)
        {
            throw new NotImplementedException();
        }

        public void UpdateStations(Trip trip)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
