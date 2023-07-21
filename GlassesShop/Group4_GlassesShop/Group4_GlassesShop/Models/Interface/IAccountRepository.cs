using Group4_GlassesShop.Models.DataModel;
using Group4_GlassesShop.Models.Register;

namespace Group4_GlassesShop.Models.Interface
{
    public interface IAccountRepository
    {
        public AccountModel IsExitsAccount(AccountModel account);

        public bool IsExitsEmail(RegisterModel account);

        public AccountModel AddAccount(AccountModel account, Customer customer);

    }
}
