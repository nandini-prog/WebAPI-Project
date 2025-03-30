using AutoMapper;
using SmartTaskApi.DTO;
using SmartTaskApi.Models;
using Task = SmartTaskApi.Models.Task;
namespace SmartTaskApi.Mappings
{
    /// <summary>
    /// here we are going to define the mapping of the model to DTO
    /// means we are going to mention teh configuration.
    /// </summary>
    public class TaskProfile:Profile
    {
        public TaskProfile()
        {
            CreateMap<Task, TaskDto>();
        }
    }
}
