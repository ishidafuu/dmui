// <auto-generated />
#pragma warning disable CS0105
using DM;
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;
using MasterData.Tables;

namespace MasterData
{
   public sealed class MemoryDatabase : MemoryDatabaseBase
   {
        public PersonTable PersonTable { get; private set; }
        public SkillTable SkillTable { get; private set; }
        public SkillParameterTable SkillParameterTable { get; private set; }

        public MemoryDatabase(
            PersonTable PersonTable,
            SkillTable SkillTable,
            SkillParameterTable SkillParameterTable
        )
        {
            this.PersonTable = PersonTable;
            this.SkillTable = SkillTable;
            this.SkillParameterTable = SkillParameterTable;
        }

        public MemoryDatabase(byte[] databaseBinary, bool internString = true, MessagePack.IFormatterResolver formatterResolver = null)
            : base(databaseBinary, internString, formatterResolver)
        {
        }

        protected override void Init(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options)
        {
            this.PersonTable = ExtractTableData<Person, PersonTable>(header, databaseBinary, options, xs => new PersonTable(xs));
            this.SkillTable = ExtractTableData<Skill, SkillTable>(header, databaseBinary, options, xs => new SkillTable(xs));
            this.SkillParameterTable = ExtractTableData<SkillParameter, SkillParameterTable>(header, databaseBinary, options, xs => new SkillParameterTable(xs));
        }

        public ImmutableBuilder ToImmutableBuilder()
        {
            return new ImmutableBuilder(this);
        }

        public DatabaseBuilder ToDatabaseBuilder()
        {
            var builder = new DatabaseBuilder();
            builder.Append(this.PersonTable.GetRawDataUnsafe());
            builder.Append(this.SkillTable.GetRawDataUnsafe());
            builder.Append(this.SkillParameterTable.GetRawDataUnsafe());
            return builder;
        }

        public ValidateResult Validate()
        {
            var result = new ValidateResult();
            var database = new ValidationDatabase(new object[]
            {
                PersonTable,
                SkillTable,
                SkillParameterTable,
            });

            ((ITableUniqueValidate)PersonTable).ValidateUnique(result);
            ValidateTable(PersonTable.All, database, "PersonId", PersonTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)SkillTable).ValidateUnique(result);
            ValidateTable(SkillTable.All, database, "SkillID", SkillTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)SkillParameterTable).ValidateUnique(result);
            ValidateTable(SkillParameterTable.All, database, "SkillID", SkillParameterTable.PrimaryKeySelector, result);

            return result;
        }

        static MasterMemory.Meta.MetaDatabase metaTable;

        public static object GetTable(MemoryDatabase db, string tableName)
        {
            switch (tableName)
            {
                case "person":
                    return db.PersonTable;
                case "skill":
                    return db.SkillTable;
                case "skillParameter":
                    return db.SkillParameterTable;
                
                default:
                    return null;
            }
        }

        public static MasterMemory.Meta.MetaDatabase GetMetaDatabase()
        {
            if (metaTable != null) return metaTable;

            var dict = new Dictionary<string, MasterMemory.Meta.MetaTable>();
            dict.Add("person", MasterData.Tables.PersonTable.CreateMetaTable());
            dict.Add("skill", MasterData.Tables.SkillTable.CreateMetaTable());
            dict.Add("skillParameter", MasterData.Tables.SkillParameterTable.CreateMetaTable());

            metaTable = new MasterMemory.Meta.MetaDatabase(dict);
            return metaTable;
        }
    }
}