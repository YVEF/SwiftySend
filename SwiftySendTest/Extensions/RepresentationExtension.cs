namespace SwiftySendTest.Extensions
{
    internal static class RepresentationExtension
    {
        public static string ToBooleanString(this bool @bool) =>
            @bool.ToString().ToLowerInvariant();
    }
}
