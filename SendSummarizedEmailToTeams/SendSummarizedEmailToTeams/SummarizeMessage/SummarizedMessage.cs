using System.Text;

namespace SendSummarizedEmailToTeams.SummarizeMessage
{
    public class SummarizedMessage
    {
        public SummarizedMessage(params string[] sentences)
        {
            var stringBuilder = new StringBuilder();
            foreach (var sentence in sentences)
            {
                stringBuilder.AppendLine(sentence);
            }
            Summary = stringBuilder.ToString();
        }

        public string Summary { get; }
    }
}