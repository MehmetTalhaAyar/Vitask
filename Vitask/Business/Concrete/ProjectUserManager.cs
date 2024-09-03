
using Business.Abstract;
using DataAccessLayer.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class ProjectUserManager : IProjectUserService
    {
        private readonly IProjectUserDal _projectUserDal;

        public ProjectUserManager(IProjectUserDal projectUserDal)
        {
            _projectUserDal = projectUserDal;
        }

		public void CreateProjectUserList(List<int> Ids, int ProjectId)
		{
			_projectUserDal.CreateProjectUserList(Ids, ProjectId);
		}

		public void Delete(ProjectUser t)
        {
            _projectUserDal.Delete(t);
        }

        public List<ProjectUser> GetAll()
        {
            return _projectUserDal.GetAll();
        }

        public ProjectUser GetById(int id)
        {
            return _projectUserDal.GetById(id);
        }

        public ProjectUser Insert(ProjectUser t)
        {
            return _projectUserDal.Insert(t);
        }

        public void Update(ProjectUser t)
        {
            _projectUserDal.Update(t);
        }
    }
}
