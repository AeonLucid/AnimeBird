using System.Collections.Generic;
using AnimeBird.Parser.Tokens;

namespace AnimeBird.Parser.Extensions
{
    internal static class LinkedListNodeTokenExtensions
    {

        public static LinkedListNode<Token> PreviousUntilNot(this LinkedListNode<Token> tokenNode, TokenCategory category)
        {
            var foundToken = false;
            while (!foundToken)
            {
                tokenNode = tokenNode.Previous;

                if (tokenNode == null)
                    break;

                if (tokenNode.Value.Category != category)
                    foundToken = true;
            }

            return foundToken ? tokenNode : null;
        }

        public static LinkedListNode<Token> NextUntilNot(this LinkedListNode<Token> tokenNode, TokenCategory category)
        {
            var foundToken = false;
            while (!foundToken)
            {
                tokenNode = tokenNode.Next;

                if (tokenNode == null)
                    break;

                if (tokenNode.Value.Category != category)
                    foundToken = true;
            }

            return foundToken ? tokenNode : null;
        }

    }
}
