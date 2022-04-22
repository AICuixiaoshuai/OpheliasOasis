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

        public Hotel() 
        { 
            rooms = new List<bool>(45);
            roomsOccupied = 0;
        }

        public List<bool> getRooms()
        {
            return this.rooms;
        }
        public void setRooms(List<bool> rooms)
        {
            this.rooms = rooms;
        }
        public int getRoomsOccupied()
        {
            return this.roomsOccupied;
        }
        public int assignRoom()
        {
            for (int i = 0; i < this.rooms.Count; i++)
            {
                if (!this.rooms[i])
                {
                    this.rooms[i] = true;
                    this.roomsOccupied++;
                    return i + 1;
                }
            }
            return -1;
        }
        public void clearRoom(int roomNo)
        {
            int i = roomNo - 1;
            if (i >= 0)
            {
                this.rooms[i] = false;
                this.roomsOccupied--;
            }
        }
    }
}
