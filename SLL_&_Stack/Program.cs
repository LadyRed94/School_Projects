using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Group 7: Alexandra Feely, Roshaun Stevens, Thuan Pham
namespace week03_SLL
{
    class Program
    {
        static void Main(string[] args)
        {
            Node someNode = new Node(7);
            Stack myList = new Stack();

            myList.Push(5);
            myList.Push(4);
            myList.Push(2);
            myList.Push(3);
            myList.Push(1);
            myList.Push(6);
            myList.Print();
            Console.WriteLine();
            myList.Pop();
            myList.Pop();
            myList.Pop();
            myList.Print();
            Console.WriteLine();
            Console.WriteLine(myList.Min());
        }
    }

    class Node
    {
        //data
        public int Value { get; set; }
        public Node Next { get; set; }

        //operations

        //ctor
        public Node(int someValue)
        {
            Value = someValue;
            //Next = null;
        }
    }

    class SinglyLinkedList2
    {
        //data
        public Node head { get; set; }

        //operations
        public void RemoveDuplicate()
        {
            if (IsEmpty() || head.Next == null) //sanity check
                return;


            //Dictionary - keep track of unique values
            Dictionary<int, string> hashTable = new Dictionary<int, string>();
            hashTable.Add(head.Value, "value");

            //traverse the list and search for duplcates - if duplicate are found, we link those nodes out
            Node finger = head;
            while (finger.Next != null)
            {
                //check if the value at this (Next) node is a duplicate
                if (hashTable.ContainsKey(finger.Next.Value))
                {
                    //you found duplicate ... remove it
                    finger.Next = finger.Next.Next;
                }
                else
                {
                    //you found a new value, not a duplicate
                    hashTable.Add(finger.Next.Value, "value");
                    finger = finger.Next; //moves the finger to the right
                }
            }
        }

        public bool IsEmpty() //running time: O(1)
        {
            //return head == null;
            if (head == null)
                return true;
            else
                return false;
        }
        public void AddFront(int someValue) //running time O(1)
        {
            Node newNode = new Node(someValue); //make a new node
            newNode.Next = head; //newnode should point to head
            head = newNode; //change the head to point to the new node
        }
        public void AddBack(int someValue) //running time: O(n)
        {
            //make a new node
            Node newNode = new Node(someValue);

            //check if empty
            if (IsEmpty())
            {
                head = newNode; //the left side of = is changing
            }
            else
            {
                Node finger = head;
                while (finger.Next != null) //move the finger to the last node
                {
                    finger = finger.Next; //move the finger to the right
                }

                //finger now points to the last not null node
                finger.Next = newNode;
            }
        }
        public void Append(int someValue)
        {
            AddBack(someValue);
        }

        public void Print()
        {
            if (IsEmpty())
                Console.WriteLine("the list is empty!");
            else
            {
                Node finger = head; //starts at the head
                while (finger != null)//as long
                {
                    Console.Write(finger.Value + " ");
                    finger = finger.Next; //moves the finger to the right
                }

                Console.WriteLine();
                Console.WriteLine();
            }

        }

        public void DeleteFront()
        {
            if (IsEmpty())
                return; //ignore requests when list is empty
            else
                head = head.Next; //move head to the right
        }

        public void DeleteBack()
        {
            if (IsEmpty()) //head is null
            {
                Console.WriteLine("list already empty ... cannot delete back");
            }
            else if (head.Next == null) // list has only one element
            {
                head = null;
            }
            else
            {
                Node finger = head;
                while (finger.Next.Next != null)
                {
                    finger = finger.Next;//move finger to the right
                }
                //now finger points to next to last
                finger.Next = null;
            }
        }

