using System;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public enum MyEnum
    {
        Asdf,
        Qwer,
        Zxcv,
    }

    public enum MyEnum2
    {
        Asdf,
        Qwer,
        Zxcv,
    }

    public class TableRow
    {
        public string Name;
        public int Value;
    }

    private static readonly List<int> s_MyEnumData = new List<int>();
    private static readonly List<int> s_MyEnum2Data = new List<int>();

    private static void InitList<T>(ICollection<int> list, IReadOnlyList<TableRow> rows)
    {
        list.Clear();

        int index = 0;
        foreach (T value in Enum.GetValues(typeof(T)))
        {
            string name = Enum.GetName(typeof(T), value);
            if (rows[index].Name != name)
            {
                Debug.LogError($"Unmatched Name rowName:{name} name:{rows[index].Name}");
                break;
            }

            if (index >= rows.Count)
            {
                Debug.LogError($"Unmatched count under");
                break;
            }
            
            list.Add(rows[index].Value);
            index++;
        }
        
        if (index != rows.Count)
        {
            Debug.LogError($"Unmatched count over");
        }
    }

    public static void InitMyEnum(List<TableRow> rows) => InitList<MyEnum>(s_MyEnumData, rows);
    public static void InitMyEnum2(List<TableRow> rows) => InitList<MyEnum2>(s_MyEnum2Data, rows);
    public static int Get(MyEnum key) => s_MyEnumData[(int)key];
    public static int Get(MyEnum2 key) => s_MyEnum2Data[(int)key];
}