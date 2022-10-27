using System.Collections.Generic;
using System.Linq;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var result = new List<ComparisonResult>();
            for (var i = 0; i < documents.Count; i++)
            {
                for (int j = i; j < documents.Count; j++)
                {
                    if (j != i)
                        result.Add(new ComparisonResult(documents[i], documents[j],
                            LevenshteinDistance(documents[i], documents[j])));
                }
            }
            return result;
        }

        private static double LevenshteinDistance(DocumentTokens first, DocumentTokens second)
        {
            var opt = new double[first.Count + 1, second.Count + 1];
            for (var i = 0; i <= first.Count; ++i) opt[i, 0] = i;
            for (var i = 0; i <= second.Count; ++i) opt[0, i] = i;

            for (var i = 1; i <= first.Count; ++i)
            for (var j = 1; j <= second.Count; ++j)
            {
                var tokenDistance = TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]);
                if (tokenDistance == 0)
                    opt[i, j] = opt[i - 1, j - 1];
                else
                    opt[i, j] = new[]
                    {
                        1 + opt[i - 1, j], tokenDistance + opt[i - 1, j - 1], 1 + opt[i, j - 1]
                    }.Min();
            }
            return opt[first.Count, second.Count];
        }
    }
}