        public void Insert(int someValue) //assumes the linked list is sorted
        {
            //if(IsEmpty())
            //{
            //    AddFront(someValue);
            //}
            //else if(someValue<=head.Value)
            //{
            //    AddFront(someValue);
            //}
            if (IsEmpty() || (someValue <= head.Value))
            {
                AddFront(someValue);
            }
            else
            {
                Node newNode = new Node(someValue); //1

                //2 search for the last node that has a value < someValue
                Node finger = head;
                while (finger.Next != null && finger.Next.Value < someValue)
                {
                    finger = finger.Next; //move finger to the next node
                }

                //3 - link in the newNode
                newNode.Next = finger.Next;
                finger.Next = newNode;
            }
        }
        public void Delete(int someValue) //delete node containing someValue
        {
            if (IsEmpty())
            {
                Console.WriteLine($"{someValue} not found in the list");
            }
            else if (head.Value == someValue)
            {
                head = head.Next;
            }
            else
            {
                Node finger = head;
                while ((finger.Next != null) && (finger.Next.Value != someValue)) //use  while( (finger.Next!= null) && (finger.Next.Value < someValue))  for sorted lists
                {
                    finger = finger.Next;
                }

                if (finger.Next != null) //if node found ... then link out the node
                {
                    finger.Next = finger.Next.Next;
                }
                else
                {
                    Console.WriteLine($"{someValue} not found in the list");
                }
            }
        }

        public void Clear()
        {
            head = null; //let the garbace collector remove the nodes
        }

        public void Reverse1()
        {
            Node finger = head;
            head = null;

            while (finger != null)
            {
                AddFront(finger.Value);
                finger = finger.Next;
            }
        }

        public void Reverse2()
        {
            if (IsEmpty() || head.Next == null) //no work to be done if the list is empty or list has only one node in it
                return;

            Node prevNode = null;
            Node currNode = head;
            Node nextNode = currNode;

            while (currNode != null)
            {
                nextNode = nextNode.Next;
                currNode.Next = prevNode;
                prevNode = currNode;
                currNode = nextNode;
            }
            head = prevNode;
        }

        public bool ContainsACycle()
        {
            Node slow = head;
            Node fast = head;

            while (fast != null && fast.Next != null)
            {
                slow = slow.Next; //slow moves one step
                fast = fast.Next.Next; //fast moves two steps

                if (slow == fast)
                    return true; //found cycle
            }

            return false; //got to the end of the list
        }
    }

    class Stack
    {
        //data
        public Node head { get; set; }
        public SinglyLinkedList2 minimum1 = new SinglyLinkedList2();

        //operations
        public void RemoveDuplicate()
        {
            if (IsEmpty() || head.Next == null) //sanity check
                return;


            //Dictionary - keep track of unique values
            Dictionary<int, string> hashTable = new Dictionary<int, string>();
            hashTable.Add(head.Value, "value");

            //traverse the list and search for duplcates - if duplicate are found, we link those nodes out
            Node finger = head;
            while (finger.Next != null)
            {
                //check if the value at this (Next) node is a duplicate
                if (hashTable.ContainsKey(finger.Next.Value))
                {
                    //you found duplicate ... remove it
                    finger.Next = finger.Next.Next;
                }
                else
                {
                    //you found a new value, not a duplicate
                    hashTable.Add(finger.Next.Value, "value");
                    finger = finger.Next; //moves the finger to the right
                }
            }
        }

        //Group 7: Alexandra Feely, Roshaun Stevens, Thuan Pham
        //Code for Lab 5 start
        public void Push(int someValue)
        {
            AddFront(someValue);

            if (minimum1.head == null)
            {
                minimum1.AddFront(someValue);
            }

            if (someValue <= minimum1.head.Value)
            {
                minimum1.AddFront(someValue);
            }
        }

        public Node Pop()
        {
            Node top = head; //make a new node
            //top = head;
            DeleteFront();

            if (top.Value == minimum1.head.Value)
            {
                minimum1.DeleteFront();
            }
            return top;
        }

        public int Top()
        {
            return head.Value;
        }

