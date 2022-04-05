// Team: Greg Prouty, Bradley Primeau, Avery Chin, Megan Risebrough

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3020Assignment_3
{
    //--------------------------------------------------------------------------------------

    //Note: IContainer defintion in TernaryTree.cs
    public interface ILazyBinomialHeap<T> : IContainer<T> where T : IComparable
    {
        void Insert(T item);  // Insert an item to a binomial heap
        void Remove();        // Remove the item with the highest priority
        T Front();            // Return the item with the highest priority
        void Coalesce();      // Combines trees in heap until there are only one of each degree
    }

    
    //--------------------------------------------------------------------------------------

    // Used by class LazyBinomialHeap<T>
    // Implements IComparable and overrides ToString (from Object)

    public class PriorityClass : IComparable
    {
        private int priorityValue;
        private char letter;

        public PriorityClass(int priority, char letter)
        {
            this.letter = letter;
            priorityValue = priority;
        }

        public int CompareTo(Object obj)
        {
            PriorityClass other = (PriorityClass)obj;   // Explicit cast
            return priorityValue - other.priorityValue;  // High values have higher priority
        }

        public override string ToString()
        {
            return letter.ToString() + " with priority " + priorityValue;
        }
    }

    //--------------------------------------------------------------------------------------

    public class BinomialNode<T>
    {
        public T Item { get; set; }
        public int Degree { get; set; }
        public BinomialNode<T> LeftMostChild { get; set; }
        public BinomialNode<T> RightSibling { get; set; }

        // Constructor

        public BinomialNode(T item)
        {
            Item = item;
            Degree = 0;
            LeftMostChild = null;
            RightSibling = null;

        }
    }

    //--------------------------------------------------------------------------------------

    public class LazyBinomialHeap<T> : ILazyBinomialHeap<T> where T : IComparable
    {
        private List<BinomialNode<T>>[] rootListArr; // Array of size Ceiling(log2(n+1)) containing Lists of binomial trees where degree == index
        private BinomialNode<T> highest;             // Node containing the highest priority
        private int size;                            // Size of the binomial heap


        // Contructor
        // Time complexity:  O(1)

        public LazyBinomialHeap()
        {
            size = 0;
            rootListArr = new List<BinomialNode<T>>[this.MaxDegrees()]; 
            highest = null;
        }

        // MaxDegrees
        // Determines the maximum size needed for rootListArr
        private int MaxDegrees()
        {
            return (int)Math.Ceiling(Math.Log2(size + 1));
        }

        // Insert
        // Inserts an item into the binomial heap
        // Time complexity:  O(1)

        public void Insert(T item)

        {
            BinomialNode<T> insertNode = new BinomialNode<T>(item);
            
            if (highest.Item.CompareTo(item) < 0) // Update highest if necessary
                highest = insertNode;

            rootListArr[0].Add(insertNode); // Insert new binomial node to the list containing degree 0
            size++;
        }

        // Remove
        // Removes the item with the highest priority from the binomial heap
        // Time complexity:  O(log n)

        public void Remove()
        {
            if (!Empty())
            {
                rootListArr[highest.Degree].Remove(highest); // Remove highest from the respective root list

                // Now we need to add any children back to the heap
                BinomialNode<T> current = highest.LeftMostChild;
                highest.LeftMostChild = null; // sever child connection

                while (current != null)
                {
                    BinomialNode<T> next = current.RightSibling; // store the right sibling (if any)

                    current.RightSibling = null; // Sever connection to right sibling
                    rootListArr[current.Degree].Add(current); // Add tree to the proper degree index
                    current = next;

                }
            }
            size--;

            Coalesce();

            // Find new highest. Assume that after Coalesce that each index in rootListArr contains a list with at most one root

            if (!Empty())
            {
                BinomialNode<T> tempHighest = rootListArr[0][0]; // initialize with the first node in the heap

                for (int i = 1; i < rootListArr.Length; i++)
                {
                    if (tempHighest.Item.CompareTo(rootListArr[i][0].Item) < 0)
                        tempHighest = rootListArr[i][0];
                }
                highest = tempHighest;
            }
            else
                highest = null;
        }

        // Front
        // Returns the item with the highest priority
        // Time complexity:  O(1)

        public T Front()
        {

            if (!Empty())
                return highest.Item;
            else
                return default(T);
        }


        // Coalesce
        // Repeatedly combines the binomial trees together until there
        //    is at most one tree of each degree
        // Time complexity:  O(log n)
        public void Coalesce()
        {

        }

        // BinomialLink
        // Makes child the leftmost child of root
        // Time complexity:  O(1)

        private void BinomialLink(BinomialNode<T> child, BinomialNode<T> root)
        {
            child.RightSibling = root.LeftMostChild;
            root.LeftMostChild = child;
            root.Degree++;
        }


        // MakeEmpty
        // Creates an empty binomial heap
        // Time complexity:  O(1)

        public void MakeEmpty()
        {
            size = 0;
            rootListArr = new List<BinomialNode<T>>[this.MaxDegrees()];
            highest = null;
        }

        // Empty
        // Returns true is the binomial heap is empty; false otherwise
        // Time complexity:  O(1)

        public bool Empty()
        {
            return size == 0;
        }

        // Size
        // Returns the number of items in the binomial heap
        // Time complexity:  O(1)

        public int Size()
        {
            return size;
        }      
    }
}
