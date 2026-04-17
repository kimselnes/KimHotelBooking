using KimHotelBooking.Data;
using KimHotelBooking.Models;
using KimHotelBooking.Repositories;
using KimHotelBooking.Tools;

namespace KimHotelBooking.Service
{
    public class RoomService
    {

        public void Create(Room room)
        {
            room.PricePerNight = DeterminePricePerNight(room);

            var roomRep = new RoomRepository();
            roomRep.AddToDatabase(room);
        }

        public static string SetRoomType()
        {
            var type = "";

            var setTypePrompt = "Ska rummet vara Single eller Double";
            List<string> setTypeOptions =
            [
                "Single",
                "Double"
            ];

            Menu setTypeMenu = new Menu(setTypePrompt, setTypeOptions);
            int setTypeSelectedIndex = setTypeMenu.Run();

            switch (setTypeSelectedIndex)
            {
                case 0:
                    type = "Single";

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nDu har anget {type}");
                    Console.ResetColor();
                    Utility.WaitForKeyPress();
                    break;
                case 1:
                    type = "Double";
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nDu har anget {type}");
                    Console.ResetColor();
                    Utility.WaitForKeyPress();
                    break;
            }

            return type;
        }

        public static int SetRoomSquareMeters(int currentSquareMeters)
        {
            Console.WriteLine("Välj hur många kvadratmeter rummet är (20-40)m2\n");
            Console.Write("Kvadratmeter: ");

            if (!Int32.TryParse(Console.ReadLine(), out var squareMeters))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nDu angav inte ett heltal");
                Console.ResetColor();
                Utility.WaitForKeyPress();

                return currentSquareMeters;
            }
            else
            {
                if (squareMeters < 20 || squareMeters > 40)
                {
                    Console.WriteLine(
                        "\nKvadratmeter kan bara vara mellan 20 och 40");
                    Console.ResetColor();
                    Utility.WaitForKeyPress();

                    return currentSquareMeters;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nDu har anget {squareMeters} kvadratmeter");
                    Console.ResetColor();
                    Utility.WaitForKeyPress();
                }
            }

            return squareMeters;
        }

        public static bool CheckCorrectBeds(Room room)
        {
            if (CheckCorrectBedsType(room))
            {
                if (CheckCorrectBedsSquareMeters(room))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool CheckCorrectBedsType(Room room)
        {
            if (room.Type == "Single" && room.Beds > 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nDu kan inte ha mer än 1 säng i 'Single' rum.");
                Console.ResetColor();
                Utility.WaitForKeyPress();

                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool CheckCorrectBedsSquareMeters(Room room)
        {
            if (room.SquareMeters < 40 && room.Beds == 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nDu kan inte ha mer än 2 sängar under 40 m2.");
                Console.ResetColor();
                Utility.WaitForKeyPress();

                return false;
            }
            else if (room.SquareMeters < 30 && room.Beds == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nDu kan inte ha mer än 1 säng under 30 m2.");
                Console.ResetColor();
                Utility.WaitForKeyPress();

                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool SetRoomIsActive()
        {
            var active = false;

            var setActivePrompt = "Ska rummet vara AKTIVT eller INTE";
            List<string> setActiveOptions =
            [
                "Aktiv",
                "INTE aktiv"
            ];

            Menu setActiveMenu = new Menu(setActivePrompt, setActiveOptions);
            int setActiveSelectedIndex = setActiveMenu.Run();

            switch (setActiveSelectedIndex)
            {
                case 0:
                    active = true;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nDu har angett Aktiv");
                    Console.ResetColor();
                    Utility.WaitForKeyPress();
                    break;
                case 1:
                    active = false;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nDu har angett INTE aktiv");
                    Console.ResetColor();
                    Utility.WaitForKeyPress();
                    break;
            }

            return active;
        }

        public static List<Room> GetAllRoomsList()
        {
            var roomRep = new RoomRepository();
            var room = roomRep.GetAllRooms();

            return room;
        }

        public static List<string> RoomsListToString()
        {
            List<string> roomsString = new List<string>();
            var roomsList = GetAllRoomsList();

            foreach (var room in roomsList)
            {
                var tempString = $"{room.Id}. {room.Type}-rum " +
                    $"({room.SquareMeters} kvadrat - {room.Beds} säng/sängar) " +
                    $"[{(room.IsActive == true ? "\u001b[32mAKTIV\u001b[0m" : "\u001b[31mOAKTIV\u001b[0m")}]";

                roomsString.Add(tempString);
            }

            return roomsString;
        }

        public static List<string> RoomsListToStringForBooking(List<Room> roomsList)
        {
            List<string> roomsString = new List<string>();

            foreach (var room in roomsList)
            {
                var tempString = $"{room.Type}-rum " +
                    $"({room.SquareMeters} kvadrat) " +
                    $"{room.PricePerNight} SEK/natt";

                roomsString.Add(tempString);
            }

            return roomsString;
        }

        public static Room GetRoomById(int id)
        {
            var roomRep = new RoomRepository();
            var room = roomRep.GetById(id);

            return room;
        }

        public decimal DeterminePricePerNight(Room room)
        {
            var pricePerNight = 0m;

            // Price accoring to Single or Double room.
            if (room.Type == "Single")
            {
                pricePerNight += 200;
            }
            else
            {
                pricePerNight += 300;
            }

            // Price according to roomsize.
            pricePerNight += room.SquareMeters * 10;

            // Price according to number of beds.
            pricePerNight += room.Beds * 100;

            return pricePerNight;
        }

        public void CheckAvailable(Booking booking)
        {
            var allRooms = GetAllRoomsList();

            var availableRooms = allRooms.Select(r => r.Bookings);
        }
    }
}
