using System;

//Group: Patsy Albrecht, Krystine Thai, Alexandra Feely
namespace DSA_Lab_Week_8
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            //Problem 1
            int[] a = new int[10]; //{ 1, 3, 5, 7, 9 };
            int[] b = new int[5] { 2, 4, 6, 8, 10 };
            int count = 1;
            for (int i = 0; i < 5; i++)
            {
                a[i] = count;
                count += 3;
            }

            Print(a);
            Print(b);
            Print(MergeAB(a, b));

            //Problem 2
            string[] sparse = new string[] { "at", "", "", "", "ball", "", "", "car", "", "", "dad", "", "" };
            Console.WriteLine(SparseSearch(sparse, "at"));
            Console.WriteLine(SparseSearch(sparse, "ball"));
            Console.WriteLine(SparseSearch(sparse, "car"));
            Console.WriteLine(SparseSearch(sparse, "dad"));
        }

        //Problem 2
        static int SparseSearch(string[] sparse, string found)
        {
            string c = "";

            int end = sparse.Length - 1;
            int mid = end / 2;

            while(!c.Equals(found))
            {
                //Checks if mid is empty
                if (sparse[mid].Equals(""))
                {
                    //Console.WriteLine(mid);
                    int less = 1;
                    int more = 1;

                    while (sparse[mid - less].CompareTo("") == 0 && sparse[mid + more].CompareTo("") == 0)
                    {
                        //Console.WriteLine(more);
                        less = less + 1;
                        more = more + 1;
                    }

                    if (sparse[mid - less].CompareTo("") != 0)
                    {
                        mid = mid - less;
                    }

                    else
                    {
                        mid = mid + more;
                    }
                }

                if (found.CompareTo(sparse[mid]) < 0)
                {
                    mid = mid / 2;
                }

                else if (found.CompareTo(sparse[mid]) > 0)
                {
                    mid = mid + mid / 2;
                }

                else
                {
                    c = sparse[mid];
                }
            }

            return mid;
        }

        //Problem 1 Answer
        static int[] MergeAB (int[] a, int[] b)
        {
            int[] c = new int[a.Length];

            int ai = 0;
            int bi = 0;
            int i = 0;

            while (i < c.Length)
            {
                //Console.WriteLine(bi);
                if (bi == b.Length)
                {
                    c[i] = a[ai];
                    ai += 1;
                    i += 1;
                }

                else if (a[ai] <= b[bi] && a[ai] != 0)
                {
                    c[i] = a[ai];
                    ai += 1;
                    i += 1;
                }

                else
                {
                    c[i] = b[bi];
                    bi += 1;
                    i += 1;
                }
            }

            return c;
        }

        //Prints Array
        static void Print(int[] toPrint)
        {
            foreach(int item in toPrint)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }
        
    }
}