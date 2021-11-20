//A15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Smooth
{
    class SmoothNum
    {
        public BigInteger Number { get; private set; }
        public string AsBinary => ToBinaryString();
        public int NumberOfOnes => AsBinary.Count(x => x == '1');
        public bool isValid => !AsBinary.Contains("111");
        public int NumberOfDoubleOnes => Regex.Matches(AsBinary, "11").Count;

        public SmoothNum(BigInteger num)
        {
            this.Number = num;
        }
        public string ToBinaryString()
        {
            byte[] bytes = Number.ToByteArray();
            int idx = bytes.Length - 1;
            StringBuilder allBinary = new StringBuilder(bytes.Length * 8);
            string binary = Convert.ToString(bytes[idx], 2);
            if (binary[0] != '0' && Number.Sign == 1)
            {
                allBinary.Append('0');
            }
            allBinary.Append(binary);
            for (idx--; idx >= 0; idx--)
            {
                allBinary.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));
            }
            string temp = allBinary.ToString();
            while (temp.StartsWith('0'))
            {
                temp = temp.Substring(1);
            }
            return temp;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int[] tokens = Console.ReadLine().Split().Select(int.Parse).ToArray();
            int n = tokens[0];
            int k = tokens[1];

            var biggestNum = BigInteger.Pow(2, n);
            var smallestNum = BigInteger.Pow(2, n - 1);
            var numbers = new List<SmoothNum>();

            for (BigInteger i = smallestNum; i <= biggestNum; i++)
            {
                if (numbers.Count == 20)
                {
                    break;
                }

                var curr = new SmoothNum(i);
                if (curr.NumberOfOnes == k && curr.isValid)
                {
                    numbers.Add(curr);
                }
            }
            var ordered = numbers.OrderBy(n => n.NumberOfDoubleOnes).ThenBy(n => n.Number).ToList();
            Console.WriteLine(String.Join(' ', ordered.Select(x => x.Number)));

        }
    }
}