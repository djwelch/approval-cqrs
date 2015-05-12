using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace Domain.Infrastructure
{
    public class EventStore<TEvent> where TEvent : class
    {
        private static EventStore<TEvent> _instance;

        public static EventStore<TEvent> Instance
        {
            get
            {
                return _instance ?? (_instance = new EventStore<TEvent>());
            }
        }

        private readonly IEventStoreConnection eventStoreConnection;

        public EventStore()
        {
            eventStoreConnection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            eventStoreConnection.ConnectAsync().Wait();
        }

        public IEnumerable<TEvent> Read(Guid id)
        {
            string streamName = typeof(TEvent).FullName + "_" + id.ToString();

            var streamEvents = new List<ResolvedEvent>();

            StreamEventsSlice currentSlice;
            var nextSliceStart = StreamPosition.Start;
            do
            {
                currentSlice = eventStoreConnection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, 200, false).Result;
                nextSliceStart = currentSlice.NextEventNumber;

                foreach (var e in currentSlice.Events)
                {
                    yield return JsonConvert.DeserializeObject<TEvent>(Encoding.UTF8.GetString(e.Event.Data));
                }

            } while (!currentSlice.IsEndOfStream);

            //eventStoreConnection.ReadStreamEventsForwardAsync(typeof(TEvent).AssemblyQualifiedName, 0, -1, false).RunSynchronously();                
        }

        public void Append(Guid id, int? expectedVersion, IEnumerable<TEvent> events)
        {
            string streamName = typeof(TEvent).FullName + "_" + id.ToString();

            var eventdatas = events.Select(x => new EventData(Guid.NewGuid(), x.GetType().AssemblyQualifiedName, true, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(x, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.All, TypeNameHandling = TypeNameHandling.All })), new byte[] { }));

            var writeResult = eventStoreConnection.AppendToStreamAsync(streamName, expectedVersion ?? ExpectedVersion.NoStream, eventdatas).Result;
            var nextVersion = writeResult.NextExpectedVersion;
        }
    }
}
