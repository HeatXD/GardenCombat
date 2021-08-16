// source https://gist.github.com/rcmagic/f8d76bca32b5609e85ab156db38387e9
public class GameNetwork
{
    public const uint MaxRollbackFrames = 7;
    public const uint FrameAdvantageLimit = 5;
    public const int InitialFrame = 1;

    public int LocalFrame { get; set; }
    public int RemoteFrame { get; set; }
    public int SyncFrame { get; set; }
    public int RemoteFrameAdvantage { get; set; }
    public GameNetwork()
    {
        this.LocalFrame = InitialFrame;
        this.RemoteFrame = InitialFrame;
        this.SyncFrame = InitialFrame;
        //game starts even
        this.RemoteFrameAdvantage = 0;
    }

    public bool RollbackCondition(){
        return LocalFrame > SyncFrame && RemoteFrame > SyncFrame;
    }

    public bool TimeSyncedCondition(){ 
        int localFrameAdvantage = LocalFrame - RemoteFrame;
        int frameAdvantageDifference = localFrameAdvantage - RemoteFrameAdvantage;
        return localFrameAdvantage < MaxRollbackFrames && frameAdvantageDifference <= FrameAdvantageLimit;
    }
}