namespace Settle_App.Helpers
{
    public  class StringHelper
    {
        public string CapitalizeFirstLetter(string value)
        { 
            if(string.IsNullOrEmpty(value)) return value;

            return char.ToUpper(value[0]) + value[1..];

        }
    }
}
