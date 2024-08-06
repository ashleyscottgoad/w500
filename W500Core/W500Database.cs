namespace W500Core
{
    internal class W500Database
    {
        public bool Done => _done;
        public List<Guess> Guesses => _guesses;

        internal void Initialize()
        {
            _guesses = new List<Guess>();
        }

        internal HashSet<string> GetAllWords()
        {
            var words = Properties.Resources.sgb_words.Split('\n');
            var allWords = new HashSet<string>();
            foreach (var w in words)
            {
                if (w.Trim().Length != W500Constants.WordLength)
                {
                    continue;
                }
                var existingChars = new HashSet<char>();
                bool repeatingChars = false;
                foreach (var c in w)
                {
                    if (existingChars.Contains(c))
                    {
                        repeatingChars |= true;
                        break;
                    }
                    existingChars.Add(c);
                }

                if (!repeatingChars)
                {
                    allWords.Add(w.Trim());
                }
            }
            return allWords;
        }

        internal Dictionary<string, List<WordScore>> GeneratePairs(string w1, int g1, int y1, int r1, HashSet<string> words)
        {
            var wordScoresLookup = new Dictionary<string, List<WordScore>>();

            foreach (var w2 in words)
            {
                if (w1.Equals(w2)) continue;
                int green = 0;
                int yellow = 0;
                int red = 0;
                for (int i = 0; i < W500Constants.WordLength; i++)
                {
                    if (w1[i] == w2[i])
                    {
                        green++;
                    }
                    else if (w2.Contains(w1[i]))
                    {
                        yellow++;
                    }
                    else
                    {
                        red++;
                    }
                }

                if (green == g1 && yellow == y1 && red == r1)
                {
                    if (words.Contains(w2))
                    {
                        if (!wordScoresLookup.ContainsKey(w1))
                        {
                            wordScoresLookup.Add(w1, new List<WordScore>());
                        }

                        WordScore wordScore = new WordScore()
                        {
                            Word = w2,
                            Green = green,
                            Yellow = yellow,
                            Red = red
                        };
                        wordScoresLookup[w1].Add(wordScore);
                    }
                }
            }
            return wordScoresLookup;
        }

        internal HashSet<string> EnterGuess(string s, int green, int yellow, int red, Dictionary<string, List<WordScore>> wordScoresLookup)
        {
            HashSet<string> wordList = new HashSet<string>();

            if (green == W500Constants.WordLength)
            {
                //Winner winner
                _done = true;
                return wordList;
            }

            var lastGuessNumber = _guesses.Select(x => x.GuessNumber).FirstOrDefault();
            _guesses.Add(new Guess()
            {
                GuessNumber = lastGuessNumber + 1,
                Green = green,
                Yellow = yellow,
                Red = red,
                Word = s
            });

            var wordsRemaining = wordScoresLookup.SelectMany(x => x.Value).Select(x => x.Word).Distinct().ToHashSet();
            wordList = wordsRemaining.Except(wordScoresLookup.Keys).ToHashSet();
            var suggestions = string.Join(", ", wordList.Take(5));
            Console.WriteLine($"Words Remaining: {wordList.Count}");
            Console.WriteLine($"Suggestions: {suggestions}");
            return wordList;
        }

        private List<Guess> _guesses;
        private bool _done;
    }
}
