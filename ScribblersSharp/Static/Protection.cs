using System.Collections;

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
    }
}
