// Team: Greg Prouty, Bradley Primeau, Avery Chin, Megan Risebrough

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3020Assignment_3
{
    public interface IContainer<T>
    {
        void MakeEmpty();
        bool Empty();
        int Size();
    }

    //-------------------------------------------------------------------------

    public interface ITrie<T> : IContainer<T>
    {
        bool Insert(string key, T value);
        T Value(string key);
    }

    //-------------------------------------------------------------------------

    class Trie<T> : ITrie<T>
    {
        private Node root;                 // Root node of the Trie
        private int size;                  // Number of values in the Trie

        class Node
        {
            public char ch;                // Character of the key
            public T value;                // Value at Node; otherwise default
            public Node low, middle, high; // Left, middle, and right subtrees

            // Node
            // Creates an empty Node
            // All children are set to null
            // Time complexity:  O(1)

            public Node(char ch)
            {
                this.ch = ch;
                value = default(T);
                low = middle = high = null;
            }
        }

        // Trie
        // Creates an empty Trie
        // Time complexity:  O(1)

        public Trie()
        {
            MakeEmpty();
            size = 0;
        }

        // Public Insert
        // Calls the private Insert which carries out the actual insertion
        // Returns true if successful; false otherwise

        public bool Insert(string key, T value)
        {
            return Insert(ref root, key, 0, value);
        }

        // Private Insert
        // Inserts the key/value pair into the Trie
        // Returns true if the insertion was successful; false otherwise
        // Note: Duplicate keys are ignored
        // Time complexity:  O(n+L) where n is the number of nodes and 
        //                                L is the length of the given key

        private bool Insert(ref Node p, string key, int i, T value)
        {
            if (p == null)
                p = new Node(key[i]);

            // Current character of key inserted in left subtree
            if (key[i] < p.ch)
                return Insert(ref p.low, key, i, value);

            // Current character of key inserted in right subtree
            else if (key[i] > p.ch)
                return Insert(ref p.high, key, i, value);

            else if (i + 1 == key.Length)
            // Key found
            {
                // But key/value pair already exists
                if (!p.value.Equals(default(T))) //this means there is already a value at that key, and that is NOT allowed.
                    return false;
                else
                {
                    // Place value in node
                    p.value = value;
                    size++;
                    return true;
                }
            }

            else
                // Next character of key inserted in middle subtree
                return Insert(ref p.middle, key, i + 1, value);
        }

        // Value
        // Returns the value associated with a key; otherwise default
        // Time complexity:  O(d) where d is the depth of the trie

        public T Value(string key)
        {
            int i = 0;
            Node p = root;

            while (p != null)
            {
                // Search for current character of the key in left subtree
                if (key[i] < p.ch)
                    p = p.low;

                // Search for current character of the key in right subtree           
                else if (key[i] > p.ch)
                    p = p.high;

                else // if (p.ch == key[i])
                {
                    // Return the value if all characters of the key have been visited 
                    if (++i == key.Length)
                        return p.value;

                    // Move to next character of the key in the middle subtree   
                    p = p.middle;
                }
            }
            return default(T);   // Key too long
        }

        // Contains
        // Returns true if the given key is found in the Trie; false otherwise
        // Time complexity:  O(d) where d is the depth of the trie

        public bool Contains(string key)
        {
            int i = 0;
            Node p = root;

            while (p != null)
            {
                // Search for current character of the key in left subtree
                if (key[i] < p.ch)
                    p = p.low;

                // Search for current character of the key in right subtree           
                else if (key[i] > p.ch)
                    p = p.high;

                else // if (p.ch == key[i])
                {
                    // Return true if the key is associated with a non-default value; false otherwise 
                    if (++i == key.Length)
                        return !p.value.Equals(default(T));

                    // Move to next character of the key in the middle subtree   
                    p = p.middle;
                }
            }
            return false;        // Key too long
        }

        // MakeEmpty
        // Creates an empty Trie
        // Time complexity:  O(1)

        public void MakeEmpty()
        {
            root = null;
        }

        // Empty
        // Returns true if the Trie is empty; false otherwise
        // Time complexity:  O(1)

        public bool Empty()
        {
            return root == null;
        }

        // Size
        // Returns the number of Trie values
        // Time complexity:  O(1)

        public int Size()
        {
            return size;
        }

        // Public Remove
        // Calls the private Remove which carries out the actual removal
        // Returns true if successful; false otherwise
        public bool Remove(string key) { 
            if (!Contains(key))
            {
                return false;
            } else
            {
                return Remove(ref root, key, 0);
            }
        }

        // Remove
        // Remove the given key (value) from the Trie
        // Returns true if the removal was successful; false otherwise
        private bool Remove(ref Node p, string key, int i)
        {
            // Current character of key removed in left subtree
            if (key[i] < p.ch)
                return Remove(ref p.low, key, i);

            // Current character of key removed in right subtree
            else if (key[i] > p.ch)
                return Remove(ref p.high, key, i);

            else if (i + 1 == key.Length)
            // Key found
            {
                // Remove value from node
                p.value = default(T);

                // If node is a leaf, remove it
                if (p.low == null && p.middle == null && p.high == null)
                {
                    p = null;
                    key = key.Remove(key.Length - 1, 1);
                    if (key != "") Remove(ref root, key, 0);
                    else size--; // Decrease size by 1
                } else
                {
                    size--; // Decrease size by 1
                }
        
                return true;
            }

            else
                // Next character of key removed from middle subtree
                return Remove(ref p.middle, key, i + 1);
        }

        // Public Print
        // Calls private Print to carry out the actual printing

        public void Print()
        {
            Print(root, "");
        }

        // Private Print
        // Outputs the key/value pairs ordered by keys
        // Time complexity:  O(n) where n is the number of nodes

        private void Print(Node p, string key)
        {
            if (p != null)
            {
                Print(p.low, key);
                if (!p.value.Equals(default(T)))
                    Console.WriteLine(key + p.ch + " " + p.value);
                Print(p.middle, key + p.ch);
                Print(p.high, key);
            }
        }

        // Public Print
        // Calls private Print to carry out the actual printing

        public void PrintTree()
        {
            if (root != null)
            {
                Console.WriteLine(root.ch + " " + root.value);
                Console.WriteLine();
                PrintTree(root);
            } 
        }

        // Private PrintTree
        // Outputs the key/value pairs in a tree format
        // Time complexity:  O(n) where n is the number of nodes

        private void PrintTree(Node p)
        {
            if (p != null)
            {
                if (p.low != null)
                    Console.Write(p.low.ch + " " + p.low.value + "|");
                else Console.Write("___|");
                PrintTree(p.low);
                if (p.middle != null)
                    Console.Write(p.middle.ch + " " + p.middle.value + "|");
                else Console.Write("___|");
                PrintTree(p.middle);
                if (p.high != null)
                    Console.Write(p.high.ch + " " + p.high.value + " ");
                else Console.Write("___ ");
                PrintTree(p.high);
                Console.WriteLine();
            }
        }
    }
}
