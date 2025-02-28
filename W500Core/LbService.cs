using System.Collections;

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
            BitArray solved = new BitArray(BoxLength, true);
            _solvedInt = GetIntFromBitArray(solved);

            int i = 0;
            _boxLetterArray = _box.ToDictionary(x => x, y => i++);

            foreach (var word in _words)
            {
                BitArray ba = new BitArray(BoxLength);
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
                    var w1 = _bitArrays.Keys.ElementAt(i);
                    var w2 = _bitArrays.Keys.ElementAt(j);
                    var w1val = _bitArrays.Values.ElementAt(i);
                    var w2val = _bitArrays.Values.ElementAt(j);

                    if (_solvedInt == (w1val | w2val))
                    {
                        if (w1[w1.Length - 1] == w2[0] || w1[0] == w2[w2.Length - 1])
                        {
                            if (string.IsNullOrEmpty(_bestPath) || (w1.Length + w2.Length) < _bestPathLength)
                            {
                                _bestPath = w1 + " --> " + w2;
                                _bestPathLength = w1.Length + w2.Length;
                                if (_bestPathLength <= MinimumAcceptableLength) return;
                            }

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
        private int _bestPathLength = MaximumAcceptableLength;
        private const int BoxLength = 12;
        private const int MinimumAcceptableLength = 14;
        private const int MaximumAcceptableLength = 20;
        private Dictionary<char, int> _boxLetterArray = new Dictionary<char, int>();
        Dictionary<string, int> _bitArrays = new Dictionary<string, int>();
        private int _solvedInt;
    }
}
