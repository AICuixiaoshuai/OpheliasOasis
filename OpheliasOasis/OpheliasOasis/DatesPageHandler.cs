﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OpheliasOasis
{

    // view prices, view one date, change one price, setting unset prices for the year ahead.
    class DatesPageHandler
    {
        private static Calendar cal;
        private static String manPass;

        // Dates submenu
        public static MenuPage datesMenu;

        public static ProcessPage setForYear;
        public static ProcessPage setIndividual;
        public static ProcessPage showIndividual;
        public static ProcessPage showYearAhead;

        private static List<Tuple<Func<String, String>, String>> monthOfSetting;

        private static DateTime dateToChange;


        private readonly static Tuple<Func<String, String>, String> checkPS =
        Tuple.Create<Func<String, String>, String>(checkPasschangeSingle, "Input Manager Password");
        private readonly static Tuple<Func<String, String>, String> recieveDate =
        Tuple.Create<Func<String, String>, String>(getInDate, "Input Date to have price changed (DD/MM/YYYY)");
        private readonly static Tuple<Func<String, String>, String> recieveMonth =
        Tuple.Create<Func<String, String>, String>(getInMonth, "Input start of date range in DD/MM/YYYY or  MM/YYYY format");
        private readonly static Tuple<Func<String, String>, String> recievePrice =
        Tuple.Create<Func<String, String>, String>(getInPrice, "Input new base price");
        private readonly static Tuple<Func<String, String>, String> viewDate =
        Tuple.Create<Func<String, String>, String>(getInDate, "Input Date (DD/MM/YYYY)");
        private readonly static Tuple<Func<String, String>, String> viewMonth =
        Tuple.Create<Func<String, String>, String>(showMonthOfDates, "This will print a month's dates and prices (MM/YYYY)");
        private readonly static Tuple<Func<String, String>, String> getDateMonth =
        Tuple.Create<Func<String, String>, String>(getInMonth, "Input first date to change");

        public static void Init(Calendar cl, String mPass)
        {
            cal = cl;
            manPass = mPass;
            monthOfSetting = new List<Tuple<Func<String, String>, String>>();
            monthOfSetting.Add(new Tuple<Func<String, String>, String>(checkPasschangeMonth, "Input Manager Password"));
            monthOfSetting.Add(getDateMonth);
            for (int i = 2; i < 33; i++) 
            {
                monthOfSetting.Add(new Tuple<Func<String, String>, String>(inputDatePrice, "Input New Price, <Enter> to skip"));
            }







            showYearAhead = new ProcessPage("Date Information for one month", "Display a month of dates and prices", new List<Tuple<Func<String, String>, String>> { viewMonth }, null);

            showIndividual = new ProcessPage("Individual Date Information", "Display a specific date", new List<Tuple<Func<String, String>, String>> { viewDate }, null);

            setIndividual = new ProcessPage("Set Specific Date Price (Manager Only)", "Sets a specific date", new List<Tuple<Func<String, String>, String>> { checkPS, recieveDate, recievePrice }, setInPrice);

            setForYear = new ProcessPage("Set Prices For a Month (Manager Only)", "Set 31 days of prices", monthOfSetting, updateMonthPrices);


            datesMenu = new MenuPage("Dates", "Dates submenu", new List<Page> { showIndividual, showYearAhead, setIndividual , setForYear});
        }






        public static void setPassword(String mPass)
        {
            manPass = mPass;
        }





        //SET DATES FOR A MONTH
        private static double[] g = new double[31];
        private static int currDay;

        static String getInMonth(String input)
        {
            if (!DateTime.TryParse(input, out dateToChange))
            {
                return ("Not the correct format.");
            }

            currDay = 0;
            Console.Write("Current Price for " + dateToChange.AddDays(currDay).ToString("ddd dd-MM-yyyy") + ": " + cal.retrieveDate(dateToChange.AddDays(currDay)).getBasePrice());
            return "";
        }

        static String checkPasschangeMonth(String input)
        {
            if (input != manPass)
            {
                return "Password incorrect.";
            }
            return "";
        }

        static String inputDatePrice(String input)
        {

            if (String.IsNullOrEmpty(input)) 
            {
                g[currDay] = -1;
                currDay += 1;
                Console.Write("Current Price for " + dateToChange.AddDays(currDay).ToString("ddd dd-MM-yyyy") + ": " + cal.retrieveDate(dateToChange.AddDays(currDay)).getBasePrice());
                return ""; 
            }
            if (double.TryParse(input, out double inPrice))
            {
                g[currDay] = inPrice;
                currDay += 1;
                if (currDay != 31)
                { 
                    Console.Write("Current Price for " + dateToChange.AddDays(currDay).ToString("ddd dd-MM-yyyy") + ": " + cal.retrieveDate(dateToChange.AddDays(currDay)).getBasePrice());
                }
                return "";
            }
            else
            {
                return "not a valid price";
            }
        }

        static String updateMonthPrices()
        {
            for(int i = 0; i < 31; i++)
            {
                if (g[i] != -1)
                {
                    cal.retrieveDate(dateToChange.AddDays(i)).setBasePrice(g[i]);
                }
            }

            return "";
        }

        //Update a single price
        
        private static double inPrice;
        static String checkPasschangeSingle(String input)
        {
            if (input != manPass)
            {
                return "Password incorrect.";
            }
            return "";
        }

        static String getInDate(String input)
        {
            if (DateTime.TryParse(input, out dateToChange))
            {
                Console.Write("Current Price: " + cal.retrieveDate(dateToChange).getBasePrice() + " Current Occupancy: " + cal.retrieveDate(dateToChange).getOccupancy());
                return ("");
            }
            else
            {
                return ("Not the correct format.");
            }
        }

        static String getInPrice(String input)
        {
            if (double.TryParse(input, out inPrice))
            {
                return ("");
            }
            else
            {
                return ("Not a valid date");
            }
        }

        static String setInPrice()
        {
            cal.retrieveDate(dateToChange).setBasePrice(inPrice);
            return "";
        }

        //show a month of prices
        static String showMonthOfDates(String input)
        {
            if (!DateTime.TryParse(input, out dateToChange)) { return ("Not a valid month"); }
            if (dateToChange.Day != 1) { return ("Not a valid month"); }

            int g = 0;
            for(DateTime i = dateToChange; i.Month == dateToChange.Month; i = i.AddDays(1))
            {
                if (g % 7 == 0) { Console.WriteLine(""); }
                Console.Write((i.ToString("ddd dd-MM-yyyy") + ": " + cal.retrieveDate(i).getBasePrice()).PadRight(25));
                g++;
            }
            return "";
        }















    }


}