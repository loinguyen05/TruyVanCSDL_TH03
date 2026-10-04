<Query Kind="Statements">
  <Connection>
    <ID>30b38344-3e55-4795-ae16-4337850f3234</ID>
    <NamingServiceVersion>3</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.\SQLEXPRESS</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <UseMicrosoftDataSqlClient>true</UseMicrosoftDataSqlClient>
    <EncryptTraffic>true</EncryptTraffic>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>QLBanSach</Database>
    <MapXmlToString>false</MapXmlToString>
    <DriverData>
      <SkipCertificateCheck>true</SkipCertificateCheck>
    </DriverData>
  </Connection>
</Query>

var dsChuDe = from cd in CHU_DEs
              orderby cd.Ten_chu_de
              select new { cd.Mcd, cd.Ten_chu_de };
dsChuDe.Dump("1. Danh mục chủ đề");


var ds2 = from cd in CHU_DEs
          join s in SACHes on cd.Mcd equals s.Mcd into g
          select new {
              cd.Mcd,
              cd.Ten_chu_de,
              SoLuongSach = g.Count()
          };
ds2.Dump("2. Chủ đề + số lượng sách");


var ds3 = (from cd in CHU_DEs
           join s in SACHes on cd.Mcd equals s.Mcd
           select new { cd.Mcd, cd.Ten_chu_de }).Distinct()
          .OrderBy(x => x.Ten_chu_de);
ds3.Dump("3. Chủ đề có sách");

int maChuDe = 5;  

var ds4 = from s in SACHes
          where s.Mcd == maChuDe
          orderby s.Ten_sach
          select new {
              s.Ms,
              s.Ten_sach,
              s.Don_gia,
              s.Hinh_minh_hoa,
              s.Ngay_cap_nhat
          };
ds4.Dump("4. Sách theo chủ đề");


var ds5 = (from s in SACHes
           orderby s.Ngay_cap_nhat descending
           select new {
               s.Ms,
               s.Ten_sach,
               s.Hinh_minh_hoa,
               s.Ngay_cap_nhat
           }).Take(5);
ds5.Dump("5. Top 5 sách mới");


var ds6 = (from s in SACHes
           join ct in CT_DAT_HANGs on s.Ms equals ct.Ms
           group ct by new { s.Ms, s.Ten_sach, s.Hinh_minh_hoa } into g
           orderby g.Sum(x => x.So_luong) descending
           select new {
               g.Key.Ms,
               g.Key.Ten_sach,
               g.Key.Hinh_minh_hoa,
               TongSoLuongBan = g.Sum(x => x.So_luong)
           }).Take(5);
ds6.Dump("6. Top 5 sách bán chạy");


var ds7 = from qc in QUANG_CAOs
          where qc.Ngay_het_han >= DateTime.Today
          orderby qc.Ngay_het_han
          select new {
              qc.STT,
              qc.TenCTy,
              qc.Hinh_Minh_Hoa,
              qc.HREF,
qc.Ngay_bat_dau,
              qc.Ngay_het_han
          };
ds7.Dump("7. Quảng cáo còn hạn");


var ds8 = from tg in TAC_GIAs
          join t in THAM_GIAs on tg.Mtg equals t.Mtg
          where t.Ms == 2
          select new {
              tg.Mtg,
              tg.Ten_tac_gia
          };
ds8.Dump("8. Tác giả của sách mã 2");


var ds9 = from dh in DON_DAT_HANGs
          join kh in KHACH_HANGs on dh.Mkh equals kh.Mkh
          where dh.Da_giao_hang == true
          orderby dh.Ngay_giao_hang descending
          select new {
              dh.Sdh,
              dh.Ngay_dat_hang,
              dh.Ngay_giao_hang,
              dh.Tri_gia,
              dh.Da_giao_hang,
              kh.Ho_ten,
              kh.Dien_thoai
          };
ds9.Dump("9. Đơn hàng đã giao");