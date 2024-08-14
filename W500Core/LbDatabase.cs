namespace W500Core
{
    internal class LbDatabase
    {
        public bool Done => _done;
        public HashSet<char> ValidChars => _validChars;

        internal void Initialize()
        {

        }

        internal HashSet<string> ValidWords(string box)
        {
            var words = new HashSet<string>();
            _validChars = new HashSet<char>();
            if (box.Length != W500Constants.LbWordLength) return words;
            var allWords = GetAllWords();
            HashSet<string> pairs = new HashSet<string>();

            for (int i = 0; i < W500Constants.LbWordLength; i++)
            {
                for (int j = 0; j < W500Constants.LbWordLength; j++)
                {
                    if (i / 3 == j / 3) continue; //disallow same side
                    char c1 = box[i] < box[j] ? box[i] : box[j];
                    char c2 = box[i] < box[j] ? box[j] : box[i];
                    string s1 = new string(new char[] { c1, c2 });
                    if (!pairs.Contains(s1)) pairs.Add(s1);
                }
            }
            _validChars = box.ToHashSet();

            foreach (var word in allWords)
            {
                bool allow = true;
                if (word.Length < 3) continue;
                var intersection = word.ToCharArray().Intersect(_validChars).ToArray();
                if (intersection.Length != word.Length) continue;
                for (int i = 0; i < word.Length - 1; i++)
                {
                    char c1 = word[i] < word[i + 1] ? word[i] : word[i + 1];
                    char c2 = word[i] < word[i + 1] ? word[i + 1] : word[i];
                    string s1 = new string(new char[] { c1, c2 });
                    if (!pairs.Contains(s1))
                    {
                        allow = false;
                        break;
                    }
                }
                if (allow)
                {
                    words.Add(word);
                }
            }

            return words;
        }

        internal HashSet<string> GetAllWords()
        {
            return Properties.Resources.wordlist_10000.Split('\n').ToHashSet();
        }

        private bool _done;
        private HashSet<char> _validChars;
    }
}
