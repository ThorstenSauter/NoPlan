namespace NoPlan.Infrastructure.Data.Models;

public class Tag
{
    public string Name { get; set; } = null!;
    public DateTime AssignedAt { get; set; }

    public static bool operator ==(Tag? left, Tag? right) =>
        Equals(left, right);

    public static bool operator !=(Tag? left, Tag? right) =>
        !Equals(left, right);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((Tag)obj);
    }

    public override int GetHashCode() =>
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        Name.GetHashCode();

    protected bool Equals(Tag other) =>
        Name == other.Name;
}
