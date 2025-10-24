namespace Shared.Entities;

public abstract class BaseEntity : BaseEntity<Guid>;

public abstract class BaseEntity<TKey>
{
    private bool _isDeleted;
    public TKey Id { get; set; } = default!;
    public DateTime CreationDate { get; init; } = DateTime.UtcNow;
    public string? CreatedById { get; init; }
    public DateTime? ModificationDate { get; set; }
    public string? ModifiedById { get; set; }

    public DateTime? DeletionDate { get; set; }

    public string? DeletedById { get; set; }

    public bool IsDeleted
    {
        get => _isDeleted;
        set
        {
            _isDeleted = value;
            if (!_isDeleted)
            {
                DeletionDate = null;
                DeletedById = null;
            }
            else if (DeletionDate == null)
            {
                DeletionDate = DateTime.UtcNow;
            }
        }
    }
}