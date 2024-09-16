using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IProjectService : IGenericService<Project>
    {

        List<Project> GetAllByUserId(int userId,int page);

		Project GetByIdWithTasks(int id);

		public List<Project> GetAllWithCommander(int page);

		public int GetPageCount(int userId);

		public Project GetByIdWithRelations(int id);

		public int GetPageCount();
	}
}
