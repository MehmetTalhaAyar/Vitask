using Task = Entities.Concrete.Task;

namespace Business.Abstract
{
    public interface ITaskService : IGenericService<Task>
    {

		int GetTaskCountForUser(int UserId);

		List<Task> GetAllByUserId(int UserId);
	}
}
