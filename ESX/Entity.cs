namespace ESX;

public abstract class Entity : ObservableProperty
{
    private Guid _id = Guid.NewGuid();

    public Guid Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }
}