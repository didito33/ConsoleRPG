using System.Security.Cryptography.X509Certificates;
using System.Threading;

using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

using RPGInterviewTask.Data;
using RPGInterviewTask.DTOs;
using RPGInterviewTask.Races;

using static System.Net.Mime.MediaTypeNames;
#nullable disable
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome!");
        Console.WriteLine("Press any key to play.");
        var RK = Console.ReadKey();
        Console.WriteLine("Choose character type:\r\n\tOptions: \r\n\t1) Warrior \r\n\t2) Archer \r\n\t3) Mage \r\n\tYour pick: \r\n");
        Race character = null;
        string raceInput = Console.ReadLine();
        while (true)
        {
            if (raceInput == "1")
            {
                character = new Warrior();
                break;
            }
            else if (raceInput == "2")
            {
                character = new Archer();
                break;
            }
            else if (raceInput == "3")
            {
                character = new Mage();
                break;
            }
            else 
            { 
                Console.WriteLine("Please select an available class!");
                raceInput = Console.ReadLine();
            }
        }


        Console.WriteLine("Would you like to buff up your stats before starting?                    " +
            "(Limit: 3 points total)\r\n\tResponse (Y\\N): ");
        string input = Console.ReadLine();
        if (input.ToUpper() == "Y")
        {
            int statIncrease = 3;
            while (statIncrease > 0)
            {
                statIncrease = StatsIncreaseCycle(input, statIncrease, character);
            }
        }
        character.Setup();
        //Console.WriteLine("\nYour stats look like this: ");
        //Console.WriteLine($"Str - {character.Strength}");
        //Console.WriteLine($"Agi - {character.Agility}");
        //Console.WriteLine($"Intellect - {character.Intelligence}");
        //Console.WriteLine($"Hp - {character.Health}");
        //Console.WriteLine($"Mana - {character.Mana}");
        //Console.WriteLine($"Dmg - {character.Damage}");

        ConsoleRPGDatabase db = new ConsoleRPGDatabase();
        Character charDto = new Character()
        {
            Race = character.GetType().Name,
            Health = character.Health,
            Mana = character.Mana,
            Damage = character.Damage,
            Strength = character.Strength,
            Agility = character.Agility,
            Intelligence = character.Intelligence,
            Range = character.Range,
            CreatedOn = DateTime.UtcNow
        };
        db.Add(charDto);
        db.SaveChanges();

        char[,] matrix = new char[10, 10];

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                matrix[row, col] = '▒';
            }
        }

        matrix[character.PositionRow, character.PositionCol] = character.Symbol;
        PrintMatrix(matrix);

        List<Monster> monsters = new List<Monster>();
        while (character.Health > 0 && !isFinalReached(character))
        {
            Console.WriteLine("\nChoose action: \n 1) Attack \n 2) Move");
            string inputAction = Console.ReadLine();
            if(inputAction == "1")
            {
                AttackMonsters(monsters, character, matrix, inputAction);
            }
            else if (inputAction == "2")
            {
                Console.WriteLine("The available options are: W A S D E Q X Z");
                string inputDirection = Console.ReadLine();
                matrix[character.PositionRow, character.PositionCol] = '▒';
                if(inputDirection.ToUpper() == "W" )
                {
                    if (IsInField(matrix ,character.PositionRow - 1, character.PositionCol))
                    {
                        
                        character.PositionRow -= 1;
                        if(IsFieldAnEnemy(matrix ,character.PositionRow, character.PositionCol))
                        {
                            character.Health -= monsters.FirstOrDefault(x => x.PositionRow == character.PositionRow
                            && x.PositionCol == character.PositionCol).Damage;
                        }

                        matrix[character.PositionRow, character.PositionCol] = character.Symbol;
                        
                    }
                    else { Console.WriteLine("You just went out of the field, try again."); continue; }
                }
                else if (inputDirection.ToUpper() == "S")
                {
                    if (IsInField(matrix, character.PositionRow + 1, character.PositionCol))
                    {
                        character.PositionRow += 1;
                        if (IsFieldAnEnemy(matrix, character.PositionRow, character.PositionCol))
                        {
                            character.Health -= monsters.FirstOrDefault(x => x.PositionRow == character.PositionRow
                            && x.PositionCol == character.PositionCol).Damage;
                        }

                        matrix[character.PositionRow, character.PositionCol] = character.Symbol;
                    }
                    else { Console.WriteLine("You just went out of the field, try again."); continue; }
                }
                else if (inputDirection.ToUpper() == "A")
                {
                    if (IsInField(matrix, character.PositionRow, character.PositionCol - 1))
                    {
                        character.PositionCol -= 1;
                        if (IsFieldAnEnemy(matrix, character.PositionRow, character.PositionCol))
                        {
                            character.Health -= monsters.FirstOrDefault(x => x.PositionRow == character.PositionRow
                            && x.PositionCol == character.PositionCol).Damage;
                        }

                        matrix[character.PositionRow, character.PositionCol] = character.Symbol;
                    }
                    else { Console.WriteLine("You just went out of the field, try again."); continue; }
                }
                else if (inputDirection.ToUpper() == "D")
                {
                    if (IsInField(matrix, character.PositionRow, character.PositionCol + 1))
                    {
                        character.PositionCol += 1;
                        if (IsFieldAnEnemy(matrix, character.PositionRow, character.PositionCol))
                        {
                            character.Health -= monsters.FirstOrDefault(x => x.PositionRow == character.PositionRow
                            && x.PositionCol == character.PositionCol).Damage;
                        }

                        matrix[character.PositionRow, character.PositionCol] = character.Symbol;
                    }
                    else { Console.WriteLine("You just went out of the field, try again."); continue; }
                }
                else if (inputDirection.ToUpper() == "E")
                {
                    if (IsInField(matrix, character.PositionRow - 1, character.PositionCol + 1))
                    {
                        character.PositionRow -= 1;
                        character.PositionCol += 1;
                        if (IsFieldAnEnemy(matrix, character.PositionRow, character.PositionCol))
                        {
                            character.Health -= monsters.FirstOrDefault(x => x.PositionRow == character.PositionRow
                            && x.PositionCol == character.PositionCol).Damage;
                        }

                        matrix[character.PositionRow, character.PositionCol] = character.Symbol;
                    }
                    else { Console.WriteLine("You just went out of the field, try again."); continue; }
                }
                else if (inputDirection.ToUpper() == "X")
                {
                    if (IsInField(matrix, character.PositionRow + 1, character.PositionCol + 1))
                    {
                        character.PositionRow += 1;
                        character.PositionCol += 1;
                        if (IsFieldAnEnemy(matrix, character.PositionRow, character.PositionCol))
                        {
                            character.Health -= monsters.FirstOrDefault(x => x.PositionRow == character.PositionRow
                            && x.PositionCol == character.PositionCol).Damage;
                        }

                        matrix[character.PositionRow, character.PositionCol] = character.Symbol;
                    }
                    else { Console.WriteLine("You just went out of the field, try again."); continue; }
                }
                else if (inputDirection.ToUpper() == "Q")
                {
                    if (IsInField(matrix, character.PositionRow - 1, character.PositionCol - 1))
                    {
                        character.PositionRow -= 1;
                        character.PositionCol -= 1;
                        if (IsFieldAnEnemy(matrix, character.PositionRow, character.PositionCol))
                        {
                            character.Health -= monsters.FirstOrDefault(x => x.PositionRow == character.PositionRow
                            && x.PositionCol == character.PositionCol).Damage;
                        }

                        matrix[character.PositionRow, character.PositionCol] = character.Symbol;
                    }
                    else { Console.WriteLine("You just went out of the field, try again."); continue; }
                }
                else if (inputDirection.ToUpper() == "Z")
                {
                    if (IsInField(matrix, character.PositionRow + 1, character.PositionCol - 1))
                    {
                        character.PositionRow += 1;
                        character.PositionCol -= 1;
                        if (IsFieldAnEnemy(matrix, character.PositionRow, character.PositionCol))
                        {
                            character.Health -= monsters.FirstOrDefault(x => x.PositionRow == character.PositionRow
                            && x.PositionCol == character.PositionCol).Damage;
                        }

                        matrix[character.PositionRow, character.PositionCol] = character.Symbol;
                    }
                    else { Console.WriteLine("You just went out of the field, try again."); continue; }
                }
                else
                {
                    Console.WriteLine("Please enter valid movement input.");
                    matrix[character.PositionRow, character.PositionCol] = character.Symbol;
                    continue;
                }

            }
            else
            {
                Console.WriteLine("Please enter valid input.");
                continue;
            }

            Monster monster = new Monster();
            if (!(monsters.Any(x=>(x.PositionRow == monster.PositionRow && x.PositionCol == monster.PositionCol))
                           || (monster.PositionRow == character.PositionRow && monster.PositionCol == character.PositionCol)))
            {
                monsters.Add(monster);
            }

            if (monsters.Any())
            {
                matrix[monsters.Last().PositionRow, monsters.Last().PositionCol] = monsters.Last().Symbol;
            }

            Console.WriteLine($"\nHealth: {character.Health}    Mana: {character.Mana} \n");
            PrintMatrix(matrix);
        }


    }
    public static bool IsFieldAnEnemy(char[,] matrix, int row, int col)
        => matrix[row, col] == 'M' ? true : false;
    public static bool IsInputCorrect(string input, int maxValue)
    {
        if (int.Parse(input) > maxValue)
        {
            Console.WriteLine($"The input should be lower or equal to {maxValue}. Try again. \n");
            return false;
        }
        return true;
    }
    public static int StatsIncreaseCycle(string input, int statIncrease, Race character)
    {
        bool done = false;
        for (int i = 1; i <= 3; i++)
        {
            if (done) break;
            switch (i)
            {
                case 1:
                    Console.WriteLine("Add to Strength: ");
                    input = Console.ReadLine();

                    if (!IsInputCorrect(input, statIncrease)) { i--; continue; }
                    character.Strength += int.Parse(input);
                    statIncrease -= int.Parse(input);

                    Console.WriteLine($"Remaining points: {statIncrease}");
                    if (statIncrease == 0)
                    {
                        done = true;
                        return statIncrease;
                    }
                    break;
                case 2:

                    Console.WriteLine("Add to Agility: ");
                    input = Console.ReadLine();

                    if (!IsInputCorrect(input, statIncrease)) { i--; continue; }
                    character.Agility += int.Parse(input);
                    statIncrease -= int.Parse(input);

                    Console.WriteLine($"Remaining points: {statIncrease}");
                    if (statIncrease == 0)
                    {
                        done = true;
                        return statIncrease;
                    }
                    break;
                case 3:
                    Console.WriteLine("Add to Intelligence: ");
                    input = Console.ReadLine();

                    if (!IsInputCorrect(input, statIncrease)) { i--; continue; }
                    character.Intelligence += int.Parse(input);
                    statIncrease -= int.Parse(input);

                    Console.WriteLine($"Remaining points: {statIncrease}");
                    if (statIncrease == 0)
                    {
                        done = true;
                        return statIncrease;
                    }
                    break;
            }
        }
        return statIncrease;
    }
    public static bool isFinalReached(Race character) =>
        character.PositionRow == 9 && character.PositionCol == 9 ? true : false;
    private static void PrintMatrix(char[,] matrix)
    {
        for (int row = 0; row < matrix.GetLength(0); row++)
        {
            for (int col = 0; col < matrix.GetLength(1); col++)
            {
                Console.Write(matrix[row, col]);
            }
            Console.WriteLine();
        }
    }
    private static bool IsInField(char[,] board, int row, int col)
    {
        return row >= 0 && row < board.GetLength(0) &&
            col >= 0 && col < board.GetLength(1);
    }
    private static void AttackMonsters(List<Monster> monsters, Race character, char[,] matrix, string inputAction)
    {
        if (monsters.Any(x =>
           (x.PositionCol == character.PositionCol && x.PositionRow >= character.PositionRow && x.PositionRow <= character.PositionRow + character.Range)
           || (x.PositionCol == character.PositionCol && x.PositionRow <= character.PositionRow && x.PositionRow >= character.PositionRow - character.Range)
           || (x.PositionRow == character.PositionRow && x.PositionCol >= character.PositionCol && x.PositionCol <= character.PositionCol + character.Range)
           || (x.PositionRow == character.PositionRow && x.PositionCol <= character.PositionCol && x.PositionCol >= character.PositionCol - character.Range)))
        {
            int i = 0;
            Monster[] currMonsters = new Monster[10];
            foreach (var monster in monsters.Where(x =>
                (x.PositionCol == character.PositionCol && x.PositionRow >= character.PositionRow && x.PositionRow <= character.PositionRow + character.Range)
                || (x.PositionCol == character.PositionCol && x.PositionRow <= character.PositionRow && x.PositionRow >= character.PositionRow - character.Range)
                || (x.PositionRow == character.PositionRow && x.PositionCol >= character.PositionCol && x.PositionCol <= character.PositionCol + character.Range)
                || (x.PositionRow == character.PositionRow && x.PositionCol <= character.PositionCol && x.PositionCol >= character.PositionCol - character.Range)))
            {
                Console.WriteLine($"{i} - [{monster.PositionRow},{monster.PositionCol}] target with remaining {monster.Health} blood.");
                currMonsters[i] = monster;
                i++;
            }
            Console.WriteLine("Which one to attack: ");
            inputAction = Console.ReadLine();
            if (int.Parse(inputAction) < i)
            {
                currMonsters[int.Parse(inputAction)].Health -= character.Damage;
                if (currMonsters[int.Parse(inputAction)].Health > 0)
                    character.Health -= currMonsters[int.Parse(inputAction)].Damage;

                var deadMonster = monsters.FirstOrDefault(x => x.Health <= 0);
                if (deadMonster != null)
                {
                    matrix[deadMonster.PositionRow, deadMonster.PositionCol] = '▒';
                    monsters.Remove(deadMonster);
                }

            }
        }
        else Console.WriteLine("There are no enemies nearby.");


    }
}