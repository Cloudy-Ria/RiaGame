namespace Enemies.Interfaces
{
    /// <summary>
    /// Интерфейс для объектов, которые могут наносить урон
    /// </summary>
    public interface IDamaging
    {
        void ApplyDamage(HealthManager player);
    }
}

