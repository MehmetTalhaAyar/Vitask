using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using Task = Entities.Concrete.Task;

namespace DataAccessLayer.Concrete
{
    public class TaskDal : GenericRepository<Task>,ITaskDal
    {
    }
}
