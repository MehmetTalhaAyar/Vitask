using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Container
{
    public static class Extensions
    {
        public static void ContainerDependencies(this IServiceCollection Services)
        {

            Services.AddScoped<IProjectDal, ProjectDal>();
            Services.AddScoped<IProjectService, ProjectManager>();

            Services.AddScoped<ITagDal, TagDal>();
            Services.AddScoped<ITagService, TagManager>();

            Services.AddScoped<ITaskDal, TaskDal>();
            Services.AddScoped<ITaskService, TaskManager>();

            Services.AddScoped<IProjectUserDal, ProjectUserDal>();
            Services.AddScoped<IProjectUserService, ProjectUserManager>();

			Services.AddScoped<IAppUserDal, AppUserDal>();
			Services.AddScoped<IAppUserService, AppUserManager>();
		}
    }
}
