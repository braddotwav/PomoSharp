using CommunityToolkit.Mvvm.ComponentModel;

namespace PomoSharp.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    public abstract string Name { get; }

    public abstract void OnViewShow();
    public abstract void OnViewHide();
}