using Business.Abstract;
using DataAccessLayer.Abstract;
using Task = Entities.Concrete.Task;

namespace Business.Concrete
{
    public class TaskManager : ITaskService
    {
        private readonly ITaskDal _taskDal;
        public TaskManager(ITaskDal taskDal)
        {
            _taskDal = taskDal;
        }
        public void Delete(Task t)
        {
            _taskDal.Delete(t);
        }

        public List<Task> GetAll()
        {
            return _taskDal.GetAll();
        }

		public List<Task> GetAllByProjectId(int ProjectId,int Page)
		{
			return _taskDal.GetAllByProjectId(ProjectId,Page);
		}

		public List<Task> GetAllByUserId(int UserId)
		{
			return _taskDal.GetAllByUserId(UserId);
		}

		public List<Task> GetAllByResponsibleId(int UserId, int page)
		{
			return _taskDal.GetAllByResponsibleId(UserId, page);
		}

		public Task GetById(int id)
        {
            return _taskDal.GetById(id);
        }

		public int GetPageCount(int ProjectId)
		{
            return _taskDal.GetPageCount(ProjectId);
		}

		public int GetPageCountByUserId(int UserId)
		{
			return _taskDal.GetPageCountByUserId(UserId);
		}

		public int GetTaskCountForUser(int UserId)
		{
            return _taskDal.GetTaskCountForUser(UserId);
		}

		public Task Insert(Task t)
        {
            return _taskDal.Insert(t);
        }

        public void Update(Task t)
        {
            _taskDal.Update(t);
        }

        public Task GetTaskWithRelations(int id)
        {
            return _taskDal.GetTaskWithRelations(id);
        }
    }
}
