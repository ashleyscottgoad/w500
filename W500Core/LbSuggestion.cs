namespace W500Core
{
    public class LbSuggestion
    {
        public char Letter { get; set; }
        public List<string> StartsWith { get; set; }
        public List<string> EndsWith { get; set; }
        public List<string> Contains { get; set; }

        public LbSuggestion(char letter, List<string> startsWith, List<string> endsWith, List<string> contains)
        {
            Letter = letter;
            StartsWith = startsWith;
            EndsWith = endsWith;
            Contains = contains;
        }
    }
}
