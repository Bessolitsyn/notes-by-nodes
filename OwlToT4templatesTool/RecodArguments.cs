using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlToT4templatesTool
{
    internal static class RecodArguments
    {
        private static Dictionary<string, List<string>> dictionary = [];
        public static void Clean()
        {
            dictionary = [];
        }
        public static void AddRecordToDicionary(string record)
        {
            dictionary[record] = new List<string>();

        }
        public static void AddRecordArgumentToDicionary(string record, string arg)
        {
            if (!dictionary.ContainsKey(record))
                dictionary[record] = new List<string>();
            dictionary[record].Add(arg);

        }

        public static List<string> GetRecordArguments(string record)
        {
            if (dictionary.ContainsKey(record))
                return dictionary[record];
            return [];
        }

        public static string GetBaseRecordConstructor(string record, string postfix)
        {
            string constructor = $"{record}{postfix}({string.Join(", ", dictionary[record].Select(arg=> arg.Split(" ")[1]))})";           
            return constructor;
        }
    }
}
