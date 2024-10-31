using Balancy.Models;

public class BackgroundsModel 
{
    public BackgroundType CurrentBackgroundType { get; private set; }

    public BackgroundsModel()
    {
        CurrentBackgroundType = BackgroundType.None;
    }
    public void SetType(BackgroundType type)
    {
        CurrentBackgroundType = type;
    }
}
