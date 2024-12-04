namespace W500Core
{
    internal class LbDatabase
    {
        public bool Done => _done;
        public HashSet<char> ValidChars => _validChars;

        internal void Initialize()
        {

        }

        private bool _done;
        private HashSet<char> _validChars;
    }
}
