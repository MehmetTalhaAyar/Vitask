using Task = Entities.Concrete.Task;

namespace DataAccessLayer.Abstract
{
    public interface ITaskDal : IGenericDal<Task>
    {
		int GetTaskCountForUser(int UserId);

		List<Task> GetAllByUserId(int UserId);
	}
}
