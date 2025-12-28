using EliteJournalReader.Events;
using System.Threading.Tasks;

namespace EliteJournalReader.Tests
{
    internal class FakeStatusWatcher : StatusWatcher
    {
        public Task StartWatching(string path)
        {
            Initialize(path);
            return StartWatching();
        }

        public void SendFakeStatusUpdate(StatusFileEvent evt) => FireStatusUpdatedEvent(evt);
    }
}
