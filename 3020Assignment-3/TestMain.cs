// Team: Greg Prouty, Bradley Primeau, Avery Chin, Megan Risebrough

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3020Assignment_3
{
    internal class TestMain
    {
        static void Main()
        {
            Trie<int> T = new Trie<int>();

            T.Insert("bag", 10);
            T.Insert("bat", 20);
            T.Insert("cab", 70);
            T.Insert("bagel", 30);
            T.Insert("beet", 40);
            T.Insert("abc", 60);

            T.Print();

            Console.WriteLine(T.Size());

            Console.WriteLine(T.Value("abc"));
            Console.WriteLine(T.Value("beet"));
            Console.WriteLine(T.Value("a"));

            Console.WriteLine(T.Contains("baet"));
            Console.WriteLine(T.Contains("beet"));
            Console.WriteLine(T.Contains("abc"));

            T.Print();

            T.PrintTree();

            Console.WriteLine();
            Console.WriteLine("Removing:");

            Console.WriteLine("Removing 'bag' : " + T.Remove("bag"));
            Console.WriteLine("Removing 'bag' again : " + T.Remove("bag"));

            Console.WriteLine("Size: " + T.Size());
            T.Print();
            T.PrintTree();

            Console.WriteLine("Removing 'beet' : " + T.Remove("beet"));

            Console.WriteLine("Size: " + T.Size());
            T.Print();

            Console.WriteLine("Removing 'bat' : " + T.Remove("bat"));
            Console.WriteLine("Size: " + T.Size());
            T.Print();

            Console.WriteLine("Removing 'ab' : " + T.Remove("ab"));

            Console.WriteLine("Removing 'abc' : " + T.Remove("abc"));
            Console.WriteLine("Size: " + T.Size());
            T.Print();
            T.PrintTree();

            Console.WriteLine("Removing 'cab' : " + T.Remove("cab"));
            Console.WriteLine("Size: " + T.Size());
            T.Print();
            T.PrintTree();

            Console.WriteLine("Removing 'bagel' : " + T.Remove("bagel"));
            Console.WriteLine("Size: " + T.Size());
            T.Print();

            T.PrintTree();

            Console.ReadKey();
        }
    }
}
