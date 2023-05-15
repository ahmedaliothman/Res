using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ResidencyApplication.Services.Models.EntityModels;
using ResidencyApplication.Services.Models.jwt;
namespace ResidencyApplication.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationSettingsController : Controller
    {
        private readonly ResidencyApplicationContext _context;
        private readonly IMapper _mapper;
        public ResponseRequest ResponseRequest_;

        public NotificationSettingsController(ResidencyApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            ResponseRequest_ = new ResponseRequest();
        }


        [HttpGet]
        public async Task<ActionResult<ResponseRequest<NotificationSetting>>> GetNotificationSettings()
        {
            var res = await _context.NotificationSettings.FirstOrDefaultAsync();
            var resDTO = res;

            var rtnObj = new ResponseRequest
            {
                HasError = false,
                Message = "Successfull",
                Response = resDTO,
                Status = (int)HttpStatusCode.OK
            };
            return Ok(rtnObj);

        }

        [HttpPut("updateIsActive/{ColumnName}")]
        public async Task<ActionResult<ResponseRequest<ApplicationTypesDTO>>> updateIsActive(string ColumnName, [FromBody] bool isActive)
        {
            var NotificationSetting = await _context.NotificationSettings.FirstOrDefaultAsync();
            var rtnObj = new ResponseRequest
            {
                Status = (int)HttpStatusCode.NotFound,
                HasError = true,
                Response = null,
                Message = "No Such Application Type"
            };
            if (NotificationSetting == null)
            {
                return NotFound(rtnObj);
            }
            else
            {
                switch (ColumnName.Trim().ToLower())
                {
                    case "acceptemailnotification":
                        NotificationSetting.AcceptEmailNotification = isActive;
                        break;
                    case "rejectemailnotification":
                        NotificationSetting.RejectEmailNotification = isActive;
                        break;
                    case "returnemailnotification":
                        NotificationSetting.ReturnEmailNotification = isActive;
                        break;
                    case "registemailnotification":
                        NotificationSetting.RegistEmailNotification = isActive;
                        break;
                    case "paymentemailnotification":
                        NotificationSetting.PaymentEmailNotification = isActive;
                        break;
                    case "acceptsmsnotification":
                        NotificationSetting.AcceptSmsNotification = isActive;
                        break;
                    case "rejectsmsnotification":
                        NotificationSetting.RejectSmsNotification = isActive;
                        break;
                    case "returnsmsnotification":
                        NotificationSetting.ReturnSmsNotification = isActive;
                        break;
                    case "registsmsnotification":
                        NotificationSetting.RegistSmsNotification = isActive;
                        break;
                    case "paymentsmsnotification":
                        NotificationSetting.PaymentSmsNotification = isActive;
                        break;

                }

                NotificationSetting.UpdatedDate = DateTime.Now;
                _context.Entry(NotificationSetting).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                        return NotFound(rtnObj);
                   
                }
            }
            var resDTO = NotificationSetting;
            rtnObj = new ResponseRequest
            {
                Status = (int)HttpStatusCode.OK,
                HasError = false,
                Response = resDTO,
                Message = "تم الحفظ بنجاح"
            };
            return Ok(rtnObj);
        }


    }
}
