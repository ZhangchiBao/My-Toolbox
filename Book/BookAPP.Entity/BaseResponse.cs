namespace BookAPP.Entity
{
    public class BaseResponse<T>
    {
        public string Message { get; set; }

        public bool IsSuccess { get; set; } = false;

        public T Data { get; set; }
    }
}
