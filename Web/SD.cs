namespace Micro.Web
{
	public static class SD
	{
		public static string CouponApiBase { get; set; }
		public static string AuthApiBase { get; set; }
        public static string ProductApiBase { get; set; }
		public static string ShoppingCartApiBase { get; set; }
		public static string RoleAdmin = "ADMIN";
		public static string RoleCustomer = "CUSTOMER";
		public static string TokenCookie = "JwtToken";
		public enum ApiType
		{
			GET,
			POST, 
			PUT, 
			DELETE
		}
	}
}
