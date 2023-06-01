using Dapper;
using DapperCRUD.Content;
using DapperCRUD.Contracts;
using DapperCRUD.Dto;
using DapperCRUD.Entities;
using System.Data;

namespace DapperCRUD.Repository
{
    public class CompanyRepository: ICompanyRepository
    {
        private readonly DapperContext _context;

        public CompanyRepository(DapperContext context) => _context = context;

        public async Task<Company> CreateCompany(CompanyForCreationDto company)
        {
            var query = "INSERT INTO Companies (Name, Address, Country) Values (@Name, @Address, @Country)" +
                "SELECT CAST(SCOPE_IDENTITY() AS int)";
            var parameter = new DynamicParameters();
            parameter.Add("Name", company.Name, DbType.String);
            parameter.Add("Address", company.Address, DbType.String);
            parameter.Add("Country", company.Country, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameter);
                
                // =================================
                var createdCompany = new Company
                {
                    Id = id,
                    Name = company.Name,
                    Address = company.Address,
                    Country = company.Country
                };

                return createdCompany;
            }
        }

        public Task CreateMultipleCompanies(List<CompanyForCreationDto> companies)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteCompany(int id)
        {
            var query = "DELETE FROM Companies WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            var query = "SELECT * FROM Companies";

            using (var connection = _context.CreateConnection())
            {
                var companies = await connection.QueryAsync<Company>(query);

                return companies.ToList();
            }
        }

        public Task<List<Company>> GetCompaniesEmployeesMultipleMapping()
        {
            throw new NotImplementedException();
        }

        public async Task<Company> GetCompany(int id)
        {
            var query = "SELECT * FROM Companies WHERE ID = @Id";

            using (var connection = _context.CreateConnection())
            {
                var company = await connection.QuerySingleOrDefaultAsync<Company>(query, new { id});

                return company;
            }
        }

        public Task<Company> GetCompanyByEmployeeId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Company> GetCompanyEmployeesMultipleResults(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCompany(int id, CompanyForUpdateDto company)
        {
            var query = "UPDATE Companies SET Name = @Name, Address = @Address, Country = @Country WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
