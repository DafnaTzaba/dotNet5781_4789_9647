﻿using DALAPI;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
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
        string lineStationPath = @"lineStationXml.xml"; //XMLSerializer
        string lineTripPath = @"lineTripXml.xml"; //XMLSerializer
        string adjacentStationsPath = @"AdjacentStationsXml.xml"; //XMLSerializer
        string userPath = @"UserXml.xml"; //XMLSerializer
        #endregion


        #region Bus
        public Bus GetBus(string licence)
        {
            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }

            return null;
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            #region xml
            //List<Bus> ListLine = new List<Bus>
            //{
                
            //    #region line

            //     new Bus
            //    {
            //        Licence="5267008",
            //        StartingDate= new DateTime(2013, 02, 05),
            //        Kilometrz=22000,
            //        KilometrFromLastTreat=2000,
            //        FuellAmount=200,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        LastTreatment=new DateTime(2020,12,12),
            //        BusExist=true
            //    },


            //    new Bus
            //    {
            //        Licence="2784562",
            //        StartingDate= new DateTime(2014, 03, 05),
            //        Kilometrz=22000,
            //        KilometrFromLastTreat=2000,
            //        FuellAmount=700,
            //           LastTreatment=new DateTime(2020,12,12),
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true

            //    },
            //    new Bus
            //    {
            //        Licence="12345678",
            //        StartingDate= new DateTime(2019, 02, 05),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=10000,
            //        KilometrFromLastTreat=10000,
            //        FuellAmount=340,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //    new Bus
            //    {
            //        //
            //        Licence="5267408",
            //        StartingDate= new DateTime(2013, 02, 05),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=22000,
            //        KilometrFromLastTreat=2000,
            //        FuellAmount=200,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //    new Bus
            //    {
            //        Licence="1234567",
            //        StartingDate= new DateTime(2013, 09, 21),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=70000,
            //        KilometrFromLastTreat=1500.34,
            //        FuellAmount=643.98,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //    new Bus
            //    {
            //        Licence="7654321",
            //        StartingDate= new DateTime(2013, 02, 05),
            //        Kilometrz=22000,
            //           LastTreatment=new DateTime(2020,12,12),
            //        KilometrFromLastTreat=2000,
            //        FuellAmount=200,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //    new Bus
            //    {
            //        Licence="5463728",
            //        StartingDate= new DateTime(2013, 06, 20),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=22000,
            //        KilometrFromLastTreat=78500,
            //        FuellAmount=350,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //    new Bus
            //    {
            //        Licence="8216542",
            //        StartingDate= new DateTime(2010, 04, 15),
            //        Kilometrz=100000,
            //           LastTreatment=new DateTime(2020,12,12),
            //        KilometrFromLastTreat=15000,
            //        FuellAmount=900,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //    new Bus
            //    {
            //        Licence="34509814",
            //        StartingDate= new DateTime(2019, 02, 20),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=10500,
            //        KilometrFromLastTreat=1400,
            //        FuellAmount=300,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //    new Bus
            //    {
            //        Licence="10926574",
            //        StartingDate= new DateTime(2020, 04, 15),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=100000,
            //        KilometrFromLastTreat=15000,
            //        FuellAmount=900,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //   new Bus
            //    {
            //        Licence="1192657",
            //        StartingDate= new DateTime(2010, 12, 15),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=28970,
            //        KilometrFromLastTreat=8970,
            //        FuellAmount=1000,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            // new Bus
            //    {
            //        Licence="1265473",
            //        StartingDate= new DateTime(2009, 07, 18),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=20000,
            //        KilometrFromLastTreat=100,
            //        FuellAmount=900,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //          BusExist=true
            //    },
            //    new Bus
            //    {
            //        Licence="89712365",
            //        StartingDate= new DateTime(2020, 03, 15),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=100000,
            //        KilometrFromLastTreat=15000,
            //        FuellAmount=900,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //    new Bus
            //    {
            //        Licence="1778328",
            //        StartingDate= new DateTime(2010, 02, 15),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=100000,
            //        KilometrFromLastTreat=15000,
            //        FuellAmount=900,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //  new Bus
            //    {
            //        Licence="5059589",
            //        StartingDate= new DateTime(1999, 04, 15),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=100000,
            //        KilometrFromLastTreat=15000,
            //        FuellAmount=900,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //                                                                                                              new Bus
            //    {
            //        Licence="12845999",
            //        StartingDate= new DateTime(2020, 01, 15),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=100000,
            //        KilometrFromLastTreat=15000,
            //        FuellAmount=900,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //                                                                                                              new Bus
            //    {
            //        Licence="2000000",
            //           LastTreatment=new DateTime(2020,12,12),
            //        StartingDate= new DateTime(2012, 07, 25),
            //        Kilometrz=109283,
            //        KilometrFromLastTreat=15000,
            //        FuellAmount=900,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //                                                                                                          new Bus
            //    {
            //        Licence="11119999",
            //        StartingDate= new DateTime(2020, 08, 15),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=100000,
            //        KilometrFromLastTreat=15000,
            //        FuellAmount=900,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },
            //                                                                                                                new Bus
            //    {
            //        Licence="8576669",
            //        StartingDate= new DateTime(2017, 04, 15),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=100000,
            //        KilometrFromLastTreat=15000,
            //        FuellAmount=900,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //         BusExist=true
            //    },
            //                                                                                                              new Bus
            //    {
            //       Licence="10928300",
            //        StartingDate= new DateTime(2020, 09, 22),
            //           LastTreatment=new DateTime(2020,12,12),
            //        Kilometrz=100000,
            //        KilometrFromLastTreat=15000,
            //        FuellAmount=900,
            //        StatusBus=DO.STUTUS.READT_TO_TRAVEL,
            //        BusExist=true
            //    },

            //    #endregion
            //};



            //XMLTools.SaveListToXMLSerializer(ListLine, busPath);
            #endregion



            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;

        }

        public IEnumerable<Bus> GetAllBusesStusus(STUTUS stusus)
        {
            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> buscondition)
        {
            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }

        public int AddBus(Bus bus)
        {
            try
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
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return 1;

        }

        public bool DeleteBus(string licence)
        {
            try
            {
                XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath); //get the data from xml
                XElement bus = (from b in busRootElem.Elements()
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return false;

        }

        public bool UpdateBus(Bus bus)
        {
            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return false;

        }



        #endregion

        #region Line

        public Line GetLine(int idline)
        {
            try
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
                                 Area = (AREA)Enum.Parse(typeof(AREA), line.Element("Area").Value),
                                 LineExist = Convert.ToBoolean(line.Element("LineExist").Value)

                             }
                ).FirstOrDefault();

                if (b == null)
                    throw new DO.WrongIDExeption(idline, "  הקו המבוקש לא נמצא במערכת");
                return b;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;

        }

        public IEnumerable<Line> GetAllLineBy(Predicate<Line> linecondition)
        {
            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;

        }

        public IEnumerable<Line> GetAllLine()
        {
            #region xml
            //List<Line> ListLine = new List<Line>
            //{
            //    #region line
            //    new Line
            //    {
            //    IdNumber = 1,
            //    NumberLine=10,
            //    FirstStationCode=73,
            //    LastStationCode=89,
            //    Area=DO.AREA.CENTER,

            //    LineExist=true,
            //    },

            //    new Line
            //    {
            //    IdNumber = 2,
            //    NumberLine=27,
            //    FirstStationCode=85,
            //    LastStationCode=97,
            //    Area=DO.AREA.GENERAL,
            //    LineExist=true,
            //    },


            //    new Line
            //    {
            //    IdNumber =3,
            //    NumberLine=5,
            //    FirstStationCode=122,
            //    LastStationCode=1511,
            //    Area=DO.AREA.JERUSALEM,
            //    LineExist=true,
            //    },
            //    new Line
            //    {
            //    IdNumber = 4,
            //    NumberLine=6,
            //    FirstStationCode=121,
            //    LastStationCode=1491,
            //    Area=DO.AREA.NORTH,
            //    LineExist=true,
            //    },
            //    new Line
            //    {
            //    IdNumber = 5,
            //    NumberLine=33,
            //    FirstStationCode=119,
            //    LastStationCode=1491,
            //    Area=DO.AREA.SOUTH,
            //    LineExist=true,
            //    },
            //    new Line
            //    {
            //    IdNumber = 6,
            //    NumberLine=67,
            //    FirstStationCode=110,
            //    LastStationCode=1486,
            //    Area=DO.AREA.YOSH,
            //    LineExist=true,
            //    },
            //    new Line
            //    {
            //    IdNumber = 7,
            //    NumberLine=24,
            //    FirstStationCode=97,
            //    LastStationCode=111,
            //    Area=DO.AREA.JERUSALEM,
            //    LineExist=true,
            //    },
            //    new Line
            //    {
            //    IdNumber =8,
            //    NumberLine=20,
            //    FirstStationCode=102,
            //    LastStationCode=116,
            //    Area=DO.AREA.JERUSALEM,
            //    LineExist=true,
            //    },
            //    new Line
            //    {
            //    IdNumber = 9,
            //    NumberLine=507,
            //    FirstStationCode=85,
            //    LastStationCode=102,
            //    Area=DO.AREA.JERUSALEM,
            //    LineExist=true,
            //    },
            //    new Line
            //    {
            //    IdNumber = 10,
            //    NumberLine=21,
            //    FirstStationCode=111,
            //    LastStationCode=1488,
            //    Area=DO.AREA.JERUSALEM,
            //    LineExist=true,
            //    }


            //    #endregion
            //};
            //XMLTools.SaveListToXMLSerializer(ListLine, linePath);
            #endregion

            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }

        public IEnumerable<Line> GetAllLinesArea(AREA area)
        {
            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }

        public int AddLine(Line line)
        {  //eliezer told us that we can do here not checking because check according to idnumber is meaningless



            try
            {
                serials = XMLTools.LoadListFromXMLElement(@"serials.xml");
                int idLine = int.Parse(serials.Element("LineCounter").Value);
                serials.Element("LineCounter").Value = (++idLine).ToString();

                XElement lineRootElem = XMLTools.LoadListFromXMLElement(linePath);



                XElement lineToAdd = new XElement("Line",
                       new XElement("NumberLine", line.NumberLine),
                       new XElement("FirstStationCode", line.FirstStationCode),
                       new XElement("LastStationCode", line.LastStationCode),
                       new XElement("LineExist", line.LineExist),
                       new XElement("IdNumber", idLine),
                       new XElement("Area", line.Area.ToString()));

                lineRootElem.Add(lineToAdd);
                XMLTools.SaveListToXMLElement(lineRootElem, linePath);
                XMLTools.SaveListToXMLElement(serials, @"serials.xml");

                return idLine;
            }
            catch (DO.XMLFileLoadCreateException) { }
            return -1;
        }

        public void UpdateLine(Line line)
        {
            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }


        }

        public void DeleteLine(int idnumber)
        {
            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }


        }



        #endregion


        #region Station

        public Stations GetStations(int code)
        {
            try
            {


                XElement stationRootElem = XMLTools.LoadListFromXMLElement(stationPath); //get the data from xml
                DO.Stations b = (from station in stationRootElem.Elements()
                                 where station.Element("Code").Value == code.ToString() && Convert.ToBoolean(station.Element("StationExist").Value) == true
                                 select new DO.Stations()
                                 {
                                     Code = Convert.ToInt32(station.Element("Code").Value),
                                     Name = station.Element("Name").Value,
                                     Address = station.Element("Address").Value,
                                     Latitude = Convert.ToDouble(station.Element("Latitude").Value),
                                     Longtitude = Convert.ToDouble(station.Element("Longtitude").Value),
                                     StationExist = Boolean.Parse(station.Element("StationExist").Value)

                                 }
                ).FirstOrDefault();

                if (b == null)
                    throw new DO.WrongLicenceException(code, "התחנה המבוקשת לא נמצאת במערכת");
                return b;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;


        }

        public IEnumerable<Stations> GetAllStations()
        {
            #region xml

        //    List<Stations> ListStations1 = new List<Stations>
        //        {
        //            #region initialization stations//איתחול תחנות
        //           new Stations
        //        {
        //            Code = 73,
        //            Name = "שדרות גולדה מאיר/המשורר אצ''ג",
        //            Address = "שדרות גולדה מאיר ,ירושלים ",
        //            Latitude=31.825302,
        //            Longtitude=35.1624,
        //            StationExist=true,

        //        },
        //        new Stations
        //        {
        //            Code = 76,
        //            Name = "בית ספר צור באהר בנות/אלמדינה אלמונוורה",
        //            Address = "רחוב:אל מדינה אל מונאוורה  עיר: ירושלים",
        //            Latitude=31.738425,
        //            Longtitude=35.228765,
        //             StationExist=true,
        //        },
        //        new Stations
        //        {
        //            Code = 77,
        //            Name = "בית ספר אבן רשד/אלמדינה אלמונוורה",
        //            Address = "רחוב:אל מדינה אל מונאוורה  עיר: ירושלים ",
        //             Latitude=31.738676,
        //            Longtitude=35.226704,

        //            StationExist=true,
        //        },
        //        new Stations
        //        {
        //            Code = 78,
        //            Name = "שרי ישראל/יפו",
        //            Address = "רחוב:שדרות שרי ישראל 15 עיר: ירושלים",
        //             Latitude=31.789128,
        //            Longtitude=35.206146,

        //            StationExist=true,
        //        },
        //        new Stations
        //        {
        //            Code = 83,
        //            Name = "בטן אלהווא/חוש אל מרג",
        //            Address = "רחוב:בטן אל הווא  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.766358,
        //            Longtitude=35.240417,


        //        },
        //        new Stations
        //        {
        //            Code = 84,
        //            Name = "מלכי ישראל/הטורים",
        //            Address = " רחוב:מלכי ישראל 77 עיר: ירושלים ",
        //            StationExist=true,
        //             Latitude=31.790758,
        //            Longtitude=35.209791,


        //        },
        //        new Stations
        //        {
        //            Code = 85,
        //            Name = "בית ספר לבנים/אלמדארס",
        //            Address = "רחוב:אלמדארס  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.768643,
        //            Longtitude=35.238509,


        //        },
        //        new Stations
        //        {
        //            Code = 86,
        //            Name = "מגרש כדורגל/אלמדארס",
        //            Address = "רחוב:אלמדארס  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.769899,
        //            Longtitude=35.23973,


        //        },
        //        new Stations
        //        {
        //            Code = 88,
        //            Name = "בית ספר לבנות/בטן אלהוא",
        //            Address = " רחוב:בטן אל הווא  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.767064,
        //            Longtitude=35.238443,


        //        },
        //        new Stations
        //        {
        //            Code = 89,
        //            Name = "דרך בית לחם הישה/ואדי קדום",
        //            Address = " רחוב:דרך בית לחם הישנה  עיר: ירושלים ",
        //            StationExist=true,
        //             Latitude=31.765863,
        //            Longtitude=35.247198,


        //        },
        //        new Stations
        //        {
        //            Code = 90,
        //            Name = "גולדה/הרטום",
        //            Address = "רחוב:דרך בית לחם הישנה  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.799804,
        //            Longtitude=35.213021,


        //        },
        //        new Stations
        //        {
        //            Code = 91,
        //            Name = "דרך בית לחם הישה/ואדי קדום",
        //            Address = " רחוב:דרך בית לחם הישנה  עיר: ירושלים ",
        //            StationExist=true,
        //             Latitude=31.765717,
        //            Longtitude=35.247102,


        //        },
        //        new Stations
        //        {
        //            Code = 93,
        //            Name = "חוש סלימה 1",
        //            Address = " רחוב:דרך בית לחם הישנה  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.767265,
        //            Longtitude=35.246594,

        //        },
        //        new Stations
        //        {
        //            Code = 94,
        //            Name = "דרך בית לחם הישנה ב",
        //            Address = " רחוב:דרך בית לחם הישנה  עיר: ירושלים",
        //            StationExist=true,
        //            Latitude=31.767084,
        //            Longtitude=35.246655,


        //        },
        //        new Stations
        //        {
        //            Code = 95,
        //            Name = "דרך בית לחם הישנה א",
        //            Address = " רחוב:דרך בית לחם הישנה  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.768759,
        //            Longtitude=35.24368



        //        },
        //        new Stations
        //        {
        //            Code = 97,
        //            Name = "שכונת בזבז 2",
        //            Address = " רחוב:דרך בית לחם הישנה  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.77002,
        //            Longtitude=35.24348,

        //        },
        //        new Stations
        //        {
        //            Code = 102,
        //            Name = "גולדה/שלמה הלוי",
        //            Address = " רחוב:שדרות גולדה מאיר  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.8003,
        //            Longtitude=35.208257,


        //        },
        //        new Stations
        //        {
        //            Code = 103,
        //            Name = "גולדה/הרטום",
        //            Address = " רחוב:שדרות גולדה מאיר  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.8,
        //            Longtitude=35.214106,


        //        },
        //        new Stations
        //        {
        //            Code = 105,
        //            Name = "גבעת משה",
        //            Address = " רחוב:גבעת משה 2 עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.797708,
        //            Longtitude=35.217133,

        //        },
        //        new Stations
        //        {
        //            Code = 106,
        //            Name = "גבעת משה",
        //            Address = " רחוב:גבעת משה 3 עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.797535,
        //            Longtitude=35.217057,


        //        },
        //        //20
        //        new Stations
        //        {
        //            Code = 108,
        //            Name = "עזרת תורה/עלי הכהן",
        //            Address = "  רחוב:עזרת תורה 25 עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.797535,
        //            Longtitude=35.213728,


        //        },
        //        new Stations
        //        {
        //            Code = 109,
        //            Name = "עזרת תורה/דורש טוב",
        //            Address = "  רחוב:עזרת תורה 21 עיר: ירושלים ",
        //            StationExist=true,
        //             Latitude=31.796818,
        //            Longtitude=35.212936,


        //        },
        //        new Stations
        //        {
        //            Code = 110,
        //            Name = "עזרת תורה/דורש טוב",
        //            Address = " רחוב:עזרת תורה 12 עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.796129,
        //            Longtitude=35.212698,


        //        },
        //        new Stations
        //        {
        //            Code = 111,
        //            Name = "יעקובזון/עזרת תורה",
        //            Address = "  רחוב:יעקובזון 1 עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.794631,
        //            Longtitude=35.21161,


        //        },
        //        new Stations
        //        {
        //            Code = 112,
        //            Name = "יעקובזון/עזרת תורה",
        //            Address = " רחוב:יעקובזון  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.79508,
        //            Longtitude=35.211684,


        //        },
        //        //25
        //        new Stations
        //        {
        //            Code = 113,
        //            Name = "זית רענן/אוהל יהושע",
        //            Address = "  רחוב:זית רענן 1 עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.796255,
        //            Longtitude=35.211065,


        //        },
        //        new Stations
        //        {
        //            Code = 115,
        //            Name = "זית רענן/תורת חסד",
        //            Address = " רחוב:זית רענן  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.798423,
        //            Longtitude=35.209575,


        //        },
        //        new Stations
        //        {
        //            Code = 116,
        //            Name = "זית רענן/תורת חסד",
        //            Address = "  רחוב:הרב סורוצקין 48 עיר: ירושלים ",
        //            StationExist=true,
        //             Latitude=31.798689,
        //            Longtitude=35.208878,


        //        },
        //        new Stations
        //        {
        //            Code = 117,
        //            Name = "קרית הילד/סורוצקין",
        //            Address = "  רחוב:הרב סורוצקין  עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.799165,
        //            Longtitude=35.206918,

        //        },
        //        new Stations
        //        {
        //            Code = 119,
        //            Name = "סורוצקין/שנירר",
        //            Address = "  רחוב:הרב סורוצקין 31 עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.797829,
        //            Longtitude=35.205601,


        //        },

        //        //#endregion //30
        //        new Stations
        //        {
        //            Code = 1485,
        //            Name = "שדרות נווה יעקוב/הרב פרדס ",
        //            Address = "רחוב: שדרות נווה יעקוב  עיר:ירושלים ",
        //            StationExist=true,
        //             Latitude=31.840063,
        //            Longtitude=35.240062,



        //        },
        //        new Stations
        //        {
        //            Code = 1486,
        //            Name = "מרכז קהילתי /שדרות נווה יעקוב",
        //            Address = "רחוב:שדרות נווה יעקוב ירושלים עיר:ירושלים ",
        //            StationExist=true,
        //             Latitude=31.838481,
        //            Longtitude=35.23972,


        //        },


        //        new Stations
        //        {
        //            Code = 1487,
        //            Name = " מסוף 700 /שדרות נווה יעקוב ",
        //            StationExist=true,
        //    Address = "רחוב:שדרות נווה יעקב 7 עיר: ירושלים  ",
        //     Latitude=31.837748,
        //            Longtitude=35.231598,


        //        },
        //        new Stations
        //        {
        //            Code = 1488,
        //            Name = " הרב פרדס/אסטורהב ",
        //            StationExist=true,
        //            Address = "רחוב:מעגלות הרב פרדס  עיר: ירושלים רציף  ",
        //             Latitude=31.840279,
        //            Longtitude=35.246272,

        //        },
        //        new Stations
        //        {
        //            Code = 1490,
        //            Name = "הרב פרדס/צוקרמן ",
        //            Address = "רחוב:מעגלות הרב פרדס 24 עיר: ירושלים   ",
        //            StationExist=true,
        //             Latitude=31.843598,
        //            Longtitude=35.243639,

        //        },
        //        new Stations
        //        {
        //            Code = 1491,
        //            Name = "ברזיל ",
        //            Address = "רחוב:ברזיל 14 עיר: ירושלים",
        //            StationExist=true,
        //             Latitude=31.766256,
        //            Longtitude=35.173,

        //        },
        //        new Stations
        //        {
        //            Code = 1492,
        //            Name = "בית וגן/הרב שאג ",
        //            Address = "רחוב:בית וגן 61 עיר: ירושלים ",
        //            StationExist=true,
        //             Latitude=31.76736,
        //            Longtitude=35.184771,

        //        },
        //        new Stations
        //        {
        //            Code = 1493,
        //            Name = "בית וגן/עוזיאל ",
        //            Address = "רחוב:בית וגן 21 עיר: ירושלים    ",
        //            StationExist=true,
        //             Latitude=31.770543,
        //            Longtitude=35.183999,

        //        },
        //        new Stations
        //        {
        //            Code = 1494,
        //            Name = " קרית יובל/שמריהו לוין ",
        //            Address = "רחוב:ארתור הנטקה  עיר: ירושלים    ",
        //            StationExist=true,
        //             Latitude=31.768465,
        //            Longtitude=35.178701,

        //        },
        //        new Stations
        //        {
        //            Code = 1510,
        //            Name = " קורצ'אק / רינגלבלום ",
        //            StationExist=true,
        //            Address = "רחוב:יאנוש קורצ'אק 7 עיר: ירושלים",
        //             Latitude=31.759534,
        //            Longtitude=35.173688,

        //        },
        //        new Stations
        //        {
        //            Code = 1511,
        //            Name = " טהון/גולומב ",
        //            Address = "רחוב:יעקב טהון  עיר: ירושלים     ",
        //            StationExist=true,
        //             Latitude=31.761447,
        //            Longtitude=35.175929,

        //        },
        //        new Stations
        //        {
        //            Code = 1512,
        //            Name = "הרב הרצוג/שח''ל ",
        //            Address = "רחוב:הרב הרצוג  עיר: ירושלים רציף",
        //            StationExist=true,

        //             Latitude=31.761447,
        //            Longtitude=35.199936,

        //        },
        //        new Stations
        //        {
        //            Code = 1514,
        //            Name = "פרץ ברנשטיין/נזר דוד ",
        //            Address = "רחוב:הרב הרצוג  עיר: ירושלים רציף",
        //            StationExist=true,

        //             Latitude=31.759186,
        //            Longtitude=35.189336,
        //        },


        //     new Stations
        //    {
        //    Code = 1518,
        //    Name = "פרץ ברנשטיין/נזר דוד",
        //    Address = " רחוב:פרץ ברנשטיין 56 עיר: ירושלים ",
        //    StationExist=true,

        //     Latitude=31.759121,
        //            Longtitude=35.189178,

        //},
        //      new Stations
        //      {
        //    Code = 1522,
        //    Name = "מוזיאון ישראל/רופין",
        //    Address = "  רחוב:דרך רופין  עיר: ירושלים ",
        //    StationExist=true,

        //     Latitude=31.774484,
        //            Longtitude=35.204882,

        //        },

        //     new Stations
        //          {
        //     Code = 1523,
        //    Name = "הרצוג/טשרניחובסקי",
        //    Address = "   רחוב:הרב הרצוג 21  עיר: ירושלים  ",
        //    StationExist=true,

        //     Latitude=31.769609,
        //            Longtitude=35.209732,

        //        },
        //      new Stations
        //        {
        //      Code = 1524,
        //    Name = "רופין/שד' הזז",
        //    Address = "    רחוב:הרב הרצוג  עיר: ירושלים   ",
        //    StationExist=true,
        //     Latitude=31.769652,
        //            Longtitude=35.208248,

        //         },
        //        new Stations
        //        {
        //            Code = 121,
        //            Name = "מרכז סולם/סורוצקין ",
        //            Address = " רחוב:הרב סורוצקין 13 עיר: ירושלים",
        //            StationExist=true,

        //             Latitude=31.796033,
        //            Longtitude=35.206094,

        //        },
        //        new Stations
        //        {
        //            Code = 123,
        //            Name = "אוהל דוד/סורוצקין ",
        //            Address = "  רחוב:הרב סורוצקין 9 עיר: ירושלים",
        //            StationExist=true,

        //             Latitude=31.794958,
        //            Longtitude=35.205216,

        //        },
        //        new Stations
        //        {
        //            Code = 122,
        //            Name = "מרכז סולם/סורוצקין ",
        //            Address = "  רחוב:הרב סורוצקין 28 עיר: ירושלים",
        //            StationExist=true,

        //             Latitude=31.79617,
        //            Longtitude=35.206158,

        //        }



        //            #endregion
        //        };
        //    XMLTools.SaveListToXMLSerializer(ListStations1, stationPath);
            #endregion
            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }

        public IEnumerable<Stations> GetAllStationsBy(Predicate<Stations> Stationscondition)
        {
            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }

        public void AddStations(Stations station)
        {
            try
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
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
        }


        public void DeleteStations(int code)
        {
            try
            {


                List<Stations> ListStation = XMLTools.LoadListFromXMLSerializer<Stations>(stationPath);
                DO.Stations sta = ListStation.Find(p => p.Code == code && p.StationExist == true);
                if (sta != null)
                {
                    sta.StationExist = false;
                }
                else
                    throw new DO.WrongIDExeption(code, "לא נמצאו פרטים עבור התחנה המבוקשת");

                XMLTools.SaveListToXMLSerializer(ListStation, stationPath);

            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
        }

        public void UpdateStations(Stations stations, int oldCode)
        {
            try
            {


                List<Stations> ListStation = XMLTools.LoadListFromXMLSerializer<Stations>(stationPath);
                DO.Stations sta = ListStation.Find(p => p.Code == oldCode && p.StationExist == true);
                if (sta != null)
                {
                    ListStation.Remove(sta);
                    ListStation.Add(stations);
                }
                else
                    throw new DO.WrongIDExeption(oldCode, "לא נמצאו פרטים עבור התחנה המבוקשת");
                XMLTools.SaveListToXMLSerializer(ListStation, stationPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
        }

        #endregion


        #region LineStation


        public LineStation GetLineStation(int Scode, int idline)
        {
            try
            {


                List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
                DO.LineStation sta = ListLineStation.Find((b => b.StationCode == Scode && b.LineId == idline && b.LineStationExist == true));
                if (sta != null)
                    return sta;
                else
                    throw new DO.WrongIDExeption(Scode, "לא נמצאו פרטים עבור התחנה המבוקשת");
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }


        public IEnumerable<LineStation> GetAllStationsLine(int idline)
        {

            try
            {


                List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);

                return from station in ListLineStation
                       where (station.LineId == idline && station.LineStationExist)
                       select station;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }



        public IEnumerable<LineStation> GetAllStationsCodeline(int code)
        {
            try
            {


                List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);

                return from station in ListLineStation
                       where (station.StationCode == code)
                       select station;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> StationsLinecondition)
        {

            #region

            //List<LineStation> ListLineStations = new List<LineStation>
            //  {
            //       line number 18
            //    new LineStation
            //    {
            //        LineId=1,
            //        StationCode=73,
            //        LineStationIndex=1,
            //        LineStationExist=true,
            //        PrevStation=0,
            //        NextStation=76,
            //    },
            //    new LineStation
            //    {
            //        LineId=1,
            //        StationCode=76,
            //        LineStationIndex=2,
            //        LineStationExist=true,
            //        PrevStation=73,
            //        NextStation=77,
            //    },
            //    new LineStation
            //    {
            //        LineId=1,
            //        StationCode=77,
            //        LineStationIndex=3,
            //        LineStationExist=true,
            //        PrevStation=76,
            //        NextStation=78,
            //    },
            //    new LineStation
            //    {
            //        LineId=1,
            //        StationCode=78,
            //        LineStationIndex=4,
            //        LineStationExist=true,
            //        PrevStation=77,
            //        NextStation=83,
            //    },
            //    new LineStation
            //    {
            //        LineId=1,
            //        StationCode=83,
            //        LineStationIndex=5,
            //        LineStationExist=true,
            //        PrevStation=78,
            //        NextStation=84,
            //    },
            //    new LineStation
            //    {
            //        LineId=1,
            //        StationCode=84,
            //        LineStationIndex=6,
            //        LineStationExist=true,
            //        PrevStation=83,
            //        NextStation=85,
            //    },
            //    new LineStation
            //    {
            //        LineId=1,
            //        StationCode=85,
            //        LineStationIndex=7,
            //        LineStationExist=true,
            //        PrevStation=84,
            //        NextStation=86,
            //    },
            //    new LineStation
            //    {
            //        LineId=1,
            //        StationCode=86,
            //        LineStationIndex=8,
            //        LineStationExist=true,
            //        PrevStation=85,
            //        NextStation=88,
            //    },
            //    new LineStation
            //    {
            //        LineId=1,
            //        StationCode=88,
            //        LineStationIndex=9,
            //        LineStationExist=true,
            //        PrevStation=86,
            //        NextStation=89,
            //    },
            //    new LineStation
            //    {
            //        LineId=1,
            //        StationCode=89,
            //        LineStationIndex=10,
            //        LineStationExist=true,
            //        PrevStation=88,
            //        NextStation=0,
            //    },

            // line 10
            //    new LineStation
            //    {
            //        LineId=2,
            //        StationCode=85,
            //        LineStationIndex=1,
            //        LineStationExist=true,
            //        PrevStation=0,
            //        NextStation=86,
            //    },
            //    new LineStation
            //    {
            //        LineId=2,
            //        StationCode=86,
            //        LineStationIndex=2,
            //        LineStationExist=true,
            //        PrevStation=85,
            //        NextStation=88,
            //    },
            //    new LineStation
            //    {
            //        LineId=2,
            //        StationCode=88,
            //        LineStationIndex=3,
            //        LineStationExist=true,
            //        PrevStation=86,
            //        NextStation=89,
            //    },
            //    new LineStation
            //    {
            //        LineId=2,
            //        StationCode=89,
            //        LineStationIndex=4,
            //        LineStationExist=true,
            //        PrevStation=88,
            //        NextStation=90,
            //    },
            //    new LineStation
            //    {
            //        LineId=2,
            //        StationCode=90,
            //        LineStationIndex=5,
            //        LineStationExist=true,
            //        PrevStation=89,
            //        NextStation=91,
            //    },
            //    new LineStation
            //    {
            //        LineId=2,
            //        StationCode=91,
            //        LineStationIndex=6,
            //        LineStationExist=true,
            //        PrevStation=90,
            //        NextStation=93,
            //    },
            //    new LineStation
            //    {
            //        LineId=2,
            //        StationCode=93,
            //        LineStationIndex=7,
            //        LineStationExist=true,
            //        PrevStation=91,
            //        NextStation=94,
            //    },
            //    new LineStation
            //    {
            //        LineId=2,
            //        StationCode=94,
            //        LineStationIndex=8,
            //        LineStationExist=true,
            //        PrevStation=93,
            //        NextStation=95,
            //    },
            //    new LineStation
            //    {
            //        LineId=2,
            //        StationCode=95,
            //        LineStationIndex=9,
            //        LineStationExist=true,
            //        PrevStation=94,
            //        NextStation=97,
            //    },
            //    new LineStation
            //    {
            //        LineId=2,
            //        StationCode=97,
            //        LineStationIndex=10,
            //        LineStationExist=true,
            //        PrevStation=95,
            //        NextStation=0,
            //    },
            // line 5
            //    new LineStation
            //    {
            //        LineId=3,
            //        StationCode=122,
            //        LineStationIndex=1,
            //        LineStationExist=true,
            //        PrevStation=0,
            //        NextStation=123,
            //    },
            //    new LineStation
            //    {
            //        LineId=3,
            //        StationCode=123,
            //        LineStationIndex=2,
            //        LineStationExist=true,
            //        PrevStation=122,
            //        NextStation=121,
            //    },
            //    new LineStation
            //    {
            //        LineId=3,
            //        StationCode=121,
            //        LineStationIndex=3,
            //        LineStationExist=true,
            //        PrevStation=123,
            //        NextStation=1524,
            //    },
            //    new LineStation
            //    {
            //        LineId=3,
            //        StationCode=1524,
            //        LineStationIndex=4,
            //        LineStationExist=true,
            //        PrevStation=121,
            //        NextStation=1523,
            //    },
            //    new LineStation
            //    {
            //        LineId=3,
            //        StationCode=1523,
            //        LineStationIndex=5,
            //        LineStationExist=true,
            //        PrevStation=1524,
            //        NextStation=1522,
            //    },
            //    new LineStation
            //    {
            //        LineId=3,
            //        StationCode=1522,
            //        LineStationIndex=6,
            //        LineStationExist=true,
            //        PrevStation=1523,
            //        NextStation=1518,
            //    },
            //    new LineStation
            //    {
            //        LineId=3,
            //        StationCode=1518,
            //        LineStationIndex=7,
            //        LineStationExist=true,
            //        PrevStation=1522,
            //        NextStation=1514,
            //    },
            //    new LineStation
            //    {
            //        LineId=3,
            //        StationCode=1514,
            //        LineStationIndex=8,
            //        LineStationExist=true,
            //        PrevStation=1518,
            //        NextStation=1512,
            //    },
            //    new LineStation
            //    {
            //        LineId=3,
            //        StationCode=1512,
            //        LineStationIndex=9,
            //        LineStationExist=true,
            //        PrevStation=1514,
            //        NextStation=1511,
            //    },
            //    new LineStation
            //    {
            //        LineId=3,
            //        StationCode=1511,
            //        LineStationIndex=10,
            //        LineStationExist=true,
            //        PrevStation=1512,
            //        NextStation=0,
            //    },

            //    line=6
            //    new LineStation
            //    {
            //        LineId=4,
            //        StationCode=121,
            //        LineStationIndex=1,
            //        LineStationExist=true,
            //        PrevStation=0,
            //        NextStation=123,
            //    },
            //    new LineStation
            //    {
            //        LineId=4,
            //        StationCode=123,
            //        LineStationIndex=2,
            //        LineStationExist=true,
            //        PrevStation=121,
            //        NextStation=122,
            //    },
            //    new LineStation
            //    {
            //        LineId=4,
            //        StationCode=122,
            //        LineStationIndex=3,
            //        LineStationExist=true,
            //        PrevStation=123,
            //        NextStation=1524,
            //    },
            //    new LineStation
            //    {
            //        LineId=4,
            //        StationCode=1524,
            //        LineStationIndex=4,
            //        LineStationExist=true,
            //        PrevStation=122,
            //        NextStation=1523,
            //    },
            //    new LineStation
            //    {
            //        LineId=4,
            //        StationCode=1523,
            //        LineStationIndex=5,
            //        LineStationExist=true,
            //        PrevStation=1524,
            //        NextStation=1522,
            //    },
            //    new LineStation
            //    {
            //        LineId=4,
            //        StationCode=1522,
            //        LineStationIndex=6,
            //        LineStationExist=true,
            //        PrevStation=1523,
            //        NextStation=1518,
            //    },
            //    new LineStation
            //    {
            //        LineId=4,
            //        StationCode=1518,
            //        LineStationIndex=7,
            //        LineStationExist=true,
            //        PrevStation=1522,
            //        NextStation=1514,
            //    },
            //    new LineStation
            //    {
            //        LineId=4,
            //        StationCode=1514,
            //        LineStationIndex=8,
            //        LineStationExist=true,
            //        PrevStation=1518,
            //        NextStation=1512,
            //    },
            //    new LineStation
            //    {
            //        LineId=4,
            //        StationCode=1512,
            //        LineStationIndex=9,
            //        LineStationExist=true,
            //        PrevStation=1514,
            //        NextStation=1491,
            //    },
            //    new LineStation
            //    {
            //        LineId=4,
            //        StationCode=1491,
            //        LineStationIndex=10,
            //        LineStationExist=true,
            //        PrevStation=1512,
            //        NextStation=0,
            //    },

            //    line=33
            //    new LineStation
            //    {
            //        LineId=5,
            //        StationCode=119,
            //        LineStationIndex=1,
            //        LineStationExist=true,
            //        PrevStation=0,
            //        NextStation=1485,
            //    },
            //    new LineStation
            //    {
            //        LineId=5,
            //        StationCode=1485,
            //        LineStationIndex=2,
            //        LineStationExist=true,
            //        PrevStation=119,
            //        NextStation=1486,
            //    },
            //    new LineStation
            //    {
            //        LineId=5,
            //        StationCode=1486,
            //        LineStationIndex=3,
            //        LineStationExist=true,
            //        PrevStation=1485,
            //        NextStation=1487,
            //    },
            //    new LineStation
            //    {
            //        LineId=5,
            //        StationCode=1487,
            //        LineStationIndex=4,
            //        LineStationExist=true,
            //        PrevStation=1486,
            //        NextStation=1488,
            //    },
            //    new LineStation
            //    {
            //        LineId=5,
            //        StationCode=1488,
            //        LineStationIndex=5,
            //        LineStationExist=true,
            //        PrevStation=1487,
            //        NextStation=1490,
            //    },
            //    new LineStation
            //    {
            //        LineId=5,
            //        StationCode=1490,
            //        LineStationIndex=6,
            //        LineStationExist=true,
            //        PrevStation=1488,
            //        NextStation=1494,
            //    },
            //    new LineStation
            //    {
            //        LineId=5,
            //        StationCode=1494,
            //        LineStationIndex=7,
            //        LineStationExist=true,
            //        PrevStation=1490,
            //        NextStation=1492,
            //    },
            //    new LineStation
            //    {
            //        LineId=5,
            //        StationCode=1492,
            //        LineStationIndex=8,
            //        LineStationExist=true,
            //        PrevStation=1494,
            //        NextStation=1493,
            //    },
            //    new LineStation
            //    {
            //        LineId=5,
            //        StationCode=1493,
            //        LineStationIndex=9,
            //        LineStationExist=true,
            //        PrevStation=1492,
            //        NextStation=1491,
            //    },
            //    new LineStation
            //    {
            //        LineId=5,
            //        StationCode=1491,
            //        LineStationIndex=10,
            //        LineStationExist=true,
            //        PrevStation=1493,
            //        NextStation=0,
            //    },

            //    line=67,
            //    new LineStation
            //    {
            //        LineId=6,
            //        StationCode=110,
            //        LineStationIndex=1,
            //        LineStationExist=true,
            //        PrevStation=0,
            //        NextStation=111,
            //    },
            //    new LineStation
            //    {
            //        LineId=6,
            //        StationCode=111,
            //        LineStationIndex=2,
            //        LineStationExist=true,
            //        PrevStation=110,
            //        NextStation=112,
            //    },
            //    new LineStation
            //    {
            //        LineId=6,
            //        StationCode=112,
            //        LineStationIndex=3,
            //        LineStationExist=true,
            //        PrevStation=111,
            //        NextStation=113,
            //    },
            //    new LineStation
            //    {
            //        LineId=6,
            //        StationCode=113,
            //        LineStationIndex=4,
            //        LineStationExist=true,
            //        PrevStation=112,
            //        NextStation=115,
            //    },
            //    new LineStation
            //    {
            //        LineId=6,
            //        StationCode=115,
            //        LineStationIndex=5,
            //        LineStationExist=true,
            //        PrevStation=113,
            //        NextStation=116,
            //    },
            //    new LineStation
            //    {
            //        LineId=6,
            //        StationCode=116,
            //        LineStationIndex=6,
            //        LineStationExist=true,
            //        PrevStation=115,
            //        NextStation=117,
            //    },
            //    new LineStation
            //    {
            //        LineId=6,
            //        StationCode=117,
            //        LineStationIndex=7,
            //        LineStationExist=true,
            //        PrevStation=116,
            //        NextStation=119,
            //    },
            //    new LineStation
            //    {
            //        LineId=6,
            //        StationCode=119,
            //        LineStationIndex=8,
            //        LineStationExist=true,
            //        PrevStation=117,
            //        NextStation=1485,
            //    },
            //    new LineStation
            //    {
            //        LineId=6,
            //        StationCode=1485,
            //        LineStationIndex=9,
            //        LineStationExist=true,
            //        PrevStation=119,
            //        NextStation=1486,
            //    },
            //    new LineStation
            //    {
            //        LineId=6,
            //        StationCode=1486,
            //        LineStationIndex=10,
            //        LineStationExist=true,
            //        PrevStation=1485,
            //        NextStation=0,
            //    },

            //    line=24,
            //    new LineStation
            //    {
            //        LineId=7,
            //        StationCode=97,
            //        LineStationIndex=1,
            //        LineStationExist=true,
            //        PrevStation=0,
            //        NextStation=102,
            //    },
            //    new LineStation
            //    {
            //        LineId=7,
            //        StationCode=102,
            //        LineStationIndex=2,
            //        LineStationExist=true,
            //        PrevStation=97,
            //        NextStation=103,
            //    },
            //    new LineStation
            //    {
            //        LineId=7,
            //        StationCode=103,
            //        LineStationIndex=3,
            //        LineStationExist=true,
            //        PrevStation=102,
            //        NextStation=105,
            //    },
            //    new LineStation
            //    {
            //        LineId=7,
            //        StationCode=105,
            //        LineStationIndex=4,
            //        LineStationExist=true,
            //        PrevStation=103,
            //        NextStation=106,
            //    },
            //    new LineStation
            //    {
            //        LineId=7,
            //        StationCode=106,
            //        LineStationIndex=5,
            //        LineStationExist=true,
            //        PrevStation=105,
            //        NextStation=108,
            //    },
            //    new LineStation
            //    {
            //        LineId=7,
            //        StationCode=108,
            //        LineStationIndex=6,
            //        LineStationExist=true,
            //        PrevStation=106,
            //        NextStation=109,
            //    },
            //    new LineStation
            //    {
            //        LineId=7,
            //        StationCode=109,
            //        LineStationIndex=7,
            //        LineStationExist=true,
            //        PrevStation=108,
            //        NextStation=110,
            //    },
            //    new LineStation
            //    {
            //        LineId=7,
            //        StationCode=110,
            //        LineStationIndex=8,
            //        LineStationExist=true,
            //        PrevStation=109,
            //        NextStation=112,
            //    },
            //    new LineStation
            //    {
            //        LineId=7,
            //        StationCode=112,
            //        LineStationIndex=9,
            //        LineStationExist=true,
            //        PrevStation=110,
            //        NextStation=111,
            //    },
            //    new LineStation
            //    {
            //        LineId=7,
            //        StationCode=111,
            //        LineStationIndex=10,
            //        LineStationExist=true,
            //        PrevStation=112,
            //        NextStation=0,
            //    },

            //      NumberLine=20
            //    new LineStation
            //    {
            //        LineId=8,
            //        StationCode=102,
            //        LineStationIndex=1,
            //        LineStationExist=true,
            //        PrevStation=0,
            //        NextStation=103,
            //    },
            //    new LineStation
            //    {
            //        LineId=8,
            //        StationCode=103,
            //        LineStationIndex=2,
            //        LineStationExist=true,
            //        PrevStation=102,
            //        NextStation=105,
            //    },
            //    new LineStation
            //    {
            //        LineId=8,
            //        StationCode=105,
            //        LineStationIndex=3,
            //        LineStationExist=true,
            //        PrevStation=103,
            //        NextStation=106,
            //    },
            //    new LineStation
            //    {
            //        LineId=8,
            //        StationCode=106,
            //        LineStationIndex=4,
            //        LineStationExist=true,
            //        PrevStation=105,
            //        NextStation=108,
            //    },
            //    new LineStation
            //    {
            //        LineId=8,
            //        StationCode=108,
            //        LineStationIndex=5,
            //        LineStationExist=true,
            //        PrevStation=106,
            //        NextStation=109,
            //    },
            //    new LineStation
            //    {
            //        LineId=8,
            //        StationCode=109,
            //        LineStationIndex=6,
            //        LineStationExist=true,
            //        PrevStation=108,
            //        NextStation=110,
            //    },
            //    new LineStation
            //    {
            //        LineId=8,
            //        StationCode=110,
            //        LineStationIndex=7,
            //        LineStationExist=true,
            //        PrevStation=109,
            //        NextStation=111,
            //    },
            //    new LineStation
            //    {
            //        LineId=8,
            //        StationCode=111,
            //        LineStationIndex=8,
            //        LineStationExist=true,
            //        PrevStation=110,
            //        NextStation=112,
            //    },
            //    new LineStation
            //    {
            //        LineId=8,
            //        StationCode=112,
            //        LineStationIndex=9,
            //        LineStationExist=true,
            //        PrevStation=111,
            //        NextStation=116,
            //    },
            //    new LineStation
            //    {
            //        LineId=8,
            //        StationCode=116,
            //        LineStationIndex=10,
            //        LineStationExist=true,
            //        PrevStation=112,
            //        NextStation=0,
            //    },

            //    line=27
            //    new LineStation
            //    {
            //        LineId=9,
            //        StationCode=85,
            //        LineStationIndex=1,
            //        LineStationExist=true,
            //        PrevStation=0,
            //        NextStation=86,
            //    },
            //    new LineStation
            //    {
            //        LineId=9,
            //        StationCode=86,
            //        LineStationIndex=2,
            //        LineStationExist=true,
            //        PrevStation=85,
            //        NextStation=88,
            //    },
            //    new LineStation
            //    {
            //        LineId=9,
            //        StationCode=88,
            //        LineStationIndex=3,
            //        LineStationExist=true,
            //        PrevStation=86,
            //        NextStation=89,
            //    },
            //    new LineStation
            //    {
            //        LineId=9,
            //        StationCode=89,
            //        LineStationIndex=4,
            //        LineStationExist=true,
            //        PrevStation=88,
            //        NextStation=90,
            //    },
            //    new LineStation
            //    {
            //        LineId=9,
            //        StationCode=90,
            //        LineStationIndex=5,
            //        LineStationExist=true,
            //        PrevStation=89,
            //        NextStation=91,
            //    },
            //    new LineStation
            //    {
            //        LineId=9,
            //        StationCode=91,
            //        LineStationIndex=6,
            //        LineStationExist=true,
            //        PrevStation=90,
            //        NextStation=93,
            //    },
            //    new LineStation
            //    {
            //        LineId=9,
            //        StationCode=93,
            //        LineStationIndex=7,
            //        LineStationExist=true,
            //        PrevStation=91,
            //        NextStation=94,
            //    },
            //    new LineStation
            //    {
            //        LineId=9,
            //        StationCode=94,
            //        LineStationIndex=8,
            //        LineStationExist=true,
            //        PrevStation=93,
            //        NextStation=95,
            //    },
            //    new LineStation
            //    {
            //        LineId=9,
            //        StationCode=95,
            //        LineStationIndex=9,
            //        LineStationExist=true,
            //        PrevStation=94,
            //        NextStation=102,
            //    },
            //    new LineStation
            //    {
            //        LineId=9,
            //        StationCode=102,
            //        LineStationIndex=10,
            //        LineStationExist=true,
            //        PrevStation=95,
            //        NextStation=0,
            //    },

            //    line=21,
            //    new LineStation
            //    {
            //        LineId=10,
            //        StationCode=111,
            //        LineStationIndex=1,
            //        LineStationExist=true,
            //        PrevStation=0,
            //        NextStation=112,
            //    },
            //    new LineStation
            //    {
            //        LineId=10,
            //        StationCode=112,
            //        LineStationIndex=2,
            //        LineStationExist=true,
            //        PrevStation=111,
            //        NextStation=113,
            //    },
            //    new LineStation
            //    {
            //        LineId=10,
            //        StationCode=113,
            //        LineStationIndex=3,
            //        LineStationExist=true,
            //        PrevStation=112,
            //        NextStation=115,
            //    },
            //    new LineStation
            //    {
            //        LineId=10,
            //        StationCode=115,
            //        LineStationIndex=4,
            //        LineStationExist=true,
            //        PrevStation=113,
            //        NextStation=116,
            //    },
            //    new LineStation
            //    {
            //        LineId=10,
            //        StationCode=116,
            //        LineStationIndex=5,
            //        LineStationExist=true,
            //        PrevStation=115,
            //        NextStation=117,
            //    },
            //    new LineStation
            //    {
            //        LineId=10,
            //        StationCode=117,
            //        LineStationIndex=6,
            //        LineStationExist=true,
            //        PrevStation=116,
            //        NextStation=119,
            //    },
            //    new LineStation
            //    {
            //        LineId=10,
            //        StationCode=119,
            //        LineStationIndex=7,
            //        LineStationExist=true,
            //        PrevStation=117,
            //        NextStation=1485,
            //    },
            //    new LineStation
            //    {
            //        LineId=10,
            //        StationCode=1485,
            //        LineStationIndex=8,
            //        LineStationExist=true,
            //        PrevStation=119,
            //        NextStation=1486,
            //    },
            //    new LineStation
            //    {
            //        LineId=10,
            //        StationCode=1486,
            //        LineStationIndex=9,
            //        LineStationExist=true,
            //        PrevStation=1485,
            //        NextStation=1488,
            //    },
            //    new LineStation
            //    {
            //        LineId=10,
            //        StationCode=1488,
            //        LineStationIndex=10,
            //        LineStationExist=true,
            //        PrevStation=1486,
            //        NextStation=0,
            //    },
        
            //#endregion



            //    };

            //XMLTools.SaveListToXMLSerializer(ListLineStations, lineStationPath);
            #endregion
            try
            {
                List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
                return from stations in ListLineStation
                       where (stations.LineStationExist && StationsLinecondition(stations))
                       select stations;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }


        public void AddLineStations(LineStation station)
        {
            try
            {


                List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
                if (ListLineStation.FirstOrDefault(b => b.StationCode == station.StationCode && b.LineId == station.LineId && b.LineStationExist) != null)
                    throw new DO.WrongIDExeption(station.StationCode, "התחנה המבוקשת קיימת כבר במערכת");
                ListLineStation.Add(station);

                XMLTools.SaveListToXMLSerializer(ListLineStation, lineStationPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }

        }

        public int DeleteStationsFromLine(int Scode, int idline)
        {
            try
            {

                List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
                DO.LineStation stations = ListLineStation.FirstOrDefault(b => b.StationCode == Scode && b.LineId == idline && b.LineStationExist);
                if (stations != null)
                    stations.LineStationExist = false;
                else
                    throw new DO.WrongIDExeption(Scode, "התחנה המבוקשת לא נמצאה במערכת");
                XMLTools.SaveListToXMLSerializer(ListLineStation, lineStationPath);
                return stations.LineStationIndex;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return -1;
        }


        public void DeleteStationsFromLine(int Scode)
        {
            try
            {


                List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);

                foreach (DO.LineStation item in ListLineStation)
                {
                    if (item.StationCode == Scode)
                        item.LineStationExist = false;
                }

                XMLTools.SaveListToXMLSerializer(ListLineStation, lineStationPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }

        }

        public void DeleteStationsOfLine(int idline)
        {
            try
            {


                List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);

                foreach (DO.LineStation item in ListLineStation)
                {
                    if (item.LineId == idline)
                        item.LineStationExist = false;
                }

                XMLTools.SaveListToXMLSerializer(ListLineStation, lineStationPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }

        }


        public void UpdateLineStations(LineStation linestations)
        {
            try
            {


                List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
                DO.LineStation station = ListLineStation.FirstOrDefault(b => b.StationCode == linestations.StationCode && b.LineId == linestations.LineId && b.LineStationExist);
                if (station != null)
                {
                    ListLineStation.Remove(station);
                    ListLineStation.Add(linestations);
                }
                else
                    throw new DO.WrongIDExeption(linestations.StationCode, "התחנה המבוקשת לא נמצאה במערכת");
                XMLTools.SaveListToXMLSerializer(ListLineStation, lineStationPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }


        }

        public void UpdateLineStationsCode(LineStation linestations, int newCode)
        {
            try
            {


                List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
                DO.LineStation station = ListLineStation.FirstOrDefault(b => b.StationCode == linestations.StationCode && b.LineId == linestations.LineId && b.LineStationExist);
                if (station != null)
                {
                    linestations.StationCode = newCode;
                    ListLineStation.Remove(station);
                    ListLineStation.Add(linestations);
                }

                XMLTools.SaveListToXMLSerializer(ListLineStation, lineStationPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }




        }




        #region LineTrip

        public LineTrip GetLineTrip(TimeSpan start, int idline)
        {
            try
            {


                XElement ListLineTrip = XMLTools.LoadListFromXMLElement(lineTripPath); //get the data from xml
                DO.LineTrip b = (from line in ListLineTrip.Elements()
                                 where line.Element("KeyId").Value == idline.ToString() && Convert.ToBoolean(line.Element("TripLineExist").Value) == true
                                 select new DO.LineTrip
                                 {
                                     KeyId = int.Parse(line.Element("KeyId").Value),
                                     StartAt = TimeSpan.Parse(line.Element("StartAt").Value),
                                     Frequency = Double.Parse(line.Element("Frequency").Value),
                                     FinishAt = TimeSpan.Parse(line.Element("FinishAt").Value),
                                     TripLineExist = Convert.ToBoolean(line.Element("TripLineExist").Value),

                                 }).FirstOrDefault();

                if (b == null)
                    throw new DO.WrongLineTripExeption(idline, $"{start} לא נמצאו פרטים עבור הקו המבוקש בשעה זו");
                return b;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }

        public IEnumerable<LineTrip> GetAllTripline(int idline)
        {
            #region
            //XElement a = new XElement(lineTripPath);
            //List<LineTrip> ListLineTrip1 = new List<LineTrip>
            //{
            // #region LineTrip
            //new LineTrip
            //    {
            //     KeyId=1,
            //     StartAt=new TimeSpan(06,00,00),
            //     FinishAt=new TimeSpan(23,59,59),
            //     TripLineExist=true,
            //     Frequency=19,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=2,
            //     StartAt=new TimeSpan(06,00,00),
            //     FinishAt=new TimeSpan(24,00,00),
            //     TripLineExist=true,
            //     Frequency=13,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=3,
            //     StartAt=new TimeSpan(06,00,00),
            //     FinishAt=new TimeSpan(23,59,59),
            //     TripLineExist=true,
            //     Frequency=15,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=4,
            //     StartAt=new TimeSpan(06,00,00),
            //     FinishAt=new TimeSpan(19,00,00),
            //     TripLineExist=true,
            //     Frequency=10,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=4,
            //     StartAt=new TimeSpan(19,00,00),
            //     FinishAt=new TimeSpan(23,59,59),
            //     TripLineExist=true,
            //     Frequency=30,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=5,
            //     StartAt=new TimeSpan(06,00,00),
            //     FinishAt=new TimeSpan(08,30,00),
            //     TripLineExist=true,
            //     Frequency=3,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=5,
            //     StartAt=new TimeSpan(08,30,00),
            //     FinishAt=new TimeSpan(14,00,00),
            //     TripLineExist=true,
            //     Frequency=60,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=5,
            //     StartAt=new TimeSpan(14,00,00),
            //     FinishAt=new TimeSpan(23,59,59),
            //     TripLineExist=true,
            //     Frequency=20,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=6,
            //     StartAt=new TimeSpan(06,00,00),
            //     FinishAt=new TimeSpan(23,59,59),
            //     TripLineExist=true,
            //     Frequency=17,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=7,
            //     StartAt=new TimeSpan(08,00,00),
            //     FinishAt=new TimeSpan(23,59,59),
            //     TripLineExist=true,
            //     Frequency=30,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=8,
            //     StartAt=new TimeSpan(06,00,00),
            //     FinishAt=new TimeSpan(12,00,00),
            //     TripLineExist=true,
            //     Frequency=60,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=9,
            //     StartAt=new TimeSpan(05,00,00),
            //     FinishAt=new TimeSpan(10,00,00),
            //     TripLineExist=true,
            //     Frequency=20,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=9,
            //     StartAt=new TimeSpan(10,00,00),
            //     FinishAt=new TimeSpan(23,59,59),
            //     TripLineExist=true,
            //     Frequency=40,
            //     },
            //    new LineTrip
            //    {
            //     KeyId=10,
            //     StartAt=new TimeSpan(6,00,00),
            //     FinishAt=new TimeSpan(23,59,59),
            //     TripLineExist=true,
            //     Frequency=40,
            //     }
            // #endregion LineTrip
            //};
            //a.Add(XMLTools.ToXML(ListLineTrip1[0]));
            //for (int i = 1; i < ListLineTrip1.Count(); i++)
            //{

            //    a.Add(XMLTools.ToXML(ListLineTrip1[i]));
            //}
            //XMLTools.SaveListToXMLElement(a, lineTripPath);

            #endregion
            try
            {
                XElement ListLineTrip = XMLTools.LoadListFromXMLElement(lineTripPath); //get the data from xml

                List<DO.LineTrip> b = new List<LineTrip>();
                foreach (var line in ListLineTrip.Elements())
                {
                    if (line.Element("KeyId").Value == idline.ToString() && Convert.ToBoolean(line.Element("TripLineExist").Value) == true)
                    {
                        b.Add(new DO.LineTrip()
                        {
                            KeyId = int.Parse(line.Element("KeyId").Value),
                            StartAt = TimeSpan.Parse(line.Element("StartAt").Value),
                            Frequency = Double.Parse(line.Element("Frequency").Value),
                            FinishAt = TimeSpan.Parse(line.Element("FinishAt").Value),
                            TripLineExist = Convert.ToBoolean(line.Element("TripLineExist").Value),

                        });
                    }

                }
                return b.AsEnumerable();
            }
            catch (DO.XMLFileLoadCreateException ex) { string ma = ""; ma += ex; }
            return null;
        }



        public IEnumerable<LineTrip> GetAllLineTripsBy(Predicate<LineTrip> StationsLinecondition)
        {
            try
            {


                XElement ListLineTrip = XMLTools.LoadListFromXMLElement(lineTripPath); //get the data from xml
                return from line in ListLineTrip.Elements()
                       let l1 = new DO.LineTrip
                       {
                           KeyId = int.Parse(line.Element("KeyId").Value),
                           StartAt = TimeSpan.Parse(line.Element("StartAt").Value),
                           Frequency = Double.Parse(line.Element("Frequency").Value),
                           FinishAt = TimeSpan.Parse(line.Element("FinishAt").Value),
                           TripLineExist = Convert.ToBoolean(line.Element("TripLineExist").Value),


                       }
                       where StationsLinecondition(l1) && Convert.ToBoolean(line.Element("TripLineExist").Value) == true
                       select l1;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;

        }

        public void AddLineTrip(LineTrip lineTrip)
        {
            try
            {


                XElement ListLineTrip = XMLTools.LoadListFromXMLElement(lineTripPath); //get the data from xml
                XElement lineToAdd = XMLTools.ToXML(lineTrip);

                ListLineTrip.Add(lineToAdd);
                XMLTools.SaveListToXMLElement(ListLineTrip, lineTripPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }


        }


        public void DeleteLineTrip(int idline)
        {
            try
            {


                XElement ListLineTrip = XMLTools.LoadListFromXMLElement(lineTripPath);
                // List<LineTrip> ListLineTrip = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripPath);

                foreach (var item in ListLineTrip.Elements())
                    if (item.Element("KeyId").Value == idline.ToString())
                        item.Element("TripLineExist").Value = "false";
                XMLTools.SaveListToXMLElement(ListLineTrip, lineTripPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
        }

        public void DeleteLineTrip1(LineTrip lineTrip)
        {
            try
            {


                XElement ListLineTrip = XMLTools.LoadListFromXMLElement(lineTripPath);
                //foreach (var item in ListLineTrip.Elements())
                //    if (item.Element("KeyId").Value == lineTrip.KeyId.ToString() && item.Element("StartAt").Value == lineTrip.StartAt.ToString())
                //        item.Element("TripLineExist").Value = "false";

                var v = from item in ListLineTrip.Elements()
                        where item.Element("KeyId").Value == lineTrip.KeyId.ToString() && item.Element("StartAt").Value == lineTrip.StartAt.ToString()
                        select item;
                v.ElementAt(0).Element("TripLineExist").Value = "false";

                XMLTools.SaveListToXMLElement(ListLineTrip, lineTripPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }


        }



        public void UpdatelineTrip(LineTrip lineTrip)
        {
            try
            {


                XElement ListLineTrip = XMLTools.LoadListFromXMLElement(lineTripPath); //get the data from xml
                XElement findLine = (from b in ListLineTrip.Elements()
                                     where b.Element("KeyId").Value == lineTrip.KeyId.ToString() && Convert.ToBoolean(b.Element("TripLineExist").Value) == true
                                     select b).FirstOrDefault();

                if (findLine == null) //we not find the bus
                    throw new DO.WrongLineTripExeption(lineTrip.KeyId, "לא נמצאו זמני נסיעות עבור קו זה");
                else
                {
                    findLine.Element("KeyId").Value = lineTrip.KeyId.ToString();
                    findLine.Element("StartAt").Value = lineTrip.StartAt.ToString();
                    findLine.Element("Frequency").Value = lineTrip.Frequency.ToString();
                    findLine.Element("FinishAt").Value = lineTrip.FinishAt.ToString();
                    findLine.Element("TripLineExist").Value = lineTrip.TripLineExist.ToString();

                    XMLTools.SaveListToXMLElement(ListLineTrip, lineTripPath);
                }
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }


        }


        #endregion

        #region AdjacentStations

        public AdjacentStations GetAdjacentStations(int Scode1, int Scode2)
        {
      //      double speed = 666.66;//m/s- 50 km/h
            #region
            //List<AdjacentStations> ListAdjacentStations1 = new List<AdjacentStations>
            //                      {

            //            #region AdjacentStations  
            //            #region lineId1
            //            new AdjacentStations
            //            {
            //                Station1 = 73,
            //                Station2 = 76,
            //                Distance = 10387.6464817987,
            //                TimeAverage = ((1.5 * 10387.6464817987) / speed),//i.5- air to ground
            //                AdjacExsis=true,
            //            },

            //                new AdjacentStations
            //                {
            //                    Station1 = 76,
            //                    Station2 = 77,
            //                    Distance = 197.059830127369,
            //                    TimeAverage = ((1.5 * 197.059830127369) / speed),
            //                                    AdjacExsis=true,


            //                },

            //                new AdjacentStations
            //                {
            //                    Station1 = 77,
            //                    Station2 = 78,
            //                    Distance = 5942.26478400092,
            //                    TimeAverage = ((1.5 * 5942.26478400092) / speed),
            //                                    AdjacExsis=true,


            //                },

            //                new AdjacentStations
            //                {
            //                    Station1 = 78,
            //                    Station2 = 83,
            //                    Distance = 4115.12303761144,
            //                    TimeAverage = ((1.5 * 4115.12303761144) / speed),
            //                                    AdjacExsis=true,


            //                },

            //                new AdjacentStations
            //                {
            //                    Station1 = 83,
            //                    Station2 = 84,
            //                    Distance = 3971.03321849724,
            //                    TimeAverage = ((1.5 * 3971.03321849724) / speed),

            //                                    AdjacExsis=true,

            //                },

            //                new AdjacentStations
            //                {
            //                    Station1 = 84,
            //                    Station2 = 85,
            //                    Distance = 3665.92895953549,
            //                    TimeAverage = ((1.5 * 3665.92895953549) / speed),
            //                                    AdjacExsis=true,


            //                },

            //                new AdjacentStations
            //                {
            //                    Station1 = 85,
            //                    Station2 = 86,
            //                    Distance = 181.343172558381,
            //                    TimeAverage = ((1.5 * 181.343172558381) / speed),
            //                                    AdjacExsis=true,


            //                },
            //                new AdjacentStations
            //                {
            //                    Station1 = 86,
            //                    Station2 = 88,
            //                    Distance = 338.193775824042,
            //                    TimeAverage = ((1.5 * 338.193775824042) / speed),
            //                                    AdjacExsis=true,


            //                },
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 88,
            //                    Station2 = 89,
            //                    Distance = 839.108713036705,
            //                    TimeAverage = ((1.5 * 839.108713036705) / speed)


            //                },
            //#endregion LineId1

            //                #region lineId2





            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 89,
            //                    Station2 = 90,
            //                    Distance = 4972.12709975181,
            //                    TimeAverage = ((1.5 * 4972.12709975181) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 90,
            //                    Station2 = 91,
            //                    Distance = 4978.59764273959,
            //                    TimeAverage = ((1.5 * 4978.59764273959) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 91,
            //                    Station2 = 93,
            //                    Distance = 178.858161402408,
            //                    TimeAverage = ((1.5 * 178.858161402408) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 93,
            //                    Station2 = 94,
            //                    Distance = 20.9542368918975,
            //                    TimeAverage = ((1.5 * 20.9542368918975) / speed)


            //                },
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 94,
            //                    Station2 = 95,
            //                    Distance = 337.617552547299,
            //                    TimeAverage = ((1.5 * 337.617552547299) / speed)


            //                },
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 95,
            //                    Station2 = 97,
            //                    Distance = 141.607925499495,
            //                    TimeAverage = ((1.5 * 141.607925499495) / speed)


            //                },
            //#endregion LineId2

            //                #region lineId3 
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 122,
            //                    Station2 = 123,
            //                    Distance = 161.658025824811,
            //                    TimeAverage = ((1.5 * 161.658025824811) / speed)
            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 123,
            //                    Station2 = 121,
            //                    Distance = 145.638205945867,
            //                    TimeAverage = ((1.5 * 145.638205945867) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 121,
            //                    Station2 = 1524,
            //                    Distance = 2943.02888382813,
            //                    TimeAverage = ((1.5 * 2943.02888382813) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1524,
            //                    Station2 = 1523,
            //                    Distance = 140.492280736979,
            //                    TimeAverage = ((1.5 * 140.492280736979) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1523,
            //                    Station2 = 1522,
            //                    Distance = 710.578511572254,
            //                    TimeAverage = ((1.5 * 710.578511572254) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1522,
            //                    Station2 = 1518,
            //                    Distance = 2265.21319232095,
            //                    TimeAverage = ((1.5 * 2265.21319232095) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1518,
            //                    Station2 = 1514,
            //                    Distance = 16.6091664944544,
            //                    TimeAverage = ((1.5 * 16.6091664944544) / speed)


            //                },
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1514,
            //                    Station2 = 1512,
            //                    Distance = 1034.11608174677,
            //                    TimeAverage = ((1.5 * 1034.11608174677) / speed)


            //                },
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1512,
            //                    Station2 = 1511,
            //                    Distance = 2271.65706974387,
            //                    TimeAverage = ((1.5 * 2271.65706974387) / speed)


            //                },
            //#endregion LineId3

            //                #region lineId4
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 121,
            //                    Station2 = 123,
            //                    Distance = 145.638205945867,
            //                    TimeAverage = ((1.5 * 145.638205945867) / speed)
            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 123,
            //                    Station2 = 122,
            //                    Distance = 161.658025824811,
            //                    TimeAverage = ((1.5 * 161.658025824811) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 122,
            //                    Station2 = 1524,
            //                    Distance = 2957.82843148296,
            //                    TimeAverage = ((1.5 * 2957.82843148296) / speed)


            //                },



            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1512,
            //                    Station2 = 1491,
            //                    Distance = 2604.33240816743,
            //                    TimeAverage = ((1.5 * 2604.33240816743) / speed)


            //                },
            //#endregion LineId4

            //                #region lineId5
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 119,
            //                    Station2 = 1485,
            //                    Distance = 5719.48141855948,
            //                    TimeAverage = ((1.5 * 5719.48141855948) / speed)
            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1485,
            //                    Station2 = 1486,
            //                    Distance = 179.006776536478,
            //                    TimeAverage = ((1.5 * 179.006776536478) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1486,
            //                    Station2 = 1487,
            //                    Distance = 772.225954779688,
            //                    TimeAverage = ((1.5 * 772.225954779688) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1487,
            //                    Station2 = 1488,
            //                    Distance = 1415.66487905204,
            //                    TimeAverage = ((1.5 * 1415.66487905204) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1488,
            //                    Station2 = 1490,
            //                    Distance = 445.425376124488,
            //                    TimeAverage = ((1.5 * 445.425376124488) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1490,
            //                    Station2 = 1494,
            //                    Distance = 10374.8817694688,
            //                    TimeAverage = ((1.5 * 10374.8817694688) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1494,
            //                    Station2 = 1492,
            //                    Distance = 587.350643701609,
            //                    TimeAverage = ((1.5 * 587.350643701609) / speed)


            //                },
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1492,
            //                    Station2 = 1493,
            //                    Distance = 361.691515745432,
            //                    TimeAverage = ((1.5 * 361.691515745432) / speed)


            //                },
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1493,
            //                    Station2 = 1491,
            //                    Distance = 1144.85124079156,
            //                    TimeAverage = ((1.5 * 1144.85124079156) / speed)


            //                },
            //#endregion LineId5

            //                #region lineId6
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 110,
            //                    Station2 = 111,
            //                    Distance = 195.920342315088,
            //                    TimeAverage = ((1.5 * 195.920342315088) / speed)
            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 111,
            //                    Station2 = 112,
            //                    Distance = 50.4574978208662,
            //                    TimeAverage = ((1.5 * 50.4574978208662) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 112,
            //                    Station2 = 113,
            //                    Distance = 143.276626873244,
            //                    TimeAverage = ((1.5 * 143.276626873244) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 113,
            //                    Station2 = 115,
            //                    Distance = 279.425185419296,
            //                    TimeAverage = ((1.5 * 279.425185419296) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 115,
            //                    Station2 = 116,
            //                    Distance = 72.2684666055651,
            //                    TimeAverage = ((1.5 * 72.2684666055651) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 116,
            //                    Station2 = 117,
            //                    Distance = 192.809573417871,
            //                    TimeAverage = ((1.5 * 192.809573417871) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 117,
            //                    Station2 = 119,
            //                    Distance = 193.971761810414,
            //                    TimeAverage = ((1.5 * 193.971761810414) / speed)


            //                },


            //#endregion LineId6

            //                #region lineId7
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 97,
            //                    Station2 = 102,
            //                    Distance = 4739.1072386442,
            //                    TimeAverage = ((1.5 * 4739.1072386442) / speed)
            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 102,
            //                    Station2 = 103,
            //                    Distance = 554.235295622813,
            //                    TimeAverage = ((1.5 * 554.235295622813) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 103,
            //                    Station2 = 105,
            //                    Distance = 383.45864939499,
            //                    TimeAverage = ((1.5 * 383.45864939499) / speed)

            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 105,
            //                    Station2 = 106,
            //                    Distance = 20.551585590077,
            //                    TimeAverage = ((1.5 * 20.551585590077) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 106,
            //                    Station2 = 108,
            //                    Distance = 314.882994150425,
            //                    TimeAverage = ((1.5 * 314.882994150425) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 108,
            //                    Station2 = 109,
            //                    Distance = 109.450560373063,
            //                    TimeAverage = ((1.5 * 109.450560373063) / speed)


            //                },

            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 109,
            //                    Station2 = 110,
            //                    Distance = 79.9157986778367,
            //                    TimeAverage = ((1.5 * 79.9157986778367) / speed)

            //                },
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 110,
            //                    Station2 = 112,
            //                    Distance = 151.091747503085,
            //                    TimeAverage = ((1.5 * 151.091747503085) / speed)


            //                },
            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 112,
            //                    Station2 = 111,
            //                    Distance = 50.4574978208662,
            //                    TimeAverage = ((1.5 * 50.4574978208662) / speed)


            //                },
            //#endregion LineId7

            //                #region lineId8








            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 112,
            //                    Station2 = 116,
            //                    Distance = 481.422062665161,
            //                    TimeAverage = ((1.5 * 481.422062665161) / speed)


            //                },
            //#endregion LineId8

            //                #region lineId9






            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 95,
            //                    Station2 = 102,
            //                    Distance = 4852.96580098656,
            //                    TimeAverage = ((1.5 * 4852.96580098656) / speed)


            //                },
            //#endregion LineId9

            //                #region lineId10
            //           new AdjacentStations
            //           {                AdjacExsis=true,

            //               Station1 = 97,
            //               Station2 = 122,
            //               Distance = 400,
            //               TimeAverage = ((1.5 * 400) / speed)



            //           },
            //                   new AdjacentStations
            //                   {                AdjacExsis=true,

            //                       Station1 = 1524,
            //                       Station2 = 1522,
            //                       Distance = 300,
            //                       TimeAverage = ((1.5 * 300) / speed)


            //                   },
            //                           new AdjacentStations
            //                           {                AdjacExsis=true,

            //                               Station1 = 1511,
            //                               Station2 = 121,
            //                               Distance = 100,
            //                               TimeAverage = ((1.5 * 100) / speed)


            //                           },







            //                new AdjacentStations
            //                {                AdjacExsis=true,

            //                    Station1 = 1486,
            //                    Station2 = 1488,
            //                    Distance = 650.976014538566,
            //                    TimeAverage = ((1.5 * 650.976014538566) / speed)


            //                },
            //#endregion LineId10
            //            };
            //#endregion AdjacentStations
            //XMLTools.SaveListToXMLSerializer(ListAdjacentStations1, adjacentStationsPath);

            #endregion

            try
            {



                List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(adjacentStationsPath);
                DO.AdjacentStations linestations = ListAdjacentStations.Find(b => b.Station1 == Scode1 && b.Station2 == Scode2 && b.AdjacExsis);//|| b.Station1 == Scode2 && b.Station2 == Scode1);
                if (linestations != null)
                {
                    return linestations;
                }
                else
                    throw new DO.WrongIDExeption(Scode1, "לא נמצאו פרטים במערכת עבור זוג התחנות המבוקש");
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }


        public IEnumerable<AdjacentStations> GetAllAdjacentStations(int stationCode) //return all the AdjacentStations that we have for this station code
        {
            try
            {


                List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(adjacentStationsPath);

                return from station in ListAdjacentStations
                       where ((stationCode == station.Station1 || stationCode == station.Station2) && station.AdjacExsis)
                       select station;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }

        public IEnumerable<AdjacentStations> GetAllAdjacentStationsBy(Predicate<AdjacentStations> StationsLinecondition)
        {
            try
            {


                List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(adjacentStationsPath);


                return from stations in ListAdjacentStations
                       where (StationsLinecondition(stations) && stations.AdjacExsis)
                       select stations;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }



        public void AddLineStations(DO.AdjacentStations adjacentStations)
        {
            try
            {


                List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(adjacentStationsPath);

                DO.AdjacentStations temp = ListAdjacentStations.Find(b => b.Station1 == adjacentStations.Station1 && b.Station2 == adjacentStations.Station2 && adjacentStations.AdjacExsis);
                if (temp != null)
                    throw new DO.WrongIDExeption(adjacentStations.Station1, " התחנה עוקבת כבר קיימת במערכת");
                ListAdjacentStations.Add(adjacentStations);

                XMLTools.SaveListToXMLSerializer(ListAdjacentStations, adjacentStationsPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }

        }

        public void DeleteAdjacentStationse(int Scode1, int Scode2)
        {
            try
            {


                List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(adjacentStationsPath);

                DO.AdjacentStations stations = ListAdjacentStations.Find(b => b.Station1 == Scode1 && b.Station2 == Scode2 && b.AdjacExsis);
                if (stations != null)
                {
                    stations.AdjacExsis = false;
                    //  ListAdjacentStations.Remove(stations);
                }
                else
                    throw new DO.WrongIDExeption(Scode1, "לא נמצאו פרטים עבור התחנה עוקבת המבוקשת");

                XMLTools.SaveListToXMLSerializer(ListAdjacentStations, adjacentStationsPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }


        }

        public void DeleteAdjacentStationseBStation(int Scode1)////
        {
            try
            {


                List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(adjacentStationsPath);

                var v = from item in ListAdjacentStations
                        where ((item.Station1 == Scode1 || item.Station2 == Scode1) && item.AdjacExsis)
                        select item;
                foreach (DO.AdjacentStations item in v)
                {
                    item.AdjacExsis = false;

                    //ListAdjacentStations.Remove(item);
                }

                XMLTools.SaveListToXMLSerializer(ListAdjacentStations, adjacentStationsPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }

        }

        public void UpdateAdjacentStations(DO.AdjacentStations adjacentStations)
        {
            try
            {


                List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(adjacentStationsPath);

                DO.AdjacentStations station = ListAdjacentStations.Find(b => b.Station1 == adjacentStations.Station1 && b.Station2 == adjacentStations.Station2 && adjacentStations.AdjacExsis);
                if (station != null)
                {
                    ListAdjacentStations.Remove(station);
                    ListAdjacentStations.Add(adjacentStations);
                }
                else
                    throw new DO.WrongIDExeption(adjacentStations.Station1, "לא נמצאו פרטים עבור התחנה עוקבת המבוקשת");

                XMLTools.SaveListToXMLSerializer(ListAdjacentStations, adjacentStationsPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }

        }


        public void UpdateAdjacentStations(int code1, int code2, int codeChange, int oldCode)
        {
            try
            {
                List<AdjacentStations> ListAdjacentStations = XMLTools.LoadListFromXMLSerializer<AdjacentStations>(adjacentStationsPath);

                DO.AdjacentStations station = ListAdjacentStations.Find(b => b.Station1 == code1 && b.Station2 == code2 && b.AdjacExsis);
                if (station != null)
                {
                    ListAdjacentStations.Remove(station);
                    if (station.Station1 == oldCode)
                        station.Station1 = codeChange;
                    else
                        station.Station2 = codeChange;

                    ListAdjacentStations.Add(station);
                }
                else
                    throw new DO.WrongIDExeption(code1, "לא נמצאו פרטים עבור התחנה עוקבת המבוקשת");

                XMLTools.SaveListToXMLSerializer(ListAdjacentStations, adjacentStationsPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }

        }




        #endregion



        #region User


        public DO.User GetUser(string name) //check if the user exsis according to the name
        {
            #region restartuser
            //List<User> ListUsers = new List<User>
            //{
            //   #region User
            //    //new User
            //    //{
            //    //    UserName="Herout",
            //    //    Password="12345",
            //    //    Admin=true,
            //    //    UserExist=true,
            //    //    MailAddress="heroot12@gmail.com"
            //    //},

            //    //new User
            //    //{
            //    //    UserName="Dafna",
            //    //    Password="12345",
            //    //    Admin=true,
            //    //    UserExist=true,
            //    //     MailAddress="da0773412369@gmail.com"

            //    //},
            //    //new User
            //    //{
            //    //    UserName="notUs",
            //    //    Password="12345",
            //    //    Admin=false,
            //    //    UserExist=true,
            //    //     MailAddress="heroot12@gmail.com"
            //    //},
            //    //new User
            //    //{
            //    //    UserName="OriyaShmoel",
            //    //    Password="busbus123",
            //    //    Admin=false,
            //    //    UserExist=true,
            //    //     MailAddress="heroot12@gmail.com"
            //    //},
            //     #endregion User
            //};


            //  XMLTools.SaveListToXMLSerializer(ListUsers, userPath);
            #endregion

            try
            {


                List<User> userStations = XMLTools.LoadListFromXMLSerializer<User>(userPath);

                DO.User user = userStations.Find(b => b.UserName == name && b.UserExist);
                if (user != null)
                {
                    return user;
                }
                else
                    throw new DO.WrongNameExeption(name, $"{1}השם לא קיים במערכת או אחד מהפרטים שהזנת שגוי");/////////////////////////////////////////////////////////
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;

        }
        public IEnumerable<DO.User> GetAlluser() //return all the user that we have
        {
            try
            {


                List<User> userStations = XMLTools.LoadListFromXMLSerializer<User>(userPath);

                return from user in userStations
                       where (user.UserExist)
                       select user;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }
        public IEnumerable<DO.User> GetAlluserAdmin() //return all the user Admin we have
        {
            try
            {


                List<User> userStations = XMLTools.LoadListFromXMLSerializer<User>(userPath);

                return from user in userStations
                       where (user.UserExist && user.Admin)
                       select user;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }
        public IEnumerable<DO.User> GetAlluserNAdmin() //return all the user not Admin we have
        {
            try
            {


                List<User> userStations = XMLTools.LoadListFromXMLSerializer<User>(userPath);

                return from user in userStations
                       where (user.UserExist && !user.Admin)
                       select user;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }

        public IEnumerable<DO.User> GetAlluserBy(Predicate<DO.User> userConditions) //איך כותבים??
        {
            try
            {


                List<User> userStations = XMLTools.LoadListFromXMLSerializer<User>(userPath);

                return from u in userStations
                       where (u.UserExist && userConditions(u))
                       select u;
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }

        public void AddUser(DO.User user)
        {
            try
            {


                List<User> userStations = XMLTools.LoadListFromXMLSerializer<User>(userPath);

                if (userStations.FirstOrDefault(b => b.UserName == user.UserName) != null) //if != null its means that this name is allready exsis
                    throw new DO.WrongNameExeption(user.UserName, "שם משתמש כבר קיים במערכת, בבקשה הכנס שם אחר");/////////////////////////////////////////////////////////////////
                userStations.Add(user);
                XMLTools.SaveListToXMLSerializer(userStations, userPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }

        }
        public DO.User getUserBy(Predicate<DO.User> userConditions)
        {
            try
            {


                List<User> userStations = XMLTools.LoadListFromXMLSerializer<User>(userPath);

                var users = from u in userStations
                            where (u.UserExist && userConditions(u))
                            select u;
                return users.ElementAt(0);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
            return null;
        }


        public void DeleteUser(string name)
        {
            try
            {


                List<User> userStations = XMLTools.LoadListFromXMLSerializer<User>(userPath);

                DO.User userDelete = userStations.Find(b => b.UserName == name && b.UserExist);
                if (userDelete != null)
                {
                    userDelete.UserExist = false;

                }
                else
                    throw new DO.WrongNameExeption(name, "לא נמצאו פרטים במערכת עבור משתמש זה");
                XMLTools.SaveListToXMLSerializer(userStations, userPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
        }

        public void UpdateUser(DO.User user)
        {
            try
            {


                List<User> userStations = XMLTools.LoadListFromXMLSerializer<User>(userPath);

                DO.User u = userStations.Find(b => b.UserName == user.UserName && b.UserExist);
                if (u != null)
                {
                    userStations.Remove(u);
                    userStations.Add(user);
                }
                else
                    throw new DO.WrongNameExeption(user.UserName, "לא נמצאו פרטים במערכת עבור שם זה");
                XMLTools.SaveListToXMLSerializer(userStations, userPath);
            }
            catch (DO.XMLFileLoadCreateException ex) { string a = ""; a += ex; }
        }

        #endregion User

    }

}
#endregion