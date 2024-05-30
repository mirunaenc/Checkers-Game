using System;
using System.IO;
using System.Linq;

namespace Tema2.Models
{
    public static class GameStatistics
    {
        private static string filePath = @"D:\FACULTATE\Anul II\Semestrul II\MVP\Teme\Backup tema2 - bun, de prezentat\Tema2\Tema2\Resources\gameStatistics.txt";

        public static void RecordGameResult(string winnerColor, int remainingPieces)
        {
            string newData = $"{winnerColor},{remainingPieces}";
            File.AppendAllText(filePath, newData + Environment.NewLine);
        }

        public static (int whiteWins, int redWins, int maxRemainingPieces) GetStatistics()
        {
            if (!File.Exists(filePath)) return (0, 0, 0);

            var data = File.ReadAllLines(filePath);
            int whiteWins = data.Count(line => line.StartsWith("White"));
            int redWins = data.Count(line => line.StartsWith("Red"));

            //val implicita pentru cazul în care nu există elemente
            int maxRemainingPieces = data.Select(line => int.Parse(line.Split(',')[1]))
                                         .DefaultIfEmpty(0) // 0 dacă secvența este goală
                                         .Max();

            return (whiteWins, redWins, maxRemainingPieces);
        }
    }
}
