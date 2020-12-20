using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefulFuncs : MonoBehaviour
{
    public static string PrimitiveToJsonValue(object o)
    {
        return "\"" + o + "\"";
    }

    public static string CombineJsons(KeyValuePair<string, object>[] pairs)
    {
        string json = "{";
        int count = pairs.Length;
        for(int i = 0; i < count; i++)
        {
            var pair = pairs[i];
            json += "\"" + pair.Key + "\":";
            json += pair.Value;
            if(i < count - 1)
            {
                json += ",";
            }
        }
        json += "}";
        return json;
    }

    public static float SmoothStep(float edge0, float edge1, float value)
    {
        value = Mathf.Clamp((value - edge0) / (edge1 - edge0), 0.0f, 1.0f);
        return value * value * (3f - 2f * value);
    }

    /*public static string FormatChronoTime(float time)
    {
        int s = Mathf.FloorToInt(time);
        int ds = Mathf.FloorToInt((time - s) * 99.9f);
        string strS = (s < 10 ? "0" : "") + s;
        string strDs = (ds < 10 ? "0" : "") + ds;
        return "" + strS + ":" + strDs;
    }

    public static string FormatTime(float time)
    {
        int s = Mathf.FloorToInt(time);
        int ds = Mathf.FloorToInt((time - s) * 99.9f);
        string strS = (s < 10 ? "0" : "") + s;
        string strDs = (ds < 10 ? "0" : "") + ds;
        return "" + strS + "s " + strDs+"ds";
    }*/

    public static string[] Split(string original, char splitChar = ' ')
    {
        List<string> allStrings = new List<string>();
        string current = "";
        char c;
        for(int i = 0; i < original.Length; i++)
        {
            c = original[i];
            if(c == splitChar)
            {
                if(current != "")
                {
                    allStrings.Add(current);
                    current = "";
                }
            } else
            {
                current += c;
            }
        }
        if(current != "")
        {
            allStrings.Add(current);
        }

        return allStrings.ToArray();
    }
}
