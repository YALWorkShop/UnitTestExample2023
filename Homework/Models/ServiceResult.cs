namespace Homework.Models
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }
    }


    public class ServiceResult<T> : ServiceResult
    {
        public T Result { get; set; }
    }
}
