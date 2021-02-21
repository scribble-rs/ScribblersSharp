using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Create lobby response data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class CreateLobbyResponseData : EnterLobbyResponseData
    {
        // ...
    }
}
