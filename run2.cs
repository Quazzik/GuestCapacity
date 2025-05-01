
using System;
using System.Collections.Generic;
using System.Linq;


class Program
{
    // Константы для символов ключей и дверей
    static readonly char[] keys_char = Enumerable.Range('a', 26).Select(i => (char)i).ToArray();
    static readonly char[] doors_char = keys_char.Select(char.ToUpper).ToArray();

    // Метод для чтения входных данных
    static List<List<char>> GetInput()
    {
        var data = new List<List<char>>();
        string line;
        while ((line = Console.ReadLine()) != null && line != "")
        {
            data.Add(line.ToCharArray().ToList());
        }
        return data;
    }

    static int Solve(List<List<char>> grid)
    {
        int rows = grid.Count;
        int cols = grid[0].Count;
        var directions = new (int dx, int dy)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

        var startPositions = new List<(int x, int y)>();
        int allKeys = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i][j] == '@')
                {
                    startPositions.Add((i, j));
                }
                else if (char.IsLower(grid[i][j]))
                {
                    allKeys |= (1 << (grid[i][j] - 'a'));
                }
            }
        }

        var queue = new Queue<(List<(int x, int y)> positions, int keys, int steps)>();
        var visited = new HashSet<string>();

        var startState = (positions: startPositions, keys: 0, steps: 0);
        queue.Enqueue(startState);
        visited.Add(EncodeState(startPositions, 0));

        while (queue.Count > 0)
        {
            var (positions, keys, steps) = queue.Dequeue();

            if (keys == allKeys)
                return steps; //Made in collaboration with ChatGPT and Habr community, Sorry :(

            for (int i = 0; i < 4; i++)
            {
                var (x, y) = positions[i];

                foreach (var (dx, dy) in directions)
                {
                    int newX = x + dx;
                    int newY = y + dy;

                    if (newX < 0 || newY < 0 || newX >= rows || newY >= cols)
                        continue;

                    char cell = grid[newX][newY];
                    if (cell == '#') continue;

                    if (char.IsUpper(cell) && ((keys & (1 << (cell - 'A'))) == 0))
                        continue;

                    int newKeys = keys;
                    if (char.IsLower(cell))
                        newKeys |= (1 << (cell - 'a'));

                    var newPositions = new List<(int x, int y)>(positions);
                    newPositions[i] = (newX, newY);

                    string stateKey = EncodeState(newPositions, newKeys);
                    if (!visited.Contains(stateKey))
                    {
                        visited.Add(stateKey);
                        queue.Enqueue((newPositions, newKeys, steps + 1));
                    }
                }
            }
        }

        return -1;
    }

    // Кодирует состояние: позиции роботов и собранные ключи
    static string EncodeState(List<(int x, int y)> positions, int keys)
    {
        return string.Join(",", positions.Select(p => $"{p.x}-{p.y}")) + $"|{keys}";
    }

    static void Main()
    {
        var data = GetInput();
        int result = Solve(data);

        if (result == -1)
        {
            Console.WriteLine("No solution found");
        }
        else
        {
            Console.WriteLine(result);
        }
    }
}