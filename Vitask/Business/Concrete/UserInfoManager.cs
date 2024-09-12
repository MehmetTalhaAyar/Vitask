using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccessLayer.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
	public class UserInfoManager : IUserInfoService
	{
		public readonly IUserInfoDal _userInfoDal;

		public UserInfoManager(IUserInfoDal userInfoDal)
		{
			_userInfoDal = userInfoDal;
		}

		public void Delete(UserInfo t)
		{
			_userInfoDal.Delete(t);
		}

		public List<UserInfo> GetAll()
		{
			return _userInfoDal.GetAll();
		}

		public UserInfo GetById(int id)
		{
			return _userInfoDal.GetById(id);
		}

		public UserInfo Insert(UserInfo t)
		{
			return _userInfoDal.Insert(t);
		}

		public void Update(UserInfo t)
		{
			_userInfoDal.Update(t);
		}
	}
}
