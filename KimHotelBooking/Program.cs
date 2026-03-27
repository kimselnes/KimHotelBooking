namespace KimHotelBooking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var app = new Application();
            app.Run();

            // ÄNDRA 'Initial migration' till något bättre beskrivande
            // När man uppdaterar strukturen på databasen.
            // add-migration "Initial migration"
        }
    }
}
