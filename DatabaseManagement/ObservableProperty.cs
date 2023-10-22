using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DatabaseManagement;

public abstract class ObservableProperty : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string property = "")
    {
        Console.WriteLine(property);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }

    protected void SetProperty<T>(ref T obj, T value)
    {
        if (Equals(value, obj)) return;
        obj = value;
        OnPropertyChanged(nameof(obj));
    }
}