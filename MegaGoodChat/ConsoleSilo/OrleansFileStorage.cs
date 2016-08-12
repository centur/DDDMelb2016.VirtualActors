using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Storage;

namespace ConsoleSilo
{
    // This is a simple Orleans storage provider. It stores your grain's internal state in files.
    // This is not suitable for running in the cloud, but it's fine for testing on a single machine.

    // TODO: After sending some messages to a room, check out the ConsoleSilo\bin\Debug\grain_state folder.

    public class OrleansFileStorage : IStorageProvider
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings();
        private DirectoryInfo directory;
        
        public string Name { get; protected set; }
        
        public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            this.Name = name;

            // NOTE: this could be specified on the config object instead.
            this.directory = new DirectoryInfo(Path.Combine(GetAssemblyPath(), "grain_state"));
            if (!this.directory.Exists) directory.Create();

            this.Log = providerRuntime.GetLogger(this.GetType().FullName);
            return Task.FromResult(0);
        }

        public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var grainTypeName = grainType.Split('.').Last();
           
            var fileInfo = GetStorageFilePath(grainTypeName, grainReference.ToKeyString());
            if (!fileInfo.Exists) return;

            using (var stream = fileInfo.OpenText())
            {
                var data = await stream.ReadToEndAsync();
                if (data != null)
                {
                    JsonConvert.PopulateObject(data, grainState.State);
                }
            }
        }
        
        public async Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var grainTypeName = grainType.Split('.').Last();
            var data = JsonConvert.SerializeObject(grainState.State, SerializerSettings);
            var fileInfo = GetStorageFilePath(grainTypeName, grainReference.ToKeyString());
            using (var stream = new StreamWriter(fileInfo.Open(FileMode.Create, FileAccess.Write)))
            {
                await stream.WriteAsync(data);
            }
        }

        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var grainTypeName = grainType.Split('.').Last();
            var fileInfo = GetStorageFilePath(grainTypeName, grainReference.ToKeyString());
            if (fileInfo.Exists) fileInfo.Delete();
            return TaskDone.Done;
        }

        public Task Close() => Task.FromResult(0);

        private FileInfo GetStorageFilePath(string collectionName, string key)
        {
            var fileName = key + "." + collectionName;
            var path = Path.Combine(this.directory.FullName, fileName);
            return new FileInfo(path);
        }

        public Logger Log { get; protected set; }

        private static string GetAssemblyPath()
        {
            return Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        }
    }
}