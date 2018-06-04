using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleML.Dbo
{
    public class ManifestPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Survived;

        public double PassengerId;

    }
}
