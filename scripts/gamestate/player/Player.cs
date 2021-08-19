using System;
using AF = Abacus.Fixed64Precision;
public struct Player
{
    public sbyte ID { get; set; } // ID of the player corresponds to the player's index in the array. an id shouldnt be negative
    public AF.Vector2 Position { get; set; }
    public AF.Vector2 Velocity { get; set; }
    public AF.Vector2 Acceleration { get; set; }
    public PlayerInput GameInput { get; set; }

    public void ResolveMapCollisions()
    {
        throw new NotImplementedException();
    }
    public void ProcessMotion(AF.Fixed64 dt)
    {
        this.Velocity += this.Acceleration * dt;
        this.Position += this.Velocity * dt;
    }
}
