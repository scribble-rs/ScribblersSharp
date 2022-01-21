using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes game over data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class GameOverData : ReadyData, IValidable
    {
        /// <summary>
        /// Previous word
        /// </summary>
        [JsonProperty("previousWord")]
        public string PreviousWord { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            !string.IsNullOrWhiteSpace(PreviousWord);
    }
}
