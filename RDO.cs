using System.Collections.Generic;
using UnityEngine;
public class rdo
{
    private Dictionary<string, List<object>> PairValue;
    
    public rdo()
    {
        PairValue = new Dictionary<string,List<object>>();
    }
    public void AddPair (string key, List<object> values)
    {
        PairValue[key] = values;
    }
    public List<object> GetValues(string key)
    {
        if (PairValue.ContainsKey(key))
        {
            return new List<object>(PairValue[key]);
        }
        else 
        return null;
    }
    public void DisplayObject()
    {
        foreach (var pair in PairValue)
        {
            string temp = "";
            temp += pair.Key + ": ";
            foreach (var value in PairValue[pair.Key])
            {
                temp += "<" + value + "> ";
            }
            Debug.Log(temp);
        }
    }
}
