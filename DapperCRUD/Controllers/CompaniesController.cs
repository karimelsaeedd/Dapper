﻿using DapperCRUD.Contracts;
using DapperCRUD.Dto;
using DapperCRUD.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;

        public CompaniesController(ICompanyRepository companyRepo) => _companyRepo = companyRepo;
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _companyRepo.GetCompanies();

            return Ok(companies);
        }
        [HttpGet("{id}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _companyRepo.GetCompany(id);
            if (company is null)
                return NotFound();

            return Ok(company);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody]CompanyForCreationDto company)
        {
            var createdCompany = await _companyRepo.CreateCompany(company);

            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody]CompanyForUpdateDto company)
        {
            var dbCompany = await _companyRepo.GetCompany(id);
            if (dbCompany is null)
                return NotFound();

            await _companyRepo.UpdateCompany(id, company);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var dbCompany = await _companyRepo.GetCompany(id);
            if (dbCompany is null)
                return NotFound();

            await _companyRepo.DeleteCompany(id);

            return NoContent();
        }
    }
}
