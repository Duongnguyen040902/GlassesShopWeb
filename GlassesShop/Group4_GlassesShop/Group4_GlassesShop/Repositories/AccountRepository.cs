using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.Interface;
using Group4_GlassesShop.Models.Register;
using Microsoft.Win32;

namespace Group4_GlassesShop.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly GlassesShopContext _context;

        public AccountRepository(GlassesShopContext context)
        {
            _context = context;
        }
        public AccountModel AddAccount(AccountModel account, Customer customer)
        {
            var _account = new Account
            {
                AccountId = account.AccountId,
                Email = account.Email,
                Password = account.Password,
                Role = account.Role,
                Active = account.Active,
            };

            _context.Accounts.Add(_account);
            _context.SaveChanges();
            var newCus = new Customer
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                AccountId = _account.AccountId,
                PhoneNumber = customer.PhoneNumber,
                AvatarUrl = "/Avatar/avatar6.png"
            };
            _context.Customers.Add(newCus);
            _context.SaveChanges();
            return new AccountModel
            {
                Email = account.Email,
                Password = account.Password,
                Role = false,
                Active = false
            };
        }

        public AccountModel IsExitsAccount(AccountModel account)
        {
            var isExitsAcc = _context.Accounts.FirstOrDefault(a => a.Email == account.Email && a.Password == account.Password);
            if (isExitsAcc != null)
            {
                return new AccountModel
                {
                    AccountId = isExitsAcc.AccountId,
                    Email = account.Email,
                    Password = account.Password,
                    Role = isExitsAcc.Role,
                    Active = account.Active
                };
            }
            return null;
        }

        public bool IsExitsEmail(RegisterModel account)
        {
            var isExitsEmail = _context.Accounts.Where(a => a.Email == account.Email).FirstOrDefault();
            if (isExitsEmail != null)
            {
                return true;
            }
            return false;
        }
    }
}
