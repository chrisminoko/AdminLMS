using BackEnd.Models;
using BackEnd.Models.OnlineShop;
using BackEnd.Services;

using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using PayFast;
using PayFast.AspNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace BackEnd.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private Order_Service order_Service;



        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }

        private readonly PayFastSettings payFastSettings;

        #region Constructor

        public PaymentController()
        {
            this.payFastSettings = new PayFastSettings
            {
                MerchantId = ConfigurationManager.AppSettings["MerchantId"],
                MerchantKey = ConfigurationManager.AppSettings["MerchantKey"],
                PassPhrase = ConfigurationManager.AppSettings["PassPhrase"],
                ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"],
                ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"],
                ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"],
                CancelUrl = ConfigurationManager.AppSettings["CancelUrl"],
                NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"]
            };
            order_Service = new Order_Service();

        }

        #endregion Constructor

        #region Methods



        public ActionResult Recurring()
        {

            var recurringRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            recurringRequest.merchant_id = this.payFastSettings.MerchantId;
            recurringRequest.merchant_key = this.payFastSettings.MerchantKey;
            recurringRequest.return_url = this.payFastSettings.ReturnUrl;
            recurringRequest.cancel_url = this.payFastSettings.CancelUrl;
            recurringRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            recurringRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            recurringRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            recurringRequest.amount = 20;
            recurringRequest.item_name = "Recurring Option";
            recurringRequest.item_description = "Some details about the recurring option";

            // Transaction Options
            recurringRequest.email_confirmation = true;
            recurringRequest.confirmation_address = "drnendwandwe@gmail.com";

            // Recurring Billing Details
            recurringRequest.subscription_type = SubscriptionType.Subscription;
            recurringRequest.billing_date = DateTime.Now;
            recurringRequest.recurring_amount = 20;
            recurringRequest.frequency = BillingFrequency.Monthly;
            recurringRequest.cycles = 0;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{recurringRequest.ToString()}";

            return Redirect(redirectUrl);
        }




        public void SendMail(string Order_ID)
        {
            var userName = User.Identity.GetUserName();
            /* Find the details of the customer placing the order*/
            var customer = db.Customers.Where(x => x.Email == userName).FirstOrDefault();

            var attachments = new List<Attachment>();
            //attachments.Add(new Attachment(new MemoryStream(GeneratePDF(Order_ID)), "Order Receipt", "application/pdf"));

            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(User.Identity.GetUserName(), customer.FirstName));
            var body = $"Hello {customer.FirstName}, please see attached receipt for the recent order you made.<br/>";
            Services.Email_Service emailService = new Services.Email_Service();
            emailService.SendEmail(new Services.EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Application Statement | Ref No.:" + Order_ID,
                mailBody = body,
                mailFooter = "<br/> Many Thanks, <br/> <b>Imfuyo Ranch</b>",
                mailPriority = MailPriority.High,
                mailAttachments = attachments
            });
        }
        //public byte[] GeneratePDF(string orderID)
        //{
        //    MemoryStream memoryStream = new MemoryStream();
        //    Document document = new XDocument(PageSize.A5, 0, 0, 0, 0);
        //    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
        //    document.Open();

        //    Order order = db.Orders.Find(orderID);
        //    var userName = User.Identity.GetUserName();
        //    /* Find the details of the customer placing the order*/
        //    var customer = db.Customers.Where(x => x.Email == userName).FirstOrDefault();
        //    Order_Item order_Item = db.Order_Items.Find(orderID);

        //    //var reservation = _iReservationService.Get(Convert.ToInt64(ReservationID));
        //    //var user = _iUserService.Get(reservation.UserID);

        //    iTextSharp.text.Font font_heading_3 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
        //    iTextSharp.text.Font font_body = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.BaseColor.BLUE);

        //    // Create the heading paragraph with the headig font
        //    PdfPTable table1 = new PdfPTable(1);
        //    PdfPTable table2 = new PdfPTable(5);
        //    PdfPTable table3 = new PdfPTable(1);

        //    iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();
        //    seperator.Offset = -6f;
        //    // Remove table cell
        //    table1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    table3.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

        //    table1.WidthPercentage = 80;
        //    table1.SetWidths(new float[] { 100 });
        //    table2.WidthPercentage = 80;
        //    table3.SetWidths(new float[] { 100 });
        //    table3.WidthPercentage = 80;

        //    PdfPCell cell = new PdfPCell(new Phrase(""));
        //    cell.Colspan = 3;
        //    table1.AddCell("\n");
        //    table1.AddCell(cell);
        //    table1.AddCell("\n\n");
        //    table1.AddCell(
        //        "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t" +
        //        "Alliance Properties SA \n" +
        //        "Email :Alliance.grp18@gmail.com" + "\n" +
        //        "\n" + "\n");
        //    table1.AddCell("------------Customer Details--------------!");

        //    table1.AddCell("First Name : \t" + customer.FirstName);
        //    table1.AddCell("Last Name : \t" + customer.LastName);
        //    table1.AddCell("Phone Number : \t" + customer.phone);
        //    table1.AddCell("Address : \t" + customer.Address);

        //    table1.AddCell("\n------------Order details--------------!\n");

        //    table1.AddCell("Order # : \t" + orderID);
        //    table1.AddCell("Price : \t" + order_Item.price);
        //    table1.AddCell("Qauntity : \t" + order_Item.quantity);
        //    table1.AddCell("Items : \t" + order_Item.Item.Name);
        //    //table1.AddCell("Building name : \t" + roomBooking.BuildingId);
        //    //table1.AddCell("Building Address : \t" + roomBooking.BuildingAddress);

        //    table1.AddCell("\n");

        //    table3.AddCell("------------Looking forward to hear from you soon--------------!");

        //    //////Intergrate information into 1 document
        //    //var qrCode = iTextSharp.text.Image.GetInstance(roomBooking.QrCodeImage);
        //    //qrCode.ScaleToFit(200, 200);
        //    table1.AddCell(cell);
        //    document.Add(table1);
        //    //document.Add(qrCode);
        //    document.Add(table3);
        //    document.Close();

        //    byte[] bytes = memoryStream.ToArray();
        //    memoryStream.Close();
        //    return bytes;
        //}



        public ActionResult OnceOff(string id)
        {
            var order = order_Service.GetOrder(id);
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            order_Service.MarkOrderAsPaid(id);

            //SendMail(id);

            //var order=
            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            //double amount = Convert.ToDouble(db.Items.Select(x => x.CostPrice).FirstOrDefault());
            //var products = db.Items.Select(x => x.Name).ToList();
            // Transaction Details
            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = order_Service.GetOrderTotal(order.Order_ID);
            onceOffRequest.item_name = "Once off option";
            onceOffRequest.item_description = "Some details about the once off payment";


            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        public ActionResult AdHoc()
        {
            var adHocRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            adHocRequest.merchant_id = this.payFastSettings.MerchantId;
            adHocRequest.merchant_key = this.payFastSettings.MerchantKey;
            adHocRequest.return_url = this.payFastSettings.ReturnUrl;
            adHocRequest.cancel_url = this.payFastSettings.CancelUrl;
            adHocRequest.notify_url = this.payFastSettings.NotifyUrl;
            #endregion Methods
            // Buyer Details
            adHocRequest.email_address = "sbtu01@payfast.co.za";
            //double amount = Convert.ToDouble(db.FoodOrders.Select(x => x.Total).FirstOrDefault());
            //var products = db.FoodOrders.Select(x => x.UserEmail).ToList();
            // Transaction Details
            adHocRequest.m_payment_id = "";
            adHocRequest.amount = 70;
            adHocRequest.item_name = "Adhoc Agreement";
            adHocRequest.item_description = "Some details about the adhoc agreement";

            // Transaction Options
            adHocRequest.email_confirmation = true;
            adHocRequest.confirmation_address = "sbtu01@payfast.co.za";

            // Recurring Billing Details
            adHocRequest.subscription_type = SubscriptionType.AdHoc;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{adHocRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        public ActionResult Return()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Notify([ModelBinder(typeof(PayFastNotifyModelBinder))] PayFastNotify payFastNotifyViewModel)
        {
            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            System.Diagnostics.Debug.WriteLine($"Signature Validation Result: {isValid}");

            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases
            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, IPAddress.Parse(this.HttpContext.Request.UserHostAddress));

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            System.Diagnostics.Debug.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

            System.Diagnostics.Debug.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

            // Currently seems that the data validation only works for successful payments
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = await payfastValidator.ValidateData();

                System.Diagnostics.Debug.WriteLine($"Data Validation Result: {dataValidationResult}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                System.Diagnostics.Debug.WriteLine($"Subscription was cancelled");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}