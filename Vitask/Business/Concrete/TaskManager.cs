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

        public Task GetById(int id)
        {
            return _taskDal.GetById(id);
        }

		public int GetTaskCountForUser(int UserId)
		{
            return _taskDal.GetTaskCountForUser(UserId);
		}

		public void Insert(Task t)
        {
            _taskDal.Insert(t);
        }

        public void Update(Task t)
        {
            _taskDal.Update(t);
        }
    }
}
