using System;
using System.Collections;
using System.Collections.Generic;
#if !SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
using System.Threading.Tasks;
#endif

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

        /// <summary>
        /// Is contained in collection
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="onContains">Gets invoked for each element</param>
        /// <returns>"true" if element is contained, otherwise "false"</returns>
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
#if SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
            foreach (T element in collection)
#else
            Parallel.ForEach(collection, (element, parallelLoopState) =>
#endif
            {
                if (onContains(element))
                {
                    ret = true;
#if SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
                    break;
#else
                    parallelLoopState.Break();
#endif
                }
            }
#if !SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
            );
#endif
            return ret;
        }

        /// <summary>
        /// Are elements in collection unique
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="onAreUnique">Gets invoked for each element each element except self</param>
        /// <returns>"true" if elements in collection are unique, otherwise "false"</returns>
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
#if SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
            for (int leftIndex = 0; leftIndex < collection.Count; leftIndex++)
#else
            Parallel.For(0, collection.Count, (leftIndex, parallelLoopState) =>
#endif
            {
                for (int right_index = 0; right_index < collection.Count; right_index++)
                {
                    if ((leftIndex != right_index) && !onAreUnique(collection[leftIndex], collection[right_index]))
                    {
                        ret = false;
#if SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
                        break;
#else
                        parallelLoopState.Break();
#endif
                    }
                }
            }
#if !SCRIBBLERS_SHARP_NO_PARALLEL_LOOPS
            );
#endif
            return ret;
        }
    }
}
