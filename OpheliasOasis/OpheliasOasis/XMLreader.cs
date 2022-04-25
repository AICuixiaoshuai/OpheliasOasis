﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace OpheliasOasis
{
    [DataContract(Name = "XMLStr", Namespace = "OpheliasOasis")]
    public struct XMLformat
    {
        public XMLformat(ReservationDB db, Hotel hot, Calendar calend, String managerPass) 
        {
            R = db;
            H = hot;
            C = calend;
            M = managerPass;
        }

        [DataMember(Name = "XMLStrRDB")]
        public ReservationDB R { get; set;  }
        [DataMember(Name = "XMLStrH")]
        public Hotel H { get; set;  }
        [DataMember(Name = "XMLStrC")]
        public Calendar C { get; set; }
        [DataMember(Name = "XMLStrMP")]
        public String M { get; set; }

    }




    static class XMLreader
    {
        static private String backupfilePath = "C:\\OpheliasOasis\\Backups\\";
        static private String ResDBfilePath = "C:\\OpheliasOasis\\Data\\Reservations\\";
        static private String CalfilePath = "C:\\OpheliasOasis\\Data\\Calendar\\";
        static private String HotelfilePath = "C:\\OpheliasOasis\\Data\\Hotel\\";



        static public void XMLout(ReservationDB ResDB, Hotel hotel, Calendar cal, String ManPass) 
        {
            
            if (File.Exists(backupfilePath + DateTime.Today.ToString("D"))) 
            {
                File.Delete(backupfilePath + DateTime.Today.ToString("D"));
            }
            Console.WriteLine(backupfilePath + DateTime.Today.ToString("D"));
            
            FileStream fs = new FileStream(backupfilePath + DateTime.Today.ToString("D"), FileMode.Create);

            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(fs);
            DataContractSerializer ser = new DataContractSerializer(typeof(XMLformat));
            ser.WriteObject(writer, new XMLformat(ResDB, hotel, cal, ManPass));
            writer.Close();
            fs.Close();
        }


        static public XMLformat XMLin(DateTime day)
        {
            if (! File.Exists(backupfilePath + day.ToString("D")))
            {
                throw new FileNotFoundException();
            }

            FileStream fs = new FileStream(backupfilePath + day.ToString("D"), FileMode.OpenOrCreate);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());

            // Create the DataContractSerializer instance.
            DataContractSerializer ser = new DataContractSerializer(typeof(XMLformat));

            // Deserialize the data and read it from the instance.
            XMLformat returnStruct = (XMLformat)ser.ReadObject(reader);
            returnStruct.R.reorganize();
            returnStruct.C.WriteCaltoXML();
            changeMPass(returnStruct.M);
            changeHotel(returnStruct.H);
            reader.Close();
            fs.Close();

            return returnStruct;


        }

        static public void AddOrChangeReservationinDB(Reservation d)
        {
            if (d.getID() == 0) { throw new ArgumentException("no ID provided"); }
            if (File.Exists(ResDBfilePath + d.getID()))
            {
                File.Delete(ResDBfilePath + d.getID());
            }

            FileStream fs = new FileStream(ResDBfilePath + d.getID(), FileMode.Create);
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(fs);
            DataContractSerializer ser = new DataContractSerializer(typeof(Reservation));
            ser.WriteObject(writer, d);
            writer.Close();
            fs.Close();
        }






        static public void changeReservationDate(ReservationDate day)
        {

            if (File.Exists(CalfilePath + day.getDate().ToString("dd-MM-yyyy")))
            {
                File.Delete(CalfilePath + day.getDate().ToString("dd-MM-yyyy"));
            }

            FileStream fs = new FileStream(CalfilePath + day.getDate().ToString("dd-MM-yyyy"), FileMode.Create);
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(fs);
            DataContractSerializer ser = new DataContractSerializer(typeof(ReservationDate));
            ser.WriteObject(writer, day);
            writer.Close();
            fs.Close();
        }

        static public void changeHotel(Hotel h)
        {

            if (File.Exists(HotelfilePath + "Hotel"))
            {
                File.Delete(HotelfilePath + "Hotel");
            }

            FileStream fs = new FileStream(HotelfilePath + "Hotel", FileMode.Create);
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(fs);
            DataContractSerializer ser = new DataContractSerializer(typeof(Hotel));
            ser.WriteObject(writer, h);
            writer.Close();
            fs.Close();
        }

        static public void changeMPass(String h)
        {

            if (File.Exists(HotelfilePath + "Other"))
            {
                File.Delete(HotelfilePath + "Other");
            }

            FileStream fs = new FileStream(HotelfilePath + "Other", FileMode.Create);
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(fs);
            DataContractSerializer ser = new DataContractSerializer(typeof(String));
            ser.WriteObject(writer, h);
            writer.Close();
            fs.Close();
        }


        static public ReservationDB readInResDB()
        {
            ReservationDB loadResDB = new ReservationDB();
            FileStream fs;
            XmlDictionaryReader reader;
            DataContractSerializer ser = new DataContractSerializer(typeof(Reservation));
            IEnumerable<string> resfiles = Directory.EnumerateFiles(ResDBfilePath);
            foreach(string g in resfiles) 
            { 
                    fs = new FileStream(g, FileMode.OpenOrCreate);
                    reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
                    loadResDB.addReservationReader((Reservation)ser.ReadObject(reader));
                    reader.Close();
                    fs.Close();
            }
            return (loadResDB);


        }

        static public Calendar readInCal()
        {
            Calendar loadCal = new Calendar();
            FileStream fs;
            XmlDictionaryReader reader;
            DataContractSerializer ser = new DataContractSerializer(typeof(ReservationDate));
            IEnumerable<string> resfiles = Directory.EnumerateFiles(CalfilePath);
            foreach (string g in resfiles)
            {
                fs = new FileStream(g, FileMode.OpenOrCreate);
                reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
                loadCal.XMLReaderOnlyAdd((ReservationDate)ser.ReadObject(reader));
                reader.Close();
                fs.Close();
            }
            return (loadCal);



        }

        static public Hotel readInHotel()
        {
            if (!File.Exists(HotelfilePath + "Hotel"))
            {
                throw new FileNotFoundException();
            }

            FileStream fs = new FileStream(HotelfilePath + "Hotel", FileMode.OpenOrCreate);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());

            // Create the DataContractSerializer instance.
            DataContractSerializer ser = new DataContractSerializer(typeof(Hotel));

            // Deserialize the data and read it from the instance.
            Hotel returnHotel = (Hotel)ser.ReadObject(reader);
            reader.Close();
            fs.Close();
            return returnHotel;
        }

        static public String readInMPass()
        {
            if (!File.Exists(HotelfilePath + "Other"))
            {
                throw new FileNotFoundException();
            }

            FileStream fs = new FileStream(HotelfilePath + "Other", FileMode.OpenOrCreate);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());

            // Create the DataContractSerializer instance.
            DataContractSerializer ser = new DataContractSerializer(typeof(String));

            // Deserialize the data and read it from the instance.
            String returnMPass = (String)ser.ReadObject(reader);
            reader.Close();
            fs.Close();
            return returnMPass;
        }


        static public void clearFolders() 
        {
            IEnumerable<string> resfiles = Directory.EnumerateFiles(ResDBfilePath);
            foreach (string g in resfiles)
            {
                File.Delete(g);
            }

            resfiles = Directory.EnumerateFiles(CalfilePath);
            foreach (string g in resfiles)
            {
                File.Delete(g);
            }
            }
















    }
}
