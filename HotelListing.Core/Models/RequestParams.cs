namespace HotelListing.Core.Models
{
    public class RequestParams
    {
        const int maxPageSize = 20;
        public int page { get; set; }

        private int _size = 10;
        public int size
        {
            get { return _size; }
            set { _size = value > maxPageSize ? maxPageSize : value; }
        }
    }
}
