using MasterData;
using MasterMemory;
using MessagePack.Resolvers;
using MessagePack.Unity;
using UnityEngine;

namespace DM
{
    public class TableTest : MonoBehaviour
    {
        void Start()
        {
            Test2();
            // Test1();
        }

        private static void Test2()
        {
            var db = MasterDataDb.Db;

            foreach (var person in db.PersonTable.All)
            {
                Debug.Log($"Person Age: {person.Age}");
            }
            
            foreach (var skill in db.SkillTable.All)
            {
                Debug.Log($"Skill Name: {skill.SkillName}");

                var lv = (int)Random.Range(1, 10);
                Debug.Log($"Skill Lv: {lv}");

                var parameter = db.SkillParameterTable
                    .FindBySkillIDAndSkillLv((skill.SkillID, lv));
                Debug.Log($"Skill Damage: {parameter.Damage}");
            }
        }

        private static void Test1()
        {
            // to create database, use DatabaseBuilder and Append method.
            var builder = new DatabaseBuilder();
            builder.Append(new Person[]
            {
                new Person {PersonId = 0, Age = 13, Gender = Gender.Male, Name = "Dana Terry"},
                new Person {PersonId = 1, Age = 17, Gender = Gender.Male, Name = "Kirk Obrien"},
                new Person {PersonId = 2, Age = 31, Gender = Gender.Male, Name = "Wm Banks"},
                new Person {PersonId = 3, Age = 44, Gender = Gender.Male, Name = "Karl Benson"},
                new Person {PersonId = 4, Age = 23, Gender = Gender.Male, Name = "Jared Holland"},
                new Person {PersonId = 5, Age = 27, Gender = Gender.Female, Name = "Jeanne Phelps"},
                new Person {PersonId = 6, Age = 25, Gender = Gender.Female, Name = "Willie Rose"},
                new Person {PersonId = 7, Age = 11, Gender = Gender.Female, Name = "Shari Gutierrez"},
                new Person {PersonId = 8, Age = 63, Gender = Gender.Female, Name = "Lori Wilson"},
                new Person {PersonId = 9, Age = 34, Gender = Gender.Female, Name = "Lena Ramsey"},
            });

            // build database binary(you can also use `WriteToStream` for save to file).
            byte[] data = builder.Build();

            // -----------------------

            // for query phase, create MemoryDatabase.
            // (MemoryDatabase is recommended to store in singleton container(static field/DI)).
            var db = new MemoryDatabase(data);

            // .PersonTable.FindByPersonId is fully typed by code-generation.
            Person person = db.PersonTable.FindByPersonId(1);

            // Multiple key is also typed(***And * **), Return value is multiple if key is marked with `NonUnique`.
            RangeView<Person> result = db.PersonTable.FindByGenderAndAge((Gender.Female, 23));

            Debug.Log("result");
            foreach (var p in result)
            {
                Debug.Log($"{p.Name}");
            }

            // Get nearest value(choose lower(default) or higher).
            RangeView<Person> age1 = db.PersonTable.FindClosestByAge(31);

            Debug.Log("age1");
            foreach (var p in age1)
            {
                Debug.Log($"{p.Name}");
            }

            // Get range(min-max inclusive).
            RangeView<Person> age2 = db.PersonTable.FindRangeByAge(20, 29);

            Debug.Log("age2");
            foreach (var p in age2)
            {
                Debug.Log($"{p.Name}");
            }
        }
    }
}