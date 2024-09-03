using Task = Entities.Concrete.Task;

namespace Business.Abstract
{
    public interface ITaskService : IGenericService<Task>
    {

		int GetTaskCountForUser(int UserId);

		List<Task> GetAllByUserId(int UserId);

		List<Task> GetAllByProjectId(int ProjectId,int Page); // burada taskların en önce süresinin dolacağı düşünülerek geliyor

		int GetPageCount(int ProjectId);
	}
}
