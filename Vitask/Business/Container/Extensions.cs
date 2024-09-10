using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Task = System.Threading.Tasks.Task;

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

			Services.AddScoped<ILikeDal, LikeDal>();
			Services.AddScoped<ILikeService, LikeManager>();

			Services.AddScoped<ICommentDal, CommentDal>();
			Services.AddScoped<ICommentService, CommentManager>();

			Services.AddScoped<IUserInfoDal, UserInfoDal>();
			Services.AddScoped<IUserInfoService, UserInfoManager>();

			Services.AddScoped<IFileService, FileManager>();
		}


        public static async Task SeedRoles(IServiceProvider ServiceProvider)
        {
			var userManager = ServiceProvider.GetRequiredService<UserManager<AppUser>>();
			var roleManager = ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

			string[] roleNames = { "Admin", "User" };
			IdentityResult roleResult;

			foreach (var roleName in roleNames)
			{
				var roleExist = await roleManager.RoleExistsAsync(roleName);
				if (!roleExist)
				{
					roleResult = await roleManager.CreateAsync(new AppRole() { Name = roleName});
				}
			}

			// İlk Admin Kullanıcıyı Oluşturma
			var adminUser = new AppUser
			{
				UserName = "Kral_Kaan",
				Email = "firatcikar@hotmail.com",
				Name = "Fırat",
				Surname = "Çıkar",
				
			};

			var user = await userManager.FindByEmailAsync("firatcikar@hotmail.com");


			if (user == null)
			{

				var createAdmin = await userManager.CreateAsync(adminUser, "Aa123456+"); // buradaki password korunmalı bir yerden gelmeli 
				if (createAdmin.Succeeded)
				{
					await userManager.AddToRoleAsync(adminUser, "Admin");
				}

				
			}

			
		}
    }
}
