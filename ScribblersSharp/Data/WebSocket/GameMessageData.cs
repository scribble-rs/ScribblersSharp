using Newtonsoft.Json;
using System;

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
    internal class GameMessageData<T> : BaseGameMessageData, IGameMessageData<T>
    {
        /// <summary>
        /// Game message data
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            Protection.IsValid(Data);

        /// <summary>
        /// Constructs game message data for serializers
        /// </summary>
        public GameMessageData()
        {
            // ...
        }

        /// <summary>
        /// Constructs game message data
        /// </summary>
        /// <param name="type">Message type</param>
        /// <param name="data">Message data</param>
        public GameMessageData(string type, T data) : base(type)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (!Protection.IsValid(data))
            {
                throw new ArgumentException("Data is not valid.", nameof(data));
            }
            Data = data;
        }
    }
}
