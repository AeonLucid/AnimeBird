namespace AnitomyLib.Tokens
{
    internal enum TokenFlag
    {
        FlagNone,
        // Categories
        FlagBracket = 1 << 0, FlagNotBracket = 1 << 1,
        FlagDelimiter = 1 << 2, FlagNotDelimiter = 1 << 3,
        FlagIdentifier = 1 << 4, FlagNotIdentifier = 1 << 5,
        FlagUnknown = 1 << 6, FlagNotUnknown = 1 << 7,
        FlagValid = 1 << 8, FlagNotValid = 1 << 9,
        // Enclosed
        FlagEnclosed = 1 << 10, FlagNotEnclosed = 1 << 11,
        // Masks
        FlagMaskCategories = FlagBracket | FlagNotBracket |
                             FlagDelimiter | FlagNotDelimiter |
                             FlagIdentifier | FlagNotIdentifier |
                             FlagUnknown | FlagNotUnknown |
                             FlagValid | FlagNotValid,
        FlagMaskEnclosed = FlagEnclosed | FlagNotEnclosed,
    };
}