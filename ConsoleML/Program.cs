using System;

namespace ConsoleML
{
    class Program
    {
        static void Main(string[] args)
        {
            Prediction prediction = new Prediction();

            prediction.Training().GetAwaiter().GetResult();
            prediction.Predict();
        }
    }
}
