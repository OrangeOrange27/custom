using System;
using System.Collections.Generic;

public class DisposableBag : IDisposable
{
    private List<IDisposable> _disposables = new();
    private bool _isDisposed = false;

    public void Add(IDisposable disposable)
    {
        if (_isDisposed)
        {
            disposable.Dispose();
            return;
        }

        _disposables.Add(disposable);
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        foreach (var disposable in _disposables)
        {
            try
            {
                disposable.Dispose();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Exception during Dispose: {ex}");
            }
        }

        _disposables.Clear();
        _isDisposed = true;
    }
}