using QuickGraph;
using QuickGraph.Algorithms.ShortestPath;

namespace W500Core
{
    public class LbService
    {
        public async Task Reset()
        {
            //_db = new LbDatabase();
            //_db.Initialize();
        }

        public Task<List<LbSuggestion>> GetSuggestions()
        {
            return Task.FromResult(_suggestions);
        }

        public void SetDictionary(string[] words)
        {
            _words = words.ToHashSet();
        }

        public async Task SetBox(string box)
        {
            _box = box;
            await EnterBox();
        }

        public async Task EnterBox()
        {
            _charsLeft = _box.ToCharArray().ToHashSet();
            _suggestions = new List<LbSuggestion>();

            foreach (var c in _charsLeft)
            {
                _suggestions.Add(new LbSuggestion(
                    c,
                    _words.Where(x => x.StartsWith(c)).OrderByDescending(x => x.Length).ToList(),
                    _words.Where(x => x.EndsWith(c)).OrderByDescending(x => x.Length).ToList(),
                    _words.Where(x => x.Contains(c)).OrderByDescending(x => x.Length).ToList()
                ));
            }
        }

        private BidirectionalGraph<string, Edge<string>> ConstructGraph()
        {
            var edges = new List<Edge<string>>();

            foreach (var s in _suggestions)
            {
                foreach (var sw in s.StartsWith)
                {
                    foreach (var ew in s.EndsWith)
                    {
                        edges.Add(new Edge<string>(ew, sw));
                    }
                }
            }

            return edges.ToBidirectionalGraph<string, Edge<string>>();
        }

        public async Task FindShortestPath(BidirectionalGraph<string, Edge<string>> graph)
        {
            if (graph == null)
            {
                graph = ConstructGraph();
            }

            string bestPath = string.Empty;
            int bestPathLength = _maxGuesses;

            var orderedWords = _words.OrderByDescending(x => x.ToCharArray().Intersect(_charsLeft).Count()).ToArray();

            foreach (var rootWord in orderedWords)
            {
                var charsLeft = _charsLeft.ToHashSet();
                Func<Edge<string>, double> edgeCost = edge =>
                {
                    return 12 - edge.Target.ToCharArray().Distinct().Intersect(charsLeft).Count();
                };

                var dij = new DijkstraShortestPathAlgorithm<string, Edge<string>>(graph, edgeCost);
                HashSet<string> visited = new HashSet<string>();
                string nextWord = rootWord;

                for (int i = 0; i < bestPathLength; i++)
                {
                    visited.Add(nextWord);
                    charsLeft.RemoveWhere(x => nextWord.Contains(x));
                    dij.Compute(nextWord);
                    var distances = dij.Distances.Where(x => !visited.Contains(x.Key));
                    if (!distances.Any()) break;
                    var next = distances.OrderBy(x => x.Value).First();
                    nextWord = next.Key;
                    dij.SetRootVertex(nextWord);
                    if (charsLeft.Count == 0)
                    {
                        if (visited.Count < bestPathLength)
                        {
                            bestPathLength = visited.Count;
                            bestPath = string.Join(" -> ", visited);
                            if (bestPathLength == _minGuesses)
                            {
                                _bestPath = bestPath;
                                _bestPathLength = bestPathLength;
                                return;
                            }
                        }
                        break;
                    }
                }
            }

            _bestPath = bestPath;
            _bestPathLength = bestPathLength;
        }

        public string BestPath => _bestPath;
        public string Box => _box;
        //private LbDatabase _db;
        private string _box;
        private HashSet<string> _words;
        private string _bestPath = string.Empty;
        private int _bestPathLength;
        private List<LbSuggestion> _suggestions;
        private HashSet<char> _charsLeft = new HashSet<char>();
        private const int _maxGuesses = 4;
        private const int _minGuesses = 2;
        private List<string> _excludedWords = new List<string>();
    }
}
