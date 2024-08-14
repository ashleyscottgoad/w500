namespace W500Core
{
    public class LbSuggestion
    {
        public char Letter { get; set; }
        public List<string> StartsWith { get; set; }
        public List<string> EndsWith { get; set; }

        public LbSuggestion(char letter, List<string> startsWith, List<string> endsWith)
        {
            Letter = letter;
            StartsWith = startsWith;
            EndsWith = endsWith;
        }
    }
}
