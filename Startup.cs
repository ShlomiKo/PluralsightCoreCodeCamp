﻿using System;
using AutoMapper;
using CoreCodeCamp.Controllers;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodeCamp
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<CampContext>();
      services.AddScoped<ICampRepository, CampRepository>();

      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

      services.AddApiVersioning(opt =>
      {
          opt.AssumeDefaultVersionWhenUnspecified = true;
          opt.DefaultApiVersion = new ApiVersion(1,1);
          opt.ReportApiVersions = true;
          //support for changing the default api-version query string to api version
          //opt.ApiVersionReader = new QueryStringApiVersionReader("ver");
          opt.ApiVersionReader = ApiVersionReader.Combine(
              new HeaderApiVersionReader("X-Version"),
              new QueryStringApiVersionReader("ver", "version"));

          //same as using the attributes in the controller itself to centralize it
          opt.Conventions.Controller<TalksController>()
              .HasApiVersion(new ApiVersion(1, 0))
              .HasApiVersion(new ApiVersion(1, 1))
              .Action(c => c.Delete(default(string), default(int)))
                .MapToApiVersion(1, 1);
      });

      services.AddMvc()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      
      app.UseMvc();
    }
  }
}
