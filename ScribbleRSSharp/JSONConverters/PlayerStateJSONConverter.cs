using System;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// scribble.rs # JSON converters namespace
/// </summary>
namespace ScribbleRSSharp.JSONConverters
{
    internal class PlayerStateJSONConverter : JsonConverter<EPlayerState>
    {
        public override EPlayerState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            EPlayerState ret = EPlayerState.Standby;
            if (reader.TokenType == JsonTokenType.String)
            {
                switch (reader.GetString())
                {
                    case "standby":
                        ret = EPlayerState.Standby;
                        break;
                    case "drawing":
                        ret = EPlayerState.Drawing;
                        break;
                    case "guessing":
                        ret = EPlayerState.Guessing;
                        break;
                }
            }
            return ret;
        }

        public override void Write(Utf8JsonWriter writer, EPlayerState value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToLower());
        }
    }
}
