using System;

namespace Circular_Linked_List
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            CircularList Test = new CircularList();
            Test.Insert(5);
            Test.Insert(7);
            Test.Insert(10);
            Test.Insert(555);
            Test.Insert(20);
            Test.Insert(2);

            Test.Print();

            Test.DeleteFront();

            Test.Print();
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
            Next = null;
        }
    }

    class CircularList
    {
        private Node head;

        public bool IsEmpty() //running time: O(1)
        {
            //return head == null;
            if (head == null)
                return true;
            else
                return false;
        }


        public void Print()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Is Empty");
                return;
            }

            else
            {
                Node pointer = head;
                while (pointer.Next != head)
                {
                    Console.Write(pointer.Value + " ");
                    pointer = pointer.Next;
                }
                Console.Write(pointer.Value + " ");
                pointer = pointer.Next;
                Console.WriteLine(pointer.Value);
            }
        }

        public void Insert(int someValue)
        {
            Node toAdd = new Node(someValue);

            if (IsEmpty())
            {
                head = toAdd;
                toAdd.Next = head;
            }

            else
            {
                Node pointer = head;

                while (pointer.Next != head)
                {
                    pointer = pointer.Next;
                }

                toAdd.Next = head;
                pointer.Next = toAdd;
                head = toAdd;
            }
        }

        public void DeleteFront()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Is Empty");
                return;
            }

            else
            {
                Node pointer = head;
                while(pointer.Next != head)
                {
                    pointer = pointer.Next;
                }

                pointer.Next = head.Next;
                head = head.Next;
            }
        }

        //display() :
        //Begin
        //    if head is null, then
        //        Nothing to print and return
        //    else
        //        ptr := head
        //        while next of ptr is not head, do
        //            display data of ptr
        //            ptr := next of ptr
        //        display data of ptr
        //end if
        //End
    }
}
