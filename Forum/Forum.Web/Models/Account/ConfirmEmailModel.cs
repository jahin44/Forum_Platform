namespace Forum.Web.Models.Account
{
    public class ConfirmEmailModel
    {
        public ConfirmEmailModel()
        {
        }
        public string? StatusMessage { get; set; }
        public bool IsSuccess { get; set; }       
    }
}