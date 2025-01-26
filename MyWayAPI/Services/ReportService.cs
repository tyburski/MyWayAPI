using Azure.Core.GeoJson;
using Geocoding.Google;
using Geocoding;
using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using static System.Net.WebRequestMethods;
using NPOI.HSSF.UserModel;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.Formula.Functions;

namespace MyWayAPI.Services
{
    public interface IReportService
    {
        public async void GenerateReport(int id, bool forUser)
        {
            throw new NotImplementedException();
        }
    }
    public class ReportService: IReportService
    {
        private readonly MWDbContext dbContext;
        private readonly IMailService mailService;
        private static readonly HttpClient client = new HttpClient();
        public ReportService(MWDbContext dbContext, IMailService mailService)
        {
            this.dbContext = dbContext;
            this.mailService = mailService;
        }

        public async void GenerateReport(int id, bool forUser)
        {

            var route = dbContext.Routes.Include(r=>r.RouteEvents).Include(r=>r.User).Include(r => r.Vehicle).Include(r => r.Company).FirstOrDefault(r=>r.Id == id);
            int counter = 1;

            for (int i=0; i < route.RouteEvents.Count; i++)
            {
                if (route.RouteEvents[i].DropDate is not null)
                {
                    route.RouteEvents[i].EventName = $"p.{counter}";
                    var newEvent = new RouteEvent
                    {
                        EventName = $"d.{counter}",
                        Date = route.RouteEvents[i].DropDate.GetValueOrDefault(),
                        Latitude = route.RouteEvents[i].DropLatitude.GetValueOrDefault(),
                        Longitude = route.RouteEvents[i].DropLongitude.GetValueOrDefault(),
                        PickupCount = null,
                        PickupWeight = null,
                        PickupComment = null,
                        DropDate = null,
                        DropLatitude = null,
                        DropLongitude = null,
                        RefuelCount = null,
                        RefuelTotal = null,
                        RefuelCurrency = null,
                        RefuelType = null,
                        BorderFrom = null,
                        BorderTo = null,
                        Route = route
                    };
                    route.RouteEvents.Add(newEvent);
                    counter++;
                }
            }
            List<RouteEvent> sortedList = route.RouteEvents.OrderBy(e => e.Date).ToList();
            IWorkbook wb = new XSSFWorkbook();

            XSSFFont headerFont = (XSSFFont)wb.CreateFont();
            headerFont.FontHeightInPoints = (short)10;
            headerFont.FontName = "Arial";
            headerFont.Color = IndexedColors.White.Index;
            headerFont.IsBold = true;
            headerFont.IsItalic = false;

            XSSFFont textFont = (XSSFFont)wb.CreateFont();
            textFont.FontHeightInPoints = (short)10;
            textFont.FontName = "Arial";
            textFont.Color = IndexedColors.Black.Index;
            textFont.IsBold = false;
            textFont.IsItalic = false;

            XSSFCellStyle headerStyle = (XSSFCellStyle)wb.CreateCellStyle();
            headerStyle.WrapText = false;
            headerStyle.FillForegroundColor = IndexedColors.LightBlue.Index;
            headerStyle.FillPattern = FillPattern.SolidForeground;
            headerStyle.Alignment = HorizontalAlignment.Left;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.BorderBottom = BorderStyle.Thin;
            headerStyle.BorderTop = BorderStyle.Thin;
            headerStyle.BorderLeft = BorderStyle.Thin;
            headerStyle.BorderRight = BorderStyle.Thin;
            headerStyle.SetFont(headerFont);

            XSSFCellStyle textStyle = (XSSFCellStyle)wb.CreateCellStyle();
            textStyle.WrapText = true;
            textStyle.FillForegroundColor = IndexedColors.White.Index;
            textStyle.FillPattern = FillPattern.SolidForeground;
            textStyle.Alignment = HorizontalAlignment.Left;
            textStyle.VerticalAlignment = VerticalAlignment.Center;
            textStyle.BorderBottom = BorderStyle.Thin;
            textStyle.BorderTop = BorderStyle.Thin;
            textStyle.BorderLeft = BorderStyle.Thin;
            textStyle.BorderRight = BorderStyle.Thin;
            textStyle.SetFont(textFont);


            ISheet ws = wb.CreateSheet("Raport");

            IRow r1 = ws.CreateRow(0);
            ICell c1 = r1.CreateCell(0);
            c1.SetCellValue("Imię i nazwisko");
            ICell c2 = r1.CreateCell(1);
            c2.SetCellValue("Data rozpoczęcia");
            ICell c3 = r1.CreateCell(2);
            c3.SetCellValue("Data zakończenia");
            ICell c4 = r1.CreateCell(3);
            c4.SetCellValue("Pojazd");

            IRow r2 = ws.CreateRow(1);
            ICell cd1 = r2.CreateCell(0);
            cd1.SetCellValue($"{route.User.FirstName} {route.User.LastName}");
            ICell cd2 = r2.CreateCell(1);
            cd2.SetCellValue($"{sortedList.First().Date.ToString()}");
            ICell cd3 = r2.CreateCell(2);
            cd3.SetCellValue($"{sortedList.Last().Date.ToString()}");
            ICell cd4 = r2.CreateCell(3);
            cd4.SetCellValue($"{route.Vehicle.LicensePlate}");

            IRow r3 = ws.CreateRow(3);
            ICell cl1 = r3.CreateCell(0);
            ICell cl2 = r3.CreateCell(1);
            ICell cl3 = r3.CreateCell(2);
            ICell cl4 = r3.CreateCell(3);
            ICell cl5 = r3.CreateCell(4);
            ICell cl6 = r3.CreateCell(5);
            ICell cl7 = r3.CreateCell(6);
            ICell cl8 = r3.CreateCell(7);
            ICell cl9 = r3.CreateCell(8);
            ICell cl10 = r3.CreateCell(9);
            ICell cl11 = r3.CreateCell(10);
            ICell cl12 = r3.CreateCell(11);

            cl1.SetCellValue("Data zdarzenia");
            cl2.SetCellValue("Nazwa zdarzenia");
            cl3.SetCellValue("Miejsce zdarzenia");
            cl4.SetCellValue("Granica - Kraj 1");
            cl5.SetCellValue("Granica - Kraj 2");
            cl6.SetCellValue("Tankowanie - Rodzaj");
            cl7.SetCellValue("Tankowanie - Ilość");
            cl8.SetCellValue("Tankowanie - Wartość");
            cl9.SetCellValue("Tankowanie - Waluta");
            cl10.SetCellValue("Załadunek - Liczba colli");
            cl11.SetCellValue("Załadunek - Waga[kg]");
            cl12.SetCellValue("Załadunek - Komentarz");


            for (int i = 0; i < sortedList.Count; i++)
            {
                
                IRow r = ws.CreateRow(i+4);
                ICell dateCell = r.CreateCell(1);
                dateCell.SetCellValue(sortedList[i].Date.ToString());

                ICell eventNameCell = r.CreateCell(0);
                if (sortedList[i].EventName.Equals("border"))
                {
                    eventNameCell.SetCellValue($"Granica");
                }
                if (sortedList[i].EventName.Equals("refuel"))
                {
                    eventNameCell.SetCellValue($"Tankowanie");
                }
                if (sortedList[i].EventName.Split('.')[0].Equals("p"))
                {
                    eventNameCell.SetCellValue($"Załadunek [Zlecenie: {sortedList[i].EventName.Split('.')[1]}]");
                    counter++;
                }
                if (sortedList[i].EventName.Split('.')[0].Equals("d"))
                {
                    eventNameCell.SetCellValue($"Rozładunek [Zlecenie: {sortedList[i].EventName.Split('.')[1]}]");
                }
                if (sortedList[i].EventName.Equals("start"))
                {
                    eventNameCell.SetCellValue($"Rozpoczęcie trasy");
                }
                if (sortedList[i].EventName.Equals("finish"))
                {
                    eventNameCell.SetCellValue($"Zakończenie trasy");
                }

                GoogleGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyA9hduwtw4JwrFRrBgsMqHR8S9KSYyHvWI" };
                IEnumerable<GoogleAddress> addresses = await geocoder.ReverseGeocodeAsync(sortedList[i].Latitude, sortedList[i].Longitude);
                var city = addresses.Where(a => !a.IsPartialMatch).Select(a => a[GoogleAddressType.Locality]).First();
                var country = addresses.Where(a => !a.IsPartialMatch).Select(a => a[GoogleAddressType.Country]).First();

                ICell cityCell = r.CreateCell(2);
                cityCell.SetCellValue($"{city.ShortName} [{country.ShortName}]");
                ICell borderFromCell = r.CreateCell(3);
                ICell borderToCell = r.CreateCell(4);
                ICell refuelTypeCell = r.CreateCell(5);
                ICell refuelCountCell = r.CreateCell(6);
                ICell refuelTotalCell = r.CreateCell(7);
                ICell refuelCurrencyCell = r.CreateCell(8);
                ICell pickupCountCell = r.CreateCell(9);
                ICell pickupWeightCell = r.CreateCell(10);
                ICell pickupCommentCell = r.CreateCell(11);
                if (sortedList[i].EventName.Equals("border"))
                {
                    borderFromCell.SetCellValue(sortedList[i].BorderFrom);
                    borderToCell.SetCellValue(sortedList[i].BorderTo);
                }
                else
                {
                    borderFromCell.SetCellValue("-");
                    borderToCell.SetCellValue("-");
                }
                if (sortedList[i].EventName.Equals("refuel"))
                {
                    refuelTypeCell.SetCellValue(sortedList[i].RefuelType);
                    refuelCountCell.SetCellValue(sortedList[i].RefuelCount.ToString());
                    refuelTotalCell.SetCellValue(sortedList[i].RefuelTotal.ToString());
                    refuelCurrencyCell.SetCellValue(sortedList[i].RefuelCurrency);
                }
                else
                {
                    refuelTypeCell.SetCellValue("-");
                    refuelCountCell.SetCellValue("-");
                    refuelTotalCell.SetCellValue("-");
                    refuelCurrencyCell.SetCellValue("-");
                }
                if (sortedList[i].EventName.Split('.')[0].Equals("p"))
                {
                    pickupCountCell.SetCellValue(sortedList[i].PickupCount.ToString());
                    pickupWeightCell.SetCellValue(sortedList[i].PickupWeight.ToString());
                    if (sortedList[i].PickupComment is not null)
                    {
                        pickupCommentCell.SetCellValue(sortedList[i].PickupComment.ToString());
                    }
                    else
                    {
                        pickupCommentCell.SetCellValue("brak");
                    }
                    
                }
                else
                {
                    pickupCountCell.SetCellValue("-");
                    pickupWeightCell.SetCellValue("-");
                    pickupCommentCell.SetCellValue("-");
                }
                
                for (int c = 0; c < r.Cells.Count; c++)
                {
                    r.Cells[c].CellStyle = textStyle;
                }

            }
            for (int c = 0; c < r1.Cells.Count; c++)
            {
               r1.Cells[c].CellStyle = headerStyle;
            }
            for (int c = 0; c < r2.Cells.Count; c++)
            {
                r2.Cells[c].CellStyle = textStyle;
            }
            for (int c = 0; c < r3.Cells.Count; c++)
            {
                r3.Cells[c].CellStyle = headerStyle;
            }
            for (int i = 0; i<=12; i++)
            {
                ws.AutoSizeColumn(i);
            }
            string fileName = $"{route.User.FirstName}_{route.User.LastName}_{route.RouteEvents[0].Date.ToShortDateString()}-{route.RouteEvents.Last().Date.ToShortDateString()}.xlsx";
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                wb.Write(stream);
                var sendTo = "";
                if(forUser is true)
                {
                    sendTo = route.User.EmailAddress;
                }
                else
                {
                    sendTo = route.Company.Email;
                }
                mailService.SendEmail(sendTo, $"{route.User.FirstName} {route.User.LastName} {route.Vehicle.LicensePlate} {route.RouteEvents.First().Date.ToShortDateString()} - {route.RouteEvents.Last().Date.ToShortDateString()}", fileName);
            }
            System.IO.File.Delete($"./{fileName}");
        }
    }
}