        public int Min()
        {
            return minimum1.head.Value;
        }
        public bool IsEmpty() //running time: O(1)
        {
            //return head == null;
            if (head == null)
                return true;
            else
                return false;
        }
        public void AddFront(int someValue) //running time O(1)
        {
            Node newNode = new Node(someValue); //make a new node
            newNode.Next = head; //newnode should point to head
            head = newNode; //change the head to point to the new node
        }
        public void AddBack(int someValue) //running time: O(n)
        {
            //make a new node
            Node newNode = new Node(someValue);

            //check if empty
            if (IsEmpty())
            {
                head = newNode; //the left side of = is changing
            }
            else
            {
                Node finger = head;
                while (finger.Next != null) //move the finger to the last node
                {
                    finger = finger.Next; //move the finger to the right
                }

                //finger now points to the last not null node
                finger.Next = newNode;
            }
        }
        public void Append(int someValue)
        {
            AddBack(someValue);
        }

        public void Print()
        {
            if (IsEmpty())
                Console.WriteLine("the list is empty!");
            else
            {
                Node finger = head; //starts at the head
                while (finger != null)//as long
                {
                    Console.Write(finger.Value + " ");
                    finger = finger.Next; //moves the finger to the right
                }

                Console.WriteLine();
                Console.WriteLine();
            }

        }

        public void DeleteFront()
        {
            if (IsEmpty())
                return; //ignore requests when list is empty
            else
                head = head.Next; //move head to the right
        }

        public void DeleteBack()
        {
            if (IsEmpty()) //head is null
            {
                Console.WriteLine("list already empty ... cannot delete back");
            }
            else if (head.Next == null) // list has only one element
            {
                head = null;
            }
            else
            {
                Node finger = head;
                while (finger.Next.Next != null)
                {
                    finger = finger.Next;//move finger to the right
                }
                //now finger points to next to last
                finger.Next = null;
            }
        }

        public void Insert(int someValue) //assumes the linked list is sorted
        {
            //if(IsEmpty())
            //{
            //    AddFront(someValue);
            //}
            //else if(someValue<=head.Value)
            //{
            //    AddFront(someValue);
            //}
            if (IsEmpty() || (someValue <= head.Value))
            {
                AddFront(someValue);
            }
            else
            {
                Node newNode = new Node(someValue); //1

                //2 search for the last node that has a value < someValue
                Node finger = head;
                while (finger.Next != null && finger.Next.Value < someValue)
                {
                    finger = finger.Next; //move finger to the next node
                }

                //3 - link in the newNode
                newNode.Next = finger.Next;
                finger.Next = newNode;
            }
        }
        public void Delete(int someValue) //delete node containing someValue
        {
            if (IsEmpty())
            {
                Console.WriteLine($"{someValue} not found in the list");
            }
            else if (head.Value == someValue)
            {
                head = head.Next;
            }
            else
            {
                Node finger = head;
                while ((finger.Next != null) && (finger.Next.Value != someValue)) //use  while( (finger.Next!= null) && (finger.Next.Value < someValue))  for sorted lists
                {
                    finger = finger.Next;
                }

                if (finger.Next != null) //if node found ... then link out the node
                {
                    finger.Next = finger.Next.Next;
                }
                else
                {
                    Console.WriteLine($"{someValue} not found in the list");
                }
            }
        }

        public void Clear()
        {
            head = null; //let the garbace collector remove the nodes
        }

        public void Reverse1()
        {
            Node finger = head;
            head = null;

            while (finger != null)
            {
                AddFront(finger.Value);
                finger = finger.Next;
            }
        }

        public void Reverse2()
        {
            if (IsEmpty() || head.Next == null) //no work to be done if the list is empty or list has only one node in it
                return;

            Node prevNode = null;
            Node currNode = head;
            Node nextNode = currNode;

            while (currNode != null)
            {
                nextNode = nextNode.Next;
                currNode.Next = prevNode;
                prevNode = currNode;
                currNode = nextNode;
            }
            head = prevNode;
        }

        public bool ContainsACycle()
        {
            Node slow = head;
            Node fast = head;

            while (fast != null && fast.Next != null)
            {
                slow = slow.Next; //slow moves one step
                fast = fast.Next.Next; //fast moves two steps

                if (slow == fast)
                    return true; //found cycle
            }

            return false; //got to the end of the list
        }
    }
}  