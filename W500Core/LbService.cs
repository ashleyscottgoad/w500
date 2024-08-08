namespace W500Core
{
    public class LbService
    {
        public async Task Reset()
        {
            _db = new LbDatabase();
            _db.Initialize();
        }

        public Task<string> GetSuggestions(int n)
        {
            return Task.FromResult(String.Join(" => ", _words.OrderByDescending(x=>x.Length).Take(n).ToArray()));
        }

        public async Task EnterBox(string word)
        {
            _words = _db.ValidWords(word);
        }

        private LbDatabase _db;
        private HashSet<string> _words;
    }
}
