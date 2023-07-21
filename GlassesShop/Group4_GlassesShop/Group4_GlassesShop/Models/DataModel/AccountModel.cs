namespace Group4_GlassesShop.Models.DataModel
{
	public class AccountModel
	{
		public int AccountId { get; set; }
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public bool? Role { get; set; }
		public bool Active { get; set; }

        public AccountModel()
        {
            
        }

        public AccountModel(string email, string password, bool? role, bool active)
		{
			Email = email;
			Password = password;
			Role = role;
			Active = active;
		}
	}
}
