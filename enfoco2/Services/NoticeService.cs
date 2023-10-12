using System.Diagnostics;
using enfoco2.Data;
using enfoco2.Models;

namespace enfoco2.Services
{
    public class NoticeService
    {
        private readonly EnfocoDb _context;

        public NoticeService(EnfocoDb context)
        {
            _context = context;
        }

        public IList<Notice> GetNotice()
        {
            return _context.Notices.ToList();
        }

        public IList<Newsletter> GetNewsletter()
        {
            return _context.Newsletter.ToList();
        }

        public async Task<Notice?> GetNoticeByIdAsync(int id)
        {
            var notice = await _context.Notices.FindAsync(id);
            return notice;
        }

        public async Task AddNoticeAsync(Notice notice)
        {
            if (notice != null)
            {
                _context.Notices.Add(notice);
                await _context.SaveChangesAsync();
            }
        }

        public IList<Notice> SearchNotices(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Notice>();
            }

            searchTerm = searchTerm.ToLower();
            var searchResults = _context.Notices
                .Where(notice => notice.Title.ToLower().Contains(searchTerm) || notice.Text.ToLower().Contains(searchTerm))
                .ToList();

            return searchResults;
        }

        public async Task DeleteNoticeAsync(int id)
        {
            var notice = await _context.Notices.FindAsync(id);
            if (notice != null)
            {
                _context.Notices.Remove(notice);
                await _context.SaveChangesAsync();
            }
        }


    }
}
