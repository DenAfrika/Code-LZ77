using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class LZ77
{
    public static int BufferSize = 5;
    public static int WindowSize = 14;

    public struct Node
    {
        public int Offset;
        public int Length;
        public Char Next;
    }

    public static void Main()
    {
        Console.WriteLine("Введите строку или нажмите Enter для просмотра тестов: ");
        String s = Console.ReadLine();
        if (s.Length > 0)
        {
            var resList = Encode(s);

            foreach (var res in resList)
            {
                Console.WriteLine("{0}\t{1}\t{2}", res.Offset, res.Length, res.Next);
            }
            Decode(resList);
        }
        else
        {
            Console.WriteLine("Тест 1 'красная краска'");
            var resList = Encode("красная краска");

            foreach (var res in resList)
            {
                Console.WriteLine("{0}\t{1}\t{2}", res.Offset, res.Length, res.Next);
            }
            Decode(resList);
            Console.WriteLine("Тест 2 'красивая краска'");
            resList = Encode("красивая краска");

            foreach (var res in resList)
            {
                Console.WriteLine("{0}\t{1}\t{2}", res.Offset, res.Length, res.Next);
            }
            Decode(resList);
            Console.WriteLine("Тест 3 'helloooooo'");
            resList = Encode("helloooooo");

            foreach (var res in resList)
            {
                Console.WriteLine("{0}\t{1}\t{2}", res.Offset, res.Length, res.Next);
            }
            Decode(resList);
        }
        
    }

    public static List<Node> Encode(String s)
    {
        var resList = new List<Node>();

        int pos = 0;

        while (pos < s.Length)
        {
            int offset, length;
            
            FindMatching(s, pos, out offset, out length);
            pos += length + 1;
            if (pos <= s.Length) 
                resList.Add(new Node() { Offset = offset, Length = length, Next = s[pos - 1] });
            else 
                resList.Add(new Node() { Offset = offset, Length = length, Next = new Char() });
        }

        return resList;
    }
    public static void FindMatching(String s, int pos, out int offset, out int length) // Поиск совпадения
    {
        offset = 0;
        length = 0;

        int endOfBuffer = Math.Min(pos + 1 + BufferSize, s.Length + 1);

        int bestMatchDistance = -1;
        int bestMatchLength = -1;

        for (int j = pos + 1; j < endOfBuffer; j++)
        {
            String part = s.Substring(pos, j - pos);
            int startIndex = Math.Max(0, pos - WindowSize);

            for (int i = startIndex; i < pos; i++)
            {
                var repetitions = part.Length / (pos - i);
                var last = part.Length % (pos - i);
                var matchedString = String.Concat(Enumerable.Repeat(s.Substring(i, pos - i), repetitions)) + s.Substring(i, last);

                if (matchedString == part && part.Length >= bestMatchLength)
                {
                    bestMatchDistance = pos - i;
                    bestMatchLength = part.Length;
                }
            }
        }

        if (bestMatchDistance > 0 && bestMatchLength > 0)
        { 
            offset = bestMatchDistance;
            length = bestMatchLength;
        }
    }
    public static void Decode(List<Node> resList)
    {
        string res = "";
        int i = 0;
        foreach (var list in resList)
        {
            if (list.Offset == 0 && list.Length == 0)
            {
                res += list.Next;
            }
            else
            {
                if (list.Offset >= list.Length)
                    res += res.Substring(res.Length - list.Offset, list.Length) + list.Next;
                else
                {
                    i = list.Length + 1;
                    while (list.Offset < i)
                    {
                        res += res.Substring(res.Length - list.Offset, list.Offset);
                        i -= list.Offset;
                    }
                    res += list.Next;
                }
            }
        }
        Console.WriteLine(res);
    }
}