using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.BusinessModel
{
    public class StringHelper
    {
        public static string ConvertToUrlSlug(string input)
        {
            string convert = string.Empty;
            if (input == null)
                return convert;
            char[] arrayChar = Regex.Replace(input, "[^a-zA-Zа-я-А-Я0-9]", "_").ToCharArray();
            foreach (char c in arrayChar)
                convert += ReplaceToEngChar(c);
            return convert;
        }

        private static string ReplaceToEngChar(char input)
        {
            switch(input)
            {
                case 'А':
                    return "A";
                case 'а':
                    return "a";
                case 'Б':
                    return "B";
                case 'б':
                    return "b";
                case 'В':
                    return "V";
                case 'в':
                    return "v";
                case 'Г':
                    return "G";
                case 'г':
                    return "g";
                case 'Д':
                    return "D";
                case 'д':
                    return "d";
                case 'Е':
                    return "E";
                case 'е':
                    return "e";
                case 'Ё':
                    return "E";
                case 'ё':
                    return "e";
                case 'Ж':
                    return "Zh";
                case 'ж':
                    return "zh";
                case 'З':
                    return "Z";
                case 'з':
                    return "z";
                case 'И':
                    return "I";
                case 'и':
                    return "i";
                case 'Й':
                    return "Y";
                case 'й':
                    return "y";
                case 'К':
                    return "K";
                case 'к':
                    return "k";
                case 'Л':
                    return "L";
                case 'л':
                    return "l";
                case 'М':
                    return "M";
                case 'м':
                    return "m";
                case 'Н':
                    return "N";
                case 'н':
                    return "n";
                case 'О':
                    return "O";
                case 'о':
                    return "o";
                case 'П':
                    return "P";
                case 'п':
                    return "p";
                case 'Р':
                    return "R";
                case 'р':
                    return "r";
                case 'С':
                    return "S";
                case 'с':
                    return "s";
                case 'Т':
                    return "T";
                case 'т':
                    return "t";
                case 'У':
                    return "U";
                case 'у':
                    return "u";
                case 'Ф':
                    return "F";
                case 'ф':
                    return "f";
                case 'Х':
                    return "Kh";
                case 'х':
                    return "kh";
                case 'Ц':
                    return "Ts";
                case 'ц':
                    return "ts";
                case 'Ч':
                    return "Ch";
                case 'ч':
                    return "ch";
                case 'Ш':
                    return "Sh";
                case 'ш':
                    return "sh";
                case 'Щ':
                    return "Shch";
                case 'щ':
                    return "shch";
                case 'Ъ':
                    return "";
                case 'ъ':
                    return "";
                case 'Ы':
                    return "Y";
                case 'ы':
                    return "y";
                case 'Ь':
                    return "";
                case 'ь':
                    return "";
                case 'Э':
                    return "E";
                case 'э':
                    return "e";
                case 'Ю':
                    return "Yu";
                case 'ю':
                    return "yu";
                case 'Я':
                    return "Ya";
                case 'я':
                    return "ya";

                default:
                    return input.ToString();
            }
        }
    }
}
