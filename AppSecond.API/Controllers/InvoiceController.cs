﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppSecond.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetInvoices()
        {
            var userName = HttpContext.User.Identity.Name; //Pull userId data from accessToken in payload 

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier); //Pull userName data from accessToken in payload - claims (TokenService class Configures claimsTypes)

            //Pull the necessary datas on Database by using relation with userId and userName
            //StockId - StockQuantity - Category  UserId - UserName

            return Ok($"Stock Operations => UserName : {userName} - UserId : {userIdClaim.Value} ");
        }




    }
}
