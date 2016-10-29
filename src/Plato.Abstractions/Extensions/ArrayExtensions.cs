﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace Plato.Abstractions.Extensions
{
    public static class ArrayExtensions
    {

        public static string ToDelimitedString(
        this string[] input, char delimiter = ',')
        {

            var sb = new StringBuilder();
            if (input != null)
            {
                for (var i = 0; i <= input.Length - 1; i++)
                {
                    sb.Append(input.GetValue(i).ToString())
                        .Append(delimiter);
                }
            }
            return sb.ToString().TrimEnd(delimiter);

        }


        public static string ToDelimitedString(
            this int[] input, char delimiter = ',')
        {

            var sb = new StringBuilder();
            if (input != null)
            {
                for (var i = 0; i <= input.Length - 1; i++)
                {
                    sb.Append(input.GetValue(i).ToString())
                        .Append(delimiter);
                }
            }
            return sb.ToString().TrimEnd(delimiter);

        }



    }
}
