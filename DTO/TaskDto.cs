namespace SmartTaskApi.DTO
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
