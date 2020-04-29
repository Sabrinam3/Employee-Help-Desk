/*
 * Class Name: CallReport
 * Purpose: Generates a report of all the calls currently in the database
 * Date: November 26, 2018
 */
 using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HelpdeskViewModels;


namespace HelpdeskWebsite.Reports
{
    public class CallReport
    {
        static Font catFont = new Font(Font.FontFamily.HELVETICA, 24, Font.BOLD);
        static Font subFont = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD);
        static Font smallFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
        static Font listFont = new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD);
        static string mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
        static string IMG = "Content/img/reportlogo.png";

        public void doIt()
        {
            try
            {
                Document document = new Document();
                PdfWriter.GetInstance(document,
                    new FileStream(mappedPath + "Pdfs/Call.pdf", FileMode.Create));
                document.Open();
                Paragraph para = new Paragraph();
                Image image1 = Image.GetInstance(mappedPath + IMG);               
                image1.SetAbsolutePosition(10f, 680f);
                para.Add(image1);
                para.Alignment = Element.ALIGN_RIGHT;
                //A big header
                addEmptyLine(para, 3);
                Paragraph mainHead = new Paragraph(String.Format("{0,8}", "Calls"), catFont);
                mainHead.Alignment = Element.ALIGN_CENTER;
                para.Add(mainHead);
                addEmptyLine(para, 3);

                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 90.00F;
                table.AddCell(addCell("Opened", "h"));
                table.AddCell(addCell("Lastname", "h"));
                table.AddCell(addCell("Tech", "h"));
                table.AddCell(addCell("Problem", "h"));
                table.AddCell(addCell("Status", "h"));
                table.AddCell(addCell("Closed", "h"));
                CallViewModel call = new CallViewModel();
                List<CallViewModel> calls = call.GetAll();

                foreach (CallViewModel cll in calls)
                {
                    table.AddCell(addCell(cll.DateOpened.ToShortDateString()));
                    table.AddCell(addCell(cll.EmployeeName));
                    table.AddCell(addCell(cll.TechName));
                    table.AddCell(addCell(cll.ProblemDescription));
                    table.AddCell(addCell(cll.OpenStatus == false ? "Closed" : "Open"));
                    if (cll.DateClosed != null)
                    {
                        DateTime closedDate = Convert.ToDateTime(cll.DateClosed);
                        table.AddCell(addCell(closedDate.ToShortDateString()));
                    }
                    else
                        table.AddCell(addCell(""));
                }
                para.Add(table);
                addEmptyLine(para, 3);
                para.Alignment = Element.ALIGN_CENTER;
                Paragraph footer = new Paragraph("Call report written on - " + DateTime.Now, smallFont);
                footer.Alignment = Element.ALIGN_CENTER;
                para.Add(footer);
                document.Add(para);
                document.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error " + ex.Message);
            }
        }

        public void generateSpecificEmployeeReport(int id)
        {
            try
            {
                /*Retrieve the employee instance associated with the id being passed in to this method*/
                EmployeeViewModel emp = new EmployeeViewModel();
                emp.Id = id;
                emp.GetById();
                Document document = new Document();
                PdfWriter.GetInstance(document,
                    new FileStream(mappedPath + "Pdfs/EmployeeCall.pdf", FileMode.Create));
                document.Open();
                Paragraph para = new Paragraph();
                Image image1 = Image.GetInstance(mappedPath + IMG);
                image1.SetAbsolutePosition(10f, 680f);
                para.Add(image1);
                para.Alignment = Element.ALIGN_RIGHT;
                //A big header
                addEmptyLine(para, 3);
                Paragraph mainHead = new Paragraph(String.Format("{0,8}", "Calls : " + emp.Firstname + " " + emp.Lastname), catFont);
                mainHead.Alignment = Element.ALIGN_CENTER;
                para.Add(mainHead);
                addEmptyLine(para, 3);            
                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 90.00F;
                table.AddCell(addCell("Opened", "h"));
                table.AddCell(addCell("Lastname", "h"));
                table.AddCell(addCell("Tech", "h"));
                table.AddCell(addCell("Problem", "h"));
                table.AddCell(addCell("Status", "h"));
                table.AddCell(addCell("Closed", "h"));

                /*Get all the calls and then filter by employee id*/
                CallViewModel call = new CallViewModel();
                List<CallViewModel> allCalls = call.GetAll();
                foreach (CallViewModel cll in allCalls)
                {
                    /*Only add calls for that particular employee to the report*/
                    if(cll.EmployeeId == id)
                    {
                        table.AddCell(addCell(cll.DateOpened.ToShortDateString()));
                        table.AddCell(addCell(cll.EmployeeName));
                        table.AddCell(addCell(cll.TechName));
                        table.AddCell(addCell(cll.ProblemDescription));
                        table.AddCell(addCell(cll.OpenStatus == false ? "Closed" : "Open"));
                        if (cll.DateClosed != null)
                        {
                            DateTime closedDate = Convert.ToDateTime(cll.DateClosed);
                            table.AddCell(addCell(closedDate.ToShortDateString()));
                        }
                        else
                            table.AddCell(addCell(""));
                    }                
                }
                para.Add(table);
                addEmptyLine(para, 3);
                para.Alignment = Element.ALIGN_CENTER;
                Paragraph footer = new Paragraph("Call report written on - " + DateTime.Now, smallFont);
                footer.Alignment = Element.ALIGN_CENTER;
                para.Add(footer);
                document.Add(para);
                document.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error " + ex.Message);
            }
        }

        private static void addEmptyLine(Paragraph paragraph, int number)
        {
            for (int i = 0; i < number; i++)
                paragraph.Add(new Paragraph(" "));
        }

        private PdfPCell addCell(string data, string celltype = "d")
        {
            PdfPCell cell;
            if (celltype == "h")
            {
                cell = new PdfPCell(new Phrase(data, smallFont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = Rectangle.NO_BORDER;
            }
            else
            {
                cell = new PdfPCell(new Phrase(data, listFont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = Rectangle.NO_BORDER;
            }
            return cell;
        }
    }
}