using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using KABE_Food_Ordering_System.Models;

namespace KABE_Food_Ordering_System.Logic
{
    public class AccountLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public BaseLogic<User> accountRepo = new BaseLogic<User>(new ApplicationDbContext());
        public BaseLogic<Location> locationLogic = new BaseLogic<Location>(new ApplicationDbContext());
        public BaseLogic<Restaurant> restaurantLogic = new BaseLogic<Restaurant>(new ApplicationDbContext());
        public BaseLogic<Customer>customerLogic = new BaseLogic<Customer>(new ApplicationDbContext());


        public string Celebrate(string role, string email, string name, DateTime dateTobeCelebrated)
        {
            var celebrationText = "";
            if (dateTobeCelebrated.Date == DateTime.Now.Date)
            {
                if (role == "Customer")
                {
                    CelebrateCustomerMail(email,name);
                    celebrationText= "We are wishing you a happy birthday, Long Life and Prosperity.";
                }
                if (role == "Restaurant")
                {
                    CelebrateRestaurantMail(email, name);
                    celebrationText = "We are wishing you a happy anniversary, Looking forward to doing more business with you.";

                }

            }
            return celebrationText;
        }

        public void SaveRestaurant(User user)
        {
            Restaurant restaurant = new Restaurant();
            restaurant.RestaurantsId = user.Id;
            restaurant.LocationId = _context.Locations.First().Id;
            restaurant.Name = user.Name;
            restaurant.ResidentialAddress = "none";
            restaurant.PhoneNumber = "00000000000";
            restaurant.EstablishmentDate = DateTime.Now;
            restaurant.Email = user.Email;
            //restaurant.Foods.Add("0","0");
            restaurant.FoodId = "0,0";
            restaurant.Price = "0";
            //restaurant.Food.Add("");
            //restaurant.Locations = locationLogic.GetAll();
            restaurantLogic.Save(restaurant);
        }
        public void SaveCustomer(User user)
        {
            Customer customer = new Customer();
            customer.UsersId = user.Id;
            customer.LocationId = _context.Locations.First().Id;
            customer.Name = user.Name;
            customer.DateOfBirth = DateTime.Now;
            customer.FoodAllergies = "";
            customer.BestFood = "";
            customer.Email = user.Email;
            customer.Occupation = "";
            customer.PhoneNumber = "00000000000";
            customer.RecommendedFood = "";
            customer.ResidentialAddress = "";
            customer.Gender = "";
            customerLogic.Save(customer);
        }


        public string GeneratePassword()
        {
            var PasswordLength = 9;
            string allowedChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*<>?";
            Random randChars = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharC = allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = allowedChars[(int)((allowedChars.Length) * randChars.NextDouble())];
            }
            var password = new string(chars);
            return password.ToString();
        }

        public void SendingEmail(string email, string role, string password)
        {
            var fromEmail = new MailAddress("oyelayodeborah@gmail.com", "KABE_Food_Ordering_System Food Ordering and Recommendation System");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "15ch03774";
            string subject = "Login details !!!";

            string body = "<br/><br/>Dear "+role+",<br/>Kindly login with the provided url below <br/> " +
                "Username: " + email + "<br/>Password: " + password + 
                " <br/>"+ "Click Here to proceed <a href=http://localhost:50125/Account/Login target=_blank>Here</a>" + " <br/> This email is confidential ensure you delete it after use"; ;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {

                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    smtp.Send(message);
                    //return password;
                }
                catch (SmtpException)
                {
                    SmtpException output = new SmtpException("An error ocurred while sending the mail");
                    throw output;
                    //var output = "Error";
                    //return output;
                }

        }
        public string SendingMail(string email, string role, string password)
        {
            var fromEmail = new MailAddress("oyelayodeborah@gmail.com", "KABE Food Ordering System");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "15ch03774";
            string subject = "Login details !!!";

            string body = "<br/><br/>Dear " + role + ",<br/> We are happy to tell you your account has been created successfully<br/> " +
                "Username: " + email + "<br/>Password: " + password +" <br/> This email is confidential ensure you delete it after use";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {

                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    smtp.Send(message);
                    return "Successful";
                }
                catch (SmtpException)
                {
                    //SmtpException output= new SmtpException("An error ocurred while sending the mail");
                    //throw output;
                    var output = "Error";
                    return output;
                }

        }
        public string SendingVerificationCode(string email, string name, string verificationCode)
        {
            var fromEmail = new MailAddress("oyelayodeborah@gmail.com", "KABE Food Ordering System");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "15ch03774";
            string subject = "Verification Code !!!";

            string body = "<br/><br/>Dear " + name + ",<br/> This is the verification Code for the transaction you are about to perform is "+verificationCode+"<br/> This email is confidential ensure you delete it after use";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {

                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    smtp.Send(message);
                    return "Successful";
                }
                catch (SmtpException)
                {
                    //SmtpException output= new SmtpException("An error ocurred while sending the mail");
                    //throw output;
                    var output = "Error";
                    return output;
                }

        }
        public string CelebrateCustomerMail(string email, string name)
        {
            var fromEmail = new MailAddress("oyelayodeborah@gmail.com", "KABE Food Ordering System");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "15ch03774";
            string subject = "Greetings from KABE Food Ordering System !!!";

            string body = "<br/><br/>Dear " + name + ",<br/> KABE Food Ordering System is wishing you a happy birthday, Long Life and Prosperity.<br/> Have a great day";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {

                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    smtp.Send(message);
                    return "Successful";
                }
                catch (SmtpException)
                {
                    //SmtpException output= new SmtpException("An error ocurred while sending the mail");
                    //throw output;
                    var output = "Error";
                    return output;
                }

        }
        public string CelebrateRestaurantMail(string email, string name)
        {
            var fromEmail = new MailAddress("oyelayodeborah@gmail.com", "KABE Food Ordering System");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "15ch03774";
            string subject = "Greetings from KABE Food Ordering System !!!";

            string body = "<br/><br/>Dear " + name + ",<br/> KABE Food Ordering System is wishing you a happy anniversary, we love doing business with you.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {

                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    smtp.Send(message);
                    return "Successful";
                }
                catch (SmtpException)
                {
                    //SmtpException output= new SmtpException("An error ocurred while sending the mail");
                    //throw output;
                    var output = "Error";
                    return output;
                }

        }

        public User GetUserByEmail(string Email)
        {
            var finddetails = _context.Users.Where(a => a.Email == Email).FirstOrDefault();

            return finddetails;
        }

        public bool IsNameDetailsExist(string value)
        {
            var finddetails = _context.Users.Where(a => a.Name == value).Count();

            if (finddetails == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDetailsExist(string value)
        {
            var finddetails = _context.Users.Where(a => a.Email == value).Count();

            if (finddetails == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsEditDetailsExist(string value)
        {

            var findDetails = _context.Users.Where(a => a.Email == value).Count();
            if (findDetails > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void NotifyEmail(string email, string role)
        {
            var fromEmail = new MailAddress("oyelayodeborah@gmail.com", "KABE Food Ordering System");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "15ch03774";
            string subject = "Login details !!!";

            string body = "<br/><br/>Dear " + role + ",<br/>This is to inform you that your account password has been changed<br/> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {

                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    smtp.Send(message);
                    //return password;
                }
                catch (SmtpException)
                {
                    SmtpException output = new SmtpException("An error ocurred while sending the mail");
                    throw output;
                    //var output = "Error";
                    //return output;
                }

        }

    }
}