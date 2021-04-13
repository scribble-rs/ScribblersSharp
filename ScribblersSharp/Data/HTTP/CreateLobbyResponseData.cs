using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes response data for creating a lobby
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class CreateLobbyResponseData : EnterLobbyResponseData
    {
        // ...
    }
}
