namespace RoverSimulator.Data.Config
{
    public interface IConfigProvider
    {
        bool IsConfigOverridden { get; set; }
        RoverConfig GetRoverConfig();
        void OverrideRoverConfig(RoverConfig config);
    }
}