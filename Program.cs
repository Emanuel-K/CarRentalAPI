using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using CarRentalAPI;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));