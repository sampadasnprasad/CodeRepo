using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombinationGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> seq = new List<string>() { "sport_name", "tournament_organizer", "SoccerLeague", "group_name_altset", "intent_standings" };
            TextWriter tw = new StreamWriter(@"D:\VinData\Productions\productions_new.txt");

            var startingProduction = 1975;
            var output = new List<List<string>>();
            foreach (var permu in Permutate(seq, seq.Count))
            {
                var line = new List<string>();
                tw.Write(string.Format("<production id=\"{0}\" scenario=\"GroupStandings\">\n", startingProduction));
                //var production = string.Format("<production id=\"{0}\" scenario=\"TeamScore\">\n", startingProduction);
                foreach (var i in permu)
                {
                    line.Add(i.ToString());
                    tw.Write(string.Format("\t<token altset=\"{0}\"/>\n", i.ToString()));
                    Console.Write(i.ToString() + " ");
                }

                tw.Write(string.Format("</production>\n\n"));
                startingProduction++;
                output.Add(line);
                Console.WriteLine();
            }
            
        }

        public static void RotateRight(IList sequence, int count)
        {
            object tmp = sequence[count - 1];
            sequence.RemoveAt(count - 1);
            sequence.Insert(0, tmp);
        }

        public static IEnumerable<IList> Permutate(IList sequence, int count)
        {
            if (count == 1) yield return sequence;
            else
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (var perm in Permutate(sequence, count - 1))
                        yield return perm;
                    RotateRight(sequence, count);
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> GetPowerSet<T>(List<T> list)
        {
            return from m in Enumerable.Range(0, 1 << list.Count)
                   select
                       from i in Enumerable.Range(0, list.Count)
                       where (m & (1 << i)) != 0
                       select list[i];
        }

        public static IEnumerable<IEnumerable<T>> QuickPerm<T>(List<T> set)
        {
            int N = set.Count();
            int[] a = new int[N];
            int[] p = new int[N];

            var yieldRet = new T[N];

            List<T> list = new List<T>(set);

            int i, j, tmp; // Upper Index i; Lower Index j

            for (i = 0; i < N; i++)
            {
                // initialize arrays; a[N] can be any type
                a[i] = i + 1; // a[i] value is not revealed and can be arbitrary
                p[i] = 0; // p[i] == i controls iteration and index boundaries for i
            }
            yield return list;
            //display(a, 0, 0);   // remove comment to display array a[]
            i = 1; // setup first swap points to be 1 and 0 respectively (i & j)
            while (i < N)
            {
                if (p[i] < i)
                {
                    j = i % 2 * p[i]; // IF i is odd then j = p[i] otherwise j = 0
                    tmp = a[j]; // swap(a[j], a[i])
                    a[j] = a[i];
                    a[i] = tmp;

                    //MAIN!

                    for (int x = 0; x < N; x++)
                    {
                        yieldRet[x] = list[a[x] - 1];
                    }
                    yield return yieldRet;
                    //display(a, j, i); // remove comment to display target array a[]

                    // MAIN!

                    p[i]++; // increase index "weight" for i by one
                    i = 1; // reset index i to 1 (assumed)
                }
                else
                {
                    // otherwise p[i] == i
                    p[i] = 0; // reset p[i] to zero
                    i++; // set new index value for i (increase by one)
                } // if (p[i] < i)
            } // while(i < N)
        }
    }
}
