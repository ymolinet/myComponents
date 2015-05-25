namespace Gaia.WebWidgets.Samples.Extensions.InPlaceEdit.Matrix
{
    using System;

    public class Helper
    {
        public static int[][] GenerateMatrixNumbers(int power)
        {
            var rnd = new Random();
            var numbers = new int[power][];
            for (int i = 0; i < power; i++)
            {
                numbers[i] = new int[power];

                for (int j = 0; j < power; j++)
                    numbers[i][j] = rnd.Next(0, 100);
            }
            return numbers;
        }
    }
}
