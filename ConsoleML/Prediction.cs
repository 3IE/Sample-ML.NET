using ConsoleML.Dbo;
using CsvHelper;
using Microsoft.ML;
using Microsoft.ML.Models;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleML
{
    public class Prediction
    {
        string dataPath = Path.Combine("Data", "TitanicTrain.csv");
        string predictPath = Path.Combine("Data", "TitanicPredict.csv");
        string testPath = Path.Combine("Data", "TitanicTest.csv");
        string outputPath = "output.csv";

        private PredictionModel<ManifestDataTraining, ManifestPrediction> model = null;
        public Prediction()
        {


        }

        public async Task Training()
        {

            var pipeline = new LearningPipeline();

            pipeline.Add(new TextLoader<ManifestDataTraining>(dataPath, useHeader: true, separator: ";"));

            pipeline.Add(new ColumnConcatenator("Features",
                  "Sex",
                   "Pclass",
                   "Age",
                   "Fare",
                   "SibSp",
                   "TEmbarked"
                   ));

            pipeline.Add(new MissingValuesRowDropper() { Column = new[] { "Age" } });

            var classifier = new FastForestBinaryClassifier() { NumLeaves = 10, NumTrees = 13, MinDocumentsInLeafs = 5 };
            pipeline.Add(classifier);


            pipeline.Add(new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" });

            model = pipeline.Train<ManifestDataTraining, ManifestPrediction>();

            await model.WriteAsync("model.train");
            if (model != null)
            {
                TestModel(); 
            }
        }

        private void TestModel()
        {
            var evaluator = new BinaryClassificationEvaluator();
            var testData = new TextLoader<ManifestDataTraining>(testPath, useHeader: true, separator: ";");
            var metrics = evaluator.Evaluate(model, testData);
            Console.WriteLine($"Accuracy = {metrics.Accuracy}");
        }

        public async Task LoadModel()
        {
             model = await PredictionModel.ReadAsync<ManifestDataTraining, ManifestPrediction>("model.train");
        }

        public void Predict()
        {
            List<ManifestDataTraining> records = ReadInputData();

            var res = model.Predict(records);

            foreach (var item in res)
            {


                records.First(x => x.PassengerId == item.PassengerId).Survived = item.Survived;
            }

            WriteResult(records);

        }

        private void WriteResult(List<ManifestDataTraining> records)
        {
            TextWriter textWriter = new StreamWriter(outputPath);

            var csvWriter = new CsvWriter(textWriter);
            csvWriter.Configuration.Delimiter = ";";
            csvWriter.Configuration.HasHeaderRecord = true;
            csvWriter.Configuration.RegisterClassMap<ManifestDataTrainingWriteMap>();
            csvWriter.WriteRecords(records);

            textWriter.Flush();
            textWriter.Close();
        }

        private List<ManifestDataTraining> ReadInputData()
        {
            TextReader textReader = new StreamReader(predictPath);

            var csvReader = new CsvReader(textReader);
            csvReader.Parser.Configuration.Delimiter = ";";
            csvReader.Configuration.HasHeaderRecord = true;
            csvReader.Configuration.RegisterClassMap<ManifestDataTrainingMap>();


            var records = csvReader.GetRecords<ManifestDataTraining>().ToList();
            return records;
        }
    }
}
