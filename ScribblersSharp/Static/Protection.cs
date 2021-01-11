using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Scribble.rs ♯ namespace
/// </summary>
namespace ScribblersSharp
{
    /// <summary>
    /// A class used for protection
    /// </summary>
    internal static class Protection
    {
        /// <summary>
        /// Is object valid
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>"true" if object the specified object is valid, otherwise "false"</returns>
        public static bool IsValid(object obj)
        {
            bool ret = false;
            if (obj != null)
            {
                ret = true;
                if (obj is IValidable validable_object)
                {
                    ret = validable_object.IsValid;
                }
                else if (obj is IEnumerable enumerable_object)
                {
                    foreach (object element in enumerable_object)
                    {
                        if (!IsValid(element))
                        {
                            ret = false;
                            break;
                        }
                    }
                }
            }
            return ret;
        }

        public static bool IsContained<T>(IEnumerable<T> collection, ContainsDelegate<T> onContains)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (onContains == null)
            {
                throw new ArgumentNullException(nameof(onContains));
            }
            bool ret = false;
            Parallel.ForEach(collection, (element, parallelLoopState) =>
            {
                if (onContains(element))
                {
                    ret = true;
                    parallelLoopState.Break();
                }
            });
            return ret;
        }

        public static bool AreUnique<T>(IReadOnlyList<T> collection, AreUniqueDelegate<T> onAreUnique)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (onAreUnique == null)
            {
                throw new ArgumentNullException(nameof(onAreUnique));
            }
            bool ret = true;
            Parallel.For(0, collection.Count, (leftIndex, parallelLoopState) =>
            {
                for (int right_index = 0; right_index < collection.Count; right_index++)
                {
                    if ((leftIndex != right_index) && !onAreUnique(collection[leftIndex], collection[right_index]))
                    {
                        ret = false;
                        parallelLoopState.Break();
                    }
                }
            });
            return ret;
        }
    }
}
