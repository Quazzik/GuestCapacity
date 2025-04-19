using System.Globalization;
using System.Text.RegularExpressions;


class HotelCapacity
{
    static bool CheckCapacity(int maxCapacity, List<Guest> guests)
    {
        {
            var guestsMoves = new List<(DateTime date, int delta)>();
            foreach (var guest in guests)
            {
                var checkIn = DateTime.ParseExact(guest.CheckIn, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var checkOut = DateTime.ParseExact(guest.CheckOut, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                guestsMoves.Add((checkIn, 1));
                guestsMoves.Add((checkOut, -1));
            }

            guestsMoves.Sort((a, b) =>
            {
                int compare = a.date.CompareTo(b.date);
                if (compare == 0) return a.delta.CompareTo(b.delta);
                return compare;
            });

            int currentGuests = 0;
            foreach (var move in guestsMoves)
            {
                currentGuests += move.delta;
                if (currentGuests > maxCapacity)
                    return false;
            }
            return true;
        }
    }

    class Guest
    {
        public string Name { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
    }

    static void Main()
    {
        int maxCapacity = int.Parse(Console.ReadLine());
        int n = int.Parse(Console.ReadLine());


        List<Guest> guests = new List<Guest>();


        for (int i = 0; i < n; i++)
        {
            string line = Console.ReadLine();
            Guest guest = ParseGuest(line);
            guests.Add(guest);
        }


        bool result = CheckCapacity(maxCapacity, guests);


        Console.WriteLine(result ? "True" : "False");
    }


    // Простой парсер JSON-строки для объекта Guest
    static Guest ParseGuest(string json)
    {
        var guest = new Guest();

        // Извлекаем имя
        Match nameMatch = Regex.Match(json, "\"name\"\\s*:\\s*\"([^\"]+)\"");
        if (nameMatch.Success)
            guest.Name = nameMatch.Groups[1].Value;
        else
            throw new Exception("Не удалось извлечь name из строки: " + json);

        // Извлекаем дату заезда
        Match checkInMatch = Regex.Match(json, "\"check-in\"\\s*:\\s*\"([^\"]+)\"");
        if (checkInMatch.Success)
            guest.CheckIn = checkInMatch.Groups[1].Value;
        else
            throw new Exception("Не удалось извлечь check-in из строки: " + json);

        // Извлекаем дату выезда
        Match checkOutMatch = Regex.Match(json, "\"check-out\"\\s*:\\s*\"([^\"]+)\"");
        if (checkOutMatch.Success)
            guest.CheckOut = checkOutMatch.Groups[1].Value;
        else
            throw new Exception("Не удалось извлечь check-out из строки: " + json);

        return guest;
    }
}