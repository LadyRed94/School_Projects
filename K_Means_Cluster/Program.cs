using System;
using System.Threading.Tasks;

namespace KMeans
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter number of points: ");
            int nump = int.Parse(Console.ReadLine());
            Console.Write("Enter number of clusters: ");
            int k = int.Parse(Console.ReadLine());
            Console.WriteLine();


            Point[] points = gen_points(nump);
            Cluster[] clusters = gen_clusters(k);

            for (int i = 0; i < 20; i++)
            {
                points = point_sort(nump, points, k, clusters);
                clusters = compute_centroid(nump, points, k, clusters);
            }

            Console.WriteLine("Clusters: ");
            print_clusters(k, clusters);
            Console.WriteLine();
            Console.WriteLine("Points: ");
            print_pts(nump, points);
        }

        public static Cluster[] compute_centroid(int n, Point[] points, int k, Cluster[] cluster)
        {
            double sum_x = 0.0;
            double sum_y = 0.0;

            for (int i = 0; i < k; i++)
            {
                cluster[i].Count = 0;

                Parallel.For(0, n, j =>
                {
                    if (points[j].centroid_x == cluster[i].Centroid.x && points[j].centroid_y == cluster[i].Centroid.y)
                    {
                        sum_x += points[j].x;
                        sum_y += points[j].y;
                        cluster[i].Count += 1.0;
                    }
                });

                cluster[i].Centroid.x = sum_x / cluster[i].Count;
                cluster[i].Centroid.y = sum_y / cluster[i].Count;
                sum_x = 0.0;
                sum_y = 0.0;
            }

            return cluster;
        }

        public static Point[] gen_points(int length)
        {
            Point[] points = new Point[length];

            for (int i = 0; i < length; i++)
            {
                points[i] = new Point();
                points[i].x = i * 2 + 5;
                points[i].y = i * 5 + 2;
            }

            return points;
        }

        public static void print_pts(int length, Point[] points)
        {

            for (int i = 0; i < length; i++)
            {
                Console.Write(points[i].x + ", ");
                Console.WriteLine(points[i].y);
                Console.WriteLine($"Centroid = {points[i].centroid_x}, {points[i].centroid_y}");
                Console.WriteLine();
            }
        }

        public static void print_clusters(int k, Cluster[] clusters)
        {
            for (int i = 0; i < k; i++)
            {
                Console.WriteLine("Count = " + clusters[i].Count);
                Console.WriteLine("Centroid = " + clusters[i].Centroid.x + ", " + clusters[i].Centroid.y);
                Console.WriteLine();
            }
        }

        public static Point[] point_sort(int n, Point[] points, int k, Cluster[] clusters)
        {
            double length_dif;
            double width_dif;

            Parallel.For(0, n, j =>
            {
                points[j].distance = -1;

                for (int i = 0; i < k; i++)
                {

                    length_dif = points[j].x - clusters[i].Centroid.x;
                    width_dif = points[j].y - clusters[i].Centroid.y;
                    double total = (length_dif * length_dif) + (width_dif * width_dif);
                    total = Math.Sqrt(total);

                    if (points[j].distance == -1)
                    {
                        points[j].distance = total;
                        points[j].centroid_x = clusters[i].Centroid.x;
                        points[j].centroid_y = clusters[i].Centroid.y;
                    }

                    else if (total <= points[j].distance)
                    {
                        points[j].distance = total;
                        points[j].centroid_x = clusters[i].Centroid.x;
                        points[j].centroid_y = clusters[i].Centroid.y;
                    }
                }
            });

            return points;
        }

        public static Cluster[] gen_clusters(int k)
        {
            Cluster[] clusters = new Cluster[k];
            for (int i = 0; i < k; i++)
            {
                clusters[i] = new Cluster();
                clusters[i].Centroid.x = i * 10;
                clusters[i].Centroid.y = i * 10;
            }

            return clusters;
        }
    }

    public class Point
    {
        public double x;
        public double y;
        public double distance;
        public double centroid_x;
        public double centroid_y;

        public Point()
        {
            distance = -1;
        }
    }

    public class Cluster
    {
        public double Count;
        public Point Centroid;

        public Cluster()
        {
            Centroid = new Point();
        }
    }
}
