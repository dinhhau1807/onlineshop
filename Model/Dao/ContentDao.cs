using Common;
using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model.Dao
{
    public class ContentDao
    {
        OnlineShopDbContext _context = null;

        public ContentDao()
        {
            _context = new OnlineShopDbContext();
        }

        public Content GetByID(long id)
        {
            return _context.Contents.Find(id);
        }

        public Tag GetTag(string id)
        {
            return _context.Tags.Find(id);
        }

        public long Create(Content content)
        {
            // Xu ly alias
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }

            content.CreatedDate = DateTime.Now;
            content.ViewCount = 0;

            _context.Contents.Add(content);
            _context.SaveChanges();

            // Xu ly tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);

                    // Insert to Tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    // Insert to ContentTag table
                    this.InsertContentTag(content.ID, tagId);
                }
            }

            return content.ID;
        }

        public long Edit(Content content)
        {
            // Xu ly alias
            if (string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }

            content.ModifiedDate = DateTime.Now;
            _context.SaveChanges();

            // Xu ly tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                this.RemoveAllContentTag(content.ID);
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existedTag = this.CheckTag(tagId);

                    // Insert to Tag table
                    if (!existedTag)
                    {
                        this.InsertTag(tagId, tag);
                    }

                    // Insert to ContentTag table
                    this.InsertContentTag(content.ID, tagId);
                }
            }

            return content.ID;
        }

        public void RemoveAllContentTag(long contentId)
        {
            _context.ContentTags.RemoveRange(_context.ContentTags.Where(x => x.ContentID == contentId));
            _context.SaveChanges();
        }

        public void InsertTag(string id, string name)
        {
            var tag = new Tag
            {
                ID = id,
                Name = name
            };

            _context.Tags.Add(tag);
            _context.SaveChanges();
        }


        public void InsertContentTag(long contentId, string tagId)
        {
            var contentTag = new ContentTag
            {
                ContentID = contentId,
                TagID = tagId
            };

            _context.ContentTags.Add(contentTag);
            _context.SaveChanges();
        }

        public bool CheckTag(string id)
        {
            return _context.Tags.Count(x => x.ID == id) > 0;
        }

        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> model = _context.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// List all content for client
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Content> ListAllPaging(int page, int pageSize)
        {
            IQueryable<Content> model = _context.Contents;
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<Content> ListAllByTag(string tag, int page, int pageSize)
        {
            var model = (from a in _context.Contents
                         join b in _context.ContentTags
                         on a.ID equals b.ContentID
                         where b.TagID == tag
                         select new
                         {
                             ID = a.ID,
                             Name = a.Name,
                             MetatTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreatedDate = a.CreatedDate,
                             CreatedBy = a.CreatedBy
                         }).AsEnumerable().Select(x => new Content
                         {
                             ID = x.ID,
                             Name = x.Name,
                             MetaTitle = x.MetatTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreatedDate = x.CreatedDate,
                             CreatedBy = x.CreatedBy
                         });

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in _context.Tags
                         join b in _context.ContentTags
                         on a.ID equals b.TagID
                         where b.ContentID == contentId
                         select new
                         {
                             ID = b.TagID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tag
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }
    }
}
