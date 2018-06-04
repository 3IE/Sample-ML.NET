using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleML.Dbo
{
    public sealed class ManifestDataTrainingMap : ClassMap<ManifestDataTraining>
    {
        public ManifestDataTrainingMap()
        {

            Map(m => m.Survived).Ignore();
            Map(m => m.PassengerId).Index(0);
            Map(m => m.Pclass).Index(2);
            Map(m => m.Sex).Index(4);
            Map(m => m.Age).Index(5).Default(30);
            Map(m => m.SibSp).Index(6);
            Map(m => m.Fare).Index(9);
            Map(m => m.TEmbarked).Index(14);


        }
    }

    public sealed class ManifestDataTrainingWriteMap : ClassMap<ManifestDataTraining>
    {
        public ManifestDataTrainingWriteMap()
        {

            Map(m => m.Survived).Index(0);
            Map(m => m.PassengerId).Index(1);
        }
    }
}
