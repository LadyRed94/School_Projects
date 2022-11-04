using System;
using System.Linq;
using System.Collections.Generic;

//Group: Alexandra Feely, Patsy Albrecht, Alexander Moran
namespace DSA_HW9
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph myGraph = new Graph(false);

            foreach ((int Start, int End) edge in new (int, int)[] { (1, 5), (1, 4), (1, 2), (2, 3), (3, 5), (3, 6), (4, 5), (5, 6) })
            {
                myGraph.AddEdge(edge.Start, edge.End);
            }

            foreach (int vertex in myGraph.Dijkstra(4, 6))
            {
                Console.Write($"{vertex} ");
            }

            Console.WriteLine();

            //foreach (int vertex in myGraph.AlexandraDijkstra(4, 6))
            //{
            //    Console.Write($"{vertex} ");
            //}
        }
    }

    public class Vertex<T> //just a node
    {
        public T Label { get; set; }
        public int distance;

        public Vertex(T newLabel)
        {
            Label = newLabel;
            distance = 0;
        }
    }

    class Graph
    {
        //DATA
        //G = (V, E) graph is a pair, V - set of vertices, E - set of edges, Adjacency List
        List<Vertex<int>> vertices = new List<Vertex<int>>();
        List<List<int>> adjacencyList = new List<List<int>>();
        public bool isDirectedGraph { get; private set; }


        List<Vertex<int>> Lost = new List<Vertex<int>>();
        Dictionary<int, int> track = new Dictionary<int, int>();


        //I didn't really undertand anything about the Dijkstra algorithm when attempting this problem
        //however Alexander Moran did and went through his code on the problem and explained it to me
        //and Patsy. It really helped! I'm leaving his correct code in as it is what our group did and went over but
        //I did a quick attempt at how to do it with recursion just help get my mind around the algorithm.
        //It helped a lot in my understanding but I'm leaving it unfinished for now.

        //This is the attempted recursive version that I've been working on that has some bugs
        //Had to add some global variables due to not knowing pointers in C#
        //Seth L recommended in lab to at ref keyword to help
        public List<int> AlexandraDijkstra(int start, int end)
        {
            List<Vertex<int>> BeenThere = new List<Vertex<int>>();
            Vertex<int> ToStart = vertices[FindIndex(start)];
            Vertex<int> ToEnd = vertices[FindIndex(end)];
            List<int> Path = new List<int>();

            //Creates list of potential vertices
            foreach (Vertex<int> V in vertices)
            {
                Lost.Add(V);
            }

            //awful paramater setup, could definitly clean up
            RecursiveDijkstra(ToStart, ToEnd, BeenThere);

            Path.Add(end);

            //Attempts to find shortest path from dictionary
            //Something is breaking here
            while (true)
            {
                if (Path.Last() == start)
                {
                    break;
                }

                else if(adjacencyList[FindIndex(Path.Last())].Contains(track[Path.Last()]))
                {
                    Path.Add(track[Path.Last()]);
                }

                else
                {
                    track.Remove(track[Path.Last()]);
                }

            }

            return Path;
        }

        //Attempted Recusion a few different ways as there seems to be several potential ways to implement
        //currently unfinished
        public void RecursiveDijkstra(Vertex<int> start, Vertex<int> end, List<Vertex<int>> Been,int dist = 0 )
        {
            Been.Add(start); //mostly used for testing
            Lost.Remove(start); //removes vertex from possible vertices
            Vertex<int> next; //where to go next

            //start.distance = dist; //updates distance

            if (start.Label == end.Label)
            {
                //track.Add(end.Label, start.Label);
                return;
            }

            //Sets distances for attached vertices
            foreach (int vert in adjacencyList[FindIndex(start.Label)])
            {
                foreach (Vertex<int> V in Lost)
                {
                    if (V.Label == vert && V.distance > dist+1)
                    {
                        V.distance += dist + 1;
                    }
                }
            }

            //Finds vertex with the lowest distance
            if (Lost.Count != 0)
            {
                //sets a default next node
                next = Lost[0];

                foreach (int vert in adjacencyList[FindIndex(start.Label)])
                {
                        foreach(Vertex<int> V in Lost)
                    {
                        if (V.Label == vert && V.distance < next.distance)
                            next = V;
                    }
                }

                //adds vertices to dictionary to trace path taken
                track.Add(next.Label, start.Label);

                //Function calls itself on next vertex
                RecursiveDijkstra(next, end, Been, dist += 1);
            }
            return;
        }

        //METHODS - action
        //This is an implementation of Dijkstra's shortest path algorithm. Since the Graph
        //class from class is an unweighted graph, this will essentially find the path with
        //the fewest number of edges. Function assumes both start and end exist in graph.
        //This implementation is O(N^3), since the while loop is O(N) and contains an O(N) loop
        //with a third O(N) loop inside. On average, it will be faster than that, but not in a way
        //that matters for time complexity.
        public List<int> Dijkstra(int start, int end)
        {
            int[] included = new int[vertices.Count];                       //We hold onto everywhere we've been here.
            List<int[]> notIncluded = new List<int[]>();                    //We hold onto everywhere we haven't been here.
            Dictionary<int, int> predecessors = new Dictionary<int, int>(); //We'll hold onto each vertex's predecessor here.

            int[] next = new int[] { -1, -1, -1 }; // [vertex label, distance, predecessor], -1 for dummy values
            for (int i = 0; i < vertices.Count; i++)            //This loop will load the 'notIncluded list with 
            {                                                   //every vertex in the graph. The start node is
                if (vertices[i].Label == start)                 //loaded in with 0 distance.
                {
                    notIncluded.Add(new int[] { start, 0, -1 });
                }
                else
                {
                    notIncluded.Add(new int[] { vertices[i].Label, -1, -1 });
                }
            }

            while (notIncluded.Count > 0)//O(n) //We start with a number of iterations equal
            {                                   //to the number of vertices in the graph.
                next = new int[] { -1, -1, -1 };    //reset next to dummy values
                foreach (int[] vertex in notIncluded.Where(o => o[1] >= 0))  //This generates a list of 
                {                                                           //all the vertices that don't 
                    if (next[1] == -1 || vertex[1] < next[1])               //currently have a dummy value as distance
                    {                                                       //and finds the one with the lowest distance
                        next = vertex;
                    }
                }
                predecessors.Add(next[0], next[2]);         //Add the label and predecessor to the dictionary of predecessors.
                notIncluded.Remove(next);                   //Removes it from the list
                if (next[0] == end)                         //If we found the end, we've found our path.
                {
                    break;
                }

                foreach (int vertex in adjacencyList[FindIndex(next[0])])//O(n)              //Here, we go through every vertex that
                {                                                                           //is adjacent to the one we're looking at
                    for (int i = 0; i < notIncluded.Count; i++)//O(n)                       //and see if it's in the list. If it is,
                    {                                                                       //and we either have a dummy value or larger
                        if (notIncluded[i][0] == vertex &&                                   //value for distance, we update the distance
                            (notIncluded[i][1] == -1 || notIncluded[i][1] >= next[1] + 1))  //and the predecessor
                        {
                            notIncluded[i][1] = next[1] + 1;
                            notIncluded[i][2] = next[0];
                        }
                    }
                }
            }

            List<int> path = new List<int>();   //This list keeps track of the path being taken
            path.Add(next[0]);                  //We add the one that broke out of the loop above,
            while (true)                        //and then add each node's predecessor to the list
            {                                   //until we find the start.
                if (path.Last() == start)
                {
                    break;
                }
                path.Add(predecessors[path.Last()]);
            }

            return path;

        }

        //add a new vertex
        public void AddVertex(int label)
        {
            vertices.Add(new Vertex<int>(label));
            adjacencyList.Add(new List<int>());
        }

        //add a new edge, options: weight=1, directed/undirected graph
        public void AddEdge(int originLabel, int destLabel)
        {
            AddEdge(originLabel, destLabel, isDirectedGraph);
        }

        private void AddEdge(int originLabel, int destLabel, bool isDirected)
        {
            //find origin in vertices
            int i = FindIndex(originLabel);

            if (i == -1)
            //throw new Exception($"{originLabel} vertex does not exist yet");
            {
                AddVertex(originLabel);
                i = vertices.Count - 1;
            }

            adjacencyList[i].Add(destLabel);

            int j = FindIndex(destLabel);
            if (j == -1)
                AddVertex(destLabel);

            if (isDirected == false)
            {
                AddEdge(destLabel, originLabel, true);
            }
        }


        //find index of a vertex in a list of vertices given a label
        public int FindIndex(int label)
        {
            for (int i = 0; i < vertices.Count; i++)
                if (vertices[i].Label == label)
                {
                    return i;
                }
            return -1;
        }


        //remove an edge
        public void RemoveEdge(int originLabel, int destLabel)
        {
            RemoveEdge(originLabel, destLabel, isDirectedGraph);
        }

        private void RemoveEdge(int originLabel, int destLabel, bool isDirected)
        {
            //find origin in vertices
            int i = FindIndex(originLabel);

            if (i == -1)
                throw new Exception($"{originLabel} vertex does not exist yet");


            //adjacencyList[i].Add(destLabel);
            if (adjacencyList[i].Remove(destLabel) == false)
                throw new Exception($"{destLabel} was not found in the list");

            if (isDirected == false)
            {
                RemoveEdge(destLabel, originLabel, true);
            }
        }
        //remove vertex - long
        public void RemoveVertex(int label)
        {
            int i = FindIndex(label);

            if (i == -1)
                throw new Exception($"{label} vertex does not exist");

            vertices.RemoveAt(i);
            adjacencyList.RemoveAt(i);

            //to do - remove "label" from all the other lists
            foreach (var list in adjacencyList)
            {
                while (list.Remove(label))
                    ;
            }

        }

        //display edges
        public void DisplayEdges()
        {
            Console.WriteLine("Displaying Adjacency List .... ");
            for (int i = 0; i < adjacencyList.Count; i++)
            {
                Console.Write(vertices[i].Label + ": ");


                foreach (var label in adjacencyList[i])
                    Console.Write(label + " ");


                Console.WriteLine();
            }
        }

        //display vertices
        public void DisplayVertices()
        {
            Console.WriteLine("Displaying Vertices .... ");
            foreach (Vertex<int> v in vertices)
                Console.Write(v.Label + " ");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        //CTOR
        public Graph(bool isDirectedGraph = true)
        {
            this.isDirectedGraph = isDirectedGraph;
        }
        //n/a
    }
}
