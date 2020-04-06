/// <summary>
/// scribble.rs # namespace
/// </summary>
namespace ScribbleRSSharp
{
    /// <summary>
    /// Request data interface
    /// </summary>
    /// <typeparam name="T">Expected response data type</typeparam>
    internal interface IRequestData<T> where T : IResponseData
    {
        // ...
    }
}
