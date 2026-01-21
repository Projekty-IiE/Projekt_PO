using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using TradingSimulator.Core.Models;

namespace TradingSimulator.Core.Services
{
    /// <summary>
    /// Handles persistence of application session snapshots to and from the filesystem.
    /// Uses JSON serialization (Newtonsoft.Json) with <c>TypeNameHandling.Auto</c> to preserve concrete
    /// types when restoring polymorphic collections (transactions, portfolio items, stocks).
    /// </summary>
    public class FileService
    {
        /// <summary>
        /// Serializes the provided <see cref="SessionState"/> and writes it to <paramref name="path"/>.
        /// The JSON is written using indented formatting to make files human-readable.
        /// </summary>
        /// <param name="path">Full path to the file to write. If the file exists it will be overwritten.</param>
        /// <param name="state">Session snapshot to save. Passing <c>null</c> will serialize the JSON literal <c>null</c>.</param>
        /// <remarks>
        /// The method uses <see cref="JsonSerializerSettings.TypeNameHandling"/> = <see cref="TypeNameHandling.Auto"/>
        /// so that runtime types (e.g., derived Transaction types) are preserved during deserialization.
        /// Any I/O exceptions (for example <see cref="IOException"/>, <see cref="UnauthorizedAccessException"/>)
        /// thrown by <see cref="File.WriteAllText(string,string)"/> will propagate to the caller.
        /// </remarks>
        public void Save(string path, SessionState state)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            var json = JsonConvert.SerializeObject(state, settings);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Loads a <see cref="SessionState"/> from the specified file path.
        /// </summary>
        /// <param name="path">Full path to the saved JSON file.</param>
        /// <returns>
        /// Deserialized <see cref="SessionState"/> when the file exists and deserialization succeeds;
        /// otherwise <c>null</c> when the file does not exist. Note: deserialization or I/O errors will throw exceptions.
        /// </returns>
        /// <remarks>
        /// Uses <see cref="JsonSerializerSettings.TypeNameHandling"/> = <see cref="TypeNameHandling.Auto"/>
        /// so that concrete derived types included in the snapshot are correctly restored.
        /// The method deliberately returns <c>null</c> when the file is missing to simplify callers that
        /// want to treat "no saved session" as a valid state.
        /// </remarks>
        /// <exception cref="System.IO.IOException">If an I/O error occurs while reading the file.</exception>
        /// <exception cref="Newtonsoft.Json.JsonException">If JSON is invalid or cannot be deserialized to <see cref="SessionState"/>.</exception>
        public SessionState? Load(string path)
        {
            if (!File.Exists(path)) return null;
            var json = File.ReadAllText(path);
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            return JsonConvert.DeserializeObject<SessionState>(json, settings);
        }
    }
}
