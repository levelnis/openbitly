namespace OpenBitly
{
    public static class StringExtensions
    {
        public static bool IsNullOrBlank(this string input)
        {
            return string.IsNullOrEmpty(input) || input.Trim().Length == 0;
        }
    }
}