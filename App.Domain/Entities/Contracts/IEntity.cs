namespace App.Domain.Entities.Contracts
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
