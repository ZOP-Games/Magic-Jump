using JetBrains.Annotations;

namespace GameExtensions
{
    //this generates the warehouse everyone uses
    public class WarehouseFactory
    {
        [CanBeNull] public static Warehouse Warehouse { get; private set; }

        public WarehouseFactory(Player player)
        {
            Warehouse = new Warehouse(player);
        }
    }

        

    
}
