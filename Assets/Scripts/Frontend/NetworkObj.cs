using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkObj : MonoBehaviour
{
    public static HashSet<NetworkObj> allObjs = new HashSet<NetworkObj>();
    public static Dictionary<string, NetworkObj> objsDict = new Dictionary<string, NetworkObj>();

    public class BasicStructure
    {
        public string key = "";
    }

    virtual public string CollectInfo()
    {
        return "";
    }
    virtual public void SetInfo(string json)
    {

    }
}
