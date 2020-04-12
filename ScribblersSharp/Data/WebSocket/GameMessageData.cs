using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Game message data class
    /// </summary>
    /// <typeparam name="T">Game message data type</typeparam>
    [JsonObject(MemberSerialization.OptIn)]
    internal class GameMessageData<T> : BaseGameMessageData
    {
        /// <summary>
        /// Game message data
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
