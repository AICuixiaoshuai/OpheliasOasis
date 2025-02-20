﻿/*
 * ReservationDate
 * 
 * Stores the price and occupancy for a given day
 * 
 * TODO: Should more or less be done. subject to changes
 * Changelog:
 * 4/4/2022: created/initially coded by Nathaniel
 * 4/20/2022: added public to methods - Nathaniel
 * 4/22/2022: Added IsFull method - Alex
*/




using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace OpheliasOasis
{

    [DataContract(Name = "ResDate", Namespace = "OpheliasOasis")]
    public class ReservationDate
    {
        [DataMember(Name = "Date")]
        private DateTime date;
        [DataMember(Name = "ResPrice")]
        private double basePrice;
        [DataMember(Name = "Occ")]
        private int occupancy = 0;


        public ReservationDate()
        {
        }

        public ReservationDate(DateTime newDate) 
        {
            date = newDate;
        }
        public DateTime getDate() 
        {
            return (date);
        }


        public double getBasePrice() 
        {
            return (basePrice);
        }
        public void setBasePrice(double newPrice)
        {
            basePrice = newPrice;
            XMLreader.changeReservationDate(this);
        }
        public void increaseOccupancy() 
        {
            occupancy++;
            XMLreader.changeReservationDate(this);
        }
        public void decreaseOccupancy()
        {
            occupancy--;
            XMLreader.changeReservationDate(this);
        }
        public int getOccupancy()
        {
            return (occupancy);
        }
        /// <summary>
        /// returns whether or not the day is full
        /// </summary>
        /// <returns></returns>
        public Boolean IsFull()
        {
            return occupancy >= Hotel.HOTEL_SIZE;
        }
    }
}
