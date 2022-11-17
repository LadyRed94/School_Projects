using System;
using System.Threading;
using System.Threading.Tasks;

namespace Merge_Sort
{
    class MainClass
    {
        static void Main(string[] args)
        {
            int[] nums = { 10, 24, 13, 15, 87, 64, 92, 5, 1004 };

            Console.WriteLine("Original Array:");

            foreach(int i in nums)
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine();

            Merge_Sort(nums);
            Console.WriteLine();

            Console.WriteLine("Sorted Array:");

            foreach (int i in nums)
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine();
        }

        //Just a function to call the recursive method
        static public void Merge_Sort(int[] nums)
        {
            //Pre-Sorts Threads using Parallelism
            int[] lefts = Partition(nums);
            Thread[] threads = new Thread[lefts.Length];
            if (threads.Length != 0)
            {
                for (int i = 0; i < threads.Length; i++)
                {
                    int calcRight = i * (nums.Length / lefts.Length) - 1;
                    int calcLeft = lefts[i];
                    threads[i] = new Thread(j =>
                    {
                        SortMethod(nums, calcLeft, calcRight);
                    });
                    threads[i].Start();
                    threads[i].Join();
                }
            }

            //Cleans up sort
            SortMethod(nums, 0, nums.Length - 1);
        }

        //Will partition for most numbers
        static public int[] Partition(int[]nums)
        {
            if (nums.Length % 4 == 0)
            {
                int[] lefts = new int[4];
                int dif = nums.Length / 4;
                for (int i = 1; i < 4; i++)
                {
                    lefts[i] = dif * 4;
                }

                return lefts;
            }

            else if (nums.Length % 2 == 0)
            {
                int[] lefts = new int[2];
                int dif = nums.Length / 2;
                for (int i = 1; i < 2; i++)
                {
                    lefts[i] = dif * 2;
                }

                return lefts;
            }

            else if (nums.Length % 3 == 0)
            {
                int[] lefts = new int[3];
                int dif = nums.Length / 3;
                for (int i = 1; i < 3; i++)
                {
                    lefts[i] = dif * 3;
                }

                return lefts;
            }

            else
            {
                int[] threads = new int[0];
                return threads;
            }
        }

        //Recursive method
        static public void SortMethod(int[] nums, int left, int right)
        {
            int mid;

            if (right > left)
            {
                //Sets mid
                mid = (right + left) / 2;

                //Sort Left Side
                SortMethod(nums, left, mid);

                //Sort Right Side
                SortMethod(nums, (mid + 1), right);

                //Merge Sides
                Merge(nums, left, (mid + 1), right);
            }
        }

        //Merge function
        static public void Merge(int[] nums, int left, int mid, int right)
        {
            //creats holder array
            int[] holder = new int[nums.Length];

            //initialize and set variables
            int i, end, elements, pos;

            //sets end for left array
            end = (mid - 1);

            //position tracker
            pos = left;

            //number of total elements
            elements = (right - left + 1);

            //sorts array till end of one array
            while ((left <= end) && (mid <= right))
            {
                if (nums[left] <= nums[mid])
                {
                    holder[pos++] = nums[left++];
                }

                else
                {
                    holder[pos++] = nums[mid++];
                }
            }

            //puts in leftover data from left
            while (left <= end)
            {
                holder[pos++] = nums[left++];
            }

            //puts in leftover data from right
            while (mid <= right)
            {
                holder[pos++] = nums[mid++];
            }

            //reads sorted data into original array
            for (i = 0; i < elements; i++)
            {
                nums[right] = holder[right];
                right--;
            }
        }
    }
}
