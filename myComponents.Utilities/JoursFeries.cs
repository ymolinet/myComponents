using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace myComponents.Utilities
{
    public static class JoursFeries
    {
        // Methods
        public static DateTime GetJoursPaques(int Year)
        {
            int intGoldNumber = Year % 0x13;
            int intAnneeDiv100 = Year / 100;
            int intEpacte = ((((intAnneeDiv100 - (intAnneeDiv100 / 4)) - (((8 * intAnneeDiv100) + 13) / 0x19)) + (0x13 * intGoldNumber)) + 15) % 30;
            int intDaysEquinoxeToMoonFull = intEpacte - ((intEpacte / 0x1c) * (1 - (((intEpacte / 0x1c) * (0x1d / (intEpacte + 1))) * ((0x15 - intGoldNumber) / 11))));
            int intWeekDayMoonFull = (((((Year + (Year / 4)) + intDaysEquinoxeToMoonFull) + 2) - intAnneeDiv100) + (intAnneeDiv100 / 4)) % 7;
            int intDaysEquinoxeBeforeFullMoon = intDaysEquinoxeToMoonFull - intWeekDayMoonFull;
            int intMonthPaques = 3 + ((intDaysEquinoxeBeforeFullMoon + 40) / 0x2c);
            return new DateTime(Year, intMonthPaques, (intDaysEquinoxeBeforeFullMoon + 0x1c) - (0x1f * (intMonthPaques / 4)));
        }

        public static bool IsFerie(DateTime dtDate, bool LundiPentecote)
        {
            ArrayList arrDateFerie = new ArrayList();
            arrDateFerie.Add(new DateTime(dtDate.Year, 1, 1));
            arrDateFerie.Add(new DateTime(dtDate.Year, 5, 1));
            arrDateFerie.Add(new DateTime(dtDate.Year, 5, 8));
            arrDateFerie.Add(new DateTime(dtDate.Year, 7, 14));
            arrDateFerie.Add(new DateTime(dtDate.Year, 8, 15));
            arrDateFerie.Add(new DateTime(dtDate.Year, 11, 1));
            arrDateFerie.Add(new DateTime(dtDate.Year, 11, 11));
            arrDateFerie.Add(new DateTime(dtDate.Year, 12, 0x19));
            DateTime dtMondayPaques = GetJoursPaques(dtDate.Year).AddDays(1.0);
            arrDateFerie.Add(dtMondayPaques);
            DateTime dtAscension = dtMondayPaques.AddDays(38.0);
            arrDateFerie.Add(dtAscension);
            if (LundiPentecote)
            {
                DateTime dtMondayPentecote = dtMondayPaques.AddDays(49.0);
                arrDateFerie.Add(dtMondayPentecote);
            }
            return arrDateFerie.Contains(dtDate.Date);
        }
    }


}
