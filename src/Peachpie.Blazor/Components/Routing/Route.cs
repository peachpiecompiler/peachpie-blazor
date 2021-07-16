using System;

namespace Peachpie.Blazor
{
    /// <summary>
    /// The class represents a routable component.
    /// </summary>
    public class Route
    {
        public Type Handler;
        public string[] UriSegments;

        public Route(Type handler, string[] uriSegments)
        {
            Handler = handler;
            UriSegments = uriSegments;
        }

        public MatchResult Match(string[] segments)
        {
            if (segments.Length != UriSegments.Length)
                return MatchResult.NoMatch();

            for (var i = 0; i < UriSegments.Length - 1; i++)
            {
                if (string.Compare(segments[i], UriSegments[i], StringComparison.OrdinalIgnoreCase) != 0)
                {
                    return MatchResult.NoMatch();
                }
            }

            if (string.Compare(UriSegments[UriSegments.Length - 1], segments[segments.Length - 1], StringComparison.OrdinalIgnoreCase) != 0)
                return MatchResult.NoMatch();
            else
                return MatchResult.Match(this);
        }
    }
}
