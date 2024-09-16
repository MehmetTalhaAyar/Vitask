
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class ProjectDal : GenericRepository<Project>, IProjectDal
    {
        public List<Project> GetAllByUserId(int userId, int page)
        {
            using (VitaskContext context = new VitaskContext())
            {

                var projectIds = context.Projects.Include(x => x.Users).Select(x => x.Users.Where(y=> y.UserId == userId).FirstOrDefault()).ToList().Where(x=> x!= null).Select(x => x.ProjectId);

                var projects = context.Projects.Include(x=>x.Commander)
                    .Where(x => projectIds.Contains(x.Id)).Skip((page - 1) * 8).Take(8).ToList();
                return projects;
                    


            }
        }

		public Project GetByIdWithTasks(int id)
		{
			using(VitaskContext context = new VitaskContext())
            {
                return context.Projects.Include(x => x.Tasks)
                        .Where(x => x.Id == id).FirstOrDefault();
            }
		}

		public List<Project> GetAllWithCommander(int page)
		{
			using(VitaskContext context = new VitaskContext())
            {
                return context.Projects.Include(x => x.Commander).Skip((page-1) * 8).Take(8).ToList();



            }
		}

        public int GetPageCount()
        {
            using(VitaskContext context = new VitaskContext())
            {
                double pageCount = (double)context.Projects.Count() / 8;
                return (int)Math.Ceiling(pageCount);
            }
        }

		public int GetPageCount(int userId)
		{
            using(VitaskContext context = new VitaskContext())
            {
                double pagecount = (double)context.Projects.Include(x => x.Users).Where(x=> x.Users.Any(y=> y.UserId == userId)).Count() / 8;
                return (int)Math.Ceiling(pagecount);
            }
		}
        

		public Project GetByIdWithRelations(int id)
		{
            using (VitaskContext context = new VitaskContext())
            {
                return context.Projects.Include(x=> x.Users).Where(x=> x.Id == id).FirstOrDefault();  
            }
		}
	}
}
