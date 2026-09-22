namespace StudentJobHub.Client.Services;

public enum ToastLevel
{
    Info,
    Success,
    Warning,
    Error
}

public class ToastMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Message { get; set; } = string.Empty;
    public ToastLevel Level { get; set; } = ToastLevel.Info;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class ToastService
{
    public event Action? OnToastsChanged;
    public List<ToastMessage> Toasts { get; } = new();

    public void ShowToast(string message, ToastLevel level = ToastLevel.Info, int autoHideMs = 4000)
    {
        var toast = new ToastMessage { Message = message, Level = level };
        Toasts.Add(toast);
        OnToastsChanged?.Invoke();

        _ = Task.Run(async () =>
        {
            await Task.Delay(autoHideMs);
            RemoveToast(toast.Id);
        });
    }

    public void ShowSuccess(string message) => ShowToast(message, ToastLevel.Success);
    public void ShowError(string message) => ShowToast(message, ToastLevel.Error);
    public void ShowWarning(string message) => ShowToast(message, ToastLevel.Warning);
    public void ShowInfo(string message) => ShowToast(message, ToastLevel.Info);

    public void RemoveToast(string id)
    {
        var toast = Toasts.FirstOrDefault(t => t.Id == id);
        if (toast != null)
        {
            Toasts.Remove(toast);
            OnToastsChanged?.Invoke();
        }
    }
}
