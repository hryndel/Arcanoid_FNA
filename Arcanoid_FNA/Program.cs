namespace Arcanoid_FNA
{
    static class Program
    {
        static void Main()
        {
            using (BL arcanoid = new BL())
            {
                arcanoid.Run();
            }
        }
    }
}
