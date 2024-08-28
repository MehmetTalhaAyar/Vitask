using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Task = Entities.Concrete.Task;

namespace DataAccessLayer.Concrete
{
	public class TaskDal : GenericRepository<Task>, ITaskDal
	{
		public int GetTaskCountForUser(int UserId)
		{
			using(VitaskContext context = new VitaskContext())
			{
				return context.Tasks.Where(x => x.ResponsibleId == UserId).Count();
			}
		}
	}
}
