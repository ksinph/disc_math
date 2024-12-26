using System;
using System.Collections.Generic;
using System.Linq;

public class Minimization
{
    private List<List<int>> beforePCNF = new List<List<int>>();  // СКНФ
    private List<List<int>> beforePDNF = new List<List<int>>();  // СДНФ
    public List<List<int>> resultGluing = new List<List<int>>(); // Результат склеивания
    private string vectBool;
    private int countVar;
    private List<List<int>> implicants = new List<List<int>>(); // Импликанты

    public void ReadVector()
    {
        Console.WriteLine("Введите вектор истинности:");
        vectBool = Console.ReadLine();
        countVar = (int)Math.Log2(vectBool.Length);
    }

    public void CreateTruthTable()
    {
        int numRows = 1 << countVar; 

        if (vectBool.Length != numRows)
        {
            throw new ArgumentException("Вектор истинности должен соответствовать числу переменных.");
        }
        Console.WriteLine("Таблица истинности:");
        Console.WriteLine(string.Join("\t", Enumerable.Range(1, countVar).Select(i => $"x{i}")) + "\tF");

        for (int i = 0; i < numRows; i++) 
        {
            var tmp = new List<int>();
            for (int j = countVar - 1; j >= 0; j--) 
            {
                tmp.Add((i >> j) & 1);
            }

            
            Console.WriteLine(string.Join("\t", tmp) + $"\t{vectBool[i]}");

            
            if (vectBool[i] == '0')
                beforePCNF.Add(tmp); 
            else
                beforePDNF.Add(tmp); 
        }
    }


    public void ShowNF()
    {
        // Вывод СКНФ и СДНФ
        Console.WriteLine("СКНФ:");
        foreach (var row in beforePCNF)
        {
            Console.WriteLine(string.Join("", row));
        }
        Console.WriteLine("СДНФ:");
        foreach (var row in beforePDNF)
        {
            Console.WriteLine(string.Join("", row));
        }
    }

    private bool CanCombine(List<int> a, List<int> b, out List<int> result)
    {
        // Проверка возможности склеивания двух терминов
        result = new List<int>(a);
        int diffCount = 0;

        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i])
            {
                if (a[i] == -1 || b[i] == -1)
                    return false;

                diffCount++;
                result[i] = -1;
            }
        }

        return diffCount == 1;
    }

    private List<List<int>> CombineTerms(List<List<int>> terms)
    {
        // Склеивание терминов
        var newTerms = new List<List<int>>();
        var usedFlag = new bool[terms.Count];

        for (int i = 0; i < terms.Count; i++)
        {
            for (int j = i + 1; j < terms.Count; j++)
            {
                if (CanCombine(terms[i], terms[j], out var combined))
                {
                    newTerms.Add(combined);
                    usedFlag[i] = true;
                    usedFlag[j] = true;
                }
            }
        }

        for (int i = 0; i < terms.Count; i++)
        {
            if (!usedFlag[i])
            {
                newTerms.Add(terms[i]);
            }
        }

        return newTerms;
    }

    public void Gluing()
    {
        implicants = new List<List<int>>(beforePDNF);

        while (true)
        {
            var newImplicants = CombineTerms(implicants);

            if (newImplicants.SequenceEqual(implicants))
                break;

            implicants = newImplicants;
        }

        resultGluing = implicants;
    }

    public void PrintAllCombinedTerms()
    {
        // Вывод всех этапов склеивания
        Console.WriteLine("Склеивание:");
        foreach (var term in resultGluing)
        {
            Console.WriteLine(TermToString(term));
        }
    }

    public List<List<int>> BuildImplicantMatrix()
    {
        // Создание матрицы импликант
        var matrix = new List<List<int>>(resultGluing.Count);

        for (int i = 0; i < resultGluing.Count; i++)
        {
            var row = new List<int>(beforePDNF.Count);
            for (int j = 0; j < beforePDNF.Count; j++)
            {
                bool covers = true;
                for (int k = 0; k < beforePDNF[j].Count; k++)
                {
                    if (resultGluing[i][k] != -1 && resultGluing[i][k] != beforePDNF[j][k])
                    {
                        covers = false;
                        break;
                    }
                }
                row.Add(covers ? 1 : 0);
            }
            matrix.Add(row);
        }

        return matrix;
    }

    public List<int> MinimizeCover(List<List<int>> matrix)
    {
        // Минимизация покрытия
        var result = new List<int>();
        var covered = new bool[matrix[0].Count];

        while (covered.Any(c => !c))
        {
            int bestRow = -1, maxCover = -1;

            for (int i = 0; i < matrix.Count; i++)
            {
                int coverCount = 0;

                for (int j = 0; j < matrix[i].Count; j++)
                {
                    if (matrix[i][j] == 1 && !covered[j])
                        coverCount++;
                }

                if (coverCount > maxCover)
                {
                    maxCover = coverCount;
                    bestRow = i;
                }
            }

            if (bestRow != -1)
            {
                result.Add(bestRow);
                for (int j = 0; j < matrix[bestRow].Count; j++)
                {
                    if (matrix[bestRow][j] == 1)
                        covered[j] = true;
                }
            }
        }

        return result;
    }

    public void PrintImplicants()
    {
        Console.WriteLine("Итоговые импликанты:");
        foreach (var term in resultGluing)
        {
            Console.WriteLine(TermToString(term));
        }
    }

    public string TermToString(List<int> term) 
    {
        return string.Join("", term.Select(t => t == -1 ? "-" : t.ToString()));
    }

    public void PrintMatrix(List<List<int>> matrix)
    {
        Console.WriteLine("Матрица импликант:");
        foreach (var row in matrix)
        {
            Console.WriteLine(string.Join(" ", row));
        }
    }
}

class Program
{
    static void Main()
    {
        var minimization = new Minimization();
        minimization.ReadVector();
        minimization.CreateTruthTable();
        Console.WriteLine();
        minimization.ShowNF();
        minimization.Gluing();
        minimization.PrintAllCombinedTerms();
        var matrix = minimization.BuildImplicantMatrix();
        minimization.PrintMatrix(matrix);
        var minimizedCover = minimization.MinimizeCover(matrix);
        Console.WriteLine("Минимизированная функция:");
        foreach (var idx in minimizedCover)
        {
            Console.WriteLine(minimization.TermToString(minimization.resultGluing[idx]));
        }
        // Вывод итоговых импликантов
        minimization.PrintImplicants();
    }
}
