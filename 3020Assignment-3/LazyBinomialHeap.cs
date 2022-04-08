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
            rootListArr = new List<BinomialNode<T>>[1]; 
            rootListArr[0] = new List<BinomialNode<T>>();
            highest = null;
        }

        // Insert
        // Inserts an item into the binomial heap
        // Time complexity:  O(1)

        public void Insert(T item)

        {
            BinomialNode<T> insertNode = new BinomialNode<T>(item);
            
            if ((highest == null) || (highest.Item.CompareTo(item) < 0)) // Update highest if necessary
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

                size--;

                Coalesce(); // combine trees

                // Find new highest. Assume that after Coalesce that each index in rootListArr contains a list with at most one root

                if (!Empty())
                {
                    
                    BinomialNode<T> tempHighest = null;

                    for (int i = 0; i < rootListArr.Length; i++)
                    {
                        if ((rootListArr[i].Count > 0) &&
                            ((tempHighest == null) || (tempHighest.Item.CompareTo(rootListArr[i][0].Item) < 0)))
                            tempHighest = rootListArr[i][0];
                    }
                    highest = tempHighest;
                }
                else
                    highest = null;

            } // end if

        }// end Remove

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
            // First we might need to resize the rootListArr
            Resize();

            foreach (List<BinomialNode<T>> rootList in rootListArr)
            {
                BinomialNode<T> temp1 = null;
                BinomialNode<T> temp2 = null;

                while (rootList.Count > 1)
                {
                    temp1 = rootList[rootList.Count - 1]; // last tree in the list
                    temp2 = rootList[rootList.Count - 2]; // second-last tree

                    rootList.RemoveAt(rootList.Count - 1); // remove the last 2 trees from the root list as they will be combined
                    rootList.RemoveAt(rootList.Count - 1);

                    if (temp1.Item.CompareTo(temp2.Item) > 0) // temp1 has higher priority
                    {
                        BinomialLink(temp2, temp1);           // temp1 will be the root
                        rootListArr[temp1.Degree].Add(temp1); // add temp1 to the root list 1 degree higher
                    }
                    else                                      // temp2 has higher priority
                    {
                        BinomialLink(temp1, temp2);           // temp2 will be the root
                        rootListArr[temp2.Degree].Add(temp2); // add temp2 to the root list 1 degree higher
                    }
                        
                } // end while
            }
        }// end Coalesce

        // Print
        // Outputs the lazy binomial heap structure to the console

        public void Print()
        {
            Console.WriteLine(this.ToString());
        }

        // ToString
        // returns a string representing the lazy binomial heap structure
        public override string ToString()
        {
            string BHstring = "";

            if (!Empty())
            {
                BHstring += "##################################################\n";
                int degree = 0;
                foreach (List<BinomialNode<T>> rootList in rootListArr)
                {
                    BHstring += $"degree: {degree}\n\n";

                    foreach (BinomialNode<T> node in rootList)
                    {
                        Queue<BinomialNode<T>> nextNodes = new Queue<BinomialNode<T>>(); // represent the next level of the tree
                        Queue<BinomialNode<T>> currNodes = new Queue<BinomialNode<T>>(); // current level of tree
                        BinomialNode<T> current = node;
                        nextNodes.Enqueue(current);

                        while (nextNodes.Count > 0) // loop as long as there are more node to add to string
                        {
                            while (nextNodes.Count > 0) // transfer all to currNodes
                            {
                                currNodes.Enqueue(nextNodes.Dequeue());
                            }

                            while (currNodes.Count > 0)
                            {
                                current = currNodes.Dequeue();

                                while (current != null)
                                {
                                    BHstring += $" {{{current.Item.ToString()}}} ";

                                    if (current.LeftMostChild != null) // add all Left Children to the next nodes queue
                                        nextNodes.Enqueue(current.LeftMostChild);

                                    current = current.RightSibling;
                                }
                            }

                            BHstring += "\n";  // all nodes of the current level have been added to string
                        }

                        BHstring += "\n";

                    }// end foreach (2)

                    if(degree != rootListArr.Length - 1)
                        BHstring += "==============================\n";
                    degree++;

                }//end foreach (1)

                BHstring += "##################################################\n";

            }// end if

            return BHstring;
        }// end ToString



        // Resize
        // Creates a new array with adjusted size for rootListArr
        private void Resize()
        {
            if (MaxDegrees() > rootListArr.Length) // only resize if it is making the rootListArr bigger
            {
                List<BinomialNode<T>>[] newRootListArr = new List<BinomialNode<T>>[MaxDegrees()];

                for(int i = 0; i < MaxDegrees(); i++)
                {
                    newRootListArr[i] = new List<BinomialNode<T>>();
                }

                for(int i = 0; i < rootListArr.Length; i++) // move everything into the new array
                {
                    newRootListArr[i] = rootListArr[i];
                }

                rootListArr = newRootListArr;
            }
        }

        // MaxDegrees
        // Determines the maximum size needed for rootListArr
        public int MaxDegrees()
        {
            return (int)Math.Ceiling(Math.Log2(size + 1));
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
            // Essentially make a new LazyBinomialHeap
            size = 0;
            rootListArr = new List<BinomialNode<T>>[1];
            rootListArr[0] = new List<BinomialNode<T>>();
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
