using System.IO;
using System.Text.RegularExpressions;

namespace Дискретная_математика___Свойства_отношений
{
    internal class Program
    {
        int ChekInput()
        {
            bool isCorectInput = false;

            string str;

            int number;

            do
            {
                isCorectInput = Int32.TryParse(str = Console.ReadLine(), out number);
            } while (!isCorectInput);

            return number;
        }

        void ProcessingString(string str, ref bool[,] matrix, int i)
        {
            Regex regex = new Regex("[0-1]");

            MatchCollection matches = regex.Matches(str);

            int j = 0;

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {

                    switch (match.Value)
                    {
                        case "0":
                            {
                                matrix[i, j] = false;
                                break;
                            }
                        case "1":
                            {
                                matrix[i, j] = true;
                                break;
                            }
                    }

                    j++;
                }
            }
            else
            {
                Console.WriteLine("Совпадений не найдено");
            }
        }
        void WritingDataFile(ref bool[,] matrix)
        {
            StreamReader file = new StreamReader("C:\\Users\\Asus\\source\\repos\\lab_disc_2\\lab_disc_2\\matrix.txt");

            string str;

            int count = 0;

            while (!file.EndOfStream)
            {
                str = file.ReadLine();
                ProcessingString(str, ref matrix, count);
                count++;
            }

            file.Close();
        }

        void InputMatrix(ref bool[,] matrix)
        {
            string strin;

            for (int str = 0; str < 6; str++)
            {
                for (int col = 0; col < 6; col++)
                {
                    strin = Console.ReadLine();
                    if (strin == "0" || strin == "1")
                    {
                        if (strin == "0")
                        {
                            matrix[str, col] = false;
                        }
                        else
                        {
                            matrix[str, col] = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Введите другое значение");
                        col--;
                    }
                }
            }
        }

        void PrintMatrix(ref bool[,] matrix)
        {
            if (matrix.Length != 0)
            {
                Console.WriteLine("Матрица отношений: ");

                for (int i = 0; i < 6; i++)
                {
                    Console.Write('|');
                    for (int j = 0; j < 6; j++)
                    {
                        if (matrix[i, j])
                        {
                            Console.Write(1);
                        }
                        else
                        {
                            Console.Write(0);
                        }

                        Console.Write('\t');
                    }
                    Console.WriteLine('|');
                }
            }
            else
            {
                Console.WriteLine("Матрица отношений пуста");
            }
        }

        void AnalysisMatrix(ref bool[,] matrix)
        {
            bool isChekReflex = true;

            CheckReflex(ref matrix, ref isChekReflex);

            ChekSymmetry(ref matrix, ref isChekReflex);

            ChekTransitivity(ref matrix);

            ChekConnectivity(ref matrix);
        }

        void CheckReflex(ref bool[,] matrix, ref bool isChekReflex)
        {
            int count = 0;

            bool isChange = true;
            bool isTemp = true;

            while (count < 6 && isChange)
            {
                if (matrix[count, count])
                {
                    isChekReflex = true;
                }
                else
                {
                    isChekReflex = false;
                }

                if (count != 0)
                {
                    if (!(isTemp == isChekReflex))
                    {
                        isChange = false;
                    }
                }
                else
                {
                    isTemp = isChekReflex;
                }

                count++;
            }

            if (isChange)
            {
                if (isChekReflex)
                {
                    Console.WriteLine("1) Матрица рефлексивная. На главной диагонали 1");
                }
                else
                {
                    Console.WriteLine("1) Матрица антирефлексивная. На главной диагонали 0");
                }
            }
            else
            {
                Console.WriteLine("1) Не выполняется");
                isChekReflex = true;
            }
        }

        void ChekSymmetry(ref bool[,] matrix, ref bool isChekReflex) // При 7 или 6 не прочитывает смену
        {
            bool isChekSymmetry = true;

            bool isChange = true;

            bool isTemp = true;

            int str = 0, col = 0;

            while (str < 6 && isChange)
            {
                while (col < 6 && isChange)
                {
                    if (str != col)
                    {
                        if (matrix[str, col] == matrix[col, str])
                        {
                            isChekSymmetry = true;
                        }
                        else
                        {
                            isChekSymmetry = false;
                        }

                        if (str == 0 && col == 1)
                        {
                            isTemp = isChekSymmetry;
                        }
                        else
                        {
                            if (isTemp != isChekSymmetry)
                            {
                                isChange = false;
                            }
                        }
                    }
                    col++;
                }
                str++;
                col = str;
            }

            if (!isChange)
            {
                Console.WriteLine("2) Не выполняется");
            }
            else
            {
                if (isChekSymmetry && isChekReflex)
                {
                    Console.WriteLine("2) Матрица симметричная");
                }
                else
                {
                    if (isChekReflex)
                    {
                        Console.WriteLine("2) Матрица антисимметричная");
                    }
                    else
                    {
                        Console.WriteLine("2) Матрица асимметриная");
                    }
                }
            }
        }

        void ChekTransitivity(ref bool[,] matrix)
        {
            int x = 0, y = 0, z = 0;

            bool isChekTransit = true;

            while (x < 6 && isChekTransit)
            {
                while (y < 6 && isChekTransit)
                {
                    if (x != y)
                    {
                        while (z < 6 && isChekTransit)
                        {
                            if (y != z)
                            {
                                if (matrix[x, y])
                                {
                                    if (matrix[y, z])
                                    {
                                        isChekTransit = matrix[x, z] == matrix[y, z];
                                    }
                                }
                            }
                            z++;
                        }
                    }
                    y++;
                    z = 0;
                }
                x++;
                y = 0;
            }

            if (isChekTransit)
            {
                Console.WriteLine("3) Матрица транзитивная");
            }
            else
            {
                Console.WriteLine("3) Матрица не транзитивная");
            }
        }

        void ChekConnectivity(ref bool[,] matrix)
        {
            bool isChekConnectivety = true;

            int str = 0, col = 0;

            while (str < 6 && isChekConnectivety)
            {
                while (col < 6 && isChekConnectivety)
                {
                    if (str != col)
                    {
                        if (!matrix[str, col])
                        {
                            if (!matrix[col, str])
                            {
                                isChekConnectivety = false;
                            }
                        }
                    }
                    col++;
                }
                str++;
                col = str;
            }

            if (isChekConnectivety)
            {
                Console.WriteLine("4) Матрица связная");
            }
            else
            {
                Console.WriteLine("4) Матрица несвязная");
            }
        }

        void Menu()
        {
            bool isEnd = true;

            bool[,] matrix = new bool[6, 6];

            do
            {
                Console.WriteLine('\t' + "Меню");
                Console.WriteLine("1) Извлечение данных из файла");
                Console.WriteLine("2) Заполнение матрицы вручную");
                Console.WriteLine("3) Отображение системы отношений");
                Console.WriteLine("4) Анализ свойств отношений");
                Console.WriteLine("5) Законьчить работу");
                Console.WriteLine("Введите номер действия");
                switch (ChekInput())
                {
                    case 1:
                        {
                            WritingDataFile(ref matrix);
                            break;
                        }
                    case 2:
                        {
                            InputMatrix(ref matrix);
                            break;
                        }
                    case 3:
                        {
                            PrintMatrix(ref matrix);
                            break;
                        }
                    case 4:
                        {
                            AnalysisMatrix(ref matrix);
                            break;
                        }
                    case 5:
                        {
                            isEnd = false;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Введите номер действия из списка.");
                            break;
                        }
                };

            } while (isEnd);
        }

        static void Main(string[] args)
        {
            Program main = new Program();

            main.Menu();
        }
    }
}