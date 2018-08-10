using CotizadorRojoBetabel.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizadorRojoBetabel.Controllers
{
    static class DocumentsCreator
    {
        private static Font bold = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD);
        private static Font boldTitle = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
        private static Font Title = new Font(Font.FontFamily.HELVETICA, 12);
        private static Font regular = new Font(Font.FontFamily.HELVETICA, 10);
        private static Font footerFont = new Font(Font.FontFamily.HELVETICA, 7);
        private static Font boldFooterFont = new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD);

        internal static bool ProductsList(ObservableCollection<Products> products, string savePath)
        {
            try
            {
                //create document's page size and margins
                Document doc = new Document(PageSize.LETTER, 10, 10, 40, 10);

                //create the pdf file
                var stream = new FileStream(savePath, FileMode.Create);
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);

                //open the file to start write the content
                doc.Open();

                //Add the Images that will be displayed and set the position where they will be in the document
                Image logo = Image.GetInstance(Directory.GetCurrentDirectory() + @"\Resources\LogoSmall.png");
                logo.SetAbsolutePosition(20f, 680f);

                //Create a table, someway like HTML almost all the components will be inside a table to manage their margins and padding
                //I am creating a Table with 8 columns
                var table = new PdfPTable(8)
                {
                    WidthPercentage = 90f
                };

                //Create the item that will be inside the cell of the table
                var CompanyPhrase = new Phrase("NUTRICIÓN ROJO BETABEL", boldTitle);
                //Create the cell that will be storing the item created
                var CompanyCell = new PdfPCell(CompanyPhrase)
                {
                    Colspan = 8,    //The table has 6 columns but I want to use the total width of the table
                    HorizontalAlignment = 1, // 0 = Left Alignment,  1 = Center Alignment,  2 = Right Alignment
                    Border = Rectangle.NO_BORDER
                };

                var BossPhrase = new Phrase("L.N.G. María Martha Heiras Ley", bold);
                var BossCell = new PdfPCell(BossPhrase)
                {
                    Colspan = 8,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };

                var CellPhrase = new Phrase("Cel. 3316064972", bold);
                var CellCell = new PdfPCell(CellPhrase)
                {
                    Colspan = 8,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };

                var EmailPhrase = new Phrase("E-Mail nutricion.rojobetabel@gmail.com", bold);
                var EmailCell = new PdfPCell(EmailPhrase)
                {
                    PaddingBottom = 10f,
                    Colspan = 8,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };

                var TitlePhrase = new Phrase("LISTA DE PRODUCTOS", Title);
                var TitleCell = new PdfPCell(TitlePhrase)
                {
                    PaddingBottom = 10f,
                    Colspan = 8,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };

                var NameHeaderPhrase = new Phrase("PRODUCTO", bold);
                var NameHeaderCell = new PdfPCell(NameHeaderPhrase)
                {
                    HorizontalAlignment = 1
                };
                var CategoryHeaderPhrase = new Phrase("CATEGORÍA", bold);
                var CategoryHeaderCell = new PdfPCell(CategoryHeaderPhrase)
                {
                    HorizontalAlignment = 1
                };
                var PriceHeaderPhrase = new Phrase("PRECIO", bold);
                var PriceHeaderCell = new PdfPCell(PriceHeaderPhrase)
                {
                    HorizontalAlignment = 1
                };
                var UnitHeaderPhrase = new Phrase("UNIDAD", bold);
                var UnitHeaderCell = new PdfPCell(UnitHeaderPhrase)
                {
                    HorizontalAlignment = 1
                };
                var WeightHeaderPhrase = new Phrase("PSO-CANT.", bold);
                var WeightHeaderCell = new PdfPCell(WeightHeaderPhrase)
                {
                    HorizontalAlignment = 1
                };
                var WasteHeaderPhrase = new Phrase("%MERMA", bold);
                var WasteHeaderCell = new PdfPCell(WasteHeaderPhrase)
                {
                    HorizontalAlignment = 1
                };
                var YieldHeaderPhrase = new Phrase("%REND.", bold);
                var YieldHeaderCell = new PdfPCell(YieldHeaderPhrase)
                {
                    HorizontalAlignment = 1
                };
                var CostHeaderPhrase = new Phrase("COSTO", bold);
                var CostHeaderCell = new PdfPCell(CostHeaderPhrase)
                {
                    HorizontalAlignment = 1
                };

                //All the cells that were created, need to be added to the table
                table.AddCell(CompanyCell);

                table.AddCell(BossCell);

                table.AddCell(CellCell);

                table.AddCell(EmailCell);

                table.AddCell(TitleCell);

                table.AddCell(NameHeaderCell);
                table.AddCell(CategoryHeaderCell);
                table.AddCell(PriceHeaderCell);
                table.AddCell(UnitHeaderCell);
                table.AddCell(WeightHeaderCell);
                table.AddCell(WasteHeaderCell);
                table.AddCell(YieldHeaderCell);
                table.AddCell(CostHeaderCell);

                foreach (var product in products)
                {
                    var NamePhrase = new Phrase(product.Name, regular);
                    var NameCell = new PdfPCell(NamePhrase)
                    {
                        HorizontalAlignment = 0
                    };
                    var CategoryPhrase = new Phrase(product.Category.ToString(), regular);
                    var CategoryCell = new PdfPCell(CategoryPhrase)
                    {
                        HorizontalAlignment = 0
                    };
                    var PricePhrase = new Phrase(product.Price.ToString("C"), regular);
                    var PriceCell = new PdfPCell(PricePhrase)
                    {
                        HorizontalAlignment = 0
                    };
                    var UnitPhrase = new Phrase(product.Unit.ToString());
                    var UnitCell = new PdfPCell(UnitPhrase)
                    {
                        HorizontalAlignment = 0
                    };
                    var WeightPhrase = new Phrase(product.Weight.ToString(), regular);
                    var WeightCell = new PdfPCell(WeightPhrase)
                    {
                        HorizontalAlignment = 0
                    };
                    var waste = product.Waste / 100;
                    var WastePhrase = new Phrase(waste.ToString("P"), regular);
                    var WasteCell = new PdfPCell(WastePhrase)
                    {
                        HorizontalAlignment = 0
                    };
                    var yield = product.Yield / 100;
                    var YieldPhrase = new Phrase(yield.ToString("P"), regular);
                    var YieldCell = new PdfPCell(YieldPhrase)
                    {
                        HorizontalAlignment = 0
                    };
                    var CostPhrase = new Phrase(product.Cost.ToString("C"), regular);
                    var CostCell = new PdfPCell(CostPhrase)
                    {
                        HorizontalAlignment = 0
                    };

                    table.AddCell(NameCell);
                    table.AddCell(CategoryCell);
                    table.AddCell(PriceCell);
                    table.AddCell(UnitCell);
                    table.AddCell(WeightCell);
                    table.AddCell(WasteCell);
                    table.AddCell(YieldCell);
                    table.AddCell(CostCell);
                }

                //Add the Images
                doc.Add(logo);

                //Add the Paragraph to the document
                doc.Add(table);

                //Close the document when all the elements were added
                doc.Close();

                stream.Dispose();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
