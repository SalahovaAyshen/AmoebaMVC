namespace Amoeba.Utilities.Extensions
{
    public static class StringValidator
    {
        public static bool Check(this string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (Char.IsDigit(word[i])) return false;
            }
            return true;
        }
    }
}
