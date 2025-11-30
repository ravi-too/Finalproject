using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace HangmanGame
{
     
    public static class InputValidator
    {
        public static bool IsValidLetter(string input)
        {
            return input.Length == 1 && char.IsLetter(input[0]);
        }

        public static bool IsValidWord(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
    }

 
    public class PlayerScore
    {
        public string Name { get; set; }
        public int BestScore { get; set; }
    }


   
    public class ScoreService
    {
        private readonly string filePath = "scores.xml";

        public ScoreService()
        {
            if (!File.Exists(filePath))
            {
                new XDocument(new XElement("Scores")).Save(filePath);
            }
        }

        public void SaveScore(string name, int score)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);
                var root = doc.Element("Scores");

                var player = root.Elements("Player")
                    .FirstOrDefault(x => x.Attribute("name")?.Value == name);

                if (player == null)
                {
                    root.Add(new XElement("Player",
                        new XAttribute("name", name),
                        new XAttribute("best", score)));
                }
                else
                {
                    int oldScore = int.Parse(player.Attribute("best").Value);
                    if (score < oldScore)
                        player.SetAttributeValue("best", score);
                }

                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"shecdoma shedegebis chasawerad: {ex.Message}");
            }
        }

        public List<PlayerScore> LoadScores()
        {
            var list = new List<PlayerScore>();

            try
            {
                XDocument doc = XDocument.Load(filePath);
                foreach (var x in doc.Descendants("Player"))
                {
                    list.Add(new PlayerScore
                    {
                        Name = x.Attribute("name").Value,
                        BestScore = int.Parse(x.Attribute("best").Value)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"shecdoma XML-is tvirtis dros: {ex.Message}");
            }

            return list;
        }

        public void ShowTop10()
        {
            var scores = LoadScores()
                .OrderBy(x => x.BestScore)
                .Take(10)
                .ToList();

            Console.WriteLine("\n--- TOP 10 motamashis shedegebi ---");

            foreach (var s in scores)
                Console.WriteLine($"{s.Name} - {s.BestScore} cdela");
        }
    }


    
    public class HangmanService
    {
        private readonly Random random = new Random();

        public string GetRandomWord(List<string> words)
        {
            int index = random.Next(words.Count);
            return words[index];
        }

        public bool IsLetterInWord(char letter, string word)
        {
            return word.Contains(letter);
        }

        public string RevealLetters(string word, char letter, string currentState)
        {
            char[] updated = currentState.ToCharArray();

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == letter)
                    updated[i] = letter;
            }

            return new string(updated);
        }
    }

     
    class Program
    {
        static void Main(string[] args)
        {
            List<string> words = new List<string>
            {
                "farcry", "gta", "fifa", "spiderman", "lol",
                "pepsi", "cocacola","water", "mortalkombat", "csgo"
            };

            HangmanService game = new HangmanService();
            ScoreService scoreService = new ScoreService();

            Console.WriteLine("chawere sheni nickname:");
            string name = Console.ReadLine();

            while (true)
            {
                string word = game.GetRandomWord(words);
                string current = new string('_', word.Length);

                int attempts = 6;
                int mistakes = 0;
                bool won = false;

                Console.WriteLine("\ntamashi daiwyo! gamoicanit asoebi:");
                Console.WriteLine(current);

                while (mistakes < attempts)
                {
                    Console.Write("chawere aso: ");
                    string input = Console.ReadLine().ToLower();

                    if (!InputValidator.IsValidLetter(input))
                    {
                        Console.WriteLine("unda chawero mxolod 1 aso!");
                        continue;
                    }

                    char letter = input[0];

                    if (game.IsLetterInWord(letter, word))
                    {
                        current = game.RevealLetters(word, letter, current);
                        Console.WriteLine($"sworia! {current}");
                    }
                    else
                    {
                        mistakes++;
                        Console.WriteLine($"nah arasworia! dagrcha {attempts - mistakes} mcdeloba.");
                    }
 
                    if (!current.Contains('_'))
                    {
                        won = true;
                        break;
                    }
                }

                if (won)
                {
                    Console.WriteLine("vau moige !cheterobdi dzma!");
                    scoreService.SaveScore(name, mistakes);
                }
                else
                {
                    Console.WriteLine($"waage ddd! swori pasuxi iyo: {word}");
                }

                scoreService.ShowTop10();

                Console.WriteLine("\n gaagrdzeleb megobaro ? (y/n)");
                if (Console.ReadLine().ToLower() != "y")
                    break;
            }
        }
    }
}
