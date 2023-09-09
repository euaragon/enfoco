
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

        public async Task AddNoticeAsync(Notice notice)
        {
            if (notice != null)
            {
                _context.Notices.Add(notice);
                await _context.SaveChangesAsync();
            }
        }

        // Nuevo método para realizar la búsqueda de noticias por término.
        public IList<Notice> SearchNotices(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Notice>();
            }

            searchTerm = searchTerm.ToLower(); // Convertir el término de búsqueda a minúsculas.

            // Realiza la búsqueda en la base de datos, convirtiendo los valores a minúsculas antes de comparar.
            var searchResults = _context.Notices
                .Where(notice => notice.Title.ToLower().Contains(searchTerm) || notice.Text.ToLower().Contains(searchTerm))
                .ToList();

            return searchResults;
        }



    }


}

