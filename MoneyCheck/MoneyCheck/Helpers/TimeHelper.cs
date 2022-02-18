using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneyCheck.Helpers
{
    public class TimeHelper
    {
        public static string GetTimePhrase()
        {
            DateTime now = DateTime.Now;

            var phrase = times.FirstOrDefault(x => now.Hour >= x.Start && now.Hour < x.End);
            if(phrase == default) return null;

            return phrase.Phrase;
        }

        private class TimeClass
        {
            public int Start { get; set; }
            public int End { get; set; }
            public string Phrase { get; set; }
        }
        private static List<TimeClass> times = new List<TimeClass>()
        {
            new TimeClass() { Start = 0, End = 4, Phrase = "Здравствуйте"},
            new TimeClass() { Start = 4, End = 12, Phrase = "Доброе утро"},
            new TimeClass() { Start = 12, End = 18, Phrase = "Добрый день"},
            new TimeClass() { Start = 18, End = 24, Phrase = "Добрый вечер"},
        };
    }
}
