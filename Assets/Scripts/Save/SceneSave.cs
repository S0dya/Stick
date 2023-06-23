using System.Collections.Generic;

[System.Serializable]
public class SceneSave
{
    public List<int> intList;
    public Dictionary<string, int> intDictionary;
    public Dictionary<string, bool> boolDictionary;
    public Dictionary<string, string> stringDictionary;
    public Dictionary<string, int[]> intArrayDictionary;
}
