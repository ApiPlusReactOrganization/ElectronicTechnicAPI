namespace Domain.Sockets;

public record SocketId(Guid Value)
{
    public static SocketId New() => new(Guid.NewGuid());
    public static SocketId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}