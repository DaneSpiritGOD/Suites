using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suites.Core.Math
{
    public class DefaultIntegerRange : IIntegerRecurrentRange
    {
        private int maxValue;
        public int MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                if (value < minValue) throw new NotSupportedException();
                maxValue = value;
                refreshRange();
            }
        }

        private int minValue;
        public int MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                if (value > maxValue) throw new NotSupportedException();
                if (value < 0) throw new NotSupportedException("_minObj needs >=0");

                minValue = value;
                refreshRange();
            }
        }

        public int Range
        {
            get
            {
                return refreshRange();
            }
        }

        int range;
        private int refreshRange()
        {
            range = maxValue - minValue + 1;
            return range;
        }

        public DefaultIntegerRange(int minValue, int maxValue)
        {
            if (minValue > maxValue) throw new ArgumentException("_maxObj needs >=_minObj");
            if (minValue < 0) throw new ArgumentOutOfRangeException("_minObj needs >=0");

            this.minValue = minValue;
            this.maxValue = maxValue;
            refreshRange();
        }

        public RecurrentRangeDistance PureDistance(int start, int end)
        {
            if (!IsPureLegal(start) || !IsPureLegal(end)) throw new ArgumentOutOfRangeException();

            return diffCorrect(start, end);
        }

        private RecurrentRangeDistance diffCorrect(int start, int end)
        {
            if (start == end)
                return new RecurrentRangeDistance(0, 0);
            else if (start < end)
                return new RecurrentRangeDistance(end - start, -range + (end - start));
            else
            {
                var result = diffCorrect(end, start);
                return new RecurrentRangeDistance(-result.Backward, -result.Forward);
            }
        }

        public bool IsPureLegal(int pure)
        {
            return (pure <= maxValue && pure >= minValue);
        }

        public bool IsLegal(int value)
        {
            return value >= minValue;
        }

        public bool IsEqual(int num1, int num2)
        {
            return Recover(num1) == Recover(num2);
        }

        public bool IsPureEqual(int num1, int num2)
        {
            if (!IsPureLegal(num1))
                throw new ArgumentOutOfRangeException($"{num1} must be between {minValue} and {maxValue}.");
            if (!IsPureLegal(num2))
                throw new ArgumentOutOfRangeException($"{num2} must be between {minValue} and {maxValue}.");

            return num1 == num2;
        }

        public int Recover(int num)
        {
            if (!IsLegal(num))
                throw new ArgumentOutOfRangeException($"{num} must greater than _minObj.");

            return (num - minValue) % range + minValue;
        }

        public bool IsInRange(int start, int middle, int end)
        {
            return (PureDistance(start, end).Forward ==
               PureDistance(start, middle).Forward +
               PureDistance(middle, end).Forward);
        }
    }
}
