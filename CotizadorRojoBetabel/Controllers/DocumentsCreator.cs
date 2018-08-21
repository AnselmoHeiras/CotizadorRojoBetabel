using CotizadorRojoBetabel.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LibreR.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CotizadorRojoBetabel.Controllers
{
    static class DocumentsCreator
    {
        private static readonly Font bold = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD);
        private static readonly Font boldTitle = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
        private static readonly Font Title = new Font(Font.FontFamily.HELVETICA, 12);
        private static readonly Font regular = new Font(Font.FontFamily.HELVETICA, 10);
        private static readonly Font footerFont = new Font(Font.FontFamily.HELVETICA, 7);
        private static readonly Font boldFooterFont = new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD);
        private static readonly BaseColor PinkHeaderForeground = new BaseColor(252, 135, 174);
        private static readonly BaseColor PinkRowForeground = new BaseColor(255, 212, 244);
        private static readonly BaseColor GreenHeaderForeground = new BaseColor(92, 184, 92);
        private static readonly BaseColor GreenRowForeground = new BaseColor(169, 251, 169);
        private static readonly BaseColor BlueHeaderForeground = new BaseColor(146, 153, 248);
        private static readonly BaseColor PurpleHeaderForeground = new BaseColor(194, 146, 248);
        private static readonly string _tempImageFile = Directory.GetCurrentDirectory() + @"\temp\Image.bmp";
        private static decimal _totalCost;        

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

        internal static bool Dish(Dishes dish, string savePath)
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

                var EmptyPhrase = new Phrase(" ", regular);
                var EmptyCol = new PdfPCell(EmptyPhrase)
                {
                    Colspan = 8,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };
                var EmptyCell = new PdfPCell(EmptyPhrase)
                {
                    HorizontalAlignment = 1,
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
                    PaddingBottom = 25f,
                    Colspan = 8,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };

                var TitlePhrase = new Phrase(dish.Name.ToUpper(), boldTitle);
                var TitleCell = new PdfPCell(TitlePhrase)
                {
                    PaddingBottom = 10f,
                    Colspan = 8,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };

                var LineHeaderPhrase = new Phrase("Línea", bold);
                var LineHeaderCell = new PdfPCell(LineHeaderPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 3,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 0,
                    BackgroundColor = PinkHeaderForeground
                };

                var PortionsHeaderPhrase = new Phrase("Porciones", bold);
                var PortionsHeaderCell = new PdfPCell(PortionsHeaderPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 0,
                    BackgroundColor = PinkHeaderForeground
                };

                var DateHeaderPhrase = new Phrase("Fecha", bold);
                var DateHeaderCell = new PdfPCell(DateHeaderPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 3,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 0,
                    BackgroundColor = PinkHeaderForeground
                };

                var LinePhrase = new Phrase(dish.Line.ToString(), regular);
                var LineCell = new PdfPCell(LinePhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 3,
                    HorizontalAlignment = 1,
                    BorderWidthTop = 0
                };

                var PortionsPhrase = new Phrase(dish.Portions.ToString(), regular);
                var PortionsCell = new PdfPCell(PortionsPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    BorderWidthTop = 0
                };

                var DatePhrase = new Phrase(DateTime.Now.ToString("dd-MM-yyyy"), regular);
                var DateCell = new PdfPCell(DatePhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 3,
                    HorizontalAlignment = 1,
                    BorderWidthTop = 0
                };

                var IngredientsHeaderPhrase = new Phrase("Ingredientes", bold);
                var IngredientsHeaderCell = new PdfPCell(IngredientsHeaderPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 8,
                    HorizontalAlignment = 1,
                    BorderWidthBottom = 0,
                    BackgroundColor = GreenHeaderForeground
                };

                var ProductsHeaderPhrase = new Phrase("Producto", bold);
                var ProductsHeaderCell = new PdfPCell(ProductsHeaderPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    BorderWidthTop = 0,
                    BackgroundColor = GreenRowForeground
                };

                var UnitHeaderPhrase = new Phrase("Unidad", bold);
                var UnitHeaderCell = new PdfPCell(UnitHeaderPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    BorderWidthTop = 0,
                    BackgroundColor = GreenRowForeground
                };

                var QuantityHeaderPhrase = new Phrase("Cantidad", bold);
                var QuantityHeaderCell = new PdfPCell(QuantityHeaderPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    BorderWidthTop = 0,
                    BackgroundColor = GreenRowForeground
                };

                var TotalHeaderPhrase = new Phrase("Total", bold);
                var TotalHeaderCell = new PdfPCell(TotalHeaderPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    BorderWidthTop = 0,
                    BackgroundColor = GreenRowForeground
                };


                Image dishDraw;
                Phrase ImageHeaderPhrase = new Phrase("Imagen Ilustrativa", bold);
                PdfPCell ImageHeaderCell;
                Phrase InstructionsHeaderPhrase = new Phrase("Instrucciones", bold);
                PdfPCell InstructionsHeaderCell;
                PdfPCell ImageCell;
                Phrase InstructionsPhrase = new Phrase(dish.Instructions, regular);
                PdfPCell InstructionsCell = new PdfPCell(InstructionsPhrase);


                if (dish.Draw != null)
                {
                    //Save in a file the draw from the object dish
                    MemoryStream ms = new MemoryStream(dish.Draw);
                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(ms);
                    System.Drawing.Bitmap bmResize = new System.Drawing.Bitmap(255, 255);
                    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bmResize))
                    {
                        //set the resize quality modes to high quality 
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //draw the image into the target bitmap 
                        graphics.DrawImage(bm, 0, 0, bmResize.Width, bmResize.Height);
                    }
                    bmResize.Save(_tempImageFile);

                    dishDraw = Image.GetInstance(_tempImageFile);
                    ImageHeaderCell = new PdfPCell(ImageHeaderPhrase)
                    {
                        PaddingBottom = 3f,
                        Colspan = 4,
                        HorizontalAlignment = 1,
                        BackgroundColor = BlueHeaderForeground
                    };
                    
                    InstructionsHeaderCell = new PdfPCell(InstructionsHeaderPhrase)
                    {
                        PaddingBottom = 3f,
                        Colspan = 4,
                        HorizontalAlignment = 1,
                        BackgroundColor = BlueHeaderForeground
                    };

                    ImageCell = new PdfPCell(dishDraw)
                    {
                        PaddingBottom = 3f,
                        Colspan = 4,
                        HorizontalAlignment = 1
                    };

                    InstructionsPhrase = new Phrase(dish.Instructions, regular);
                    InstructionsCell = new PdfPCell(InstructionsPhrase)
                    {
                        PaddingBottom = 3f,
                        Colspan = 4,
                        HorizontalAlignment = 1
                    };
                }
                else
                {
                    ImageHeaderCell = new PdfPCell(ImageHeaderPhrase)
                    {
                        PaddingBottom = 3f,
                        Colspan = 4,
                        HorizontalAlignment = 1,
                        BackgroundColor = BlueHeaderForeground

                    };
                    ImageCell = new PdfPCell(ImageHeaderPhrase)
                    {
                        PaddingBottom = 3f,
                        Colspan = 4,
                        HorizontalAlignment = 1
                    };

                    InstructionsHeaderPhrase = new Phrase("Instrucciones", bold);
                    InstructionsHeaderCell = new PdfPCell(InstructionsHeaderPhrase)
                    {
                        PaddingBottom = 3f,
                        Colspan = 8,
                        HorizontalAlignment = 1,
                        BackgroundColor = BlueHeaderForeground
                    };

                    InstructionsPhrase = new Phrase(dish.Instructions, regular);
                    InstructionsCell = new PdfPCell(InstructionsPhrase)
                    {
                        PaddingBottom = 3f,
                        Colspan = 8,
                        HorizontalAlignment = 1
                    };
                }

                var NotesHeaderPhrase = new Phrase("Notas", bold);
                var NotesHeaderCell = new PdfPCell(NotesHeaderPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 8,
                    HorizontalAlignment = 1,
                    BackgroundColor = PurpleHeaderForeground
                };

                var NotesPhrase = new Phrase(dish.Notes, regular);
                var NotesCell = new PdfPCell(NotesPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 8,
                    HorizontalAlignment = 1
                };

                var TotalCostHeaderPhrase = new Phrase("Costo Total", bold);
                var TotalCostHeaderCell = new PdfPCell(TotalCostHeaderPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };

                var DishCostHeaderPhrase = new Phrase("Costo Plato", bold);
                var DishCostHeaderCell = new PdfPCell(DishCostHeaderPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };

                var SalePriceHeaderPhrase = new Phrase("Precio Venta", bold);
                var SalePriceHeaderCell = new PdfPCell(SalePriceHeaderPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };

                //All the cells that were created, need to be added to the table
                table.AddCell(CompanyCell);

                table.AddCell(BossCell);

                table.AddCell(CellCell);

                table.AddCell(EmailCell);

                table.AddCell(TitleCell);

                table.AddCell(LineHeaderCell);
                table.AddCell(PortionsHeaderCell);
                table.AddCell(DateHeaderCell);

                table.AddCell(LineCell);
                table.AddCell(PortionsCell);
                table.AddCell(DateCell);

                table.AddCell(EmptyCol);

                table.AddCell(IngredientsHeaderCell);

                table.AddCell(ProductsHeaderCell);
                table.AddCell(UnitHeaderCell);
                table.AddCell(QuantityHeaderCell);
                table.AddCell(TotalHeaderCell);

                foreach (var ingredient in dish.Ingredients)
                {
                    var ProductPhrase = new Phrase(ingredient.Ingredient.Name, regular);
                    var ProductCell = new PdfPCell(ProductPhrase)
                    {

                        HorizontalAlignment = 1,
                        Colspan = 2
                    };

                    var UnitPhrase = new Phrase(ingredient.Ingredient.Unit.ToString(), regular);
                    var UnitCell = new PdfPCell(UnitPhrase)
                    {

                        HorizontalAlignment = 1,
                        Colspan = 2
                    };

                    var QuantityPhrase = new Phrase(ingredient.Quantity.ToString(), regular);
                    var QuantityCell = new PdfPCell(QuantityPhrase)
                    {
                        HorizontalAlignment = 1,
                        Colspan = 2
                    };

                    var total = ingredient.Ingredient.Cost * ingredient.Quantity;
                    _totalCost = _totalCost + total;
                    var TotalPhrase = new Phrase(total.ToString("C"), regular);
                    var TotalCell = new PdfPCell(TotalPhrase)
                    {
                        HorizontalAlignment = 1,
                        Colspan = 2
                    };

                    table.AddCell(ProductCell);
                    table.AddCell(UnitCell);
                    table.AddCell(QuantityCell);
                    table.AddCell(TotalCell);
                }

                table.AddCell(EmptyCol);

                if (dish.Draw != null)
                {
                    table.AddCell(ImageHeaderCell);
                }

                table.AddCell(InstructionsHeaderCell);

                if(dish.Draw != null)
                {

                    table.AddCell(ImageCell);
                }
                table.AddCell(InstructionsCell);

                table.AddCell(EmptyCol);

                table.AddCell(NotesHeaderCell);
                table.AddCell(NotesCell);

                table.AddCell(EmptyCol);

                table.AddCell(TotalCostHeaderCell);
                table.AddCell(EmptyCell);
                table.AddCell(DishCostHeaderCell);
                table.AddCell(EmptyCell);
                table.AddCell(SalePriceHeaderCell);

                var TotalCostPhrase = new Phrase(_totalCost.ToString("C"), regular);
                var TotalCostCell = new PdfPCell(TotalCostPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };

                var dishCost = Math.Round(_totalCost / dish.Portions, 2);
                var DishCostPhrase = new Phrase(dishCost.ToString("C"), regular);
                var DishCostCell = new PdfPCell(DishCostPhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };

                var salePrice = Math.Round((dishCost * (Config.Current.EarningsPercent / 100)) + dishCost, 2);
                var SalePricePhrase = new Phrase(salePrice.ToString("C"), regular);
                var SalePriceCell = new PdfPCell(SalePricePhrase)
                {
                    PaddingBottom = 3f,
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    Border = Rectangle.NO_BORDER
                };

                table.AddCell(TotalCostCell);
                table.AddCell(EmptyCell);
                table.AddCell(DishCostCell);
                table.AddCell(EmptyCell);
                table.AddCell(SalePriceCell);

                //Add the Images
                doc.Add(logo);

                //Add the Paragraph to the document
                doc.Add(table);

                //Close the document when all the elements were added
                doc.Close();

                stream.Dispose();

                DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\temp");
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                return true;
            }
            catch (Exception ex)
            {
                App.Log.Message(ex.Serialize());
                return false;
            }
        }
    }
}
