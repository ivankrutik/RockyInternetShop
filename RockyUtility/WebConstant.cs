using System.Collections.ObjectModel;

namespace RockyUtility
{
    public static class WebConstant
    {
        public const string ImgPath = @"\images\product\";
        public const string SessionCart = @"ShoppingCartSession";
        public const string SessionInquiryId = @"SessionInquiryId";


        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        public const string EmailAdmin = "ivan.krutik@gmail.com";

        public const string CategoryName = "Category";
        public const string AppTypeName = "AppType";

        public const string Success = "Success";
        public const string Error = "Error";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "InProcess";
        public const string StatusShiped = "Shiped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static readonly IEnumerable<string> AllStatuses =
            new ReadOnlyCollection<string>(new List<string>()
            {
                StatusApproved, StatusCancelled, StatusInProcess, StatusPending, StatusRefunded, StatusShiped
            });
    }
}
