using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;
using Orleans.Providers;
using Orleans.Streams;

namespace Grains
{
    [ImplicitStreamSubscription("messages")]
    public class CharCountGrain : Grain, ICharCountGrain
    {
        public override async Task OnActivateAsync()
        {
            var id = this.GetPrimaryKey().ToString("N");
            var log = this.GetLogger($"CharCountGrain-{id}");
            var provider = this.GetStreamProvider("InMemory");
            await provider.GetStream<ChatMessage>(this.GetPrimaryKey(), "messages").SubscribeAsync(async (message, token) =>
            {
                // Create a map of char -> frequency
                var freqs = new Dictionary<char, int>();

                // Create a character frequency histogram for the message body.
                // (Pretend this is the kind of thing we would want to fan out)
                foreach (var c in message.Body)
                {
                    int initialValue;
                    freqs.TryGetValue(c, out initialValue);
                    freqs[c] = initialValue + 1;
                }

                log.Info($"Processed message of length {message.Body.Length}");

                // Send the stats of to the collector grain.
                await GrainFactory.GetGrain<IStatisticsCollectorGrain>(0).ReportStats(freqs);
            });
        }
    }
}
