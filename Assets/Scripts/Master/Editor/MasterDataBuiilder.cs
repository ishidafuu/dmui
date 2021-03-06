﻿using System.Collections.Generic;
using System.IO;
using MasterData;
using UnityEditor;
using UnityEngine;

namespace DM
{
    public static class MasterDataBuilder
    {
        public static void Build()
        {
            try
            {
                MessagePackResolver.SetupMessagePackResolver();
            }
            catch
            {
                // ignored
            }

            var builder = new DatabaseBuilder();
            builder = BuildParson(builder);
            builder = BuildSkill(builder);
            builder = BuildSkillParameter(builder);

            byte[] data = builder.Build();

            string resourcesDir = $"{Application.dataPath}/Resources/";
            string fileName = MasterDataDb.MASTER_RESOURCE_PATH + ".bytes";
            Directory.CreateDirectory(resourcesDir);

            using (var fs = new FileStream(resourcesDir + fileName, FileMode.Create))
            {
                fs.Write(data, 0, data.Length);
            }

            Debug.Log($"Write byte[] to: {resourcesDir + fileName}");

            AssetDatabase.Refresh();
        }

        private static DatabaseBuilder BuildParson(DatabaseBuilder builder)
        {
            builder.Append(new[]
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
            return builder;
        }

        private static DatabaseBuilder BuildSkill(DatabaseBuilder builder)
        {
            builder.Append(new[]
            {
                new Skill {SkillID = 0, SkillName = "スキル0"},
                new Skill {SkillID = 1, SkillName = "スキル1"},
                new Skill {SkillID = 2, SkillName = "スキル2"},
                new Skill {SkillID = 3, SkillName = "スキル3"},
            });
            return builder;
        }

        private static DatabaseBuilder BuildSkillParameter(DatabaseBuilder builder)
        {
            var skillParameters = new List<SkillParameter>();
            for (int i = 0; i < 4; i++)
            {
                for (int lv = 1; lv < 10; lv++)
                {
                    skillParameters.Add(new SkillParameter
                    {
                        SkillID = i,
                        SkillLv = lv,
                        Damage = lv * 100
                    });
                }
            }

            builder.Append(skillParameters);
            return builder;
        }
    }
}