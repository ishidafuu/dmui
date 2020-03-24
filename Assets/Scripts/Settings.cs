using System.Collections.Generic;
using System;
using UniRx.Async;
using UnityEngine;

namespace DM
{
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
        
        public static int Get(MyEnum key) => s_MyEnumData[(int)key];
        public static int Get(MyEnum2 key) => s_MyEnum2Data[(int)key];
        
        private static void InitMyEnum(TableRow[] rows) => InitList<MyEnum>(s_MyEnumData, rows);
        private static void InitMyEnum2(TableRow[] rows) => InitList<MyEnum2>(s_MyEnum2Data, rows);
        private static void InitList<T>(ICollection<int> list, TableRow[] rows)
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

                if (index >= rows.Length)
                {
                    Debug.LogError($"Unmatched count under");
                    break;
                }

                list.Add(rows[index].Value);
                index++;
            }

            if (index != rows.Length)
            {
                Debug.LogError($"Unmatched count over");
            }
        }

        public static async UniTask LoadTableAsync()
        {
            AirTableClient client = new AirTableClient("key3NNedymjZdyPup");
            AirTableBase clientBaseMyEnum = client.GetBase("appsj9JjmBwaF3Hbz");
            AirTableBase clientBaseMyEnum2 = client.GetBase("appsj9JjmBwaF3Hbz");
            
            try
            {
                TableRow[] allRows = await clientBaseMyEnum.LoadTableAsync<TableRow>();
                InitMyEnum(allRows);
                
                TableRow[] allRows2 = await clientBaseMyEnum2.LoadTableAsync<TableRow>();
                InitMyEnum2(allRows2);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}