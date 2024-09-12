using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccessLayer.Abstract;
using Entities.Concrete;
using Vitask.Models;

namespace Business.Concrete
{
	public class AppUserManager : IAppUserService
	{
		private readonly IAppUserDal _appUserDal;

		public AppUserManager(IAppUserDal appUserDal)
		{
			_appUserDal = appUserDal;
		}

		public List<AppUser> GetAllUsers(int Page,int? exceptId = null)
		{
			return _appUserDal.GetAllUsers(Page,exceptId);
		}

		public AppUser GetById(int id)
		{
			return _appUserDal.GetById(id);
		}

		public List<AppUser> GetUsersByKeyword(string keyword, int? ProjectId)
		{
			return _appUserDal.GetUsersByKeyword(keyword,ProjectId);
		}

		public void Delete(int id)
		{
			_appUserDal.Delete(id);
		}

        public List<SelectListItemViewModel> SelectList(string keyword, int? ProjectId, List<int>? selectedUsers = null)
        {
            var selectList = GetUsersByKeyword(keyword,ProjectId).Select(x => new SelectListItemViewModel()
            {
                id = x.Id,
                text = x.UserName
            }).ToList();

			
            if (selectedUsers != null)
            {
				selectedUsers = selectedUsers.Distinct().ToList(); // aynı elemandan varsa alınmayacak
				var selectListIds = selectList.Select(x => x.id).ToList();
                var goingtoadds = selectedUsers.Where(x => !selectListIds.Contains(x));

                foreach (var item in goingtoadds)
                {
                    var user = GetById(item);
                    if (user != null)
                    {
                        SelectListItemViewModel selectListItemViewModel = new SelectListItemViewModel()
                        {
                            id = user.Id,
                            text = user.UserName
                        };
                        selectList.Add(selectListItemViewModel);
                    }
                }
            }
            return selectList;
        }

		public int GetPageCount(int? exceptId = null)
		{
			return _appUserDal.GetPageCount(exceptId);
		}

		public void Update(AppUser appUser)
		{
			_appUserDal.Update(appUser);
		}

		public AppUser GetByIdWithUserInfo(int id)
		{
			return _appUserDal.GetByIdWithUserInfo(id);
		}

        public AppUser GetByUsernameWithUserInfo(string username)
        {
			return _appUserDal.GetByUsernameWithUserInfo(username);
        }
    }
}
