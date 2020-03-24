using System.Collections.Generic;

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

    private static void InitList(ICollection<int> list, IEnumerable<TableRow> rows)
    {
        list.Clear();
        foreach (TableRow row in rows)
        {
            list.Add(row.Value);
        }
    }

    public static void InitMyEnum(IEnumerable<TableRow> rows) => InitList(s_MyEnumData, rows);
    public static void InitMyEnum2(IEnumerable<TableRow> rows) => InitList(s_MyEnum2Data, rows);
    public static int Get(MyEnum key) => s_MyEnumData[(int)key];
    public static int Get(MyEnum2 key) => s_MyEnum2Data[(int)key];
}