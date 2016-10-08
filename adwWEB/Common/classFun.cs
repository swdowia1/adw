using System;
using System.Collections.Generic;


namespace adwWEB.Common
{
    public class classFun
    {
        public static List<int> Losuj(List<int> zbior, int v)
        {
            if (v >= zbior.Count)
                return zbior;
            List<int> wynik = new List<int>();
            int start = 0;
            int ilosc = zbior.Count - 1;
            while (start < v)
            {
                Random r = new Random();

                int los = r.Next(0, ilosc);
                if (!wynik.Contains(zbior[los]))
                {
                    wynik.Add(zbior[los]);
                    start++;
                }
            }

            return wynik;
        }
    }
}