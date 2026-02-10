namespace InventoryManagementSystem
{
    /// <summary>
    /// Centralized database configuration
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        /// SQL Server connection string - Update this with your database server details
        /// </summary>
        public static string ConnectionString { get; } = 
            "Data Source=DESKTOP-9BMMS5L\\SQLEXPRESS;Initial Catalog=InventoryManagements;Integrated Security=True;Trust Server Certificate=True";
    }
}
