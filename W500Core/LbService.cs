using QuickGraph;
using QuickGraph.Algorithms.ShortestPath;
using System.Collections;
using System.Collections.Generic;

namespace W500Core
{
    public class LbService
    {
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
            BitArray solved = new BitArray(12, true);
            _solvedInt = GetIntFromBitArray(solved);

            int i = 0;
            _boxLetterArray = _box.ToDictionary(x => x, y => i++);

            foreach (var word in _words)
            {
                BitArray ba = new BitArray(12);
                foreach (var c in word)
                {
                    ba[_boxLetterArray[c]] = true;
                }
                _bitArrays[word] = GetIntFromBitArray(ba);
            }
        }

        private int GetIntFromBitArray(BitArray bitArray)
        {
            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }

        public async Task FindShortestPath()
        {
            for (int i = 0; i < _bitArrays.Count - 1; i++)
            {
                for (int j = i + 1; j < _bitArrays.Count; j++)
                {
                    var first = _bitArrays.Values.ElementAt(i);
                    var second = _bitArrays.Values.ElementAt(j);

                    if (_solvedInt == (first | second))
                    {
                        var w1 = _bitArrays.Keys.ElementAt(i);
                        var w2 = _bitArrays.Keys.ElementAt(j);
                        var testPath = w1 + " --> " + w2;
                        if (string.IsNullOrEmpty(_bestPath) || testPath.Length < _bestPathLength)
                        {
                            _bestPath = testPath;
                            _bestPathLength = w1.Length + w2.Length;
                            if (_bestPathLength == 12) return;
                        }
                    }
                }
            }
        }

        public string BestPath => _bestPath;
        public string Box => _box;
        private string _box = string.Empty;
        private HashSet<string> _words = new HashSet<string>();
        private string _bestPath = string.Empty;
        private int _bestPathLength;
        private Dictionary<char, int> _boxLetterArray = new Dictionary<char, int>();
        Dictionary<string, int> _bitArrays = new Dictionary<string, int>();
        private int _solvedInt;
    }
}
