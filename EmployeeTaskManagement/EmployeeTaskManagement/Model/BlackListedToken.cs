namespace EmployeeTaskManagement.Model
{
    public class BlackListedToken
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
