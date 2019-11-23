using System;
using System.Collections.Generic;

namespace SwiftySend.ConsoleRunner
{
    public struct Ser
    {
        public string Name;
        public object Value;
    }
        
    public class Program
    {
        public static Ser[] T(List<string> list)
        {
            var result = new Ser[list.Count];
            for(int i=0; i<list.Count; i++)
            {
                result[i] = new Ser() { Name = list[i], Value = list[i] };
            }
            return result;
        }
        private static void Main()
        {

        }
    }
}