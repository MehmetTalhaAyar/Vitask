using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IProjectDal : IGenericDal<Project>
    {

        List<Project> GetAllByUserId(int userId);


        Project GetByIdWithTasks(int id);

		public List<Project> GetAllWithCommander();
	}
}
