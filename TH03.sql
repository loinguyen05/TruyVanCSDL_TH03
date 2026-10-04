
USE QLBanSach;
GO

SELECT Mcd, Ten_chu_de
FROM CHU_DE;


SELECT
    cd.Mcd,
    cd.Ten_chu_de,
    COUNT(s.Ms) AS So_luong_sach
FROM CHU_DE cd
LEFT JOIN SACH s ON cd.Mcd = s.Mcd
GROUP BY cd.Mcd, cd.Ten_chu_de;


SELECT DISTINCT
    cd.Mcd,
    cd.Ten_chu_de
FROM CHU_DE cd
INNER JOIN SACH s ON cd.Mcd = s.Mcd;

DECLARE @MaChuDe INT = 1;

SELECT
    Ms,
    Ten_sach,
    Don_gia,
    Hinh_minh_hoa
FROM SACH
WHERE Mcd = @MaChuDe;


SELECT TOP 5
    Ms,
    Ten_sach,
    Hinh_minh_hoa
FROM SACH
ORDER BY Ngay_cap_nhat DESC;


SELECT TOP 5
    s.Ms,
    s.Ten_sach,
    s.Hinh_minh_hoa,
    SUM(ct.So_luong) AS Tong_so_luong_ban
FROM SACH s
INNER JOIN CT_DAT_HANG ct ON s.Ms = ct.Ms
GROUP BY
    s.Ms,
    s.Ten_sach,
    s.Hinh_minh_hoa
ORDER BY Tong_so_luong_ban DESC;

SELECT
    STT,
    TenCty,
    Hinh_Minh_Hoa,
    HREF,
    Ngay_bat_dau,
    Ngay_het_han
FROM QUANG_CAO
WHERE Ngay_bat_dau <= GETDATE()
  AND Ngay_het_han >= GETDATE();

SELECT
    tg.Mtg,
    tg.Ten_tac_gia,
    tg.Dia_chi,
    tg.Dien_thoai,
    tg2.Vai_tro
FROM TAC_GIA tg
INNER JOIN THAM_GIA tg2 ON tg.Mtg = tg2.Mtg
WHERE tg2.Ms = 2;

SELECT
    Sdh,
    Mkh,
    Ngay_dat_hang,
    Tri_gia,
    Ngay_giao_hang
FROM DON_DAT_HANG
WHERE Da_giao_hang = 1;