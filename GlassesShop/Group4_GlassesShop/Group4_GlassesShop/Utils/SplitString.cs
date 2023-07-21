using System.Text.RegularExpressions;

namespace Group4_GlassesShop.Utils
{
    public class SplitString
    {
        public List<string> SplitParagraphIntoSentences(string paragraph)
        {
            // Split the paragraph into sentences
            var sentences = Regex.Split(paragraph, @"(?<=[.!?])\s+");

            // Group the sentences into pairs to form paragraphs
            var paragraphs = new List<string>();
            for (int i = 0; i < sentences.Length; i += 2)
            {
                string paragraphText = string.Join(" ", sentences.Skip(i).Take(2));
                paragraphs.Add(paragraphText);
            }

            // Return the list of paragraphs
            return paragraphs;
        }


    }
}
