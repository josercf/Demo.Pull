using System.Collections.Generic;
using System.Linq;

namespace Demo.Pull
{
    public class Connection
    {
        //Quantas requisições estão ativas por instancia da DLL
        private IDictionary<string, long> requestsCounter;

        //Gerenciar as instancias da DLL
        private IDictionary<string, App> connections;

        private static Connection _connection;

        private Connection()
        {
            requestsCounter = new Dictionary<string, long>();
            connections = new Dictionary<string, App>();
        }

        public static Connection GetConnection()
        {
            if (_connection == null)
                _connection = new Connection();

            return _connection;
        }

        public App GetAppInstance()
        {
            if (connections == null || connections.Count == 0)
            {
                var instance = new App();

                requestsCounter.Add(instance.SessionId, 1);
                connections.Add(instance.SessionId, instance);
            }

            return GetBestInstance();
        }

        private App GetBestInstance()
        {
            var connectionOrdered = requestsCounter.OrderBy(x => x.Value)
                                                   .ToDictionary(x => x.Key, x => x.Value);

            var empty = new KeyValuePair<string, long>();
            KeyValuePair<string, long> item = new KeyValuePair<string, long>();

            for (int i = 0; i < connectionOrdered.Count; i++)
            {
                //Verificar se o IIS fechou a coneção
                // validações
                item = connectionOrdered.ElementAt(i);
                if (item.Value >= 25) break;
            }

            if (item.Equals(empty))
            {
                var instance = new App();

                requestsCounter.Add(instance.SessionId, 0);
                connections.Add(instance.SessionId, instance);
                item = new KeyValuePair<string, long>(instance.SessionId, 0);
            }

            requestsCounter[item.Key] = requestsCounter[item.Key] + 1;
            return connections[item.Key];
        }


        public void AddRequestCount(string instanceId)
        {
            requestsCounter[instanceId] = requestsCounter[instanceId] + 1;
        }

        public void ResetCall(string instanceId)
        {
            if (requestsCounter[instanceId] > 1)
                requestsCounter[instanceId] = requestsCounter[instanceId] - 1;
            else
                requestsCounter[instanceId] = 0;
        }
    }

    public class App
    {
        public string SessionId { get; set; }


        public void FazAlgo()
        {
        }
    }
}
