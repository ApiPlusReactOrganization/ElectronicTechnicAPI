using Domain.Products;

namespace Domain.Sockets
{
    public class Socket
    {
        public SocketId Id { get; }
        public string Name { get; private set; }
        private Socket(SocketId id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Socket New(SocketId id, string name)
        {
            return new Socket(id, name);
        }

        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}
