﻿/*
 * Hotel
 * TODO: 
 * Changelog:
 * 4/18/2022: created/initially coded by Alec
 * 4/20/2022: added public to methods - Nathaniel
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace OpheliasOasis
{
    [DataContract(Name = "Hotel", Namespace = "OpheliasOasis")]
    public class Hotel
    {
        [DataMember(Name = "Rooms")]
        private List<bool> rooms;
        [DataMember(Name = "RoomsOcc")]
        private int roomsOccupied;

        public static readonly int HOTEL_SIZE = 45;
        /// <summary>
        /// initializes a Hotel
        /// </summary>
        /// <param name="numrooms"></param>
        public Hotel(int numrooms) 
        { 
            rooms = new List<bool>(new bool[numrooms]);
            roomsOccupied = 0;
        }
        /// <summary>
        /// returns number of occupied rooms
        /// </summary>
        /// <returns></returns>
        public int getRoomsOccupied()
        {
            return this.roomsOccupied;
        }
        /// <summary>
        /// returns a free room, marking it as taken
        /// </summary>
        /// <returns></returns>
        public int assignRoom()
        {
            for (int i = 0; i < this.rooms.Count; i++)
            {
                if (!this.rooms[i])
                {
                    this.rooms[i] = true;
                    this.roomsOccupied++;
                    XMLreader.changeHotel(this);
                    return i + 1;
                }
            }
            return -1;
        }
        /// <summary>
        /// clears the room number
        /// </summary>
        /// <param name="roomNo"></param>
        public void clearRoom(int roomNo)
        {
            int i = roomNo - 1;
            if (i >= 0)
            {
                this.rooms[i] = false;
                this.roomsOccupied--;
                XMLreader.changeHotel(this);
            }
        }
    }
}
