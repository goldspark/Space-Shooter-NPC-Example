using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utils
{
    public static class Algorithms
    {


        public static void QuickSort<T>(T[] array, int start, int end) where T : IComparable<T>
        {

            if(end <= start) return;

            int pivot = Partition(array, start, end);
            QuickSort(array, start, pivot - 1);
            QuickSort(array, pivot + 1, end);

        }

        private static int Partition<T>(T[] array, int start, int end) where T: IComparable<T>
        {
            int i = start - 1;
            T pivot = array[end];
            for(int j = start; j <= end - 1; j++)
            {
                if (array[j].CompareTo(pivot) < 0)
                {
                    i++;
                    Swap(array, i, j);
                }
            }

            i++;
            Swap(array, i, end);
            return i;
        }

        public static void Swap<T>(T[] array, int i, int j)
        {
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }


        public static bool BinarySearch<T>(T[] array, T item, out int index) where T : IComparable<T>
        {
            int low = 0;
            int high = array.Length - 1;
            while (low <= high)
            {
                int mid = low + (high - low) / 2;

                if (array[mid].CompareTo(item) == 0)
                {
                    index = mid;
                    return true;
                }
                else if (array[mid].CompareTo(item) < 0)
                    low = mid + 1;
                else
                    high = mid - 1;

            }

            index = -1;
            return false;
        }
    }
}
