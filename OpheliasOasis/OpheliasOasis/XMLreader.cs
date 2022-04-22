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
        public XMLformat(ReservationDB db, Hotel hot, Calendar calend) 
        {
            R = db;
            H = hot;
            C = calend;
        }

        [DataMember(Name = "XMLStrRDB")]
        public ReservationDB R { get; set;  }
        [DataMember(Name = "XMLStrH")]
        public Hotel H { get; set;  }
        [DataMember(Name = "XMLStrC")]
        public Calendar C { get; set; }

    }




    static class XMLreader
    {
        static public void XMLout(ReservationDB ResDB, Hotel hotel, Calendar cal) 
        {
            if (File.Exists(@".\" + DateTime.Today.ToString("D"))) 
            {
                File.Delete(@".\" + DateTime.Today.ToString("D"));
            }
            Console.WriteLine(@".\" + DateTime.Today.ToString("D"));
            
            FileStream fs = new FileStream(@".\" + DateTime.Today.ToString("D"), FileMode.Create);

            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(fs);
            DataContractSerializer ser = new DataContractSerializer(typeof(XMLformat));
            ser.WriteObject(writer, new XMLformat(ResDB, hotel, cal));
            writer.Close();
            fs.Close();
        }


        static public XMLformat ResDBin(DateTime day)
        {
            FileStream fs = new FileStream(@".\" + day.ToString("D"), FileMode.OpenOrCreate);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());

            // Create the DataContractSerializer instance.
            DataContractSerializer ser = new DataContractSerializer(typeof(XMLformat));

            // Deserialize the data and read it from the instance.
            XMLformat returnStruct = (XMLformat)ser.ReadObject(reader);
            returnStruct.R.reorganize();
            return returnStruct;


        }


    }
}
