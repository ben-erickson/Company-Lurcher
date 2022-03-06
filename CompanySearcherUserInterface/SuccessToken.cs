namespace CompanySearcherUserInterface
{
    public class SuccessToken
    {
        private bool _success;

        public SuccessToken()
        {
            _success = false;
        }

        public void ProcessSuccess()
        {
            this._success = true;
        }

        public bool CheckSuccess()
        {
            return this._success;
        }
    }
}
