using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IProjectUserDal : IGenericDal<ProjectUser>
    {

		void CreateProjectUserList(List<int> Ids, int ProjectId);

		List<int> GetUserIdByProject(int ProjectId);

		void UpdateProjectUserList(List<int> Ids, int ProjectId);

	}
}
