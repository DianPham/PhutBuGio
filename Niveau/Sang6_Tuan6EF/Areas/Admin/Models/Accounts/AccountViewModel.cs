namespace Niveau.Areas.Admin.Models.Accounts
{
    public class AccountViewModel
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }
    }
}
