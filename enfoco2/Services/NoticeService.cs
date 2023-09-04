
using System.Diagnostics;
using enfoco2.Data;
using enfoco2.Models;

namespace enfoco2.Services
{
	public class NoticeService
	{
        private readonly EnfocoDb _context = default!;
        public NoticeService(EnfocoDb context)
		{
            _context = context;

        }

        public IList<Notice> GetNotice()
        {
            if (_context.Notices != null)
            {
                var notices = _context.Notices.ToList();
                Debug.WriteLine($"Cuantas noticias hay: {notices.Count}");
                return notices;
            }


            return new List<Notice>();
        }

        public async Task<Notice?> GetNoticeByIdAsync(int id)
        {
            var notice = await _context.Notices.FindAsync(id);
            return notice;
        }


    }


}

