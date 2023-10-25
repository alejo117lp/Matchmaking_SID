using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public static class IDCreator
{
    public static float ExtractID(string inputString)
    {
        float result = 0.0f;

        // Use regular expressions to match all numbers in the input string
        MatchCollection matches = Regex.Matches(inputString, "\\d+");

        // Loop through the matched numbers and accumulate them into the 'result' variable
        foreach (Match match in matches)
        {
            string numberStr = match.Value;
            float number;

            // Try parsing the matched number as a float, and add it to 'result'
            if (float.TryParse(numberStr, out number))
            {
                result += number;
            }
        }

        return result;
    }
}
