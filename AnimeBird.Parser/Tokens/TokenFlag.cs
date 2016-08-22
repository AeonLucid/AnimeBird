namespace AnimeBird.Parser.Tokens
{
    internal enum TokenFlag
    {
        None,
        // Categories
        Bracket = 1 << 0, NotBracket = 1 << 1,
        Delimiter = 1 << 2, NotDelimiter = 1 << 3,
        Identifier = 1 << 4, NotIdentifier = 1 << 5,
        Unknown = 1 << 6, NotUnknown = 1 << 7,
        Valid = 1 << 8, NotValid = 1 << 9,
        // Enclosed
        Enclosed = 1 << 10, NotEnclosed = 1 << 11,
        // Masks
        MaskCategories = Bracket | NotBracket |
                             Delimiter | NotDelimiter |
                             Identifier | NotIdentifier |
                             Unknown | NotUnknown |
                             Valid | NotValid,
        MaskEnclosed = Enclosed | NotEnclosed,
    };
}