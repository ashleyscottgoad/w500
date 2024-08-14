namespace W500Core
{
    public class LbService
    {
        public async Task Reset()
        {
            _db = new LbDatabase();
            _db.Initialize();
        }

        public Task<List<LbSuggestion>> GetSuggestions(int n)
        {
            List<LbSuggestion> result = new List<LbSuggestion>();

            foreach (var c in _db.ValidChars)
            {
                result.Add(new LbSuggestion(
                    c,
                    _words.Where(x => x.StartsWith(c)).OrderByDescending(x => x.Length).Take(n).ToList(),
                    _words.Where(x => x.EndsWith(c)).OrderByDescending(x => x.Length).Take(n).ToList()));
            }

            return Task.FromResult(result);
        }

        public async Task EnterBox(string word)
        {
            _words = _db.ValidWords(word);
        }

        private LbDatabase _db;
        private HashSet<string> _words;
    }
}
