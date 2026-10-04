using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TH03_BookList.Models;
using System.Reflection;

namespace TH03_BookList.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult QueryDemo(int? id)
        {
            var queries = new List<SachQuery>
            {
                new SachQuery { Id = 1, QueryName = "1. Lấy tất cả danh mục chủ đề" },
                new SachQuery { Id = 2, QueryName = "2. Lấy danh mục chủ đề có sách" },
                new SachQuery { Id = 3, QueryName = "3. Lấy sách theo chủ đề (Mcd = 5)" },
                new SachQuery { Id = 4, QueryName = "4. Top 5 sách mới" },
                new SachQuery { Id = 5, QueryName = "5. Top 5 sách bán chạy" },
                new SachQuery { Id = 6, QueryName = "6. Quảng cáo còn hạn" },
                new SachQuery { Id = 7, QueryName = "7. Tác giả của sách mã 2" },
                new SachQuery { Id = 8, QueryName = "8. Đơn hàng đã giao" }
            };

            ViewBag.Queries = new SelectList(queries, "Id", "QueryName", id);

            return id switch
            {
                1 => View(GetAllChuDe()),
                2 => View(GetChuDeCoSach()),
                3 => View(GetSachTheoChuDe(5)),
                4 => View(GetTop5SachMoi()),
                5 => View(GetTop5SachBanChay()),
                6 => View(GetQuangCaoConHan()),
                7 => View(GetTacGiaSach2()),
                8 => View(GetDonHangDaGiao()),
                _ => View(null)
            };
        }

        // ========== Helper chuyển sang Dictionary ==========
        private List<Dictionary<string, object>> ToDict<T>(IEnumerable<T> data)
        {
            return data.Select(item => item!.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(
                    p => p.Name,
                    p => p.GetValue(item) ?? "NULL"
                )).ToList();
        }

        // 1. Tất cả chủ đề
        private List<Dictionary<string, object>> GetAllChuDe()
        {
            var q = _context.ChuDes
                .OrderBy(c => c.TenChuDe)
                .Select(c => new { c.Mcd, c.TenChuDe });
            return ToDict(q);
        }

        // 2. Chủ đề có sách
        private List<Dictionary<string, object>> GetChuDeCoSach()
        {
            var q = _context.ChuDes
                .Where(c => c.Saches.Any())
                .Select(c => new {
                    c.Mcd,
                    c.TenChuDe,
                    SoLuongSach = c.Saches.Count()
                })
                .OrderBy(x => x.TenChuDe);
            return ToDict(q);
        }

        // 3. Sách theo chủ đề
        private List<Dictionary<string, object>> GetSachTheoChuDe(int mcd)
        {
            var q = _context.Saches
                .Where(s => s.Mcd == mcd)
                .OrderBy(s => s.TenSach)
                .Select(s => new {
                    s.Ms,
                    s.TenSach,
                    s.DonGia,
                    s.HinhMinhHoa,   // ← nếu lỗi thì đổi thành tên thực tế trong Sach.cs
                    s.NgayCapNhat
                });
            return ToDict(q);
        }

        // 4. Top 5 sách mới
        private List<Dictionary<string, object>> GetTop5SachMoi()
        {
            var q = _context.Saches
                .OrderByDescending(s => s.NgayCapNhat)
                .Take(5)
                .Select(s => new {
                    s.Ms,
                    s.TenSach,
                    s.HinhMinhHoa,
                    s.NgayCapNhat
                });
            return ToDict(q);
        }

        // 5. Top 5 sách bán chạy
        private List<Dictionary<string, object>> GetTop5SachBanChay()
        {
            var q = _context.Saches
                .Select(s => new {
                    s.Ms,
                    s.TenSach,
                    s.HinhMinhHoa,
                    TongBan = s.CtDatHangs.Sum(ct => (int?)ct.SoLuong) ?? 0
                })
                .OrderByDescending(x => x.TongBan)
                .Take(5);
            return ToDict(q);
        }

        // 6. Quảng cáo còn hạn
        private List<Dictionary<string, object>> GetQuangCaoConHan()
        {
            var q = _context.QuangCaos
                .Where(qc => qc.NgayHetHan >= DateTime.Today)
                .OrderBy(qc => qc.NgayHetHan)
                .Select(qc => new {
                    qc.Stt,
                    qc.TenCty,
                    qc.HinhMinhHoa,
                    qc.Href,
                    qc.NgayBatDau,
                    qc.NgayHetHan
                });
            return ToDict(q);
        }

        // 7. Tác giả sách mã 2
        private List<Dictionary<string, object>> GetTacGiaSach2()
        {
            var q = from tg in _context.TacGia
                    join t in _context.ThamGia on tg.Mtg equals t.Mtg
                    where t.Ms == 2
                    select new { tg.Mtg, tg.TenTacGia };
            return ToDict(q);
        }

        // 8. Đơn hàng đã giao
        private List<Dictionary<string, object>> GetDonHangDaGiao()
        {
            var q = from dh in _context.DonDatHangs
                    join kh in _context.KhachHangs on dh.Mkh equals kh.Mkh
                    where dh.DaGiaoHang == true
                    orderby dh.NgayGiaoHang descending
                    select new
                    {
                        dh.Sdh,
                        dh.NgayDatHang,
                        dh.NgayGiaoHang,
                        dh.TriGia,
                        dh.DaGiaoHang,
                        kh.HoTen,
                        kh.DienThoai
                    };
            return ToDict(q);
        }
    }
}