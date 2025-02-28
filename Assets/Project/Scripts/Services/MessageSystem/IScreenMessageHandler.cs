namespace Project.Services.MessageSystem
{
    public interface IScreenMessageHandler
    {
        void ShowMessage(string key, string additionalText = "");
    }
}