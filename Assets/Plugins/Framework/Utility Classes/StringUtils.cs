using System;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

/// <summary>
/// Utility class for handling common string manipulation tasks.
/// </summary>
public static class StringUtils
{

    private static string[] _namesOfUnits;
    private static string[] _namesOfTens;

    /// <summary>
    /// Returns the input string with all white-space characters removed.
    /// </summary>
    /// <param name="str">The input string</param>
    /// <returns>The input string, but with all white-space characters removed</returns>
    public static string RemoveWhiteSpace(string str)
    {
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < str.Length; i++)
        {
            if (!char.IsWhiteSpace(str[i]))
            {
                result.Append(str[i]);
            }
        }

        return result.ToString();
    }

    public static string SplitIntoLines(string text, int maxCharsInLine)
    {
        int charsInLine = 0;
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (char.IsWhiteSpace(c) && charsInLine >= maxCharsInLine)
            {
                builder.AppendLine();
                charsInLine = 0;
            }
            else
            {
                builder.Append(c);
                charsInLine++;
            }
        }
        return builder.ToString();
    }

    public static string SplitIntoLines(string text, int maxCharsInLine, out int numLines)
    {
        int charsInLine = 0;
        numLines = 1;
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (char.IsWhiteSpace(c) && charsInLine >= maxCharsInLine)
            {
                numLines++;
                builder.AppendLine();
                charsInLine = 0;
            }
            else
            {
                builder.Append(c);
                charsInLine++;
            }
        }
        return builder.ToString();
    }


    /// <summary>
    /// Returns the input string, but with the first character in upper case.
    /// </summary>
    /// <param name="word">Word to capitalise</param>
    /// <returns>Capitalised word</returns>
    public static string Capitalise(string word)
    {
        return word[0].ToString().ToUpper() + word.Substring(1);
    }

    /// <summary>
    /// Returns the string name of an integer as you would say it in English.
    /// </summary>
    /// <param name="value">The integer to name</param>
    /// <returns>The English name of the value</returns>
    public static string GetNameOfInt(int value)
    {
        if (value == 0) return "zero";
        if (value < 0) return "negative " + GetNameOfInt(-value);

        string words = "";

        if ((value / 1000000) > 0)
        {
            words += GetNameOfInt(value / 1000000) + " million";
            value %= 1000000;
        }

        if ((value / 1000) > 0)
        {
            words += GetNameOfInt(value / 1000) + " thousand";
            value %= 1000;
        }

        if ((value / 100) > 0)
        {
            words += GetNameOfInt(value / 100) + " hundred";
            value %= 100;
        }

        if (value > 0)
        {
            if (words != "")
            {
                words += " and ";
            }


            if (_namesOfUnits == null)
            {
                _namesOfUnits = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            }


            if (value < 20)
            {
                words += _namesOfUnits[value];
            }
            else
            {

                if (_namesOfTens == null)
                {
                    _namesOfTens = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
                }

                words += _namesOfTens[value / 10];
                if ((value % 10) > 0)
                {
                    words += "-" + _namesOfUnits[value % 10];
                }
            }
        }

        return words;
    }

    /// <summary>
    /// Takes an amount of seconds and returns a human readable version of that time period in hours, minutes and seconds.
    /// </summary>
    /// <param name="time">The amount of time, in seconds</param>
    /// <returns>The time in a nice HH:MM:SS format</returns>
    public static string GetTimeString(float time)
    {
        if (time <= 0) return "00:00";

        DateTime dateTime = new DateTime().AddSeconds(time);
        if (time > 60)
        {
            if (time > 3600)
            {
                return dateTime.Hour.ToString("D2") + ":" + dateTime.Minute.ToString("D2") + ":" + dateTime.Second.ToString("D2");
            }
            return dateTime.Minute.ToString("D2") + ":" + dateTime.Second.ToString("D2");
        }

        return "00:" + dateTime.Second.ToString("D2");
    }

    /// <summary>
    /// Takes a camel case string and returns a human readable title case version of it. eg. "_thingNumber" becomes "Thing Number".
    /// </summary>
    /// <param name="input">The input string</param>
    /// <returns>The titleized string</returns>
    public static string Titelize(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";

        StringBuilder str = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (!IsAsciiLetter(c) && !IsAsciiNumber(c))
            {
                continue;
            }

            if (str.Length == 0)
            {
                str.Append(char.ToUpper(c));
            }
            else if (i > 0 && char.IsWhiteSpace(input[i - 1]))
            {
                str.Append(' ');
                str.Append(char.ToUpper(c));
            }
            else if (char.IsUpper(c) && i > 0 && char.IsLower(input[i - 1]))
            {
                str.Append(' ');
                str.Append(char.ToUpper(c));
            }
            else
            {
                str.Append(c);
            }
        }

        return str.ToString();
    }

    /// <summary>
    /// Removes any character from a string that isn't a simple ASCII character.
    /// </summary>
    /// <param name="input">The string to sanitise</param>
    /// /// <param name="allowPunctuationAndSpaces">Whether or not to allow puctuation or the space character. Note that underscore is always allowed!</param>
    /// <returns>The sanitised string</returns>
    public static string Santise(string input, bool allowPunctuationAndSpaces)
    {
        if (string.IsNullOrEmpty(input)) return "";

        StringBuilder str = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (allowPunctuationAndSpaces)
            {
                if (IsAsciiLetter(c) || IsAsciiNumber(c) || IsAsciiPunctuation(c) || c == ' ')
                {
                    str.Append(c);
                }
            }
            else
            {
                if (IsAsciiLetter(c) || IsAsciiNumber(c) || c == '_')
                {
                    str.Append(c);
                }
            }
        }

        return str.ToString();
    }

    static bool IsAsciiNumber(char c)
    {
        return c > 47 && c < 58;
    }

    static bool IsAsciiLetter(char c)
    {
        return (c > 64 && c < 91) || (c > 96 && c < 123);
    }

    static bool IsAsciiPunctuation(char c)
    {
        return (c > 32 && c < 48) || (c > 57 && c < 65) || (c > 90 && c < 97) || (c > 122 && c < 127);
    }

    /// <summary>
    /// Returns the ordinal form of a number, eg: 2nd, 23rd 19th, 1st etc.
    /// </summary>
    /// <param name="number">The number to ordinalize</param>
    /// <returns>The ordinalized string of the number</returns>
    public static string Ordinalize(int number)
    {
        return Inflector.Ordinalize(number);
    }

    /// <summary>
    /// Returns the plural form of a word.
    /// </summary>
    /// <param name="word">The singular form to pluralise</param>
    /// <returns>The plural string of the word</returns>
    public static string Pluralise(string word)
    {
        return Inflector.Pluralize(word);
    }

    /// <summary>
    /// Returns the plural form of a word if necessary. (ie. there is more than one item)
    /// </summary>
    /// <param name="word">The singular form to pluralise</param>
    /// <param name="count">The number of items</param>
    /// <returns>Either the plural or singular form of the word (depending on the count)</returns>
    public static string Pluralise(string word, int count)
    {
        return count != 1 ? Inflector.Pluralize(word) : word;
    }

    /// <summary>
    /// Returns the singular form of a word.
    /// </summary>
    /// <param name="pluralWord">The plural form to singularise</param>
    /// <returns>The singular string of the word</returns>
    public static string Singularise(string pluralWord)
    {
        return Inflector.Singularize(pluralWord);
    }

    //Copyright (c) 2013 Scott Kirkland, used under the MIT license (https://github.com/srkirkland/Inflector)
    static class Inflector
    {

        #region Default Rules

        static Inflector()
        {
            AddPlural("$", "s");
            AddPlural("s$", "s");
            AddPlural("(ax|test)is$", "$1es");
            AddPlural("(octop|vir|alumn|fung)us$", "$1i");
            AddPlural("(alias|status)$", "$1es");
            AddPlural("(bu)s$", "$1ses");
            AddPlural("(buffal|tomat|volcan)o$", "$1oes");
            AddPlural("([ti])um$", "$1a");
            AddPlural("sis$", "ses");
            AddPlural("(?:([^f])fe|([lr])f)$", "$1$2ves");
            AddPlural("(hive)$", "$1s");
            AddPlural("([^aeiouy]|qu)y$", "$1ies");
            AddPlural("(x|ch|ss|sh)$", "$1es");
            AddPlural("(matr|vert|ind)ix|ex$", "$1ices");
            AddPlural("([m|l])ouse$", "$1ice");
            AddPlural("^(ox)$", "$1en");
            AddPlural("(quiz)$", "$1zes");

            AddSingular("s$", "");
            AddSingular("(n)ews$", "$1ews");
            AddSingular("([ti])a$", "$1um");
            AddSingular("((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis");
            AddSingular("(^analy)ses$", "$1sis");
            AddSingular("([^f])ves$", "$1fe");
            AddSingular("(hive)s$", "$1");
            AddSingular("(tive)s$", "$1");
            AddSingular("([lr])ves$", "$1f");
            AddSingular("([^aeiouy]|qu)ies$", "$1y");
            AddSingular("(s)eries$", "$1eries");
            AddSingular("(m)ovies$", "$1ovie");
            AddSingular("(x|ch|ss|sh)es$", "$1");
            AddSingular("([m|l])ice$", "$1ouse");
            AddSingular("(bus)es$", "$1");
            AddSingular("(o)es$", "$1");
            AddSingular("(shoe)s$", "$1");
            AddSingular("(cris|ax|test)es$", "$1is");
            AddSingular("(octop|vir|alumn|fung|cact)i$", "$1us");
            AddSingular("(alias|status)es$", "$1");
            AddSingular("^(ox)en", "$1");
            AddSingular("(vert|ind)ices$", "$1ex");
            AddSingular("(matr)ices$", "$1ix");
            AddSingular("(quiz)zes$", "$1");

            AddIrregular("person", "people");
            AddIrregular("man", "men");
            AddIrregular("child", "children");
            AddIrregular("sex", "sexes");
            AddIrregular("move", "moves");
            AddIrregular("goose", "geese");
            AddIrregular("alumna", "alumnae");

            AddUncountable("equipment");
            AddUncountable("information");
            AddUncountable("rice");
            AddUncountable("money");
            AddUncountable("species");
            AddUncountable("series");
            AddUncountable("fish");
            AddUncountable("sheep");
            AddUncountable("deer");
            AddUncountable("aircraft");
        }

        #endregion

        private class Rule
        {
            private readonly Regex _regex;
            private readonly string _replacement;

            public Rule(string pattern, string replacement)
            {
                _regex = new Regex(pattern, RegexOptions.IgnoreCase);
                _replacement = replacement;
            }

            public string Apply(string word)
            {
                if (!_regex.IsMatch(word))
                {
                    return null;
                }

                return _regex.Replace(word, _replacement);
            }
        }

        private static void AddIrregular(string singular, string plural)
        {
            AddPlural("(" + singular[0] + ")" + singular.Substring(1) + "$", "$1" + plural.Substring(1));
            AddSingular("(" + plural[0] + ")" + plural.Substring(1) + "$", "$1" + singular.Substring(1));
        }

        private static void AddUncountable(string word)
        {
            _uncountables.Add(word.ToLower());
        }

        private static void AddPlural(string rule, string replacement)
        {
            _plurals.Add(new Rule(rule, replacement));
        }

        private static void AddSingular(string rule, string replacement)
        {
            _singulars.Add(new Rule(rule, replacement));
        }

        private static readonly List<Rule> _plurals = new List<Rule>();
        private static readonly List<Rule> _singulars = new List<Rule>();
        private static readonly List<string> _uncountables = new List<string>();

        public static string Pluralize(string word)
        {
            return ApplyRules(_plurals, word);
        }

        public static string Singularize(string word)
        {
            return ApplyRules(_singulars, word);
        }

        private static string ApplyRules(List<Rule> rules, string word)
        {
            string result = word;

            if (!_uncountables.Contains(word.ToLower()))
            {
                for (int i = rules.Count - 1; i >= 0; i--)
                {
                    if ((result = rules[i].Apply(word)) != null)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        public static string Titleize(string word)
        {
            return Regex.Replace(Humanize(Underscore(word)), @"\b([a-z])",
                                 delegate (Match match)
                                 {
                                     return match.Captures[0].Value.ToUpper();
                                 });
        }

        public static string Humanize(string lowercaseAndUnderscoredWord)
        {
            return Capitalize(Regex.Replace(lowercaseAndUnderscoredWord, @"_", " "));
        }

        public static string Pascalize(string lowercaseAndUnderscoredWord)
        {
            return Regex.Replace(lowercaseAndUnderscoredWord, "(?:^|_)(.)",
                                 delegate (Match match)
                                 {
                                     return match.Groups[1].Value.ToUpper();
                                 });
        }

        public static string Camelize(string lowercaseAndUnderscoredWord)
        {
            return Uncapitalize(Pascalize(lowercaseAndUnderscoredWord));
        }

        public static string Underscore(string pascalCasedWord)
        {
            return Regex.Replace(
                Regex.Replace(
                    Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])",
                    "$1_$2"), @"[-\s]", "_").ToLower();
        }

        public static string Capitalize(string word)
        {
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        }

        public static string Uncapitalize(string word)
        {
            return word.Substring(0, 1).ToLower() + word.Substring(1);
        }

        public static string Ordinalize(string numberString)
        {
            return Ordanize(int.Parse(numberString), numberString);
        }

        public static string Ordinalize(int number)
        {
            return Ordanize(number, number.ToString());
        }

        private static string Ordanize(int number, string numberString)
        {
            int nMod100 = number % 100;

            if (nMod100 >= 11 && nMod100 <= 13)
            {
                return numberString + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return numberString + "st";
                case 2:
                    return numberString + "nd";
                case 3:
                    return numberString + "rd";
                default:
                    return numberString + "th";
            }
        }


        public static string Dasherize(string underscoredWord)
        {
            return underscoredWord.Replace('_', '-');
        }
    }
}
