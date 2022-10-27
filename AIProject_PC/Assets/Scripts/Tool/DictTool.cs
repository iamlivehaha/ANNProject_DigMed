using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DicTool 
{
    public static Tvalue GetValue<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> Dict, Tkey key)
    {
        Tvalue value = default(Tvalue);
        Dict.TryGetValue(key, out value);
        return value;
    }
}
