
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
                return _context.Notices.ToList();
            }


            return new List<Notice>();
        }

        public async Task<Notice> GetNoticeByIdAsync(int id)
        {
            return await _context.Notices.FindAsync(id).AsTask();
        }


    }


}

