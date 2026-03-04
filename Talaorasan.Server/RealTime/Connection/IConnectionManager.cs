using System.Collections.Concurrent;

namespace Talaorasan.Server.RealTime.Connection
{
    public interface IConnectionManager
    {
        void AddConnection(string deviceId, string connectionId);
        void RemoveConnection(string connectionId);
        List<string> GetConnections(string userId);
    }

    public class ConnectionManager
    {
        private readonly ConcurrentDictionary<string, List<string>> _deviceConnections = new();
        private readonly ConcurrentDictionary<string, string> _connectionToDevice = new();

        public void AddConnection(string deviceId, string connectionId)
        {
            _connectionToDevice[connectionId] = deviceId;

            _deviceConnections.AddOrUpdate(deviceId,
                _ => new List<string> { connectionId },
                (_, list) =>
                {
                    lock (list) list.Add(connectionId);
                    return list;
                });
        }

        public void RemoveConnection(string connectionId)
        {
            if (_connectionToDevice.TryRemove(connectionId, out var deviceId) &&
                _deviceConnections.TryGetValue(deviceId, out var list))
            {
                lock (list)
                {
                    list.Remove(connectionId);
                    if (list.Count == 0)
                        _deviceConnections.TryRemove(deviceId, out _);
                }
            }
        }

        public List<string> GetConnections(string userId)
        {
            return _deviceConnections.TryGetValue(userId, out var list) ? list : new List<string>();
        }
    }
}
