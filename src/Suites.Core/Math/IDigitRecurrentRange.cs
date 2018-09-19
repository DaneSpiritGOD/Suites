using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suites.Core.Math
{
    public interface IIntegerRecurrentRange
    {
        int MaxValue { get; set; }
        int MinValue { get; set; }
        int Range { get; }

        /// <summary>
        /// constraint _minObj<=value<=maxValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IsPureLegal(int value);

        /// <summary>
        /// constraint _minObj<=value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IsLegal(int value);

        /// <summary>
        /// distance from start to end
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>2,5:return (3,-range+3);5,2:return (range-3,-3)</returns>
        RecurrentRangeDistance PureDistance(int start, int end);

        bool IsPureEqual(int num1, int num2);
        bool IsEqual(int num1, int num2);

        /// <summary>
        /// 将num恢复到区间内
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        int Recover(int num);

        bool IsInRange(int start, int middle, int end);
    }

    public class RecurrentRangeDistance
    {
        public int Forward { get; set; }
        public int Backward { get; set; }

        public RecurrentRangeDistance(int forward, int backward)
        {
            if (forward < 0)
                throw new ArgumentOutOfRangeException();
            if (backward > 0)
                throw new ArgumentOutOfRangeException();

            Forward = forward;
            Backward = backward;
        }
    }
}
