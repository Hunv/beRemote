namespace beRemote.Core.Definitions.Enums.Filter
{
    //Values are shared with "FilterCombination"
    public enum FilterOperation
    {
        Like = 2097152,
        NotLike = 3145728, // Like + NotEqual
        Equal = 0,
        NotEqual = 1048576,
    }
}
