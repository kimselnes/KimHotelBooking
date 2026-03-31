namespace KimHotelBooking.Tools
{
    public class Menu
    {
        // USER TEMPLATE

        //var prompt = "prompt";
        //List<string> options = ["option1", "option2", "option3"];

        //Menu menu = new Menu(prompt, options);
        //int selectedIndex = menu.Run();

        // And then use switch(selectedIndex) as an example.
        // ---------------------------------------------------------------------

        private int SelectedIndex { get; set; }
        //private string[] Options { get; set; }
        private List<string> Options { get; set; }
        private string Prompt { get; set; }

        public Menu(string prompt, List<string> options)
        {
            Prompt = prompt;
            Options = options;
            SelectedIndex = 0;
        }

        private void DisplayOptions()
        {
            Console.WriteLine(Prompt);
            Console.WriteLine();

            for (int i = 0; i < Options.Count; i++)
            {
                string currentOption = Options[i].ToString();
                //string prefix;

                if (i == SelectedIndex)
                {
                    //prefix = ">";
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                }
                else
                {
                    //prefix = " ";
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                //Console.WriteLine($"{prefix}{currentOption}");
                Console.WriteLine($"  {currentOption}  ");
            }
            Console.ResetColor();
        }

        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                DisplayOptions();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                    if (SelectedIndex == -1)
                    {
                        SelectedIndex = Options.Count - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    if (SelectedIndex == Options.Count)
                    {
                        SelectedIndex = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.Escape)
                {
                    return -1;
                }
            }
            while (keyPressed != ConsoleKey.Enter);

            return SelectedIndex;
        }
    }
}
