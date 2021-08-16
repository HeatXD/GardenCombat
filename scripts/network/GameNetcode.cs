// source https://gist.github.com/rcmagic/f8d76bca32b5609e85ab156db38387e9
using AF = Abacus.Fixed64Precision;
public class GameNetcode
{
    //Game Events
    public delegate void GameEventDelegate();
    public delegate void GameEventDelegateDT(AF.Fixed64 dt);
    public event GameEventDelegate StoreGameEvent;
    public event GameEventDelegate RestoreGameEvent;
    public event GameEventDelegateDT UpdateGameEvent;
    //Specifies the maximum number of frames that can be resimulated
    public const uint MaxRollbackFrames = 10;
    //Specifies the number of frames the local client can progress ahead of the remote
    public const uint FrameAdvantageLimit = 7;
    //Specifies the initial frame the game starts in. Cannot rollback before this frame.
    public const int InitialFrame = 1;
    //Tracks the latest updates frame. 
    public int LocalFrame { get; set; }
    //Tracks the latest frame received from the remote client
    public int RemoteFrame { get; set; }
    //Tracks the last frame where we synchronized the game state with the remote client. Never rollback before this frame.
    public int SyncFrame { get; set; }
    // Latest frame advantage received from the remote client.
    public int RemoteFrameAdvantage { get; set; }

    public GameNetcode()
    {
        //set variables to InitialFrame
        this.LocalFrame = InitialFrame;
        this.RemoteFrame = InitialFrame;
        this.SyncFrame = InitialFrame;
        //game starts even
        this.RemoteFrameAdvantage = 0;
    }
    public bool RollbackCondition()
    {
        //No need to rollback if we don't have a frame after the previous sync frame to synchronize to.
        return LocalFrame > SyncFrame && RemoteFrame > SyncFrame;
    }
    public bool TimeSyncedCondition()
    {
        //How far the client is ahead of the last reported frame by the remote client
        int localFrameAdvantage = LocalFrame - RemoteFrame;
        //How different is the frame advantage reported by the remote client and this one.
        int frameAdvantageDifference = localFrameAdvantage - RemoteFrameAdvantage;
        //Only allow the local client to get so far ahead of remote
        return localFrameAdvantage < MaxRollbackFrames && frameAdvantageDifference <= FrameAdvantageLimit;
    }
    public void UpdateGamestate(AF.Fixed64 dt)
    {
        UpdateGameEvent(dt);
    }
    public void RestoreGamestate()
    {
        RestoreGameEvent();
    }
    public void StoreGamestate()
    {
        StoreGameEvent();
    }
}