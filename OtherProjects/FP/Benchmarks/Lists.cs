using System;
using System.Collections.Generic;
using System.Linq;
using FP.Collections.Persistent;
using MiniBench;

namespace Benchmarks {
    public class Lists {
        public static void Main() {
            var numbers = Enumerable.Range(0, 100);

            var results1 = new TestSuite<IEnumerable<int>, int>("Adding to end of the list")
                .Plus(AddToEndSCGList, "SCG.List")
                .Plus(AddToEndRandomAccessSequence, "RandomAccessSequence")
                .Plus(AddToEndVector, "Vector")
                .RunTests(numbers, 100)
                .ScaleByBest(ScalingMode.VaryDuration);

            results1.Display(
                ResultColumns.NameAndDuration | ResultColumns.Score,
                results1.FindBest());

            var results2 = new TestSuite<IEnumerable<int>, int>("Adding to beginning of the list")
                .Plus(AddToBeginningSCGList, "SCG.List")
                .Plus(AddToBeginningRandomAccessSequence, "RandomAccessSequence")
                .RunTests(numbers, 100)
                .ScaleByBest(ScalingMode.VaryDuration);

            results2.Display(
                ResultColumns.NameAndDuration | ResultColumns.Score,
                results1.FindBest());

            Console.ReadLine();
        }

        private static int AddToBeginningRandomAccessSequence(IEnumerable<int> input) {
            var list = RandomAccessSequence.Empty<int>();
            
            foreach (var i in input) {
                list = list.Prepend(i);
            }

            return list.Count;
        }

        private static int AddToBeginningSCGList(IEnumerable<int> input) {
            var list = new List<int>();

            foreach (var i in input) {
                list.Insert(0, i);
            }

            return list.Count;
        }

        public static int AddToEndSCGList(IEnumerable<int> input) {
            var list = new List<int>();

            foreach (var i in input) {
                list.Add(i);
            }

            return list.Count;
        }

        public static int AddToEndRandomAccessSequence(IEnumerable<int> input) {
            var list = RandomAccessSequence.Empty<int>();

            list = list.AppendRange(input);

            return list.Count;
        }

        public static int AddToEndVector(IEnumerable<int> input) {
            var list = Vector<int>.Empty;

            foreach (var i in input) {
                list = list.Append(i);
            }

            return list.Count;
        }
    }
}
