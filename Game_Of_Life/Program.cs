using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PDC_HW_3
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            int[,] start = make_board(800, 800);
            Stopwatch timer = new Stopwatch();

            //Runs Game of Life Using Thread Class
            timer.Reset();
            timer.Start();
            Run_Thread_Board(start, 8, 10, 1);
            timer.Stop();
            Console.WriteLine("No Unraveling: " + timer.ElapsedMilliseconds);

            //Unrolled twice
            timer.Reset();
            timer.Start();
            Run_Thread_Board(start, 8, 10, 2);
            timer.Stop();
            Console.WriteLine("Unrolled two times: " + timer.ElapsedMilliseconds);

            //Unrolled four times
            timer.Reset();
            timer.Start();
            Run_Thread_Board(start, 8, 10, 4);
            timer.Stop();
            Console.WriteLine("Unrolled four times: " + timer.ElapsedMilliseconds);

            void Run_Thread_Board(int[,] board, int threads, int iter, int roll)
            {
                for (int i = 0; i < iter; i++)
                {
                    board = eval_thread_board(board, threads, roll);
                    //PrintBoard(board);
                }
            }

            int[,] eval_thread_board(int[,] board, int threads, int roll)
            {
                //threads?
                int perThread = board.GetLength(0) - 2 / threads;
                int begin = 1;
                int count = begin + perThread;
                int[,] temp = board;

                for (int i = 0; i < threads; i++)
                {
                    Thread thread = new Thread(
                        () =>
                        {
                            if (roll == 4)
                            {
                                temp = eval_thread_row_4(board, begin, perThread);
                            }

                            else if (roll == 2)
                            {
                                temp = eval_thread_row_2(board, begin, perThread);
                            }

                            else
                            {
                                temp = eval_thread_row(board, begin, perThread);
                            }
                        });
                    thread.Start();
                    thread.Join();
                    board = temp;
                }

                return board;
            }

            //Task to hand to threads
            int[,] eval_thread_row(int[,] board, int begin1, int rows)
            {
                //prevents overflow of rows
                if (begin1 + rows > board.GetLength(0) - 1)
                {
                    rows = board.GetLength(0) - 1 - begin1;
                }

                int[,] temp = board;

                for (int i = begin1; i < begin1 + rows; i++)
                {
                    for (int j = 1; j < board.GetLength(1) - 1; j++)
                    {
                        temp[i, j] = life_eval(cell_eval(board, i, j), board[i, j]);
                    }
                }
                return temp;
            }

            //Unrolled twice
            int[,] eval_thread_row_2(int[,] board, int begin1, int rows)
            {
                //prevents overflow of rows
                if (begin1 + rows > board.GetLength(0) - 1)
                {
                    rows = board.GetLength(0) - 1 - begin1;
                }

                int[,] temp = board;

                for (int i = begin1; i < begin1 + rows; i+=2)
                {
                    for (int j = 1; j < board.GetLength(1) - 1; j+=2)
                    {
                        temp[i, j] = life_eval(cell_eval(board, i, j), board[i, j]);
                        temp[i+1, j+1] = life_eval(cell_eval(board, i+1, j+1), board[i+1, j+1]);
                    }
                }
                return temp;
            }

            //unrolled 4 times
            int[,] eval_thread_row_4(int[,] board, int begin1, int rows)
            {
                //prevents overflow of rows
                if (begin1 + rows > board.GetLength(0) - 1)
                {
                    rows = board.GetLength(0) - 1 - begin1;
                }

                int[,] temp = board;

                for (int i = begin1; i < begin1 + rows; i += 4)
                {
                    for (int j = 1; j < board.GetLength(1) - 1; j += 4)
                    {
                        temp[i, j] = life_eval(cell_eval(board, i, j), board[i, j]);
                        temp[i + 1, j + 1] = life_eval(cell_eval(board, i + 1, j + 1), board[i + 1, j + 1]);
                        temp[i + 2, j + 2] = life_eval(cell_eval(board, i + 2, j + 2), board[i + 2, j + 2]);
                        temp[i + 3, j + 3] = life_eval(cell_eval(board, i + 3, j + 3), board[i + 3, j + 3]);
                    }
                }
                return temp;
            }

            int[,] make_board(int x, int y)
            {
                int[,] new_board = new int[x + 2, y + 2];

                for (int i = 1; i < new_board.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < new_board.GetLength(1) - 1; j++)
                    {
                        if ((i + j) % 3 == 0)
                        {
                            new_board[i, j] = 1;
                        }

                        else
                        {
                            new_board[i, j] = 0;
                        }
                    }
                }
                return new_board;
            }

            int cell_eval(int[,] board, int i, int j)
            {
                int count = 0;
                for (int x = i - 1; x < i + 2; x++)
                {
                    for (int y = j - 1; y < j + 2; y++)
                    {
                        if (board[x, y] == 1)
                        {
                            count++;
                        }
                    }
                }

                if (board[i, j] == 1)
                {
                    count--;
                }
                return count;
            }

            int life_eval(int i, int alive)
            {

                if (alive == 1)
                {
                    //check loneliness/overcrowding
                    if (i == 2 || i == 3)
                    {
                        return 1;
                    }

                    else
                    {
                        return 0;
                    }
                }

                else
                {
                    if (i == 3)
                    {
                        return 1;
                    }

                    else
                    {
                        return 0;
                    }
                }
            }

            //prints the state of the board
            void PrintBoard(int[,] board)
            {
                for (int i = 1; i < board.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < board.GetLength(1) - 1; j++)
                    {
                        Console.Write(" " + board[i, j]);
                    }

                    Console.WriteLine(" ");

                }
                Console.WriteLine();
            }
        }
    }
}