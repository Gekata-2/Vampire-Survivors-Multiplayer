namespace _Project.Scripts.Player
{
    public interface IPlayerView
    {
        void SetHealth(float value);

        void SetMaxHealth(float value);

        void SetLevel(int lvl);

        void SetExperienceNormalized(float value);
    }
}