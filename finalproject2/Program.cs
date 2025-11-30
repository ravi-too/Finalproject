using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace NumberGuessGame
{

    public static class InputValidator
    {
        public static bool TryReadNumber(string input, out int number)
        {
            return int.TryParse(input, out number);
        }

        public static bool IsValidDifficulty(string diff)
        {
            return diff.ToLower() == "easy" || diff.ToLower() == "medium" || diff.ToLower() == "hard";
        }
    }

   
    public class PlayerScore
    {
        public string Name { get; set; }
        public int BestScore { get; set; }
    }

   
    public class ScoreService
    {
        private readonly string filePath = "scores.csv";

        public ScoreService()
        {
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "Name,BestScore\n");
        }

        public void SaveScore(string name, int score)
        {
            try
            {
                var scores = LoadScores();

                var existing = scores.FirstOrDefault(x => x.Name == name);

                if (existing == null)
                    scores.Add(new PlayerScore { Name = name, BestScore = score });
                else if (score < existing.BestScore)
                    existing.BestScore = score;

                var lines = new List<string> { "Name,BestScore" };
                lines.AddRange(scores.Select(s => $"{s.Name},{s.BestScore}"));

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"shecdoma shedegebis chasawerad: {ex.Message}");
            }
        }

        public List<PlayerScore> LoadScores()
        {
            var scores = new List<PlayerScore>();

            try
            {
                var lines = File.ReadAllLines(filePath).Skip(1);

                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                    {
                        scores.Add(new PlayerScore
                        {
                            Name = parts[0],
                            BestScore = score
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"shecdoma shedegebis chatvirtis dros: {ex.Message}");
            }

            return scores;
        }

        public void ShowTop10()
        {
            var scores = LoadScores()
                .OrderBy(s => s.BestScore)
                .Take(10)
                .ToList();

            Console.WriteLine("\n--- TOP 10 motamashis shedegebi ---");

            foreach (var s in scores)
                Console.WriteLine($"{s.Name} - {s.BestScore} mdgomareoba");
        }
    }


    public class GameService
    {
        private readonly Random random = new Random();

        public int SetDifficulty(string diff)
        {
            return diff.ToLower() switch
            {
                "easy" => 15,
                "medium" => 25,
                "hard" => 50,
                _ => 15
            };
        }

        public int GenerateNumber(int max)
        {
            return random.Next(1, max + 1);
        }
    }

   
    class Program
    {
        static void Main(string[] args)
        {
            GameService game = new GameService();
            ScoreService scoreService = new ScoreService();

            Console.WriteLine("chaweret tqveni saxeli:");
            string name = Console.ReadLine();

            while (true)
            {
                Console.WriteLine("\nairchiet sirtule (Easy / Medium / Hard):");
                string diff = Console.ReadLine();

                if (!InputValidator.IsValidDifficulty(diff))
                {
                    Console.WriteLine("arswori ricxvix!");
                    continue;
                }

                int maxRange = game.SetDifficulty(diff);
                int secret = game.GenerateNumber(maxRange);

                Console.WriteLine($"tamashi daiwyo! aiarchie ricxvi 1-dan {maxRange}-mde. gavt 10 mcdeloba.");

                int attempts = 10;
                int used = 0;
                bool won = false;

                while (attempts > 0)
                {
                    Console.Write("sheiyvanet ricxvi: ");
                    string input = Console.ReadLine();

                    if (!InputValidator.TryReadNumber(input, out int guess))
                    {
                        Console.WriteLine("sxvanairi mnishvneloba!");
                        continue;
                    }

                    used++;

                    if (guess == secret)
                    {
                        Console.WriteLine("gilocavt, moige!");
                        won = true;
                        break;
                    }
                    else if (guess > secret)
                    {
                        Console.WriteLine("ricxvi dabalia.");
                    }
                    else
                    {
                        Console.WriteLine("ricxvi magalia.");
                    }

                    attempts--;
                }

                if (won)
                    scoreService.SaveScore(name, used);
                else
                    Console.WriteLine($"damarcxebuli xart! gamosacnobi ricxvi iyo: {secret}");

                scoreService.ShowTop10();

                Console.WriteLine("\ngsurt gagrdzeleba? (y/n)");
                if (Console.ReadLine().ToLower() != "y")
                    break;
            }
        }
    }
}
