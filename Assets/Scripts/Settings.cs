using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
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

        private class TableRow
        {
            public string Name;
            public int Value;
        }

        private static readonly List<int> s_MyEnumData = new List<int>();
        private static readonly List<int> s_MyEnum2Data = new List<int>();
        public static int Get(MyEnum key) => s_MyEnumData[(int)key];
        public static int Get(MyEnum2 key) => s_MyEnum2Data[(int)key];
            
        public static async UniTask LoadTableAsync()
        {
            // DevでははAirTableから直接ロード
            // 最終的にはMemoryMasterからロード
            
            AirTableClient client = new AirTableClient("key3NNedymjZdyPup");
            AirTableBase clientBase = client.GetBase("appsj9JjmBwaF3Hbz");
            
            try
            {
                await LoadMyEnumData(clientBase);
                await LoadMyEnumData2(clientBase);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
        
        private static async Task LoadMyEnumData(AirTableBase clientBase)
        {
            TableRow[] allRows = await clientBase.LoadTableAsync<TableRow>("Setting1");
            InitList<MyEnum>(s_MyEnumData, allRows);
        }

        private static async Task LoadMyEnumData2(AirTableBase clientBase)
        {
            TableRow[] allRows2 = await clientBase.LoadTableAsync<TableRow>("Setting2");
            InitList<MyEnum2>(s_MyEnum2Data, allRows2);
        }

        private static void InitList<T>(IList<int> list, IReadOnlyList<TableRow> rows)
        {
            if (Enum.GetValues(typeof(T)).Length != rows.Count)
            {
                Debug.LogError($"Unmatched count enum:{Enum.GetValues(typeof(T)).Length} rows:{rows.Count}");
                return;
            }
            
            int index = 0;
            bool isUpdate = list.Count == rows.Count;
            
            if (!isUpdate)
            {
                list.Clear();
            }
            
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                string name = Enum.GetName(typeof(T), value);
                if (rows[index].Name != name)
                {
                    Debug.LogError($"Unmatched Name rowName:{name} name:{rows[index].Name}");
                    return;
                }

                if (isUpdate)
                {
                    list[index] = rows[index].Value;
                }
                else
                {
                    list.Add(rows[index].Value);
                }
                
                index++;
            }
        }
    }
}