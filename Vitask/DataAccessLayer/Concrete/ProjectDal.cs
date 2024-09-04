
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class ProjectDal : GenericRepository<Project>, IProjectDal
    {
        public List<Project> GetAllByUserId(int userId)
        {
            using (VitaskContext context = new VitaskContext())
            {

                var projectIds = context.Projects.Include(x => x.Users).Select(x => x.Users.Where(y=> y.UserId == userId).FirstOrDefault()).ToList().Where(x=> x!= null).Select(x => x.ProjectId);


              
                


                var projects = context.Projects.Where(x => projectIds.Contains(x.Id)).ToList();
                return projects;
                    


            }
        }

		


	}
}
