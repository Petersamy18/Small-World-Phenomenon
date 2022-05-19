using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Small_World_Phenomenon
{
    class SmallWorld
    {
        //Attributes
        public static List<Movie> allMovies = new List<Movie>(); //list of class movies 
        public static HashSet<string> uniqeActors = new HashSet<string>(); //list of all actors 
        public static Dictionary<string, int> actors_as_int = new Dictionary<string, int>();
        public static Dictionary<int, string> actors_as_strings = new Dictionary<int, string>();

        public static List<Dictionary<int, int>> Actors_adjlist = new List<Dictionary<int, int>>();
        public static List<Dictionary<string, string>> Actor_MoviePair = new List<Dictionary<string, string>>();


        #region Reading TestCases Files To Work On It.
        public static string[] actorsLines;
        public static string[] solutionLines;
        public readonly string textFile;
        public SmallWorld(int choice)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            if (choice == 1)
            {
                textFile = @"Testcases\Sample\movies1.txt";
                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Sample\queries1.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Sample\queries1 - Solution.txt");

            }
            else if (choice == 2)
            {
                textFile = @"Testcases\Complete\small\Case1\Movies193.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\small\Case1\queries110.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\small\Case1\Solution\queries110 - Solution.txt");
            }
            else if (choice == 3)
            {
                textFile = @"Testcases\Complete\small\Case2\Movies187.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\small\Case2\queries50.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\small\Case2\Solution\queries50 - Solution.txt");
            }
            else if (choice == 4)
            {
                textFile = @"Testcases\Complete\medium\Case1\Movies967.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case1\queries85.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case1\Solutions\queries85 - Solution.txt");

            }
            else if (choice == 44)
            {
                textFile = @"Testcases\Complete\medium\Case1\Movies967.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case1\queries4000.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case1\Solutions\queries4000 - Solution.txt");

            }
            else if (choice == 5)
            {
                textFile = @"Testcases\Complete\medium\Case2\Movies4736.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case2\queries110.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case2\Solutions\queries110 - Solution.txt");

            }
            else if (choice == 55)
            {
                textFile = @"Testcases\Complete\medium\Case2\Movies4736.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case2\queries2000.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\medium\Case2\Solutions\queries2000 - Solution.txt");
            }
            else if (choice == 6)
            {
                textFile = @"Testcases\Complete\large\Movies14129.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\large\queries26.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\large\Solutions\queries26 - Solution.txt");
            }
            else if (choice == 66)
            {
                textFile = @"Testcases\Complete\large\Movies14129.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\large\queries600.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\large\Solutions\queries600 - Solution.txt");
            }
            else if (choice == 7)
            {
                textFile = @"Testcases\Complete\extreme\Movies122806.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\extreme\queries22.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\extreme\Solutions\queries22 - Solution.txt");
            }
            else if (choice == 77)
            {
                textFile = @"Testcases\Complete\extreme\Movies122806.txt";

                actorsLines = System.IO.File.ReadAllLines(@"Testcases\Complete\extreme\queries200.txt");
                solutionLines = System.IO.File.ReadAllLines(@"Testcases\Complete\extreme\Solutions\queries200 - Solution.txt");
            }
            #endregion


            CreateAdjacencyList_MoviesActors();
            CreateAdjacencyList_Actors();

            // variables to store source and destination(Handle queries)
            for (int i = 0; i < actorsLines.Length; i++)
            {
                int firstIndex = actorsLines[i].IndexOf('/');
                string actor1 = actorsLines[i].Substring(0, firstIndex);
                string actor2 = actorsLines[i].Substring(firstIndex + 1);
                //Bfs_Visit(actor1, actor2);
            }
            watch.Stop();
            var time = watch.Elapsed;

            Console.WriteLine($"Execution Time: {time} ms");

        }
        public void Initialize()
        {
            for (int i = 0; i < uniqeActors.Count(); i++)
            {
                Actors_adjlist.Add(new Dictionary<int, int>());
                Actor_MoviePair.Add(new Dictionary<string, string>());
            }
        }


        public void CreateAdjacencyList_MoviesActors()
        {
            using (StreamReader file = new StreamReader(textFile))
            {
                string movie;
                while ((movie = file.ReadLine()) != null)
                {
                    Movie m = new Movie();
                    //Extract the movie name
                    int firstIndex = movie.IndexOf('/');
                    string movieName = movie.Substring(0, firstIndex);
                    m.NameOfMovie = movieName;

                    //remove the movie name from the line
                    movie = movie.Remove(0, firstIndex + 1);

                    //put the actors as values to the key which is the movie
                    string[] actorsList = movie.Split('/');
                    foreach (var actor in actorsList)
                    {
                        uniqeActors.Add(actor);
                        m.actors.Add(actor);
                    }

                    allMovies.Add(m);
                }
                file.Close();
                Console.WriteLine($"File Ended");
            }

            int ID = 0;
            foreach (var item in uniqeActors)
            {
                actors_as_int.Add(item.ToString(), ID); //giving each actor an ID (easier and faster to deal with later)

                actors_as_strings.Add(ID, item.ToString()); // the opposite to the above statement
                ID++;
            }

        }
        
        
           public void CreateAdjacencyList_Actors()
        {
            Initialize();


            foreach (Movie movie in allMovies)
            {
                int first = 0;
                int movie_actorsL = movie.actors.Count();
                while (first < movie_actorsL)
                {
                    for (int second = 0; second < movie_actorsL; second++)
                    {
                        int secondActor = actors_as_int[movie.actors[second]];
                        int firstActor = actors_as_int[movie.actors[first]];

                        //checking if the two actors is not the same
                        if (movie.actors[first] != movie.actors[second])
                        {
                            //checking if they have not been discovered with each other before
                            if (Actors_adjlist[firstActor].ContainsKey(secondActor) == false)
                            {
                                Actor_MoviePair[firstActor].Add(actors_as_strings[secondActor], movie.NameOfMovie);

                                //saving that these two actors have performed together
                                Actors_adjlist[firstActor].Add(secondActor, 1);
                            }

                            //checking if they have been discovered with each other before
                            else if (Actors_adjlist[firstActor].ContainsKey(secondActor) == true)
                            {
                                Actors_adjlist[firstActor][secondActor]++; //increment the no. Movies they have performed in together
                            }

                        }
                    }

                    first++;
                }

            }
        }


        
    }
}
