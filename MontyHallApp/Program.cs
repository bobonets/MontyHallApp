using System;
using System.Collections.Generic;
using System.Linq;

namespace MontyHallApp
{
    public static class Program
    {
        private static void MontyHall(int setLength, int iterations)
        {
            var successOrigChoice = 0;
            var successNewChoice = 0;

            for (var i = 0; i < iterations; i++)
                RunGame();

            void RunGame()
            {
                //create and populate set
                var set = Enumerable.Range(0, setLength).ToList();
                var random = new Random();
                //generate winner
                var winner = random.Next(setLength);
                //generate player choice
                var guess = random.Next(setLength);
                //create unique random list
                var uRandomList = UniqueRandomListGenerator().ToList();
                Print("");
                Print($"NEW GAME SET: {Aggregator(set)}");
                Print($"WINNER: {winner}");
                Print($"ORIGINAL PLAYER CHOICE: {guess}");
                if (guess == winner)
                {
                    successOrigChoice++;
                    Print($"VICTORY WITH ORIGINAL CHOICE REGISTERED");
                }

                #region Remove members for the second guess

                //winner or guess can't be removed from the set
                var offset = 3; //hardcoded

                for (var i = 0; i < setLength - offset; i++)
                {
                    var item = uRandomList[i]; // sequential unique value
                    var hasProtectedMember = item == winner || item == guess;

                    if (hasProtectedMember)
                    {
                        Print($"PROTECTED MEMBER REGISTERED");

                        offset--;
                        continue;
                    }

                    set.Remove(item);
                    Print($"ITEM {item} REMOVED FROM SET");
                }

                #endregion

                //change mind scenario
                // generate random index amongst remaining items
                int RIndex(int size) => random.Next(size);

                var newIndex = RIndex(offset);
                var newGuess = set[newIndex];

                while (newGuess == guess)
                    newGuess = RIndex(offset);

                Print($"CHANGED MIND CHOICE: {newGuess}");

                if (newGuess == winner)
                {
                    successNewChoice++;
                    Print($"VICTORY WITH CHANGED MIND REGISTERED");
                }

                Print($"ENDGAME SET: {Aggregator(set)} LENGTH: {set.Count}");
            }

            Print($"");
            Print($"REPORT: ");
            Print($"GAMES PLAYED: {iterations}");
            Print($"SUCCESSES ORIGINAL CHOICE: {successOrigChoice}");
            Print($"SUCCESSES CHANGED MIND: {successNewChoice}");

            //utility methods
            IEnumerable<int> UniqueRandomListGenerator()
            {
                var random = new Random();

                var possibilities = Enumerable.Range(0, setLength).ToList();
                var randomized = new List<int>();

                for (var i = 0; i < setLength; i++)
                {
                    var j = random.Next(possibilities.Count);
                    randomized.Add(possibilities[j]);
                    possibilities.RemoveAt(j);
                }

                return randomized;
            }

            //unnecessary - could use Console.Write
            string Aggregator<T>(IEnumerable<T> values) => string.Join(' ', values);

            void Print<T>(T data) => Console.WriteLine(data);
        }

        private static void Main(string[] args)
        {
            MontyHall(3, 64);
        }
    }
}