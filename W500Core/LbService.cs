
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Search;
using QuickGraph.Algorithms.ShortestPath;
using QuickGraph.Graphviz;

namespace W500Core
{
    public class LbService
    {
        public async Task Reset()
        {
            _db = new LbDatabase();
            _db.Initialize();
        }

        public Task<List<LbSuggestion>> GetSuggestions()
        {
            return Task.FromResult(_suggestions);
        }

        public async Task EnterBox(string word)
        {
            //word = "shlngeriyjut";
            //jerseys, sunlight
            _words = _db.ValidWords(word);
            _suggestions = new List<LbSuggestion>();

            foreach (var c in _db.ValidChars)
            {
                _suggestions.Add(new LbSuggestion(
                    c,
                    _words.Where(x => x.StartsWith(c)).OrderByDescending(x => x.Length).ToList(),
                    _words.Where(x => x.EndsWith(c)).OrderByDescending(x => x.Length).ToList(),
                    _words.Where(x => x.Contains(c)).OrderByDescending(x => x.Length).ToList()
                    ));
            }
        }

        public async Task<string> ConstructNetwork()
        {
            var edges = new List<Edge<string>>();

            foreach (var s in _suggestions)
            {
                foreach (var sw in s.StartsWith)
                {
                    foreach (var ew in s.EndsWith)
                    {
                        edges.Add (new Edge<string>(ew, sw));
                    }
                }
            }

            HashSet<char> charsLeft = _db.ValidChars;
            var graph = edges.ToBidirectionalGraph<string, Edge<string>>();
            Func<Edge<string>, double> edgeCost = edge =>
            {
                return 12 - edge.Target.ToCharArray().Distinct().Intersect(charsLeft).Count();
            };
            var dij = new DijkstraShortestPathAlgorithm<string, Edge<string>>(graph, edgeCost);
            HashSet<string> visited = new HashSet<string>();
            var smallestPool = _suggestions.OrderBy(x => x.Contains.Count).First();
            var rootWord = smallestPool.Contains.First();

            for(int i = 0; i<10; i++)
            {
                visited.Add(rootWord);
                charsLeft.RemoveWhere(x=>rootWord.Contains(x));
                dij.Compute(rootWord);
                var next = dij.Distances.Where(x=>!visited.Contains(x.Key)).OrderBy(x => x.Value).First();
                rootWord = next.Key;
                dij.SetRootVertex(rootWord);
                if (charsLeft.Count == 0) break;
            }

            return string.Join(" -> ",visited);
        }


        private LbDatabase _db;
        private HashSet<string> _words;
        private List<LbSuggestion> _suggestions;
        private int _maxGuesses = 6;
    }
}
