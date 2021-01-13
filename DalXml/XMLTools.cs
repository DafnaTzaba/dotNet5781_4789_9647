﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using DALAPI;
using System.Xml;

namespace DL
{
    static class XMLTools
    {
        static string dir = @"xml\";
        static XMLTools()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        #region SaveLoadWithXElement
        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(dir + filePath);
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        /*
           public int KeyId { get; set; } //id number line
        public TimeSpan StartAt { get; set; }
        public double Frequency { get; set; } //if 0 so its mean single exit 
        public TimeSpan FinishAt { get; set; } //It is possible to have several end times per hour
        public bool TripLineExist { get; set; }        
         
         */
        public static XElement ToXML(this DO.LineTrip st)
        {
            XElement result = new XElement("LineTrip",
                              new XElement("KeyId", st.KeyId),
                              new XElement("Frequency",  st.Frequency),
                              new XElement("StartAt", st.StartAt.ToString()),
                              new XElement("FinishAt", st.FinishAt.ToString()),
                              new XElement("TripLineExist", st.TripLineExist));
            return result;

        }


        //public static XElement ToXML(this DO.Stations st)
        //{
        //    XElement result = new XElement("Stations",
        //                            new XElement("Code", st.Code),
        //                            new XElement("Latitude", st.Coordinate.Latitude),
        //                            new XElement("Longitude", st.Coordinate.Longitude));
        //    return result;
        //}

        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    return XElement.Load(dir + filePath);
                }
                else
                {
                    XElement rootElem = new XElement(dir + filePath);
                    rootElem.Save(dir + filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion

        #region SaveLoadWithXMLSerializer
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(dir + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }
        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(dir + filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion
    }
}