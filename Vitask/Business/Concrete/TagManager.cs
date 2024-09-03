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
    public class TagManager : ITagService
    {
        private readonly ITagDal _tagDal;
        public TagManager(ITagDal tagDal)
        {
            _tagDal = tagDal;
        }

        public void Delete(Tag t)
        {
            _tagDal.Delete(t);
        }

        public List<Tag> GetAll()
        {
            return _tagDal.GetAll();
        }

        public Tag GetById(int id)
        {
            return _tagDal.GetById(id);
        }

        public Tag Insert(Tag t)
        {
            return _tagDal.Insert(t);
        }

        public void Update(Tag t)
        {
            _tagDal.Update(t);
        }
    }
}
