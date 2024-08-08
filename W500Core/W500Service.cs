namespace W500Core
{
    public class W500Service
    {
        public async Task Reset()
        {
            _db = new W500Database();
            _db.Initialize();
            _words = _db.GetAllWords();
        }

        public Task<string> GetSuggestions(int n)
        {
            return Task.FromResult(String.Join(",", _words.Take(n).ToArray()));
        }

        public async Task EnterGuess(string word, int green, int yellow, int red)
        {
            var wordLookup = _db.GeneratePairs(word, green, yellow, red, _words);
            _words = _db.EnterGuess(word, green, yellow, red, wordLookup);
        }

        public Task<int> GetWordsRemaining()
        {
            int count = _words?.Count ?? 0;
            return Task.FromResult(count);
        }

        private W500Database _db;
        private HashSet<string> _words;
    }
}
