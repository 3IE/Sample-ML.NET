using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleML.Dbo
{
    public class ManifestDataTraining
    {
        [Column(ordinal: "0")]
        public double PassengerId;
        [Column(ordinal: "1", name: "Label")]
        public bool Survived;
        [Column(ordinal: "2")]
        public float Pclass;
        [Column(ordinal: "4")]
        public float Sex;
        [Column(ordinal: "5")]
        public float Age;



        [Column(ordinal: "6")]
        public float SibSp;
        
        [Column(ordinal: "9")]
        public float Fare;
        
        [Column(ordinal: "14")]
        public float TEmbarked;

    }
}
