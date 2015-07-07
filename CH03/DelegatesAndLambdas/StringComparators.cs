namespace DelegatesAndLambdas
{
    class StringComparators
    {
        public static bool CompareLength(string first, string second)
        {
            return first.Length == second.Length;
        }
        public bool CompareContent(string first, string second)
        {
            return first == second;
        }

    }
}