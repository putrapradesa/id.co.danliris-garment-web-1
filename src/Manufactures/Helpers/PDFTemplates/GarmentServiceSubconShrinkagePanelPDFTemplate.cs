﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using Manufactures.Dtos.GarmentSubcon.GarmentServiceSubconShrinkagePanels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Helpers.PDFTemplates
{
    public class GarmentServiceSubconShrinkagePanelPDFTemplate
    {
        public static MemoryStream Generate(GarmentServiceSubconShrinkagePanelDto garmentSubconShrinkagePanel)
        {
            Document document = new Document(PageSize.A5.Rotate(), 10, 10, 10, 10);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            PdfPCell cellLeftNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellCenterNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            PdfPCell cellCenterTopNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_TOP };
            PdfPCell cellRightNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT };
            PdfPCell cellJustifyNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_JUSTIFIED };
            PdfPCell cellJustifyAllNoBorder = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL };

            PdfPCell cellCenter = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            PdfPCell cellRight = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            PdfPCell cellLeft = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };


            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

            Paragraph title = new Paragraph("SUBCON BB SHRINKAGE / PANEL", bold_font);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 10f;
            document.Add(title);

            #region Header
            PdfPTable tableHeader = new PdfPTable(2);
            tableHeader.SetWidths(new float[] { 1f, 1f });

            PdfPCell cellHeaderContentLeft = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellHeaderContentLeft.AddElement(new Phrase("PT DAN LIRIS", normal_font));
            cellHeaderContentLeft.AddElement(new Phrase("SUKOHARJO", normal_font));
            cellHeaderContentLeft.AddElement(new Phrase("BANARAN, GROGOL", normal_font));
            tableHeader.AddCell(cellHeaderContentLeft);

            PdfPCell cellHeaderContentCenter = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellHeaderContentCenter.AddElement(new Paragraph("Tanggal Subcon    : " + garmentSubconShrinkagePanel.ServiceSubconShrinkagePanelDate.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID")), normal_font));
            cellHeaderContentCenter.AddElement(new Paragraph("No Subcon             : " + garmentSubconShrinkagePanel.ServiceSubconShrinkagePanelNo, normal_font));
            tableHeader.AddCell(cellHeaderContentCenter);

            PdfPCell cellHeader = new PdfPCell(tableHeader);
            tableHeader.ExtendLastRow = false;
            tableHeader.SpacingAfter = 15f;
            document.Add(tableHeader);
            #endregion

            List<GarmentSubconShrinkagePanelItemVM> itemData = new List<GarmentSubconShrinkagePanelItemVM>();

            foreach (var item in garmentSubconShrinkagePanel.Items)
            {

                foreach (var detail in item.Details)
                {
                    GarmentSubconShrinkagePanelItemVM garmentSubconShrinkagePanelItemVM = new GarmentSubconShrinkagePanelItemVM();
                    garmentSubconShrinkagePanelItemVM.UnitExpenditureNo = item.UnitExpenditureNo;
                    garmentSubconShrinkagePanelItemVM.ProductCode = detail.Product.Code;
                    garmentSubconShrinkagePanelItemVM.ProductName = detail.Product.Name;
                    garmentSubconShrinkagePanelItemVM.DesignColor = detail.DesignColor;
                    garmentSubconShrinkagePanelItemVM.Composition = detail.Composition;
                    garmentSubconShrinkagePanelItemVM.Quantity = detail.Quantity;
                    garmentSubconShrinkagePanelItemVM.UomUnit = detail.Uom.Unit;
                    itemData.Add(garmentSubconShrinkagePanelItemVM);
                }
            }

            #region content

            PdfPTable tableContent = new PdfPTable(7);
            List<float> widths = new List<float>();
            widths.Add(4f);
            widths.Add(4f);
            widths.Add(6f);
            widths.Add(4f);
            widths.Add(4f);
            widths.Add(3f);
            widths.Add(3f);

            tableContent.SetWidths(widths.ToArray());

            cellCenter.Phrase = new Phrase("No Bon Pengeluaran", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Kode Barang", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Nama Barang", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Design/Color", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Komposisi", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Jumlah", bold_font);
            cellCenter.Colspan = 1;
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("Satuan", bold_font);
            cellCenter.Rowspan = 2;
            tableContent.AddCell(cellCenter);

            decimal grandTotal = 0;
            foreach (var i in itemData)
            {
                cellCenter.Phrase = new Phrase(i.UnitExpenditureNo, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.ProductCode, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.ProductName, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.DesignColor, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Composition, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.Quantity.ToString(), normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                cellCenter.Phrase = new Phrase(i.UomUnit, normal_font);
                cellCenter.Rowspan = 1;
                tableContent.AddCell(cellCenter);
                grandTotal += i.Quantity;
            }

            cellRight.Phrase = new Phrase("TOTAL", bold_font);
            cellRight.Rowspan = 1;
            cellRight.Colspan = 5;
            tableContent.AddCell(cellRight);
            cellCenter.Phrase = new Phrase(grandTotal.ToString(), bold_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("", normal_font);
            cellCenter.Rowspan = 1;
            tableContent.AddCell(cellCenter);

            PdfPCell cellContent = new PdfPCell(tableContent); // dont remove
            tableContent.ExtendLastRow = false;
            tableContent.SpacingAfter = 20f;
            document.Add(tableContent);
            #endregion

            Paragraph remark = new Paragraph("Keterangan : ", normal_font);
            remark.SpacingAfter = 5f;
            document.Add(remark);
            #region TableSignature

            PdfPTable tableSignature = new PdfPTable(2);
            cellCenterTopNoBorder.Phrase = new Paragraph("Penerima\n\n\n\n\n\n\n\n(                                   )", normal_font);
            cellCenterTopNoBorder.HorizontalAlignment = Element.ALIGN_CENTER;
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("Bag. Gudang\n\n\n\n\n\n\n\n(                                   )", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("\n\n", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph($"Dicetak : {DateTimeOffset.Now.ToOffset(new TimeSpan(7, 0, 0)).ToString("dd MMMM yyyy / HH:mm:ss", new CultureInfo("id-ID"))}", normal_font);
            cellCenterTopNoBorder.HorizontalAlignment = Element.ALIGN_LEFT;
            tableSignature.AddCell(cellCenterTopNoBorder);
            cellCenterTopNoBorder.Phrase = new Paragraph("", normal_font);
            tableSignature.AddCell(cellCenterTopNoBorder);

            PdfPCell cellSignature = new PdfPCell(tableSignature);
            tableSignature.ExtendLastRow = false;
            document.Add(tableSignature);

            #endregion

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }

    public class GarmentSubconShrinkagePanelItemVM
    {
        public string UnitExpenditureNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string DesignColor { get; set; }
        public string Composition { get; set; }
        public decimal Quantity { get; set; }
        public string UomUnit { get; set; }
    }
}
