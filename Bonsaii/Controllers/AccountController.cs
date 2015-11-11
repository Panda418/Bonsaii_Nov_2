using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Bonsaii.Models;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Linq.Expressions;

namespace Bonsaii.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private int VerifyCodeOverTimeSeconds = 180;        //验证码的过期时间
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private struct BalanceResult
        {
            public int nResult;
            public long dwCorpId;
            public int nStaffId;
            public float fRemain;
        }

        // 接口地址
        private static readonly string POST_URL = "http://smsapi.c123.cn/OpenPlatform/OpenApi";

        // 接口帐号, 请换成你的帐号, 格式为 1001@+8位登录帐号+0001
        private static readonly string ACCOUNT = "1001@501209470001";

        // 接口密钥, 请换成你的帐号对应的接口密钥
        private static readonly string AUTHKEY = "02D0FD280193B79827453D90F1152E4B";

        // 通道组编号, 请换成你的帐号可以使用的通道组编号
        private static readonly uint CGID = 52;

        /************************************************************************/
        /* UrlEncode
        /* 对指定字符串进行URL标准化转码
        /************************************************************************/
        private static string UrlEncode(string text, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byData = encoding.GetBytes(text);
            for (int i = 0; i < byData.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byData[i], 16));
            }
            return sb.ToString();
        }

        /************************************************************************/
        /* sendQuery
        /* 向指定的接口地址POST数据并返回响应数据
        /************************************************************************/
        private static string sendQuery(string url, string param)
        {
            // 准备要POST的数据
            byte[] byData = Encoding.UTF8.GetBytes(param);

            // 设置发送的参数
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "POST";
            req.Timeout = 5000;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = byData.Length;

            // 提交数据
            Stream rs = req.GetRequestStream();
            rs.Write(byData, 0, byData.Length);
            rs.Close();

            // 取响应结果
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);

            try
            {
                return sr.ReadToEnd();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

            sr.Close();
            return null;
        }

        /************************************************************************/
        /* 分析返回的结果值
        /************************************************************************/
        private static int parseResult(string sResult)
        {
            if (sResult != null)
            {
                try
                {
                    // 对返回值分析
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(sResult);
                    XmlElement root = xml.DocumentElement;
                    string sRet = root.GetAttribute("result");
                    return Convert.ToInt32(sRet);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return -100;
        }

        private static BalanceResult parseBalanceResult(string sResult)
        {
            BalanceResult ret = new BalanceResult();
            int nRet = parseResult(sResult);
            ret.nResult = nRet;
            if (nRet < 0) return ret;

            try
            {
                // 对返回值分析
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(sResult);
                XmlElement root = xml.DocumentElement;
                if (nRet > 0)
                {
                    XmlNode item = xml.SelectSingleNode("/xml/Item");
                    if (item != null)
                    {
                        ret.dwCorpId = Convert.ToInt64(item.Attributes["cid"].Value);
                        ret.nStaffId = Convert.ToInt32(item.Attributes["sid"].Value);
                        ret.fRemain = (float)Convert.ToDouble(item.Attributes["remain"].Value);
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return ret;
        }

        /************************************************************************/
        /* 群发接口
        /* 手机号码, 如有多个使用逗号分隔, 支持1~3000个号码
        /* 内容, 500字以内 
        /************************************************************************/
        public static int sendOnce(string mobile, string content, uint uCgid = 0, uint uCsid = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("action=sendOnce&ac=");
            sb.Append(ACCOUNT);
            sb.Append("&authkey=");
            sb.Append(AUTHKEY);
            sb.Append("&cgid=");
            sb.Append(uCgid > 0 ? uCgid : CGID);
            if (uCsid > 0)
            {
                sb.Append("&csid=");
                sb.Append(uCsid);
            }
            sb.Append("&m=");
            sb.Append(mobile);
            sb.Append("&c=");
            sb.Append(UrlEncode(content, Encoding.UTF8));

            string sResult = sendQuery(POST_URL, sb.ToString());
            return parseResult(sResult);
        }
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (SystemDbContext con = new SystemDbContext())
            {
                ApplicationUser user = UserManager.FindByName(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "用户名或密码错误！请重新登录");
                    return View(model);
                }
                if (!user.IsProved)
                {
                    ModelState.AddModelError("", "您所属的企业尚未通过审核。请与系统管理员联系。");
                    return View(model);
                }
                if (!user.IsAvailable)
                {
                    ModelState.AddModelError("", "抱歉，您已经被企业管理员禁用！请与企业管理员联系");
                    return View(model);
                }
            }

            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);     
            switch (result)
            {
                case SignInStatus.Success:
                    var user = UserManager.FindByName(model.UserName);
                    //设置session
                    Session["ConnectionString"] = user.ConnectionString;
                    Session["CompanyId"] = user.CompanyId;
                    Session["UserName"] = user.UserName;
                    Session["CompanyFullName"] = user.CompanyFullName;
                    Session["IsProved"] = user.IsProved;

                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "无效的登录尝试。");
                    return View(model);
            }
        }

   

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // 要求用户已通过使用用户名/密码或外部登录名登录
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 以下代码可以防范双重身份验证代码遭到暴力破解攻击。
            // 如果用户输入错误代码的次数达到指定的次数，则会将
            // 该用户帐户锁定指定的时间。
            // 可以在 IdentityConfig 中配置帐户锁定设置
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "代码无效。");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //首先核对用户的短信验证码是否合法
                using (var vCode = new SystemDbContext())
                {
                    var CurrentUserCode = vCode.VerifyCodes.Find(model.PhoneNumber);
                    DateTime CurTime = System.DateTime.Now;
                    if (CurTime > CurrentUserCode.OverTime)     //用户短信验证码超时
                    {
                        ModelState.AddModelError("", "抱歉，您的验证码已经过期！");
                        return View(model);
                    }
                    else if (!CurrentUserCode.Code.Equals(model.Code))
                    {
                        ModelState.AddModelError("", "抱歉，您的验证码输入错误！");
                        return View(model);
                    }
                }

                //验证企业全称是否已经被注册
                using (var vUser = new SystemDbContext())
                {
                    var tmp = vUser.Users.Where(p => p.CompanyFullName.Equals(model.CompanyFullName)).ToList();
                    if (tmp.Count != 0)
                    {
                        ModelState.AddModelError("", "抱歉，该企业全称已经被注册！");
                        return View(model);

                    }
                }
                var user = new ApplicationUser
                {
                    CompanyFullName = model.CompanyFullName,
                    PhoneNumber = model.PhoneNumber,

                    UserName = model.PhoneNumber,
                    IsProved = false,           //是否审核的标志
                    IsAvailable = true,         //是否是可用的管理员
                    IsRoot = true               //注册企业号的人默认就是企业的超级管理员
                };
                //生成企业ID号
                user.CompanyId = Generate.GenerateCompanyId();
                string CompanyDbName = "Bonsaii" + user.CompanyId;
                user.ConnectionString = ConfigurationManager.AppSettings["UserDbConnectionString"] + CompanyDbName + ";" ;   //"Data Source = localhost,1433;Network Library = DBMSSOCN;Initial Catalog = " + CompanyDbName + ";User ID = test;Password = admin;";
                
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    //添加注册的企业信息到Companies数据表当中
                    using (SystemDbContext sys = new SystemDbContext())
                    {
                        Company company = new Company()
                        {
                            CompanyId = user.CompanyId,
                            FullName = user.CompanyFullName,
                            TelNumber = user.UserName,
                            UserName = user.UserName
                        };
                        sys.Companies.Add(company);
                        sys.SaveChanges();
                    }
                    /**
                     * 注册成功并不会为企业创建独有的数据库，只有系统平台的超级管理员通过相应用户的审核之后才会为用户创建数据库
                     * */
                    //        await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false); 
                    // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
                    // 发送包含此链接的电子邮件
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "确认你的帐户", "请通过单击 <a href=\"" + callbackUrl + "\">這裏</a>来确认你的帐户");
                    return RedirectToAction("Login", "Account");
                }
                AddErrors(result);
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }


       

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        /*提交成功的页面*/
        [AllowAnonymous]
        public ActionResult ForgotPasswordInfo()
        {
            return View();
        }
        /*提交失败的页面*/
        [AllowAnonymous]
        public ActionResult ForgotPasswordInfoError()
        {
            return View();
        }

        /*忘记密码申请：跳转到找回密码页面之前需要验证身份*/
        // GET: /Account/ForgotPasswordInfo
        [AllowAnonymous]
        public ActionResult ForgotPasswordApply()
        {
            UserPasswordInfo view = new UserPasswordInfo();
            return View(view);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPasswordApply(UserPasswordInfo info, HttpPostedFileBase image)
        {
            SystemDbContext db = new SystemDbContext();
            if (ModelState.IsValid)
            {
                //UserPasswordInfo info = new UserPasswordInfo();
                db.UserPasswordInfos.Add(info);
                if (image != null)
                {
                    info.BusinessLicenseType = image.ContentType;//获取图片类型
                    //view.BusinessLicence = new byte[image.ContentLength];//新建一个长度等于图片大小的二进制地址
                    // image.InputStream.Read(view.BusinessLicence, 0, image.ContentLength);//将image读取到Logo中
                    info.BusinessLicense = new byte[image.ContentLength];
                    image.InputStream.Read(info.BusinessLicense, 0, image.ContentLength);
                }

                DateTime time = DateTime.Now;

                /*利用Expression表达式树可以解决:LINQ to Entities 不识别方法“System.DateTime AddMinutes(Double)*/
                Expression<Func<UserPasswordInfo, bool>> where = p => p.UserName == info.UserName && (p.SubmitTime.AddMinutes(2) >= time);
                // var item = from p in db.UserPasswordInfos where (p.UserName == info.UserName && (p.SubmitTime.AddMinutes(2)<=time)) select p;
                var item = db.UserPasswordInfos.Where(where.Compile()).ToList();
                if (item.Count() == 0)
                {
                    /*审核状态字段*/
                   // info.AuditStatus = "ture";
                    info.SubmitTime = time;
                    db.SaveChanges();
                   // return RedirectToAction("ForgotPasswordInfo");
                    return RedirectToAction("ForgotPassword");
                }
                else
                    return RedirectToAction("ForgotPasswordInfoError");
            }
            return View(info);
        }
        /*获取图片*/
        public FileContentResult GetImage()
        {
            //  SystemDbContext db = new SystemDbContext();
            UserPasswordInfo info = new UserPasswordInfo();
            if (info != null)
            {
                return File(info.BusinessLicense, info.BusinessLicenseType);//File方法直接将二进制转化为指定类型了。
            }
            else
            {
                return null;
            }
        }



        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //首先核对用户的短信验证码是否合法
                using (SystemDbContext vCode = new SystemDbContext())
                {
                    var CurrentUserCode = vCode.VerifyCodes.Find(model.PhoneNumber);
                    DateTime CurTime = System.DateTime.Now;
                    if (CurTime > CurrentUserCode.OverTime)     //用户短信验证码超时
                    {
                        ModelState.AddModelError("", "抱歉，您的验证码已经过期！");
                        return View(model);
                    }
                    else if (!CurrentUserCode.Code.Equals(model.Code))
                    {
                        ModelState.AddModelError("", "抱歉，您的验证码输入错误！");
                        return View(model);
                    }
                }


                ApplicationUser user = UserManager.FindByName(model.PhoneNumber);
                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var result = UserManager.ResetPassword(user.Id, code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }


                // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
                // 发送包含此链接的电子邮件
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "重置密码", "请通过单击 <a href=\"" + callbackUrl + "\">此处</a>来重置你的密码");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }


        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // 请不要显示该用户不存在
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }



        public ActionResult ModifyPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ModifyPassword(ModifyPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (SystemDbContext con = new SystemDbContext())
            {
                ApplicationUser user = UserManager.FindByName(model.UserName);
                var result = UserManager.ChangePassword(user.Id, model.Password, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("ModifyPasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError("", "用户名或密码错误！");
                    return View(model);
                }
            }
        }

        public ActionResult ModifyPasswordConfirmation()
        {
            return View();
        }
        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 请求重定向到外部登录提供程序
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // 生成令牌并发送该令牌
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // 如果用户已具有登录名，则使用此外部登录提供程序将该用户登录
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // 如果用户没有帐户，则提示该用户创建帐户
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // 从外部登录提供程序获取有关用户的信息
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        [HttpPost]
        public JsonResult GenerateVerifyCode()
        {
          
            var PhoneNumber = Request["PhoneNumber"];
            string code = (new Random().Next(1111, 9999)).ToString();

            string content = "盆景办公提醒您，您的验证码为：" + code;  
            //向用户发送验证码
       //    int rect  = sendOnce(PhoneNumber, content);
            int rect = 1;
            //短信验证发送失败！
           if (rect <= 0)
           {
               return Json(new
               {
                   errorcode = rect
               });
           }

           using (var vCode = new SystemDbContext())
           {
               var tmp = vCode.VerifyCodes.Find(PhoneNumber);
               DateTime createTime = System.DateTime.Now;
               //电话在数据库中已经存在，更新Code和CreateTime以及OverTime就可以
               if (tmp != null)
               {
                   tmp.Code = code;
                   tmp.CreateTime = createTime;
                   tmp.OverTime = createTime.AddSeconds(VerifyCodeOverTimeSeconds);
                   vCode.Entry(tmp).State = System.Data.Entity.EntityState.Modified;
                   vCode.SaveChanges();
               }
               else
               {
                   VerifyCode tmpCode = new VerifyCode();
                   tmpCode.Code = code;
                   tmpCode.PhoneNumber = PhoneNumber;
                   tmpCode.CreateTime = createTime;
                   tmpCode.OverTime = createTime.AddSeconds(VerifyCodeOverTimeSeconds);
                   vCode.VerifyCodes.Add(tmpCode);
                   vCode.SaveChanges();
               }
           }
            return Json(new {
                 errorcode="1"});
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }



        #region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}