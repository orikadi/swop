/*using System;
using System.Collections.Generic;

namespace swop.Requests
{
    class DateRange
    {
        const int SUB_DAYS = -2;
        const int ADD_DAYS = 2;
        protected readonly DateTime _start;
        protected readonly DateTime _end;

        public DateRange(DateTime start, DateTime end)
        {
            _start = start;
            _end = end;
        }

        public List<DateRange> GenerateAllRanges()
        {
            List<DateRange> ranges = new List<DateRange>();
            for (int i = SUB_DAYS; i <= ADD_DAYS; i++)
            {
                for (int j = SUB_DAYS; j <= ADD_DAYS; j++)
                {
                    DateTime s = _start.AddDays(i);
                    DateTime e = _end.AddDays(j);
                    if (DateTime.Compare(s, e) <= 0 && DateTime.Compare(s, DateTime.Today) >= 0)
                        ranges.Add(new DateRange(s, e));
                }
            }
            return ranges;
        }

        public DateTime GetStart()
        {
            return _start;
        }

        public DateTime GetEnd()
        {
            return _end;
        }

        public override string ToString()
        {
            return _start.Date.ToString("d") + "-" + _end.Date.ToString("d");
        }

        public bool Equals(DateRange dr)
        {
            return dr.GetStart().Equals(_start) && dr.GetEnd().Equals(_end);
        }

    }
}
*/