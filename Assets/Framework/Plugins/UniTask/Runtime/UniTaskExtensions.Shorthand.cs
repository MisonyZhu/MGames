#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System.Collections.Generic;

namespace Cysharp.Threading.Tasks
{
    public static partial class UniTaskExtensions
    {
        // shorthand of WhenAll
    
        public static UniTask.Awaiter GetAwaiter(this UniTask[] tasks)
        {
            return UniTask.WhenAll(tasks).GetAwaiter();
        }

        public static UniTask.Awaiter GetAwaiter(this IEnumerable<UniTask> tasks)
        {
            return UniTask.WhenAll(tasks).GetAwaiter();
        }

        public static UniTask<T[]>.Awaiter GetAwaiter<T>(this UniTask<T>[] tasks)
        {
            return UniTask.WhenAll(tasks).GetAwaiter();
        }

        public static UniTask<T[]>.Awaiter GetAwaiter<T>(this IEnumerable<UniTask<T>> tasks)
        {
            return UniTask.WhenAll(tasks).GetAwaiter();
        }

        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3, UniTask task4) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3, UniTask task4, UniTask task5) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3, UniTask task4, UniTask task5, UniTask task6) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3, UniTask task4, UniTask task5, UniTask task6, UniTask task7) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3, UniTask task4, UniTask task5, UniTask task6, UniTask task7, UniTask task8) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3, UniTask task4, UniTask task5, UniTask task6, UniTask task7, UniTask task8, UniTask task9) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3, UniTask task4, UniTask task5, UniTask task6, UniTask task7, UniTask task8, UniTask task9, UniTask task10) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3, UniTask task4, UniTask task5, UniTask task6, UniTask task7, UniTask task8, UniTask task9, UniTask task10, UniTask task11) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3, UniTask task4, UniTask task5, UniTask task6, UniTask task7, UniTask task8, UniTask task9, UniTask task10, UniTask task11, UniTask task12) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11, tasks.Item12).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3, UniTask task4, UniTask task5, UniTask task6, UniTask task7, UniTask task8, UniTask task9, UniTask task10, UniTask task11, UniTask task12, UniTask task13) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11, tasks.Item12, tasks.Item13).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3, UniTask task4, UniTask task5, UniTask task6, UniTask task7, UniTask task8, UniTask task9, UniTask task10, UniTask task11, UniTask task12, UniTask task13, UniTask task14) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11, tasks.Item12, tasks.Item13, tasks.Item14).GetAwaiter();
        }


        public static UniTask.Awaiter GetAwaiter(this (UniTask task1, UniTask task2, UniTask task3, UniTask task4, UniTask task5, UniTask task6, UniTask task7, UniTask task8, UniTask task9, UniTask task10, UniTask task11, UniTask task12, UniTask task13, UniTask task14, UniTask task15) tasks)
        {
            return UniTask.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5, tasks.Item6, tasks.Item7, tasks.Item8, tasks.Item9, tasks.Item10, tasks.Item11, tasks.Item12, tasks.Item13, tasks.Item14, tasks.Item15).GetAwaiter();
        }


    }
}