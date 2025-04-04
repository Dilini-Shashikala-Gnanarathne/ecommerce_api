    public static class Permissions
    {
        // General permissions
        public const string Login = "Permission.Login";
        public const string Register = "Permission.Register";
        public const string CreateOrder = "Permission.CreateOrder";
        public const string AddProduct = "Permission.AddProduct";
        public const string ViewAllProducts = "Permission.ViewAllProducts";

        // Cart related permissions
        public const string ViewCart = "Permission.ViewCart"; 
        public const string AddToCart = "Permission.AddToCart"; 

        // Order related permissions
        public const string ViewAllOrders = "Permission.ViewAllOrders"; 
        public const string ViewUserOrders = "Permission.ViewUserOrders"; 

        // Product related permissions
        public const string ViewProductsCust = "Permission.ViewProductsCust"; 
        public const string ViewProductsAdmin = "Permission.ViewProductsAdmin"; 
        public const string CreateProduct = "Permission.CreateProduct"; 

        // User management permissions
        public const string ViewUsers = "Permission.ViewUsers"; 
}
