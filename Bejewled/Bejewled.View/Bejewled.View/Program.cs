namespace Bejewled.View
{
    internal static class Program
    {

        private static void Main(string[] args)
        {
            using (var game = new BejeweledView())
            {
                game.Run();
            }
        }
    }
}